using System;

namespace Photon.Voice
{
	public abstract class ObjectPool<TType, TInfo> : ObjectFactory<TType, TInfo>, IDisposable
	{
		protected int capacity;

		protected TInfo info;

		private TType[] freeObj = new TType[0];

		protected int pos;

		protected string name;

		private bool inited;

		internal string LogPrefix => "[ObjectPool] [" + name + "]";

		public TInfo Info => info;

		protected abstract TType createObject(TInfo info);

		protected abstract void destroyObject(TType obj);

		protected abstract bool infosMatch(TInfo i0, TInfo i1);

		public ObjectPool(int capacity, string name, TInfo info)
		{
			this.capacity = capacity;
			this.name = name;
			init(info);
		}

		private void init(TInfo info)
		{
			lock (this)
			{
				while (pos > 0)
				{
					destroyObject(freeObj[--pos]);
				}
				this.info = info;
				freeObj = new TType[capacity];
				inited = true;
			}
		}

		public TType New()
		{
			lock (this)
			{
				if (pos > 0)
				{
					return freeObj[--pos];
				}
				if (!inited)
				{
					throw new Exception(LogPrefix + " not initialized");
				}
			}
			return createObject(info);
		}

		public TType New(TInfo info)
		{
			if (!infosMatch(this.info, info))
			{
				init(info);
			}
			return New();
		}

		public virtual bool Free(TType obj, TInfo objInfo)
		{
			if (infosMatch(info, objInfo))
			{
				lock (this)
				{
					if (pos < freeObj.Length)
					{
						freeObj[pos++] = obj;
						return true;
					}
				}
			}
			destroyObject(obj);
			return false;
		}

		public virtual bool Free(TType obj)
		{
			lock (this)
			{
				if (pos < freeObj.Length)
				{
					freeObj[pos++] = obj;
					return true;
				}
			}
			destroyObject(obj);
			return false;
		}

		public void Dispose()
		{
			lock (this)
			{
				while (pos > 0)
				{
					destroyObject(freeObj[--pos]);
				}
				freeObj = new TType[0];
			}
		}
	}
}
