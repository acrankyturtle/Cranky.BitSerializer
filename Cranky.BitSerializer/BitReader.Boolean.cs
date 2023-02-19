namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static bool ReadBool(ref BitSpan span) => Read(ref span, 1, b => b) != 0;
}
