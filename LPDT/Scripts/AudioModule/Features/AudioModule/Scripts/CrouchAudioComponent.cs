using FMODUnity;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	public class CrouchAudioComponent : MonoBehaviour
	{
		[SerializeField]
		private CrouchAnimationFunctionReactor _crouchAnimationFunctionReactor;

		[SerializeField]
		private EventReference _crouchReference;

		[SerializeField]
		private EventReference _crouchEndReference;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private IAudioService _audioService;

		[Inject]
		private void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		private void OnEnable()
		{
			_crouchAnimationFunctionReactor.OnCrouch += PlayCrouchSound;
			_crouchAnimationFunctionReactor.OnCrouchEnd += PlayCrouchEndSound;
		}

		private void OnDisable()
		{
			_crouchAnimationFunctionReactor.OnCrouch -= PlayCrouchSound;
			_crouchAnimationFunctionReactor.OnCrouchEnd -= PlayCrouchEndSound;
		}

		private void PlayCrouchEndSound()
		{
			_audioService.PlayOneShotAttached(_crouchEndReference, _soundSourceBehaviour);
		}

		private void PlayCrouchSound()
		{
			_audioService.PlayOneShotAttached(_crouchReference, _soundSourceBehaviour);
		}
	}
}
