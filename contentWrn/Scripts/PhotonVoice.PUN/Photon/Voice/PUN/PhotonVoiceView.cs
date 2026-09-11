using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

namespace Photon.Voice.PUN
{
	[AddComponentMenu("Photon Voice/PUN/Photon Voice View")]
	[RequireComponent(typeof(PhotonView))]
	[HelpURL("https://doc.photonengine.com/en-us/voice/v2/getting-started/voice-for-pun")]
	public class PhotonVoiceView : VoiceComponent
	{
		private PhotonView photonView;

		private PunVoiceClient punVoiceClient;

		public Recorder RecorderInUse { get; private set; }

		public Speaker SpeakerInUse { get; private set; }

		public bool IsSpeaking
		{
			get
			{
				if (SpeakerInUse != null)
				{
					return SpeakerInUse.IsPlaying;
				}
				return false;
			}
		}

		public bool IsRecording
		{
			get
			{
				if (RecorderInUse != null)
				{
					return RecorderInUse.IsCurrentlyTransmitting;
				}
				return false;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			punVoiceClient = PunVoiceClient.Instance;
			photonView = GetComponent<PhotonView>();
		}

		private void Start()
		{
			if (photonView.IsMine)
			{
				SetupRecorder();
				if (RecorderInUse == null)
				{
					base.Logger.LogWarning("Recorder not setup for PhotonVoiceView: playback may not work properly.");
				}
				else
				{
					if (!RecorderInUse.TransmitEnabled)
					{
						base.Logger.LogWarning("PhotonVoiceView.RecorderInUse.TransmitEnabled is false, don't forget to set it to true to enable transmission.");
					}
					if (!RecorderInUse.isActiveAndEnabled)
					{
						base.Logger.LogWarning("PhotonVoiceView.RecorderInUse may not work properly if recorder is disabled or attached to an inactive GameObject.");
					}
				}
			}
			SetupSpeaker();
			if (SpeakerInUse == null)
			{
				base.Logger.LogWarning("Speaker not setup for PhotonVoiceView: voice chat will not work.");
			}
			else
			{
				punVoiceClient.AddSpeaker(SpeakerInUse, photonView.ViewID);
			}
		}

		private void SetupRecorder()
		{
			Recorder recorder = null;
			Recorder[] componentsInChildren = GetComponentsInChildren<Recorder>();
			if (componentsInChildren.Length != 0)
			{
				if (componentsInChildren.Length > 1)
				{
					base.Logger.LogWarning("Multiple Recorder components found attached to the GameObject or its children.");
				}
				recorder = componentsInChildren[0];
			}
			if (null == recorder && null != punVoiceClient.PrimaryRecorder)
			{
				recorder = punVoiceClient.PrimaryRecorder;
			}
			if (null == recorder)
			{
				base.Logger.LogWarning("Cannot find Recorder. Assign a Recorder to PhotonVoiceView object or set up PunVoiceClient.PrimaryRecorder.");
			}
			else
			{
				recorder.UserData = photonView.ViewID;
				punVoiceClient.AddRecorder(recorder);
			}
			RecorderInUse = recorder;
		}

		private void OnDestroy()
		{
			punVoiceClient.RemoveRecorder(RecorderInUse);
		}

		private void SetupSpeaker()
		{
			Speaker speaker = null;
			Speaker[] componentsInChildren = GetComponentsInChildren<Speaker>(includeInactive: true);
			if (componentsInChildren.Length != 0)
			{
				speaker = componentsInChildren[0];
				if (componentsInChildren.Length > 1)
				{
					base.Logger.LogWarning("Multiple Speaker components found attached to the GameObject or its children. Using the first one we found.");
				}
			}
			if (null == speaker && null != punVoiceClient.SpeakerPrefab)
			{
				speaker = punVoiceClient.InstantiateSpeakerPrefab(base.gameObject, destroyOnRemove: false);
			}
			if (null == speaker)
			{
				base.Logger.LogError("No Speaker component or prefab found. Assign a Speaker to PhotonVoiceView object or set up PunVoiceClient.SpeakerPrefab.");
			}
			else
			{
				base.Logger.LogInfo("Speaker instantiated.");
			}
			SpeakerInUse = speaker;
		}
	}
}
