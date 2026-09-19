using System;
using System.Linq;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	public class NextServer : NextCommon, IServer
	{
		private BidirectionalDictionary<HSteamNetConnection, int> connToMirrorID;

		private BidirectionalDictionary<CSteamID, int> steamIDToMirrorID;

		private int maxConnections;

		private int nextConnectionID;

		private HSteamListenSocket listenSocket;

		private Callback<SteamNetConnectionStatusChangedCallback_t> c_onConnectionChange;

		private static NextServer server;

		private event Action<int, string> OnConnectedWithAddress;

		private event Action<int, byte[], int> OnReceivedData;

		private event Action<int> OnDisconnected;

		private event Action<int, TransportError, string> OnReceivedError;

		private NextServer(int maxConnections)
		{
			this.maxConnections = maxConnections;
			connToMirrorID = new BidirectionalDictionary<HSteamNetConnection, int>();
			steamIDToMirrorID = new BidirectionalDictionary<CSteamID, int>();
			nextConnectionID = 1;
			c_onConnectionChange = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(OnConnectionStatusChanged);
		}

		public static NextServer CreateServer(FizzySteamworks transport, int maxConnections)
		{
			server = new NextServer(maxConnections);
			server.OnConnectedWithAddress += delegate(int id, string addres)
			{
				transport.OnServerConnectedWithAddress(id, addres);
			};
			server.OnDisconnected += delegate(int id)
			{
				transport.OnServerDisconnected(id);
			};
			server.OnReceivedData += delegate(int id, byte[] data, int ch)
			{
				transport.OnServerDataReceived(id, new ArraySegment<byte>(data), ch);
			};
			server.OnReceivedError += delegate(int id, TransportError error, string reason)
			{
				transport.OnServerError(id, error, reason);
			};
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			server.Host();
			return server;
		}

		private void Host()
		{
			SteamNetworkingConfigValue_t[] array = new SteamNetworkingConfigValue_t[0];
			listenSocket = SteamNetworkingSockets.CreateListenSocketP2P(0, array.Length, array);
		}

		private void OnConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t param)
		{
			ulong steamID = param.m_info.m_identityRemote.GetSteamID64();
			if (param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting)
			{
				EResult eResult;
				if (connToMirrorID.Count >= maxConnections)
				{
					Debug.Log($"Incoming connection {steamID} would exceed max connection count. Rejecting.");
					SteamNetworkingSockets.CloseConnection(param.m_hConn, 0, "Max Connection Count", bEnableLinger: false);
				}
				else if ((eResult = SteamNetworkingSockets.AcceptConnection(param.m_hConn)) == EResult.k_EResultOK)
				{
					Debug.Log($"Accepting connection {steamID}");
				}
				else
				{
					Debug.Log($"Connection {steamID} could not be accepted: {eResult}");
				}
			}
			else if (param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
			{
				int num = nextConnectionID++;
				connToMirrorID.Add(param.m_hConn, num);
				steamIDToMirrorID.Add(param.m_info.m_identityRemote.GetSteamID(), num);
				this.OnConnectedWithAddress?.Invoke(num, server.ServerGetClientAddress(num));
				Debug.Log($"Client with SteamID {steamID} connected. Assigning connection id {num}");
			}
			else if (param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer || param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally)
			{
				if (connToMirrorID.TryGetValue(param.m_hConn, out var value))
				{
					InternalDisconnect(value, param.m_hConn);
				}
			}
			else
			{
				Debug.Log($"Connection {steamID} state changed: {param.m_info.m_eState}");
			}
		}

		private void InternalDisconnect(int connId, HSteamNetConnection socket)
		{
			this.OnDisconnected?.Invoke(connId);
			SteamNetworkingSockets.CloseConnection(socket, 0, "Graceful disconnect", bEnableLinger: false);
			connToMirrorID.Remove(connId);
			steamIDToMirrorID.Remove(connId);
			Debug.Log($"Client with ConnectionID {connId} disconnected.");
		}

		public void Disconnect(int connectionId)
		{
			if (connToMirrorID.TryGetValue(connectionId, out var value))
			{
				Debug.Log($"Connection id {connectionId} disconnected.");
				SteamNetworkingSockets.CloseConnection(value, 0, "Disconnected by server", bEnableLinger: false);
				steamIDToMirrorID.Remove(connectionId);
				connToMirrorID.Remove(connectionId);
				this.OnDisconnected?.Invoke(connectionId);
			}
			else
			{
				Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId);
			}
		}

		public void FlushData()
		{
			foreach (HSteamNetConnection item in connToMirrorID.FirstTypes.ToList())
			{
				SteamNetworkingSockets.FlushMessagesOnConnection(item);
			}
		}

		public void ReceiveData()
		{
			foreach (HSteamNetConnection item in connToMirrorID.FirstTypes.ToList())
			{
				if (!connToMirrorID.TryGetValue(item, out var value))
				{
					continue;
				}
				IntPtr[] array = new IntPtr[256];
				int num;
				if ((num = SteamNetworkingSockets.ReceiveMessagesOnConnection(item, array, 256)) > 0)
				{
					for (int i = 0; i < num; i++)
					{
						var (arg, arg2) = ProcessMessage(array[i]);
						this.OnReceivedData?.Invoke(value, arg, arg2);
					}
				}
			}
		}

		public void Send(int connectionId, byte[] data, int channelId)
		{
			if (connToMirrorID.TryGetValue(connectionId, out var value))
			{
				EResult eResult = SendSocket(value, data, channelId);
				switch (eResult)
				{
				case EResult.k_EResultNoConnection:
				case EResult.k_EResultInvalidParam:
					Debug.Log($"Connection to {connectionId} was lost.");
					InternalDisconnect(connectionId, value);
					break;
				default:
					Debug.LogError($"Could not send: {eResult}");
					break;
				case EResult.k_EResultOK:
					break;
				}
			}
			else
			{
				Debug.LogError("Trying to send on an unknown connection: " + connectionId);
				this.OnReceivedError?.Invoke(connectionId, TransportError.Unexpected, "ERROR Unknown Connection");
			}
		}

		public string ServerGetClientAddress(int connectionId)
		{
			if (steamIDToMirrorID.TryGetValue(connectionId, out var value))
			{
				return value.ToString();
			}
			Debug.LogError("Trying to get info on an unknown connection: " + connectionId);
			this.OnReceivedError?.Invoke(connectionId, TransportError.Unexpected, "ERROR Unknown Connection");
			return string.Empty;
		}

		public void Shutdown()
		{
			SteamNetworkingSockets.CloseListenSocket(listenSocket);
			c_onConnectionChange?.Dispose();
			c_onConnectionChange = null;
		}
	}
}
