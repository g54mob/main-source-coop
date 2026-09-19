using FMOD.Studio;
using FMODUnity;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public class RatsHoleEnemyAudioController : MonoBehaviour
	{
		private const float StepVelocityDeadZone = 0.05f;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		private MovableAnimationFunctionReactor _movableReactor;

		[SerializeField]
		private DamageableAnimationFunctionReactor _attackReactor;

		[SerializeField]
		private EventReference _attackSound;

		[SerializeField]
		private EventReference _stepSound;

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

		private RatsHoleEnemyContext _context;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		[Inject]
		private void InjectDependencies(RatsHoleEnemyContext context, IAudioService audioService, PlayerMovableModel playerMovableModel)
		{
			_context = context;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
		}

		private void OnEnable()
		{
			if (!(_movableReactor == null))
			{
				_attackReactor.TryDealBaseAttackDamage += PlayAttackSound;
				_movableReactor.OnFootstepPerformed += PlayStepSound;
			}
		}

		private void OnDisable()
		{
			if (!(_movableReactor == null))
			{
				_attackReactor.TryDealBaseAttackDamage -= PlayAttackSound;
				_movableReactor.OnFootstepPerformed -= PlayStepSound;
			}
		}

		private void PlayAttackSound()
		{
			PlayOneShot(_attackSound);
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
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSource);
				_audioService.ReleaseInstance(eventInstance);
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

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_playerMovableModel?.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = ((_soundSource.SoundSourceTransform != null) ? _soundSource.SoundSourceTransform.position : base.transform.position) - position;
				float magnitude = offset.magnitude;
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, _occlusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					float value = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, 1f);
				}
			}
		}
	}
}
