namespace Cranky.BitSerializer;

public static partial class BitWriter
{
	public static void WriteBool(ref BitSpan span, bool value) =>
		WriteBinary(ref span, value ? (byte)1 : (byte)0, 1);
}
