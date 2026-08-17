using System;

namespace FishNet.Utility.Performance
{
	public class BasicQueue<T>
	{
		private T[] Collection = new T[4];

		private T[] _resizeBuffer = new T[0];

		private int _read;

		private int _written;

		public int Capacity => Collection.Length;

		public int Count => _written;

		public int WriteIndex { get; private set; }

		public T this[int simulatedIndex]
		{
			get
			{
				int num = Capacity - _written + simulatedIndex + WriteIndex;
				if (num >= Capacity)
				{
					num -= Capacity;
				}
				return Collection[num];
			}
			set
			{
				int num = Capacity - _written + simulatedIndex + WriteIndex;
				if (num >= Capacity)
				{
					num -= Capacity;
				}
				Collection[num] = value;
			}
		}

		public void Enqueue(T data)
		{
			if (_written == Collection.Length)
			{
				Resize();
			}
			if (WriteIndex >= Collection.Length)
			{
				WriteIndex = 0;
			}
			Collection[WriteIndex] = data;
			WriteIndex++;
			_written++;
		}

		public bool TryDequeue(out T result)
		{
			if (_written == 0)
			{
				result = default(T);
				return false;
			}
			result = Dequeue();
			return true;
		}

		public T Dequeue()
		{
			if (_written == 0)
			{
				throw new Exception("Queue of type " + typeof(T).Name + " is empty.");
			}
			T result = Collection[_read];
			_written--;
			_read++;
			if (_read >= Collection.Length)
			{
				_read = 0;
			}
			return result;
		}

		public bool TryPeek(out T result)
		{
			if (_written == 0)
			{
				result = default(T);
				return false;
			}
			result = Peek();
			return true;
		}

		public T Peek()
		{
			if (_written == 0)
			{
				throw new Exception("Queue of type " + typeof(T).Name + " is empty.");
			}
			return Collection[_read];
		}

		public void Clear()
		{
			_read = 0;
			WriteIndex = 0;
			_written = 0;
			DefaultCollection(Collection);
			DefaultCollection(_resizeBuffer);
			static void DefaultCollection(T[] array)
			{
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					array[i] = default(T);
				}
			}
		}

		private void Resize()
		{
			int written = _written;
			int num = written * 2;
			int read = _read;
			T[] array = _resizeBuffer;
			if (array.Length < num)
			{
				Array.Resize(ref array, num);
			}
			int num2 = written - read;
			Array.Copy(Collection, read, array, 0, num2);
			if (read > 0)
			{
				Array.Copy(Collection, 0, array, num2, read);
			}
			Collection = array;
			_read = 0;
			WriteIndex = written;
		}
	}
}
