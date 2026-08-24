using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[HelpURL("https://kb.heathenengineering.com/assets/steamworks")]
	[DisallowMultipleComponent]
	public class FriendManager : MonoBehaviour
	{
		public GameConnectedFriendChatMsgEvent evtGameConnectedChatMsg;

		public FriendRichPresenceUpdateEvent evtRichPresenceUpdated;

		public PersonaStateChangeEvent evtPersonaStateChanged;

		public bool ListenForFriendsMessages
		{
			get
			{
				return Friends.Client.ListenForFriendsMessages;
			}
			set
			{
				Friends.Client.ListenForFriendsMessages = value;
			}
		}

		private void OnEnable()
		{
			Friends.Client.EventGameConnectedFriendChatMsg.AddListener(evtGameConnectedChatMsg.Invoke);
			Friends.Client.EventFriendRichPresenceUpdate.AddListener(evtRichPresenceUpdated.Invoke);
			Friends.Client.EventPersonaStateChange.AddListener(evtPersonaStateChanged.Invoke);
		}

		private void OnDisable()
		{
			Friends.Client.EventGameConnectedFriendChatMsg.RemoveListener(evtGameConnectedChatMsg.Invoke);
			Friends.Client.EventFriendRichPresenceUpdate.RemoveListener(evtRichPresenceUpdated.Invoke);
			Friends.Client.EventPersonaStateChange.RemoveListener(evtPersonaStateChanged.Invoke);
		}

		public UserData[] GetFriends(EFriendFlags flags)
		{
			return Friends.Client.GetFriends(flags);
		}

		public UserData[] GetCoplayFriends()
		{
			return Friends.Client.GetCoplayFriends();
		}

		public string GetFriendMessage(UserData userId, int index, out EChatEntryType type)
		{
			return Friends.Client.GetFriendMessage(userId, index, out type);
		}

		public bool SendMessage(UserData friend, string message)
		{
			return Friends.Client.ReplyToFriendMessage(friend, message);
		}
	}
}
