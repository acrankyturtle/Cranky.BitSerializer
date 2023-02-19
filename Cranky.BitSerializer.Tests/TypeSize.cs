using System.Reflection.Emit;

namespace Cranky.BitSerializer.Tests;

internal static class TypeSize<T>
{
	public readonly static int Size;

	static TypeSize()
	{
		var dm = new DynamicMethod("SizeOfType", typeof(int), Type.EmptyTypes);
		ILGenerator il = dm.GetILGenerator();
		il.Emit(OpCodes.Sizeof, typeof(T));
		il.Emit(OpCodes.Ret);
		Size = (int)dm.Invoke(null, null)!;
	}
}
