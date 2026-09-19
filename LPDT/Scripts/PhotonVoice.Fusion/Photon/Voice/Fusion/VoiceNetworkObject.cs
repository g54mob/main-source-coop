using Fusion;
using Photon.Voice.Unity;
using UnityEngine;

namespace Photon.Voice.Fusion
{
	[AddComponentMenu("Photon Voice/Fusion/Voice Network Object")]
	[NetworkBehaviourWeaved(0)]
	public class VoiceNetworkObject : NetworkBehaviour
	{
		private VoiceComponentImpl voiceComponentImpl = new VoiceComponentImpl();

		private VoiceConnection voiceConnection;

		protected ILogger Logger => voiceComponentImpl.Logger;

		public VoiceLogger VoiceLogger => voiceComponentImpl.VoiceLogger;

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

		public bool IsLocal
		{
			get
			{
				if (base.Runner.Topology != Topologies.Shared)
				{
					return base.Object.HasInputAuthority;
				}
				return base.Object.HasStateAuthority;
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
					Logger.Log(LogLevel.Warning, "Multiple Recorder components found attached to the GameObject or its children.");
				}
				recorder = componentsInChildren[0];
			}
			if (null == recorder && null != voiceConnection.PrimaryRecorder)
			{
				recorder = voiceConnection.PrimaryRecorder;
			}
			if (null == recorder)
			{
				Logger.Log(LogLevel.Warning, "Cannot find Recorder. Assign a Recorder to VoiceNetworkObject object or set up FusionVoiceClient.PrimaryRecorder.");
			}
			else
			{
				recorder.UserData = GetUserData();
				voiceConnection.AddRecorder(recorder);
			}
			RecorderInUse = recorder;
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
					Logger.Log(LogLevel.Warning, "Multiple Speaker components found attached to the GameObject or its children. Using the first one we found.");
				}
			}
			if (null == speaker && null != voiceConnection.SpeakerPrefab)
			{
				speaker = voiceConnection.InstantiateSpeakerPrefab(base.gameObject, destroyOnRemove: false);
			}
			if (null == speaker)
			{
				Logger.Log(LogLevel.Error, "No Speaker component or prefab found. Assign a Speaker to VoiceNetworkObject object or set up FusionVoiceClient.SpeakerPrefab.");
			}
			else
			{
				Logger.Log(LogLevel.Info, "Speaker instantiated.");
			}
			SpeakerInUse = speaker;
		}

		private object GetUserData()
		{
			return base.Object.Id;
		}

		public override void Spawned()
		{
			voiceComponentImpl.Awake(this);
			voiceConnection = base.Runner.GetComponent<VoiceConnection>();
			if (IsLocal)
			{
				SetupRecorder();
				if (RecorderInUse == null)
				{
					Logger.Log(LogLevel.Warning, "Recorder not setup for VoiceNetworkObject: playback may not work properly.");
				}
				else if (!RecorderInUse.TransmitEnabled)
				{
					Logger.Log(LogLevel.Warning, "VoiceNetworkObject.RecorderInUse.TransmitEnabled is false, don't forget to set it to true to enable transmission.");
				}
			}
			SetupSpeaker();
			if (SpeakerInUse == null)
			{
				Logger.Log(LogLevel.Warning, "Speaker not setup for VoiceNetworkObject: voice chat will not work.");
			}
			else
			{
				voiceConnection.AddSpeaker(SpeakerInUse, GetUserData());
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			voiceConnection.RemoveRecorder(RecorderInUse);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
