using System;
using FishNet.Managing;
using FishNet.Transporting.Yak.Client;
using FishNet.Transporting.Yak.Server;
using UnityEngine;

namespace FishNet.Transporting.Yak
{
	[AddComponentMenu("FishNet/Transport/Yak")]
	public class Yak : Transport
	{
		private ClientSocket _client;

		private ServerSocket _server;

		internal const int CLIENT_HOST_ID = 32767;

		private const int MTU = 5000;

		public override event Action<ClientConnectionStateArgs> OnClientConnectionState;

		public override event Action<ServerConnectionStateArgs> OnServerConnectionState;

		public override event Action<RemoteConnectionStateArgs> OnRemoteConnectionState;

		public override event Action<ClientReceivedDataArgs> OnClientReceivedData;

		public override event Action<ServerReceivedDataArgs> OnServerReceivedData;

		public override void Initialize(NetworkManager networkManager, int transportIndex)
		{
			base.Initialize(networkManager, transportIndex);
			_client = new ClientSocket();
			_server = new ServerSocket();
			_client.Initialize(this, _server);
			_server.Initialize(this, _client);
		}

		private void OnDestroy()
		{
			Shutdown();
		}

		public override string GetConnectionAddress(int connectionId)
		{
			return string.Empty;
		}

		public override LocalConnectionState GetConnectionState(bool server)
		{
			if (server)
			{
				return _server.GetLocalConnectionState();
			}
			return _client.GetLocalConnectionState();
		}

		public override RemoteConnectionState GetConnectionState(int connectionId)
		{
			return _server.GetConnectionState(connectionId);
		}

		public override void HandleClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
		{
			OnClientConnectionState?.Invoke(connectionStateArgs);
		}

		public override void HandleServerConnectionState(ServerConnectionStateArgs connectionStateArgs)
		{
			OnServerConnectionState?.Invoke(connectionStateArgs);
		}

		public override void HandleRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
		{
			OnRemoteConnectionState?.Invoke(connectionStateArgs);
		}

		public override void IterateIncoming(bool server)
		{
			if (server)
			{
				_server.IterateIncoming();
			}
			else
			{
				_client.IterateIncoming();
			}
		}

		public override void IterateOutgoing(bool server)
		{
		}

		public override void HandleClientReceivedDataArgs(ClientReceivedDataArgs receivedDataArgs)
		{
			OnClientReceivedData?.Invoke(receivedDataArgs);
		}

		public override void HandleServerReceivedDataArgs(ServerReceivedDataArgs receivedDataArgs)
		{
			OnServerReceivedData?.Invoke(receivedDataArgs);
		}

		public override void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			_client.SendToServer(channelId, segment);
		}

		public override void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			_server.SendToClient(channelId, segment, connectionId);
		}

		public override bool IsLocalTransport(int connectionId)
		{
			return true;
		}

		public override int GetMaximumClients()
		{
			return 32767;
		}

		public override void SetMaximumClients(int value)
		{
		}

		public override void SetClientAddress(string address)
		{
		}

		public override void SetServerBindAddress(string address, IPAddressType addressType)
		{
		}

		public override void SetPort(ushort port)
		{
		}

		public override bool StartConnection(bool server)
		{
			if (server)
			{
				return StartServer();
			}
			return StartClient();
		}

		public override bool StopConnection(bool server)
		{
			if (server)
			{
				return StopServer();
			}
			return StopClient();
		}

		public override bool StopConnection(int connectionId, bool immediately)
		{
			return StopClient(connectionId, immediately);
		}

		public override void Shutdown()
		{
			StopConnection(server: false);
			StopConnection(server: true);
		}

		private bool StartServer()
		{
			if (_server.GetLocalConnectionState() != LocalConnectionState.Stopped)
			{
				base.NetworkManager.LogError("Server is already running.");
				return false;
			}
			return _server.StartConnection();
		}

		private bool StopServer()
		{
			if (_server != null)
			{
				return _server.StopConnection();
			}
			return false;
		}

		private bool StartClient()
		{
			if (_client.GetLocalConnectionState() != LocalConnectionState.Stopped)
			{
				base.NetworkManager.LogError("Client is already running.");
				return false;
			}
			_client.StartConnection();
			return true;
		}

		private bool StopClient()
		{
			if (_client != null)
			{
				return _client.StopConnection();
			}
			return false;
		}

		private bool StopClient(int connectionId, bool immediately)
		{
			return _server.StopConnection(connectionId);
		}

		public override int GetMTU(byte channel)
		{
			return 5000;
		}
	}
}
