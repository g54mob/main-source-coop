using System;

namespace Features.DisconnectHandlerModule.Scripts.Data
{
	public class DisconnectRequestEventClass
	{
		public event Action<DisconnectRequestReason> OnRequestDisconnect;

		public void Publish(DisconnectRequestReason reason)
		{
			this.OnRequestDisconnect?.Invoke(reason);
		}
	}
}
