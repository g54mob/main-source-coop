using System;
using System.Collections;
using Epic.OnlineServices;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.P2P;
using Mirror;
using UnityEngine;

namespace EpicTransport
{
	public class EosTransport : Transport
	{
		private const string EPIC_SCHEME = "epic";

		private Client client;

		private Server server;

		private Common activeNode;

		[SerializeField]
		public PacketReliability[] Channels = new PacketReliability[2]
		{
			PacketReliability.ReliableOrdered,
			PacketReliability.UnreliableUnordered
		};

		[Tooltip("Timeout for connecting in seconds.")]
		public int timeout = 25;

		[Tooltip("The max fragments used in fragmentation before throwing an error.")]
		public int maxFragments = 55;

		public float ignoreCachedMessagesAtStartUpInSeconds = 2f;

		private float ignoreCachedMessagesTimer;

		public RelayControl relayControl = RelayControl.AllowRelays;

		[Header("Info")]
		[Tooltip("This will display your Epic Account ID when you start or connect to a server.")]
		public ProductUserId productUserId;

		private int packetId;

		private void Awake()
		{
			if (Channels[0] != PacketReliability.ReliableOrdered)
			{
				Debug.LogWarning("EOS Transport Channel[0] is not ReliableOrdered, Mirror expects Channel 0 to be ReliableOrdered, only change this if you know what you are doing.");
			}
			if (Channels[1] != PacketReliability.UnreliableUnordered)
			{
				Debug.LogWarning("EOS Transport Channel[1] is not UnreliableUnordered, Mirror expects Channel 1 to be UnreliableUnordered, only change this if you know what you are doing.");
			}
			StartCoroutine("FetchEpicAccountId");
			StartCoroutine("ChangeRelayStatus");
		}

		public override void ClientEarlyUpdate()
		{
			EOSSDKComponent.Tick();
			if (activeNode != null)
			{
				ignoreCachedMessagesTimer += Time.deltaTime;
				if (ignoreCachedMessagesTimer <= ignoreCachedMessagesAtStartUpInSeconds)
				{
					activeNode.ignoreAllMessages = true;
				}
				else
				{
					activeNode.ignoreAllMessages = false;
					if (client != null && !client.isConnecting)
					{
						if (EOSSDKComponent.Initialized)
						{
							client.Connect(client.hostAddress);
						}
						else
						{
							Debug.LogError("EOS not initialized");
							client.EosNotInitialized();
						}
						client.isConnecting = true;
					}
				}
			}
			if (base.enabled)
			{
				activeNode?.ReceiveData();
			}
		}

		public override void ClientLateUpdate()
		{
		}

		public override void ServerEarlyUpdate()
		{
			EOSSDKComponent.Tick();
			if (activeNode != null)
			{
				ignoreCachedMessagesTimer += Time.deltaTime;
				if (ignoreCachedMessagesTimer <= ignoreCachedMessagesAtStartUpInSeconds)
				{
					activeNode.ignoreAllMessages = true;
				}
				else
				{
					activeNode.ignoreAllMessages = false;
				}
			}
			if (base.enabled)
			{
				activeNode?.ReceiveData();
			}
		}

		public override void ServerLateUpdate()
		{
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
			if (!EOSSDKComponent.Initialized)
			{
				Debug.LogError("EOS not initialized. Client could not be started.");
				OnClientDisconnected();
				return;
			}
			StartCoroutine("FetchEpicAccountId");
			if (ServerActive())
			{
				Debug.LogError("Transport already running as server!");
			}
			else if (!ClientActive() || client.Error)
			{
				Debug.Log("Starting client, target address " + address + ".");
				client = Client.CreateClient(this, address);
				activeNode = client;
				if (EOSSDKComponent.CollectPlayerMetrics)
				{
					BeginPlayerSessionOptions options = new BeginPlayerSessionOptions
					{
						AccountId = new BeginPlayerSessionOptionsAccountId
						{
							Epic = EOSSDKComponent.LocalUserAccountId
						},
						ControllerType = UserControllerType.Unknown,
						DisplayName = EOSSDKComponent.DisplayName,
						GameSessionId = null,
						ServerIp = null
					};
					if (EOSSDKComponent.GetMetricsInterface().BeginPlayerSession(ref options) == Result.Success)
					{
						Debug.Log("Started Metric Session");
					}
				}
			}
			else
			{
				Debug.LogError("Client already running!");
			}
		}

		public override void ClientConnect(Uri uri)
		{
			if (uri.Scheme != "epic")
			{
				throw new ArgumentException(string.Format("Invalid url {0}, use {1}://EpicAccountId instead", uri, "epic"), "uri");
			}
			ClientConnect(uri.Host);
		}

