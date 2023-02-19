using System.Numerics;

namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static sbyte ReadInt8(ref BitSpan span) => Read(ref span, sizeof(sbyte) * 8, x => (sbyte)x);

	public static byte ReadUInt8(ref BitSpan span) => Read(ref span, sizeof(byte) * 8, x => x);

	public static short ReadInt16(ref BitSpan span) => Read(ref span, sizeof(short) * 8, x => (short)x);

	public static ushort ReadUInt16(ref BitSpan span) => Read(ref span, sizeof(ushort) * 8, x => (ushort)x);

	public static int ReadInt32(ref BitSpan span) => Read(ref span, sizeof(int) * 8, x => (int)x);

	public static uint ReadUInt32(ref BitSpan span) => Read(ref span, sizeof(uint) * 8, x => (uint)x);

	public static long ReadInt64(ref BitSpan span) => Read(ref span, sizeof(long) * 8, x => (long)x);

	public static ulong ReadUInt64(ref BitSpan span) => Read(ref span, sizeof(ulong) * 8, x => (ulong)x);
}
