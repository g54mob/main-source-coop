using UnityEngine;
using Zenject;

namespace Features.EnvironmentSpawnModule.Scripts
{
	public class EnvironmentSpawnPointAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private GameObject _prefabToSpawn;

		[SerializeField]
		private int _countToSpawn = 1;

		[SerializeField]
		private bool _useOriginalRotation;

		private EnvironmentSpawnPointsModel _environmentSpawnPointsModel;

		private EnvironmentSpawnPointData _environmentSpawnPointData;

		[Inject]
		public void InjectDependencies(EnvironmentSpawnPointsModel environmentSpawnPointsModel)
		{
			_environmentSpawnPointsModel = environmentSpawnPointsModel;
		}

		private void Awake()
		{
			if (_prefabToSpawn == null)
			{
				Debug.LogWarning("EnvironmentSpawnPointAutoRegister on " + base.gameObject.name + " has no prefab assigned!");
			}
			else
			{
				_environmentSpawnPointData = new EnvironmentSpawnPointData(base.transform.position, base.transform.rotation, _prefabToSpawn, _countToSpawn, _useOriginalRotation);
			}
		}

		private void OnEnable()
		{
			if (_environmentSpawnPointData != null)
			{
				_environmentSpawnPointsModel.RegisterPoint(_environmentSpawnPointData);
			}
		}

		private void OnDisable()
		{
			if (_environmentSpawnPointData != null)
			{
				_environmentSpawnPointsModel.UnregisterPoint(_environmentSpawnPointData);
			}
		}
	}
}
