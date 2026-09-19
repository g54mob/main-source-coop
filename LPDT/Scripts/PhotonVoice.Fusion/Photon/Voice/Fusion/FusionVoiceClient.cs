using System;
using System.Collections.Generic;
using System.IO;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using Photon.Client;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;

namespace Photon.Voice.Fusion
{
	[AddComponentMenu("Photon Voice/Fusion/Fusion Voice Client")]
	[RequireComponent(typeof(NetworkRunner))]
	public class FusionVoiceClient : VoiceFollowClient, INetworkRunnerCallbacks, IPublicFacingInterface
	{
		private NetworkRunner networkRunner;

		private EnterRoomParams voiceRoomParams = new EnterRoomParams
		{
			RoomOptions = new RoomOptions
			{
				IsVisible = false
			}
		};

		private bool voiceFollowClientStarted;

		[SerializeField]
		public bool UseFusionAppSettings = true;

		[SerializeField]
		public bool UseFusionAuthValues = true;

		private string fusionOfflineVoiceRoomName;

		private const byte FusionNetworkIdTypeCode = 0;

		private static byte[] memCompressedUInt64 = new byte[10];

		protected override bool LeaderInRoom => networkRunner.SessionInfo.IsValid;

		protected override bool LeaderOfflineMode => networkRunner.GameMode == GameMode.Single;

		private string FusionOfflineVoiceRoomName
		{
			get
			{
				if (fusionOfflineVoiceRoomName == null)
				{
					fusionOfflineVoiceRoomName = $"fusion_offline_{Guid.NewGuid()}_voice";
				}
				return fusionOfflineVoiceRoomName;
			}
		}

		protected override void Start()
		{
			if (networkRunner.State != NetworkRunner.States.Shutdown)
			{
				VoiceFollowClientStart();
			}
		}

