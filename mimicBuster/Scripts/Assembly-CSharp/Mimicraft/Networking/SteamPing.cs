using System;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class SteamPing
	{
		public const string LobbyKey = "ping_location";

		public const int Unknown = -1;

		private static bool requested;

		public static bool IsReady
		{
			get
			{
				if (!SteamClient.IsValid)
				{
					return false;
				}
				try
				{
					return SteamNetworkingUtils.Status == SteamNetworkingAvailability.Current;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public static string LocalLocation
		{
			get
			{
				if (!SteamClient.IsValid)
				{
					return "";
				}
				try
				{
					NetPingLocation? localPingLocation = SteamNetworkingUtils.LocalPingLocation;
					return localPingLocation.HasValue ? localPingLocation.Value.ToString() : "";
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[SteamPing] Konum okunamadı: " + ex.Message);
					return "";
				}
			}
		}

		public static void Prepare()
		{
			if (requested || !SteamClient.IsValid)
			{
				return;
			}
			requested = true;
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
			}
			catch (Exception ex)
			{
				requested = false;
				Debug.LogWarning("[SteamPing] Relay ağı başlatılamadı: " + ex.Message);
			}
		}

		public static async Task WaitAsync(float seconds)
		{
			if (!SteamClient.IsValid || IsReady)
			{
				return;
			}
			Prepare();
			try
			{
				await SteamNetworkingUtils.WaitForPingDataAsync(seconds);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[SteamPing] Ping verisi beklenirken hata: " + ex.Message);
			}
		}

		public static int EstimateMs(string location)
		{
			if (string.IsNullOrEmpty(location) || !SteamClient.IsValid)
			{
				return -1;
			}
			try
			{
				NetPingLocation? netPingLocation = NetPingLocation.TryParseFromString(location);
				if (!netPingLocation.HasValue)
				{
					return -1;
				}
				int num = SteamNetworkingUtils.EstimatePingTo(netPingLocation.Value);
				return (num < 0) ? (-1) : num;
			}
			catch (Exception)
			{
				return -1;
			}
		}
	}
}
