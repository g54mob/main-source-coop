using Photon.Pun.UtilityScripts;
using UnityEngine;
using Zorro.Core.CLI;

namespace DefaultNamespace._10._SCRIPTS
{
	public class NetworkStats : RetrievableSingleton<NetworkStats>
	{
		protected override void OnCreated()
		{
			base.OnCreated();
			Object.DontDestroyOnLoad(base.gameObject);
			base.gameObject.AddComponent<PhotonStatsGui>();
		}

		[ConsoleCommand]
		public static void ShowStats()
		{
			RetrievableSingleton<NetworkStats>.Instance.gameObject.name = "NetworkStats";
		}
	}
}
