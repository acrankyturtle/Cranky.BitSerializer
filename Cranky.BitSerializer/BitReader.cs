using System.Numerics;

namespace Cranky.BitSerializer;

public static partial class BitReader
{
	public static T Read<T>(ref BitSpan bitSpan, int numBits, Func<byte, T> castAsTFunc)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		bitSpan.Split(numBits, out var valueBits, out var outBits);
		var value = valueBits.ReadAs(castAsTFunc);
		bitSpan = outBits;

		return value;
	}
}
