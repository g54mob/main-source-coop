using System.Collections.Generic;
using Photon.Realtime;

namespace Fusion.Matchmaking.Extensions
{
	internal static class RealtimeExtensionsRoomInfo
	{
		public static Dictionary<string, SessionProperty> GetCustomProperties(this RoomInfo roomInfo)
		{
			return roomInfo.CustomProperties.ConvertToDictionaryProperty();
		}
	}
}
