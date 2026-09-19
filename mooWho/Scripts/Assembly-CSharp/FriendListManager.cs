using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class FriendListManager : MonoBehaviour
{
	[SerializeField]
	private GameObject _steamFriendItem;

	[SerializeField]
	private Transform _friendListContainer;

	[SerializeField]
	private List<FriendItem> _friendList = new List<FriendItem>();

	private Dictionary<CSteamID, FriendItem> friendDictionary = new Dictionary<CSteamID, FriendItem>();

	[SerializeField]
	private float timeToRefreshList = 30f;

	private float timer;

	private bool initialized;

	private void Start()
	{
	}

	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!initialized)
		{
			GetSteamFriends();
			initialized = true;
			return;
		}
		timer += Time.deltaTime;
		if (timer > timeToRefreshList)
		{
			GetSteamFriends();
			timer = 0f;
		}
	}

	private void GetSteamFriends()
	{
		if (_friendList.Count > 0)
		{
			foreach (FriendItem friend in _friendList)
			{
				Object.Destroy(friend.gameObject);
			}
			_friendList.Clear();
		}
		int num = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
		int num2 = 0;
		if (num == -1)
		{
			Debug.LogError("Friend count returned at -1, the user is not logged in");
			num = 0;
		}
		for (int i = 0; i < num; i++)
		{
			CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
			string friendPersonaName = SteamFriends.GetFriendPersonaName(friendByIndex);
			EPersonaState friendPersonaState = SteamFriends.GetFriendPersonaState(friendByIndex);
			bool flag = false;
			if (friendPersonaState != EPersonaState.k_EPersonaStateOffline)
			{
				flag = true;
				FriendItem component = Object.Instantiate(_steamFriendItem, _friendListContainer).GetComponent<FriendItem>();
				component.InitializeFriendItem(friendPersonaName, friendByIndex.m_SteamID, flag);
				_friendList.Add(component);
				num2++;
			}
		}
		for (int j = 0; j < num; j++)
		{
			CSteamID friendByIndex2 = SteamFriends.GetFriendByIndex(j, EFriendFlags.k_EFriendFlagImmediate);
			string friendPersonaName2 = SteamFriends.GetFriendPersonaName(friendByIndex2);
			EPersonaState friendPersonaState2 = SteamFriends.GetFriendPersonaState(friendByIndex2);
			bool flag2 = false;
			if (friendPersonaState2 == EPersonaState.k_EPersonaStateOffline)
			{
				flag2 = false;
				FriendItem component2 = Object.Instantiate(_steamFriendItem, _friendListContainer).GetComponent<FriendItem>();
				component2.InitializeFriendItem(friendPersonaName2, friendByIndex2.m_SteamID, flag2);
				_friendList.Add(component2);
			}
		}
	}
}
