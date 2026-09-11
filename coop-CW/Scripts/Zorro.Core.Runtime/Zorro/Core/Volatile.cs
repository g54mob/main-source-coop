using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Zorro.Core
{
	public static class Volatile
	{
		[StructLayout(LayoutKind.Explicit, Size = 128)]
		public struct PaddedLong
		{
			[FieldOffset(64)]
			private long _value;

			public PaddedLong(long value)
			{
				_value = value;
			}

			public long ReadUnfenced()
			{
				return _value;
			}

			public long ReadAcquireFence()
			{
				long value = _value;
				Thread.MemoryBarrier();
				return value;
			}

			public long ReadFullFence()
			{
				Thread.MemoryBarrier();
				return _value;
			}

			[MethodImpl(MethodImplOptions.NoOptimization)]
			public long ReadCompilerOnlyFence()
			{
				return _value;
			}

			public void WriteReleaseFence(long newValue)
			{
				Thread.MemoryBarrier();
				_value = newValue;
			}

			public void WriteFullFence(long newValue)
			{
				Thread.MemoryBarrier();
				_value = newValue;
			}

			[MethodImpl(MethodImplOptions.NoOptimization)]
			public void WriteCompilerOnlyFence(long newValue)
			{
				_value = newValue;
			}

			public void WriteUnfenced(long newValue)
			{
				_value = newValue;
			}

			public bool AtomicCompareExchange(long newValue, long comparand)
			{
				return Interlocked.CompareExchange(ref _value, newValue, comparand) == comparand;
			}

			public long AtomicExchange(long newValue)
			{
				return Interlocked.Exchange(ref _value, newValue);
			}

			public long AtomicAddAndGet(long delta)
			{
				return Interlocked.Add(ref _value, delta);
			}

			public long AtomicIncrementAndGet()
			{
				return Interlocked.Increment(ref _value);
			}

			public long AtomicDecrementAndGet()
			{
				return Interlocked.Decrement(ref _value);
			}

			public override string ToString()
			{
				return ReadFullFence().ToString();
			}
		}

		private const int CacheLineSize = 64;
	}
}
