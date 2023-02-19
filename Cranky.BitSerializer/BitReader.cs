using System.Numerics;

namespace Cranky.BitSerializer;

[PublicAPI]
public static partial class BitReader
{
	public static T ReadBinary<T>(ref BitSpan bitSpan, int numBits)
		where T : IBinaryInteger<T>
	{
		bitSpan.Split(numBits, out var valueBits, out var outBits);
		var value = valueBits.ReadAs<T>();
		bitSpan = outBits;

		return value;
	}

	[PublicAPI]
	public static T ReadBinary<T>(ref ReadOnlyBitSpan bitSpan, int numBits)
		where T : IBinaryInteger<T>
	{
		bitSpan.Split(numBits, out var valueBits, out var outBits);
		var value = valueBits.ReadAs<T>();
		bitSpan = outBits;

		return value;
	}
}
