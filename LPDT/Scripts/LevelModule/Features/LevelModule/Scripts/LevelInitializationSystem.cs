using System;
using Cysharp.Threading.Tasks;
using Features.AIModule.Data;
using Features.LevelModule.Scripts.RoomVariations;
using Features.MultiplayerSessionServices.Scripts;
using Unity.AI.Navigation;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	public class LevelInitializationSystem : ILevelNavigationRebake
	{
		private readonly SpawnedRoomsModel _spawnedRoomsModel;

		private readonly NavigationModel _navigationModel;

		public LevelInitializationSystem(SpawnedRoomsModel spawnedRoomsModel, NavigationModel navigationModel)
		{
			_spawnedRoomsModel = spawnedRoomsModel;
			_navigationModel = navigationModel;
		}

		public async UniTask RebakeForCurrentLevelAsync()
		{
			await UniTask.WaitUntil(() => _spawnedRoomsModel.IsAllTasksCompleted).TimeoutWithoutException(TimeSpan.FromSeconds(12.0));
			if (!_spawnedRoomsModel.IsAllTasksCompleted)
			{
				Debug.LogWarning("[LevelInit] room-spawn readiness timed out (stale SpawnedRoomsModel counter, e.g. after a host migration) — baking NavMesh anyway to avoid stranding Level Enter.");
			}
			RebakeNavMesh();
		}

		private void RebakeNavMesh()
		{
			foreach (NavMeshSurface activeSurface in _navigationModel.ActiveSurfaces)
			{
				activeSurface.BuildNavMesh();
			}
			RefreshNavMeshLinks();
		}

		private void RefreshNavMeshLinks()
		{
			NavMeshLink[] array = UnityEngine.Object.FindObjectsByType<NavMeshLink>(FindObjectsSortMode.None);
			foreach (NavMeshLink navMeshLink in array)
			{
				if (!(navMeshLink == null) && navMeshLink.isActiveAndEnabled)
				{
					navMeshLink.UpdateLink();
				}
			}
		}
	}
}
