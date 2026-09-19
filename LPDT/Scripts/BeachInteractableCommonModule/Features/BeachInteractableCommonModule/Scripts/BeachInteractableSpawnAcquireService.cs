using System.Collections.Generic;
using System.Linq;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class BeachInteractableSpawnAcquireService : IBeachInteractableSpawnAcquireService
	{
		private readonly BeachInteractableSpawnModel _beachInteractableSpawnModel;

		public BeachInteractableSpawnAcquireService(BeachInteractableSpawnModel beachInteractableSpawnModel)
		{
			_beachInteractableSpawnModel = beachInteractableSpawnModel;
		}

		public List<BeachInteractableSpawnData> GetAllowedSpawnPoints(BeachInteractableType type)
		{
			return _beachInteractableSpawnModel.BeachInteractablesSpawnData.Where((BeachInteractableSpawnData spawnData) => spawnData.AllowedInteractables.Contains(type)).ToList();
		}
	}
}
