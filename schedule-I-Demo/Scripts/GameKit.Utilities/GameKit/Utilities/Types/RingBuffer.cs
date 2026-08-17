using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameKit.Utilities.Types
{
	public class RingBuffer<T>
	{
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private RingBuffer<T> _rollingCollection;

			private readonly T[] _collection;

			private int _read;

			private int _startIndex;

			public T Current { get; private set; }

			public int ActualIndex
			{
				get
				{
					int num = _startIndex + (_read - 1);
					int capacity = _rollingCollection.Capacity;
					if (num >= capacity)
					{
						num -= capacity;
					}
					return num;
				}
			}

			public int SimulatedIndex => _read - 1;

			object IEnumerator.Current => Current;

			public Enumerator(RingBuffer<T> c)
			{
				_read = 0;
				_startIndex = 0;
				_rollingCollection = c;
				_collection = c.Collection;
				Current = default(T);
			}

			public bool MoveNext()
			{
				int count = _rollingCollection.Count;
				if (_read >= count)
				{
					ResetRead();
					return false;
				}
				int num = _startIndex + _read;
				int capacity = _rollingCollection.Capacity;
				if (num >= capacity)
				{
					num -= capacity;
				}
				Current = _collection[num];
				_read++;
				return true;
			}

			public void SetStartIndex(int index)
			{
				_startIndex = index;
				ResetRead();
			}

			public void AddStartIndex(int value)
			{
				_startIndex += value;
				int capacity = _rollingCollection.Capacity;
				if (_startIndex > capacity)
				{
					_startIndex -= capacity;
				}
				else if (_startIndex < 0)
				{
					_startIndex += capacity;
				}
				ResetRead();
			}

			public void ResetRead()
			{
				_read = 0;
			}

			public void Reset()
			{
				_startIndex = 0;
				ResetRead();
			}

			public void Dispose()
			{
			}
		}

		public T[] Collection = new T[0];

		private int _written;

		private Enumerator _enumerator;

		public int WriteIndex { get; private set; }

		public int Count => _written;

		public int Capacity => Collection.Length;

		public bool Initialized { get; private set; }

		public T this[int simulatedIndex]
		{
			get
			{
				int realIndex = GetRealIndex(simulatedIndex);
				return Collection[realIndex];
			}
			set
			{
				int realIndex = GetRealIndex(simulatedIndex);
				Collection[realIndex] = value;
			}
		}

		public void Initialize(int capacity)
		{
			if (capacity <= 0)
			{
				Debug.LogError("Collection length must be larger than 0.");
				return;
			}
			Collection = new T[capacity];
			_enumerator = new Enumerator(this);
			Initialized = true;
		}

		public void Clear()
		{
			for (int i = 0; i < Collection.Length; i++)
			{
				Collection[i] = default(T);
			}
			Reset();
		}

		public void Reset()
		{
			_written = 0;
			WriteIndex = 0;
			_enumerator.Reset();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Add(T data)
		{
			if (!IsInitializedWithError())
			{
				return default(T);
			}
			T result = Collection[WriteIndex];
			Collection[WriteIndex] = data;
			IncreaseWritten();
			return result;
		}

		private void IncreaseWritten()
		{
			int capacity = Capacity;
			WriteIndex++;
			_written++;
			if (WriteIndex >= capacity)
			{
				WriteIndex = 0;
			}
			if (_written > capacity)
			{
				_written = capacity;
				_enumerator.SetStartIndex(WriteIndex);
			}
		}

		private int GetRealIndex(int simulatedIndex)
		{
			int num = Capacity - _written + simulatedIndex + WriteIndex;
			if (num >= Capacity)
			{
				num -= Capacity;
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Enumerator GetEnumerator()
		{
			if (!IsInitializedWithError())
			{
				return default(Enumerator);
			}
			_enumerator.ResetRead();
			return _enumerator;
		}

		public void RemoveRange(bool fromStart, int length)
		{
			if (length == 0)
			{
				return;
			}
			if (length < 0)
			{
				Debug.LogError("Negative values cannot be removed.");
				return;
			}
			if (length >= _written)
			{
				Reset();
				return;
			}
			_written -= length;
			if (fromStart)
			{
				_enumerator.AddStartIndex(length);
				return;
			}
			WriteIndex -= length;
			if (WriteIndex < 0)
			{
				WriteIndex += Capacity;
			}
		}

		private bool IsInitializedWithError()
		{
			if (!Initialized)
			{
				Debug.LogError("RingBuffer has not yet been initialized.");
				return false;
			}
			return true;
		}
	}
}
