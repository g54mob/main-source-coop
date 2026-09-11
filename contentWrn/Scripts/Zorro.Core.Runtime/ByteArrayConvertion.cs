using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

public static class ByteArrayConvertion
{
	public unsafe static void MoveToByteArray<T>(ref NativeArray<T> src, ref byte[] dst) where T : struct
	{
		int num = UnsafeUtility.SizeOf<T>() * src.Length;
		byte* unsafeReadOnlyPtr = (byte*)src.GetUnsafeReadOnlyPtr();
		if (dst.Length != num)
		{
			dst = new byte[num];
		}
		fixed (byte* ptr = dst)
		{
			UnsafeUtility.MemCpy(ptr, unsafeReadOnlyPtr, num);
		}
	}

	public unsafe static void MoveFromByteArray<T>(ref byte[] src, ref NativeArray<T> dst) where T : struct
	{
		int num = UnsafeUtility.SizeOf<T>();
		if (src.Length != num * dst.Length)
		{
			dst.Dispose();
			dst = new NativeArray<T>(src.Length / num, Allocator.Persistent);
		}
		byte* unsafeReadOnlyPtr = (byte*)dst.GetUnsafeReadOnlyPtr();
		fixed (byte* ptr = src)
		{
			UnsafeUtility.MemCpy(unsafeReadOnlyPtr, ptr, src.Length);
		}
	}
}
