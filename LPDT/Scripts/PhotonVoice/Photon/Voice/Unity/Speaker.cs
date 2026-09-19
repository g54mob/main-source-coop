using System;
using System.Threading;
using UnityEngine;

namespace Photon.Voice.Unity
{
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu("Photon Voice/Speaker")]
	[DisallowMultipleComponent]
	public class Speaker : VoiceComponent
	{
		protected IAudioOut<float> audioOutput;

		[SerializeField]
		protected AudioOutDelayControl.PlayDelayConfig playDelayConfig = AudioOutDelayControl.PlayDelayConfig.Default;

		[SerializeField]
		protected bool restartOnDeviceChange = true;

		private int restartPlaybackPending;

		public bool IsPlaying
		{
			get
			{
				if (audioOutput != null)
				{
					return audioOutput.IsPlaying;
				}
				return false;
			}
		}

		public int Lag
		{
			get
			{
				if (audioOutput != null)
				{
					return audioOutput.Lag;
				}
				return 0;
			}
		}

		public Action<Speaker> OnRemoteVoiceRemoveAction { get; set; }

		public RemoteVoiceLink RemoteVoice { get; private set; }

		public bool IsLinked => RemoteVoice != null;

		public AudioOutDelayControl.PlayDelayConfig PlayDelayConfig
		{
			get
			{
				return playDelayConfig;
			}
			set
			{
				if (playDelayConfig.Low != value.Low || playDelayConfig.High != value.High || playDelayConfig.Max != value.Max)
				{
					playDelayConfig = value;
					RestartPlayback();
				}
			}
		}

		public int PlayDelay
		{
			get
			{
				return playDelayConfig.Delay;
			}
			set
			{
				AudioOutDelayControl.PlayDelayConfig playDelayConfig = this.playDelayConfig;
				playDelayConfig.Delay = value;
				if (!playDelayConfig.Equals(this.playDelayConfig))
				{
					this.playDelayConfig = playDelayConfig;
					RestartPlayback();
				}
			}
		}

		public bool RestartOnDeviceChange
		{
			get
			{
				return restartOnDeviceChange;
			}
			set
			{
				restartOnDeviceChange = value;
				AudioSettings.OnAudioConfigurationChanged -= AudioConfigurationChangeHandler;
				if (restartOnDeviceChange)
				{
					AudioSettings.OnAudioConfigurationChanged += AudioConfigurationChangeHandler;
				}
			}
		}

		public event Action<Speaker, float[], int, int> OnDecodedFrame;

		protected override void Awake()
		{
			base.Awake();
			RestartOnDeviceChange = restartOnDeviceChange;
		}

		private void AudioConfigurationChangeHandler(bool deviceWasChanged)
		{
			base.Logger.Log(LogLevel.Info, "Audio configuration changed. Restarting.");
			RestartPlayback();
		}

		private void Initialize()
		{
			base.Logger.Log(LogLevel.Info, "Initializing.");
			audioOutput = CreateAudioOut();
			base.Logger.Log(LogLevel.Info, "Initialized.");
		}

		protected virtual IAudioOut<float> CreateAudioOut()
		{
			return new UnityAudioOut(GetComponent<AudioSource>(), playDelayConfig, base.Logger, string.Empty, debugInfo: true);
		}

		internal bool Link(RemoteVoiceLink stream)
		{
			if (IsLinked)
			{
				base.Logger.Log(LogLevel.Warning, "Speaker already linked to {0}, cancelled linking to {1}", RemoteVoice, stream);
				return false;
			}
			if (stream.VoiceInfo.Channels <= 0)
			{
				base.Logger.Log(LogLevel.Error, "Received voice info channels is not expected (<= 0), cancelled linking to {0}", stream);
				return false;
			}
			base.Logger.Log(LogLevel.Info, "Link {0}", stream);
			stream.RemoteVoiceRemoved += OnRemoteVoiceRemove;
			stream.FloatFrameDecoded += OnAudioFrame;
			RemoteVoice = stream;
			Initialize();
			return StartPlayback();
		}

		private void OnRemoteVoiceRemove()
		{
			base.Logger.Log(LogLevel.Info, "OnRemoteVoiceRemove {0}", RemoteVoice);
			StopPlayback();
			if (OnRemoteVoiceRemoveAction != null)
			{
				OnRemoteVoiceRemoveAction(this);
			}
			Unlink();
		}

		private void OnAudioFrame(FrameOut<float> frame)
		{
			IAudioOut<float> audioOut = audioOutput;
			if (audioOut != null)
			{
				RemoteVoiceLink remoteVoice = RemoteVoice;
				if (remoteVoice != null)
				{
					VoiceInfo voiceInfo = remoteVoice.VoiceInfo;
					this.OnDecodedFrame?.Invoke(this, frame.Buf, voiceInfo.SamplingRate, voiceInfo.Channels);
				}
				audioOut.Push(frame.Buf);
				if (frame.EndOfStream)
				{
					audioOut.Flush();
				}
			}
		}

		private bool StartPlayback()
		{
			if (RemoteVoice == null)
			{
				base.Logger.Log(LogLevel.Warning, "Cannot start playback because speaker is not linked");
				return false;
			}
			if (audioOutput == null)
			{
				base.Logger.Log(LogLevel.Warning, "Cannot start playback because not initialized yet");
				return false;
			}
			VoiceInfo voiceInfo = RemoteVoice.VoiceInfo;
			audioOutput.Start(voiceInfo.SamplingRate, voiceInfo.Channels, voiceInfo.FrameDurationSamples);
			base.Logger.Log(LogLevel.Info, "Speaker started playback: {0}, delay {1}", voiceInfo, playDelayConfig);
			return true;
		}

		protected virtual void OnDestroy()
		{
			base.Logger.Log(LogLevel.Info, "OnDestroy");
			StopPlayback();
			Unlink();
			this.OnDecodedFrame = null;
			AudioSettings.OnAudioConfigurationChanged -= AudioConfigurationChangeHandler;
		}

		private void StopPlayback()
		{
			base.Logger.Log(LogLevel.Info, "StopPlayback");
			if (audioOutput != null)
			{
				audioOutput.Stop();
				audioOutput = null;
			}
		}

		private void Unlink()
		{
			if (RemoteVoice != null)
			{
				RemoteVoice.FloatFrameDecoded -= OnAudioFrame;
				RemoteVoice.RemoteVoiceRemoved -= OnRemoteVoiceRemove;
				RemoteVoice = null;
			}
		}

		protected void Update()
		{
			if (Interlocked.Exchange(ref restartPlaybackPending, 0) != 0)
			{
				base.Logger.Log(LogLevel.Info, "Restarting playback");
				StopPlayback();
				Initialize();
				StartPlayback();
			}
			if (audioOutput != null)
			{
				audioOutput.Service();
			}
		}

		public void RestartPlayback()
		{
			restartPlaybackPending = 1;
		}
	}
}
