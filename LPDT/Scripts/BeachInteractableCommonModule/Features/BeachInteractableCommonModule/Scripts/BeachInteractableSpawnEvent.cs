using System;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class BeachInteractableSpawnEvent
	{
		public event Action<BeachInteractableType> OnBeachInteractableSpawned;

		public void InvokeBeachInteractableSpawned(BeachInteractableType beachInteractableType)
		{
			this.OnBeachInteractableSpawned?.Invoke(beachInteractableType);
		}
	}
}
