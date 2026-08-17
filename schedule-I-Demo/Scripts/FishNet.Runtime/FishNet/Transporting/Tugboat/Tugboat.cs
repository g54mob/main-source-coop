using System;
using System.Runtime.CompilerServices;
using FishNet.Managing;
using FishNet.Transporting.Tugboat.Client;
using FishNet.Transporting.Tugboat.Server;
using LiteNetLib.Layers;
using UnityEngine;

namespace FishNet.Transporting.Tugboat
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Transport/Tugboat")]
	public class Tugboat : Transport
	{
		[Header("Channels")]
		[Tooltip("Maximum transmission unit for the unreliable channel.")]
		[Range(576f, 1023f)]
		[SerializeField]
		private int _unreliableMTU = 1023;

		[Header("Server")]
		[Tooltip("IPv4 Address to bind server to.")]
		[SerializeField]
		private string _ipv4BindAddress;

		[Tooltip("IPv6 Address to bind server to.")]
		[SerializeField]
		private string _ipv6BindAddress;

		[Tooltip("Port to use.")]
		[SerializeField]
		private ushort _port = 7770;

		[Tooltip("Maximum number of players which may be connected at once.")]
		[Range(1f, 9999f)]
		[SerializeField]
		private int _maximumClients = 4095;

		[Header("Client")]
		[Tooltip("Address to connect.")]
		[SerializeField]
		private string _clientAddress = "localhost";

		[Header("Misc")]
		[Tooltip("How long in seconds until either the server or client socket must go without data before being timed out. Use 0f to disable timing out.")]
		[Range(0f, 1800f)]
		[SerializeField]
		private ushort _timeout = 15;

		private PacketLayerBase _packetLayer;

		private ServerSocket _server = new ServerSocket();

		private ClientSocket _client = new ClientSocket();

		private const ushort MAX_TIMEOUT_SECONDS = 1800;

		private const int MINIMUM_UDP_MTU = 576;

		private const int MAXIMUM_UDP_MTU = 1023;

		public override event Action<ClientConnectionStateArgs> OnClientConnectionState;

		public override event Action<ServerConnectionStateArgs> OnServerConnectionState;

		public override event Action<RemoteConnectionStateArgs> OnRemoteConnectionState;

		public override event Action<ClientReceivedDataArgs> OnClientReceivedData;

		public override event Action<ServerReceivedDataArgs> OnServerReceivedData;

		~Tugboat()
		{
			Shutdown();
		}

		public override void Initialize(NetworkManager networkManager, int transportIndex)
		{
			base.Initialize(networkManager, transportIndex);
			networkManager.TimeManager.OnUpdate += TimeManager_OnUpdate;
		}

		protected void OnDestroy()
		{
			Shutdown();
			if (base.NetworkManager != null)
			{
				base.NetworkManager.TimeManager.OnUpdate -= TimeManager_OnUpdate;
			}
		}

		public override string GetConnectionAddress(int connectionId)
		{
			return _server.GetConnectionAddress(connectionId);
		}

		public override LocalConnectionState GetConnectionState(bool server)
		{
			if (server)
			{
				return _server.GetConnectionState();
			}
			return _client.GetConnectionState();
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
			UpdateTimeout();
		}

		public override void HandleRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
		{
			OnRemoteConnectionState?.Invoke(connectionStateArgs);
		}

		private void TimeManager_OnUpdate()
		{
			_server?.PollSocket();
			_client?.PollSocket();
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
			if (server)
			{
				_server.IterateOutgoing();
			}
			else
			{
				_client.IterateOutgoing();
			}
		}

		public override void HandleClientReceivedDataArgs(ClientReceivedDataArgs receivedDataArgs)
		{
			OnClientReceivedData?.Invoke(receivedDataArgs);
		}

		public override void HandleServerReceivedDataArgs(ServerReceivedDataArgs receivedDataArgs)
		{
			OnServerReceivedData?.Invoke(receivedDataArgs);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			SanitizeChannel(ref channelId);
			_client.SendToServer(channelId, segment);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			SanitizeChannel(ref channelId);
			_server.SendToClient(channelId, segment, connectionId);
		}

		public void SetPacketLayer(PacketLayerBase packetLayer)
		{
			_packetLayer = packetLayer;
			if (GetConnectionState(server: true) != LocalConnectionState.Stopped)
			{
				base.NetworkManager.LogWarning("PacketLayer is set but will not be applied until the server stops.");
			}
			if (GetConnectionState(server: false) != LocalConnectionState.Stopped)
			{
				base.NetworkManager.LogWarning("PacketLayer is set but will not be applied until the client stops.");
			}
			_server.Initialize(this, _unreliableMTU, _packetLayer);
			_client.Initialize(this, _unreliableMTU, _packetLayer);
		}

		public override float GetTimeout(bool asServer)
		{
			return (int)_timeout;
		}

		public override void SetTimeout(float value, bool asServer)
		{
			_timeout = (ushort)value;
		}

		public override int GetMaximumClients()
		{
			return _server.GetMaximumClients();
		}

		public override void SetMaximumClients(int value)
		{
			_maximumClients = value;
			_server.SetMaximumClients(value);
		}

		public override void SetClientAddress(string address)
		{
			_clientAddress = address;
		}

		public override string GetClientAddress()
		{
			return _clientAddress;
		}

		public override void SetServerBindAddress(string address, IPAddressType addressType)
		{
			if (addressType == IPAddressType.IPv4)
			{
				_ipv4BindAddress = address;
			}
			else
			{
				_ipv6BindAddress = address;
			}
		}

		public override string GetServerBindAddress(IPAddressType addressType)
		{
			if (addressType == IPAddressType.IPv4)
			{
				return _ipv4BindAddress;
			}
			return _ipv6BindAddress;
		}

		public override void SetPort(ushort port)
		{
			_port = port;
		}

		public override ushort GetPort()
		{
			ushort? num = _server?.GetPort();
			if (num.HasValue)
			{
				return num.Value;
			}
			num = _client?.GetPort();
			if (num.HasValue)
			{
				return num.Value;
			}
			return _port;
		}

		public override bool StartConnection(bool server)
		{
			if (server)
			{
				return StartServer();
			}
			return StartClient(_clientAddress);
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
			return _server.StopConnection(connectionId);
		}

		public override void Shutdown()
		{
			StopConnection(server: false);
			StopConnection(server: true);
		}

		private bool StartServer()
		{
			_server.Initialize(this, _unreliableMTU, _packetLayer);
			UpdateTimeout();
			return _server.StartConnection(_port, _maximumClients, _ipv4BindAddress, _ipv6BindAddress);
		}

		private bool StopServer()
		{
			if (_server == null)
			{
				return false;
			}
			return _server.StopConnection();
		}

		private bool StartClient(string address)
		{
			_client.Initialize(this, _unreliableMTU, _packetLayer);
			UpdateTimeout();
			return _client.StartConnection(address, _port);
		}

		private void UpdateTimeout()
		{
			int timeout = (Application.isEditor ? 1800 : _timeout);
			_client.UpdateTimeout(timeout);
			_server.UpdateTimeout(timeout);
		}

		private bool StopClient()
		{
			if (_client == null)
			{
				return false;
			}
			return _client.StopConnection();
		}

		private void SanitizeChannel(ref byte channelId)
		{
			if (channelId < 0 || channelId >= 2)
			{
				base.NetworkManager.LogWarning($"Channel of {channelId} is out of range of supported channels. Channel will be defaulted to reliable.");
				channelId = 0;
			}
		}

		public override int GetMTU(byte channel)
		{
			return _unreliableMTU;
		}
	}
}
