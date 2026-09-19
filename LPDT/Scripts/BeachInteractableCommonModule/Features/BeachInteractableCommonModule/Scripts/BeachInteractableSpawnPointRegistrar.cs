using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BeachInteractableCommonModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BeachInteractableSpawnPointRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private Transform _spawnPoint;

		[SerializeField]
		private int _marker;

		[SerializeField]
		private List<BeachInteractableType> _allowedBeachInteractables;

		private BeachInteractableSpawnModel _beachInteractableSpawnModel;

		private BeachInteractableSpawnData _beachInteractableSpawnData;

		[Inject]
		public void InjectDependencies(BeachInteractableSpawnModel beachInteractableSpawnModel)
		{
			_beachInteractableSpawnModel = beachInteractableSpawnModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_beachInteractableSpawnData = new BeachInteractableSpawnData(_spawnPoint.position, _spawnPoint.rotation, _allowedBeachInteractables, _marker);
			_beachInteractableSpawnModel.AddBeachInteractableSpawnData(_beachInteractableSpawnData);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_beachInteractableSpawnModel.RemoveBeachInteractableSpawnData(_beachInteractableSpawnData);
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
