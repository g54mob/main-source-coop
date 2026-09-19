using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	public class LegacyServer : LegacyCommon, IServer
	{
		private BidirectionalDictionary<CSteamID, int> steamToMirrorIds;

		private int maxConnections;

		private int nextConnectionID;

		private static LegacyServer server;

		private event Action<int, string> OnConnectedWithAddress;

		private event Action<int, byte[], int> OnReceivedData;

		private event Action<int> OnDisconnected;

		private event Action<int, TransportError, string> OnReceivedError;

		public static LegacyServer CreateServer(FizzySteamworks transport, int maxConnections)
		{
			server = new LegacyServer(transport, maxConnections);
			server.OnConnectedWithAddress += delegate(int id, string addres)
			{
				transport.OnServerConnectedWithAddress(id, addres);
			};
			server.OnDisconnected += delegate(int id)
			{
				transport.OnServerDisconnected(id);
			};
			server.OnReceivedData += delegate(int id, byte[] data, int channel)
			{
				transport.OnServerDataReceived(id, new ArraySegment<byte>(data), channel);
			};
			server.OnReceivedError += delegate(int id, TransportError error, string reason)
			{
				transport.OnServerError(id, error, reason);
			};
			try
			{
				InteropHelp.TestIfAvailableClient();
			}
			catch
			{
				Debug.LogError("SteamWorks not initialized.");
			}
			return server;
		}

		private LegacyServer(FizzySteamworks transport, int maxConnections)
			: base(transport)
		{
			this.maxConnections = maxConnections;
			steamToMirrorIds = new BidirectionalDictionary<CSteamID, int>();
			nextConnectionID = 1;
		}

		protected override void OnNewConnection(P2PSessionRequest_t result)
		{
			try
			{
				SteamNetworking.AcceptP2PSessionWithUser(result.m_steamIDRemote);
			}
			catch (Exception ex)
			{
				Debug.LogError("Steam Server error durring new connect, " + ex.Message);
				Shutdown();
			}
		}

		protected override void OnReceiveInternalData(InternalMessages type, CSteamID clientSteamID)
		{
			switch (type)
			{
			case InternalMessages.CONNECT:
			{
				if (steamToMirrorIds.Count >= maxConnections)
				{
					SendInternal(clientSteamID, InternalMessages.DISCONNECT);
					break;
				}
				SendInternal(clientSteamID, InternalMessages.ACCEPT_CONNECT);
				int num = nextConnectionID++;
				steamToMirrorIds.Add(clientSteamID, num);
				this.OnConnectedWithAddress(num, server.ServerGetClientAddress(num));
				Debug.Log($"Client with SteamID {clientSteamID} connected. Assigning connection id {num}");
				break;
			}
			case InternalMessages.DISCONNECT:
			{
				if (steamToMirrorIds.TryGetValue(clientSteamID, out var value))
				{
					this.OnDisconnected(value);
					CloseP2PSessionWithUser(clientSteamID);
					steamToMirrorIds.Remove(clientSteamID);
					Debug.Log($"Client with SteamID {clientSteamID} disconnected.");
				}
				break;
			}
			default:
				Debug.Log("Received unknown message type");
				break;
			}
		}

		protected override void OnReceiveData(byte[] data, CSteamID clientSteamID, int channel)
		{
			try
			{
				if (steamToMirrorIds.TryGetValue(clientSteamID, out var value))
				{
					this.OnReceivedData(value, data, channel);
					return;
				}
				CloseP2PSessionWithUser(clientSteamID);
				CSteamID cSteamID = clientSteamID;
				Debug.LogError("Data received from steam client thats not known " + cSteamID.ToString());
				this.OnReceivedError(-1, TransportError.DnsResolve, "ERROR Unknown SteamID");
			}
			catch (Exception ex)
			{
				Debug.LogError("Error while recive data " + ex.Message);
				Shutdown();
			}
		}

		public void Disconnect(int connectionId)
		{
			if (steamToMirrorIds.TryGetValue(connectionId, out var value))
			{
				SendInternal(value, InternalMessages.DISCONNECT);
				steamToMirrorIds.Remove(connectionId);
			}
			else
			{
				Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId);
			}
		}

		public void Shutdown()
		{
			foreach (KeyValuePair<CSteamID, int> steamToMirrorId in steamToMirrorIds)
			{
				Disconnect(steamToMirrorId.Value);
				WaitForClose(steamToMirrorId.Key);
			}
			Dispose();
		}

		public void Send(int connectionId, byte[] data, int channelId)
		{
			if (steamToMirrorIds.TryGetValue(connectionId, out var value))
			{
				Send(value, data, channelId);
				return;
			}
			Debug.LogError("Trying to send on unknown connection: " + connectionId);
			this.OnReceivedError(connectionId, TransportError.Unexpected, "ERROR Unknown Connection");
			Shutdown();
		}

		public string ServerGetClientAddress(int connectionId)
		{
			if (steamToMirrorIds.TryGetValue(connectionId, out var value))
			{
				return value.ToString();
			}
			Debug.LogError("Trying to get info on unknown connection: " + connectionId);
			this.OnReceivedError(connectionId, TransportError.Unexpected, "ERROR Unknown Connection");
			return string.Empty;
		}

		protected override void OnConnectionFailed(CSteamID remoteId)
		{
			int value;
			int obj = (steamToMirrorIds.TryGetValue(remoteId, out value) ? value : nextConnectionID++);
			this.OnDisconnected(obj);
			steamToMirrorIds.Remove(remoteId);
		}

		public void FlushData()
		{
		}
	}
}
