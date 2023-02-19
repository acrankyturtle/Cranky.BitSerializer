using System.Numerics;

namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static void ReadArray<T>(
		ref BitSpan span,
		Span<T> dstArray,
		int numItems,
		int numBitsPerElement,
		Func<byte, T> castAsTFunc
	)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		for (int i = 0; i < numItems; i++)
			dstArray[i] = Read(ref span, numBitsPerElement, castAsTFunc);
	}

	public static T[] ReadArray<T>(
		ref BitSpan span,
		int numItems,
		int numBitsPerElement,
		Func<byte, T> castAsTFunc
	)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		var array = new T[numItems];
		ReadArray(ref span, array, numItems, numBitsPerElement, castAsTFunc);

		return array;
	}
}
