using System;
using System.Threading;
using POpusCodec.Enums;
using Photon.Voice.IOS;
using UnityEngine;
using UnityEngine.Serialization;

namespace Photon.Voice.Unity
{
	[AddComponentMenu("Photon Voice/Recorder")]
	[HelpURL("https://doc.photonengine.com/en-us/voice/v2/getting-started/recorder")]
	[DisallowMultipleComponent]
	public class Recorder : VoiceComponent
	{
		public enum InputSourceType
		{
			Microphone = 0,
			AudioClip = 1,
			Factory = 2
		}

		public enum MicType
		{
			Unity = 0,
			Photon = 1
		}

		public const int MIN_OPUS_BITRATE = 6000;

		public const int MAX_OPUS_BITRATE = 510000;

		[SerializeField]
		private bool voiceDetection;

		[SerializeField]
		private float voiceDetectionThreshold = 0.01f;

		[SerializeField]
		private int voiceDetectionDelayMs = 500;

		private object userData;

		private LocalVoice voice = LocalVoiceAudioDummy.Dummy;

		private IAudioDesc inputSource;

		private VoiceConnection voiceConnection;

		[SerializeField]
		[FormerlySerializedAs("audioGroup")]
		private byte interestGroup;

		[SerializeField]
		private bool useTargetPlayers;

		[SerializeField]
		private int[] targetPlayers;

		[SerializeField]
		private bool debugEchoMode;

		[SerializeField]
		private bool reliableMode;

		[SerializeField]
		private bool encrypt;

		[SerializeField]
		private bool transmitEnabled = true;

		[SerializeField]
		private SamplingRate samplingRate = SamplingRate.Sampling24000;

		[SerializeField]
		private OpusCodec.FrameDuration frameDuration = OpusCodec.FrameDuration.Frame40ms;

		[SerializeField]
		[Range(6000f, 510000f)]
		private int bitrate = 30000;

		[SerializeField]
		private InputSourceType sourceType;

		[SerializeField]
		private MicType microphoneType;

		[SerializeField]
		private AudioClip audioClip;

		[SerializeField]
		private bool loopAudioClip = true;

		[SerializeField]
		private bool recordingEnabled = true;

		private Func<IAudioDesc> inputFactory;

		[SerializeField]
		private AudioSessionParameters audioSessionParameters = AudioSessionParametersPresets.Game;

		[SerializeField]
		private AndroidAudioInParameters androidMicrophoneSettings = AndroidAudioInParameters.Default;

		private bool isPausedOrInBackground;

		[SerializeField]
		private bool stopRecordingWhenPaused;

		[SerializeField]
		private bool useOnAudioFilterRead;

		[SerializeField]
		private bool useMicrophoneTypeFallback = true;

		[SerializeField]
		private bool recordWhenJoined = true;

		private DeviceInfo microphoneDevice = DeviceInfo.Default;

		private int microphoneDeviceChangePending;

		private int restartRecordingPending;

		public bool TransmitEnabled
		{
			get
			{
				return transmitEnabled;
			}
			set
			{
				if (value != transmitEnabled)
				{
					transmitEnabled = value;
					if (voice != LocalVoiceAudioDummy.Dummy)
					{
						voice.TransmitEnabled = value;
					}
				}
			}
		}

		public bool Encrypt
		{
			get
			{
				return voice.Encrypt;
			}
			set
			{
				voice.Encrypt = value;
				encrypt = value;
			}
		}

		public bool DebugEchoMode
		{
			get
			{
				return voice.DebugEchoMode;
			}
			set
			{
				voice.DebugEchoMode = value;
				debugEchoMode = value;
			}
		}

		public bool ReliableMode
		{
			get
			{
				return voice.Reliable;
			}
			set
			{
				voice.Reliable = value;
				reliableMode = value;
			}
		}

		public bool VoiceDetection
		{
			get
			{
				return voiceDetection;
			}
			set
			{
				voiceDetection = value;
				if (VoiceDetector != null)
				{
					VoiceDetector.On = value;
				}
			}
		}

		public float VoiceDetectionThreshold
		{
			get
			{
				return voiceDetectionThreshold;
			}
			set
			{
				if (voiceDetectionThreshold.Equals(value))
				{
					return;
				}
				if (value < 0f || value > 1f)
				{
					base.Logger.Log(LogLevel.Error, "Value out of range: VAD Threshold needs to be between [0..1], requested value: {0}", value);
					return;
				}
				voiceDetectionThreshold = value;
				if (VoiceDetector != null)
				{
					VoiceDetector.Threshold = voiceDetectionThreshold;
				}
			}
		}

