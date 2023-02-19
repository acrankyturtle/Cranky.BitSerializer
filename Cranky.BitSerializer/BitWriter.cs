using System.Numerics;

namespace Cranky.BitSerializer;

[PublicAPI]
public static partial class BitWriter
{
	public static void WriteBinary<T>(ref BitSpan span, T value, int numBits)
		where T : IBinaryInteger<T>
	{
		span.Split(numBits, out var valueBits, out var outBits);
		valueBits.Write(value);

		span = outBits;
	}
}
