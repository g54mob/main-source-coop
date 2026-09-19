using UnityEngine;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public interface IBeachInteractableSpawnService
	{
		IBeachInteractableController SpawnBeachInteractable(BeachInteractableIdentifier beachInteractableIdentifier, BeachInteractableLocationType locationType, Vector3 position, Quaternion rotation);
	}
}