		public int VoiceDetectionDelayMs
		{
			get
			{
				return voiceDetectionDelayMs;
			}
			set
			{
				if (voiceDetectionDelayMs != value)
				{
					voiceDetectionDelayMs = value;
					if (VoiceDetector != null)
					{
						VoiceDetector.ActivityDelayMs = value;
					}
				}
			}
		}

		public object UserData
		{
			get
			{
				return userData;
			}
			set
			{
				if (userData != value)
				{
					userData = value;
					base.Logger.Log(LogLevel.Info, "Recorder.UserData changed");
					RestartRecording();
				}
			}
		}

		public Func<IAudioDesc> InputFactory
		{
			get
			{
				return inputFactory;
			}
			set
			{
				if (inputFactory != value)
				{
					inputFactory = value;
					base.Logger.Log(LogLevel.Info, "Recorder.InputFactory changed");
					if (SourceType == InputSourceType.Factory)
					{
						RestartRecording();
					}
				}
			}
		}

		public AudioUtil.IVoiceDetector VoiceDetector
		{
			get
			{
				if (voiceAudio == null)
				{
					return null;
				}
				return voiceAudio.VoiceDetector;
			}
		}

		public byte InterestGroup
		{
			get
			{
				return voice.InterestGroup;
			}
			set
			{
				voice.InterestGroup = value;
				interestGroup = value;
			}
		}

		public int[] TargetPlayers
		{
			get
			{
				return voice.TargetPlayers;
			}
			set
			{
				voice.TargetPlayers = value;
				targetPlayers = value;
				useTargetPlayers = value != null;
			}
		}

		public bool IsCurrentlyTransmitting
		{
			get
			{
				if (RecordingEnabled && TransmitEnabled)
				{
					return voice.IsCurrentlyTransmitting;
				}
				return false;
			}
		}

		public AudioUtil.ILevelMeter LevelMeter
		{
			get
			{
				if (voiceAudio == null)
				{
					return null;
				}
				return voiceAudio.LevelMeter;
			}
		}

		public bool VoiceDetectorCalibrating
		{
			get
			{
				if (voiceAudio != null && TransmitEnabled)
				{
					return voiceAudio.VoiceDetectorCalibrating;
				}
				return false;
			}
		}

		protected ILocalVoiceAudio voiceAudio => voice as ILocalVoiceAudio;

		public InputSourceType SourceType
		{
			get
			{
				return sourceType;
			}
			set
			{
				if (sourceType != value)
				{
					sourceType = value;
					base.Logger.Log(LogLevel.Info, "Recorder.Source changed");
					RestartRecording();
				}
			}
		}

		public MicType MicrophoneType
		{
			get
			{
				if (Application.platform == RuntimePlatform.WebGLPlayer)
				{
					return MicType.Photon;
				}
				return microphoneType;
			}
			set
			{
				if (microphoneType != value)
				{
					microphoneType = value;
					base.Logger.Log(LogLevel.Info, "Recorder.MicrophoneType changed");
					if (SourceType == InputSourceType.Microphone)
					{
						RestartRecording();
					}
				}
			}
		}

		public AudioClip AudioClip
		{
			get
			{
				return audioClip;
			}
			set
			{
				if (audioClip != value)
				{
					audioClip = value;
					base.Logger.Log(LogLevel.Info, "Recorder.AudioClip change");
					if (SourceType == InputSourceType.AudioClip)
					{
						RestartRecording();
					}
				}
			}
		}

		public bool LoopAudioClip
		{
			get
			{
				return loopAudioClip;
			}
			set
			{
				if (loopAudioClip == value)
				{
					return;
				}
				loopAudioClip = value;
				if (RecordingEnabled && SourceType == InputSourceType.AudioClip)
				{
					if (inputSource is AudioClipWrapper audioClipWrapper)
					{
						audioClipWrapper.Loop = value;
					}
					else
					{
						base.Logger.Log(LogLevel.Error, "Unexpected: Recorder inputSource is not of AudioClipWrapper type or is null.");
					}
				}
			}
		}

		public SamplingRate SamplingRate
		{
			get
			{
				return samplingRate;
			}
			set
			{
				if (samplingRate != value)
				{
					samplingRate = value;
					base.Logger.Log(LogLevel.Info, "Recorder.SamplingRate changed");
					RestartRecording();
				}
			}
		}

