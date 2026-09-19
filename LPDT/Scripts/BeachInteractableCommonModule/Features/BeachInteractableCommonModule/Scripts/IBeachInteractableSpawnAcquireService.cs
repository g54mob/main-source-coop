using System.Collections.Generic;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public interface IBeachInteractableSpawnAcquireService
	{
		List<BeachInteractableSpawnData> GetAllowedSpawnPoints(BeachInteractableType type);
	}
}
