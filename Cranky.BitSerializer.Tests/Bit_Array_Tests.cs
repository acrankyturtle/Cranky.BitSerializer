using System.Numerics;

namespace Cranky.BitSerializer.Tests;

public class Bit_Array_Tests
{
	[Theory]
	[MemberData(nameof(GetParameters_Byte))]
	public static void Test_Byte(byte[] items, int offset)
	{
		TestInternal(items, sizeof(byte) * 8, offset);
	}

	[Theory]
	[MemberData(nameof(GetParameters_Long))]
	public static void Test_Long(long[] items, int offset)
	{
		TestInternal(items, sizeof(long) * 8, offset);
	}

	private static void TestInternal<T>(T[] items, int numBitsPerItem, int offset)
		where T : IBinaryInteger<T>
	{
		var data = Utilities.GetRandomBytes(items.Length * numBitsPerItem / 8 + 2);
		var writeSpan = new BitSpan(data, offset, items.Length * numBitsPerItem);
		var readSpan = new BitSpan(data, offset, items.Length * numBitsPerItem);
		
		BitWriter.WriteArray<T>(ref writeSpan, items, numBitsPerItem);
		var actualArray = BitReader.ReadArray<T>(ref readSpan, items.Length, numBitsPerItem);

		foreach (var (actualValue, expectedValue) in actualArray.Zip(items))
			actualValue.Should().Be(expectedValue);
	}

	public static IEnumerable<object[]> GetParameters_Byte()
	{
		const int maxElementCount = 4;

		for (var elements = 1; elements < maxElementCount; elements++)
			for (var offset = 0; offset < 8; offset++)
				yield return new object[] { Utilities.GetRandomBytes(elements).ToArray(), offset };
	}

	public static IEnumerable<object[]> GetParameters_Long()
	{
		const int maxElementCount = 4;

		for (var elements = 1; elements < maxElementCount; elements++)
			for (var offset = 0; offset < 8; offset++)
				yield return new object[]
				{
					Enumerable
						.Range(0, elements)
						.Select(_ => BitConverter.ToInt64(Utilities.GetRandomBytes(sizeof(long))))
						.ToArray(),
					offset,
				};
	}
}
