using System.Numerics;

namespace Cranky.BitSerializer;

public static partial class BitWriter
{
	public static void WriteArray<T>(ref BitSpan span, ReadOnlySpan<T> array, int numBitsPerElement)
		where T : IBinaryInteger<T>
	{
		// ReSharper disable once ForCanBeConvertedToForeach
		for (var i = 0; i < array.Length; i++)
			WriteBinary(ref span, array[i], numBitsPerElement);
	}
}
