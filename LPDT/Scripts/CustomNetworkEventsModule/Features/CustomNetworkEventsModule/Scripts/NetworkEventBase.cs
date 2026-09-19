using System;
using System.Text;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces;
using Zenject;

namespace Features.CustomNetworkEventsModule.Scripts
{
	public abstract class NetworkEventBase<TInfo> : ICustomNetworkEvent
	{
		private IJsonConvertor _jsonConvertor;

		public event Action<TInfo> OnNetworkEventSend;

		private event Action<ICustomNetworkEvent> InternalNetworkEvent;

		event Action<ICustomNetworkEvent> ICustomNetworkEvent.OnInternalNetworkEventSend
		{
			add
			{
				InternalNetworkEvent += value;
			}
			remove
			{
				InternalNetworkEvent -= value;
			}
		}

		[Inject]
		private void InjectDependencies(IJsonConvertor jsonConvertor)
		{
			_jsonConvertor = jsonConvertor;
		}

		void ICustomNetworkEvent.SynchronizeInternal(byte[] value)
		{
			TInfo obj = _jsonConvertor.Convert<TInfo>(Encoding.UTF8.GetString(value));
			this.OnNetworkEventSend?.Invoke(obj);
		}

		byte[] ICustomNetworkEvent.GetValue()
		{
			string s = _jsonConvertor.Convert(this);
			return Encoding.UTF8.GetBytes(s);
		}

		protected void Send()
		{
			this.InternalNetworkEvent?.Invoke(this);
		}
	}
}
