using FMOD.Studio;
using FMODUnity;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	public class MimicMovementAudioComponent : MonoBehaviour
	{
		private const int OCCLUSION_HITS_BUFFER_SIZE = 1;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private MovementStepsAnimationFunctionReactor _movementStepsAnimationFunctionReactor;

		[SerializeField]
		private EventReference _stepReference;

		[SerializeField]
		private EventReference _fustStepReference;

		[SerializeField]
		private float _smoothSpeed = 1f;

		[SerializeField]
		private float _lowSpeedThreshold = 0.1f;

		[SerializeField]
		private float _highSpeedThreshold = 3f;

		[SerializeField]
		private LayerMask _occlusionLayerMask;

		[SerializeField]
		private float _occlusionMaxDistance;

		[SerializeField]
		private float _verticalDistanceMultiplier = 3f;

		[SerializeField]
		private float _verticalFalloffExponent = 2f;

		[SerializeField]
		private float _lowPassMinValue;

		[SerializeField]
		private string _voiceOcclusionLowPassParameterName = "VoiceOcclusionLowPass";

		private float _smoothedSpeed;

		private PlayerMovableModel _playerMovableModel;

		private IAudioService _audioService;

		private MimicEnemyContext _mimicEnemyContext;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[1];

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel, IAudioService audioService, MimicEnemyContext mimicEnemyContext)
		{
			_playerMovableModel = playerMovableModel;
			_audioService = audioService;
			_mimicEnemyContext = mimicEnemyContext;
		}

		private void OnEnable()
		{
			_movementStepsAnimationFunctionReactor.OnStep += PlayStepSound;
			_movementStepsAnimationFunctionReactor.OnRunStep += PlayRunStepSound;
		}

		private void OnDisable()
		{
			_movementStepsAnimationFunctionReactor.OnStep -= PlayStepSound;
			_movementStepsAnimationFunctionReactor.OnRunStep -= PlayRunStepSound;
		}

		private void Update()
		{
			_smoothedSpeed = Mathf.Lerp(_smoothedSpeed, _animator.GetFloat("Velosity"), Time.deltaTime * _smoothSpeed);
		}

		private void PlayStepSound()
		{
			if (!(_smoothedSpeed < _lowSpeedThreshold))
			{
				EventInstance eventInstance = _audioService.CreateInstance(_stepReference);
				ProcessSoundOcclusion(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _mimicEnemyContext.SoundSourceBehaviour);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private void PlayRunStepSound()
		{
			if (!(_smoothedSpeed < _highSpeedThreshold))
			{
				EventInstance eventInstance = _audioService.CreateInstance(_fustStepReference);
				ProcessSoundOcclusion(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _mimicEnemyContext.SoundSourceBehaviour);
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

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (!(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(_voiceOcclusionLowPassParameterName, out var value);
				if (Physics.RaycastNonAlloc(position, offset.normalized, _occlusionHits, magnitude, _occlusionLayerMask) > 0)
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					float b = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value3);
				}
			}
		}
	}
}
