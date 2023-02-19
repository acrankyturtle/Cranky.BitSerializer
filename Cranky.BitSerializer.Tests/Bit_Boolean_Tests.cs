namespace Cranky.BitSerializer.Tests;

public class Bit_Boolean_Tests
{
	[Fact]
	public void Test_Read_True() => ReadInternal(new byte[] { 128 }, true);

	[Fact]
	public void Test_Read_False() => ReadInternal(new byte[] { 0 }, false);

	[Fact]
	public void Test_Write_True() => WriteInternal(new byte[1], true);

	[Fact]
	public void Test_Write_False() => WriteInternal(new byte[1], false);

	private static void ReadInternal(byte[] data, bool expectedValue)
	{
		var span = new BitSpan(data, 0, data.Length * 8);
		var actualValue = BitReader.ReadBool(ref span);

		actualValue.Should().Be(expectedValue);
	}

	private static void WriteInternal(byte[] data, bool value)
	{
		var span = new BitSpan(data, 0, data.Length * 8);
		BitWriter.WriteBool(ref span, value);

		data[0].Should().Be(value ? (byte)128 : (byte)0);
	}
}
