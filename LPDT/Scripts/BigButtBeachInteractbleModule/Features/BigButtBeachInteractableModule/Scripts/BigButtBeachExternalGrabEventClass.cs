using System;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachExternalGrabEventClass
	{
		public event Action OnExternalGrab;

		internal void InvokeExternalGrab()
		{
			this.OnExternalGrab?.Invoke();
		}
	}
}
