using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	public class RatsHoleSpawnService : IRatsHoleSpawnService
	{
		private const float ExistingHoleSpawnPointRadius = 0.25f;

		private readonly RatsHoleSpawnPointsModel _spawnPointsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private readonly RatsHoleRegistry _ratsHoleRegistry;

		public RatsHoleSpawnService(RatsHoleSpawnPointsModel spawnPointsModel, MultiplayerModel multiplayerModel, CoinRobSwarmEnemySettings swarmSettings, RatsHoleRegistry ratsHoleRegistry)
		{
			_spawnPointsModel = spawnPointsModel;
			_multiplayerModel = multiplayerModel;
			_swarmSettings = swarmSettings;
			_ratsHoleRegistry = ratsHoleRegistry;
		}

		public void EnsureAllSpawned()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			RegisterExistingRunnerHoles(networkRunner);
			if (_spawnPointsModel.AreSpawned || _spawnPointsModel.SpawnPoints.Count == 0 || !_swarmSettings.RatsHolePrefab.IsValid)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < _spawnPointsModel.SpawnPoints.Count; i++)
			{
				RatsHoleSpawnPointData ratsHoleSpawnPointData = _spawnPointsModel.SpawnPoints[i];
				Vector3 position = ratsHoleSpawnPointData.Position;
				Quaternion rotation = ratsHoleSpawnPointData.Rotation;
				if (!_ratsHoleRegistry.HasReadyHoleNearSpawnPosition(position, 0.25f) && networkRunner.Spawn(_swarmSettings.RatsHolePrefab, position, rotation, null, delegate(NetworkRunner networkRunner2, NetworkObject spawnedObject)
				{
					spawnedObject.transform.SetPositionAndRotation(position, rotation);
				}) == null)
				{
					flag = true;
				}
			}
			_spawnPointsModel.AreSpawned = !flag;
		}

		private void RegisterExistingRunnerHoles(NetworkRunner runner)
		{
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && allNetworkObject.TryGetComponent<RatsHole>(out var component))
				{
					_ratsHoleRegistry.Register(component);
				}
			}
		}
	}
}
