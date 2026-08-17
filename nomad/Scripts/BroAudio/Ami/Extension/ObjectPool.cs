using System.Collections.Generic;

namespace Ami.Extension
{
	public abstract class ObjectPool<T> where T : class
	{
		protected readonly int MaxPoolSize;

		protected T BaseObject;

		protected List<T> Pool = new List<T>();

		protected abstract T CreateObject();

		protected abstract void DestroyObject(T instance);

		protected ObjectPool(T baseObject, int maxInternalPoolSize)
		{
			BaseObject = baseObject;
			MaxPoolSize = maxInternalPoolSize;
		}

		public virtual void Init(int initialCount)
		{
			for (int i = 0; i < initialCount; i++)
			{
				T item = CreateObject();
				Pool.Add(item);
			}
		}

		public virtual T Extract()
		{
			T val = null;
			if (Pool.Count == 0)
			{
				val = CreateObject();
			}
			else
			{
				int index = Pool.Count - 1;
				val = Pool[index];
				Pool.RemoveAt(index);
			}
			return val;
		}

		public virtual void Recycle(T obj)
		{
			if (Pool.Count == MaxPoolSize)
			{
				DestroyObject(obj);
			}
			else
			{
				Pool.Add(obj);
			}
		}
	}
}
