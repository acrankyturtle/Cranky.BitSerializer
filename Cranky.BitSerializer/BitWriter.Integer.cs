using System.Numerics;

namespace Cranky.BitSerializer;

public static partial class Extensions_BitWriter
{
	private static BitWriter WriteInteger<T>(
		this BitWriter writer,
		T value,
		int numBits,
		Func<T, byte> castAsByteFunc
	)
		where T : IBinaryNumber<T>, IShiftOperators<T, int, T> =>
		writer.Write(value, numBits, castAsByteFunc);

	public static BitWriter WriteInt8(this BitWriter writer, sbyte value) =>
		writer.WriteInteger(value, sizeof(sbyte) * 8, x => (byte)x);

	public static BitWriter WriteUInt8(this BitWriter writer, byte value) =>
		writer.WriteInteger(value, sizeof(byte) * 8, x => x);

	public static BitWriter WriteInt16(this BitWriter writer, short value) =>
		writer.WriteInteger(value, sizeof(short) * 8, x => (byte)x);

	public static BitWriter WriteUInt16(this BitWriter writer, ushort value) =>
		writer.WriteInteger(value, sizeof(ushort) * 8, x => (byte)x);

	public static BitWriter WriteInt32(this BitWriter writer, int value) =>
		writer.WriteInteger(value, sizeof(int) * 8, x => (byte)x);

	public static BitWriter WriteUInt32(this BitWriter writer, uint value) =>
		writer.WriteInteger(value, sizeof(uint) * 8, x => (byte)x);

	public static BitWriter WriteInt64(this BitWriter writer, long value) =>
		writer.WriteInteger(value, sizeof(long) * 8, x => (byte)x);

	public static BitWriter WriteUInt64(this BitWriter writer, ulong value) =>
		writer.WriteInteger(value, sizeof(ulong) * 8, x => (byte)x);
}
