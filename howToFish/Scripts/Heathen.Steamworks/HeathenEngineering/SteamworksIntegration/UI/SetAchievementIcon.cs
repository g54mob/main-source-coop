using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[RequireComponent(typeof(RawImage))]
	public class SetAchievementIcon : MonoBehaviour
	{
		public AchievementObject achievement;

		private RawImage image;

		private void Start()
		{
			image = GetComponent<RawImage>();
			if (!(achievement != null))
			{
				return;
			}
			achievement.StatusChanged.AddListener(HandleUpdate);
			if (App.Initialized)
			{
				achievement.GetIcon(delegate(Texture2D texture)
				{
					image.texture = texture;
				});
			}
			else
			{
				App.evtSteamInitialized.AddListener(Refresh);
			}
		}

		private void HandleUpdate(bool arg0)
		{
			Refresh();
		}

		public void Refresh()
		{
			Debug.Log(base.name + " Updated");
			achievement.GetIcon(delegate(Texture2D texture)
			{
				image.texture = texture;
			});
			App.evtSteamInitialized.RemoveListener(Refresh);
		}
	}
}
