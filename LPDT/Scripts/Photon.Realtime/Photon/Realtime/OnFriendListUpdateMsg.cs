using System.Collections.Generic;

namespace Photon.Realtime
{
	public class OnFriendListUpdateMsg
	{
		public List<FriendInfo> friendList;

		internal OnFriendListUpdateMsg(List<FriendInfo> friendList)
		{
			this.friendList = friendList;
		}
	}
}
