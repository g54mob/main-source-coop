using System;
using UnityEngine.Pool;

namespace Coffee.UIEffectInternal
{
	internal class InternalObjectPool<T> where T : class
	{
		private readonly Predicate<T> _onValid;

		private readonly ObjectPool<T> _pool;

		public InternalObjectPool(Func<T> onCreate, Predicate<T> onValid, Action<T> onReturn)
		{
			_pool = new ObjectPool<T>(onCreate, null, onReturn);
			_onValid = onValid;
		}

		public T Rent()
		{
			while (0 < _pool.CountInactive)
			{
				T val = _pool.Get();
				if (_onValid(val))
				{
					return val;
				}
			}
			return _pool.Get();
		}

		public void Return(ref T instance)
		{
			if (instance != null)
			{
				_pool.Release(instance);
				instance = null;
			}
		}
	}
}
