using UnityEngine;

namespace Dissonance.Audio.Playback
{
	public class VoicePlayback : BaseVoicePlayback
	{
		private static readonly Log Log = Logs.Create(LogCategory.Playback, "Voice Playback Component");

		public float ReverbMargin = 7.5f;

		private float _stopPlayingCounter;

		private SamplePlaybackComponent _player;

		public AudioSource AudioSource { get; private set; }

		public override float Amplitude
		{
			get
			{
				if (!(_player == null))
				{
					return _player.ARV;
				}
				return 0f;
			}
		}

		public override bool AllowPositionalPlayback
		{
			get
			{
				return !RemoteSpeakerDeathState.IsDead(base.PlayerName);
			}
			set
			{
			}
		}

		public void Awake()
		{
			AudioSource = GetComponent<AudioSource>();
			_player = GetComponent<SamplePlaybackComponent>();
			((IVoicePlaybackInternal)this).Reset();
		}

		public override void Setup(IPriorityManager priority, IVolumeProvider volume)
		{
			base.Setup(priority, volume);
			AudioSource audioSource = base.gameObject.GetComponent<AudioSource>();
			if (audioSource == null)
			{
				audioSource = base.gameObject.AddComponent<AudioSource>();
				audioSource.rolloffMode = AudioRolloffMode.Linear;
				audioSource.bypassReverbZones = true;
			}
			audioSource.loop = true;
			audioSource.pitch = 1f;
			audioSource.clip = null;
			audioSource.playOnAwake = false;
			audioSource.ignoreListenerPause = true;
			audioSource.Stop();
			if (base.gameObject.GetComponent<SamplePlaybackComponent>() == null)
			{
				base.gameObject.AddComponent<SamplePlaybackComponent>();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			AudioSource.Stop();
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.identity;
			AudioSource.spatialBlend = 1f;
			if (AudioSource.spatialize)
			{
				AudioSource.spatialize = false;
			}
			AudioSource.clip = AudioClip.Create("Flatline", 4096, 1, AudioSettings.outputSampleRate, stream: false, delegate(float[] buf)
			{
				for (int i = 0; i < buf.Length; i++)
				{
					buf[i] = 1f;
				}
			});
			AudioSource.loop = true;
			AudioSource.pitch = 1f;
			AudioSource.dopplerLevel = 0f;
			AudioSource.mute = false;
			AudioSource.priority = 0;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (AudioSource != null && AudioSource.clip != null)
			{
				AudioClip clip = AudioSource.clip;
				AudioSource.clip = null;
				Object.Destroy(clip);
			}
		}

		protected override void Update()
		{
			base.Update();
			if (!_player.HasActiveSession)
			{
				SpeechSession? speechSession = TryDequeueSession();
				if (speechSession.HasValue)
				{
					_player.Play(speechSession.Value);
					AudioSource.Play();
					_stopPlayingCounter = 0f;
				}
				else if (AudioSource.isPlaying)
				{
					_stopPlayingCounter += Time.unscaledDeltaTime;
					if (_stopPlayingCounter > ReverbMargin)
					{
						_stopPlayingCounter = 0f;
						AudioSource.Stop();
					}
				}
			}
			if (AudioSource.mute)
			{
				Log.Warn("Voice AudioSource was muted, unmuting source. To mute a specific Dissonance player see: https://placeholder-software.co.uk/dissonance/docs/Reference/Other/VoicePlayerState.html#islocallymuted-bool");
				AudioSource.mute = false;
			}
			UpdatePositionalPlayback();
		}

		private void UpdatePositionalPlayback()
		{
			if (_player.HasActiveSession)
			{
				bool flag = base.LatestPlaybackOptions?.IsPositional ?? true;
				float num = ((AllowPositionalPlayback && flag) ? 1f : 0f);
				if (!Mathf.Approximately(AudioSource.spatialBlend, num))
				{
					AudioSource.spatialBlend = num;
				}
			}
		}

		protected override void ForceReset()
		{
			SamplePlaybackComponent player = _player;
			if (player != null)
			{
				player.Clear();
			}
		}

		protected override SpeechSession? TryGetActiveSession()
		{
			if (!(_player == null))
			{
				return _player.Session;
			}
			return null;
		}
	}
}
