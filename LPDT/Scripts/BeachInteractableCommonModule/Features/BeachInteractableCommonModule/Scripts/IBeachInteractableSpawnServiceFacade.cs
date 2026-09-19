using Features.LevelModule.Scripts;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public interface IBeachInteractableSpawnServiceFacade
	{
		IBeachInteractableController SpawnBeachInteractable(BeachInteractableType type, BeachInteractableLocationType beachInteractableLocationType, BeachInteractableSpawnRules spawnRules, LevelType levelType);

		IBeachInteractableController SpawnBeachInteractable(BeachInteractableType type, BeachInteractableLocationType beachInteractableLocationType, int spawnPointMarker, LevelType levelType);
	}
}