		public override void ClientSend(ArraySegment<byte> segment, int channelId = 0)
		{
			Send(channelId, segment);
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
			if (!EOSSDKComponent.Initialized)
			{
				Debug.LogError("EOS not initialized. Server could not be started.");
				return;
			}
			StartCoroutine("FetchEpicAccountId");
			if (ClientActive())
			{
				Debug.LogError("Transport already running as client!");
			}
			else if (!ServerActive())
			{
				Debug.Log("Starting server.");
				server = Server.CreateServer(this, NetworkManager.singleton.maxConnections);
				activeNode = server;
				if (EOSSDKComponent.CollectPlayerMetrics)
				{
					BeginPlayerSessionOptions options = new BeginPlayerSessionOptions
					{
						AccountId = new BeginPlayerSessionOptionsAccountId
						{
							Epic = EOSSDKComponent.LocalUserAccountId
						},
						ControllerType = UserControllerType.Unknown,
						DisplayName = EOSSDKComponent.DisplayName,
						GameSessionId = null,
						ServerIp = null
					};
					if (EOSSDKComponent.GetMetricsInterface().BeginPlayerSession(ref options) == Result.Success)
					{
						Debug.Log("Started Metric Session");
					}
				}
			}
			else
			{
				Debug.LogError("Server already started!");
			}
		}

		public override Uri ServerUri()
		{
			return new UriBuilder
			{
				Scheme = "epic",
				Host = EOSSDKComponent.LocalUserProductIdString
			}.Uri;
		}

		public override void ServerSend(int connectionId, ArraySegment<byte> segment, int channelId = 0)
		{
			if (ServerActive())
			{
				Send(channelId, segment, connectionId);
			}
		}

		public override void ServerDisconnect(int connectionId)
		{
			server.Disconnect(connectionId);
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

		private void Send(int channelId, ArraySegment<byte> segment, int connectionId = -2147483648)
		{
			Packet[] packetArray = GetPacketArray(channelId, segment);
			for (int i = 0; i < packetArray.Length; i++)
			{
				if (connectionId == -2147483648)
				{
					client.Send(packetArray[i].ToBytes(), channelId);
				}
				else
				{
					server.SendAll(connectionId, packetArray[i].ToBytes(), channelId);
				}
			}
			packetId++;
		}

		private Packet[] GetPacketArray(int channelId, ArraySegment<byte> segment)
		{
			Packet[] array = new Packet[Mathf.CeilToInt((float)segment.Count / (float)GetMaxSinglePacketSize(channelId))];
			for (int i = 0; i < segment.Count; i += GetMaxSinglePacketSize(channelId))
			{
				int num = i / GetMaxSinglePacketSize(channelId);
				array[num] = default(Packet);
				array[num].id = packetId;
				array[num].fragment = num;
				array[num].moreFragments = segment.Count - i > GetMaxSinglePacketSize(channelId);
				array[num].data = new byte[(segment.Count - i > GetMaxSinglePacketSize(channelId)) ? GetMaxSinglePacketSize(channelId) : (segment.Count - i)];
				Array.Copy(segment.Array, i, array[num].data, 0, array[num].data.Length);
			}
			return array;
		}

		public override void Shutdown()
		{
			if (EOSSDKComponent.CollectPlayerMetrics)
			{
				EndPlayerSessionOptions options = new EndPlayerSessionOptions
				{
					AccountId = new EndPlayerSessionOptionsAccountId
					{
						Epic = EOSSDKComponent.LocalUserAccountId
					}
				};
				if (EOSSDKComponent.GetMetricsInterface().EndPlayerSession(ref options) == Result.Success)
				{
					Debug.LogError("Stopped Metric Session");
				}
			}
			try
			{
				server?.Shutdown();
			}
			catch (Exception arg)
			{
				Debug.LogError($"[EosTransport] Server shutdown failed: {arg}");
			}
			try
			{
				client?.Disconnect();
			}
			catch (Exception arg2)
			{
				Debug.LogError($"[EosTransport] Client disconnect failed: {arg2}");
			}
			server = null;
			client = null;
			activeNode = null;
			Debug.Log("Transport shut down.");
		}

		public int GetMaxSinglePacketSize(int channelId)
		{
			return 1160;
		}

		public override int GetMaxPacketSize(int channelId)
		{
			return 1170 * maxFragments;
		}

		public override int GetBatchThreshold(int channelId = 0)
		{
			return 1170;
		}

		public override bool Available()
		{
			try
			{
				return EOSSDKComponent.Initialized;
			}
			catch
			{
				return false;
			}
		}

		private IEnumerator FetchEpicAccountId()
		{
			while (!EOSSDKComponent.Initialized)
			{
				yield return null;
			}
			productUserId = EOSSDKComponent.LocalUserProductId;
		}

		private IEnumerator ChangeRelayStatus()
		{
			while (!EOSSDKComponent.Initialized)
			{
				yield return null;
			}
			P2PInterface p2PInterface = EOSSDKComponent.GetP2PInterface();
			while (p2PInterface == null)
			{
				yield return null;
				p2PInterface = EOSSDKComponent.GetP2PInterface();
			}
			SetRelayControlOptions options = new SetRelayControlOptions
			{
				RelayControl = relayControl
			};
			p2PInterface.SetRelayControl(ref options);
		}

		public void ResetIgnoreMessagesAtStartUpTimer()
		{
			ignoreCachedMessagesTimer = 0f;
		}

		private void OnDestroy()
		{
			if (activeNode != null)
			{
				Shutdown();
			}
		}
	}
}
