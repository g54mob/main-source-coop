using System;

namespace Features.CustomNetworkEventsModule.Scripts
{
	public interface ICustomNetworkEvent
	{
		internal event Action<ICustomNetworkEvent> OnInternalNetworkEventSend;

		internal void SynchronizeInternal(byte[] value);

		internal byte[] GetValue();
	}
}
