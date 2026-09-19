#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fusion.Statistics;

namespace Fusion
{
	public sealed class Allocator : IDisposable
	{
		private struct Block
		{
			public int Prev;

			public int Next;

			public int Bucket;

			public Ptr SegmentsFree;

			public int SegmentsUsed;

			public int SegmentsAllocated;

			public readonly int Index;

			public int AllocCount;

			public Block(int index)
			{
				SegmentsFree = default(Ptr);
				SegmentsUsed = 0;
				SegmentsAllocated = 0;
				AllocCount = 0;
				Prev = -1;
				Next = -1;
				Bucket = 255;
				Index = index;
			}

			public unsafe int SegmentsFreeCount(in Allocator a)
			{
				int num = 0;
				Ptr ptr = SegmentsFree;
				while ((bool)ptr)
				{
					num++;
					ptr = ((Segment*)a.AsVoid(ptr))->Next;
				}
				return num;
			}

			public unsafe bool SegmentsFreeContains(in Allocator a, Ptr ptr)
			{
				Assert.Check(ptr, "ptr");
				Ptr ptr2 = SegmentsFree;
				while ((bool)ptr2)
				{
					if (ptr2 == ptr)
					{
						return true;
					}
					ptr2 = ((Segment*)a.AsVoid(ptr2))->Next;
				}
				return false;
			}

			public override readonly string ToString()
			{
				return $"[Block: Bucket={Bucket}, SegmentsUsed={SegmentsUsed}, SegmentsAllocated={SegmentsAllocated}, Index={Index}]";
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 8)]
		private struct BlockList
		{
			[FieldOffset(0)]
			public int Head;

			[FieldOffset(4)]
			public int Tail;

			public const int SIZE = 8;

			public const int WORD_COUNT = 2;

			internal const int BYTE_OF_HEAD = 0;

			internal const int BYTE_COUNT_OF_HEAD = 4;

			internal const int BYTE_OF_TAIL = 4;

			internal const int BYTE_COUNT_OF_TAIL = 4;

			private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

			public readonly bool IsEmpty => Head == -1;

			public BlockList()
			{
				Head = -1;
				Tail = -1;
			}

			public void AddFirst(in Allocator a, ref Block item)
			{
				Assert.Check(item.Next == -1, "item.Next == BLOCK_INVALID");
				Assert.Check(item.Prev == -1, "item.Prev == BLOCK_INVALID");
				Assert.Check(!Contains(in a, ref item), "Contains(a, ref item) == false");
				item.Next = Head;
				item.Prev = -1;
				if (Head > -1)
				{
					a._blocks[Head].Prev = item.Index;
					Head = item.Index;
				}
				else
				{
					Head = item.Index;
					Tail = item.Index;
				}
				Assert.Check(Contains(in a, ref item), "Contains(a, ref item)");
				DebugVerifyListIntegrity(in a);
			}

			public void AddLast(in Allocator a, ref Block item)
			{
				Assert.Check(item.Next == -1, "item.Next == BLOCK_INVALID");
				Assert.Check(item.Prev == -1, "item.Prev == BLOCK_INVALID");
				Assert.Check(!Contains(in a, ref item), "Contains(a, ref item) == false");
				item.Next = -1;
				item.Prev = Tail;
				if (Tail > -1)
				{
					a._blocks[Tail].Next = item.Index;
					Tail = item.Index;
				}
				else
				{
					Head = item.Index;
					Tail = item.Index;
				}
				Assert.Check(Contains(in a, ref item), "Contains(a, ref item)");
				DebugVerifyListIntegrity(in a);
			}

			public void MoveFirst(in Allocator a, ref Block item)
			{
				Assert.Check(Contains(in a, ref item), "Contains(a, ref item)");
				if (item.Index != Head)
				{
					Remove(in a, ref item);
					AddFirst(in a, ref item);
				}
			}

			public void MoveLast(in Allocator a, ref Block item)
			{
				Assert.Check(Contains(in a, ref item), "Contains(a, ref item)");
				if (item.Index != Tail)
				{
					Remove(in a, ref item);
					AddLast(in a, ref item);
				}
			}

			public ref Block RemoveHead(in Allocator a)
			{
				Assert.Check(!IsEmpty, "!IsEmpty");
				ref Block reference = ref a._blocks[Head];
				Remove(in a, ref reference);
				return ref reference;
			}

			public void Remove(in Allocator a, ref Block item)
			{
				Assert.Check(Contains(in a, ref item), "Contains(a, ref item)");
				if (item.Prev > -1)
				{
					a._blocks[item.Prev].Next = item.Next;
				}
				if (item.Next > -1)
				{
					a._blocks[item.Next].Prev = item.Prev;
				}
				if (item.Index == Tail)
				{
					Tail = item.Prev;
				}
				if (item.Index == Head)
				{
					Head = item.Next;
				}
				item.Prev = -1;
				item.Next = -1;
				DebugVerifyListIntegrity(in a);
				Assert.Check(!Contains(in a, ref item), "Contains(a, ref item) == false");
			}

