using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	public class ItemEquipAudioComponent : MonoBehaviour
	{
		[SerializeField]
		private EventReference _equipReference;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

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
			_simplePointGrabable.LocalOnGrab += PlayEquipSound;
		}

		private void OnDisable()
		{
			_simplePointGrabable.LocalOnGrab -= PlayEquipSound;
		}

		private void PlayEquipSound(int i)
		{
			if (!(_soundSourceBehaviour == null))
			{
				_audioService.PlayOneShotAttached(_equipReference, _soundSourceBehaviour);
			}
		}
	}
}
