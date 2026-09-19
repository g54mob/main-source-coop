#define DEBUG
using System;

namespace Fusion.Sockets
{
	internal struct NetBitBufferStack
	{
		private int _capacity;

		public unsafe NetBitBuffer** Stack;

		public int Count;

		public unsafe bool TryPop(NetBitBuffer** result)
		{
			Assert.Check(Count >= 0, "Count >= 0");
			if (Count == 0)
			{
				return false;
			}
			*result = Stack[--Count];
			return true;
		}

		public unsafe static NetBitBufferStack Create(int capacity)
		{
			return new NetBitBufferStack
			{
				_capacity = capacity,
				Stack = FusionUnsafe.AllocAndClearPtrArray<NetBitBuffer>(capacity, 8, "Fusion\\Fusion.Sockets\\NetBitBufferStack.cs", 25)
			};
		}

		public unsafe static void Dispose(ref NetBitBufferStack stack)
		{
			FusionUnsafe.Free(ref stack.Stack);
			stack.Count = 0;
			stack._capacity = 0;
		}

		public unsafe void PushFromHead(NetBitBuffer* head)
		{
			while (head != null)
			{
				NetBitBuffer* next = head->Next;
				head->Next = null;
				Assert.Check(Count >= 0 && Count <= _capacity, "Count >= 0 && Count <= _capacity");
				if (Count == _capacity)
				{
					try
					{
						Stack = FusionUnsafe.ExpandPtrArray(Stack, _capacity, _capacity * 2, "Fusion\\Fusion.Sockets\\NetBitBufferStack.cs", 47);
					}
					catch (OutOfMemoryException)
					{
						InternalLogStreams.LogInfo?.Log($"OOM resize to _capacity:{_capacity}, Count:{Count}");
						throw;
					}
					_capacity *= 2;
				}
				Stack[Count++] = head;
				head = next;
			}
		}
	}
}
