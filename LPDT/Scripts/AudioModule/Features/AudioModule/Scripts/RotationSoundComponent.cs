using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	public class RotationSoundComponent : MonoBehaviour
	{
		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private EventReference _hingeEvent;

		[SerializeField]
		private float _angleThreshold = 1f;

		[SerializeField]
		private float _soundCooldown = 3f;

		private EventInstance _instance;

		private Quaternion _lastRotation;

		private bool _playedThisMovement;

		private float _cooldownEndTime;

		private IAudioService _audioService;

		[Inject]
		private void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		private void Start()
		{
			_instance = _audioService.CreateInstance(_hingeEvent);
			_lastRotation = base.transform.localRotation;
		}

		private void Update()
		{
			if (_soundSourceBehaviour == null || _soundSourceBehaviour.SoundSourceTransform == null)
			{
				_audioService.StopInstance(_instance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				base.enabled = false;
				return;
			}
			if (Quaternion.Angle(_lastRotation, base.transform.localRotation) > _angleThreshold && !_playedThisMovement && Time.time >= _cooldownEndTime)
			{
				_audioService.StartInstanceWith3DAttributes(_instance, _soundSourceBehaviour);
				_playedThisMovement = true;
			}
			if (_playedThisMovement)
			{
				Update3D();
				_instance.getPlaybackState(out var state);
				if (state == PLAYBACK_STATE.STOPPED)
				{
					_playedThisMovement = false;
					_cooldownEndTime = Time.time + _soundCooldown;
				}
			}
			_lastRotation = base.transform.localRotation;
		}

		private void Update3D()
		{
			_instance.set3DAttributes(_soundSourceBehaviour.SoundSourceTransform.To3DAttributes());
		}

		private void OnDestroy()
		{
			_audioService.StopInstance(_instance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(_instance);
		}
	}
}
