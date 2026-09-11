using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;

namespace Photon.Voice.PUN.UtilityScripts
{
	[RequireComponent(typeof(PhotonVoiceView))]
	public class VoiceDebugScript : MonoBehaviourPun
	{
		private PhotonVoiceView photonVoiceView;

		public bool ForceRecordingAndTransmission;

		public AudioClip TestAudioClip;

		public bool TestUsingAudioClip;

		public bool DisableVad;

		public bool IncreaseLogLevels;

		public bool LocalDebug;

		private void Awake()
		{
			photonVoiceView = GetComponent<PhotonVoiceView>();
		}

		private void Update()
		{
			MaxLogs();
			if (!(photonVoiceView.RecorderInUse != null))
			{
				return;
			}
			if (TestUsingAudioClip)
			{
				if ((object)TestAudioClip == null || !TestAudioClip)
				{
					Debug.LogError("Set an AudioClip first");
				}
				else
				{
					photonVoiceView.RecorderInUse.SourceType = Recorder.InputSourceType.AudioClip;
					photonVoiceView.RecorderInUse.AudioClip = TestAudioClip;
					photonVoiceView.RecorderInUse.LoopAudioClip = true;
					photonVoiceView.RecorderInUse.RestartRecording();
				}
			}
			if (ForceRecordingAndTransmission)
			{
				photonVoiceView.RecorderInUse.RecordingEnabled = true;
				photonVoiceView.RecorderInUse.TransmitEnabled = true;
			}
			if (DisableVad)
			{
				photonVoiceView.RecorderInUse.VoiceDetection = false;
			}
		}

		[ContextMenu("CantHearYou")]
		public void CantHearYou()
		{
			if (!PunVoiceClient.Instance.Client.InRoom)
			{
				Debug.LogError("local voice client is not joined to a voice room");
				return;
			}
			if (!photonVoiceView.SpeakerInUse.IsLinked)
			{
				Debug.LogError("locally speaker not linked, trying late linking & asking anyway");
			}
			base.photonView.RPC("CantHearYou", base.photonView.Owner, PunVoiceClient.Instance.Client.CurrentRoom.Name, PunVoiceClient.Instance.Client.LoadBalancingPeer.ServerIpAddress, PunVoiceClient.Instance.Client.AppVersion);
		}

		[PunRPC]
		private void CantHearYou(string roomName, string serverIp, string appVersion, PhotonMessageInfo photonMessageInfo)
		{
			string why;
			if (!PunVoiceClient.Instance.Client.InRoom)
			{
				why = "voice client not in a room";
			}
			else if (!PunVoiceClient.Instance.Client.CurrentRoom.Name.Equals(roomName))
			{
				why = $"voice client is on another room {PunVoiceClient.Instance.Client.CurrentRoom.Name} != {roomName}";
			}
			else if (!PunVoiceClient.Instance.Client.LoadBalancingPeer.ServerIpAddress.Equals(serverIp))
			{
				why = $"voice client is on another server {PunVoiceClient.Instance.Client.LoadBalancingPeer.ServerIpAddress} != {serverIp}, maybe different Photon Cloud regions";
			}
			else if (!PunVoiceClient.Instance.Client.AppVersion.Equals(appVersion))
			{
				why = $"voice client uses different AppVersion {PunVoiceClient.Instance.Client.AppVersion} != {appVersion}";
			}
			else if (photonVoiceView.RecorderInUse == null)
			{
				why = "recorder not setup (yet?)";
			}
			else if (!photonVoiceView.RecorderInUse.RecordingEnabled)
			{
				why = "recorder is not recording";
				photonVoiceView.RecorderInUse.RecordingEnabled = true;
			}
			else if (!photonVoiceView.RecorderInUse.TransmitEnabled)
			{
				why = "recorder is not transmitting";
				photonVoiceView.RecorderInUse.TransmitEnabled = true;
			}
			else if (photonVoiceView.RecorderInUse.InterestGroup != 0)
			{
				why = "recorder.InterestGroup is not zero? is this on purpose? switching it back to zero";
				photonVoiceView.RecorderInUse.InterestGroup = 0;
			}
			else if (!(photonVoiceView.RecorderInUse.UserData is int) || (int)photonVoiceView.RecorderInUse.UserData != base.photonView.ViewID)
			{
				why = $"recorder.UserData ({photonVoiceView.RecorderInUse.UserData}) != photonView.ViewID ({base.photonView.ViewID}), fixing it now";
				photonVoiceView.RecorderInUse.UserData = base.photonView.ViewID;
				photonVoiceView.RecorderInUse.RestartRecording();
			}
			else if (photonVoiceView.RecorderInUse.VoiceDetection && DisableVad)
			{
				why = "recorder vad is enabled, disable it for testing";
				photonVoiceView.RecorderInUse.VoiceDetection = false;
			}
			else if (base.photonView.OwnerActorNr == photonMessageInfo.Sender.ActorNumber)
			{
				if (LocalDebug)
				{
					if (photonVoiceView.SpeakerInUse != null)
					{
						why = "no idea why!, should be working (1)";
						photonVoiceView.RecorderInUse.RestartRecording();
					}
					else if (!photonVoiceView.RecorderInUse.DebugEchoMode)
					{
						why = "recorder debug echo mode not enabled, enabling it now";
						photonVoiceView.RecorderInUse.DebugEchoMode = true;
					}
					else
					{
						why = "locally not speaker (yet?) (2)";
					}
				}
				else
				{
					why = "local object, are you trying to hear yourself? (feedback DebugEcho), LocalDebug is disabled, enable it if you want to diagnose this";
				}
			}
			else
			{
				why = "no idea why!, should be working (2)";
				photonVoiceView.RecorderInUse.RestartRecording();
			}
			Reply(why, photonMessageInfo.Sender);
		}

		private void Reply(string why, Player player)
		{
			base.photonView.RPC("HeresWhy", player, why);
		}

		[PunRPC]
		private void HeresWhy(string why, PhotonMessageInfo photonMessageInfo)
		{
			Debug.LogErrorFormat("Player {0} replied to my CantHearYou message with {1}", photonMessageInfo.Sender, why);
		}

		private void MaxLogs()
		{
			if (IncreaseLogLevels)
			{
				VoiceLogger[] array = Object.FindObjectsOfType<VoiceLogger>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].LogLevel = DebugLevel.ALL;
				}
			}
		}
	}
}
