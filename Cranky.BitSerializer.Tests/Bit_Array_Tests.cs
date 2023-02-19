using System.Numerics;
using System.Xml.Linq;

namespace Cranky.BitSerializer.Tests;

public class Bit_Array_Tests
{
	[Theory]
	[MemberData(nameof(GetParameters_Byte))]
	public static void Test_Byte(byte[] items, int offset)
	{
		TestInternal(items, sizeof(byte) * 8, offset, x => x, x => x);
	}

	[Theory]
	[MemberData(nameof(GetParameters_Long))]
	public static void Test_Long(long[] items, int offset)
	{
		TestInternal(items, sizeof(long) * 8, offset, x => (byte)x, x => x);
	}

	private static void TestInternal<T>(
		T[] items,
		int numBitsPerItem,
		int offset,
		Func<T, byte> castAsByteFunc,
		Func<byte, T> castAsTFunc
	)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		var data = Utilities.GetRandomBytes(items.Length * numBitsPerItem / 8 + 2);
		var bitSpan = new BitSpan(data, offset, items.Length * numBitsPerItem);
		var writer = new BitWriter(bitSpan);

		writer.WriteArray(items, numBitsPerItem, castAsByteFunc);
		var actualArray = BitReader.ReadArray(ref bitSpan, items.Length, numBitsPerItem, castAsTFunc);

		foreach (var (actualValue, expectedValue) in actualArray.Zip(items))
			actualValue.Should().Be(expectedValue);
	}

	public static IEnumerable<object[]> GetParameters_Byte()
	{
		const int maxElementCount = 4;

		for (int elements = 1; elements < maxElementCount; elements++)
			for (int offset = 0; offset < 8; offset++)
				yield return new object[] { Utilities.GetRandomBytes(elements).ToArray(), offset };
	}

	public static IEnumerable<object[]> GetParameters_Long()
	{
		const int maxElementCount = 4;

		for (int elements = 1; elements < maxElementCount; elements++)
			for (int offset = 0; offset < 8; offset++)
				yield return new object[]
				{
					Enumerable
						.Range(0, elements)
						.Select(i => BitConverter.ToInt64(Utilities.GetRandomBytes(sizeof(long))))
						.ToArray(),
					offset
				};
	}
}
