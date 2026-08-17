using System;
using Epic.OnlineServices;

namespace PlayEveryWare.EpicOnlineServices
{
	public abstract class FileRequestTransferWrapper<T> : IFileTransferRequest, IDisposable where T : class
	{
		protected T _instance;

		private bool _disposed;

		protected FileRequestTransferWrapper(T instance)
		{
			_instance = instance;
		}

		~FileRequestTransferWrapper()
		{
			Dispose();
		}

		public static bool operator ==(FileRequestTransferWrapper<T> wrapper, object obj)
		{
			if ((object)wrapper == null || wrapper._instance == null)
			{
				return obj == null;
			}
			return wrapper._instance.Equals(obj);
		}

		public static bool operator !=(FileRequestTransferWrapper<T> wrapper, object obj)
		{
			return !(wrapper == obj);
		}

		public override bool Equals(object obj)
		{
			if ((object)this == obj)
			{
				return true;
			}
			if (_instance == obj)
			{
				return true;
			}
			if (obj is FileRequestTransferWrapper<T> fileRequestTransferWrapper)
			{
				return object.Equals(_instance, fileRequestTransferWrapper._instance);
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (_instance == null)
			{
				return 0;
			}
			return _instance.GetHashCode();
		}

		public abstract Result CancelRequest();

		public abstract void Release();

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Release();
					_instance = null;
				}
				_disposed = true;
			}
		}
	}
}
