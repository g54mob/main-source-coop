using System;
using Mimicraft.Analytics;
using Mimicraft.Localization;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class GameVersion
	{
		public const string MismatchReason = "Lobby.VersionMismatch";

		private const char ReasonSeparator = '|';

		public const string LobbyKey = "version";

		private static NetworkManager watched;

		public static string Current => $"{Application.version}/{Build.Kind}";

		public static bool Matches(string offered)
		{
			if (!string.IsNullOrEmpty(offered))
			{
				return string.Equals(offered, Current, StringComparison.Ordinal);
			}
			return false;
		}

		public static string Refusal()
		{
			return "Lobby.VersionMismatch|" + Current;
		}

		public static bool IsRefusal(string reason, out string hostVersion)
		{
			hostVersion = "";
			if (string.IsNullOrEmpty(reason) || !reason.StartsWith("Lobby.VersionMismatch", StringComparison.Ordinal))
			{
				return false;
			}
			int num = reason.IndexOf('|');
			if (num >= 0 && num + 1 < reason.Length)
			{
				hostVersion = reason.Substring(num + 1);
			}
			return true;
		}

		public static string RefusalMessage(string hostVersion)
		{
			if (string.IsNullOrEmpty(hostVersion))
			{
				return Loc.Get("Lobby.VersionMismatchUnknown");
			}
			int num = CompareNumbers(hostVersion, Current);
			if (num > 0)
			{
				return Loc.Format("Lobby.VersionMismatch", hostVersion, Current);
			}
			if (num < 0)
			{
				return Loc.Format("Lobby.VersionMismatchOlder", hostVersion, Current);
			}
			return Loc.Format("Lobby.VersionMismatchOther", hostVersion, Current);
		}

		public static bool CanJoin(string published, out string refusal)
		{
			refusal = "";
			if (Matches(published))
			{
				return true;
			}
			refusal = RefusalMessage(published);
			return false;
		}

		public static void ShowRefusal(string refusal)
		{
			DialogView.Show(new DialogRequest(refusal, Loc.Get("Common.Ok")), delegate
			{
			});
			Telemetry.Send("disconnect", ("reason", "version"), ("session_seconds", Telemetry.SessionSeconds));
		}

		private static int CompareNumbers(string a, string b)
		{
			if (!TryNumber(a, out var version) || !TryNumber(b, out var version2))
			{
				return 0;
			}
			return version.CompareTo(version2);
		}

		private static bool TryNumber(string value, out Version version)
		{
			int num = value.IndexOf('/');
			return Version.TryParse((num >= 0) ? value.Substring(0, num) : value, out version);
		}

		public static void WatchForRefusal(NetworkManager manager)
		{
			if (!(manager == null) && !(watched == manager))
			{
				if (watched != null)
				{
					watched.OnClientDisconnectCallback -= OnClientDisconnected;
				}
				watched = manager;
				watched.OnClientDisconnectCallback += OnClientDisconnected;
			}
		}

		private static void OnClientDisconnected(ulong clientId)
		{
			if (!(watched == null) && !watched.IsServer && clientId == watched.LocalClientId && IsRefusal(watched.DisconnectReason, out var hostVersion))
			{
				ShowRefusal(RefusalMessage(hostVersion));
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			watched = null;
		}
	}
}
