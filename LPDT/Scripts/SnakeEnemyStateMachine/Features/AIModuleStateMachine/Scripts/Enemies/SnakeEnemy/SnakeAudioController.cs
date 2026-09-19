using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	public class SnakeAudioController : MonoBehaviour
	{
		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[Header("State sounds")]
		[SerializeField]
		private EventReference _idleLoopReference;

		[SerializeField]
		private float _idleLoopFadeOutDuration = 1.5f;

		[SerializeField]
		private EventReference _startWrapSound;

		[SerializeField]
		private EventReference _startWrapVictimSound;

		[SerializeField]
		private EventReference _wrapVictimLoopSound;

		[SerializeField]
		private float _wrapVictimLoopFadeOutDuration = 1.5f;

		[SerializeField]
		private EventReference _underTableAttackSound;

		[Header("Occlusion")]
		[SerializeField]
		private LayerMask _occlusionLayerMask;

		[SerializeField]
		private float _occlusionMaxDistance = 3f;

		[SerializeField]
		private float _verticalDistanceMultiplier = 3f;

		[SerializeField]
		private float _verticalFalloffExponent = 2f;

		[SerializeField]
		private float _lowPassMinValue = 0.35f;

		[SerializeField]
		private string _voiceOcclusionLowPassParameterName = "VoiceOcclusionLowPass";

		private SnakeEnemyContext _context;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private EventInstance _idleLoopInstance;

		private bool _idleLoopPlaying;

		private bool _idleLoopFadingOut;

		private float _idleLoopFadeElapsed;

		private SnakeVisualState _lastVisualState;

		private int _victimWrapSoundPlayedForPlayerId;

		private EventInstance _wrapVictimLoopInstance;

		private bool _wrapVictimLoopPlaying;

		private bool _wrapVictimLoopFadingOut;

		private float _wrapVictimLoopFadeElapsed;

		[Inject]
		public void InjectDependencies(SnakeEnemyContext context, IAudioService audioService, PlayerMovableModel playerMovableModel)
		{
			_context = context;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
		}

		private void Update()
		{
			if (_context == null)
			{
				return;
			}
			SnakeVisualState visualState = _context.VisualState;
			if (visualState != _lastVisualState)
			{
				if (visualState == SnakeVisualState.Wrap)
				{
					PlayOccludedOneShot(_startWrapSound);
				}
				_lastVisualState = visualState;
			}
			TryPlayVictimWrapSound();
			UpdateVictimWrapLoop();
			UpdateIdleLoop(visualState);
		}

		private void LateUpdate()
		{
			if (_idleLoopPlaying && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				_idleLoopInstance.set3DAttributes(_soundSource.SoundSourceTransform.position.To3DAttributes());
				ApplyOcclusion(_idleLoopInstance, smooth: true);
			}
		}

		public void PlayUnderTableAttack()
		{
			PlayOccludedOneShot(_underTableAttackSound);
		}

		public void StopAllImmediate()
		{
			StopIdleLoopImmediate();
			StopVictimWrapLoopImmediate();
			SnakeSegmentMovementAudio[] componentsInChildren = GetComponentsInChildren<SnakeSegmentMovementAudio>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					componentsInChildren[i].StopImmediate();
				}
			}
		}

		private void UpdateIdleLoop(SnakeVisualState state)
		{
			if (state == SnakeVisualState.Idle)
			{
				if (_idleLoopFadingOut)
				{
					_idleLoopFadingOut = false;
					_idleLoopFadeElapsed = 0f;
					if (_idleLoopPlaying && _idleLoopInstance.isValid())
					{
						_idleLoopInstance.setVolume(1f);
						return;
					}
					_idleLoopPlaying = false;
				}
				if (!_idleLoopPlaying)
				{
					StartIdleLoop();
				}
			}
			else
			{
				if (_idleLoopPlaying && !_idleLoopFadingOut)
				{
					RequestIdleLoopStop();
				}
				TickIdleLoopFadeOut();
			}
		}

		private void StartIdleLoop()
		{
			if (!_idleLoopPlaying && !_idleLoopReference.IsNull && _audioService != null && !(_soundSource == null))
			{
				_idleLoopInstance = _audioService.CreateInstance(_idleLoopReference);
				if (_idleLoopInstance.isValid())
				{
					_idleLoopInstance.setVolume(1f);
					ApplyOcclusion(_idleLoopInstance, smooth: false);
					_audioService.StartInstanceWith3DAttributes(_idleLoopInstance, _soundSource);
					_idleLoopPlaying = true;
					_idleLoopFadingOut = false;
					_idleLoopFadeElapsed = 0f;
				}
			}
		}

		private void RequestIdleLoopStop()
		{
			if (_idleLoopPlaying && !_idleLoopFadingOut)
			{
				_idleLoopFadingOut = true;
				_idleLoopFadeElapsed = 0f;
			}
		}

		private void TickIdleLoopFadeOut()
		{
			if (!_idleLoopFadingOut)
			{
				return;
			}
			if (!_idleLoopInstance.isValid())
			{
				_idleLoopPlaying = false;
				_idleLoopFadingOut = false;
				return;
			}
			float num = Mathf.Max(0.01f, _idleLoopFadeOutDuration);
			_idleLoopFadeElapsed += Time.deltaTime;
			float volume = Mathf.Lerp(1f, 0f, Mathf.Clamp01(_idleLoopFadeElapsed / num));
			_idleLoopInstance.setVolume(volume);
			if (!(_idleLoopFadeElapsed < num))
			{
				StopIdleLoopImmediate();
			}
		}

		private void StopIdleLoopImmediate()
		{
			if (_audioService != null && _idleLoopInstance.isValid())
			{
				_audioService.StopInstance(_idleLoopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(_idleLoopInstance);
			}
			_idleLoopPlaying = false;
			_idleLoopFadingOut = false;
			_idleLoopFadeElapsed = 0f;
		}

		private void PlayOccludedOneShot(EventReference reference)
		{
			if (_audioService != null && !reference.IsNull && !(_soundSource == null))
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				ApplyOcclusion(eventInstance, smooth: false);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSource);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private void TryPlayVictimWrapSound()
		{
			if (_context.VisualState != SnakeVisualState.Wrap)
			{
				_victimWrapSoundPlayedForPlayerId = 0;
				return;
			}
			int wrapTargetPlayerId = _context.WrapTargetPlayerId;
			if (wrapTargetPlayerId > 0 && wrapTargetPlayerId != _victimWrapSoundPlayedForPlayerId && IsLocalWrapVictim(wrapTargetPlayerId))
			{
				PlayLocal2DOneShot(_startWrapVictimSound);
				_victimWrapSoundPlayedForPlayerId = wrapTargetPlayerId;
			}
		}

		private bool IsLocalWrapVictim(int wrapTargetPlayerId)
		{
			if (_playerMovableModel?.LocalMovable == null || _playerMovableModel.LocalMovable.Object == null)
			{
				return false;
			}
			return _playerMovableModel.LocalMovable.Object.InputAuthority.PlayerId == wrapTargetPlayerId;
		}

		private void PlayLocal2DOneShot(EventReference reference)
		{
			if (_audioService != null && !reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				if (eventInstance.isValid())
				{
					eventInstance.start();
					eventInstance.release();
				}
			}
		}

		private void UpdateVictimWrapLoop()
		{
			if (_context.VisualState == SnakeVisualState.Wrap && _context.WrapTargetPlayerId > 0 && IsLocalWrapVictim(_context.WrapTargetPlayerId))
			{
				if (_wrapVictimLoopFadingOut)
				{
					_wrapVictimLoopFadingOut = false;
					_wrapVictimLoopFadeElapsed = 0f;
					if (_wrapVictimLoopInstance.isValid())
					{
						_wrapVictimLoopInstance.setVolume(1f);
					}
				}
				if (!_wrapVictimLoopPlaying)
				{
					StartVictimWrapLoop();
				}
			}
			else
			{
				if (_wrapVictimLoopPlaying && !_wrapVictimLoopFadingOut)
				{
					_wrapVictimLoopFadingOut = true;
					_wrapVictimLoopFadeElapsed = 0f;
				}
				if (_wrapVictimLoopFadingOut)
				{
					TickVictimWrapLoopFadeOut();
				}
			}
		}

		private void StartVictimWrapLoop()
		{
			if (!_wrapVictimLoopSound.IsNull && _audioService != null)
			{
				_wrapVictimLoopInstance = _audioService.CreateInstance(_wrapVictimLoopSound);
				if (_wrapVictimLoopInstance.isValid())
				{
					_wrapVictimLoopInstance.setVolume(1f);
					_wrapVictimLoopInstance.start();
					_wrapVictimLoopPlaying = true;
					_wrapVictimLoopFadingOut = false;
					_wrapVictimLoopFadeElapsed = 0f;
				}
			}
		}

		private void TickVictimWrapLoopFadeOut()
		{
			if (!_wrapVictimLoopInstance.isValid())
			{
				_wrapVictimLoopPlaying = false;
				_wrapVictimLoopFadingOut = false;
				return;
			}
			float num = Mathf.Max(0.01f, _wrapVictimLoopFadeOutDuration);
			_wrapVictimLoopFadeElapsed += Time.deltaTime;
			float volume = Mathf.Lerp(1f, 0f, Mathf.Clamp01(_wrapVictimLoopFadeElapsed / num));
			_wrapVictimLoopInstance.setVolume(volume);
			if (!(_wrapVictimLoopFadeElapsed < num))
			{
				_wrapVictimLoopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				_wrapVictimLoopInstance.release();
				_wrapVictimLoopPlaying = false;
				_wrapVictimLoopFadingOut = false;
			}
		}

		private void StopVictimWrapLoopImmediate()
		{
			if (_wrapVictimLoopPlaying)
			{
				if (_wrapVictimLoopInstance.isValid())
				{
					_wrapVictimLoopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					_wrapVictimLoopInstance.release();
				}
				_wrapVictimLoopPlaying = false;
				_wrapVictimLoopFadingOut = false;
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _verticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * _verticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _verticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private void ApplyOcclusion(EventInstance eventInstance, bool smooth)
		{
			if (!(_playerMovableModel?.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = ((_soundSource != null && _soundSource.SoundSourceTransform != null) ? _soundSource.SoundSourceTransform.position : base.transform.position) - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(_voiceOcclusionLowPassParameterName, out var value);
				float num = 1f;
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, _occlusionLayerMask, QueryTriggerInteraction.Ignore))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					num = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
				}
				if (!smooth)
				{
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, num, ignoreseekspeed: true);
					return;
				}
				float value2 = Mathf.Lerp(value, num, Time.deltaTime * 5f);
				eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value2, ignoreseekspeed: true);
			}
		}

		private void OnDestroy()
		{
			StopAllImmediate();
		}
	}
}