		public OpusCodec.FrameDuration FrameDuration
		{
			get
			{
				return frameDuration;
			}
			set
			{
				if (frameDuration != value)
				{
					frameDuration = value;
					base.Logger.Log(LogLevel.Info, "Recorder.FrameDuration changed");
					RestartRecording();
				}
			}
		}

		public int Bitrate
		{
			get
			{
				return bitrate;
			}
			set
			{
				if (bitrate != value)
				{
					if (value < 6000 || value > 510000)
					{
						base.Logger.Log(LogLevel.Error, "Unsupported bitrate value {0}, valid range: {1}-{2}", value, 6000, 510000);
					}
					else
					{
						bitrate = value;
						base.Logger.Log(LogLevel.Info, "Recorder.Bitrate changed");
						RestartRecording();
					}
				}
			}
		}

		public bool RecordingEnabled
		{
			get
			{
				return recordingEnabled;
			}
			set
			{
				if (recordingEnabled != value)
				{
					recordingEnabled = value;
					if (recordingEnabled)
					{
						RestartRecording();
					}
					else
					{
						StopRecording();
					}
				}
			}
		}

		public bool StopRecordingWhenPaused
		{
			get
			{
				return stopRecordingWhenPaused;
			}
			set
			{
				stopRecordingWhenPaused = value;
			}
		}

		public bool UseOnAudioFilterRead
		{
			get
			{
				return useOnAudioFilterRead;
			}
			set
			{
				if (useOnAudioFilterRead != value)
				{
					useOnAudioFilterRead = value;
					base.Logger.Log(LogLevel.Info, "Recorder.UseOnAudioFilterRead changed");
					if (SourceType == InputSourceType.Microphone && MicrophoneType == MicType.Unity)
					{
						RestartRecording();
					}
				}
			}
		}

		public bool UseMicrophoneTypeFallback
		{
			get
			{
				return useMicrophoneTypeFallback;
			}
			set
			{
				useMicrophoneTypeFallback = value;
			}
		}

		public bool RecordWhenJoined
		{
			get
			{
				return recordWhenJoined;
			}
			set
			{
				recordWhenJoined = value;
			}
		}

		public DeviceInfo MicrophoneDevice
		{
			get
			{
				return microphoneDevice;
			}
			set
			{
				if (microphoneDevice != value)
				{
					microphoneDevice = value;
					base.Logger.Log(LogLevel.Info, "Recorder.MicrophoneDevice changed");
					if (SourceType == InputSourceType.Microphone)
					{
						RestartRecording();
					}
				}
			}
		}

		public bool AndroidMicrophoneAGC => androidMicrophoneSettings.EnableAGC;

		public bool AndroidMicrophoneAEC => androidMicrophoneSettings.EnableAEC;

		public bool AndroidMicrophoneNS => androidMicrophoneSettings.EnableNS;

		internal void MicrophoneDeviceChangeDetected()
		{
			microphoneDeviceChangePending = 1;
		}

		internal bool Init(VoiceConnection connection)
		{
			if (!base.isActiveAndEnabled)
			{
				base.Logger.Log(LogLevel.Warning, "Recorder is disabled.");
				return false;
			}
			if (voiceConnection != null)
			{
				base.Logger.Log(LogLevel.Warning, "Recorder already initialized.");
				return false;
			}
			voiceConnection = connection;
			RestartRecording();
			return true;
		}

		internal bool Deinit(VoiceConnection connection)
		{
			StopRecording();
			voiceConnection = null;
			return true;
		}

		public bool RestartRecording()
		{
			if (RecordingEnabled)
			{
				restartRecordingPending = 1;
			}
			return RecordingEnabled;
		}

		public void VoiceDetectorCalibrate(int durationMs, Action<float> detectionEndedCallback = null)
		{
			if (voiceAudio == null)
			{
				return;
			}
			if (!TransmitEnabled)
			{
				base.Logger.Log(LogLevel.Warning, "Cannot start voice detection calibration when transmission is not enabled");
				return;
			}
			voiceAudio.VoiceDetectorCalibrate(durationMs, delegate
			{
				voiceDetectionThreshold = VoiceDetector.Threshold;
				if (detectionEndedCallback != null)
				{
					detectionEndedCallback(voiceDetectionThreshold);
				}
			});
		}

