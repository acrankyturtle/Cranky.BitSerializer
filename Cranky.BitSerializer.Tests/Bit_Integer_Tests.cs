using System.Numerics;

namespace Cranky.BitSerializer.Tests;

public class Bit_Integer_Tests
{
	public record Parameters(long Value);

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_Int8(Parameters parameters)
	{
		var value = (sbyte)parameters.Value;
		TestInternal(value, w => w.WriteInt8(value), bits => BitReader.ReadInt8(ref bits));
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_UInt8(Parameters parameters)
	{
		var value = (byte)parameters.Value;
		TestInternal(value, w => w.WriteUInt8(value), bits => BitReader.ReadUInt8(ref bits));
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_Int16(Parameters parameters)
	{
		var value = (short)parameters.Value;
		TestInternal(value, w => w.WriteInt16(value), bits => BitReader.ReadInt16(ref bits));
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_UInt16(Parameters parameters)
	{
		var value = (ushort)parameters.Value;
		TestInternal(value, w => w.WriteUInt16(value), bits => BitReader.ReadUInt16(ref bits));
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_Int32(Parameters parameters)
	{
		var value = (int)parameters.Value;
		TestInternal(value, w => w.WriteInt32(value), bits => BitReader.ReadInt32(ref bits));
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_UInt32(Parameters parameters)
	{
		var value = (uint)parameters.Value;
		TestInternal(value, w => w.WriteUInt32(value), bits => BitReader.ReadUInt32(ref bits));
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_Int64(Parameters parameters)
	{
		var value = parameters.Value;
		TestInternal(value, w => w.WriteInt64(value), bits => BitReader.ReadInt64(ref bits));
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public static void Test_UInt64(Parameters parameters)
	{
		var value = (ulong)parameters.Value;
		TestInternal(value, w => w.WriteUInt64(value), bits => BitReader.ReadUInt64(ref bits));
	}

	private delegate T ReadFunc<T>(BitSpan bits);
	private delegate void WriteFunc(BitWriter writer);

	private static void TestInternal<T>(T expectedValue, WriteFunc writeFunc, ReadFunc<T> readFunc)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
	{
		var data = Utilities.GetRandomBytes(8);
		var bitSpan = new BitSpan(data, 0, data.Length * 8);
		var bitWriter = new BitWriter(bitSpan);

		writeFunc(bitWriter);
		var actualValue = readFunc(bitSpan);

		actualValue.Should().Be(expectedValue);
	}

	public static IEnumerable<object[]> GetTestParameters()
	{
		var values = GetValues(100);

		return GetParameters(values, 64).Select(x => new object[] { x });

		static IEnumerable<long> GetValues(int numRandomValues)
		{
			yield return 0;
			yield return 1;
			yield return -1;
			yield return long.MinValue;
			yield return long.MaxValue;

			const int seed = 347;
			var random = new Random(seed);

			for (int i = 0; i < numRandomValues; i++)
				yield return BitConverter.ToInt64(Utilities.GetRandomBytes(8));
		}

		static IEnumerable<Parameters> GetParameters(IEnumerable<long> values, int maxNumBits)
		{
			foreach (var value in values)
				yield return new Parameters(value);
		}
	}
}
