using BenchmarkDotNet.Attributes;

namespace Cranky.BitSerializer.Perf;

public class PrimitiveNoOffset
{
	private readonly long _value;
	private readonly byte[] _buffer;

	private readonly BinaryReader _binReader;
	private readonly BinaryWriter _binaryWriter;

	public PrimitiveNoOffset()
	{
		var rng = new Random();
		_value = rng.NextInt64();

		_buffer = new byte[8];

		_binReader = new(new MemoryStream(_buffer));
		_binaryWriter = new(new MemoryStream());
	}

	#region BitSpan
	[Benchmark]
	public sbyte BitSpan_ReadInt8() => Helper.Read(_buffer, bits => BitReader.ReadInt8(ref bits));

	[Benchmark]
	public short BitSpan_ReadInt16() => Helper.Read(_buffer, bits => BitReader.ReadInt16(ref bits));

	[Benchmark]
	public int BitSpan_ReadInt32() => Helper.Read(_buffer, bits => BitReader.ReadInt32(ref bits));

	[Benchmark]
	public long BitSpan_ReadInt64() => Helper.Read(_buffer, bits => BitReader.ReadInt64(ref bits));

	[Benchmark]
	public byte BitSpan_ReadUInt8() => Helper.Read(_buffer, bits => BitReader.ReadUInt8(ref bits));

	[Benchmark]
	public ushort BitSpan_ReadUInt16() => Helper.Read(_buffer, bits => BitReader.ReadUInt16(ref bits));

	[Benchmark]
	public uint BitSpan_ReadUInt32() => Helper.Read(_buffer, bits => BitReader.ReadUInt32(ref bits));

	[Benchmark]
	public ulong BitSpan_ReadUInt64() => Helper.Read(_buffer, bits => BitReader.ReadUInt64(ref bits));

	[Benchmark]
	public void BitSpan_WriteInt8() =>
		Helper.Write(_buffer, bits => BitWriter.WriteInt8(ref bits, (sbyte)_value));

	[Benchmark]
	public void BitSpan_WriteInt16() =>
		Helper.Write(_buffer, bits => BitWriter.WriteInt16(ref bits, (short)_value));

	[Benchmark]
	public void BitSpan_WriteInt32() =>
		Helper.Write(_buffer, bits => BitWriter.WriteInt32(ref bits, (int)_value));

	[Benchmark]
	public void BitSpan_WriteInt64() => Helper.Write(_buffer, bits => BitWriter.WriteInt64(ref bits, _value));

	[Benchmark]
	public void BitSpan_WriteUInt8() =>
		Helper.Write(_buffer, bits => BitWriter.WriteUInt8(ref bits, (byte)_value));

	[Benchmark]
	public void BitSpan_WriteUInt16() =>
		Helper.Write(_buffer, bits => BitWriter.WriteUInt16(ref bits, (ushort)_value));

	[Benchmark]
	public void BitSpan_WriteUInt32() =>
		Helper.Write(_buffer, bits => BitWriter.WriteUInt32(ref bits, (uint)_value));

	[Benchmark]
	public void BitSpan_WriteUInt64() =>
		Helper.Write(_buffer, bits => BitWriter.WriteUInt64(ref bits, (ulong)_value));
#endregion

	#region BinaryReader/BinaryWriter
	[Benchmark]
	public sbyte BinaryReader_ReadInt8() => _binReader.ReadSByte();

	[Benchmark]
	public short BinaryReader_ReadInt16() => _binReader.ReadInt16();

	[Benchmark]
	public int BinaryReader_ReadInt32() => _binReader.ReadInt32();

	[Benchmark]
	public long BinaryReader_ReadInt64() => _binReader.ReadInt64();

	[Benchmark]
	public byte BinaryReader_ReadUInt8() => _binReader.ReadByte();

	[Benchmark]
	public ushort BinaryReader_ReadUInt16() => _binReader.ReadUInt16();

	[Benchmark]
	public uint BinaryReader_ReadUInt32() => _binReader.ReadUInt32();

	[Benchmark]
	public ulong BinaryReader_ReadUInt64() => _binReader.ReadUInt64();

	[Benchmark]
	public void BinaryWriter_WriteInt8() => _binaryWriter.Write((sbyte)_value);

	[Benchmark]
	public void BinaryWriter_WriteInt16() => _binaryWriter.Write((short)_value);

	[Benchmark]
	public void BinaryWriter_WriteInt32() => _binaryWriter.Write((int)_value);

	[Benchmark]
	public void BinaryWriter_WriteInt64() => _binaryWriter.Write(_value);

	[Benchmark]
	public void BinaryWriter_WriteUInt8() => _binaryWriter.Write((byte)_value);

	[Benchmark]
	public void BinaryWriter_WriteUInt16() => _binaryWriter.Write((ushort)_value);

	[Benchmark]
	public void BinaryWriter_WriteUInt32() => _binaryWriter.Write((uint)_value);

	[Benchmark]
	public void BinaryWriter_WriteUInt64() => _binaryWriter.Write((ulong)_value);
#endregion
}

internal static class Helper
{
	public static T Read<T>(byte[] buffer, ReadFunc<T> readFunc) =>
		readFunc(new(buffer, 0, buffer.Length * 8));

	public delegate T ReadFunc<out T>(BitSpan bits);

	public static BitSpan Write(byte[] buffer, WriteFunc writeFunc)
	{
		var bits = new BitSpan(buffer, 0, buffer.Length * 8);
		writeFunc(bits);
		return bits;
	}

	public delegate void WriteFunc(BitSpan bits);
}
