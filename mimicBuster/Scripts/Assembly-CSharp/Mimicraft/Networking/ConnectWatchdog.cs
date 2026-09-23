using System.Threading.Tasks;
using Mimicraft.Analytics;
using Mimicraft.Localization;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class ConnectWatchdog
	{
		public const float ConnectSeconds = 20f;

		public const string TimeoutKey = "Status.ConnectTimeout";

		private static int attempt;

		public static async void Watch(NetworkManager manager)
		{
			if (manager == null)
			{
				return;
			}
			int mine = ++attempt;
			float deadline = Time.realtimeSinceStartup + 20f;
			while (Time.realtimeSinceStartup < deadline)
			{
				await Task.Yield();
				if (mine != attempt || manager == null || manager.IsConnectedClient || (!manager.IsListening && !manager.ShutdownInProgress))
				{
					return;
				}
			}
			if (mine == attempt && !(manager == null) && !manager.IsConnectedClient)
			{
				GiveUp(manager);
			}
		}

		public static void Cancel()
		{
			attempt++;
		}

		private static void GiveUp(NetworkManager manager)
		{
			Debug.LogWarning($"[Baglanti] {20f:0} saniyede sunucuya baglanilamadi - vazgeciliyor.");
			if (manager.IsListening)
			{
				manager.Shutdown();
			}
			SteamManager.LeaveActiveLobby();
			LoadingScreen.Hide();
			DialogView.Show(new DialogRequest(Loc.Get("Status.ConnectTimeout"), Loc.Get("Common.Ok")), delegate
			{
			});
			Telemetry.Send("disconnect", ("reason", "timeout"), ("session_seconds", Telemetry.SessionSeconds));
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			attempt = 0;
		}
	}
}
