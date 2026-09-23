using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class LanBeaconListener : MonoBehaviour
	{
		private const float StaleTimeout = 5f;

		private UdpClient client;

		private readonly Dictionary<string, LanLobbyInfo> lobbiesByEndpoint = new Dictionary<string, LanLobbyInfo>();

		public void StartListening()
		{
			if (client != null)
			{
				return;
			}
			try
			{
				client = new UdpClient();
				client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
				client.Client.Bind(new IPEndPoint(IPAddress.Any, 47778));
				client.EnableBroadcast = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("LAN dinleme başlatılamadı: " + ex.Message);
				client = null;
			}
		}

		public void StopListening()
		{
			client?.Close();
			client = null;
			lobbiesByEndpoint.Clear();
		}

		private void Update()
		{
			if (client == null)
			{
				return;
			}
			while (client.Available > 0)
			{
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
				byte[] bytes;
				try
				{
					bytes = client.Receive(ref remoteEP);
				}
				catch (Exception)
				{
					break;
				}
				string raw = Encoding.UTF8.GetString(bytes);
				if (LanBeaconPacket.TryParse(raw, out var settings, out var port, out var players, out var maxPlayers))
				{
					string key = $"{remoteEP.Address}:{port}";
					lobbiesByEndpoint[key] = new LanLobbyInfo
					{
						Ip = remoteEP.Address.ToString(),
						Port = port,
						Settings = settings,
						LastSeenTime = Time.unscaledTime,
						Players = players,
						MaxPlayers = maxPlayers,
						TickRate = LanBeaconPacket.TickRateOf(raw),
						Version = LanBeaconPacket.VersionOf(raw),
						Phase = LanBeaconPacket.PhaseOf(raw)
					};
				}
			}
			List<string> list = null;
			foreach (KeyValuePair<string, LanLobbyInfo> item in lobbiesByEndpoint)
			{
				if (Time.unscaledTime - item.Value.LastSeenTime > 5f)
				{
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(item.Key);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (string item2 in list)
			{
				lobbiesByEndpoint.Remove(item2);
			}
		}

		public IEnumerable<LanLobbyInfo> GetLobbies()
		{
			return lobbiesByEndpoint.Values;
		}

		private void OnDestroy()
		{
			StopListening();
		}
	}
}
