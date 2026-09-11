using System;
using UnityEngine;

namespace Photon.Voice.Unity.UtilityScripts
{
	public class MicrophonePermission : VoiceComponent
	{
		private bool hasPermission;

		[SerializeField]
		private bool autoStart = true;

		public bool HasPermission
		{
			get
			{
				return hasPermission;
			}
			private set
			{
				base.Logger.LogInfo("Microphone Permission Granted: {0}", value);
				MicrophonePermission.MicrophonePermissionCallback?.Invoke(value);
				if (hasPermission == value)
				{
					return;
				}
				hasPermission = value;
				if (!hasPermission || !autoStart)
				{
					return;
				}
				Recorder component = GetComponent<Recorder>();
				if (component != null)
				{
					if (!component.RecordingEnabled)
					{
						base.Logger.LogInfo("Starting recording automatically");
					}
					component.RecordingEnabled = true;
				}
				else
				{
					base.Logger.LogInfo("Recorder not found. Assign MicrophonePermission to an object with Recorder to automatically start recording");
				}
			}
		}

		public static event Action<bool> MicrophonePermissionCallback;

		protected override void Awake()
		{
			base.Awake();
			InitVoice();
		}

		public void InitVoice()
		{
			HasPermission = true;
		}
	}
}
