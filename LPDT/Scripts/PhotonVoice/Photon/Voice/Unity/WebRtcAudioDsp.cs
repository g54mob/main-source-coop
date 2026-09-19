using System.Collections.Generic;
using UnityEngine;

namespace Photon.Voice.Unity
{
	[RequireComponent(typeof(Recorder))]
	[AddComponentMenu("Photon Voice/WebRTC Audio DSP")]
	[DisallowMultipleComponent]
	public class WebRtcAudioDsp : VoiceComponent
	{
		[SerializeField]
		private bool aec = true;

		[SerializeField]
		private bool aecHighPass;

		[SerializeField]
		private bool agc = true;

		[SerializeField]
		[Range(0f, 90f)]
		private int agcCompressionGain = 9;

		[SerializeField]
		[Range(0f, 31f)]
		private int agcTargetLevel = 3;

		[SerializeField]
		private bool vad = true;

		[SerializeField]
		private bool highPass;

		private bool bypass;

		[SerializeField]
		private bool noiseSuppression = true;

		[SerializeField]
		private int reverseStreamDelayMs = 120;

		private int reverseChannels;

		private WebRTCAudioProcessor proc;

		private static readonly Dictionary<AudioSpeakerMode, int> channelsMap = new Dictionary<AudioSpeakerMode, int>
		{
			{
				AudioSpeakerMode.Mono,
				1
			},
			{
				AudioSpeakerMode.Stereo,
				2
			},
			{
				AudioSpeakerMode.Quad,
				4
			},
			{
				AudioSpeakerMode.Surround,
				5
			},
			{
				AudioSpeakerMode.Mode5point1,
				6
			},
			{
				AudioSpeakerMode.Mode7point1,
				8
			},
			{
				AudioSpeakerMode.Prologic,
				2
			}
		};

		private LocalVoiceAudioShort localVoice;

		private int outputSampleRate;

		public bool AEC
		{
			get
			{
				return aec;
			}
			set
			{
				if (value != aec)
				{
					aec = value;
					applyToProc();
				}
			}
		}

		public bool AecHighPass
		{
			get
			{
				return aecHighPass;
			}
			set
			{
				if (value != aecHighPass)
				{
					aecHighPass = value;
					applyToProc();
				}
			}
		}

		public int ReverseStreamDelayMs
		{
			get
			{
				return reverseStreamDelayMs;
			}
			set
			{
				if (value != reverseStreamDelayMs)
				{
					reverseStreamDelayMs = value;
					applyToProc();
				}
			}
		}

		public bool NoiseSuppression
		{
			get
			{
				return noiseSuppression;
			}
			set
			{
				if (value != noiseSuppression)
				{
					noiseSuppression = value;
					applyToProc();
					Restart();
				}
			}
		}

		public bool HighPass
		{
			get
			{
				return highPass;
			}
			set
			{
				if (value != highPass)
				{
					highPass = value;
					applyToProc();
				}
			}
		}

		public bool Bypass
		{
			get
			{
				return bypass;
			}
			set
			{
				if (value != bypass)
				{
					bypass = value;
					applyToProc();
				}
			}
		}

		public bool AGC
		{
			get
			{
				return agc;
			}
			set
			{
				if (value != agc)
				{
					agc = value;
					applyToProc();
				}
			}
		}

		public int AgcCompressionGain
		{
			get
			{
				return agcCompressionGain;
			}
			set
			{
				if (value != agcCompressionGain)
				{
					agcCompressionGain = value;
					applyToProc();
				}
			}
		}

		public int AgcTargetLevel
		{
			get
			{
				return agcTargetLevel;
			}
			set
			{
				if (value != agcTargetLevel)
				{
					agcTargetLevel = value;
					applyToProc();
				}
			}
		}

		public bool VAD
		{
			get
			{
				return vad;
			}
			set
			{
				if (value != vad)
				{
					vad = value;
					applyToProc();
				}
			}
		}

		public bool IsSupported => true;

		protected override void Awake()
		{
			base.Awake();
			if (IsSupported)
			{
				AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
				return;
			}
			base.Logger.Log(LogLevel.Warning, "WebRtcAudioDsp is not supported on this platform {0}. The component will be disabled.", Application.platform);
		}

		private void Start()
		{
		}

		public void AdjustVoiceInfo(ref VoiceInfo voiceInfo, ref AudioSampleType st)
		{
			if (IsSupported && base.enabled)
			{
				st = AudioSampleType.Short;
				base.Logger.Log(LogLevel.Info, "Type Conversion set to Short. Audio samples will be converted if source samples types differ.");
				switch (voiceInfo.SamplingRate)
				{
				case 12000:
					base.Logger.Log(LogLevel.Warning, "Sampling rate requested (12kHz) is not supported by WebRtcAudioDsp, switching to the closest supported value: 16kHz.");
					voiceInfo.SamplingRate = 16000;
					break;
				case 24000:
					base.Logger.Log(LogLevel.Warning, "Sampling rate requested (24kHz) is not supported by WebRtcAudioDsp, switching to the closest supported value: 48kHz.");
					voiceInfo.SamplingRate = 48000;
					break;
				}
				if (voiceInfo.FrameDurationUs < 10000)
				{
					base.Logger.Log(LogLevel.Warning, "Frame duration requested ({0}ms) is not supported by WebRtcAudioDsp (it needs to be N x 10ms), switching to the closest supported value: 10ms.", voiceInfo.FrameDurationUs / 1000);
					voiceInfo.FrameDurationUs = 10000;
				}
			}
		}

