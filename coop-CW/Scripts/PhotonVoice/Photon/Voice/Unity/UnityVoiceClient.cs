using Photon.Realtime;
using UnityEngine;

namespace Photon.Voice.Unity
{
	[AddComponentMenu("Photon Voice/Unity Voice Client")]
	[HelpURL("https://doc.photonengine.com/en-us/voice/v2/getting-started/voice-intro")]
	public class UnityVoiceClient : VoiceConnection
	{
		[SerializeField]
		public bool UseVoiceAppSettings;

		public override bool AlwaysUsePrimaryRecorder => true;

		protected virtual void Start()
		{
			if (base.PrimaryRecorder != null)
			{
				AddRecorder(base.PrimaryRecorder);
			}
		}

		public override bool ConnectUsingSettings(AppSettings overwriteSettings = null)
		{
			if (overwriteSettings != null)
			{
				return base.ConnectUsingSettings(overwriteSettings);
			}
			if (UseVoiceAppSettings)
			{
				return base.ConnectUsingSettings(PhotonAppSettings.Instance.AppSettings);
			}
			return base.ConnectUsingSettings();
		}

		protected override Speaker InstantiateSpeakerForRemoteVoice(int playerId, byte voiceId, object userData)
		{
			return InstantiateSpeakerPrefab(base.gameObject, destroyOnRemove: true);
		}
	}
}
