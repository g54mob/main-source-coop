using System;

namespace Fusion
{
	public readonly struct UniquePtr
	{
		public unsafe readonly void* Pointer;

		public readonly uint TypeId;

		public unsafe UniquePtr(void* ptr, uint typeId)
		{
			Pointer = ptr;
			TypeId = typeId;
		}

		public unsafe static implicit operator bool(UniquePtr ptr)
		{
			return ptr.Pointer != null;
		}

		public static UniquePtr MoveUniquePtr(ref UniquePtr ptr)
		{
			UniquePtr result = ptr;
			ptr = default(UniquePtr);
			return result;
		}

		public unsafe static UniquePtr<T> MakeUniquePtrT<T>(ref T* ptr) where T : unmanaged
		{
			UniquePtr<T> result = new UniquePtr<T>(ptr);
			ptr = default(T*);
			return result;
		}

		[Obsolete("Should not be used directly. Generator should create a type-specific method")]
		public unsafe static UniquePtr MakeUniquePtr<T>(ref T* ptr) where T : unmanaged
		{
			throw new NotSupportedException();
		}

		[Obsolete("Should not be used directly. Generator should create a type-specific method")]
		public unsafe static bool ReleaseUniquePtr<T>(ref UniquePtr ptr, out T* result) where T : unmanaged
		{
			throw new NotSupportedException();
		}

		public unsafe static void FreeNative(ref UniquePtr memory)
		{
			void* ptr = memory.Pointer;
			memory = default(UniquePtr);
			FusionUnsafe.Free(ref ptr);
		}

		public static UniquePtr<T> MoveUniquePtr<T>(ref UniquePtr<T> ptr) where T : unmanaged
		{
			UniquePtr<T> result = ptr;
			ptr = default(UniquePtr<T>);
			return result;
		}

		public unsafe static bool ReleaseUniquePtr<T>(ref UniquePtr<T> ptr, out T* result) where T : unmanaged
		{
			if (ptr.Pointer == null)
			{
				result = null;
				return false;
			}
			result = ptr.Pointer;
			ptr = default(UniquePtr<T>);
			return true;
		}

		public unsafe static void FreeNative<T>(ref UniquePtr<T> memory) where T : unmanaged
		{
			T* ptr = memory.Pointer;
			memory = default(UniquePtr<T>);
			FusionUnsafe.Free(ref ptr);
		}
	}
	public readonly struct UniquePtr<T> where T : unmanaged
	{
		public unsafe readonly T* Pointer;

		public unsafe UniquePtr(T* ptr)
		{
			Pointer = ptr;
		}

		public unsafe static implicit operator bool(UniquePtr<T> ptr)
		{
			return ptr.Pointer != null;
		}
	}
}
