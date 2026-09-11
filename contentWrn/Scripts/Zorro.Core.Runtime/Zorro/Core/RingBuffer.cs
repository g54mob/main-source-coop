using System.Threading;

namespace Zorro.Core
{
	public class RingBuffer<T>
	{
		private readonly T[] _entries;

		private readonly int _modMask;

		private Volatile.PaddedLong _consumerCursor;

		private Volatile.PaddedLong _producerCursor;

		public int Capacity => _entries.Length;

		public T this[long index]
		{
			get
			{
				return _entries[index & _modMask];
			}
			set
			{
				_entries[index & _modMask] = value;
			}
		}

		public int Count => (int)(_producerCursor.ReadFullFence() - _consumerCursor.ReadFullFence());

		public RingBuffer(int capacity)
		{
			capacity = NextPowerOfTwo(capacity);
			_modMask = capacity - 1;
			_entries = new T[capacity];
		}

		public T Dequeue()
		{
			long num = _consumerCursor.ReadAcquireFence() + 1;
			while (_producerCursor.ReadAcquireFence() < num)
			{
				Thread.SpinWait(1);
			}
			T result = this[num];
			_consumerCursor.WriteReleaseFence(num);
			return result;
		}

		public bool TryDequeue(out T obj)
		{
			long num = _consumerCursor.ReadAcquireFence() + 1;
			if (_producerCursor.ReadAcquireFence() < num)
			{
				obj = default(T);
				return false;
			}
			obj = Dequeue();
			return true;
		}

		public void Enqueue(T item)
		{
			long num = _producerCursor.ReadAcquireFence() + 1;
			long num2 = num - _entries.Length;
			long num3 = _consumerCursor.ReadAcquireFence();
			while (num2 > num3)
			{
				num3 = _consumerCursor.ReadAcquireFence();
				Thread.SpinWait(1);
			}
			this[num] = item;
			_producerCursor.WriteReleaseFence(num);
		}

		private static int NextPowerOfTwo(int x)
		{
			int num;
			for (num = 2; num < x; num <<= 1)
			{
			}
			return num;
		}
	}
}
