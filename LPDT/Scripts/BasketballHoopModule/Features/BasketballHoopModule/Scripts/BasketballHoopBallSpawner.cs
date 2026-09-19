using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BasketballHoopModule.Scripts
{
	public class BasketballHoopBallSpawner : MonoBehaviour, ILevelContentSpawnProvider
	{
		[SerializeField]
		private NetworkObject _basketballPrefab;

		[SerializeField]
		private Transform _spawnPoint;

		[SerializeField]
		private Transform _distanceTrackPoint;

		private MultiplayerModel _multiplayerModel;

		private ILevelContentSpawnRegistry _levelContentSpawnRegistry;

		private BasketballHoopSpawnLocationsModel _spawnLocationsModel;

		private Vector3 SpawnPosition => _spawnLocationsModel.SpawnPosition;

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel, ILevelContentSpawnRegistry levelContentSpawnRegistry, BasketballHoopSpawnLocationsModel spawnLocationsModel)
		{
			_multiplayerModel = multiplayerModel;
			_levelContentSpawnRegistry = levelContentSpawnRegistry;
			_spawnLocationsModel = spawnLocationsModel;
		}

		private void Awake()
		{
			_spawnLocationsModel.Configure(_spawnPoint, _distanceTrackPoint, base.transform);
		}

		private void Start()
		{
			_levelContentSpawnRegistry.Register(this);
		}

		private void OnDestroy()
		{
			_levelContentSpawnRegistry.Unregister(this);
		}

		public async UniTask SpawnLevelContentAsync()
		{
			if (_basketballPrefab == null)
			{
				return;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner.IsSharedModeMasterClient)
			{
				Transform transform = ((_spawnPoint != null) ? _spawnPoint : base.transform);
				NetworkObject networkObject = await networkRunner.SpawnAsync(_basketballPrefab, SpawnPosition, transform.rotation, networkRunner.LocalPlayer);
				if (!(networkObject == null))
				{
					networkObject.transform.SetParent(base.transform, worldPositionStays: true);
				}
			}
		}
	}
}
