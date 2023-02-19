using System.Numerics;

namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static TFloat ReadFloat01<TFloat, TBinary>(
		ref BitSpan span,
		int numBits,
		Func<byte, TBinary> byteToBinFunc,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryNumber<TBinary>,
			IShiftOperators<TBinary, int, TBinary>,
			IUnsignedNumber<TBinary>
	{
		var maxRawValue = binToFloatFunc(~(TBinary.AllBitsSet << numBits));
		var binVaue = Read(ref span, numBits, byteToBinFunc);
		var rawValue = binToFloatFunc(binVaue);
		return rawValue / maxRawValue;
	}

	public static TFloat ReadFloatRanged<TFloat, TBinary>(
		ref BitSpan span,
		TFloat minimum,
		TFloat maximum,
		int numBits,
		Func<byte, TBinary> byteToBinFunc,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryNumber<TBinary>,
			IShiftOperators<TBinary, int, TBinary>,
			IUnsignedNumber<TBinary>
	{
		var pct = ReadFloat01(ref span, numBits, byteToBinFunc, binToFloatFunc);
		var range = maximum - minimum;
		return pct * range + minimum;
	}

	public static float ReadSingle01(ref BitSpan span, int numBits) =>
		ReadFloat01<float, uint>(ref span, numBits, x => x, x => x);

	public static double ReadDouble01(ref BitSpan span, int numBits) =>
		ReadFloat01<double, ulong>(ref span, numBits, x => x, x => x);

	public static float ReadSingleRanged(ref BitSpan span, float minimum, float maximum, int numBits) =>
		ReadFloatRanged<float, uint>(ref span, minimum, maximum, numBits, x => x, x => x);

	public static double ReadDoubleRanged(ref BitSpan span, double minimum, double maximum, int numBits) =>
		ReadFloatRanged<double, ulong>(ref span, minimum, maximum, numBits, x => x, x => x);
}
