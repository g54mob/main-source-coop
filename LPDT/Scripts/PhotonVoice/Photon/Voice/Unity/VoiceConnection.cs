using System;
using System.Collections.Generic;
using Photon.Client;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Photon.Voice.Unity
{
	[DisallowMultipleComponent]
	public class VoiceConnection : ConnectionHandler
	{
		public const int ChannelAudio = 1;

		public const int ChannelVideo = 2;

		private VoiceComponentImpl voiceComponentImpl = new VoiceComponentImpl();

		private const string PlayerPrefsKey = "VoiceCloudBestRegion";

		private Realtime5Transport client;

		[SerializeField]
		private bool runInBackground = true;

		private const int ServiceInterval = 33;

		[SerializeField]
		private int statsResetInterval = 1000;

		private int lastService = Environment.TickCount;

		private int lastStatsUpdate = Environment.TickCount;

		private float statsReferenceTime;

		private int referenceFramesLost;

		private int referenceFramesReceived;

		[SerializeField]
		private GameObject speakerPrefab;

		private List<RemoteVoiceLink> cachedRemoteVoices = new List<RemoteVoiceLink>();

		[SerializeField]
		[FormerlySerializedAs("PrimaryRecorder")]
		private Recorder primaryRecorder;

		[SerializeField]
		[Tooltip("Use primary recorder directly by Voice Client")]
		private bool usePrimaryRecorder;

		[SerializeField]
		[Tooltip("Use the protocol compatible with Photon Voice C++ API")]
		private bool cppCompatibilityMode;

		private List<Speaker> linkedSpeakers = new List<Speaker>();

		private List<Recorder> recorders = new List<Recorder>();

		public AppSettings Settings;

		public bool ApplyDontDestroyOnLoad = true;

		public virtual bool AlwaysUsePrimaryRecorder => false;

		public ILogger Logger => voiceComponentImpl.Logger;

		public VoiceLogger VoiceLogger => voiceComponentImpl.VoiceLogger;

		public new Realtime5Transport Client => client;

		public VoiceClient VoiceClient => Client.VoiceClient;

		public ClientState ClientState => Client.State;

		public float FramesReceivedPerSecond { get; private set; }

		public float FramesLostPerSecond { get; private set; }

		public float FramesLostPercent { get; private set; }

		public GameObject SpeakerPrefab
		{
			get
			{
				return speakerPrefab;
			}
			set
			{
				speakerPrefab = value;
			}
		}

		public Recorder PrimaryRecorder
		{
			get
			{
				return primaryRecorder;
			}
			set
			{
				primaryRecorder = value;
			}
		}

		public bool UsePrimaryRecorder => usePrimaryRecorder;

		public string BestRegionSummaryInPreferences
		{
			get
			{
				return PlayerPrefs.GetString("VoiceCloudBestRegion", null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					PlayerPrefs.DeleteKey("VoiceCloudBestRegion");
				}
				else
				{
					PlayerPrefs.SetString("VoiceCloudBestRegion", value);
				}
			}
		}

		public event Action<Speaker> SpeakerLinked;

		public event Action<RemoteVoiceLink> RemoteVoiceAdded;

		private void Init()
		{
			client = new Realtime5Transport2(Logger, ConnectionProtocol.Udp, cppCompatibilityMode);
			VoiceClient voiceClient = client.VoiceClient;
			voiceClient.OnRemoteVoiceInfoAction = (VoiceClient.RemoteVoiceInfoDelegate)Delegate.Combine(voiceClient.OnRemoteVoiceInfoAction, new VoiceClient.RemoteVoiceInfoDelegate(OnRemoteVoiceInfo));
			client.StateChanged += OnVoiceStateChanged;
			client.OpResponseReceived += OnOperationResponseReceived;
			base.Client = client;
			StartFallbackSendAckThread();
		}

		public virtual bool ConnectUsingSettings(AppSettings overwriteSettings = null)
		{
			if (Client.RealtimePeer.PeerState != PeerStateValue.Disconnected)
			{
				Logger.Log(LogLevel.Warning, "ConnectUsingSettings() failed. Can only connect while in state 'Disconnected'. Current state: {0}", Client.RealtimePeer.PeerState);
				return false;
			}
			if (overwriteSettings != null)
			{
				Settings = overwriteSettings;
			}
			if (Settings == null)
			{
				Logger.Log(LogLevel.Error, "Settings are null");
				return false;
			}
			if (string.IsNullOrEmpty(Settings.AppIdVoice) && string.IsNullOrEmpty(Settings.Server))
			{
				Logger.Log(LogLevel.Error, "Provide an AppId or a Server address in Settings to be able to connect");
				return false;
			}
			if (Settings.IsMasterServerAddress && string.IsNullOrEmpty(Client.UserId))
			{
				Client.UserId = Guid.NewGuid().ToString();
			}
			if (string.IsNullOrEmpty(Settings.BestRegionSummaryFromStorage))
			{
				Settings.BestRegionSummaryFromStorage = BestRegionSummaryInPreferences;
			}
			return client.ConnectUsingSettings(Settings);
		}

		public bool AddSpeaker(Speaker speaker, object userData)
		{
			for (int i = 0; i < cachedRemoteVoices.Count; i++)
			{
				RemoteVoiceLink remoteVoiceLink = cachedRemoteVoices[i];
				if (userData.Equals(remoteVoiceLink.VoiceInfo.UserData))
				{
					Logger.Log(LogLevel.Debug, "Speaker linking for remoteVoice {0}.", remoteVoiceLink);
					LinkSpeaker(speaker, remoteVoiceLink);
					return speaker.IsLinked;
				}
			}
			return false;
		}

		protected virtual void Awake()
		{
			voiceComponentImpl.Awake(this);
			Init();
			if (ApplyDontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(voiceComponentImpl.VoiceLogger.gameObject);
			}
			if (runInBackground)
			{
				Application.runInBackground = runInBackground;
			}
		}

		protected virtual void Update()
		{
			int tickCount = Environment.TickCount;
			if (tickCount - lastService > 33)
			{
				lastService = tickCount;
				Client.LoadBalancingPeer.Service();
				VoiceClient.Service();
			}
			if (statsResetInterval > 0 && tickCount - lastStatsUpdate > statsResetInterval)
			{
				lastStatsUpdate = tickCount;
				CalcStatistics();
			}
		}

		protected virtual void OnDestroy()
		{
			client.StateChanged -= OnVoiceStateChanged;
			client.OpResponseReceived -= OnOperationResponseReceived;
			client.Disconnect();
			if (client.RealtimePeer != null)
			{
				client.Disconnect();
			}
			client.Dispose();
		}

		protected virtual Speaker InstantiateSpeakerForRemoteVoice(int playerId, byte voiceId, object userData)
		{
			throw new Exception("FindSpeakerByUserData: VoiceConnection does not provide userData linkage");
		}

		public Speaker InstantiateSpeakerPrefab(GameObject parent, bool destroyOnRemove)
		{
			if (SpeakerPrefab == null)
			{
				Logger.Log(LogLevel.Error, "SpeakerPrefab is not set.");
				return null;
			}
			GameObject go = UnityEngine.Object.Instantiate(SpeakerPrefab);
			Speaker[] componentsInChildren = go.GetComponentsInChildren<Speaker>(includeInactive: true);
			if (componentsInChildren.Length != 0)
			{
				if (componentsInChildren.Length > 1)
				{
					Logger.Log(LogLevel.Warning, "Multiple Speaker components found attached to the GameObject (VoiceConnection.SpeakerPrefab) or its children. Using the first one we found.");
				}
				if (destroyOnRemove)
				{
					Speaker obj = componentsInChildren[0];
					obj.OnRemoteVoiceRemoveAction = (Action<Speaker>)Delegate.Combine(obj.OnRemoteVoiceRemoveAction, (Action<Speaker>)delegate
					{
						Logger.Log(LogLevel.Info, "OnRemoteVoiceRemoveAction: destroying VoiceConnection.SpeakerPrefab instance [{0}]", go.name);
						UnityEngine.Object.Destroy(go);
					});
				}
				if (parent != null)
				{
					go.transform.SetParent(parent.transform, worldPositionStays: false);
				}
				Logger.Log(LogLevel.Info, "Instance of VoiceConnection.SpeakerPrefab instantiated.");
				return componentsInChildren[0];
			}
			Logger.Log(LogLevel.Error, "SpeakerPrefab does not have a component of type Speaker in its hierarchy.");
			UnityEngine.Object.Destroy(go);
			return null;
		}

		private void OnRemoteVoiceInfo(int channelId, int playerId, byte voiceId, VoiceInfo voiceInfo, ref RemoteVoiceOptions options)
		{
			if (voiceInfo.Codec != Codec.AudioOpus)
			{
				Logger.Log(LogLevel.Info, "OnRemoteVoiceInfo: Skipped as codec is not Opus, [p#{0} v#{1} c#{2} i:{{{3}}}]", playerId, voiceId, channelId, voiceInfo);
				return;
			}
			RemoteVoiceLink remoteVoice = new RemoteVoiceLink(voiceInfo, playerId, voiceId, channelId, ref options);
			_ = Application.platform;
			_ = 17;
			Logger.Log(LogLevel.Info, "OnRemoteVoiceInfo:  {0}", remoteVoice);
			cachedRemoteVoices.Add(remoteVoice);
			if (this.RemoteVoiceAdded != null)
			{
				this.RemoteVoiceAdded(remoteVoice);
			}
			remoteVoice.RemoteVoiceRemoved += delegate
			{
				Logger.Log(LogLevel.Info, "OnRemoteVoiceInfo: RemoteVoiceRemoved {0}", remoteVoice);
				cachedRemoteVoices.Remove(remoteVoice);
			};
			Speaker speaker = InstantiateSpeakerForRemoteVoice(playerId, voiceId, voiceInfo.UserData);
			if (speaker == null)
			{
				Logger.Log(LogLevel.Debug, "OnRemoteVoiceInfo: Remote GameObject not found or does not have a Speaker {0}", remoteVoice);
			}
			else
			{
				speaker.Name = $"Remote p#{playerId} v#{voiceId}";
				LinkSpeaker(speaker, remoteVoice);
			}
		}

		protected virtual void OnVoiceStateChanged(ClientState fromState, ClientState toState)
		{
			Logger.Log(LogLevel.Info, "OnVoiceStateChanged from {0} to {1}", fromState, toState);
			if (fromState == ClientState.Joined)
			{
				for (int i = 0; i < recorders.Count; i++)
				{
					Recorder recorder = recorders[i];
					if (recorder.RecordWhenJoined)
					{
						recorder.RecordingEnabled = false;
					}
				}
				cachedRemoteVoices.Clear();
			}
			switch (toState)
			{
			case ClientState.ConnectedToMasterServer:
				if (Client.RegionHandler != null)
				{
					if (Settings != null)
					{
						Settings.BestRegionSummaryFromStorage = Client.RegionHandler.SummaryToCache;
					}
					BestRegionSummaryInPreferences = Client.RegionHandler.SummaryToCache;
				}
				break;
			case ClientState.Joined:
			{
				for (int j = 0; j < recorders.Count; j++)
				{
					Recorder recorder2 = recorders[j];
					if (recorder2.RecordWhenJoined)
					{
						recorder2.RecordingEnabled = true;
					}
				}
				break;
			}
			}
		}

		protected void CalcStatistics()
		{
			float time = Time.time;
			int num = VoiceClient.FramesReceived - referenceFramesReceived;
			int num2 = VoiceClient.FramesLost - referenceFramesLost;
			float num3 = time - statsReferenceTime;
			if (num3 > 0f)
			{
				if (num + num2 > 0)
				{
					FramesReceivedPerSecond = (float)num / num3;
					FramesLostPerSecond = (float)num2 / num3;
					FramesLostPercent = 100f * (float)num2 / (float)(num + num2);
				}
				else
				{
					FramesReceivedPerSecond = 0f;
					FramesLostPerSecond = 0f;
					FramesLostPercent = 0f;
				}
			}
			referenceFramesReceived = VoiceClient.FramesReceived;
			referenceFramesLost = VoiceClient.FramesLost;
			statsReferenceTime = time;
		}

		private void LinkSpeaker(Speaker speaker, RemoteVoiceLink remoteVoice)
		{
			if (speaker.Link(remoteVoice))
			{
				Logger.Log(LogLevel.Info, "Speaker linked with remote voice {0}", remoteVoice);
				linkedSpeakers.Add(speaker);
				remoteVoice.RemoteVoiceRemoved += delegate
				{
					linkedSpeakers.Remove(speaker);
				};
				if (this.SpeakerLinked != null)
				{
					this.SpeakerLinked(speaker);
				}
			}
		}

		public bool AddRecorder(Recorder rec)
		{
			if (!recorders.Contains(rec))
			{
				if (rec.Init(this))
				{
					recorders.Add(rec);
					return true;
				}
				Logger.Log(LogLevel.Warning, "AddRecorder: failed to init recorder {0}.", rec);
			}
			else
			{
				Logger.Log(LogLevel.Error, "AddRecorder: recorder {0} already added.", rec);
			}
			return false;
		}

		public void RemoveRecorder(Recorder rec)
		{
			if (rec != null)
			{
				rec.Deinit(this);
				recorders.Remove(rec);
			}
		}

		protected virtual void OnOperationResponseReceived(OperationResponse operationResponse)
		{
			if (operationResponse.ReturnCode != 0 && (operationResponse.OperationCode != 225 || operationResponse.ReturnCode == 32760))
			{
				Logger.Log(LogLevel.Error, "Operation {0} response error code {1} message {2}", operationResponse.OperationCode, operationResponse.ReturnCode, operationResponse.DebugMessage);
			}
		}
	}
}
