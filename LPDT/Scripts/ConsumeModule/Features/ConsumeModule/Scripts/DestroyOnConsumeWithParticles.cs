using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.ConsumeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class DestroyOnConsumeWithParticles : NetworkBehaviour, IDestroyOnConsume
	{
		[SerializeField]
		private ParticleSystem _particlePrefab;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		public void Inject(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public async void Destroy()
		{
			(await _multiplayerModel.NetworkRunner.SpawnAsync(_particlePrefab.gameObject, base.transform.position, Quaternion.identity, _multiplayerModel.NetworkRunner.LocalPlayer)).GetComponent<ParticleSystem>().Play();
			base.Object.DespawnHierarchy();
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
