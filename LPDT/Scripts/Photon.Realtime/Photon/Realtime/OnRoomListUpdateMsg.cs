using System.Collections.Generic;

namespace Photon.Realtime
{
	public class OnRoomListUpdateMsg
	{
		public List<RoomInfo> roomList;

		internal OnRoomListUpdateMsg(List<RoomInfo> roomList)
		{
			this.roomList = roomList;
		}
	}
}
