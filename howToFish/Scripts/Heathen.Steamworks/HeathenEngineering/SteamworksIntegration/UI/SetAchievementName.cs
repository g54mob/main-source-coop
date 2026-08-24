using HeathenEngineering.SteamworksIntegration.API;
using TMPro;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class SetAchievementName : MonoBehaviour
	{
		public AchievementObject achievement;

		private TextMeshProUGUI displayName;

		private void Start()
		{
			displayName = GetComponent<TextMeshProUGUI>();
			if (achievement != null)
			{
				if (App.Initialized)
				{
					displayName.text = achievement.Name;
				}
				else
				{
					App.evtSteamInitialized.AddListener(Refresh);
				}
			}
		}

		public void Refresh()
		{
			displayName.text = achievement.Name;
			App.evtSteamInitialized.RemoveListener(Refresh);
		}
	}
}
