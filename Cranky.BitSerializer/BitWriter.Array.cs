using System.Numerics;

namespace Cranky.BitSerializer;

public static partial class Extensions_BitWriter
{
	public static BitWriter WriteArray<T>(
		this BitWriter writer,
		ReadOnlySpan<T> array,
		int numBitsPerElement,
		Func<T, byte> castAsByteFunc
	)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		for (int i = 0; i < array.Length; i++)
			writer = writer.Write(array[i], numBitsPerElement, castAsByteFunc);

		return writer;
	}
}
