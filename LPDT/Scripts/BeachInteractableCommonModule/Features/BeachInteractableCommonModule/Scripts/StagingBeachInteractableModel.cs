using System;
using System.Collections.Generic;
using Fusion;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class StagingBeachInteractableModel
	{
		private readonly Dictionary<BeachInteractableIdentifier, NetworkObject> _stagingNetworkBeachInteractables = new Dictionary<BeachInteractableIdentifier, NetworkObject>();

		public event Action<BeachInteractableIdentifier, NetworkObject> OnNetworkBeachInteractableEnqueued;

		public void EnqueueNetworkBeachInteractable(BeachInteractableIdentifier beachInteractableIdentifier, NetworkObject networkObject)
		{
			_stagingNetworkBeachInteractables[beachInteractableIdentifier] = networkObject;
			this.OnNetworkBeachInteractableEnqueued?.Invoke(beachInteractableIdentifier, networkObject);
		}

		public bool TryPopStagingBeachInteractable(BeachInteractableIdentifier beachInteractableIdentifier, out NetworkObject networkObject)
		{
			if (!_stagingNetworkBeachInteractables.TryGetValue(beachInteractableIdentifier, out networkObject))
			{
				return false;
			}
			_stagingNetworkBeachInteractables.Remove(beachInteractableIdentifier);
			return true;
		}

		public void RemoveStagingBeachInteractableIfCurrent(BeachInteractableIdentifier beachInteractableIdentifier, NetworkObject networkObject)
		{
			if (_stagingNetworkBeachInteractables.TryGetValue(beachInteractableIdentifier, out var value) && value == networkObject)
			{
				_stagingNetworkBeachInteractables.Remove(beachInteractableIdentifier);
			}
		}
	}
}
