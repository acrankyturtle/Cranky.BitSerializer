namespace Cranky.BitSerializer;

static partial class BitReader
{
	public static sbyte ReadInt8(ref BitSpan span) => ReadBinary<sbyte>(ref span, sizeof(sbyte) * 8);

	public static sbyte ReadInt8(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(sbyte) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<sbyte>(ref span, numBits);
	}

	public static byte ReadUInt8(ref BitSpan span) => ReadBinary<byte>(ref span, sizeof(byte) * 8);

	public static byte ReadUInt8(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(byte) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<byte>(ref span, numBits);
	}

	public static short ReadInt16(ref BitSpan span) => ReadBinary<short>(ref span, sizeof(short) * 8);

	public static short ReadInt16(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(short) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<short>(ref span, numBits);
	}

	public static ushort ReadUInt16(ref BitSpan span) => ReadBinary<ushort>(ref span, sizeof(ushort) * 8);

	public static ushort ReadUInt16(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(ushort) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<ushort>(ref span, numBits);
	}

	public static int ReadInt32(ref BitSpan span) => ReadBinary<int>(ref span, sizeof(int) * 8);

	public static int ReadInt32(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(int) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<int>(ref span, numBits);
	}

	public static uint ReadUInt32(ref BitSpan span) => ReadBinary<uint>(ref span, sizeof(uint) * 8);

	public static uint ReadUInt32(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(uint) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<uint>(ref span, numBits);
	}

	public static long ReadInt64(ref BitSpan span) => ReadBinary<long>(ref span, sizeof(long) * 8);

	public static long ReadInt64(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(long) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<long>(ref span, numBits);
	}

	public static ulong ReadUInt64(ref BitSpan span) => ReadBinary<ulong>(ref span, sizeof(ulong) * 8);

	public static ulong ReadUInt64(ref BitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(ulong) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<ulong>(ref span, numBits);
	}

	#region Read Only


	public static sbyte ReadInt8(ref ReadOnlyBitSpan span) => ReadBinary<sbyte>(ref span, sizeof(sbyte) * 8);

	public static sbyte ReadInt8(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(sbyte) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<sbyte>(ref span, numBits);
	}

	public static byte ReadUInt8(ref ReadOnlyBitSpan span) => ReadBinary<byte>(ref span, sizeof(byte) * 8);

	public static byte ReadUInt8(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(byte) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<byte>(ref span, numBits);
	}

	public static short ReadInt16(ref ReadOnlyBitSpan span) => ReadBinary<short>(ref span, sizeof(short) * 8);

	public static short ReadInt16(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(short) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<short>(ref span, numBits);
	}

	public static ushort ReadUInt16(ref ReadOnlyBitSpan span) =>
		ReadBinary<ushort>(ref span, sizeof(ushort) * 8);

	public static ushort ReadUInt16(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(ushort) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<ushort>(ref span, numBits);
	}

	public static int ReadInt32(ref ReadOnlyBitSpan span) => ReadBinary<int>(ref span, sizeof(int) * 8);

	public static int ReadInt32(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(int) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<int>(ref span, numBits);
	}

	public static uint ReadUInt32(ref ReadOnlyBitSpan span) => ReadBinary<uint>(ref span, sizeof(uint) * 8);

	public static uint ReadUInt32(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(uint) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<uint>(ref span, numBits);
	}

	public static long ReadInt64(ref ReadOnlyBitSpan span) => ReadBinary<long>(ref span, sizeof(long) * 8);

	public static long ReadInt64(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(long) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<long>(ref span, numBits);
	}

	public static ulong ReadUInt64(ref ReadOnlyBitSpan span) =>
		ReadBinary<ulong>(ref span, sizeof(ulong) * 8);

	public static ulong ReadUInt64(ref ReadOnlyBitSpan span, int numBits)
	{
		const int minSize = 1;
		if (numBits < minSize)
			throw SizeOutOfMinimumRange(numBits, minSize);

		const int maxSize = sizeof(ulong) * 8;
		if (numBits > maxSize)
			throw SizeOutOfMaximumRange(maxSize, numBits);

		return ReadBinary<ulong>(ref span, numBits);
	}
	#endregion

	private static Exception SizeOutOfMinimumRange(int minSize, int numBits) =>
		new ArgumentOutOfRangeException(
			nameof(numBits),
			$"Number of bits specified to be read is {numBits}, but minimum size of {minSize} expected."
		);

	private static Exception SizeOutOfMaximumRange(int maxSize, int numBits) =>
		new ArgumentOutOfRangeException(
			nameof(numBits),
			$"Number of bits specified to be read is {numBits}, but maximum size of {maxSize} expected."
		);
}
