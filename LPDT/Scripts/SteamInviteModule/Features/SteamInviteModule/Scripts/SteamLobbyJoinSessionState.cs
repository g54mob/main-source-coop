using UnityEngine;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamLobbyJoinSessionState : MonoBehaviour
	{
		public static bool HasConsumedLaunchLobbyJoin { get; private set; }

		public static bool SuppressAutoFusionJoinFromSteamLobby { get; private set; }

		public static void MarkLaunchLobbyJoinConsumed()
		{
			HasConsumedLaunchLobbyJoin = true;
		}
	}
}
