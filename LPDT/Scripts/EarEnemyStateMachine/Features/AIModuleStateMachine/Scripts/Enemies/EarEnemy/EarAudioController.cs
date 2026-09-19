using FMOD.Studio;
using FMODUnity;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	public class EarAudioController : MonoBehaviour
	{
		private const float StepVelocityDeadZone = 0.05f;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[Header("Reactors")]
		[SerializeField]
		private DamageableAnimationFunctionReactor _attackReactor;

		[SerializeField]
		private MovableAnimationFunctionReactor _movableReactor;

		[Header("Attack (Damageable reactor)")]
		[SerializeField]
		private ParticleSystem _attackParticle;

		[SerializeField]
		private EventReference _attackSound;

		[Header("Steps (Movable reactor)")]
		[SerializeField]
		private EventReference _stepSound;

		[Header("State beds / stingers")]
		[SerializeField]
		private EventReference _transitionToChaseSound;

		[SerializeField]
		private EventReference _chaseIdleLoopReference;

		[SerializeField]
		private EventReference _stunIdleLoopReference;

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

		private EarEnemyContext _context;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private EventInstance _loopInstance;

		private bool _loopPlaying;

		private EarVisualState _lastVisualState;

		[Inject]
		public void InjectDependencies(EarEnemyContext context, IAudioService audioService, PlayerMovableModel playerMovableModel)
		{
			_context = context;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
		}

		private void OnEnable()
		{
			if (_attackReactor != null)
			{
				_attackReactor.TryDealBaseAttackDamage += PlayAttack;
			}
			if (_movableReactor != null)
			{
				_movableReactor.OnFootstepPerformed += PlayStepSound;
			}
		}

		private void OnDisable()
		{
			if (_attackReactor != null)
			{
				_attackReactor.TryDealBaseAttackDamage -= PlayAttack;
			}
			if (_movableReactor != null)
			{
				_movableReactor.OnFootstepPerformed -= PlayStepSound;
			}
		}

		private void Update()
		{
			EarVisualState visualState = _context.VisualState;
			if (visualState != _lastVisualState)
			{
				EarVisualState lastVisualState = _lastVisualState;
				if (lastVisualState == EarVisualState.Aggro || lastVisualState == EarVisualState.Stun)
				{
					StopLoop();
				}
				switch (visualState)
				{
				case EarVisualState.Aggro:
					PlayOneShot(_transitionToChaseSound);
					StartLoop(_chaseIdleLoopReference);
					break;
				case EarVisualState.Stun:
					StartLoop(_stunIdleLoopReference);
					break;
				}
				_lastVisualState = visualState;
			}
		}

		private void LateUpdate()
		{
			if (_loopPlaying)
			{
				_loopInstance.set3DAttributes(_soundSource.SoundSourceTransform.position.To3DAttributes());
				ApplyOcclusion(_loopInstance, smooth: true);
			}
		}

		private void PlayAttack()
		{
			PlayOneShot(_attackSound);
			if (_attackParticle != null)
			{
				_attackParticle.Play();
			}
		}

		private void PlayStepSound()
		{
			if (_context != null)
			{
				float num = Mathf.Max(_context.MoveSpeed, 0.01f);
				if (_context.SmoothedVelocity / num <= 0.05f)
				{
					return;
				}
			}
			PlayOneShot(_stepSound);
		}

		private void PlayOneShot(EventReference reference)
		{
			if (_audioService != null && !reference.IsNull && !(_soundSource == null))
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				ApplyOcclusion(eventInstance, smooth: false);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSource);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private void StartLoop(EventReference reference)
		{
			if (!_loopPlaying && !reference.IsNull && _audioService != null && !(_soundSource == null))
			{
				_loopInstance = _audioService.CreateInstance(reference);
				ApplyOcclusion(_loopInstance, smooth: false);
				_audioService.StartInstanceWith3DAttributes(_loopInstance, _soundSource);
				_loopPlaying = true;
			}
		}

		private void StopLoop()
		{
			if (_loopPlaying)
			{
				_audioService.StopInstance(_loopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				_audioService.ReleaseInstance(_loopInstance);
				_loopPlaying = false;
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
				Vector3 offset = ((_soundSource.SoundSourceTransform != null) ? _soundSource.SoundSourceTransform.position : base.transform.position) - position;
				float magnitude = offset.magnitude;
				float num = 1f;
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, _occlusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					num = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
				}
				if (!smooth)
				{
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, num);
					return;
				}
				eventInstance.getParameterByName(_voiceOcclusionLowPassParameterName, out var value);
				float value2 = Mathf.Lerp(value, num, Time.deltaTime * 5f);
				eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value2);
			}
		}

		private void OnDestroy()
		{
			if (_loopPlaying)
			{
				_audioService.StopInstance(_loopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(_loopInstance);
				_loopPlaying = false;
			}
		}
	}
}
