using UnityEngine;
using Zenject;

namespace Features.PlayerSpawner.Scripts
{
	public class PlayerSpawnPointAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private int _priority;

		private PlayerSpawnPointsModel _playerSpawnPointsModel;

		private PlayerSpawnPointData _playerSpawnPointData;

		[Inject]
		public void InjectDependencies(PlayerSpawnPointsModel playerSpawnPointsModel)
		{
			_playerSpawnPointsModel = playerSpawnPointsModel;
		}

		private void Awake()
		{
			_playerSpawnPointData = new PlayerSpawnPointData(base.transform.position, base.transform.rotation, _priority);
		}

		private void OnEnable()
		{
			_playerSpawnPointsModel.RegisterPoint(_playerSpawnPointData);
		}

		private void OnDisable()
		{
			_playerSpawnPointsModel.UnregisterPoint(_playerSpawnPointData);
		}
	}
}
