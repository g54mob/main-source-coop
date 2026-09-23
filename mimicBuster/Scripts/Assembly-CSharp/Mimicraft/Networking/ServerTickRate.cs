using System.Globalization;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class ServerTickRate
	{
		public const uint Default = 30u;

		public const uint High = 64u;

		public const uint Ultra = 128u;

		public const uint Min = 10u;

		public const uint Max = 256u;

		public const uint Unpublished = 30u;

		public const string LobbyKey = "tickrate";

		private const string PrefKey = "Mimicraft.Net.ServerTickRate";

		public static uint Preferred
		{
			get
			{
				return Clamp(PlayerPrefs.GetInt("Mimicraft.Net.ServerTickRate", 30));
			}
			set
			{
				PlayerPrefs.SetInt("Mimicraft.Net.ServerTickRate", (int)Clamp((int)value));
				PlayerPrefs.Save();
			}
		}

		public static uint Active
		{
			get
			{
				NetworkManager singleton = NetworkManager.Singleton;
				if (!(singleton != null))
				{
					return 0u;
				}
				return singleton.NetworkConfig.TickRate;
			}
		}

		public static void ApplyForHosting(NetworkManager manager)
		{
			if (manager != null)
			{
				manager.NetworkConfig.TickRate = Preferred;
			}
		}

		public static void ApplyForJoining(NetworkManager manager, string published)
		{
			if (!(manager == null))
			{
				manager.NetworkConfig.TickRate = ((uint.TryParse(published, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) && result != 0) ? Clamp((int)result) : 30u);
			}
		}

		public static void ApplyForDirectJoin(NetworkManager manager)
		{
			if (manager != null)
			{
				manager.NetworkConfig.TickRate = Preferred;
			}
		}

		public static bool TryParse(string text, out uint rate)
		{
			rate = 0u;
			switch ((text ?? "").Trim().ToLowerInvariant())
			{
			case "default":
			case "standard":
				rate = 30u;
				return true;
			case "high":
				rate = 64u;
				return true;
			case "ultra":
				rate = 128u;
				return true;
			default:
			{
				if (!uint.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) || result < 10 || result > 256)
				{
					return false;
				}
				rate = result;
				return true;
			}
			}
		}

		public static string Describe(uint rate)
		{
			return rate switch
			{
				30u => $"{rate} (default)", 
				64u => $"{rate} (high)", 
				128u => $"{rate} (ultra)", 
				_ => $"{rate} (custom)", 
			};
		}

		private static uint Clamp(int value)
		{
			return (uint)Mathf.Clamp(value, 10, 256);
		}
	}
}
