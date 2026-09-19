using System;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.ParticleSpawnModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class ParticleSpawner : NetworkBehaviour
	{
		[SerializeField]
		private ParticleSystem _particleSystemPrefab;

		[SerializeField]
		private EventReference _destroySound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private IAudioService _audioService;

		[WeaverGenerated]
		[DefaultForProperty("IsDespawnFeedbackSuppressed", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsDespawnFeedbackSuppressed;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsDespawnFeedbackSuppressed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParticleSpawner.IsDespawnFeedbackSuppressed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ParticleSpawner.IsDespawnFeedbackSuppressed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		public void SetDespawnEffectsSuppressed(bool suppressed)
		{
			if (base.HasStateAuthority)
			{
				IsDespawnFeedbackSuppressed = suppressed;
			}
		}

		public void SpawnWorldFeedback(Vector3 worldPosition, Quaternion worldRotation, int soundSourceId)
		{
			if (!(_particleSystemPrefab == null))
			{
				UnityEngine.Object.Instantiate(_particleSystemPrefab.gameObject, worldPosition, worldRotation);
				if (!_destroySound.IsNull)
				{
					_audioService.PlayOneShot(_destroySound, new GenericSoundSource(worldPosition, soundSourceId));
				}
			}
		}

		public void SpawnWorldFeedback(Vector3 worldPosition, int soundSourceId)
		{
			SpawnWorldFeedback(worldPosition, Quaternion.identity, soundSourceId);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if ((hasState && IsDespawnFeedbackSuppressed) || _particleSystemPrefab == null)
			{
				return;
			}
			UnityEngine.Object.Instantiate(_particleSystemPrefab.gameObject, base.transform.position, Quaternion.identity);
			if (!_destroySound.IsNull)
			{
				if (_soundSourceBehaviour != null)
				{
					EventInstance soundInstance = _audioService.CreateInstance(_destroySound);
					_audioService.StartInstanceWith3DAttributes(soundInstance, _soundSourceBehaviour);
				}
				else
				{
					_audioService.PlayOneShot(_destroySound, new GenericSoundSource(base.transform.position, base.transform.GetInstanceID()));
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsDespawnFeedbackSuppressed = _IsDespawnFeedbackSuppressed;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsDespawnFeedbackSuppressed = IsDespawnFeedbackSuppressed;
		}
	}
}
