using System.Numerics;

namespace Cranky.BitSerializer;

public static partial class BitWriter
{
	public static void WriteFloat01<TFloat, TBinary>(
		ref BitSpan span,
		TFloat value,
		int numBits,
		Func<TFloat, TBinary> floatToBinFunc,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryInteger<TBinary>, IUnsignedNumber<TBinary>
	{
		var maxBinValue = ~(TBinary.AllBitsSet << numBits);
		var maxRawValue = binToFloatFunc(maxBinValue);
		var rawValue = value * maxRawValue;
		var binValue = floatToBinFunc(rawValue);

		WriteBinary(ref span, TBinary.Clamp(binValue, TBinary.Zero, maxBinValue), numBits);
	}

	public static void WriteFloatRanged<TFloat, TBinary>(
		ref BitSpan span,
		TFloat value,
		TFloat minimum,
		TFloat maximum,
		int numBits,
		Func<TFloat, TBinary> floatToBinFunc,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryInteger<TBinary>, IUnsignedNumber<TBinary>
	{
		var range = maximum - minimum;

		var pct = (value - minimum) / range;
		WriteFloat01(ref span, pct, numBits, floatToBinFunc, binToFloatFunc);
	}

	public static void WriteSingle01(ref BitSpan span, float value, int numBits) =>
		WriteFloat01(ref span, value, numBits, x => (uint)Math.Round(x), x => x);

	public static void WriteDouble01(ref BitSpan span, double value, int numBits) =>
		WriteFloat01(ref span, value, numBits, x => (ulong)Math.Round(x), x => x);

	public static void WriteSingleRanged(
		ref BitSpan span,
		float value,
		float minimum,
		float maximum,
		int numBits
	) => WriteFloatRanged(ref span, value, minimum, maximum, numBits, x => (uint)Math.Round(x), x => x);

	public static void WriteDoubleRanged(
		ref BitSpan span,
		double value,
		double minimum,
		double maximum,
		int numBits
	) => WriteFloatRanged(ref span, value, minimum, maximum, numBits, x => (ulong)Math.Round(x), x => x);
}
