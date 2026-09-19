#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Fusion
{
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(NetworkLinkedList<>.DebuggerProxy))]
	public struct NetworkLinkedList<T> : IEnumerable<T>, IEnumerable, INetworkLinkedList
	{
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private bool _first;

			private int _head;

			private NetworkLinkedList<T> _list;

			public readonly T Current
			{
				get
				{
					if (_head > 0 && _head <= _list._capacity)
					{
						return _list.ReadElement(_head);
					}
					throw new InvalidOperationException();
				}
			}

			object IEnumerator.Current => Current;

			internal Enumerator(NetworkLinkedList<T> list)
			{
				_list = list;
				_head = 0;
				_first = true;
			}

			public bool MoveNext()
			{
				if (_first)
				{
					_first = false;
					_head = _list.Head;
				}
				else
				{
					if (_head == 0)
					{
						return false;
					}
					_head = _list.EntryInt(_head, 1);
				}
				return _head > 0 && _head <= _list._capacity;
			}

			public void Reset()
			{
				_head = 0;
			}

			public void Dispose()
			{
				_list = default(NetworkLinkedList<T>);
				_head = -1;
			}
		}

		internal class DebuggerProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			public Lazy<T[]> _items;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public T[] Items => _items.Value;

			public unsafe DebuggerProxy(NetworkLinkedList<T> list)
			{
				_items = new Lazy<T[]>(() => (list._data == null) ? Array.Empty<T>() : list.ToArray());
			}
		}

		public const int ELEMENT_WORDS = 2;

		public const int META_WORDS = 3;

		private unsafe int* _data;

		private int _stride;

		private int _capacity;

		private IElementReaderWriter<T> _rw;

		private const int COUNT = 0;

		private const int HEAD = 1;

		private const int TAIL = 2;

		private const int PREV = 0;

		private const int NEXT = 1;

		private const int INVALID = 0;

		private const int OFFSET = 1;

		private unsafe int Head
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return _data[1];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				_data[1] = value;
			}
		}

		private unsafe int Tail
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return _data[2];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				_data[2] = value;
			}
		}

		public unsafe int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return *_data;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private set
			{
				*_data = value;
			}
		}

		public readonly int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _capacity;
			}
		}

		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ReadElement(GetEntryIndexByListIndex(index));
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				WriteElement(GetEntryIndexByListIndex(index), value);
			}
		}

		public unsafe void Clear()
		{
			FusionUnsafe.Clear(_data, (3 + _stride * Capacity) * 4);
		}

		public bool Contains(T value)
		{
			return Contains(value, EqualityComparer<T>.Default);
		}

		public bool Contains(T value, IEqualityComparer<T> comparer)
		{
			for (int num = Head; num != 0; num = EntryInt(num, 1))
			{
				if (comparer.Equals(ReadElement(num), value))
				{
					return true;
				}
			}
			return false;
		}

		public T Set(int index, T value)
		{
			WriteElement(GetEntryIndexByListIndex(index), value);
			return value;
		}

		public T Get(int index)
		{
			return ReadElement(GetEntryIndexByListIndex(index));
		}

		public int IndexOf(T value)
		{
			return IndexOf(value, EqualityComparer<T>.Default);
		}

		public int IndexOf(T value, IEqualityComparer<T> equalityComparer)
		{
			int num = Head;
			int num2 = 0;
			while (num != 0)
			{
				if (equalityComparer.Equals(ReadElement(num), value))
				{
					return num2;
				}
				num = EntryInt(num, 1);
				num2++;
			}
			return -1;
		}

		public bool Remove(T value)
		{
			return Remove(value, EqualityComparer<T>.Default);
		}

		public bool Remove(T value, IEqualityComparer<T> equalityComparer)
		{
			for (int num = Head; num != 0; num = EntryInt(num, 1))
			{
				if (equalityComparer.Equals(ReadElement(num), value))
				{
					RemoveEntry(num);
					return true;
				}
			}
			return false;
		}

		public void Add(T value)
		{
			Assert.Check((uint)Count <= Capacity, "(uint) Count <= Capacity");
			if (Count == Capacity)
			{
				throw new InvalidOperationException("NetworkList is full");
			}
			int num = FindFreeEntry();
			Assert.Check(num != 0, "entryIndex != INVALID");
			int count = Count + 1;
			Count = count;
			WriteElement(num, value);
			SetEntryInt(num, 0, Tail);
			SetEntryInt(num, 1, 0);
			if (Tail != 0)
			{
				SetEntryInt(Tail, 1, num);
				Tail = num;
			}
			else
			{
				Head = num;
				Tail = num;
			}
		}

		private int FindFreeEntry()
		{
			for (int i = 0; i < _capacity; i++)
			{
				int num = i + 1;
				if (num != Head && num != Tail && EntryInt(num, 0) == 0)
				{
					return num;
				}
			}
			Assert.AlwaysFail("No free entry");
			return 0;
		}

		private void RemoveEntry(int entryIndex)
		{
			Assert.Check((uint)Count <= Capacity, "(uint) Count <= Capacity");
			int num = EntryInt(entryIndex, 0);
			int num2 = EntryInt(entryIndex, 1);
			if (num != 0)
			{
				SetEntryInt(num, 1, num2);
			}
			if (num2 != 0)
			{
				SetEntryInt(num2, 0, num);
			}
			if (Tail == entryIndex)
			{
				Tail = num;
			}
			if (Head == entryIndex)
			{
				Head = num2;
			}
			SetEntryInt(entryIndex, 0, 0);
			SetEntryInt(entryIndex, 1, 0);
			int count = Count - 1;
			Count = count;
		}

		private int GetEntryIndexByListIndex(int listIndex)
		{
			int num = listIndex;
			int num2 = Head;
			while (num2 != 0)
			{
				if (listIndex == 0)
				{
					return num2;
				}
				num2 = EntryInt(num2, 1);
				listIndex--;
			}
			throw new IndexOutOfRangeException(num.ToString());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly int EntryOffset(int index)
		{
			return 3 + _stride * (index - 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe readonly int EntryInt(int entryIndex, int field)
		{
			return _data[EntryOffset(entryIndex) + field];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void SetEntryInt(int entryIndex, int field, int value)
		{
			_data[EntryOffset(entryIndex) + field] = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe readonly T ReadElement(int entryIndex)
		{
			int num = EntryOffset(entryIndex) + 2;
			return _rw.Read((byte*)_data + (nint)num * (nint)4, 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void WriteElement(int entryIndex, T value)
		{
			int num = EntryOffset(entryIndex) + 2;
			_rw.Write((byte*)_data + (nint)num * (nint)4, 0, value);
		}

		public T[] ToArray()
		{
			T[] array = new T[Count];
			int num = 0;
			for (int num2 = Head; num2 != 0; num2 = EntryInt(num2, 1))
			{
				array[num++] = ReadElement(num2);
			}
			return array;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		public unsafe NetworkLinkedList(byte* data, int capacity, IElementReaderWriter<T> rw)
		{
			Assert.Check(FusionUnsafe.IsAligned(data, 4), "FusionUnsafe.IsAligned(data, Allocator.REPLICATE_WORD_ALIGN)");
			_rw = rw;
			_data = (int*)data;
			_capacity = capacity;
			_stride = rw.GetElementWordCount() + 2;
		}

		public unsafe NetworkLinkedList<T> Remap(void* list)
		{
			Assert.Check(_data, "_data");
			Assert.Check(_capacity > 0, "_capacity > 0");
			Assert.Check(_rw, "_rw");
			NetworkLinkedList<T> result = this;
			result._data = (int*)list;
			return result;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		void INetworkLinkedList.Add(object item)
		{
			Add((T)item);
		}
	}
}
