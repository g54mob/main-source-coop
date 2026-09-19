#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fusion
{
	internal class RingBuffer<T> : IEnumerable<T>, IEnumerable where T : struct
	{
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private int _index;

			public readonly T Current => _003CringBuffer_003EP[_index];

			object IEnumerator.Current => Current;

			public Enumerator(RingBuffer<T> ringBuffer)
			{
				_003CringBuffer_003EP = ringBuffer;
				_index = -1;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				return ++_index < _003CringBuffer_003EP.Count;
			}

			public void Reset()
			{
				_index = -1;
			}
		}

		private readonly T[] _buffer;

		private int _front;

		private int _count;

		public int Count => _count;

		public int Capacity => _buffer.Length;

		public bool IsEmpty => _count == 0;

		public bool IsFull => _count == _buffer.Length;

		public T this[int index]
		{
			get
			{
				if (IsEmpty)
				{
					throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer is empty.");
				}
				if (index >= _count)
				{
					throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer has {_count} items.");
				}
				int num = InternalIndex(index);
				return _buffer[num];
			}
			set
			{
				if (IsEmpty)
				{
					throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer is empty.");
				}
				if (index >= _count)
				{
					throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer has {_count} items.");
				}
				int num = InternalIndex(index);
				_buffer[num] = value;
			}
		}

		public RingBuffer(int capacity)
			: this(capacity, new T[0])
		{
		}

		public RingBuffer(int capacity, T[] items)
		{
			if (capacity < 1)
			{
				throw new ArgumentException("Buffer cannot have negative or zero capacity.", "capacity");
			}
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (items.Length > capacity)
			{
				throw new ArgumentException("Number of items exceeds buffer capacity.", "items");
			}
			_buffer = new T[capacity];
			Array.Copy(items, _buffer, items.Length);
			_front = 0;
			_count = items.Length;
		}

		public ref readonly T Front()
		{
			ThrowIfEmpty();
			return ref _buffer[_front];
		}

		public ref T FrontMut()
		{
			ThrowIfEmpty();
			return ref _buffer[_front];
		}

		public ref readonly T Get(int index)
		{
			if (IsEmpty)
			{
				throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer is empty.");
			}
			if (index >= _count)
			{
				throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer has {_count} items.");
			}
			int num = InternalIndex(index);
			return ref _buffer[num];
		}

		public ref T GetMut(int index)
		{
			if (IsEmpty)
			{
				throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer is empty.");
			}
			if (index >= _count)
			{
				throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer has {_count} items.");
			}
			int num = InternalIndex(index);
			return ref _buffer[num];
		}

		public ref readonly T Back()
		{
			ThrowIfEmpty();
			int num = Decrement(BackIndex());
			return ref _buffer[num];
		}

		public ref T BackMut()
		{
			ThrowIfEmpty();
			int num = Decrement(BackIndex());
			return ref _buffer[num];
		}

		public void PushBack(T item)
		{
			int num = BackIndex();
			if (IsFull)
			{
				_buffer[num] = item;
				_front = Increment(num);
			}
			else
			{
				_buffer[num] = item;
				_count++;
			}
		}

		public void PushFront(T item)
		{
			_front = Decrement(_front);
			if (IsFull)
			{
				_buffer[_front] = item;
				return;
			}
			_buffer[_front] = item;
			_count++;
		}

		public T PopBack()
		{
			ThrowIfEmpty("Cannot take items from an empty buffer.");
			int num = Decrement(BackIndex());
			T result = _buffer[num];
			_buffer[num] = default(T);
			_count--;
			return result;
		}

		public T PopFront()
		{
			ThrowIfEmpty("Cannot take items from an empty buffer.");
			T result = _buffer[_front];
			_buffer[_front] = default(T);
			_front = Increment(_front);
			_count--;
			return result;
		}

		public void Clear()
		{
			_front = 0;
			_count = 0;
			Array.Clear(_buffer, 0, _buffer.Length);
		}

		public T[] ToArray()
		{
			T[] array = new T[Count];
			ReadOnlySpan<T> readOnlySpan = SpanOne();
			ReadOnlySpan<T> readOnlySpan2 = SpanTwo();
			readOnlySpan.CopyTo(array);
			readOnlySpan2.CopyTo(array.AsSpan(readOnlySpan.Length));
			return array;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private int FrontIndex()
		{
			return _front;
		}

		private int BackIndex()
		{
			return (_front + _count) % Capacity;
		}

		private int InternalIndex(int index)
		{
			return (_front + index) % Capacity;
		}

		private int Increment(int index)
		{
			Assert.Check(index >= 0 && index < Capacity, "index >= 0 && index < Capacity");
			return (index != Capacity - 1) ? (index + 1) : 0;
		}

		private int Decrement(int index)
		{
			Assert.Check(index >= 0 && index < Capacity, "index >= 0 && index < Capacity");
			return (index == 0) ? (Capacity - 1) : (index - 1);
		}

		private void ThrowIfEmpty(string message = "Cannot access an empty buffer.")
		{
			if (IsEmpty)
			{
				throw new InvalidOperationException(message);
			}
		}

		private ReadOnlySpan<T> SpanOne()
		{
			int num = BackIndex();
			if (IsEmpty)
			{
				return default(ReadOnlySpan<T>);
			}
			if (_front < num)
			{
				return new ReadOnlySpan<T>(_buffer, _front, num - _front);
			}
			return new ReadOnlySpan<T>(_buffer, _front, _buffer.Length - _front);
		}

		private ReadOnlySpan<T> SpanTwo()
		{
			int num = BackIndex();
			if (IsEmpty)
			{
				return default(ReadOnlySpan<T>);
			}
			if (_front < num)
			{
				return new ReadOnlySpan<T>(_buffer, num, 0);
			}
			return new ReadOnlySpan<T>(_buffer, 0, num);
		}
	}
}
