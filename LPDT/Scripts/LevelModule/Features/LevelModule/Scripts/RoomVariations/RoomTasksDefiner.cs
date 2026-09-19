using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts.RoomVariations
{
	public class RoomTasksDefiner : MonoBehaviour
	{
		[SerializeField]
		private List<RoomSpawner> _roomSpawners;

		[SerializeField]
		private bool _useExplicitList = true;

		private SpawnedRoomsModel _spawnedRoomsModel;

		[Inject]
		public void InjectDependencies(SpawnedRoomsModel spawnedRoomsModel)
		{
			_spawnedRoomsModel = spawnedRoomsModel;
		}

		private void OnEnable()
		{
			if (_useExplicitList)
			{
				_spawnedRoomsModel.RoomSpawnTasksCount = _roomSpawners.Count;
			}
			else
			{
				_spawnedRoomsModel.RoomSpawnTasksCount = GetComponentsInChildren<RoomSpawner>().Length;
			}
		}

		private void OnDisable()
		{
			_spawnedRoomsModel.RoomSpawnTasksCount = null;
		}
	}
}
