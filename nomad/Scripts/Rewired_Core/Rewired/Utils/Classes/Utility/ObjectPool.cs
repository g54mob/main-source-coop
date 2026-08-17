using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class ObjectPool<T> : IObjectPool, IObjectPool<T> where T : class
	{
		protected readonly Queue<T> _pool;

		protected readonly Func<T> _createInstanceDelegate;

		protected readonly Action<T> _processOnReturnDelegate;

		private ulong RSQgBTCwmgCnHXNhikZbfgLDckbs;

		protected ulong InstanceCount => RSQgBTCwmgCnHXNhikZbfgLDckbs;

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

		void IObjectPool.Clear(bool reduceSize = false)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear(reduceSize);
		}

		void IObjectPool<T>.Clear(bool reduceSize = false)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear(reduceSize);
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

		T IObjectPool<T>.Get()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Get
			return this.Get();
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

		bool IObjectPool<T>.Return(T item)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Return
			return this.Return(item);
		}

		private object lzZJtgEtOGATpjDdWZvIpRqakMni()
		{
			return Get();
		}

		object IObjectPool.Get()
		{
			//ILSpy generated this explicit interface implementation from .override directive in lzZJtgEtOGATpjDdWZvIpRqakMni
			return this.lzZJtgEtOGATpjDdWZvIpRqakMni();
		}

		private bool UyPDiIURrpEoGDSEnwxyrobLmQzE(object P_0)
		{
			return Return(P_0 as T);
		}

		bool IObjectPool.Return(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in UyPDiIURrpEoGDSEnwxyrobLmQzE
			return this.UyPDiIURrpEoGDSEnwxyrobLmQzE(P_0);
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
			RSQgBTCwmgCnHXNhikZbfgLDckbs = ((RSQgBTCwmgCnHXNhikZbfgLDckbs < 18446744073709551615uL) ? (RSQgBTCwmgCnHXNhikZbfgLDckbs + 1) : 0);
			return RSQgBTCwmgCnHXNhikZbfgLDckbs;
		}
	}
}
