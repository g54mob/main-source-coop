using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;

namespace Photon.Voice
{
	public abstract class VoiceFollowClient : VoiceConnection
	{
		public bool AutoConnectAndJoin = true;

		private EnterRoomParams voiceRoomParams = new EnterRoomParams
		{
			RoomOptions = new RoomOptions
			{
				IsVisible = false,
				PlayerTtl = 2000
			}
		};

		private bool manualDisconnect;

		private bool errAuthOrJoin;

		protected abstract bool LeaderInRoom { get; }

		protected abstract bool LeaderOfflineMode { get; }

		protected abstract string GetVoiceRoomName();

		protected abstract bool ConnectVoice();

		public bool ConnectAndJoinRoom()
		{
			if (!LeaderInRoom)
			{
				base.Logger.LogError("Cannot connect and join if Leader is not joined.");
				return false;
			}
			if (ConnectVoice())
			{
				manualDisconnect = false;
				return true;
			}
			base.Logger.LogError("Connecting to server failed.");
			return false;
		}

		public void Disconnect()
		{
			if (!base.Client.IsConnected)
			{
				base.Logger.LogError("Cannot Disconnect if not connected.");
				return;
			}
			manualDisconnect = true;
			base.Client.Disconnect();
		}

		protected virtual void Start()
		{
			manualDisconnect = false;
			FollowLeader();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		protected override void OnOperationResponseReceived(OperationResponse operationResponse)
		{
			if (operationResponse.ReturnCode != 0)
			{
				switch (operationResponse.OperationCode)
				{
				case 230:
				case 231:
					base.Logger.LogError("Setting AutoConnectAndJoin to false because authentication failed. Error: {0}. Message: {1}.", operationResponse.ReturnCode, operationResponse.DebugMessage);
					errAuthOrJoin = true;
					break;
				case 226:
					base.Logger.LogError("Failed to join room. RoomName: '{2}' Region: {3} Error: {0}. Message: {1}.", operationResponse.ReturnCode, operationResponse.DebugMessage, voiceRoomParams.RoomName, base.Client.CloudRegion);
					errAuthOrJoin = true;
					manualDisconnect = true;
					Disconnect();
					break;
				default:
					base.Logger.LogError("Operation {0} response error code {1} message {2}", operationResponse.OperationCode, operationResponse.ReturnCode, operationResponse.DebugMessage);
					break;
				}
			}
		}

		protected void LeaderStateChanged(ClientState toState)
		{
			base.Logger.LogInfo("OnLeaderStateChanged to {0}", toState);
			if (toState == ClientState.Joined)
			{
				errAuthOrJoin = false;
			}
			FollowLeader(toState);
		}

		protected override void OnVoiceStateChanged(ClientState fromState, ClientState toState)
		{
			base.OnVoiceStateChanged(fromState, toState);
			if (toState == ClientState.Disconnected)
			{
				if (manualDisconnect)
				{
					manualDisconnect = false;
					return;
				}
				if (base.Client.DisconnectedCause == DisconnectCause.ClientTimeout && base.Client.ReconnectAndRejoin())
				{
					return;
				}
				if (base.Client.DisconnectedCause == DisconnectCause.DnsExceptionOnConnect)
				{
					Debug.LogWarning($"Voice Disconnected and will not immediately reconnect. Cause: {base.Client.DisconnectedCause}");
					return;
				}
			}
			base.Logger.LogDebug("OnVoiceStateChanged  from {0} to {1}", fromState, toState);
			FollowLeader(toState);
		}

		private void ConnectOrJoinVoice()
		{
			switch (base.ClientState)
			{
			case ClientState.PeerCreated:
			case ClientState.Disconnected:
				base.Logger.LogInfo("Leader joined room, now connecting Voice client");
				if (!ConnectVoice())
				{
					base.Logger.LogError("Connecting to server failed.");
				}
				break;
			case ClientState.ConnectedToMasterServer:
				base.Logger.LogInfo("Leader joined room, now joining Voice room");
				if (!JoinVoiceRoom(GetVoiceRoomName()))
				{
					base.Logger.LogError("Joining a voice room failed.");
				}
				break;
			default:
				base.Logger.LogWarning("Leader joined room, Voice client is busy ({0}). Is this expected?", base.ClientState);
				break;
			}
		}

		private bool JoinVoiceRoom(string voiceRoomName)
		{
			if (string.IsNullOrEmpty(voiceRoomName))
			{
				base.Logger.LogError("Voice room name is null or empty.");
				return false;
			}
			voiceRoomParams.RoomName = voiceRoomName;
			Debug.Log("Calling OpJoinOrCreateRoom for room name '" + voiceRoomName + "' region " + base.Client.CloudRegion + ".");
			return base.Client.OpJoinOrCreateRoom(voiceRoomParams);
		}

		private void FollowLeader(ClientState toState)
		{
			if (toState == ClientState.Joined || (uint)(toState - 14) <= 1u)
			{
				base.Logger.LogDebug($"FollowLeader for state {toState}");
				FollowLeader();
			}
		}

		private void FollowLeader()
		{
			if (manualDisconnect || ((!AutoConnectAndJoin || errAuthOrJoin) && !base.Client.IsConnected))
			{
				return;
			}
			if (!LeaderInRoom || LeaderOfflineMode)
			{
				if (base.Client.IsConnected && base.Client.State != ClientState.Disconnecting)
				{
					base.Client.Disconnect();
				}
				return;
			}
			if (!base.Client.InRoom)
			{
				ConnectOrJoinVoice();
				return;
			}
			string voiceRoomName = GetVoiceRoomName();
			string text = base.Client.CurrentRoom.Name;
			if (string.IsNullOrEmpty(text) || !text.Equals(voiceRoomName))
			{
				base.Logger.LogWarning("Voice room mismatch: Expected:\"{0}\" Current:\"{1}\", leaving the second to join the first.", voiceRoomName, text);
				if (!base.Client.OpLeaveRoom(becomeInactive: false))
				{
					base.Logger.LogError("Leaving the current voice room failed.");
				}
			}
		}
	}
}
