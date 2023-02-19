namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static bool ReadBool(ref BitSpan span) => ReadBinary<byte>(ref span, 1) != 0;

	public static bool ReadBool(ref ReadOnlyBitSpan span) => ReadBinary<byte>(ref span, 1) != 0;
}
