using System.Numerics;

namespace Cranky.BitSerializer;

public static partial class Extensions_BitWriter
{
	public static BitWriter WriteFloat01<TFloat, TBinary>(
		this BitWriter writer,
		TFloat value,
		int numBits,
		Func<TBinary, byte> binToByteFunc,
		Func<TFloat, TBinary> floatToBinFunc,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryNumber<TBinary>,
			IShiftOperators<TBinary, int, TBinary>,
			IUnsignedNumber<TBinary>
	{
		var maxBinValue = ~(TBinary.AllBitsSet << numBits);
		var maxRawValue = binToFloatFunc(maxBinValue);
		var rawValue = value * maxRawValue;
		var binValue = floatToBinFunc(rawValue);

		return writer.Write(TBinary.Clamp(binValue, TBinary.Zero, maxBinValue), numBits, binToByteFunc);
	}

	public static BitWriter WriteFloatRanged<TFloat, TBinary>(
		this BitWriter writer,
		TFloat value,
		TFloat minimum,
		TFloat maximum,
		int numBits,
		Func<TBinary, byte> binToByteFunc,
		Func<TFloat, TBinary> floatToBinFunc,
		Func<TBinary, TFloat> binToFloatFunc
	)
		where TFloat : IFloatingPoint<TFloat>, IBinaryNumber<TFloat>
		where TBinary : IBinaryNumber<TBinary>,
			IShiftOperators<TBinary, int, TBinary>,
			IUnsignedNumber<TBinary>
	{
		var range = maximum - minimum;

		var pct = (value - minimum) / range;
		return writer.WriteFloat01(pct, numBits, binToByteFunc, floatToBinFunc, binToFloatFunc);
	}

	public static BitWriter WriteSingle01(this BitWriter writer, float value, int numBits) =>
		writer.WriteFloat01(value, numBits, x => (byte)x, x => (uint)Math.Round(x), x => x);

	public static BitWriter WriteDouble01(this BitWriter writer, double value, int numBits) =>
		writer.WriteFloat01(value, numBits, x => (byte)x, x => (ulong)Math.Round(x), x => x);

	public static BitWriter WriteSingleRanged(
		this BitWriter writer,
		float value,
		float minimum,
		float maximum,
		int numBits
	) =>
		writer.WriteFloatRanged(
			value,
			minimum,
			maximum,
			numBits,
			x => (byte)x,
			x => (uint)Math.Round(x),
			x => x
		);

	public static BitWriter WriteDoubleRanged(
		this BitWriter writer,
		double value,
		double minimum,
		double maximum,
		int numBits
	) =>
		writer.WriteFloatRanged(
			value,
			minimum,
			maximum,
			numBits,
			x => (byte)x,
			x => (ulong)Math.Round(x),
			x => x
		);
}
