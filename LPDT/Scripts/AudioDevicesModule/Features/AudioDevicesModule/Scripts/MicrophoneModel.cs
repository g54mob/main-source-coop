using System;
using System.Collections.Generic;
using Photon.Voice.Unity;
using WebSocketSharp;

namespace Features.AudioDevicesModule.Scripts
{
	public class MicrophoneModel
	{
		private bool _isMicrophoneEnabled;

		private bool _isNoiseSuppressionEnabled = true;

		private bool _isPushToTalkEnabled;

		private int _currentMicrophoneId = -1;

		private string _currentMicrophoneName;

		public bool IsMicrophoneEnabled
		{
			get
			{
				return _isMicrophoneEnabled;
			}
			set
			{
				_isMicrophoneEnabled = value;
				this.OnMicrophoneEnabledChanged?.Invoke();
			}
		}

		public bool IsNoiseSuppressionEnabled
		{
			get
			{
				return _isNoiseSuppressionEnabled;
			}
			set
			{
				_isNoiseSuppressionEnabled = value;
				this.OnNoiseSuppressionEnabledChanged?.Invoke();
			}
		}

		public bool IsPushToTalkEnabled
		{
			get
			{
				return _isPushToTalkEnabled;
			}
			set
			{
				_isPushToTalkEnabled = value;
				this.OnPushToTalkEnabledChanged?.Invoke();
			}
		}

		public int CurrentMicrophoneId
		{
			get
			{
				return _currentMicrophoneId;
			}
			set
			{
				_currentMicrophoneId = value;
			}
		}

		public string CurrentMicrophoneName
		{
			get
			{
				return _currentMicrophoneName;
			}
			set
			{
				_currentMicrophoneName = value;
				this.OnMicrophoneNameEnabledChanged?.Invoke(_currentMicrophoneName);
			}
		}

		public Recorder PlayerRecorder { get; private set; }

		public Dictionary<string, int> MicrophonesDriverID { get; set; } = new Dictionary<string, int>();

		public event Action OnMicrophoneEnabledChanged;

		public event Action OnNoiseSuppressionEnabledChanged;

		public event Action OnPushToTalkEnabledChanged;

		public event Action<string> OnMicrophoneNameEnabledChanged;

		public event Action OnRecorderRegistered;

		public event Action OnCaptureDeviceListRefreshRequested;

		public void RequestCaptureDeviceListRefresh()
		{
			this.OnCaptureDeviceListRefreshRequested?.Invoke();
		}

		public void SetMicrophoneName(string micName)
		{
			if (!micName.IsNullOrEmpty())
			{
				CurrentMicrophoneName = micName;
			}
		}

		public void RegisterRecorder(Recorder recorder)
		{
			PlayerRecorder = recorder;
			this.OnRecorderRegistered?.Invoke();
		}

		public void RestartRecording()
		{
			if (!(PlayerRecorder == null))
			{
				PlayerRecorder.RecordingEnabled = !PlayerRecorder.RecordingEnabled;
				PlayerRecorder.RecordingEnabled = !PlayerRecorder.RecordingEnabled;
			}
		}

		public void UnRegisterRecorder()
		{
			PlayerRecorder = null;
		}
	}
}
