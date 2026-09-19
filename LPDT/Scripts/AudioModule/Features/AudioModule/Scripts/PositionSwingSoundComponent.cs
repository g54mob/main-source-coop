using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	public class PositionSwingSoundComponent : MonoBehaviour
	{
		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[Header("FMOD")]
		[SerializeField]
		private EventReference _swingEvent;

		[Header("Detection Settings")]
		[SerializeField]
		private float _checkInterval = 0.15f;

		[SerializeField]
		private float _distanceThreshold = 0.2f;

		[SerializeField]
		private float _soundCooldown = 1.5f;

		private EventInstance _instance;

		private Queue<(Vector3 pos, float time)> _history = new Queue<(Vector3, float)>();

		private float _cooldownEndTime;

		private bool _playedThisSwing;

		private IAudioService _audioService;

		[Inject]
		private void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		private void Start()
		{
			_instance = _audioService.CreateInstance(_swingEvent);
		}

		private void Update()
		{
			Update3D();
			float time = Time.time;
			Vector3 position = base.transform.position;
			_history.Enqueue((position, time));
			while (_history.Count > 0 && time - _history.Peek().time > _checkInterval)
			{
				_history.Dequeue();
			}
			if (_history.Count == 0)
			{
				return;
			}
			Vector3 item = _history.Peek().pos;
			if (Vector3.Distance(position, item) > _distanceThreshold && !_playedThisSwing && time >= _cooldownEndTime)
			{
				_audioService.StartInstanceWith3DAttributes(_instance, _soundSourceBehaviour);
				_playedThisSwing = true;
			}
			if (_playedThisSwing)
			{
				_instance.getPlaybackState(out var state);
				if (state == PLAYBACK_STATE.STOPPED)
				{
					_playedThisSwing = false;
					_cooldownEndTime = time + _soundCooldown;
				}
			}
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
