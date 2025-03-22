using System.Numerics;

namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static TFloat ReadFloat01<TFloat, TBinary>(
		ref BitSpan span,
		int numBits,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryInteger<TBinary>, IUnsignedNumber<TBinary>
	{
		var maxBinValue = ~(TBinary.AllBitsSet << numBits);
		if (maxBinValue == TBinary.Zero)
			// overflow hack!
			maxBinValue = TBinary.AllBitsSet;

		var maxRawValue = binToFloatFunc(maxBinValue);
		var binValue = ReadBinary<TBinary>(ref span, numBits);
		var rawValue = binToFloatFunc(binValue);
		return rawValue / maxRawValue;
	}

	public static TFloat ReadFloatRanged<TFloat, TBinary>(
		ref BitSpan span,
		TFloat minimum,
		TFloat maximum,
		int numBits,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryInteger<TBinary>, IUnsignedNumber<TBinary>
	{
		var pct = ReadFloat01(ref span, numBits, binToFloatFunc);
		var range = maximum - minimum;
		return pct * range + minimum;
	}

	public static float ReadSingle01(ref BitSpan span, int numBits) =>
		ReadFloat01<float, uint>(ref span, numBits, x => x);

	public static double ReadDouble01(ref BitSpan span, int numBits) =>
		ReadFloat01<double, ulong>(ref span, numBits, x => x);

	public static float ReadSingleRanged(ref BitSpan span, float minimum, float maximum, int numBits) =>
		ReadFloatRanged<float, uint>(ref span, minimum, maximum, numBits, x => x);

	public static double ReadDoubleRanged(ref BitSpan span, double minimum, double maximum, int numBits) =>
		ReadFloatRanged<double, ulong>(ref span, minimum, maximum, numBits, x => x);

	#region Read Only
	public static TFloat ReadFloat01<TFloat, TBinary>(
		ref ReadOnlyBitSpan span,
		int numBits,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryInteger<TBinary>, IUnsignedNumber<TBinary>
	{
		var maxBinValue = ~(TBinary.AllBitsSet << numBits);
		if (maxBinValue == TBinary.Zero)
			// overflow hack!
			maxBinValue = TBinary.AllBitsSet;

		var maxRawValue = binToFloatFunc(maxBinValue);
		var binValue = ReadBinary<TBinary>(ref span, numBits);
		var rawValue = binToFloatFunc(binValue);
		return rawValue / maxRawValue;
	}

	public static TFloat ReadFloatRanged<TFloat, TBinary>(
		ref ReadOnlyBitSpan span,
		TFloat minimum,
		TFloat maximum,
		int numBits,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryInteger<TBinary>, IUnsignedNumber<TBinary>
	{
		var pct = ReadFloat01(ref span, numBits, binToFloatFunc);
		var range = maximum - minimum;
		return pct * range + minimum;
	}

	public static float ReadSingle01(ref ReadOnlyBitSpan span, int numBits) =>
		ReadFloat01<float, uint>(ref span, numBits, x => x);

	public static double ReadDouble01(ref ReadOnlyBitSpan span, int numBits) =>
		ReadFloat01<double, ulong>(ref span, numBits, x => x);

	public static float ReadSingleRanged(
		ref ReadOnlyBitSpan span,
		float minimum,
		float maximum,
		int numBits
	) => ReadFloatRanged<float, uint>(ref span, minimum, maximum, numBits, x => x);

	public static double ReadDoubleRanged(
		ref ReadOnlyBitSpan span,
		double minimum,
		double maximum,
		int numBits
	) => ReadFloatRanged<double, ulong>(ref span, minimum, maximum, numBits, x => x);
	#endregion
}
