using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Zorro.Core
{
	public static class NativeArrayExtensions
	{
		public static byte[] ToByteArray(this NativeArray<byte> nativeArray)
		{
			byte[] dst = new byte[nativeArray.Length];
			ByteArrayConvertion.MoveToByteArray(ref nativeArray, ref dst);
			return dst;
		}

		public unsafe static T[,] GetManaged2DArray<T>(this NativeArray<T> values, int sizeX, int sizeY) where T : unmanaged
		{
			T[,] array = new T[sizeX, sizeY];
			UnsafeUtility.MemCpy(source: values.GetUnsafeReadOnlyPtr(), destination: UnsafeUtility.PinGCArrayAndGetDataAddress(array, out var gcHandle), size: sizeX * sizeY * UnsafeUtility.SizeOf<T>());
			UnsafeUtility.ReleaseGCObject(gcHandle);
			return array;
		}
	}
}