			public readonly bool Contains(in Allocator a, ref Block item)
			{
				for (int num = Head; num > -1; num = a._blocks[num].Next)
				{
					if (num == item.Index)
					{
						return true;
					}
				}
				return false;
			}

			[Conditional("DEBUG")]
			private readonly void DebugVerifyListIntegrity(in Allocator a)
			{
				int num = Head;
				while (num > -1)
				{
					ref Block reference = ref a._blocks[num];
					if (num == Head)
					{
						Assert.Check(reference.Prev == -1, "block.Prev == BLOCK_INVALID");
					}
					if (num == Tail)
					{
						Assert.Check(reference.Next == -1, "block.Next == BLOCK_INVALID");
					}
					if (num != Head && num != Tail)
					{
						Assert.Check(reference.Prev > -1, "block.Prev > BLOCK_INVALID");
						Assert.Check(reference.Next > -1, "block.Next > BLOCK_INVALID");
					}
					num = reference.Next;
				}
			}

			public override readonly string ToString()
			{
				return $"[BlockList: Head={Head}, Tail={Tail}]";
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 16)]
		private readonly struct Bucket
		{
			[FieldOffset(0)]
			public readonly int Index;

			[FieldOffset(4)]
			public readonly int SegmentStride;

			[FieldOffset(8)]
			public readonly int SegmentWordCount;

			[FieldOffset(12)]
			public readonly int SegmentCapacity;

			public const int SIZE = 16;

			public const int WORD_COUNT = 4;

			internal const int BYTE_OF_INDEX = 0;

			internal const int BYTE_COUNT_OF_INDEX = 4;

			internal const int BYTE_OF_SEGMENT_STRIDE = 4;

			internal const int BYTE_COUNT_OF_SEGMENT_STRIDE = 4;

			internal const int BYTE_OF_SEGMENT_WORD_COUNT = 8;

			internal const int BYTE_COUNT_OF_SEGMENT_WORD_COUNT = 4;

			internal const int BYTE_OF_SEGMENT_CAPACITY = 12;

			internal const int BYTE_COUNT_OF_SEGMENT_CAPACITY = 4;

			private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

			public Bucket(int index, int stride, int wordCount, int capacity)
			{
				Index = index;
				SegmentStride = stride;
				SegmentWordCount = wordCount;
				SegmentCapacity = capacity;
			}

			public static Bucket Create(int index, int wordCount, Config config)
			{
				return new Bucket(index, wordCount * 8, wordCount, (wordCount > 0) ? (config.BlockWordCount / wordCount) : 0);
			}

			public override string ToString()
			{
				return $"[Bucket: Index={Index}, SegmentStride={SegmentStride}, SegmentWordCount={SegmentWordCount}, SegmentCapacity={SegmentCapacity}]";
			}
		}

		private static class AllocatorBucketSize
		{
			public static readonly int[] Sizes = new int[57]
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 10,
				12, 14, 16, 20, 24, 28, 32, 40, 48, 56,
				64, 80, 96, 112, 128, 160, 192, 224, 256, 320,
				384, 448, 512, 640, 768, 896, 1024, 1280, 1536, 1792,
				2048, 2560, 3072, 3584, 4096, 5120, 6144, 7168, 8192, 10240,
				12288, 14336, 16384, 20480, 24576, 28672, 32768
			};
		}

		[StructLayout(LayoutKind.Explicit, Size = 12)]
		public struct Config
		{
			public const PageSizes DEFAULT_BLOCK_SHIFT = PageSizes._32Kb;

			public const int DEFAULT_BLOCK_COUNT = 256;

			[FieldOffset(0)]
			public int BlockShift;

			[FieldOffset(4)]
			public int BlockCount;

			[FieldOffset(8)]
			public int GlobalsSize;

			public const int SIZE = 12;

			public const int WORD_COUNT = 3;

			internal const int BYTE_OF_BLOCK_SHIFT = 0;

			internal const int BYTE_COUNT_OF_BLOCK_SHIFT = 4;

			internal const int BYTE_OF_BLOCK_COUNT = 4;

			internal const int BYTE_COUNT_OF_BLOCK_COUNT = 4;

			internal const int BYTE_OF_GLOBALS_SIZE = 8;

			internal const int BYTE_COUNT_OF_GLOBALS_SIZE = 4;

			private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

			public readonly int BlockByteSize => 1 << BlockShift;

			public readonly int BlockWordCount => WordCount(BlockByteSize);

			public readonly int HeapSizeUsable => BlockByteSize * BlockCount;

			public readonly int HeapSizeAllocated => HeapSizeUsable + 8;

			public Config(PageSizes shift, int count, int globalsSize)
			{
				BlockShift = (int)shift;
				BlockCount = Math.Max(1, count);
				GlobalsSize = globalsSize;
			}

