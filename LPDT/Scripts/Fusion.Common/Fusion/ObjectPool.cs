#define DEBUG
using System.Collections.Generic;

namespace Fusion
{
	internal class ObjectPool<T> where T : class, new()
	{
		private readonly Stack<T> _pool;

		private readonly List<T> _all;

		public int CountInactive => _pool.Count;

		public int CountActive => _all.Count - _pool.Count;

		public int CountAll => _all.Count;

		public ObjectPool(int initialCapacity = 16)
		{
			_pool = new Stack<T>(initialCapacity);
			_all = new List<T>(initialCapacity);
			for (int i = 0; i < initialCapacity; i++)
			{
				T item = new T();
				_all.Add(item);
				_pool.Push(item);
			}
		}

		public T Get()
		{
			if (_pool.Count > 0)
			{
				return _pool.Pop();
			}
			T val = new T();
			_all.Add(val);
			return val;
		}

		public void Return(T item)
		{
			if (item != null)
			{
				Assert.Check(_all.Contains(item), "_all.Contains(item)");
				_pool.Push(item);
				Assert.Check(_pool.Count <= _all.Count, "_pool.Count <= _all.Count");
			}
		}
	}
}
