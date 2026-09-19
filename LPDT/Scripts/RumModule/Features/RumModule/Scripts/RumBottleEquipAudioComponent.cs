using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.CollectingModule.Scripts;
using Features.GrabModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class RumBottleEquipAudioComponent : MonoBehaviour
	{
		[SerializeField]
		private EventReference _fullEquipReference;

		[SerializeField]
		private EventReference _emptyEquipReference;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private RumBottleInteractable _rumBottleInteractable;

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

		private void PlayEquipSound(int playerId)
		{
			EventReference eventReference = (_rumBottleInteractable.IsConsumed ? _emptyEquipReference : _fullEquipReference);
			_audioService.PlayOneShotAttached(eventReference, _soundSourceBehaviour);
		}
	}
}
