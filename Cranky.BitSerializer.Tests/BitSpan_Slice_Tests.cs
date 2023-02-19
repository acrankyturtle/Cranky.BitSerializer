namespace Cranky.BitSerializer.Tests;

public class BitSpan_Slice_Tests
{
	public record Parameters(uint Value, int ByteCount, int Offset, int Length, int SliceOffset)
	{
		public int ExpectedNumBytesRemaining => ByteCount - ((Offset + SliceOffset) / 8);
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public void Test(Parameters parameters)
	{
		var data = BitConverter.GetBytes(parameters.Value);

		var oldSpan = new BitSpan(data.AsSpan(), parameters.Offset, parameters.Length);
		var newSpan = oldSpan.Slice(parameters.SliceOffset);

		var expectedLength = parameters.Length - parameters.SliceOffset;
		newSpan.Length.Should().Be(expectedLength);

		data.Length.Should().BeLessThanOrEqualTo(4, "test uses `uint` internally");

		var expectedValue =
			(parameters.Value >> (sizeof(int) * 8 - parameters.Length - parameters.Offset))
			& ~(uint.MaxValue << expectedLength);
		var actualValue = newSpan.ReadAs<uint>();

		actualValue.Should().Be(expectedValue);
	}

	public static IEnumerable<object[]> GetTestParameters()
	{
		const int dataLen = 2;

		for (var spanSize = 1; spanSize < dataLen; spanSize++)
		{
			const uint data = 0b10101010101010101010101010101010;

			for (var length = 1; length < dataLen * 8; length++)
			{
				for (var offset = 0; offset < 8; offset++)
				{
					for (var sliceOffset = 0; sliceOffset < length; sliceOffset++)
					{
						yield return new object[]
						{
							new Parameters(data, spanSize, offset, length, sliceOffset)
						};
					}
				}
			}
		}
	}
}
