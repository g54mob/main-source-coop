#define DEBUG
using System;
using System.Threading;

namespace Fusion.Sockets
{
	internal struct NetBitBufferBlock
	{
		private int _packetSize;

		private IntPtr _freeHead;

		private unsafe NetBitBufferBlock* _self;

		private unsafe NetBitBuffer* _allocatedHead;

		public unsafe static void Dispose(ref NetBitBufferBlock* block)
		{
			if (block != null)
			{
				NetBitBuffer* ptr = block->_allocatedHead;
				while (ptr != null)
				{
					Assert.Check(ptr->_block == block, "alloc->_block == block");
					NetBitBuffer* allocNext = ptr->_allocNext;
					ptr->_block = null;
					FusionUnsafe.Free(ref ptr);
					ptr = allocNext;
				}
				FusionUnsafe.Free(ref block);
			}
		}

		public unsafe static NetBitBufferBlock* Create(int packetSize)
		{
			NetBitBufferBlock* ptr = FusionUnsafe.AllocAndClear<NetBitBufferBlock>(8, "Fusion\\Fusion.Sockets\\NetBitBufferBlock.cs", 33);
			ptr->_self = ptr;
			ptr->_freeHead = default(IntPtr);
			ptr->_packetSize = packetSize;
			return ptr;
		}

		public unsafe void Release(NetBitBuffer* ptr)
		{
			Assert.Check(ptr->_block == _self, "ptr->_block == _self");
			IntPtr freeHead;
			do
			{
				freeHead = _freeHead;
				ptr->Next = (NetBitBuffer*)(void*)freeHead;
			}
			while (Interlocked.CompareExchange(ref _freeHead, (IntPtr)ptr, freeHead) != freeHead);
		}

		public unsafe NetBitBuffer* TryAcquire()
		{
			if (TryAcquire(out var ptr))
			{
				return ptr;
			}
			return null;
		}

		public unsafe bool TryAcquire(out NetBitBuffer* ptr)
		{
			IntPtr intPtr;
			do
			{
				intPtr = Volatile.Read(ref _freeHead);
				if (intPtr == IntPtr.Zero)
				{
					NetBitBuffer* ptr2 = NetBitBuffer.Allocate(0, _packetSize);
					ptr2->_block = _self;
					ptr2->_allocNext = _allocatedHead;
					_allocatedHead = ptr2;
					intPtr = new IntPtr(ptr2);
					break;
				}
			}
			while (Interlocked.CompareExchange(ref _freeHead, (IntPtr)((NetBitBuffer*)(void*)intPtr)->Next, intPtr) != intPtr);
			ptr = (NetBitBuffer*)(void*)intPtr;
			Assert.Check(ptr->_block == _self, "ptr->_block == _self");
			ptr->SetBufferLengthBytes(ptr->Data, _packetSize);
			FusionUnsafe.Clear(ptr->Data, _packetSize);
			ptr->OffsetBits = 0;
			ptr->_block = _self;
			return true;
		}
	}
}
