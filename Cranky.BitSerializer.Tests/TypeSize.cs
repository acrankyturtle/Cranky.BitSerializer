using System.Reflection.Emit;

namespace Cranky.BitSerializer.Tests;

internal static class TypeSize<T>
{
	// ReSharper disable once StaticMemberInGenericType (we want this to vary for each T)
	public static readonly int Size;

	static TypeSize()
	{
		var dm = new DynamicMethod("SizeOfType", typeof(int), Type.EmptyTypes);
		var il = dm.GetILGenerator();
		il.Emit(OpCodes.Sizeof, typeof(T));
		il.Emit(OpCodes.Ret);
		Size = (int)dm.Invoke(null, null)!;
	}
}
