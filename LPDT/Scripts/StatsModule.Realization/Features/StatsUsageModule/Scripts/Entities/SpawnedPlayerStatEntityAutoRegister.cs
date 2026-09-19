using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StatsUsageModule.Scripts.Entities
{
	public class SpawnedPlayerStatEntityAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		private EntityStatEntityNetworkedBase _entityStatEntityNetworked;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private EntityStatEntityNetworkedBase _cashedEntityStats;

		private PlayerRef _cashedPlayerRef;

		[Inject]
		public void InjectDependencies(SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		private void Start()
		{
			_cashedPlayerRef = _networkObject.InputAuthority;
			_spawnedEntityStatsModel.RegisterPlayerStats(_cashedPlayerRef.PlayerId, _entityStatEntityNetworked);
		}

		private void OnDestroy()
		{
			_spawnedEntityStatsModel.UnregisterPlayerStats(_cashedPlayerRef.PlayerId, _entityStatEntityNetworked);
		}
	}
}
