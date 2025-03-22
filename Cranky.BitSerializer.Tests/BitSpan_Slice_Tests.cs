namespace Cranky.BitSerializer.Tests;

public class BitSpan_Slice_Tests
{
	public record Parameters(uint Value, int OffsetBits, int LengthBits, int SliceOffsetBits);

	[Fact]
	public void Can_slice_to_zero_length()
	{
		var data = new byte[2];
		var span = new BitSpan(data, 0, data.Length * 8);

		var newSpan = span[(data.Length * 8)..];
		newSpan.Length.Should().Be(0);
	}

	[Theory]
	[MemberData(nameof(GetTestParameters))]
	public void Test(Parameters parameters)
	{
		var data = BitConverter.GetBytes(parameters.Value);

		if (data.Length > 4)
			throw new ArgumentException("test uses `uint` internally");

		if (BitConverter.IsLittleEndian)
			// easier to work with big endian
			Array.Reverse(data);

		var oldSpan = new BitSpan(data.AsSpan(), parameters.OffsetBits, parameters.LengthBits);
		var oldSpanReadOnly = (ReadOnlyBitSpan)oldSpan;
		var newSpan = oldSpan[parameters.SliceOffsetBits..];
		var newSpanReadOnly = oldSpanReadOnly[parameters.SliceOffsetBits..];

		var expectedLength = parameters.LengthBits - parameters.SliceOffsetBits;
		newSpan.Length.Should().Be(expectedLength);
		newSpanReadOnly.Length.Should().Be(expectedLength);

		var rightShift = sizeof(uint) * 8 - parameters.OffsetBits - parameters.LengthBits;
		var leftMask = ~(~0 << expectedLength);
		var expectedValue = (uint)((parameters.Value >> rightShift) & leftMask);

		var actualValue = newSpan.ReadAs<uint>();
		var actualValueReadOnly = newSpanReadOnly.ReadAs<uint>();

		actualValue.Should().Be(expectedValue);
		actualValueReadOnly.Should().Be(expectedValue);
	}

	public static IEnumerable<object[]> GetTestParameters()
	{
		const int dataLen = 2;

		for (var spanSize = 1; spanSize < dataLen; spanSize++)
		{
			const uint data = 0b11111110101011111110101111101110;

			for (var lengthBits = 1; lengthBits < dataLen * 8; lengthBits++)
			{
				for (var offsetBits = 0; offsetBits < lengthBits; offsetBits++)
				{
					for (var sliceOffsetBits = 0; sliceOffsetBits < lengthBits; sliceOffsetBits++)
					{
						yield return
						[
							new Parameters(data, offsetBits, lengthBits, sliceOffsetBits),
						];
					}
				}
			}
		}
	}
}
