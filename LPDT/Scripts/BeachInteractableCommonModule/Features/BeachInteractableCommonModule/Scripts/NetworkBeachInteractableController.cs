using Fusion;
using NetworkServices.ObjectsProvider;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class NetworkBeachInteractableController : IBeachInteractableController
	{
		private readonly StagingBeachInteractableModel _stagingBeachInteractableModel;

		private NetworkObject _networkObject;

		public BeachInteractableIdentifier BeachInteractableIdentifier { get; private set; }

		public NetworkBeachInteractableController(StagingBeachInteractableModel stagingBeachInteractableModel)
		{
			_stagingBeachInteractableModel = stagingBeachInteractableModel;
		}

		public void Initialize(NetworkObject networkObject, BeachInteractableIdentifier beachInteractableIdentifier)
		{
			_networkObject = networkObject;
			BeachInteractableIdentifier = beachInteractableIdentifier;
			_stagingBeachInteractableModel.TryPopStagingBeachInteractable(beachInteractableIdentifier, out var _);
		}

		public void Initialize(BeachInteractableIdentifier beachInteractableIdentifier)
		{
			BeachInteractableIdentifier = beachInteractableIdentifier;
			if (_stagingBeachInteractableModel.TryPopStagingBeachInteractable(BeachInteractableIdentifier, out var networkObject))
			{
				ReplicateNetworkObject(BeachInteractableIdentifier, networkObject);
			}
			else
			{
				_stagingBeachInteractableModel.OnNetworkBeachInteractableEnqueued += ReplicateNetworkObject;
			}
		}

		public void DespawnInteractable()
		{
			_stagingBeachInteractableModel.OnNetworkBeachInteractableEnqueued -= ReplicateNetworkObject;
			if (!(_networkObject == null) && _networkObject.HasStateAuthority)
			{
				_networkObject.DespawnHierarchy();
			}
		}

		private void ReplicateNetworkObject(BeachInteractableIdentifier beachInteractableIdentifier, NetworkObject networkObject)
		{
			if (BeachInteractableIdentifier.Equals(beachInteractableIdentifier))
			{
				_networkObject = networkObject;
			}
		}
	}
}
