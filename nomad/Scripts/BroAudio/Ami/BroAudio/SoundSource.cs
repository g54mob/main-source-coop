using UnityEngine;
using UnityEngine.Serialization;

namespace Ami.BroAudio
{
	[HelpURL("https://man572142s-organization.gitbook.io/broaudio/core-features/no-code-components/sound-source")]
	[AddComponentMenu("BroAudio/SoundSource")]
	public class SoundSource : MonoBehaviour
	{
		public enum PositionMode
		{
			Global = 0,
			FollowGameObject = 1,
			StayHere = 2
		}

		[SerializeField]
		private bool _onlyPlayOnce;

		[SerializeField]
		[FormerlySerializedAs("_playOnStart")]
		private bool _playOnEnable = true;

		[SerializeField]
		private bool _stopOnDisable;

		[SerializeField]
		private float _overrideFadeOut = -1f;

		[Space]
		[SerializeField]
		private SoundID _sound;

		[SerializeField]
		private PlaybackGroup _overrideGroup;

		[SerializeField]
		private PositionMode _positionMode;

		[SerializeField]
		private float _delay;

		public IAudioPlayer CurrentPlayer { get; private set; }

		public bool IsPlaying
		{
			get
			{
				if (CurrentPlayer != null)
				{
					return CurrentPlayer.IsPlaying;
				}
				return false;
			}
		}

		public bool IsActive
		{
			get
			{
				if (CurrentPlayer != null)
				{
					return CurrentPlayer.IsActive;
				}
				return false;
			}
		}

		public void Play()
		{
			switch (_positionMode)
			{
			case PositionMode.Global:
				PlayGlobally();
				break;
			case PositionMode.FollowGameObject:
				Play(base.transform);
				break;
			case PositionMode.StayHere:
				Play(base.transform.position);
				break;
			}
		}

		public void PlayGlobally()
		{
			Stop();
			CurrentPlayer = BroAudio.Play(_sound, _overrideGroup);
		}

		public void Play(Transform followTarget)
		{
			Stop();
			CurrentPlayer = BroAudio.Play(_sound, followTarget, (IPlayableValidator)_overrideGroup);
		}

		public void Play(Vector3 positon)
		{
			Stop();
			CurrentPlayer = BroAudio.Play(_sound, positon, _overrideGroup);
		}

		public void Stop()
		{
			Stop(-1f);
		}

		public void Stop(float fadeTime)
		{
			if (IsActive)
			{
				CurrentPlayer.Stop(fadeTime);
			}
		}

		public void Pause()
		{
			Pause(-1f);
		}

		public void Pause(float fadeTime)
		{
			if (IsActive)
			{
				CurrentPlayer.Pause(fadeTime);
			}
		}

		public void UnPause()
		{
			UnPause(-1f);
		}

		public void UnPause(float fadeTime)
		{
			if (IsActive)
			{
				CurrentPlayer.UnPause(fadeTime);
			}
		}

		public void SetVolume(float vol)
		{
			SetVolume(vol, 0f);
		}

		public void SetVolume(float vol, float fadeTime)
		{
			if (IsPlaying)
			{
				CurrentPlayer.SetVolume(vol, fadeTime);
			}
		}

		public void SetPitch(float pitch)
		{
			SetPitch(pitch, 0f);
		}

		public void SetPitch(float pitch, float fadeTime)
		{
			if (IsPlaying)
			{
				CurrentPlayer.SetPitch(pitch, fadeTime);
			}
		}

		private void OnEnable()
		{
			if (_playOnEnable)
			{
				Play();
				if (_delay > 0f)
				{
					CurrentPlayer.SetDelay(_delay);
				}
				if (_onlyPlayOnce)
				{
					_playOnEnable = false;
				}
			}
		}

		private void OnDisable()
		{
			if (_stopOnDisable && CurrentPlayer != null && CurrentPlayer.IsPlaying)
			{
				CurrentPlayer.Stop(_overrideFadeOut);
			}
		}
	}
}