			public readonly bool Equals(Config other)
			{
				return BlockShift == other.BlockShift && BlockCount == other.BlockCount;
			}

			public override readonly bool Equals(object obj)
			{
				return obj is Config other && Equals(other);
			}

			public override readonly int GetHashCode()
			{
				return (BlockShift * 397) ^ BlockCount;
			}

			public override readonly string ToString()
			{
				return $"[Allocator.Config: {12}/{BlockShift}/{BlockCount}/{GlobalsSize}/{BlockByteSize}/{BlockWordCount}/{HeapSizeUsable}/{HeapSizeAllocated}]";
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 4)]
		[NetworkStructWeaved(1)]
		private struct Ptr : IEquatable<Ptr>, INetworkStruct
		{
			public sealed class EqualityComparer : IEqualityComparer<Ptr>
			{
				public bool Equals(Ptr x, Ptr y)
				{
					return x.Address == y.Address;
				}

				public int GetHashCode(Ptr obj)
				{
					return obj.Address;
				}
			}

			[FieldOffset(0)]
			public int Address;

			public const int SIZE = 4;

			public const int WORD_COUNT = 1;

			internal const int BYTE_OF_ADDRESS = 0;

			internal const int BYTE_COUNT_OF_ADDRESS = 4;

			private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

			public static Ptr Null => default(Ptr);

			public readonly bool Equals(Ptr other)
			{
				return Address == other.Address;
			}

			public override readonly bool Equals(object obj)
			{
				return obj is Ptr other && Equals(other);
			}

			public override readonly int GetHashCode()
			{
				return Address;
			}

			public override readonly string ToString()
			{
				return $"0x{Address:X}";
			}

			public static implicit operator bool(Ptr a)
			{
				return a.Address != 0;
			}

			public static bool operator ==(Ptr a, Ptr b)
			{
				return a.Address == b.Address;
			}

			public static bool operator !=(Ptr a, Ptr b)
			{
				return a.Address != b.Address;
			}

			public static Ptr operator +(Ptr p, int v)
			{
				p.Address += v;
				return p;
			}

			public static Ptr operator -(Ptr p, int v)
			{
				p.Address -= v;
				return p;
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 4)]
		private struct Segment
		{
			[FieldOffset(0)]
			public Ptr Next;

			public const int SIZE = 4;

			public const int WORD_COUNT = 1;

			internal const int BYTE_OF_NEXT = 0;

			internal const int BYTE_COUNT_OF_NEXT = 4;

			private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;
		}

		private const int WORD_SHIFT = 3;

		private const int WORD_BYTE_SIZE = 8;

		internal const byte PATTERN = 170;

		public const int HEAP_ALIGNMENT = 8;

		public const int REPLICATE_WORD_SHIFT = 2;

		public const int REPLICATE_WORD_SIZE = 4;

		public const int REPLICATE_WORD_ALIGN = 4;

		public const int BUCKET_COUNT = 57;

		public const byte BUCKET_INVALID = byte.MaxValue;

		private const int PTR_SIZE = 8;

		private unsafe byte* _root;

		private unsafe byte* _heap;

		private readonly Block[] _blocks;

		private BlockList _blocksFreeList;

		private readonly Bucket[] _buckets;

		private readonly byte[] _bucketsMap;

		private readonly BlockList[] _bucketsLists;

		private readonly Config _config;

		private HashSet<IntPtr> _allocated = new HashSet<IntPtr>();

		private const int SentinelLeadingSize = 0;

		private const int SentinelTrailingSize = 0;

		private const ulong SentinelLeadingPattern = 6086271824754218827uL;

		private const ulong SentinelTrailingPattern = 14358455476591988877uL;

		private const int BLOCK_INVALID = -1;

		internal Config Configuration => _config;

		internal static bool Free<T>(Allocator allocator, ref T[] ptr) where T : unmanaged
		{
			if (ptr == null)
			{
				return false;
			}
			ptr = null;
			return true;
		}

		internal unsafe static bool Free<T>(Allocator allocator, ref T* ptr) where T : unmanaged
		{
			if (ptr == null)
			{
				return false;
			}
			T* ptr2 = ptr;
			ptr = null;
			try
			{
				allocator.FreeInternal(ptr2);
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(error);
			}
			return true;
		}

		internal unsafe static T* AllocAndClearArray<T>(Allocator allocator, int length) where T : unmanaged
		{
			return (T*)allocator.AllocAndClear(sizeof(T) * length);
		}

		internal unsafe static void* AllocAndClear(Allocator allocator, int size)
		{
			return allocator.AllocAndClear(size);
		}

		internal unsafe static T* AllocAndClear<T>(Allocator allocator) where T : unmanaged
		{
			return (T*)allocator.AllocAndClear(sizeof(T));
		}

		internal static void Dispose(Allocator allocator)
		{
			allocator.Dispose();
		}

