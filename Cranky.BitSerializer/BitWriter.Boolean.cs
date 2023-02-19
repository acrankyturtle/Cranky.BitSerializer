namespace Cranky.BitSerializer;

public static partial class Extensions_BitWriter
{
	public static BitWriter WriteBool(this BitWriter writer, bool value) =>
		writer.Write(value ? (byte)1 : (byte)0, 1, b => b);
}
