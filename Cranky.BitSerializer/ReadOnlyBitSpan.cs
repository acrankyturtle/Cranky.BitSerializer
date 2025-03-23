using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cranky.BitSerializer;

[PublicAPI]
public readonly ref struct ReadOnlyBitSpan
{
	private readonly ReadOnlySpan<byte> _data;
	private readonly int _offset;

	public int Length { get; }

	public ReadOnlyBitSpan(ReadOnlySpan<byte> data, int offset, int length)
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
	public ReadOnlyBitSpan Slice(int skipBits)
	{
		var newLength = Length - skipBits;
		return Slice(skipBits, newLength);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlyBitSpan Slice(int skipBits, int length)
	{
		if (_data.Length * 8 < _offset + skipBits + length)
			throw new OverflowException();

		var startBit = _offset + skipBits;
		var newByteIndex = startBit / 8;
		var newOffset = startBit % 8;

		return new(_data[newByteIndex..], newOffset, length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Split(int offset, out ReadOnlyBitSpan left, out ReadOnlyBitSpan right)
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
		var bitsRemaining = Length - mapping.FirstByte.NumBits;

		// first byte
		{
			var b = (byte)(_data[0] & mapping.FirstByte.Mask);
			value = CastHelpers.FromByte<T>(ref b) << bitsRemaining;
		}

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

	public static ReadOnlyBitSpan Empty => default;
}

[PublicAPI]
public static class Extensions_ReadOnlyBitSpan
{
	public static T ReadAs<T>(this ReadOnlyBitSpan span, int numBits)
		where T : IBinaryInteger<T> => span[..numBits].ReadAs<T>();
}
