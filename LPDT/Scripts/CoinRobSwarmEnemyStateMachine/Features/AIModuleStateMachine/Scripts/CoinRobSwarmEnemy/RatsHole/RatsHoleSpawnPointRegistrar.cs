using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	[NetworkBehaviourWeaved(0)]
	public class RatsHoleSpawnPointRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private Transform _spawnPoint;

		[Header("Editor preview")]
		[SerializeField]
		private GameObject _previewPrefab;

		[SerializeField]
		private bool _showPreview = true;

		private RatsHoleSpawnPointsModel _ratsHoleSpawnPointsModel;

		private RatsHoleSpawnPointData _spawnPointData;

		public Transform SpawnPoint => _spawnPoint;

		public GameObject PreviewPrefab => _previewPrefab;

		public bool ShowPreview => _showPreview;

		[Inject]
		public void InjectDependencies(RatsHoleSpawnPointsModel ratsHoleSpawnPointsModel)
		{
			_ratsHoleSpawnPointsModel = ratsHoleSpawnPointsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.gameObject.activeSelf)
			{
				Transform transform = ((_spawnPoint != null) ? _spawnPoint : base.transform);
				_spawnPointData = new RatsHoleSpawnPointData(transform.position, transform.rotation);
				_ratsHoleSpawnPointsModel.Register(_spawnPointData);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (base.gameObject.activeSelf && _spawnPointData != null)
			{
				_ratsHoleSpawnPointsModel.Unregister(_spawnPointData);
				_spawnPointData = null;
			}
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
