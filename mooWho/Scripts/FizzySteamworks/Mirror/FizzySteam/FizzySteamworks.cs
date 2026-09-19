using System;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	[HelpURL("https://github.com/Chykary/FizzySteamworks")]
	public class FizzySteamworks : Transport
	{
		private const string STEAM_SCHEME = "steam";

		private static IClient client;

		private static IServer server;

		[SerializeField]
		public EP2PSend[] Channels = new EP2PSend[2]
		{
			EP2PSend.k_EP2PSendReliable,
			EP2PSend.k_EP2PSendUnreliableNoDelay
		};

		[Tooltip("Timeout for connecting in seconds.")]
		public int Timeout = 25;

		[Tooltip("Allow or disallow P2P connections to fall back to being relayed through the Steam servers if a direct connection or NAT-traversal cannot be established.")]
		public bool AllowSteamRelay = true;

		[Tooltip("Use SteamSockets instead of the (deprecated) SteamNetworking. This will always use Relay.")]
		public bool UseNextGenSteamNetworking = true;

		private void OnEnable()
		{
			Invoke("InitRelayNetworkAccess", 1f);
		}

		public override void ClientEarlyUpdate()
		{
			if (base.enabled)
			{
				client?.ReceiveData();
			}
		}

		public override void ServerEarlyUpdate()
		{
			if (base.enabled)
			{
				server?.ReceiveData();
			}
		}

		public override void ClientLateUpdate()
		{
			if (base.enabled)
			{
				client?.FlushData();
			}
		}

		public override void ServerLateUpdate()
		{
			if (base.enabled)
			{
				server?.FlushData();
			}
		}

		public override bool ClientConnected()
		{
			if (ClientActive())
			{
				return client.Connected;
			}
			return false;
		}

		public override void ClientConnect(string address)
		{
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				InitRelayNetworkAccess();
				if (ServerActive())
				{
					Debug.LogError("Transport already running as server!");
				}
				else if (!ClientActive() || client.Error)
				{
					if (UseNextGenSteamNetworking)
					{
						Debug.Log("Starting client [SteamSockets], target address " + address + ".");
						client = NextClient.CreateClient(this, address);
					}
					else
					{
						Debug.Log($"Starting client [DEPRECATED SteamNetworking], target address {address}. Relay enabled: {AllowSteamRelay}");
						SteamNetworking.AllowP2PPacketRelay(AllowSteamRelay);
						client = LegacyClient.CreateClient(this, address);
					}
				}
				else
				{
					Debug.LogError("Client already running!");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception: " + ex.Message + ". Client could not be started.");
				OnClientDisconnected();
			}
		}

		public override void ClientConnect(Uri uri)
		{
			if (uri.Scheme != "steam")
			{
				throw new ArgumentException(string.Format("Invalid url {0}, use {1}://SteamID instead", uri, "steam"), "uri");
			}
			ClientConnect(uri.Host);
		}

		public override void ClientSend(ArraySegment<byte> segment, int channelId)
		{
			byte[] array = new byte[segment.Count];
			Array.Copy(segment.Array, segment.Offset, array, 0, segment.Count);
			client.Send(array, channelId);
		}

		public override void ClientDisconnect()
		{
			if (ClientActive())
			{
				Shutdown();
			}
		}

		public bool ClientActive()
		{
			return client != null;
		}

		public override bool ServerActive()
		{
			return server != null;
		}

		public override void ServerStart()
		{
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				InitRelayNetworkAccess();
				if (ClientActive())
				{
					Debug.LogError("Transport already running as client!");
				}
				else if (!ServerActive())
				{
					if (UseNextGenSteamNetworking)
					{
						Debug.Log("Starting server [SteamSockets].");
						server = NextServer.CreateServer(this, NetworkManager.singleton.maxConnections);
					}
					else
					{
						Debug.Log($"Starting server [DEPRECATED SteamNetworking]. Relay enabled: {AllowSteamRelay}");
						SteamNetworking.AllowP2PPacketRelay(AllowSteamRelay);
						server = LegacyServer.CreateServer(this, NetworkManager.singleton.maxConnections);
					}
				}
				else
				{
					Debug.LogError("Server already started!");
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		public override Uri ServerUri()
		{
			return new UriBuilder
			{
				Scheme = "steam",
				Host = SteamUser.GetSteamID().m_SteamID.ToString()
			}.Uri;
		}

		public override void ServerSend(int connectionId, ArraySegment<byte> segment, int channelId)
		{
			if (ServerActive())
			{
				byte[] array = new byte[segment.Count];
				Array.Copy(segment.Array, segment.Offset, array, 0, segment.Count);
				server.Send(connectionId, array, channelId);
			}
		}

		public override void ServerDisconnect(int connectionId)
		{
			if (ServerActive())
			{
				server.Disconnect(connectionId);
			}
		}

		public override string ServerGetClientAddress(int connectionId)
		{
			if (!ServerActive())
			{
				return string.Empty;
			}
			return server.ServerGetClientAddress(connectionId);
		}

		public override void ServerStop()
		{
			if (ServerActive())
			{
				Shutdown();
			}
		}

		public override void Shutdown()
		{
			if (client != null)
			{
				client.Disconnect();
				client = null;
				Debug.Log("Transport shut down - client.");
			}
			if (server != null)
			{
				server.Shutdown();
				server = null;
				Debug.Log("Transport shut down - server.");
			}
		}

		public override int GetMaxPacketSize(int channelId)
		{
			if (UseNextGenSteamNetworking)
			{
				return 524288;
			}
			if (channelId >= Channels.Length)
			{
				Debug.LogError("Channel Id exceeded configured channels! Please configure more channels.");
				return 1200;
			}
			switch (Channels[channelId])
			{
			case EP2PSend.k_EP2PSendUnreliable:
			case EP2PSend.k_EP2PSendUnreliableNoDelay:
				return 1200;
			case EP2PSend.k_EP2PSendReliable:
			case EP2PSend.k_EP2PSendReliableWithBuffering:
				return 1048576;
			default:
				throw new NotSupportedException();
			}
		}

		public override bool Available()
		{
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void InitRelayNetworkAccess()
		{
			try
			{
				if (UseNextGenSteamNetworking)
				{
					SteamNetworkingUtils.InitRelayNetworkAccess();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to initialize relay network access: " + ex.Message);
			}
		}
	}
}
