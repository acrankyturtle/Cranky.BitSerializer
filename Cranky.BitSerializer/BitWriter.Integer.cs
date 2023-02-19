using System.Numerics;

namespace Cranky.BitSerializer;

public static partial class BitWriter
{
	private static void WriteInteger<T>(ref BitSpan span, T value, int numBits)
		where T : IBinaryInteger<T> => WriteBinary(ref span, value, numBits);

	public static void WriteInt8(ref BitSpan span, sbyte value) =>
		WriteInteger(ref span, value, sizeof(sbyte) * 8);

	public static void WriteInt8(ref BitSpan span, sbyte value, int numBits) =>
		WriteInteger(ref span, value, numBits);

	public static void WriteUInt8(ref BitSpan span, byte value) =>
		WriteInteger(ref span, value, sizeof(byte) * 8);

	public static void WriteUInt8(ref BitSpan span, byte value, int numBits) =>
		WriteInteger(ref span, value, numBits);

	public static void WriteInt16(ref BitSpan span, short value) =>
		WriteInteger(ref span, value, sizeof(short) * 8);

	public static void WriteInt16(ref BitSpan span, short value, int numBits) =>
		WriteInteger(ref span, value, numBits);

	public static void WriteUInt16(ref BitSpan span, ushort value) =>
		WriteInteger(ref span, value, sizeof(ushort) * 8);

	public static void WriteUInt16(ref BitSpan span, ushort value, int numBits) =>
		WriteInteger(ref span, value, numBits);

	public static void WriteInt32(ref BitSpan span, int value) =>
		WriteInteger(ref span, value, sizeof(int) * 8);

	public static void WriteInt32(ref BitSpan span, int value, int numBits) =>
		WriteInteger(ref span, value, numBits);

	public static void WriteUInt32(ref BitSpan span, uint value) =>
		WriteInteger(ref span, value, sizeof(uint) * 8);

	public static void WriteUInt32(ref BitSpan span, uint value, int numBits) =>
		WriteInteger(ref span, value, numBits);

	public static void WriteInt64(ref BitSpan span, long value) =>
		WriteInteger(ref span, value, sizeof(long) * 8);

	public static void WriteInt64(ref BitSpan span, long value, int numBits) =>
		WriteInteger(ref span, value, numBits);

	public static void WriteUInt64(ref BitSpan span, ulong value) =>
		WriteInteger(ref span, value, sizeof(ulong) * 8);

	public static void WriteUInt64(ref BitSpan span, ulong value, int numBits) =>
		WriteInteger(ref span, value, numBits);
}
