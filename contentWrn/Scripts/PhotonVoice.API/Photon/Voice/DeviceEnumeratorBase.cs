using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Photon.Voice
{
	public abstract class DeviceEnumeratorBase : IDeviceEnumerator, IDisposable, IEnumerable<DeviceInfo>, IEnumerable
	{
		protected List<DeviceInfo> devices = new List<DeviceInfo>();

		protected ILogger logger;

		private Action onReady;

		public virtual bool IsSupported => true;

		public virtual string Error { get; protected set; }

		public Action OnReady
		{
			protected get
			{
				return onReady;
			}
			set
			{
				onReady = value;
				if (devices != null && onReady != null)
				{
					onReady();
				}
			}
		}

		public DeviceEnumeratorBase(ILogger logger)
		{
			this.logger = logger;
		}

		public IEnumerator<DeviceInfo> GetEnumerator()
		{
			IEnumerable<DeviceInfo> enumerable2;
			if (devices != null)
			{
				IEnumerable<DeviceInfo> enumerable = devices;
				enumerable2 = enumerable;
			}
			else
			{
				enumerable2 = Enumerable.Empty<DeviceInfo>();
			}
			return enumerable2.GetEnumerator();
		}

		public abstract void Refresh();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public abstract void Dispose();
	}
}
