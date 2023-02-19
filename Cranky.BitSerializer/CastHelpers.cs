using System.Runtime.CompilerServices;

namespace Cranky.BitSerializer;

internal static class CastHelpers
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T FromByte<T>(ref byte b) => Unsafe.As<byte, T>(ref b);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte ToByte<T>(T value) => Unsafe.As<T, byte>(ref value);
}