		private void OnAudioConfigurationChanged(bool deviceWasChanged)
		{
			if (outputSampleRate != AudioSettings.outputSampleRate)
			{
				base.Logger.Log(LogLevel.Info, "AudioConfigChange: outputSampleRate from {0} to {1}. WebRtcAudioDsp will be restarted.", outputSampleRate, AudioSettings.outputSampleRate);
				outputSampleRate = AudioSettings.outputSampleRate;
				Restart();
			}
			if (reverseChannels != channelsMap[AudioSettings.speakerMode])
			{
				base.Logger.Log(LogLevel.Info, "AudioConfigChange: speakerMode channels from {0} to {1}. WebRtcAudioDsp will be restarted.", reverseChannels, channelsMap[AudioSettings.speakerMode]);
				reverseChannels = channelsMap[AudioSettings.speakerMode];
				Restart();
			}
		}

		private void OnAudioOutFrameFloat(float[] data, int outChannels)
		{
			if (outChannels != reverseChannels)
			{
				base.Logger.Log(LogLevel.Warning, "OnAudioOutFrame channel count {0} != initialized {1}.", outChannels, reverseChannels);
			}
			else
			{
				proc.OnAudioOutFrameFloat(data);
			}
		}

		private void PhotonVoiceCreated(PhotonVoiceCreatedParams p)
		{
			if (IsSupported && base.enabled)
			{
				if (p.Voice.Info.Channels != 1)
				{
					base.Logger.Log(LogLevel.Error, "Only mono audio signals supported. WebRtcAudioDsp component will be disabled.");
				}
				else if (p.Voice is LocalVoiceAudioShort v)
				{
					StartProc(v);
					localVoice = v;
				}
				else
				{
					base.Logger.Log(LogLevel.Error, "Only short audio voice supported. WebRtcAudioDsp component will be disabled.");
				}
			}
		}

		private void PhotonVoiceRemoved()
		{
			StopProc(localVoice);
			localVoice = null;
		}

		private void OnDestroy()
		{
			StopProc(localVoice);
			AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
		}

		private void StartProc(LocalVoiceAudioShort v)
		{
			base.Logger.Log(LogLevel.Info, "Start");
			reverseChannels = channelsMap[AudioSettings.speakerMode];
			outputSampleRate = AudioSettings.outputSampleRate;
			proc = new WebRTCAudioProcessor(base.Logger, v.Info.FrameSize, v.Info.SamplingRate, v.Info.Channels, outputSampleRate, reverseChannels);
			applyToProc();
			v.AddPostProcessor(proc);
		}

		private void StopProc(LocalVoiceAudioShort v)
		{
			base.Logger.Log(LogLevel.Info, "Stop");
			setOutputListener(set: false);
			if (proc != null)
			{
				proc.Dispose();
			}
			v?.RemoveProcessor(proc);
		}

		private void Restart()
		{
			base.Logger.Log(LogLevel.Info, "Restart");
			StopProc(localVoice);
			if (localVoice != null)
			{
				StartProc(localVoice);
			}
		}

		private void setOutputListener(bool set)
		{
			AudioListener audioListener = Object.FindFirstObjectByType<AudioListener>();
			if (!(audioListener != null))
			{
				return;
			}
			AudioOutCapture audioOutCapture = audioListener.gameObject.GetComponent<AudioOutCapture>();
			if (audioOutCapture != null)
			{
				audioOutCapture.OnAudioFrame -= OnAudioOutFrameFloat;
			}
			if (set)
			{
				if (audioOutCapture == null)
				{
					audioOutCapture = audioListener.gameObject.AddComponent<AudioOutCapture>();
				}
				audioOutCapture.OnAudioFrame += OnAudioOutFrameFloat;
			}
		}

		private void applyToProc()
		{
			if (proc != null)
			{
				proc.AEC = aec;
				proc.AECMobile = aec && Application.isMobilePlatform;
				setOutputListener(aec);
				proc.AECStreamDelayMs = reverseStreamDelayMs;
				proc.AECHighPass = aecHighPass;
				proc.HighPass = highPass;
				proc.NoiseSuppression = noiseSuppression;
				proc.AGC = agc;
				proc.AGCCompressionGain = agcCompressionGain;
				proc.AGCTargetLevel = agcTargetLevel;
				proc.VAD = VAD;
				proc.Bypass = Bypass;
			}
		}
	}
}
