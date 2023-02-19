using System.Numerics;

namespace Cranky.BitSerializer.Tests;

public class Bit_Float_Tests
{
	public abstract record Parameters(int NumBits, int NumBytes, double Value);

	public record Parameters_01(int NumBits, int NumBytes, double Value)
		: Parameters(NumBits, NumBytes, Value);

	public record Parameters_Ranged(int NumBits, int NumBytes, double Value, double Minimum, double Maximum)
		: Parameters(NumBits, NumBytes, Value);

	[Theory]
	[MemberData(nameof(GetTestParameters_Single_Ranged))]
	public static void Test_Single_Ranged(Parameters_Ranged parameters)
	{
		var value = (float)parameters.Value;
		var minimum = (float)parameters.Minimum;
		var maximum = (float)parameters.Maximum;

		TestInternal(
			value,
			float.Abs((float)(parameters.Maximum - parameters.Minimum) / (1 << parameters.NumBits)) + 0.00001,
			parameters.NumBytes,
			parameters.NumBits,
			w => w.WriteSingleRanged(value, minimum, maximum, parameters.NumBits),
			bits => BitReader.ReadSingleRanged(ref bits, minimum, maximum, parameters.NumBits)
		);
	}

	[Theory]
	[MemberData(nameof(GetTestParameters_Double_Ranged))]
	public static void Test_Double_Ranged(Parameters_Ranged parameters)
	{
		var value = (float)parameters.Value;
		var minimum = (float)parameters.Minimum;
		var maximum = (float)parameters.Maximum;

		TestInternal(
			value,
			double.Abs((parameters.Maximum - parameters.Minimum) / (1 << parameters.NumBits)) + 0.00001,
			parameters.NumBytes,
			parameters.NumBits,
			w => w.WriteDoubleRanged(value, minimum, maximum, parameters.NumBits),
			bits => BitReader.ReadDoubleRanged(ref bits, minimum, maximum, parameters.NumBits)
		);
	}

	private delegate T ReadFunc<T>(BitSpan bits);
	private delegate void WriteFunc(BitWriter writer);

	private static void TestInternal<T>(
		T expectedValue,
		T maxError,
		int numBytes,
		int numBits,
		WriteFunc writeFunc,
		ReadFunc<T> readFunc
	)
		where T : IFloatingPoint<T>
	{
		var data = Utilities.GetRandomBytes(numBytes);
		var bitSpan = new BitSpan(data, 0, numBits);
		var bitWriter = new BitWriter(bitSpan);

		writeFunc(bitWriter);
		var actualValue = readFunc(bitSpan[..numBits]);

		actualValue.Should().BeInRange(expectedValue - maxError, expectedValue + maxError);
	}

	public static IEnumerable<object[]> GetTestParameters_Single_Ranged() => GetTestParameters_Ranged(32);

	public static IEnumerable<object[]> GetTestParameters_Double_Ranged() => GetTestParameters_Ranged(64);

	private static IEnumerable<object[]> GetTestParameters_Ranged(int maxNumBits)
	{
		var values = GetValues(100);

		return GetParameters(values, maxNumBits).Select(x => new object[] { x });

		static IEnumerable<(double Min, double Max, double Value)> GetValues(int numRandomValues)
		{
			yield return (0, 1, 0);
			yield return (0, 1, 1);
			yield return (0, 1, 0.5);
			yield return (-1, 1, -1);
			yield return (-1, 1, 1);
			yield return (-1, 1, 0);
			yield return (0, 0, 0);
			yield return (1, 1, 1);
			yield return (-3, -2, -3);
			yield return (-3, -2, -2);
			yield return (-3, -2, -2.33);
			yield return (10, 500, 40);

			const int seed = 347;
			var random = new Random(seed);
			const double randomScale = 100;

			for (int i = 0; i < numRandomValues; i++)
			{
				var range = random.NextDouble() * randomScale;
				var offset = (random.NextDouble() * 2 - 1) * randomScale;
				var min = offset - range / 2;
				var max = offset + range / 2;
				var value = random.NextDouble() * range + min;

				yield return (min, max, value);
			}
		}

		static IEnumerable<Parameters_Ranged> GetParameters(
			IEnumerable<(double Min, double Max, double Value)> values,
			int maxNumBits
		)
		{
			for (int i = 1; i < maxNumBits; i++)
			{
				foreach (var (min, max, value) in values)
					yield return new Parameters_Ranged(i, maxNumBits / 8 + 1, value, min, max);
			}
		}
	}
}
