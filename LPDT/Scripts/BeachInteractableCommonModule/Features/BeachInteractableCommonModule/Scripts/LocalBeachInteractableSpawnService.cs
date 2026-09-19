using UnityEngine;
using Zenject;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class LocalBeachInteractableSpawnService : IBeachInteractableSpawnService
	{
		private readonly DiContainer _diContainer;

		private readonly BeachInteractableSpawnConfiguration _beachInteractableSpawnConfiguration;

		public LocalBeachInteractableSpawnService(DiContainer diContainer, BeachInteractableSpawnConfiguration beachInteractableSpawnConfiguration)
		{
			_diContainer = diContainer;
			_beachInteractableSpawnConfiguration = beachInteractableSpawnConfiguration;
		}

		public IBeachInteractableController SpawnBeachInteractable(BeachInteractableIdentifier beachInteractableIdentifier, BeachInteractableLocationType locationType, Vector3 position, Quaternion rotation)
		{
			if (!_beachInteractableSpawnConfiguration.TryGetLocalBeachInteractablePrefab(beachInteractableIdentifier.BeachInteractableType, locationType, out var interactablePrefab))
			{
				Debug.LogWarning($"[HoopSpawn] Local interactable '{beachInteractableIdentifier.BeachInteractableType}' not in LocalBeachInteractablePool — skipping local spawn.");
				return null;
			}
			GameObject beachInteractable = _diContainer.InstantiatePrefab(interactablePrefab, position, rotation, null);
			LocalBeachInteractableController localBeachInteractableController = _diContainer.Instantiate<LocalBeachInteractableController>();
			localBeachInteractableController.Initialize(beachInteractable, beachInteractableIdentifier);
			Debug.Log($"[HoopSpawn] Spawned local interactable '{beachInteractableIdentifier.BeachInteractableType}' at {position} on {beachInteractableIdentifier.LevelType}.");
			return localBeachInteractableController;
		}
	}
}
