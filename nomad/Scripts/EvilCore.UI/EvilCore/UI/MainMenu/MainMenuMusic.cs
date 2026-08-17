using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Extensions;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.MainMenu
{
	public class MainMenuMusic : MonoBehaviour
	{
		[SerializeField]
		private SoundID musicTrack;

		[Tooltip("Fade-out length when leaving the menu into the game.")]
		[SerializeField]
		private float fadeOutSeconds = 1.5f;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private IGameLoadingManager _loadingManager;

		private AudioHandle _handle;

		private bool _stopped;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		private void Start()
		{
			if (musicTrack.IsValid())
			{
				_handle = _audioManager.PlayEvent(musicTrack);
			}
			_loadingManager?.OnLoadingComplete.AddListener(StopMusic);
		}

		private void StopMusic()
		{
			if (!_stopped)
			{
				_stopped = true;
				if (_handle.IsValid)
				{
					_audioManager?.StopEvent(_handle, AudioStopMode.AllowFadeout, fadeOutSeconds);
				}
			}
		}

		private void OnDestroy()
		{
			_loadingManager?.OnLoadingComplete.RemoveListener(StopMusic);
			StopMusic();
		}
	}
}
