using System.Collections.Generic;
using System.Linq;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/ui-components/friend-list")]
	public class FriendList : MonoBehaviour
	{
		public enum Filter
		{
			All = 0,
			InThisGame = 1,
			InOtherGame = 2,
			InAnyGame = 3,
			NotInThisGame = 4,
			NotInGame = 5,
			AnyOnline = 6,
			AnyOffline = 7,
			Away = 8,
			Busy = 9,
			Followed = 10
		}

		public bool includeFollowed;

		[SerializeField]
		private Filter filter;

		public Transform content;

		public GameObject recordTemplate;

		private Dictionary<UserData, GameObject> records = new Dictionary<UserData, GameObject>();

		public Filter ActiveFilter
		{
			get
			{
				return filter;
			}
			set
			{
				filter = value;
				UpdateDisplay();
			}
		}

		private void OnEnable()
		{
			Friends.Client.EventPersonaStateChange.AddListener(HandleStateChange);
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
			Friends.Client.EventPersonaStateChange.RemoveListener(HandleStateChange);
			Clear();
		}

		private void DelayUpdate()
		{
			UpdateDisplay();
			App.evtSteamInitialized.RemoveListener(DelayUpdate);
		}

		private void HandleStateChange(PersonaStateChange arg0)
		{
			UserData userData = arg0.SubjectId;
			if (MatchFilter(userData))
			{
				Add(userData);
			}
			else
			{
				Remove(userData);
			}
		}

		private void Remove(UserData user)
		{
			if (records.ContainsKey(user))
			{
				GameObject obj = records[user];
				records.Remove(user);
				Object.Destroy(obj.gameObject);
			}
		}

		private void Add(UserData user)
		{
			if (!records.ContainsKey(user))
			{
				AddNewRecord(user);
				SortRecords();
			}
			else
			{
				records[user].GetComponent<IUserProfile>().UserData = user;
			}
		}

		private void AddNewRecord(UserData user)
		{
			GameObject gameObject = Object.Instantiate(recordTemplate, content);
			gameObject.GetComponent<IUserProfile>().UserData = user;
			records.Add(user, gameObject);
		}

		private void SortRecords()
		{
			List<UserData> list = records.Keys.ToList();
			list.Sort((UserData a, UserData b) => a.Nickname.CompareTo(b.Nickname));
			foreach (UserData item in list)
			{
				records[item].transform.SetAsLastSibling();
			}
		}

		public void Clear()
		{
			records.Clear();
			if (content.childCount <= 0)
			{
				return;
			}
			foreach (Transform item in content)
			{
				try
				{
					Object.Destroy(item.gameObject);
				}
				catch
				{
				}
			}
		}

		public void UpdateDisplay()
		{
			Clear();
			List<UserData> filtered = new List<UserData>();
			List<UserData> friends = new List<UserData>(Friends.Client.GetFriends(EFriendFlags.k_EFriendFlagImmediate));
			if (includeFollowed)
			{
				List<UserData> followed = new List<UserData>();
				Friends.Client.GetFollowed(delegate(CSteamID[] r)
				{
					if (r != null && r.Length != 0)
					{
						IEnumerable<CSteamID> enumerable = r.Where((CSteamID p) => p.GetEAccountType() == EAccountType.k_EAccountTypeIndividual);
						if (enumerable.Count() > 0)
						{
							foreach (CSteamID item in enumerable)
							{
								if (!friends.Contains(item))
								{
									friends.Add(item);
								}
								followed.Add(item);
							}
						}
					}
					if (filter == Filter.Followed)
					{
						foreach (UserData item2 in followed)
						{
							if (item2 != UserData.Me && !records.ContainsKey(item2))
							{
								AddNewRecord(item2);
							}
						}
					}
					else
					{
						foreach (UserData item3 in friends)
						{
							if (item3 != UserData.Me && MatchFilter(item3))
							{
								filtered.Add(item3);
							}
						}
						foreach (UserData item4 in filtered)
						{
							if (item4 != UserData.Me && !records.ContainsKey(item4))
							{
								AddNewRecord(item4);
							}
						}
					}
					SortRecords();
				});
				return;
			}
			foreach (UserData item5 in friends)
			{
				if (item5 != UserData.Me && MatchFilter(item5))
				{
					AddNewRecord(item5);
				}
			}
			SortRecords();
		}

		public bool MatchFilter(UserData friend)
		{
			switch (filter)
			{
			case Filter.All:
				return true;
			case Filter.AnyOffline:
				return friend.State == EPersonaState.k_EPersonaStateOffline;
			case Filter.AnyOnline:
				return friend.State != EPersonaState.k_EPersonaStateOffline;
			case Filter.Away:
				return friend.State == EPersonaState.k_EPersonaStateAway;
			case Filter.Busy:
				return friend.State == EPersonaState.k_EPersonaStateBusy;
			case Filter.InAnyGame:
				return friend.InGame;
			case Filter.InOtherGame:
			{
				if (friend.GetGamePlayed(out var gameInfo2))
				{
					if (!gameInfo2.Game.IsMe)
					{
						return true;
					}
					return false;
				}
				return false;
			}
			case Filter.InThisGame:
			{
				if (friend.GetGamePlayed(out var gameInfo3))
				{
					if (gameInfo3.Game.IsMe)
					{
						return true;
					}
					return false;
				}
				return false;
			}
			case Filter.NotInThisGame:
			{
				if (friend.GetGamePlayed(out var gameInfo))
				{
					if (!gameInfo.Game.IsMe)
					{
						return true;
					}
					return false;
				}
				if (friend.State != EPersonaState.k_EPersonaStateOffline)
				{
					return true;
				}
				return false;
			}
			case Filter.NotInGame:
				return !friend.InGame;
			default:
				return false;
			}
		}
	}
}
