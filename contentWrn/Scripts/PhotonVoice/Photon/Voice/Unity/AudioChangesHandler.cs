using UnityEngine;

namespace Photon.Voice.Unity
{
	[RequireComponent(typeof(Recorder))]
	[AddComponentMenu("Photon Voice/Audio Changes Handler")]
	[DisallowMultipleComponent]
	public class AudioChangesHandler : VoiceComponent
	{
		private IAudioInChangeNotifier photonMicChangeNotifier;

		private Recorder recorder;

		[Tooltip("React to device change notification when Recorder is started.")]
		public bool HandleDeviceChange = true;

		[Tooltip("iOS: React to device change notification when Recorder is started.")]
		public bool HandleDeviceChangeIOS;

		[Tooltip("Android: React to device change notification when Recorder is started.")]
		public bool HandleDeviceChangeAndroid;

		protected override void Awake()
		{
			base.Awake();
			recorder = GetComponent<Recorder>();
			base.Logger.LogInfo("Subscribing to system (audio) changes.");
			photonMicChangeNotifier = Platform.CreateAudioInChangeNotifier(PhotonMicrophoneChangeDetected, base.Logger);
			if (photonMicChangeNotifier.IsSupported)
			{
				if (photonMicChangeNotifier.Error == null)
				{
					base.Logger.LogInfo("Subscribed to audio in change notifications via Photon plugin.");
					return;
				}
				base.Logger.LogError("Error creating instance of photonMicChangeNotifier: {0}", photonMicChangeNotifier.Error);
			}
			else
			{
				base.Logger.LogInfo("Skipped subscribing to audio change notifications via Photon's AudioInChangeNotifier as not supported on current platform: {0}", Application.platform);
				AudioSettings.OnAudioConfigurationChanged += OnAudioConfigChanged;
				base.Logger.LogInfo("Subscribed to audio configuration changes via Unity OnAudioConfigurationChanged callback.");
			}
		}

		private void OnDestroy()
		{
			if (photonMicChangeNotifier != null)
			{
				photonMicChangeNotifier.Dispose();
				photonMicChangeNotifier = null;
				base.Logger.LogInfo("Unsubscribed from audio in change notifications via Photon plugin.");
			}
			AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigChanged;
			base.Logger.LogInfo("Unsubscribed from audio in change notifications via Unity OnAudioConfigurationChanged callback.");
		}

		private void PhotonMicrophoneChangeDetected()
		{
			base.Logger.LogInfo("Microphones change detected by Photon native plugin.");
			OnDeviceChange();
		}

		private void OnDeviceChange()
		{
			bool flag = false;
			if (Application.platform switch
			{
				RuntimePlatform.IPhonePlayer => HandleDeviceChangeIOS, 
				RuntimePlatform.Android => HandleDeviceChangeAndroid, 
				_ => HandleDeviceChange, 
			})
			{
				recorder.MicrophoneDeviceChangeDetected();
				base.Logger.LogInfo("Device change detected and the recording will be restarted.");
			}
			else
			{
				base.Logger.LogInfo("Device change detected but its handling is disabled.");
			}
		}

		private void OnAudioConfigChanged(bool deviceWasChanged)
		{
			base.Logger.LogInfo("OnAudioConfigurationChanged: {0}", deviceWasChanged ? "Device was changed." : "AudioSettings.Reset was called.");
			OnDeviceChange();
		}
	}
}
