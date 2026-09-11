using System;
using Photon.Realtime;
using UnityEngine;

namespace Photon.Voice
{
	[Serializable]
	public class PhotonAppSettings : ScriptableObject
	{
		[Tooltip("Core Photon Server/Cloud settings.")]
		public AppSettings AppSettings;

		private static PhotonAppSettings instance;

		private const string SettingsFileName = "VoiceAppSettings";

		private const string PhotonVoiceFolderGUID = "d3a9df3027b4a45679a2a3e978dde78e";

		public static PhotonAppSettings Instance
		{
			get
			{
				if (instance == null)
				{
					LoadOrCreateSettings();
				}
				return instance;
			}
		}

		public void UseCloud(string cloudAppid, string code = "")
		{
			AppSettings.AppIdRealtime = cloudAppid;
			AppSettings.Server = null;
			AppSettings.FixedRegion = (string.IsNullOrEmpty(code) ? null : code);
		}

		public static void LoadOrCreateSettings()
		{
			instance = (PhotonAppSettings)Resources.Load("VoiceAppSettings", typeof(PhotonAppSettings));
			if (!(instance != null) && instance == null)
			{
				instance = (PhotonAppSettings)ScriptableObject.CreateInstance(typeof(PhotonAppSettings));
				if (instance == null)
				{
					Debug.LogError("Failed to create ServerSettings. PUN is unable to run this way. If you deleted it from the project, reload the Editor.");
				}
			}
		}

		public override string ToString()
		{
			return "VoiceAppSettings: " + AppSettings.ToStringFull();
		}
	}
}
