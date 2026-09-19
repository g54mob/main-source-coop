using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelObjectSpawnModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class LevelSpawnPointAutoRegister : NetworkBehaviour
	{
		[SerializeField]
		private bool _useOriginalRotation;

		[SerializeField]
		private List<LevelObjectType> _targetObjects;

		[SerializeField]
		private LevelType _testSpawnLevel;

		private LevelSpawnPointsModel _levelSpawnPointsModel;

		private LevelObjectSpawnPointData _environmentSpawnPointData;

		private ILevelObjectsSpawnService _levelObjectsSpawnService;

		[Inject]
		public void InjectDependencies(LevelSpawnPointsModel levelSpawnPointsModel, ILevelObjectsSpawnService levelObjectsSpawnService)
		{
			_levelSpawnPointsModel = levelSpawnPointsModel;
			_levelObjectsSpawnService = levelObjectsSpawnService;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
			{
				_environmentSpawnPointData = new LevelObjectSpawnPointData(base.transform.position, base.transform.rotation, _targetObjects, _useOriginalRotation);
				_levelSpawnPointsModel.RegisterSpawnPoint(_environmentSpawnPointData);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
			{
				_levelSpawnPointsModel.UnRegisterSpawnPoint(_environmentSpawnPointData);
			}
		}

		[ContextMenu("Spawn Object at Point")]
		private void SpawnObjectAtPoint()
		{
			if (!Application.isPlaying)
			{
				Debug.LogWarning("Spawn Object at Point can only be used in Play Mode");
				return;
			}
			if (_targetObjects == null || _targetObjects.Count == 0)
			{
				Debug.LogWarning("No target objects specified on " + base.gameObject.name);
				return;
			}
			if (_levelObjectsSpawnService == null)
			{
				Debug.LogWarning("LevelObjectsSpawnService is not injected on " + base.gameObject.name + ". Make sure the scene has proper Zenject setup.");
				return;
			}
			LevelObjectType objectType = _targetObjects[0];
			LevelObjectSpawnPointData spawnPoint = new LevelObjectSpawnPointData(base.transform.position, base.transform.rotation, _targetObjects, _useOriginalRotation);
			_levelObjectsSpawnService.SpawnItemAtPoint(spawnPoint, objectType, _testSpawnLevel);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
