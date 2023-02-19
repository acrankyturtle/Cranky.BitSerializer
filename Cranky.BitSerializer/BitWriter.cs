using System.Numerics;

namespace Cranky.BitSerializer;

public readonly ref struct BitWriter
{
	private readonly BitSpan _span;

	public BitWriter(BitSpan span)
	{
		_span = span;
	}

	public BitWriter Write<T>(T value, int numBits, Func<T, byte> castAsByteFunc)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		_span.Split(numBits, out var valueBits, out var outBits);
		valueBits.Write(value, castAsByteFunc);

		return new(outBits);
	}
}