		private void StartRecording()
		{
			StopRecording();
			if (voiceConnection == null)
			{
				base.Logger.Log(LogLevel.Info, "Recording can't be started if Recorder is not initialized.");
				return;
			}
			base.Logger.Log(LogLevel.Info, "Starting recording");
			if (inputSource != null)
			{
				inputSource.Dispose();
				inputSource = null;
			}
			voice.RemoveSelf();
			voice = CreateLocalVoiceAudioAndSource();
			if (voice == LocalVoiceAudioDummy.Dummy)
			{
				base.Logger.Log(LogLevel.Error, "Local input source setup and voice stream creation failed. No recording or transmission will be happening. See previous error log messages for more details.");
				if (inputSource != null)
				{
					inputSource.Dispose();
					inputSource = null;
				}
				return;
			}
			if (VoiceDetector != null)
			{
				VoiceDetector.Threshold = voiceDetectionThreshold;
				VoiceDetector.ActivityDelayMs = voiceDetectionDelayMs;
				VoiceDetector.On = voiceDetection;
			}
			SendPhotonVoiceCreatedMessage();
			voice.TransmitEnabled = TransmitEnabled;
		}

		private void StopRecording()
		{
			base.Logger.Log(LogLevel.Info, "Stopping recording");
			if (voice != LocalVoiceAudioDummy.Dummy)
			{
				voice.RemoveSelf();
				voice = LocalVoiceAudioDummy.Dummy;
				base.gameObject.SendMessage("PhotonVoiceRemoved", SendMessageOptions.DontRequireReceiver);
			}
			if (inputSource != null)
			{
				inputSource.Dispose();
				inputSource = null;
			}
		}

		public bool SetIosAudioSessionParameters(AudioSessionParameters asp)
		{
			return SetIosAudioSessionParameters(asp.Category, asp.Mode, asp.CategoryOptions);
		}

		public bool SetIosAudioSessionParameters(AudioSessionCategory category, AudioSessionMode mode, AudioSessionCategoryOption[] options)
		{
			int num = 0;
			if (options != null)
			{
				for (int i = 0; i < options.Length; i++)
				{
					num |= (int)options[i];
				}
			}
			if (audioSessionParameters.Category != category || audioSessionParameters.Mode != mode || audioSessionParameters.CategoryOptionsToInt() != num)
			{
				audioSessionParameters.Category = category;
				audioSessionParameters.Mode = mode;
				audioSessionParameters.CategoryOptions = options;
				base.Logger.Log(LogLevel.Info, "Recorder.iOSAudioSessionParameters changed to {0}", audioSessionParameters);
				if (SourceType == InputSourceType.Microphone && MicrophoneType == MicType.Photon)
				{
					RestartRecording();
				}
				return true;
			}
			return false;
		}

		public bool SetAndroidNativeMicrophoneSettings(bool aec = false, bool agc = false, bool ns = false)
		{
			if (androidMicrophoneSettings.EnableAEC != aec || androidMicrophoneSettings.EnableAGC != agc || androidMicrophoneSettings.EnableNS != ns)
			{
				androidMicrophoneSettings.EnableAEC = aec;
				androidMicrophoneSettings.EnableAGC = agc;
				androidMicrophoneSettings.EnableNS = ns;
				base.Logger.Log(LogLevel.Info, "Recorder.nativeAndroidMicrophoneSettings changed to aec = {0}, agc = {1}, ns = {2}", aec, agc, ns);
				if (SourceType == InputSourceType.Microphone && MicrophoneType == MicType.Photon)
				{
					RestartRecording();
				}
				return true;
			}
			return false;
		}

		public bool ResetLocalAudio()
		{
			if (inputSource != null && inputSource is IResettable)
			{
				base.Logger.Log(LogLevel.Info, "Resetting local audio.");
				(inputSource as IResettable).Reset();
				return true;
			}
			base.Logger.Log(LogLevel.Debug, "InputSource is null or not resettable.");
			return false;
		}

