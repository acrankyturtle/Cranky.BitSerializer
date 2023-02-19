using System.Numerics;

namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static void ReadArray<T>(ref BitSpan span, Span<T> dstArray, int numItems, int numBitsPerElement)
		where T : IBinaryInteger<T>
	{
		for (var i = 0; i < numItems; i++)
			dstArray[i] = ReadBinary<T>(ref span, numBitsPerElement);
	}

	public static T[] ReadArray<T>(ref BitSpan span, int numItems, int numBitsPerElement)
		where T : IBinaryInteger<T>
	{
		var array = new T[numItems];
		ReadArray<T>(ref span, array, numItems, numBitsPerElement);

		return array;
	}

	public static void ReadArray<T>(
		ref ReadOnlyBitSpan span,
		Span<T> dstArray,
		int numItems,
		int numBitsPerElement
	)
		where T : IBinaryInteger<T>
	{
		for (var i = 0; i < numItems; i++)
			dstArray[i] = ReadBinary<T>(ref span, numBitsPerElement);
	}

	public static T[] ReadArray<T>(ref ReadOnlyBitSpan span, int numItems, int numBitsPerElement)
		where T : IBinaryInteger<T>
	{
		var array = new T[numItems];
		ReadArray<T>(ref span, array, numItems, numBitsPerElement);

		return array;
	}
}