		public unsafe string LogPointerInfo(void* p)
		{
			return $"heap-start:{(IntPtr)_heap} heap-end:{(IntPtr)(_heap + _config.HeapSizeUsable)} ptr:{(IntPtr)p}";
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe Ptr AsPtr(void* p)
		{
			Assert.Check(IsPointerInHeap(p), "IsPointerInHeap(p)");
			Ptr result = default(Ptr);
			result.Address = (int)((byte*)p - _root);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void* AsVoid(Ptr ptr)
		{
			Assert.Check(ptr, "ptr");
			byte* ptr2 = _root + ptr.Address;
			Assert.Check(IsPointerInHeap(ptr2), ptr.Address);
			return ptr2;
		}

		internal unsafe bool IsPointerInHeap(void* p)
		{
			return p >= _heap && p < _heap + _config.HeapSizeUsable;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int WordCount(int size)
		{
			Assert.Check(size > 0, "size > 0");
			return size + 7 >> 3;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ref Bucket GetBucket(int index)
		{
			Assert.Check(index >= 0 && index < 57, "index >= 0 && index < BUCKET_COUNT");
			return ref _buckets[index];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ref Bucket GetBucketForBlock(ref Block block)
		{
			return ref GetBucket(block.Bucket);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ref BlockList GetBucketList(int index)
		{
			Assert.Check(index >= 0 && index < 57, "index >= 0 && index < BUCKET_COUNT");
			return ref _bucketsLists[index];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ref Block GetBlock(int index)
		{
			Assert.Check(index >= 0 && index < _config.BlockCount, "index >= 0 && index < _config.BlockCount");
			return ref _blocks[index];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ref Block GetBlock(long index)
		{
			Assert.Check(index >= 0 && index < _config.BlockCount, "index >= 0 && index < _config.BlockCount");
			return ref _blocks[index];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetBlockBucket(long index)
		{
			Assert.Check(index >= 0 && index < _config.BlockCount, "index >= 0 && index < _config.BlockCount");
			return _blocks[index].Bucket;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe ref Block GetBlockForPointer(void* ptr)
		{
			Assert.Check(IsPointerInHeap(ptr), "IsPointerInHeap(ptr)");
			Assert.Check((byte*)ptr - _heap >> _config.BlockShift >= 0, "((((byte*) ptr) - _heap) >> _config.BlockShift) >= 0");
			Assert.Check((byte*)ptr - _heap >> _config.BlockShift < _config.BlockCount, "((((byte*) ptr) - _heap) >> _config.BlockShift) < _config.BlockCount");
			return ref _blocks[(byte*)ptr - _heap >> _config.BlockShift];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe int GetBlockIndexForPointer(void* ptr)
		{
			Assert.Check(IsPointerInHeap(ptr), "IsPointerInHeap(ptr)");
			Assert.Check((byte*)ptr - _heap >> _config.BlockShift >= 0, "((((byte*) ptr) - _heap) >> _config.BlockShift) >= 0");
			Assert.Check((byte*)ptr - _heap >> _config.BlockShift < _config.BlockCount, "((((byte*) ptr) - _heap) >> _config.BlockShift) < _config.BlockCount");
			return (int)((byte*)ptr - _heap >> _config.BlockShift);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe byte* GetBlockMemory(ref Block block)
		{
			return _heap + block.Index * _config.BlockByteSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe byte* GetBlockMemory(long blockIndex)
		{
			Assert.Check(blockIndex >= 0 && blockIndex < _config.BlockCount, "blockIndex >= 0 && blockIndex < _config.BlockCount");
			return _heap + blockIndex * _config.BlockByteSize;
		}

		internal unsafe bool TryGetSegmentRoot(void* ptr, out void* root, out long segmentIndex)
		{
			if (IsPointerInHeap(ptr))
			{
				long num = (byte*)ptr - _heap >> _config.BlockShift;
				ref Block block = ref GetBlock(num);
				byte* blockMemory = GetBlockMemory(num);
				segmentIndex = ((byte*)ptr - blockMemory) / _buckets[block.Bucket].SegmentStride;
				root = blockMemory + segmentIndex * _buckets[block.Bucket].SegmentStride;
				return true;
			}
			root = null;
			segmentIndex = 0L;
			return false;
		}

		internal unsafe void* GetSegmentRoot(void* ptr)
		{
			if (TryGetSegmentRoot(ptr, out var root, out var _))
			{
				return root;
			}
			Assert.AlwaysFail("[Allocator] TryGetSegmentRoot failed");
			return null;
		}

		[return: NotNull]
		internal unsafe T* AllocArray<T>(int length) where T : unmanaged
		{
			return (T*)Alloc(sizeof(T) * length);
		}

		[return: NotNull]
		internal unsafe T* AllocAndClearArray<T>(int length) where T : unmanaged
		{
			return (T*)AllocAndClear(sizeof(T) * length);
		}

		[return: NotNull]
		internal unsafe T* Alloc<T>() where T : unmanaged
		{
			return (T*)Alloc(sizeof(T));
		}

		[return: NotNull]
		internal unsafe T* AllocAndClear<T>() where T : unmanaged
		{
			return (T*)AllocAndClear(sizeof(T));
		}

		[return: NotNull]
		internal unsafe void* AllocAndClear(int size)
		{
			void* ptr = Alloc(size);
			FusionUnsafe.Clear(ptr, size);
			return ptr;
		}

		internal void GetMemorySnapshot(ref FusionAllocatorMemorySnapshot snapshot)
		{
			ref int[] bucketFullBlocksCount = ref snapshot.BucketFullBlocksCount;
			if (bucketFullBlocksCount == null)
			{
				bucketFullBlocksCount = new int[57];
			}
			bucketFullBlocksCount = ref snapshot.BucketAllocatedSegmentsCount;
			if (bucketFullBlocksCount == null)
			{
				bucketFullBlocksCount = new int[57];
			}
			bucketFullBlocksCount = ref snapshot.BucketFreeSegmentsCount;
			if (bucketFullBlocksCount == null)
			{
				bucketFullBlocksCount = new int[57];
			}
			bucketFullBlocksCount = ref snapshot.BucketSegmentCapacity;
			if (bucketFullBlocksCount == null)
			{
				bucketFullBlocksCount = new int[57];
			}
			bucketFullBlocksCount = ref snapshot.BucketUsedSegmentsCount;
			if (bucketFullBlocksCount == null)
			{
				bucketFullBlocksCount = new int[57];
			}
			snapshot.TotalBytesFree = 0;
			snapshot.TotalBytesUsed = 0;
			int num = 0;
			int num2 = _blocksFreeList.Head;
			while (num2 > -1)
			{
				ref Block block = ref GetBlock(num2);
				num++;
				num2 = block.Next;
			}
			snapshot.TotalFreeBlocks = num;
			snapshot.TotalBlocks = _config.BlockCount;
			for (int i = 0; i < 57; i++)
			{
				ref Bucket bucket = ref GetBucket(i);
				BlockList bucketList = GetBucketList(i);
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = bucketList.Head;
				while (num8 > -1)
				{
					ref Block block2 = ref GetBlock(num8);
					if (block2.SegmentsUsed == bucket.SegmentCapacity)
					{
						num3++;
					}
					num4 += block2.SegmentsAllocated;
					num5 += block2.SegmentsFreeCount(in this);
					num6 += block2.SegmentsUsed;
					num7 += bucket.SegmentCapacity;
					num8 = block2.Next;
				}
				snapshot.BucketFullBlocksCount[i] = num3;
				snapshot.BucketAllocatedSegmentsCount[i] = num4;
				snapshot.BucketFreeSegmentsCount[i] = num5;
				snapshot.BucketSegmentCapacity[i] = num7;
				snapshot.BucketUsedSegmentsCount[i] = num6;
				snapshot.TotalBytesUsed += num4 * bucket.SegmentStride;
				snapshot.TotalBytesFree += (bucket.SegmentCapacity - num4) * bucket.SegmentStride;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool CanAllocSize(int size)
		{
			if (size < 1 || size >= _config.BlockByteSize)
			{
				return false;
			}
			return true;
		}

		internal void ValidateSentinels()
		{
		}

		internal unsafe bool ValidatePointer(void* ptr, out string error)
		{
			if (!IsPointerInHeap(ptr))
			{
				error = $"Pointer doesn't belong to this heap, ptr:{(IntPtr)ptr}, heap-start:{(IntPtr)_heap}, heap-end:{(IntPtr)(_heap + _config.HeapSizeUsable)}";
				return false;
			}
			ref Block blockForPointer = ref GetBlockForPointer(ptr);
			string error2 = "";
			ValidateSegmentSentinels(ptr, GetBucket(blockForPointer.Bucket).SegmentStride, ref error2);
			if (error2.Length > 0)
			{
				error = error2;
				return false;
			}
			error = string.Empty;
			return true;
		}

		[return: NotNull]
		private unsafe void* Alloc(int size)
		{
			if (!CanAllocSize(size))
			{
				throw new InvalidOperationException($"[Allocator] Invalid size. [Size={size}, BlockByteSize={_config.BlockByteSize}]");
			}
			size = size;
			Assert.Check(WordCount(size) * 8 >= size, "WordCount(size) * WORD_BYTE_SIZE >= size");
			byte b = _bucketsMap[WordCount(size)];
			for (int i = b; i < 57; i++)
			{
				DebugVerifyBucketIntegrity(i);
				ref Bucket bucket = ref GetBucket(i);
				ref BlockList bucketList = ref GetBucketList(i);
				Assert.Check(bucket.SegmentStride >= size, "bucket.SegmentStride    >= size");
				Assert.Check(bucket.SegmentWordCount >= WordCount(size), "bucket.SegmentWordCount >= WordCount(size)");
				if (bucketList.Head > -1)
				{
					ref Block block = ref GetBlock(bucketList.Head);
					void* ptr = TryAllocateSegmentFromBlock(in bucket, ref block, size);
					if (ptr != null)
					{
						Assert.Check(IsPointerInHeap(ptr), "IsPointerInHeap(segment)");
						Assert.Check(block.SegmentsAllocated > 0, "block.SegmentsAllocated       > 0");
						return ptr;
					}
				}
				if (!_blocksFreeList.IsEmpty)
				{
					ref Block reference = ref _blocksFreeList.RemoveHead(in this);
					Assert.Check(reference.SegmentsFree == default(Ptr), "block.SegmentsFree      == default");
					Assert.Check(reference.SegmentsUsed == 0, "block.SegmentsUsed      == 0");
					Assert.Check(reference.SegmentsAllocated == 0, "block.SegmentsAllocated == 0");
					Assert.Check(reference.Prev == -1, "block.Prev              == BLOCK_INVALID");
					Assert.Check(reference.Next == -1, "block.Next              == BLOCK_INVALID");
					Assert.Check(reference.Bucket == 255, "block.Bucket            == BUCKET_INVALID");
					reference.Bucket = bucket.Index;
					bucketList.AddFirst(in this, ref reference);
					void* ptr2 = TryAllocateSegmentFromBlock(in bucket, ref reference, size);
					if (ptr2 == null)
					{
						throw new Exception($"[Allocator] Failed to allocate segment from block. [Size={size}, Block={reference.ToString()}]");
					}
					Assert.Check(GetBlockIndexForPointer(ptr2) == reference.Index, "GetBlockIndexForPointer(result) == block.Index");
					Assert.Check(reference.SegmentsAllocated > 0, "block.SegmentsAllocated           > 0");
					Assert.Check(IsPointerInHeap(ptr2), "IsPointerInHeap(result)");
					return ptr2;
				}
			}
			throw new OutOfMemoryException($"[Allocator] Out of Memory. All buckets are full. [Size={size}]");
		}

		private unsafe void* TryAllocateSegmentFromBlock(in Bucket bucket, ref Block block, int size)
		{
			Assert.Check(bucket.Index == block.Bucket, "bucket.Index == block.Bucket");
			Assert.Check(GetBucketList(bucket.Index).Contains(in this, ref block), "GetBucketList(bucket.Index).Contains(this, ref block)");
			Assert.Check(block.SegmentsAllocated >= 0, "block.SegmentsAllocated >= 0");
			Assert.Check(bucket.SegmentStride >= size, "bucket.SegmentStride >= size");
			bool flag = false;
			Assert.Check(block.SegmentsFreeCount(in this) + block.SegmentsAllocated == block.SegmentsUsed, "(block.SegmentsFreeCount(this) + block.SegmentsAllocated) == block.SegmentsUsed");
			void* ptr;
			int num;
			if (block.SegmentsFree.Address > 0)
			{
				Assert.Check(block.SegmentsUsed > 0, "block.SegmentsUsed > 0");
				ptr = AsVoid(block.SegmentsFree);
				block.SegmentsFree = ((Segment*)ptr)->Next;
				((Segment*)ptr)->Next = default(Ptr);
				flag = true;
				num = 666;
			}
			else if (block.SegmentsUsed < bucket.SegmentCapacity)
			{
				ptr = GetBlockMemory(ref block) + block.SegmentsUsed++ * bucket.SegmentStride;
				Assert.Check(block.SegmentsUsed <= bucket.SegmentCapacity, "block.SegmentsUsed <= bucket.SegmentCapacity");
				num = 1;
			}
			else
			{
				ptr = null;
				num = 2;
			}
			if (ptr != null)
			{
				block.AllocCount++;
				Assert.Check(block.SegmentsAllocated < bucket.SegmentCapacity, "block.SegmentsAllocated < bucket.SegmentCapacity");
				if (++block.SegmentsAllocated == bucket.SegmentCapacity)
				{
					GetBucketList(bucket.Index).MoveLast(in this, ref block);
				}
				if (!_allocated.Add((IntPtr)ptr))
				{
					InternalLogStreams.LogError?.Log($"{(IntPtr)ptr} already in _allocated set");
				}
				ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(ptr, bucket.SegmentStride);
				if (flag)
				{
					readOnlySpan = readOnlySpan.Slice(4, readOnlySpan.Length - 4);
				}
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					if (readOnlySpan[i] != 170)
					{
						Assert.Always(TryGetSegmentRoot(ptr, out var _, out var segmentIndex), "TryGetSegmentRoot(result, out _, out var segmentIndex)");
						InternalLogStreams.LogError?.Log($"Expected {(byte)170:X2} at index [{i}] but found {readOnlySpan[i]} (segmentIndex: {segmentIndex}) {num}");
						break;
					}
				}
			}
			Assert.Check(block.SegmentsFreeCount(in this) + block.SegmentsAllocated == block.SegmentsUsed, "(block.SegmentsFreeCount(this) + block.SegmentsAllocated) == block.SegmentsUsed");
			DebugVerifyBucketIntegrity(bucket.Index);
			return ptr;
		}

		private unsafe void FreeInternal(void* ptr)
		{
			if (!IsPointerInHeap(ptr))
			{
				InternalLogStreams.LogError?.Log(string.Format("{0} doesn't belong to this heap, ptr:{1}, heap-start:{2}, heap-end:{3}", "ptr", (IntPtr)ptr, (IntPtr)_heap, (IntPtr)(_heap + _config.HeapSizeUsable)));
				return;
			}
			ref Block blockForPointer = ref GetBlockForPointer(ptr);
			ptr = ptr;
			if (!_allocated.Remove((IntPtr)ptr))
			{
				InternalLogStreams.LogError?.Log($"{(IntPtr)ptr} was not in _allocated set, {LogPointerInfo(ptr)}");
				return;
			}
			if (blockForPointer.SegmentsFreeContains(in this, AsPtr(ptr)))
			{
				InternalLogStreams.LogError?.Log($"{(IntPtr)ptr} was in free list, {LogPointerInfo(ptr)}");
				return;
			}
			if (blockForPointer.SegmentsAllocated == 0)
			{
				InternalLogStreams.LogError?.Log("block has no segments allocated, " + LogPointerInfo(ptr));
				return;
			}
			int segmentStride = _buckets[blockForPointer.Bucket].SegmentStride;
			Segment* ptr2 = (Segment*)ptr;
			byte* ptr3 = (byte*)ptr2;
			for (int i = 0; i < segmentStride; i++)
			{
				ptr3[i] = 170;
			}
			ptr2->Next = blockForPointer.SegmentsFree;
			blockForPointer.SegmentsFree = AsPtr(ptr2);
			if (--blockForPointer.SegmentsAllocated == 0)
			{
				int bucket = blockForPointer.Bucket;
				GetBucketList(bucket).Remove(in this, ref blockForPointer);
				blockForPointer.Bucket = 255;
				blockForPointer.SegmentsFree = default(Ptr);
				blockForPointer.SegmentsUsed = 0;
				blockForPointer.SegmentsAllocated = 0;
				byte* blockMemory = GetBlockMemory(ref blockForPointer);
				for (int j = 0; j < _config.BlockByteSize; j++)
				{
					blockMemory[j] = 170;
				}
				_blocksFreeList.AddFirst(in this, ref blockForPointer);
				DebugVerifyBucketIntegrity(bucket);
			}
			else
			{
				Assert.Check(blockForPointer.SegmentsFreeContains(in this, AsPtr(ptr)), "block.SegmentsFreeContains(this, AsPtr(ptr))");
				ref Bucket bucketForBlock = ref GetBucketForBlock(ref blockForPointer);
				if (bucketForBlock.SegmentCapacity == blockForPointer.SegmentsAllocated + 1)
				{
					GetBucketList(blockForPointer.Bucket).MoveFirst(in this, ref blockForPointer);
				}
				DebugVerifyBucketIntegrity(bucketForBlock.Index);
			}
		}

		[Conditional("DEBUG")]
		private void DebugVerifyBucketIntegrity(int index)
		{
			ref Bucket bucket = ref GetBucket(index);
			BlockList bucketList = GetBucketList(index);
			int num = bucketList.Head;
			bool flag = false;
			while (num > -1)
			{
				ref Block reference = ref _blocks[num];
				while (true)
				{
					if (flag)
					{
						Assert.Check(reference.SegmentsUsed == bucket.SegmentCapacity, "block.SegmentsUsed == bucket.SegmentCapacity");
						Assert.Check(reference.SegmentsAllocated == bucket.SegmentCapacity, "block.SegmentsAllocated == bucket.SegmentCapacity");
						Assert.Check(reference.SegmentsFreeCount(in this) == 0, "block.SegmentsFreeCount(this) == 0");
						break;
					}
					if (reference.SegmentsAllocated == bucket.SegmentCapacity)
					{
						flag = true;
						continue;
					}
					Assert.Check(reference.SegmentsFreeCount(in this) + reference.SegmentsAllocated == reference.SegmentsUsed, reference.SegmentsFreeCount(in this) + reference.SegmentsAllocated, reference.SegmentsUsed);
					break;
				}
				num = reference.Next;
			}
		}

		public unsafe void Dispose()
		{
			FusionUnsafe.Free(ref _root);
			_heap = null;
		}

		private unsafe Allocator(Config config)
		{
			Assert.Check(sizeof(Ptr) == 4, "sizeof(Ptr) == Ptr.SIZE");
			Assert.Check(sizeof(Bucket) == 16, "sizeof(Bucket) == Bucket.SIZE");
			Assert.Check(sizeof(Config) == 12, "sizeof(Config) == Config.SIZE");
			Assert.Check(sizeof(Segment) == 4, "sizeof(Segment) == Segment.SIZE");
			Assert.Check(sizeof(BlockList) == 8, "sizeof(BlockList) == BlockList.SIZE");
			_config = config;
			_buckets = new Bucket[57];
			_bucketsMap = new byte[config.BlockWordCount];
			_bucketsLists = new BlockList[57];
			for (int i = 0; i < 57; i++)
			{
				_bucketsLists[i] = new BlockList();
			}
			_blocks = new Block[config.BlockCount];
			_blocksFreeList = new BlockList();
			Assert.Always(condition: true, "HEAP_ALIGNMENT == FusionUnsafe.DefaultAlignment");
			_root = (byte*)FusionUnsafe.AllocAndClear(config.HeapSizeAllocated + 8, 8, "Fusion\\Fusion.Runtime\\Allocator\\Allocator.cs", 836);
			Assert.Check(FusionUnsafe.IsAligned(_root), "FusionUnsafe.IsAligned(_root, HEAP_ALIGNMENT)");
			_heap = _root + 8;
			for (int j = 0; j < config.HeapSizeAllocated; j++)
			{
				_heap[j] = 170;
			}
			for (int k = 0; k < AllocatorBucketSize.Sizes.Length; k++)
			{
				_buckets[k] = Bucket.Create(k, AllocatorBucketSize.Sizes[k], config);
			}
			byte b = 0;
			for (int l = 0; l < config.BlockWordCount; l++)
			{
				if (_buckets[b].SegmentWordCount < l)
				{
					b++;
				}
				Assert.Check(_buckets[b].SegmentWordCount >= l, "_buckets[b].SegmentWordCount >= i");
				_bucketsMap[l] = b;
			}
			for (int m = 0; m < config.BlockCount; m++)
			{
				ref Block reference = ref _blocks[m];
				reference = new Block(m);
				_blocksFreeList.AddLast(in this, ref reference);
			}
		}

		public static Allocator Create(Config config)
		{
			return new Allocator(config);
		}

		[Conditional("ENABLE_ALLOCATOR_SENTINELS")]
		private unsafe static void InitSegmentSentinels(void* memory, int size)
		{
			ulong num = 6086271824754218827uL;
			ulong num2 = 14358455476591988877uL;
			Span<byte> dst = new Span<byte>(memory, 0);
			Span<byte> dst2 = new Span<byte>((byte*)memory + size, 0);
			BinUtils.RepeatingCopyTo(new ReadOnlySpan<byte>(&num, 8), dst);
			BinUtils.RepeatingCopyTo(new ReadOnlySpan<byte>(&num2, 8), dst2);
		}

		[Conditional("ENABLE_ALLOCATOR_SENTINELS")]
		private unsafe void ValidateSegmentSentinels(void* memory, int size)
		{
			ulong num = 6086271824754218827uL;
			ulong num2 = 14358455476591988877uL;
			ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(memory, 0);
			ReadOnlySpan<byte> readOnlySpan2 = new ReadOnlySpan<byte>((byte*)memory + size, 0);
			bool flag = !readOnlySpan.RepeatingSequenceEqualTo(new ReadOnlySpan<byte>(&num, 8));
			bool flag2 = !readOnlySpan2.RepeatingSequenceEqualTo(new ReadOnlySpan<byte>(&num2, 8));
			if (flag)
			{
				InternalLogStreams.LogError?.Log($"MSG500 Leading sentinel mismatch (Ptr:{(IntPtr)memory}, Size:{size}, Config:{_config}): {BinUtils.BytesToHex(readOnlySpan, 0)}");
			}
			if (flag2)
			{
				InternalLogStreams.LogError?.Log($"MSG501 Trailing sentinel mismatch (Ptr:{(IntPtr)memory}, Size:{size}, Config:{_config}): {BinUtils.BytesToHex(readOnlySpan2, 0)}");
			}
		}

		private unsafe static void ValidateSegmentSentinels(void* memory, int size, ref string error)
		{
			ulong num = 6086271824754218827uL;
			ulong num2 = 14358455476591988877uL;
			ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(memory, 0);
			ReadOnlySpan<byte> readOnlySpan2 = new ReadOnlySpan<byte>((byte*)memory + size, 0);
			bool flag = !readOnlySpan.RepeatingSequenceEqualTo(new ReadOnlySpan<byte>(&num, 8));
			bool flag2 = !readOnlySpan2.RepeatingSequenceEqualTo(new ReadOnlySpan<byte>(&num2, 8));
			if (flag && flag2)
			{
				error = "Leading & trailing mismatch: [" + BinUtils.BytesToHex(readOnlySpan, 0) + "], [" + BinUtils.BytesToHex(readOnlySpan2, 0) + "]";
			}
			else if (flag)
			{
				error = "Trailing sentinel mismatch: [" + BinUtils.BytesToHex(readOnlySpan, 0) + "]";
			}
			else if (flag2)
			{
				error = "Trailing sentinel mismatch: [" + BinUtils.BytesToHex(readOnlySpan2, 0) + "]";
			}
		}
	}
}
