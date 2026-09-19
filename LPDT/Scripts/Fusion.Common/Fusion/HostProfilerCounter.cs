#define DEBUG
using System.Runtime.CompilerServices;

namespace Fusion
{
	public readonly struct HostProfilerCounter<T> where T : unmanaged
	{
		public unsafe readonly void* Ptr;

		public unsafe HostProfilerCounter(void* ptr)
		{
			Ptr = ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Set(T value)
		{
			if (Ptr != null)
			{
				Unsafe.Write(Ptr, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Add(T delta)
		{
			if (Ptr != null)
			{
				if (typeof(T) == typeof(int))
				{
					Unsafe.AsRef<int>(Ptr) += Unsafe.As<T, int>(ref delta);
				}
				else if (typeof(T) == typeof(float))
				{
					Unsafe.AsRef<float>(Ptr) += Unsafe.As<T, float>(ref delta);
				}
				else if (typeof(T) == typeof(double))
				{
					Unsafe.AsRef<double>(Ptr) += Unsafe.As<T, double>(ref delta);
				}
				else
				{
					Assert.Fail("Type not supported");
				}
			}
		}
	}
}
