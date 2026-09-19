using System;

namespace Features.StoreModule.Scripts.MovedToBeachLocal
{
	public class MovedToBeachEventClass
	{
		public event Action OnLocalPlayerMovedToBeach;

		public void InvokeLocalPlayerMovedToBeach()
		{
			this.OnLocalPlayerMovedToBeach?.Invoke();
		}
	}
}
