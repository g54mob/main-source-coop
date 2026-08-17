using System;
using System.Collections.Generic;
using System.Threading;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class ThreadSafeObjectPool<T> : IObjectPool, IObjectPool<T> where T : class
	{
		private const int jpDFlwpWPErZTXBSeVqaZfLOtDar = 1;

		private const int mSqwwQithLWrHdDAWAaXEOREvmwh = 0;

		protected readonly AList<T> _pool;

		protected readonly Func<T> _createInstanceDelegate;

		protected readonly Action<T> _processOnReturnDelegate;

		private ulong hKXrQZpCixhYAAXCLJiYdimRxutF;

		private int HfFsRNwBRVCIuoveOIkSJFKQePwl;

		protected ulong InstanceCount => hKXrQZpCixhYAAXCLJiYdimRxutF;

		public ThreadSafeObjectPool(int P_0, Func<T> P_1, Action<T> P_2 = null)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("instancerDelegate");
			}
			_processOnReturnDelegate = P_2;
			_pool = ((P_0 > 0) ? new AList<T>(P_0) : new AList<T>());
			_createInstanceDelegate = P_1;
		}

		public ThreadSafeObjectPool(Func<T> P_0)
			: this(0, P_0, (Action<T>)null)
		{
		}

		public void Clear(bool reduceSize = false)
		{
			while (Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 1) != 0)
			{
				Thread.SpinWait(1);
			}
			_pool.Clear();
			if (reduceSize)
			{
				_pool.TrimExcess();
			}
			Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 0);
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
			while (Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 1) != 0)
			{
				Thread.SpinWait(1);
			}
			if (_pool._count == 0)
			{
				T result = CreateInstance();
				Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 0);
				return result;
			}
			_pool._count--;
			T val = _pool._items[_pool._count];
			_pool._items[_pool._count] = null;
			if (val is IPoolableObject_Internal)
			{
				(val as IPoolableObject_Internal).pool = this;
			}
			Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 0);
			return val;
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
			while (Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 1) != 0)
			{
				Thread.SpinWait(1);
			}
			if (_pool._count < _pool._items.Length)
			{
				_pool._items[_pool._count] = item;
				_pool._count++;
			}
			else
			{
				_pool.Add(item);
			}
			Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 0);
			return true;
		}

		bool IObjectPool<T>.Return(T item)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Return
			return this.Return(item);
		}

		public bool Return(IList<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			int count = items.Count;
			bool result = false;
			Action<T> processOnReturnDelegate = _processOnReturnDelegate;
			while (Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 1) != 0)
			{
				Thread.SpinWait(1);
			}
			for (int i = 0; i < count; i++)
			{
				T val = items[i];
				if (val != null)
				{
					processOnReturnDelegate?.Invoke(val);
					if (val is IPoolableObject_Internal)
					{
						(val as IPoolableObject_Internal).Clear();
					}
					if (_pool._count < _pool._items.Length)
					{
						_pool._items[_pool._count] = val;
						_pool._count++;
					}
					else
					{
						_pool.Add(val);
					}
				}
			}
			Interlocked.Exchange(ref HfFsRNwBRVCIuoveOIkSJFKQePwl, 0);
			items.Clear();
			return result;
		}

		private object spEJxZauXXOFqkMDPKtNSVMVnSCE()
		{
			return Get();
		}

		object IObjectPool.Get()
		{
			//ILSpy generated this explicit interface implementation from .override directive in spEJxZauXXOFqkMDPKtNSVMVnSCE
			return this.spEJxZauXXOFqkMDPKtNSVMVnSCE();
		}

		private bool XCczeSCVPuJDeeCgxqtnSQdsJJUi(object P_0)
		{
			return Return(P_0 as T);
		}

		bool IObjectPool.Return(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in XCczeSCVPuJDeeCgxqtnSQdsJJUi
			return this.XCczeSCVPuJDeeCgxqtnSQdsJJUi(P_0);
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
			hKXrQZpCixhYAAXCLJiYdimRxutF = ((hKXrQZpCixhYAAXCLJiYdimRxutF < 18446744073709551615uL) ? (hKXrQZpCixhYAAXCLJiYdimRxutF + 1) : 0);
			return hKXrQZpCixhYAAXCLJiYdimRxutF;
		}
	}
}
