using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class LobbyCapacity
	{
		public const int Max = 16;

		public const int Min = 2;

		public const string FullReason = "Lobby.Full";

		public static int Hosted { get; private set; } = 16;

		public static void Host(int players)
		{
			Hosted = Clamp(players);
		}

		public static int Clamp(int players)
		{
			if (players > 0)
			{
				return Mathf.Clamp(players, 2, 16);
			}
			return 16;
		}

		public static bool HasRoom(NetworkManager manager)
		{
			if (!(manager == null))
			{
				return manager.ConnectedClientsIds.Count < Hosted;
			}
			return true;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			Hosted = 16;
		}
	}
}
