using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class LanBeaconBroadcaster : MonoBehaviour
	{
		public const int DiscoveryPort = 47778;

		private const float BroadcastInterval = 1f;

		private UdpClient client;

		private LobbySettingsData settings;

		private ushort hostPort;

		private float nextSendTime;

		private bool isBroadcasting;

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void StartBroadcasting(LobbySettingsData settings, ushort port)
		{
			this.settings = settings;
			hostPort = port;
			if (client == null)
			{
				client = new UdpClient();
				client.EnableBroadcast = true;
			}
			isBroadcasting = true;
			nextSendTime = 0f;
		}

		public void StopBroadcasting()
		{
			isBroadcasting = false;
		}

		private static string CurrentPhase()
		{
			GameModeController current = GameModeController.Current;
			if (!(current != null))
			{
				return "waiting";
			}
			return LobbyPhase.Of(current.CurrentPhase.Value);
		}

		private void Update()
		{
			if (!isBroadcasting || client == null || Time.unscaledTime < nextSendTime)
			{
				return;
			}
			nextSendTime = Time.unscaledTime + 1f;
			try
			{
				string s = LanBeaconPacket.Serialize(settings, hostPort, CountPlayers(), LobbyCapacity.Hosted, CurrentPhase());
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				client.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Broadcast, 47778));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("LAN beacon gönderilemedi: " + ex.Message);
			}
		}

		private static int CountPlayers()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (!(singleton != null) || !singleton.IsListening)
			{
				return 0;
			}
			return singleton.ConnectedClientsIds.Count;
		}

		private void OnDestroy()
		{
			client?.Close();
		}

		private void OnApplicationQuit()
		{
			client?.Close();
		}
	}
}
