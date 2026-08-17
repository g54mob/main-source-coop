using System;
using System.Collections.Generic;
using Rewired.Interfaces;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class InputSourceWrapper<T> : IDisposable, IInputSource
	{
		private T eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public T source => eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

		public event Action DeviceChangedEvent
		{
			add
			{
				throw new NotImplementedException();
			}
			remove
			{
				throw new NotImplementedException();
			}
		}

		public InputSourceWrapper(T P_0)
		{
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA = P_0;
		}

		public void SystemDeviceConnected()
		{
		}

		public void SystemDeviceDisconnected()
		{
		}

		public void Update()
		{
			throw new NotImplementedException();
		}

		public void UpdateDevices(UpdateLoopType updateLoop)
		{
			throw new NotImplementedException();
		}

		public void UpdateFinished()
		{
			throw new NotImplementedException();
		}

		public IList<TJoy> GetJoysticks<TJoy>() where TJoy : class
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~InputSourceWrapper()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
