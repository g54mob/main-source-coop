using System.Collections.Generic;
using Netcode.Transports.Facepunch;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class LobbyBans
	{
		public const string BannedReason = "Lobby.Banned";

		private static readonly HashSet<ulong> banned = new HashSet<ulong>();

		public static int Count => banned.Count;

		public static bool Supported
		{
			get
			{
				NetworkManager singleton = NetworkManager.Singleton;
				if (singleton != null && singleton.GetComponent<FacepunchTransport>() != null)
				{
					return SteamManager.IsInitialized;
				}
				return false;
			}
		}

		public static void Clear()
		{
			banned.Clear();
		}

		public static bool IsBanned(ulong steamId)
		{
			if (steamId != 0L)
			{
				return banned.Contains(steamId);
			}
			return false;
		}

		public static bool Ban(ulong clientId)
		{
			ulong num = SteamIdOf(clientId);
			if (num == 0L)
			{
				return false;
			}
			banned.Add(num);
			return true;
		}

		public static ulong SteamIdOf(ulong clientId)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null)
			{
				return 0uL;
			}
			FacepunchTransport component = singleton.GetComponent<FacepunchTransport>();
			if (component == null)
			{
				return 0uL;
			}
			if (!component.TryGetSteamId(clientId, out var steamId))
			{
				return 0uL;
			}
			return steamId.Value;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			banned.Clear();
		}
	}
}