		private void VoiceFollowClientStart()
		{
			if (voiceFollowClientStarted)
			{
				return;
			}
			voiceFollowClientStarted = true;
			base.Start();
			if (base.UsePrimaryRecorder)
			{
				if (base.PrimaryRecorder != null)
				{
					AddRecorder(base.PrimaryRecorder);
				}
				else
				{
					base.Logger.Log(LogLevel.Error, "Primary Recorder is not set.");
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			networkRunner = GetComponent<NetworkRunner>();
			VoiceRegisterCustomTypes();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		protected override Speaker InstantiateSpeakerForRemoteVoice(int playerId, byte voiceId, object userData)
		{
			if (userData == null)
			{
				base.Logger.Log(LogLevel.Info, "Creating Speaker for remote voice {0}/{1} FusionVoiceClient Primary Recorder (userData == null).", playerId, voiceId);
				return InstantiateSpeakerPrefab(base.gameObject, destroyOnRemove: true);
			}
			if (!(userData is NetworkId networkId))
			{
				base.Logger.Log(LogLevel.Warning, "UserData ({0}) is not of type NetworkId. Remote voice {1}/{2} not linked. Do you have a Recorder not used with a VoiceNetworkObject? is this expected?", (userData == null) ? "null" : userData.ToString(), playerId, voiceId);
				return null;
			}
			if (!networkId.IsValid)
			{
				base.Logger.Log(LogLevel.Warning, "NetworkId is not valid ({0}). Remote voice {1}/{2} not linked.", networkId, playerId, voiceId);
				return null;
			}
			VoiceNetworkObject voiceNetworkObject = networkRunner.TryGetNetworkedBehaviourFromNetworkedObjectRef<VoiceNetworkObject>(networkId);
			if ((object)voiceNetworkObject == null || !voiceNetworkObject)
			{
				base.Logger.Log(LogLevel.Warning, "No voiceNetworkObject found with ID {0}. Remote voice {1}/{2} not linked.", networkId, playerId, voiceId);
				return null;
			}
			base.Logger.Log(LogLevel.Info, "Using VoiceNetworkObject {0} Speaker for remote voice  p#{1} v#{2}.", userData, playerId, voiceId);
			return voiceNetworkObject.SpeakerInUse;
		}

		protected override string GetVoiceRoomName()
		{
			if (networkRunner.GameMode != GameMode.Single && networkRunner.SessionInfo.IsValid)
			{
				return $"{networkRunner.SessionInfo.Name}_voice";
			}
			return FusionOfflineVoiceRoomName;
		}

		protected override bool ConnectVoice()
		{
			AppSettings appSettings = new AppSettings();
			if (UseFusionAppSettings)
			{
				appSettings.AppIdVoice = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.AppIdVoice;
				appSettings.AppVersion = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.AppVersion;
				appSettings.FixedRegion = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.FixedRegion;
				appSettings.UseNameServer = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.UseNameServer;
				appSettings.Server = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.Server;
				appSettings.Port = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.Port;
				appSettings.ProxyServer = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.ProxyServer;
				appSettings.BestRegionSummaryFromStorage = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.BestRegionSummaryFromStorage;
				appSettings.EnableLobbyStatistics = false;
				appSettings.EnableProtocolFallback = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.EnableProtocolFallback;
				appSettings.Protocol = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.Protocol;
				appSettings.AuthMode = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.AuthMode;
				appSettings.NetworkLogging = global::Fusion.Photon.Realtime.PhotonAppSettings.Global.AppSettings.NetworkLogging;
			}
			else
			{
				Settings.CopyTo(appSettings);
			}
			string region = networkRunner.SessionInfo.Region;
			if (string.IsNullOrEmpty(region))
			{
				base.Logger.Log(LogLevel.Warning, "Unexpected: fusion region is empty.");
				if (!string.IsNullOrEmpty(appSettings.FixedRegion))
				{
					base.Logger.Log(LogLevel.Warning, "Unexpected: fusion region is empty while voice region is set to \"{0}\". Setting it to null now.", appSettings.FixedRegion);
					appSettings.FixedRegion = null;
				}
			}
			else if (!string.Equals(appSettings.FixedRegion, region, StringComparison.OrdinalIgnoreCase))
			{
				if (string.IsNullOrEmpty(appSettings.FixedRegion))
				{
					base.Logger.Log(LogLevel.Info, "Setting voice region to \"{0}\" to match fusion region.", region);
				}
				else
				{
					base.Logger.Log(LogLevel.Info, "Switching voice region to \"{0}\" from \"{1}\" to match fusion region.", region, appSettings.FixedRegion);
				}
				appSettings.FixedRegion = region;
			}
			if (UseFusionAuthValues && networkRunner.AuthenticationValues != null)
			{
				base.Client.AuthValues = new AuthenticationValues(networkRunner.AuthenticationValues.UserId)
				{
					AuthGetParameters = networkRunner.AuthenticationValues.AuthGetParameters,
					AuthType = networkRunner.AuthenticationValues.AuthType
				};
				if (networkRunner.AuthenticationValues.AuthPostData != null)
				{
					if (networkRunner.AuthenticationValues.AuthPostData is byte[] authPostData)
					{
						base.Client.AuthValues.SetAuthPostData(authPostData);
					}
					else if (networkRunner.AuthenticationValues.AuthPostData is string authPostData2)
					{
						base.Client.AuthValues.SetAuthPostData(authPostData2);
					}
					else if (networkRunner.AuthenticationValues.AuthPostData is Dictionary<string, object> authPostData3)
					{
						base.Client.AuthValues.SetAuthPostData(authPostData3);
					}
				}
			}
			return ConnectUsingSettings(appSettings);
		}

		private static void VoiceRegisterCustomTypes()
		{
			PhotonPeer.RegisterType(typeof(NetworkId), 0, SerializeFusionNetworkId, DeserializeFusionNetworkId);
		}

		private static object DeserializeFusionNetworkId(StreamBuffer instream, short length)
		{
			NetworkId networkId = default(NetworkId);
			lock (memCompressedUInt64)
			{
				ulong num = ReadCompressedUInt64(instream);
				networkId.Raw = (uint)num;
			}
			return networkId;
		}

		private static ulong ReadCompressedUInt64(StreamBuffer stream)
		{
			ulong num = 0uL;
			int num2 = 0;
			byte[] buffer = stream.GetBuffer();
			int num3 = stream.Position;
			while (num2 != 70)
			{
				if (num3 >= buffer.Length)
				{
					throw new EndOfStreamException("Failed to read full ulong.");
				}
				byte b = buffer[num3];
				num3++;
				num |= (ulong)((long)(b & 0x7F) << num2);
				num2 += 7;
				if ((b & 0x80) == 0)
				{
					break;
				}
			}
			stream.Position = num3;
			return num;
		}

		private static int WriteCompressedUInt64(StreamBuffer stream, ulong value)
		{
			int num = 0;
			lock (memCompressedUInt64)
			{
				memCompressedUInt64[num] = (byte)(value & 0x7F);
				for (value >>= 7; value != 0; value >>= 7)
				{
					memCompressedUInt64[num] |= 128;
					memCompressedUInt64[++num] = (byte)(value & 0x7F);
				}
				num++;
				stream.Write(memCompressedUInt64, 0, num);
				return num;
			}
		}

		private static short SerializeFusionNetworkId(StreamBuffer outstream, object customobject)
		{
			return (short)WriteCompressedUInt64(outstream, ((NetworkId)customobject).Raw);
		}

		void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
		{
			base.Logger.Log(LogLevel.Info, "OnPlayerJoined {0}", player);
			if (runner.LocalPlayer == player)
			{
				VoiceFollowClientStart();
				base.Logger.Log(LogLevel.Info, "Local player joined, calling VoiceConnectOrJoinRoom");
				LeaderStateChanged(ClientState.Joined);
			}
		}

		void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
			base.Logger.Log(LogLevel.Info, "OnPlayerLeft {0}", player);
			if (runner.LocalPlayer == player)
			{
				base.Logger.Log(LogLevel.Info, "Local player left, calling VoiceDisconnect");
				LeaderStateChanged(ClientState.Disconnected);
			}
		}

		void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
		{
		}

		void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
		{
		}

		void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
		{
		}

		void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
		{
			LeaderStateChanged(ClientState.ConnectedToMasterServer);
		}

		void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
		{
			LeaderStateChanged(ClientState.Disconnected);
		}

		void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
		{
		}

		void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
		{
		}

		void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
		{
		}

		void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
		{
		}

		void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
		{
		}

		void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
		{
		}

		void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
		{
		}

		void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
		{
		}

		void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
		{
		}

		public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
		}

		public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
		}

		void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey reliableKey, float progress)
		{
		}
	}
}
