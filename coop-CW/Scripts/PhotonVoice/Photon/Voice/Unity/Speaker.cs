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
				return playDelayConfig.Low;
			}
			set
			{
				int num = 1000;
				if (playDelayConfig.Low != value || playDelayConfig.High != value || playDelayConfig.Max != num)
				{
					playDelayConfig.Low = value;
					playDelayConfig.High = value;
					playDelayConfig.Max = num;
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

		protected override void Awake()
		{
			base.Awake();
			RestartOnDeviceChange = restartOnDeviceChange;
		}

		private void AudioConfigurationChangeHandler(bool deviceWasChanged)
		{
			base.Logger.LogInfo("Audio configuration changed. Restarting.");
			RestartPlayback();
		}

		private void Initialize()
		{
			base.Logger.LogInfo("Initializing.");
			audioOutput = CreateAudioOut();
			base.Logger.LogInfo("Initialized.");
		}

		protected virtual IAudioOut<float> CreateAudioOut()
		{
			return new UnityAudioOut(GetComponent<AudioSource>(), playDelayConfig, base.Logger, string.Empty, debugInfo: true);
		}

		internal bool Link(RemoteVoiceLink stream)
		{
			if (IsLinked)
			{
				base.Logger.LogWarning("Speaker already linked to {0}, cancelled linking to {1}", RemoteVoice, stream);
				return false;
			}
			if (stream.VoiceInfo.Channels <= 0)
			{
				base.Logger.LogError("Received voice info channels is not expected (<= 0), cancelled linking to {0}", stream);
				return false;
			}
			base.Logger.LogInfo("Link {0}", stream);
			stream.RemoteVoiceRemoved += OnRemoteVoiceRemove;
			stream.FloatFrameDecoded += OnAudioFrame;
			RemoteVoice = stream;
			Initialize();
			return StartPlayback();
		}

		private void OnRemoteVoiceRemove()
		{
			base.Logger.LogInfo("OnRemoteVoiceRemove {0}", RemoteVoice);
			StopPlayback();
			if (OnRemoteVoiceRemoveAction != null)
			{
				OnRemoteVoiceRemoveAction(this);
			}
			Unlink();
		}

		private void OnAudioFrame(FrameOut<float> frame)
		{
			if (frame.Buf != null)
			{
				audioOutput.Push(frame.Buf);
			}
			if (frame.EndOfStream)
			{
				audioOutput.Flush();
			}
		}

		private bool StartPlayback()
		{
			if (RemoteVoice == null)
			{
				base.Logger.LogWarning("Cannot start playback because speaker is not linked");
				return false;
			}
			if (audioOutput == null)
			{
				base.Logger.LogWarning("Cannot start playback because not initialized yet");
				return false;
			}
			VoiceInfo voiceInfo = RemoteVoice.VoiceInfo;
			audioOutput.Start(voiceInfo.SamplingRate, voiceInfo.Channels, voiceInfo.FrameDurationSamples);
			base.Logger.LogInfo("Speaker started playback: {0}, delay {1}", voiceInfo, playDelayConfig);
			return true;
		}

		protected virtual void OnDestroy()
		{
			base.Logger.LogInfo("OnDestroy");
			StopPlayback();
			Unlink();
			AudioSettings.OnAudioConfigurationChanged -= AudioConfigurationChangeHandler;
		}

		private void StopPlayback()
		{
			base.Logger.LogInfo("StopPlayback");
			if (audioOutput != null)
			{
				audioOutput.Stop();
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
				base.Logger.LogInfo("Restarting playback");
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
