namespace Cranky.BitSerializer.Tests;

internal static class Utilities
{
	private static readonly Random _random = new(42);

	public static Span<byte> GetRandomBytes(int numBytes)
	{
		var data = new byte[numBytes];
		_random.NextBytes(data);

		return data.AsSpan();
	}
}
