#define DEBUG
using System;
using System.Runtime.CompilerServices;

namespace Fusion
{
	internal class NetworkObjectHeaderSnapshot
	{
		internal NetworkObjectHeaderSnapshot Prev;

		internal NetworkObjectHeaderSnapshot Next;

		public Tick Tick;

		public int WordCount;

		private unsafe int* _ptr;

		public unsafe NetworkObjectHeaderPtr HeaderPtr
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Assert.Always(_ptr != null, "_ptr != null");
				return new NetworkObjectHeaderPtr((NetworkObjectHeader*)_ptr);
			}
		}

		public unsafe ref NetworkObjectHeader Header
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Assert.Always(_ptr != null, "_ptr != null");
				return ref *(NetworkObjectHeader*)_ptr;
			}
		}

		public unsafe Span<int> Raw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (_ptr != null) ? new Span<int>(_ptr, WordCount) : Span<int>.Empty;
			}
		}

		public unsafe void PoolInit(Simulation simulation, int wordCount)
		{
			Assert.Check(_ptr == null, "_ptr == null");
			WordCount = wordCount;
			_ptr = simulation.NativeAllocObjectSnapshot(wordCount);
		}

		public unsafe void PoolReset(Simulation simulation)
		{
			simulation.NativeFreeObjectSnapshot(ref _ptr, WordCount);
			Tick = default(Tick);
			WordCount = 0;
		}

		public unsafe void AllocRaw(Allocator allocator, int wordCount)
		{
			Assert.Check(_ptr == null, "_ptr == null");
			WordCount = wordCount;
			_ptr = (int*)Allocator.AllocAndClear(allocator, wordCount * 4);
		}

		public unsafe void FreeRaw(Allocator allocator)
		{
			Tick = default(Tick);
			WordCount = 0;
			Allocator.Free(allocator, ref _ptr);
		}

		public NetworkObjectHeaderSnapshot Clone(Simulation simulation)
		{
			NetworkObjectHeaderSnapshot networkObjectHeaderSnapshot = simulation.AcquireNetworkObjectHeaderSnapshot(WordCount);
			networkObjectHeaderSnapshot.CopyFrom(this);
			return networkObjectHeaderSnapshot;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(int[] destination)
		{
			FusionUnsafe.CopyToExact(destination.AsSpan(), Raw);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void CopyFrom(NetworkObjectMeta source)
		{
			Assert.Check(source.WordCount == WordCount, "source.WordCount == WordCount");
			FusionUnsafe.Copy(_ptr, source.RawPtr, WordCount * 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void CopyTo(NetworkObjectMeta destination)
		{
			Assert.Check(destination.WordCount == WordCount, "destination.WordCount == WordCount");
			FusionUnsafe.Copy(destination.RawPtr, _ptr, WordCount * 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void CopyFrom(NetworkObjectHeaderSnapshot source)
		{
			Assert.Check(source.WordCount == WordCount, "source.WordCount == WordCount");
			FusionUnsafe.Copy(_ptr, source._ptr, WordCount * 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void CopyTo(NetworkObjectHeaderSnapshot destination)
		{
			Assert.Check(destination.WordCount == WordCount, "destination.WordCount == WordCount");
			FusionUnsafe.Copy(destination._ptr, _ptr, WordCount * 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe int* GetBehaviourPtr(NetworkBehaviour behaviour)
		{
			Assert.Always(behaviour != null && behaviour.WordOffset < WordCount, "behaviour != null && behaviour.WordOffset < WordCount");
			return (_ptr != null) ? (_ptr + behaviour.WordOffset) : null;
		}

		internal ulong BuildCRC()
		{
			return CRC64.Compute(Raw);
		}
	}
}
