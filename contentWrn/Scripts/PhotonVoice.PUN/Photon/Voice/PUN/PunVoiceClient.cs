using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;

namespace Photon.Voice.PUN
{
	[AddComponentMenu("Photon Voice/PUN/Pun Voice Client")]
	[HelpURL("https://doc.photonengine.com/en-us/voice/v2/getting-started/voice-for-pun")]
	public class PunVoiceClient : VoiceFollowClient
	{
		public const string VoiceRoomNameSuffix = "_voice_";

		private static PunVoiceClient instance;

		[SerializeField]
		private bool usePunAppSettings = true;

		[SerializeField]
		private bool usePunAuthValues = true;

		protected override bool LeaderInRoom => PhotonNetwork.InRoom;

		protected override bool LeaderOfflineMode => PhotonNetwork.OfflineMode;

		public static PunVoiceClient Instance
		{
			get
			{
				if (instance == null)
				{
					PunVoiceClient[] array = Object.FindObjectsOfType<PunVoiceClient>();
					if (array == null || array.Length < 1)
					{
						instance = new GameObject
						{
							name = "PunVoiceClient"
						}.AddComponent<PunVoiceClient>();
						instance.Logger.LogError("PunVoiceClient component was not found in the scene. Creating PunVoiceClient object.");
					}
					else if (array.Length >= 1)
					{
						instance = array[0];
						instance.Logger.LogInfo("An instance of PunVoiceClient is found in the scene.");
						if (array.Length > 1)
						{
							instance.Logger.LogError("{0} instances of PunVoiceClient found in the scene. Using a random instance.", array.Length);
						}
					}
					instance.Logger.LogInfo("PunVoiceClient singleton instance is now set.");
				}
				return instance;
			}
		}

		public bool UsePunAppSettings
		{
			get
			{
				return usePunAppSettings;
			}
			set
			{
				usePunAppSettings = value;
			}
		}

		public bool UsePunAuthValues
		{
			get
			{
				return usePunAuthValues;
			}
			set
			{
				usePunAuthValues = value;
			}
		}

		protected override void Start()
		{
			PhotonNetwork.NetworkingClient.StateChanged += OnPunStateChange;
			base.Start();
			if (Instance == this && base.UsePrimaryRecorder)
			{
				if (base.PrimaryRecorder != null)
				{
					AddRecorder(base.PrimaryRecorder);
				}
				else
				{
					base.Logger.LogError("Primary Recorder is not set.");
				}
			}
		}

		protected override void OnDestroy()
		{
			PhotonNetwork.NetworkingClient.StateChanged -= OnPunStateChange;
			base.OnDestroy();
			if (instance == this)
			{
				instance.Logger.LogInfo("PunVoiceClient singleton instance is being reset because destroyed.");
				instance = null;
			}
		}

		protected override Speaker InstantiateSpeakerForRemoteVoice(int playerId, byte voiceId, object userData)
		{
			if (userData == null)
			{
				base.Logger.LogInfo("Creating Speaker for remote voice p#{0} v#{1} PunVoiceClient Primary Recorder (userData == null).", playerId, voiceId);
				return InstantiateSpeakerPrefab(base.gameObject, destroyOnRemove: true);
			}
			if (!(userData is int))
			{
				base.Logger.LogWarning("UserData ({0}) does not contain PhotonViewId. Remote voice p#{1} v#{2} not linked. Do you have a Recorder not used with a PhotonVoiceView? is this expected?", (userData == null) ? "null" : userData.ToString(), playerId, voiceId);
				return null;
			}
			PhotonView photonView = PhotonView.Find((int)userData);
			if (null == photonView || !photonView)
			{
				base.Logger.LogWarning("No PhotonView with ID {0} found. Remote voice p#{1} v#{2} not linked.", userData, playerId, voiceId);
				return null;
			}
			PhotonVoiceView component = photonView.GetComponent<PhotonVoiceView>();
			if (null == component || !component)
			{
				base.Logger.LogWarning("No PhotonVoiceView attached to the PhotonView with ID {0}. Remote voice p#{1} v#{2} not linked.", userData, playerId, voiceId);
				return null;
			}
			base.Logger.LogInfo("Using PhotonVoiceView {0} Speaker for remote voice p#{1} v#{2}.", userData, playerId, voiceId);
			return component.SpeakerInUse;
		}

		protected override string GetVoiceRoomName()
		{
			if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null && !string.IsNullOrEmpty(PhotonNetwork.CurrentRoom.Name))
			{
				return PhotonNetwork.CurrentRoom.Name + "_voice_";
			}
			return null;
		}

		private void OnPunStateChange(ClientState s1, ClientState s2)
		{
			LeaderStateChanged(s2);
		}

		protected override bool ConnectVoice()
		{
			AppSettings appSettings = null;
			if (usePunAppSettings)
			{
				appSettings = new AppSettings();
				appSettings = PhotonNetwork.PhotonServerSettings.AppSettings.CopyTo(appSettings);
				if (!string.IsNullOrEmpty(PhotonNetwork.CloudRegion))
				{
					appSettings.FixedRegion = PhotonNetwork.CloudRegion;
				}
				base.Client.SerializationProtocol = PhotonNetwork.NetworkingClient.SerializationProtocol;
			}
			if (UsePunAuthValues)
			{
				if (PhotonNetwork.AuthValues != null)
				{
					if (base.Client.AuthValues == null)
					{
						base.Client.AuthValues = new AuthenticationValues();
					}
					base.Client.AuthValues = PhotonNetwork.AuthValues.CopyTo(base.Client.AuthValues);
				}
				base.Client.AuthMode = PhotonNetwork.NetworkingClient.AuthMode;
				base.Client.EncryptionMode = PhotonNetwork.NetworkingClient.EncryptionMode;
			}
			return ConnectUsingSettings(appSettings);
		}
	}
}
