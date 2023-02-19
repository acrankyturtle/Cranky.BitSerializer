using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cranky.BitSerializer;

public readonly ref struct BitSpan
{
	private readonly Span<byte> _data;
	private readonly int _offset;
	public readonly int Length;

	public BitSpan(Span<byte> data, int offset, int length)
	{
		if (offset < 0 || offset >= 8)
			throw new ArgumentOutOfRangeException(nameof(offset));

		if ((length + offset) >> 3 > data.Length)
			throw new ArgumentOutOfRangeException(nameof(length));

		_data = data;
		_offset = offset;
		Length = length;
	}

	public BitSpan this[Range range] => Slice(range);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BitSpan Slice(Range range) => Slice(range.Start.Value, range.End.Value - range.Start.Value);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BitSpan Slice(int skipBits)
	{
		int newLength = Length - skipBits;
		return Slice(skipBits, newLength);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BitSpan Slice(int skipBits, int length)
	{
		if (_data.Length * 8 < _offset + skipBits + length)
			throw new OverflowException();

		int startBit = _offset + skipBits;
		int newByteIndex = startBit / 8;
		int newOffset = startBit % 8;

		return new(_data[newByteIndex..], newOffset, length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Split(int offset, out BitSpan left, out BitSpan right)
	{
		if (offset < 0 || offset > Length)
			throw new ArgumentOutOfRangeException(nameof(offset));

		int startBit = _offset + offset;
		int byteSliceIndex = startBit / 8;
		int rightBitOffset = startBit % 8;
		int leftByteLength = rightBitOffset != 0 ? byteSliceIndex + 1 : byteSliceIndex;

		left = new(_data[..leftByteLength], _offset, offset);
		right = new(_data[byteSliceIndex..], rightBitOffset, Length - offset);
	}

	public T ReadAs<T>(Func<byte, T> castAsTFunc)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		var mapping = BitIndexMapping.From(_offset, Length, _data.Length);

		if (mapping.NumBytes == 1)
		{
			byte firstByte = (byte)(
				(_data[0] & mapping.FirstByte.Mask & mapping.LastByte.Mask) >> mapping.LastByte.Padding
			);
			return castAsTFunc(firstByte);
		}

		T value;

		// first byte
		{
			var b = (byte)(_data[0] & mapping.FirstByte.Mask);
			value = castAsTFunc(b) << (Length - mapping.FirstByte.NumBits);
		}

		int bitsRemaining = Length - mapping.FirstByte.NumBits;

		// whole bytes
		for (int i = 1; i < mapping.LastByteIndex; i++)
			value |= castAsTFunc(_data[i]) << (bitsRemaining -= 8);

		// last byte
		{
			var b = (byte)(
				((_data[mapping.LastByteIndex] & mapping.LastByte.Mask) >> mapping.LastByte.Padding)
			);
			value |= castAsTFunc(b);
		}

		return value;
	}

	public void Write<T>(T value, Func<T, byte> castAsByteFunc)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		var mapping = BitIndexMapping.From(_offset, Length, _data.Length);

		if (mapping.NumBytes == 1)
		{
			var valueByte = (byte)(
				(
					(castAsByteFunc(value) << mapping.LastByte.Padding)
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
				| (castAsByteFunc(value >> (Length - mapping.FirstByte.NumBits)) & mapping.FirstByte.Mask)
			);
		}

		int bitsRemaining = Length - mapping.FirstByte.NumBits;

		// whole bytes
		for (int i = 1; i < mapping.LastByteIndex; i++)
			_data[i] = castAsByteFunc(value >> (bitsRemaining -= 8));

		// last byte
		{
			_data[mapping.LastByteIndex] = (byte)(
				(_data[mapping.LastByteIndex] & ~mapping.LastByte.Mask)
				| (castAsByteFunc(value << mapping.LastByte.Padding) & mapping.LastByte.Mask)
			);
		}
	}
}

public static class Extensions_BitSpan
{
	public static T ReadAs<T>(this BitSpan span, int numBits, Func<byte, T> castAsTFunc)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T> => span[..numBits].ReadAs(castAsTFunc);

	public static void Write<T>(this BitSpan span, T value, int numBits, Func<T, byte> castAsByteFunc)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T> =>
		span[..numBits].Write(value, castAsByteFunc);
}

file readonly record struct BitIndexMapping(
	BitIndexByteMapping FirstByte,
	BitIndexByteMapping LastByte,
	int NumBytes
)
{
	public int LastByteIndex { get; } = NumBytes - 1;

	public static BitIndexMapping From(int offset, int length, int bufferLengthBytes)
	{
		int startPadding = offset;
		int endPadding = (bufferLengthBytes * 8 - length - offset) % 8;

		int startMask = (~(byte.MaxValue << (8 - startPadding))) & byte.MaxValue;
		int endMask = (byte.MaxValue << endPadding) & byte.MaxValue;

		int bitCount = length + offset;
		int numBytes = bitCount % 8 > 0 ? (bitCount / 8) + 1 : bitCount / 8;

		return new(new(startPadding, startMask), new(endPadding, endMask), numBytes);
	}
}

file readonly record struct BitIndexByteMapping(int Padding, int Mask)
{
	public int NumBits { get; } = 8 - Padding;
}
