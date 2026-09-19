using FMODUnity;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts
{
	public class TentacleAudioController : MonoBehaviour
	{
		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private EventReference _catchSound;

		[SerializeField]
		private EventReference _throwSound;

		private IAudioService _audioService;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		private void PlayCatchSound()
		{
			if (!(_soundSourceBehaviour == null))
			{
				_audioService.PlayOneShotAttached(_catchSound, _soundSourceBehaviour);
			}
		}

		private void PlayThrowSound()
		{
			if (!(_soundSourceBehaviour == null))
			{
				_audioService.PlayOneShotAttached(_throwSound, _soundSourceBehaviour);
			}
		}
	}
}
