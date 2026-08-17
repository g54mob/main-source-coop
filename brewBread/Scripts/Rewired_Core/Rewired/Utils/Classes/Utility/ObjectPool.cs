using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class ObjectPool<T> : IObjectPool<T>, IObjectPool where T : class
	{
		protected readonly Queue<T> _pool;

		protected readonly Func<T> _createInstanceDelegate;

		protected readonly Action<T> _processOnReturnDelegate;

		private ulong CpmIjziOpPELRjKVLKljMysvOoke;

		protected ulong InstanceCount => CpmIjziOpPELRjKVLKljMysvOoke;

		public ObjectPool(int P_0, Func<T> P_1, Action<T> P_2 = null)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("instancerDelegate");
			}
			_processOnReturnDelegate = P_2;
			_pool = ((P_0 > 0) ? new Queue<T>(P_0) : new Queue<T>());
			_createInstanceDelegate = P_1;
		}

		public ObjectPool(Func<T> P_0)
			: this(0, P_0, (Action<T>)null)
		{
		}

		public void Clear(bool reduceSize = false)
		{
			lock (_pool)
			{
				_pool.Clear();
				if (reduceSize)
				{
					_pool.TrimExcess();
				}
			}
		}

		public T Get()
		{
			lock (_pool)
			{
				if (_pool.Count == 0)
				{
					return CreateInstance();
				}
				T val = _pool.Dequeue();
				if (val is IPoolableObject_Internal)
				{
					(val as IPoolableObject_Internal).pool = this;
				}
				return val;
			}
		}

		public bool Return(T item)
		{
			if (item == null)
			{
				return false;
			}
			if (_processOnReturnDelegate != null)
			{
				_processOnReturnDelegate(item);
			}
			if (item is IPoolableObject_Internal)
			{
				(item as IPoolableObject_Internal).Clear();
			}
			lock (_pool)
			{
				_pool.Enqueue(item);
			}
			return true;
		}

		private object EsvQSYGIYXsjCtYxUMwMkiRHjcBX()
		{
			return Get();
		}

		object IObjectPool.Get()
		{
			//ILSpy generated this explicit interface implementation from .override directive in EsvQSYGIYXsjCtYxUMwMkiRHjcBX
			return this.EsvQSYGIYXsjCtYxUMwMkiRHjcBX();
		}

		private bool QRrYLGYgbUoxbbAAnCQVUHkPaGiW(object P_0)
		{
			return Return(P_0 as T);
		}

		bool IObjectPool.Return(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in QRrYLGYgbUoxbbAAnCQVUHkPaGiW
			return this.QRrYLGYgbUoxbbAAnCQVUHkPaGiW(P_0);
		}

		protected T CreateInstance()
		{
			T val = _createInstanceDelegate();
			if (val is IPoolableObject_Internal)
			{
				(val as IPoolableObject_Internal).pool = this;
			}
			IncrementInstanceCount();
			return val;
		}

		protected ulong IncrementInstanceCount()
		{
			CpmIjziOpPELRjKVLKljMysvOoke = ((CpmIjziOpPELRjKVLKljMysvOoke < ulong.MaxValue) ? (CpmIjziOpPELRjKVLKljMysvOoke + 1) : 0);
			return CpmIjziOpPELRjKVLKljMysvOoke;
		}
	}
}
