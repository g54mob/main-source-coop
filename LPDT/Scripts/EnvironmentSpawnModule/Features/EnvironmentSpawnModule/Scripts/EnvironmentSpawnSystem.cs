using System;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.EnvironmentSpawnModule.Scripts
{
	public class EnvironmentSpawnSystem : IInitializable, IDisposable, ILevelContentSpawnProvider
	{
		private readonly EnvironmentSpawnPointsModel _environmentSpawnPointsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ILevelContentSpawnRegistry _levelContentSpawnRegistry;

		public EnvironmentSpawnSystem(EnvironmentSpawnPointsModel environmentSpawnPointsModel, MultiplayerModel multiplayerModel, ILevelContentSpawnRegistry levelContentSpawnRegistry)
		{
			_environmentSpawnPointsModel = environmentSpawnPointsModel;
			_multiplayerModel = multiplayerModel;
			_levelContentSpawnRegistry = levelContentSpawnRegistry;
		}

		public void Initialize()
		{
			_levelContentSpawnRegistry.Register(this);
		}

		public void Dispose()
		{
			_levelContentSpawnRegistry.Unregister(this);
		}

		public async UniTask SpawnLevelContentAsync()
		{
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			foreach (EnvironmentSpawnPointData spawnPoint in _environmentSpawnPointsModel.Points)
			{
				if (!(spawnPoint.Prefab == null))
				{
					for (int i = 0; i < spawnPoint.Count; i++)
					{
						Quaternion value = ((!spawnPoint.UseOriginalRotation) ? Quaternion.Euler(spawnPoint.Rotation.eulerAngles + new Vector3(UnityEngine.Random.Range(0f, 30f), 0f, UnityEngine.Random.Range(0f, 30f))) : spawnPoint.Rotation);
						await _multiplayerModel.NetworkRunner.SpawnAsync(spawnPoint.Prefab, spawnPoint.Position + new Vector3(UnityEngine.Random.Range(0f, 1f), 0f, UnityEngine.Random.Range(0f, 1f)), value, _multiplayerModel.NetworkRunner.LocalPlayer);
					}
				}
			}
		}
	}
}
