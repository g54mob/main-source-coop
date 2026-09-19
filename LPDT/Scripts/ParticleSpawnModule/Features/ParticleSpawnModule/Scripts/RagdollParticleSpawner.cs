using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.ParticleSpawnModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class RagdollParticleSpawner : NetworkBehaviour
	{
		[SerializeField]
		private ParticleSystem _particleSystemPrefab;

		[SerializeField]
		private EventReference _destroySound;

		[SerializeField]
		private RagdollEntity _ragdollEntity;

		private IAudioService _audioService;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (!(_ragdollEntity == null) && !(_ragdollEntity.RootPhysData.RigidBody == null))
			{
				Vector3 position = (_ragdollEntity.IsSimulated ? _ragdollEntity.RootPhysData.RigidBody.position : _ragdollEntity.transform.forward);
				UnityEngine.Object.Instantiate(_particleSystemPrefab.gameObject, position, Quaternion.identity);
				EventInstance soundInstance = _audioService.CreateInstance(_destroySound);
				_audioService.StartInstanceWith3DAttributes(soundInstance, new GenericTransformBasedSoundSource(_ragdollEntity.RootPhysData.RigidBody.transform));
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
