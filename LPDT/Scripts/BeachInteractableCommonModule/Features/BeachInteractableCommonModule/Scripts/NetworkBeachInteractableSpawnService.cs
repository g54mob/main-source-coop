using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class NetworkBeachInteractableSpawnService : IBeachInteractableSpawnService
	{
		private readonly DiContainer _diContainer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly BeachInteractableSpawnConfiguration _beachInteractableSpawnConfiguration;

		private BeachInteractableIdentifier _cachedBeachInteractableIdentifier;

		public NetworkBeachInteractableSpawnService(DiContainer diContainer, MultiplayerModel multiplayerModel, BeachInteractableSpawnConfiguration beachInteractableSpawnConfiguration)
		{
			_diContainer = diContainer;
			_multiplayerModel = multiplayerModel;
			_beachInteractableSpawnConfiguration = beachInteractableSpawnConfiguration;
		}

		public IBeachInteractableController SpawnBeachInteractable(BeachInteractableIdentifier beachInteractableIdentifier, BeachInteractableLocationType locationType, Vector3 position, Quaternion rotation)
		{
			if (!_beachInteractableSpawnConfiguration.TryGetNetworkBeachInteractablePrefab(beachInteractableIdentifier.BeachInteractableType, locationType, out var interactablePrefab))
			{
				return null;
			}
			NetworkBeachInteractableController networkBeachInteractableController = _diContainer.Instantiate<NetworkBeachInteractableController>();
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_cachedBeachInteractableIdentifier = beachInteractableIdentifier;
				NetworkObject networkObject = _multiplayerModel.NetworkRunner.Spawn(interactablePrefab, position, rotation, null, InitializeBeachInteractableStager);
				Debug.Log($"[BeachSpawn] Networked interactable '{beachInteractableIdentifier.BeachInteractableType}' spawned at {position} on {beachInteractableIdentifier.LevelType}. This is where beach-apply places the whole table; knife child-points are offset from here.");
				networkBeachInteractableController.Initialize(networkObject, beachInteractableIdentifier);
			}
			else
			{
				networkBeachInteractableController.Initialize(beachInteractableIdentifier);
			}
			return networkBeachInteractableController;
		}

		private void InitializeBeachInteractableStager(NetworkRunner runner, NetworkObject networkObject)
		{
			if (networkObject.TryGetComponent<NetworkBeachInteractableStager>(out var component))
			{
				component.BeachInteractableIdentifier = _cachedBeachInteractableIdentifier;
			}
		}
	}
}
