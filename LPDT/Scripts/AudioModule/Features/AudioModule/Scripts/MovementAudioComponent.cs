using FMODUnity;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	public class MovementAudioComponent : MonoBehaviour
	{
		[SerializeField]
		private CharacterMovableBase _characterMovableBase;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private MovementStepsAnimationFunctionReactor _movementStepsAnimationFunctionReactor;

		[SerializeField]
		private EventReference _stepReference;

		[SerializeField]
		private EventReference _fustStepReference;

		[SerializeField]
		private EventReference _crouchStepReference;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private float _smoothSpeed = 1f;

		[SerializeField]
		private float _lowSpeedThreshold = 0.1f;

		[SerializeField]
		private float _highSpeedThreshold = 3f;

		private float _smoothedSpeed;

		private IAudioService _audioService;

		private bool IsCrouching
		{
			get
			{
				if (_characterMovableBase != null)
				{
					return _characterMovableBase.MovementState == MovementState.Crouching;
				}
				return false;
			}
		}

		[Inject]
		private void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
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
			if (!(_animator == null))
			{
				_smoothedSpeed = Mathf.Lerp(_smoothedSpeed, _animator.GetFloat("CurrentSpeed"), Time.deltaTime * _smoothSpeed);
			}
		}

		private void PlayStepSound()
		{
			if (CanPlayStepSound(_lowSpeedThreshold))
			{
				EventReference eventReference = ((IsCrouching && !_crouchStepReference.IsNull) ? _crouchStepReference : _stepReference);
				_audioService.PlayOneShotAttached(eventReference, _soundSourceBehaviour);
			}
		}

		private void PlayRunStepSound()
		{
			if (CanPlayStepSound(_highSpeedThreshold))
			{
				_audioService.PlayOneShotAttached(_fustStepReference, _soundSourceBehaviour);
			}
		}

		private bool CanPlayStepSound(float animatorSpeedThreshold)
		{
			if (_characterMovableBase == null || !_characterMovableBase.IsGrounded)
			{
				return false;
			}
			return _smoothedSpeed >= animatorSpeedThreshold;
		}
	}
}
