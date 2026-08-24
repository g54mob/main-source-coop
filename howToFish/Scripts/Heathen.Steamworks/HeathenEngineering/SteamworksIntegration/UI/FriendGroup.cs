using System.Collections.Generic;
using System.Linq;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/ui-components/friend-group")]
	public class FriendGroup : MonoBehaviour
	{
		private enum GroupType
		{
			None = 0,
			Online = 1,
			Offline = 2,
			InGame = 3,
			OtherGame = 4
		}

		[SerializeField]
		private TextMeshProUGUI label;

		[SerializeField]
		private TextMeshProUGUI counter;

		[SerializeField]
		private Toggle toggle;

		[SerializeField]
		private GameObject recordTemplate;

		[SerializeField]
		private Transform content;

		private Dictionary<UserData, GameObject> records = new Dictionary<UserData, GameObject>();

		private GroupType type;

		private void OnEnable()
		{
			Friends.Client.EventPersonaStateChange.AddListener(HandleStateChange);
		}

		private void OnDisable()
		{
			Friends.Client.EventPersonaStateChange.RemoveListener(HandleStateChange);
		}

		private void HandleStateChange(PersonaStateChange arg0)
		{
			UserData user = arg0.SubjectId;
			if (user.IsMe)
			{
				return;
			}
			switch (type)
			{
			case GroupType.Online:
				if (user.State == EPersonaState.k_EPersonaStateOffline || user.State == EPersonaState.k_EPersonaStateInvisible)
				{
					Remove(user);
				}
				else
				{
					Add(user);
				}
				break;
			case GroupType.Offline:
				if (user.State != EPersonaState.k_EPersonaStateOffline && user.State != EPersonaState.k_EPersonaStateInvisible)
				{
					Remove(user);
				}
				else
				{
					Add(user);
				}
				break;
			case GroupType.InGame:
			{
				if (user.GetGamePlayed(out var gameInfo2) && gameInfo2.Game.App == SteamUtils.GetAppID())
				{
					Add(user);
				}
				else
				{
					Remove(user);
				}
				break;
			}
			case GroupType.OtherGame:
			{
				if (user.GetGamePlayed(out var gameInfo) && gameInfo.Game.App != SteamUtils.GetAppID())
				{
					Add(user);
				}
				else
				{
					Remove(user);
				}
				break;
			}
			}
		}

		private void Remove(UserData user)
		{
			if (records.ContainsKey(user))
			{
				GameObject obj = records[user];
				records.Remove(user);
				Object.Destroy(obj);
				counter.text = "(" + records.Count + ")";
			}
		}

		private void Add(UserData user)
		{
			if (!records.ContainsKey(user))
			{
				AddNewRecord(user);
				SortRecords();
				counter.text = "(" + records.Count + ")";
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
			counter.text = "(" + records.Count + ")";
		}

		public void InitializeCustom(string name, List<UserData> users, bool expanded)
		{
			label.text = name;
			toggle.isOn = expanded;
			type = GroupType.None;
			foreach (UserData user in users)
			{
				if (!records.ContainsKey(user))
				{
					AddNewRecord(user);
				}
			}
			SortRecords();
		}

		public void InitializeOnline(string name, List<UserData> users, bool expanded)
		{
			label.text = name;
			toggle.isOn = expanded;
			type = GroupType.Online;
			foreach (UserData user in users)
			{
				if (!records.ContainsKey(user))
				{
					AddNewRecord(user);
				}
			}
			SortRecords();
		}

		public void InitializeOffline(string name, List<UserData> users, bool expanded)
		{
			label.text = name;
			toggle.isOn = expanded;
			type = GroupType.Offline;
			foreach (UserData user in users)
			{
				if (!records.ContainsKey(user))
				{
					AddNewRecord(user);
				}
			}
			SortRecords();
		}

		public void InitializeInGame(string name, List<UserData> users, bool expanded)
		{
			label.text = name;
			toggle.isOn = expanded;
			type = GroupType.InGame;
			foreach (UserData user in users)
			{
				if (!records.ContainsKey(user))
				{
					AddNewRecord(user);
				}
			}
			SortRecords();
		}

		public void InitializeInOther(string name, List<UserData> users, bool expanded)
		{
			label.text = name;
			toggle.isOn = expanded;
			type = GroupType.OtherGame;
			foreach (UserData user in users)
			{
				if (!records.ContainsKey(user))
				{
					AddNewRecord(user);
				}
			}
			SortRecords();
		}
	}
}
