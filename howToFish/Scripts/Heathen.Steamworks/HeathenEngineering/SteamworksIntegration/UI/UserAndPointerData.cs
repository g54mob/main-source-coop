using System;
using UnityEngine.EventSystems;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[Serializable]
	public class UserAndPointerData
	{
		public UserData user;

		public PointerEventData pointerEventData;

		public UserAndPointerData(UserData userData, PointerEventData data)
		{
			user = userData;
			pointerEventData = data;
		}
	}
}
