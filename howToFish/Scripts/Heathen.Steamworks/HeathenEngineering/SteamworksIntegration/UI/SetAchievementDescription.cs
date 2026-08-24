using HeathenEngineering.SteamworksIntegration.API;
using TMPro;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class SetAchievementDescription : MonoBehaviour
	{
		public AchievementObject achievement;

		private TextMeshProUGUI description;

		private void Start()
		{
			description = GetComponent<TextMeshProUGUI>();
			if (achievement != null)
			{
				if (App.Initialized)
				{
					description.text = achievement.Description;
				}
				else
				{
					App.evtSteamInitialized.AddListener(Refresh);
				}
			}
		}

		public void Refresh()
		{
			description.text = achievement.Description;
			App.evtSteamInitialized.RemoveListener(Refresh);
		}
	}
}
