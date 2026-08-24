using UnityEngine;
using UnityEngine.EventSystems;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/programming-tools/friendinvitebutton")]
	public abstract class UserInviteButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public UnityUserAndPointerDataEvent Click;

		public UserData UserData { get; protected set; }

		public void OnPointerClick(PointerEventData eventData)
		{
			Click.Invoke(new UserAndPointerData(UserData, eventData));
		}

		public abstract void SetFriend(UserData user);
	}
}
