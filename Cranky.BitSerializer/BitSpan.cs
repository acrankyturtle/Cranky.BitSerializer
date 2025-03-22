using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cranky.BitSerializer;

[PublicAPI]
public readonly ref struct BitSpan
{
	private readonly Span<byte> _data;
	private readonly int _offset;

	public int Length { get; }

	public BitSpan(Span<byte> data, int offset, int length)
	{
		if (length + offset > data.Length * 8)
			throw new ArgumentOutOfRangeException(nameof(length));

		var byteOffset = offset / 8;
		var bitOffset = offset % 8;
		data = data[byteOffset..];

		if (bitOffset is < 0 or >= 8)
			throw new ArgumentOutOfRangeException(nameof(bitOffset));

		_data = data;
		_offset = bitOffset;
		Length = length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BitSpan Slice(int skipBits)
	{
		var newLength = Length - skipBits;
		return Slice(skipBits, newLength);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BitSpan Slice(int skipBits, int length)
	{
		if (_data.Length * 8 < _offset + skipBits + length)
			throw new OverflowException();

		var startBit = _offset + skipBits;
		var newByteIndex = startBit / 8;
		var newOffset = startBit % 8;

		return new(_data[newByteIndex..], newOffset, length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Split(int offset, out BitSpan left, out BitSpan right)
	{
		if (offset < 0 || offset > Length)
			throw new ArgumentOutOfRangeException(nameof(offset));

		var startBit = _offset + offset;
		var byteSliceIndex = startBit / 8;
		var rightBitOffset = startBit % 8;
		var leftByteLength = rightBitOffset != 0 ? byteSliceIndex + 1 : byteSliceIndex;

		left = new(_data[..leftByteLength], _offset, offset);
		right = new(_data[byteSliceIndex..], rightBitOffset, Length - offset);
	}

	public static implicit operator ReadOnlyBitSpan(BitSpan bitSpan) =>
		new(bitSpan._data, bitSpan._offset, bitSpan.Length);

	public T ReadAs<T>()
		where T : IBinaryInteger<T>
	{
		if (Length == 0)
			return T.Zero;

		var mapping = BitIndexMapping.From(_offset, Length, _data.Length);

		if (mapping.NumBytes == 1)
		{
			var firstByte = (byte)(
				(_data[0] & mapping.FirstByte.Mask & mapping.LastByte.Mask) >> mapping.LastByte.Padding
			);

			return CastHelpers.FromByte<T>(ref firstByte);
		}

		T value;

		// first byte
		{
			var b = (byte)(_data[0] & mapping.FirstByte.Mask);
			value = CastHelpers.FromByte<T>(ref b) << (Length - mapping.FirstByte.NumBits);
		}

		var bitsRemaining = Length - mapping.FirstByte.NumBits;

		// whole bytes
		for (var i = 1; i < mapping.LastByteIndex; i++)
		{
			var b = _data[i];
			var v = CastHelpers.FromByte<T>(ref b);
			value |= v << (bitsRemaining -= 8);
		}

		// last byte
		{
			var b = (byte)(
				(_data[mapping.LastByteIndex] & mapping.LastByte.Mask) >> mapping.LastByte.Padding
			);
			value |= CastHelpers.FromByte<T>(ref b);
		}

		return value;
	}

	public void Write<T>(T value)
		where T : IBinaryInteger<T>
	{
		if (Length == 0)
			return;

		var mapping = BitIndexMapping.From(_offset, Length, _data.Length);

		if (mapping.NumBytes == 1)
		{
			var valueByte = (byte)(
				(
					(CastHelpers.ToByte(value) << mapping.LastByte.Padding)
					& mapping.FirstByte.Mask
					& mapping.LastByte.Mask
				) | (_data[0] & ~(mapping.FirstByte.Mask & mapping.LastByte.Mask))
			);
			_data[0] = valueByte;

			return;
		}

		// first byte
		{
			_data[0] = (byte)(
				(_data[0] & ~mapping.FirstByte.Mask)
				| (CastHelpers.ToByte(value >> (Length - mapping.FirstByte.NumBits)) & mapping.FirstByte.Mask)
			);
		}

		var bitsRemaining = Length - mapping.FirstByte.NumBits;

		// whole bytes
		for (var i = 1; i < mapping.LastByteIndex; i++)
			_data[i] = CastHelpers.ToByte(value >> (bitsRemaining -= 8));

		// last byte
		{
			_data[mapping.LastByteIndex] = (byte)(
				(_data[mapping.LastByteIndex] & ~mapping.LastByte.Mask)
				| (CastHelpers.ToByte(value << mapping.LastByte.Padding) & mapping.LastByte.Mask)
			);
		}
	}

	public static BitSpan Empty => default;
}

internal readonly record struct BitIndexMapping(
	BitIndexByteMapping FirstByte,
	BitIndexByteMapping LastByte,
	int NumBytes
)
{
	public int LastByteIndex { get; } = NumBytes - 1;

	public static BitIndexMapping From(int offset, int length, int bufferLengthBytes)
	{
		var startPadding = offset;
		var endPadding = (bufferLengthBytes * 8 - length - offset) % 8;

		var startMask = ~(byte.MaxValue << (8 - startPadding)) & byte.MaxValue;
		var endMask = (byte.MaxValue << endPadding) & byte.MaxValue;

		var bitCount = length + offset;
		var numBytes = bitCount % 8 > 0 ? (bitCount / 8) + 1 : bitCount / 8;

		return new(new(startPadding, startMask), new(endPadding, endMask), numBytes);
	}
}

internal readonly record struct BitIndexByteMapping(int Padding, int Mask)
{
	public int NumBits { get; } = 8 - Padding;
}

[PublicAPI]
public static class Extensions_BitSpan
{
	public static T ReadAs<T>(this BitSpan span, int numBits)
		where T : IBinaryInteger<T> => span[..numBits].ReadAs<T>();

	public static void Write<T>(this BitSpan span, T value, int numBits)
		where T : IBinaryInteger<T> => span[..numBits].Write(value);
}
