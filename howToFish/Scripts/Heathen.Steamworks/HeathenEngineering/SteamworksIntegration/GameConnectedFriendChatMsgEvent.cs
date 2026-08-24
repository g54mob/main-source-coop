using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public class GameConnectedFriendChatMsgEvent : UnityEvent<UserData, string, EChatEntryType>
	{
	}
}
