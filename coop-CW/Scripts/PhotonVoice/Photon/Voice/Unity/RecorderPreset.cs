using System;
using UnityEngine;

namespace Photon.Voice.Unity
{
	[AddComponentMenu("Photon Voice/Recorder Preset")]
	public class RecorderPreset : VoiceComponent
	{
		[Serializable]
		public struct DSP
		{
			[Tooltip("Acoustic Echo Cancellation")]
			public bool AEC;

			[Tooltip("Voice Activity Detection")]
			public bool VAD;
		}

		[Tooltip("On which platform to apply the filter.")]
		public RuntimePlatform Platform;

		[Tooltip("Which microphone API to use when the Source is set to Microphone.")]
		[Header("Overrides:")]
		public Recorder.MicType MicrophoneType;

		[Tooltip("Enable WebRtcAudioDsp component.")]
		public bool DSPEnabled;

		public DSP DSPSettings;

		protected override void Awake()
		{
			base.Awake();
			if (!base.enabled)
			{
				return;
			}
			Recorder component = GetComponent<Recorder>();
			WebRtcAudioDsp component2 = GetComponent<WebRtcAudioDsp>();
			if (Application.platform != Platform)
			{
				return;
			}
			if (component == null)
			{
				base.Logger.LogError("Can't find Recorder component");
				return;
			}
			base.Logger.LogInfo("Updating from preset for platform '{0}': Microphone Type = {1}, DSP Enabled = {2}", Application.platform, MicrophoneType, DSPEnabled);
			component.MicrophoneType = MicrophoneType;
			if (component2 == null)
			{
				base.Logger.LogError("Can't find WebRtcAudioDsp component");
				return;
			}
			component2.enabled = DSPEnabled;
			if (DSPEnabled)
			{
				base.Logger.LogInfo("Updating from preset for platform '{0}': DSP.AEC = {1}, DSP.VAD = {2}", Application.platform, DSPSettings.AEC, DSPSettings.VAD);
				component2.AEC = DSPSettings.AEC;
				component2.VAD = DSPSettings.VAD;
			}
		}
	}
}
