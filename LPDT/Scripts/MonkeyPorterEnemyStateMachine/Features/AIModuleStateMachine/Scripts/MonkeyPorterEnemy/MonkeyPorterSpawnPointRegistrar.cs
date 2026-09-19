using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class MonkeyPorterSpawnPointRegistrar : MonoBehaviour
	{
		private MonkeyPorterSpawnPointsModel _spawnPointsModel;

		private MonkeyPorterSpawnPointData _spawnPointData;

		[Inject]
		public void InjectDependencies(MonkeyPorterSpawnPointsModel spawnPointsModel)
		{
			_spawnPointsModel = spawnPointsModel;
		}

		private void Awake()
		{
			_spawnPointData = new MonkeyPorterSpawnPointData(base.transform);
		}

		private void OnEnable()
		{
			_spawnPointsModel.RegisterPoint(_spawnPointData);
		}

		private void OnDisable()
		{
			_spawnPointsModel.UnregisterPoint(_spawnPointData);
		}
	}
}
