using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/programming-tools/friendinvitebutton")]
	public class BasicFriendInviteButton : UserInviteButton
	{
		public SetUserAvatar avatar;

		public SetUserName displayName;

		public SetUserStatus status;

		public override void SetFriend(UserData user)
		{
			base.UserData = user;
			avatar.UserData = user;
			displayName.UserData = user;
			status.UserData = user;
		}
	}
}
