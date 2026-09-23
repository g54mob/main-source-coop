using System;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class LobbyPassword
	{
		public const string WrongPasswordReason = "Lobby.WrongPassword";

		private static string offered = "";

		private const string PayloadMarker = "MIMICRAFT/1";

		private const char PayloadSeparator = '\n';

		private const int MaxPayloadBytes = 1024;

		public static string Hosted { get; private set; } = "";

		public static bool Locked => !string.IsNullOrEmpty(Hosted);

		public static void Prepare(NetworkManager manager)
		{
			if (!(manager == null))
			{
				manager.NetworkConfig.ConnectionApproval = true;
				manager.ConnectionApprovalCallback = Approve;
				manager.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(Pack(offered));
				GameVersion.WatchForRefusal(manager);
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			Hosted = "";
			offered = "";
		}

		public static void Host(NetworkManager manager, string password)
		{
			LobbyBans.Clear();
			Hosted = password ?? "";
			Prepare(manager);
			Offer(manager, Hosted);
		}

		public static void Offer(NetworkManager manager, string password)
		{
			offered = password ?? "";
			if (manager != null)
			{
				Prepare(manager);
			}
		}

		private static string Pack(string password)
		{
			return "MIMICRAFT/1\n" + GameVersion.Current + "\n" + password;
		}

		private static void Unpack(byte[] payload, out string version, out string password)
		{
			version = "";
			password = "";
			if (payload != null && payload.Length != 0 && payload.Length <= 1024)
			{
				string text = Encoding.UTF8.GetString(payload);
				string[] array = text.Split('\n');
				if (array.Length < 3 || array[0] != "MIMICRAFT/1")
				{
					password = text;
					return;
				}
				version = array[1];
				password = string.Join('\n'.ToString(), array, 2, array.Length - 2);
			}
		}

		public static void Clear(NetworkManager manager)
		{
			Hosted = "";
			offered = "";
			if (manager != null)
			{
				manager.NetworkConfig.ConnectionData = Array.Empty<byte>();
			}
		}

		private static void Approve(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			bool flag = singleton != null && request.ClientNetworkId == singleton.LocalClientId;
			Unpack(request.Payload, out var version, out var password);
			bool flag2 = flag || !Locked || string.Equals(password, Hosted, StringComparison.Ordinal);
			string text = (flag2 ? "" : "Lobby.WrongPassword");
			if (flag2 && !flag && !GameVersion.Matches(version))
			{
				flag2 = false;
				text = GameVersion.Refusal();
			}
			if (flag2 && !flag && !LobbyCapacity.HasRoom(singleton))
			{
				flag2 = false;
				text = "Lobby.Full";
			}
			if (flag2 && !flag && LobbyBans.IsBanned(LobbyBans.SteamIdOf(request.ClientNetworkId)))
			{
				flag2 = false;
				text = "Lobby.Banned";
			}
			response.Approved = flag2;
			response.Reason = text;
			response.CreatePlayerObject = flag2;
			response.Pending = false;
			if (!flag2)
			{
				string hostVersion;
				string arg = ((text == "Lobby.Full") ? "lobi dolu." : ((text == "Lobby.Banned") ? "bu lobiden atilmis." : (GameVersion.IsRefusal(text, out hostVersion) ? ("surum uyusmuyor (istemci '" + version + "', sunucu '" + GameVersion.Current + "').") : "yanlis sifre.")));
				Debug.Log($"[LobbyPassword] {request.ClientNetworkId} numarali istemci reddedildi: {arg}");
			}
		}
	}
}
