using System.Numerics;
using System.Reflection;

namespace Cranky.BitSerializer.Tests;

public class BitSpan_ReadWrite_Tests
{
	public abstract record Parameters
	{
		public abstract Type Type { get; }
	}

	public record Parameters<T>(T Value, int NumBits, int BitOffset, int NumSpanPaddingBytes) : Parameters
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T>, IEqualityOperators<T, T, bool>
	{
		public override Type Type => typeof(T);
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public void Test(Parameters parameters)
	{
		var methodBase = GetType()
			.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
			.Single(m => m is { Name: nameof(TestInternal), IsGenericMethodDefinition: true });
		var method = methodBase.MakeGenericMethod(parameters.Type);

		method.Invoke(this, [parameters]);
	}

	private static void TestInternal<T>(Parameters<T> parameters)
		where T : IBinaryInteger<T>
	{
		var bitCountInLastByte = (parameters.BitOffset + parameters.NumBits) % 8;

		if (bitCountInLastByte == 0)
			bitCountInLastByte = 8;

		var numBytes =
			(parameters.NumBits + parameters.BitOffset) / 8
			+ (parameters.NumSpanPaddingBytes + bitCountInLastByte > 0 ? 1 : 0);

		if (numBytes < 1)
			throw new ArgumentException("Number of bytes must be greater than zero.", nameof(parameters));

		var data = Utilities.GetRandomBytes(numBytes);
		var bitSpan = new BitSpan(data, parameters.BitOffset, parameters.NumBits);

		var original = data.ToArray();

		bitSpan.Write(parameters.Value, parameters.NumBits);

		var readValue = bitSpan.ReadAs<T>();

		var valueMask = ~(T.AllBitsSet << parameters.NumBits);
		(readValue & valueMask)
			.Should()
			.Be(parameters.Value & valueMask, "value written should be the same as the value read");

		var firstByteMask = byte.MaxValue >> parameters.BitOffset;
		var lastByteMask = (byte)(byte.MaxValue << (8 - bitCountInLastByte));

		(data[0] & ~firstByteMask)
			.Should()
			.Be(original[0] & ~firstByteMask, "neighboring bits should not change");
		(data[^1] & ~lastByteMask)
			.Should()
			.Be(original[^1] & ~lastByteMask, "neighboring bits should not change");
	}

	public static IEnumerable<object[]> GetTestParameters()
	{
		var parameters = new IEnumerable<Parameters>[]
		{
			GetTestParameters<byte>(),
			GetTestParameters<short>(),
			GetTestParameters<ushort>(),
			GetTestParameters<int>(),
			GetTestParameters<uint>(),
			GetTestParameters<long>(),
			GetTestParameters<ulong>(),
		}
			.SelectMany(x => x)
			.Select(p => new object[] { p });

		foreach (var p in parameters)
			yield return p;
	}

	private static IEnumerable<Parameters<T>> GetTestParameters<T>()
		where T : struct, IBinaryNumber<T>, IShiftOperators<T, int, T>, IEqualityOperators<T, T, bool>
	{
		var size = TypeSize<T>.Size;

		var values = new[]
		{
			Enumerable.Range(0, size * 8 / 2).Aggregate(T.One, (v, _) => (v << 2) | T.One),
			~Enumerable.Range(0, size * 8 / 2).Aggregate(T.One, (v, _) => (v << 2) | T.One),
			FromLong(-6501144461293310406L),
			FromLong(~-6501144461293310406L),
		};

		for (var bitOffset = 0; bitOffset < 8; bitOffset++)
		{
			for (var paddingBytes = 0; paddingBytes < 3; paddingBytes++)
			{
				foreach (var value in values)
				{
					for (var numBits = 1; numBits <= size; numBits++)
					{
						yield return new(value, numBits, bitOffset, paddingBytes);
					}
				}
			}
		}

		static T FromLong(long v) // :(
		{
			var value = T.Zero;

			for (var i = 0; i < sizeof(long) * 8; i++)
			{
				if (((v >> i) & 1) == 1)
					value |= T.One << i;
			}

			return value;
		}
	}
}