		private LocalVoice CreateLocalVoiceAudioAndSource()
		{
			_ = Application.platform;
			_ = 17;
			int suggestedFrequency = (int)samplingRate;
			bool flag;
			object otherParams;
			DeviceInfo deviceInfo;
			DeviceInfo deviceInfo2;
			switch (SourceType)
			{
			case InputSourceType.Microphone:
			{
				flag = false;
				MicType micType = MicrophoneType;
				if (micType == MicType.Unity)
				{
					goto IL_004c;
				}
				if (micType != MicType.Photon)
				{
					base.Logger.Log(LogLevel.Error, "unknown MicrophoneType value {0}", MicrophoneType);
					return LocalVoiceAudioDummy.Dummy;
				}
				goto IL_012a;
			}
			case InputSourceType.AudioClip:
			{
				if ((object)AudioClip == null)
				{
					base.Logger.Log(LogLevel.Error, "AudioClip property must be set for AudioClip audio source");
					return LocalVoiceAudioDummy.Dummy;
				}
				AudioClipWrapper audioClipWrapper = new AudioClipWrapper(AudioClip);
				audioClipWrapper.Loop = LoopAudioClip;
				inputSource = audioClipWrapper;
				break;
			}
			case InputSourceType.Factory:
				if (InputFactory == null)
				{
					base.Logger.Log(LogLevel.Warning, "Recorder.Source is Factory but Recorder.InputFactory is not set. Setting it to ToneAudioReader.");
					InputFactory = () => new AudioUtil.ToneAudioPusher<float>();
				}
				inputSource = InputFactory();
				if (inputSource.Error != null)
				{
					base.Logger.Log(LogLevel.Error, "InputFactory creation failure: {0}.", inputSource.Error);
				}
				break;
			default:
				{
					base.Logger.Log(LogLevel.Error, "unknown Source value {0}", SourceType);
					return LocalVoiceAudioDummy.Dummy;
				}
				IL_012a:
				otherParams = null;
				deviceInfo = (flag ? DeviceInfo.Default : MicrophoneDevice);
				base.Logger.Log(LogLevel.Info, "Setting recorder's source to Photon microphone device={0}", deviceInfo);
				switch (Application.platform)
				{
				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
					base.Logger.Log(LogLevel.Info, "Setting recorder's source to WindowsAudioInPusher");
					break;
				case RuntimePlatform.MetroPlayerX86:
				case RuntimePlatform.MetroPlayerX64:
				case RuntimePlatform.MetroPlayerARM:
					base.Logger.Log(LogLevel.Info, "Setting recorder's source to UWP.AudioInPusher");
					break;
				case RuntimePlatform.IPhonePlayer:
					otherParams = audioSessionParameters;
					base.Logger.Log(LogLevel.Info, "Setting recorder's source to IOS.AudioInPusher with session {0}", audioSessionParameters);
					break;
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer:
					base.Logger.Log(LogLevel.Info, "Setting recorder's source to MacOS.AudioInPusher");
					break;
				case RuntimePlatform.Switch:
					base.Logger.Log(LogLevel.Info, "Setting recorder's source to Switch.AudioInPusher");
					break;
				case RuntimePlatform.Android:
					otherParams = androidMicrophoneSettings;
					base.Logger.Log(LogLevel.Info, "Setting recorder's source to UnityAndroidAudioInAEC");
					break;
				case RuntimePlatform.WebGLPlayer:
					base.Logger.Log(LogLevel.Info, "Setting recorder's source to Unity.WebAudioMicIn");
					break;
				default:
					base.Logger.Log(LogLevel.Error, "Photon microphone type is not supported for the current platform {0}", Application.platform);
					break;
				}
				inputSource = Platform.CreateDefaultAudioSource(base.Logger, deviceInfo, suggestedFrequency, 1, otherParams);
				if (inputSource != null)
				{
					if (inputSource.Error == null)
					{
						break;
					}
					base.Logger.Log(LogLevel.Error, "Photon microphone input source creation failure: {0}", inputSource.Error);
				}
				if (!UseMicrophoneTypeFallback || flag)
				{
					break;
				}
				flag = true;
				base.Logger.Log(LogLevel.Error, "Photon microphone failed. Falling back to Unity microphone");
				goto IL_004c;
				IL_004c:
				deviceInfo2 = (flag ? DeviceInfo.Default : MicrophoneDevice);
				base.Logger.Log(LogLevel.Info, "Setting recorder's source to Unity microphone device {0}", deviceInfo2);
				if (UseOnAudioFilterRead)
				{
					inputSource = new MicWrapperPusher(base.gameObject, deviceInfo2.IDString, suggestedFrequency, base.Logger);
				}
				else
				{
					inputSource = new MicWrapper(deviceInfo2.IDString, suggestedFrequency, base.Logger);
				}
				if (inputSource != null)
				{
					if (inputSource.Error == null)
					{
						break;
					}
					base.Logger.Log(LogLevel.Error, "Unity microphone input source creation failure: {0}", inputSource.Error);
				}
				if (!UseMicrophoneTypeFallback || flag)
				{
					break;
				}
				flag = true;
				base.Logger.Log(LogLevel.Error, "Unity microphone failed. Falling back to Photon microphone");
				goto IL_012a;
			}
			if (inputSource == null || inputSource.Error != null)
			{
				return LocalVoiceAudioDummy.Dummy;
			}
			if (inputSource.Channels == 0)
			{
				base.Logger.Log(LogLevel.Error, "inputSource.Channels is zero");
				return LocalVoiceAudioDummy.Dummy;
			}
			VoiceInfo voiceInfo = VoiceInfo.CreateAudioOpus(samplingRate, inputSource.Channels, frameDuration, Bitrate, UserData);
			AudioSampleType st = AudioSampleType.Source;
			WebRtcAudioDsp component = GetComponent<WebRtcAudioDsp>();
			if (null != component)
			{
				component.AdjustVoiceInfo(ref voiceInfo, ref st);
			}
			VoiceCreateOptions options = new VoiceCreateOptions
			{
				InterestGroup = interestGroup,
				TargetPlayers = (useTargetPlayers ? targetPlayers : null),
				DebugEchoMode = debugEchoMode,
				Encrypt = encrypt,
				Reliable = reliableMode
			};
			return voiceConnection.VoiceClient.CreateLocalVoiceAudioFromSource(voiceInfo, inputSource, st, 1, options);
		}

