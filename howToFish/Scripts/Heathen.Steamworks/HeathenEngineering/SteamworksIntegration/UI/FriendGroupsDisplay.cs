using System.Collections.Generic;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/ui-components/friend-groups-display")]
	public class FriendGroupsDisplay : MonoBehaviour
	{
		[SerializeField]
		private Transform inGameCollection;

		[SerializeField]
		private Transform inOtherGameCollection;

		[SerializeField]
		private Transform groupedCollection;

		[SerializeField]
		private Transform onlineCollection;

		[SerializeField]
		private Transform offlineCollection;

		[SerializeField]
		private GameObject groupPrefab;

		private void OnEnable()
		{
			if (App.Initialized)
			{
				UpdateDisplay();
			}
			else
			{
				App.evtSteamInitialized.AddListener(DelayUpdate);
			}
		}

		private void OnDisable()
		{
			Clear();
		}

		private void DelayUpdate()
		{
			UpdateDisplay();
			App.evtSteamInitialized.RemoveListener(DelayUpdate);
		}

		public void Clear()
		{
			if (inGameCollection != null && inGameCollection.childCount > 0)
			{
				foreach (Transform item in inGameCollection)
				{
					Object.Destroy(item.gameObject);
				}
			}
			if (groupedCollection != null && groupedCollection.childCount > 0)
			{
				foreach (Transform item2 in groupedCollection)
				{
					Object.Destroy(item2.gameObject);
				}
			}
			if (onlineCollection != null && onlineCollection.childCount > 0)
			{
				foreach (Transform item3 in onlineCollection)
				{
					Object.Destroy(item3.gameObject);
				}
			}
			if (offlineCollection != null && offlineCollection.childCount > 0)
			{
				foreach (Transform item4 in offlineCollection)
				{
					Object.Destroy(item4.gameObject);
				}
			}
			if (!(inOtherGameCollection != null) || inOtherGameCollection.childCount <= 0)
			{
				return;
			}
			foreach (Transform item5 in inOtherGameCollection)
			{
				Object.Destroy(item5.gameObject);
			}
		}

		public void UpdateDisplay()
		{
			Clear();
			List<UserData> list = new List<UserData>();
			List<UserData> list2 = new List<UserData>();
			List<UserData> list3 = new List<UserData>();
			List<UserData> list4 = new List<UserData>();
			Dictionary<string, List<UserData>> dictionary = new Dictionary<string, List<UserData>>();
			UserData[] friends = Friends.Client.GetFriends(EFriendFlags.k_EFriendFlagImmediate);
			FriendsGroupID_t[] friendsGroups = Friends.Client.GetFriendsGroups();
			foreach (FriendsGroupID_t groupId in friendsGroups)
			{
				string friendsGroupName = Friends.Client.GetFriendsGroupName(groupId);
				if (!dictionary.ContainsKey(friendsGroupName))
				{
					dictionary.Add(friendsGroupName, new List<UserData>());
				}
				List<UserData> list5 = dictionary[friendsGroupName];
				CSteamID[] friendsGroupMembersList = Friends.Client.GetFriendsGroupMembersList(groupId);
				foreach (CSteamID cSteamID in friendsGroupMembersList)
				{
					if (cSteamID != UserData.Me && !list5.Contains(cSteamID))
					{
						list5.Add(cSteamID);
					}
				}
			}
			UserData[] array = friends;
			for (int i = 0; i < array.Length; i++)
			{
				UserData userData = array[i];
				if (userData == UserData.Me)
				{
					continue;
				}
				if (userData.GetGamePlayed(out var gameInfo))
				{
					list.Add(userData);
					if (gameInfo.Game.IsMe)
					{
						list2.Add(userData);
					}
					else
					{
						list3.Add(userData);
					}
				}
				else if (userData.State != EPersonaState.k_EPersonaStateOffline && userData.State != EPersonaState.k_EPersonaStateInvisible)
				{
					list.Add(userData);
				}
				else
				{
					list4.Add(userData);
				}
			}
			if (onlineCollection != null)
			{
				onlineCollection.gameObject.SetActive(value: true);
				Object.Instantiate(groupPrefab, onlineCollection).GetComponent<FriendGroup>().InitializeOnline("Online", list, expanded: true);
			}
			if (offlineCollection != null)
			{
				Object.Instantiate(groupPrefab, offlineCollection).GetComponent<FriendGroup>().InitializeOffline("Offline", list4, expanded: false);
			}
			if (inGameCollection != null)
			{
				Object.Instantiate(groupPrefab, inGameCollection).GetComponent<FriendGroup>().InitializeInGame("In Game", list2, expanded: true);
			}
			if (inOtherGameCollection != null)
			{
				Object.Instantiate(groupPrefab, inOtherGameCollection).GetComponent<FriendGroup>().InitializeInOther("Other Games", list3, expanded: true);
			}
			if (dictionary.Count > 0)
			{
				foreach (KeyValuePair<string, List<UserData>> item in dictionary)
				{
					Object.Instantiate(groupPrefab, groupedCollection).GetComponent<FriendGroup>().InitializeCustom(item.Key, item.Value, expanded: true);
				}
				return;
			}
			groupedCollection.gameObject.SetActive(value: false);
		}
	}
}
