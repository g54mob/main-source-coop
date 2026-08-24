using System;
using Steamworks;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct FriendRichPresenceUpdate
	{
		public FriendRichPresenceUpdate_t data;

		public UserData Friend => data.m_steamIDFriend;

		public AppData App => data.m_nAppID;

		public static implicit operator FriendRichPresenceUpdate(FriendRichPresenceUpdate_t native)
		{
			return new FriendRichPresenceUpdate
			{
				data = native
			};
		}

		public static implicit operator FriendRichPresenceUpdate_t(FriendRichPresenceUpdate heathen)
		{
			return heathen.data;
		}
	}
}