		protected virtual void SendPhotonVoiceCreatedMessage()
		{
			base.gameObject.SendMessage("PhotonVoiceCreated", new PhotonVoiceCreatedParams
			{
				Voice = voice,
				AudioDesc = inputSource
			}, SendMessageOptions.DontRequireReceiver);
		}

		protected void Update()
		{
			if (!(voiceConnection == null))
			{
				if (Interlocked.Exchange(ref microphoneDeviceChangePending, 0) != 0)
				{
					HandleDeviceChange();
				}
				if (Interlocked.Exchange(ref restartRecordingPending, 0) != 0 && RecordingEnabled)
				{
					base.Logger.Log(LogLevel.Info, "Restarting recording");
					StartRecording();
				}
			}
		}

		private void OnDestroy()
		{
			if (!(voiceConnection == null))
			{
				base.Logger.Log(LogLevel.Info, "Recorder is about to be destroyed, removing local voice.");
				StopRecording();
				voiceConnection.RemoveRecorder(this);
			}
		}

		private void HandleDeviceChange()
		{
			if (RecordingEnabled && SourceType == InputSourceType.Microphone)
			{
				if (ResetLocalAudio())
				{
					base.Logger.Log(LogLevel.Info, "Local audio reset as a result of audio config/device change.");
					return;
				}
				base.Logger.Log(LogLevel.Info, "Restarting Recording as a result of audio config/device change.");
				RestartRecording();
			}
		}

		private void OnApplicationPause(bool paused)
		{
			if (!(voiceConnection == null))
			{
				base.Logger.Log(LogLevel.Debug, "OnApplicationPause({0})", paused);
				HandleApplicationPause(paused);
			}
		}

		private void OnApplicationFocus(bool focused)
		{
			if (!(voiceConnection == null))
			{
				base.Logger.Log(LogLevel.Debug, "OnApplicationFocus({0})", focused);
				HandleApplicationPause(!focused);
			}
		}

		private void HandleApplicationPause(bool paused)
		{
			base.Logger.Log(LogLevel.Info, "App paused?= {0}, isPausedOrInBackground = {1}, StopRecordingWhenPaused = {2}, RecordingEnabled = {3}", paused, isPausedOrInBackground, StopRecordingWhenPaused, RecordingEnabled);
			if (isPausedOrInBackground == paused)
			{
				return;
			}
			if (paused)
			{
				isPausedOrInBackground = true;
				if (StopRecordingWhenPaused && RecordingEnabled)
				{
					base.Logger.Log(LogLevel.Info, "Stopping recording as application went to background or paused");
					StopRecording();
				}
				return;
			}
			if (!StopRecordingWhenPaused)
			{
				if (ResetLocalAudio())
				{
					base.Logger.Log(LogLevel.Info, "Local audio reset as application is back from background or unpaused");
				}
			}
			else if (RecordingEnabled)
			{
				base.Logger.Log(LogLevel.Info, "Starting recording as application is back from background or unpaused");
				RestartRecording();
			}
			isPausedOrInBackground = false;
		}
	}
}
