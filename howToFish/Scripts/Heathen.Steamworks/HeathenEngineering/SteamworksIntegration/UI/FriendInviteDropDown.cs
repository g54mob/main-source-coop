using System.Collections.Generic;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/ui-components/friend-invite-dropdown")]
	public class FriendInviteDropDown : MonoBehaviour
	{
		public struct FilterOptions
		{
			public bool inThisGame;

			public bool inOtherGame;

			public bool busy;

			public bool away;

			public bool snooze;
		}

		[SerializeField]
		private TMP_InputField inputField;

		[SerializeField]
		private Button dropdownButton;

		[SerializeField]
		private Button inviteButton;

		[SerializeField]
		private RectTransform panel;

		[SerializeField]
		private Transform content;

		[SerializeField]
		private GameObject template;

		public FilterOptions filter = new FilterOptions
		{
			inThisGame = true,
			inOtherGame = true,
			busy = false,
			away = true,
			snooze = true
		};

		[Header("Events")]
		public UserDataEvent Invited = new UserDataEvent();

		private readonly List<GameObject> displayMembers = new List<GameObject>();

		private RectTransform selfTransform;

		public bool IsExpanded
		{
			get
			{
				return panel.gameObject.activeSelf;
			}
			set
			{
				if (value)
				{
					Show();
				}
				else
				{
					panel.gameObject.SetActive(value: false);
				}
			}
		}

		public string InputText
		{
			get
			{
				return inputField.text;
			}
			set
			{
				inputField.text = value;
			}
		}

		private void Start()
		{
			selfTransform = GetComponent<RectTransform>();
			dropdownButton.onClick.AddListener(InternalHandleDropDownClick);
			inviteButton.onClick.AddListener(InternalInviteButtonClicked);
		}

		private void InternalInviteButtonClicked()
		{
			if (uint.TryParse(inputField.text, out var result))
			{
				UserData arg = UserData.Get(result);
				if (arg.IsValid)
				{
					Invited.Invoke(arg);
				}
			}
		}

		private void Update()
		{
			if (panel.gameObject.activeSelf && ((Mouse.current.leftButton.wasPressedThisFrame && !RectTransformUtility.RectangleContainsScreenPoint(selfTransform, Mouse.current.position.ReadValue()) && !RectTransformUtility.RectangleContainsScreenPoint(panel, Mouse.current.position.ReadValue())) || Keyboard.current.escapeKey.wasPressedThisFrame))
			{
				panel.gameObject.SetActive(value: false);
			}
		}

		private void InternalHandleDropDownClick()
		{
			if (panel.gameObject.activeSelf)
			{
				panel.gameObject.SetActive(value: false);
			}
			else
			{
				Show();
			}
		}

		public void Show()
		{
			foreach (GameObject displayMember in displayMembers)
			{
				Object.Destroy(displayMember);
			}
			UserData[] friends = Friends.Client.GetFriends(EFriendFlags.k_EFriendFlagImmediate);
			for (int i = 0; i < friends.Length; i++)
			{
				UserData userData = friends[i];
				if (!(userData != UserData.Me))
				{
					continue;
				}
				if ((filter.inThisGame || filter.inOtherGame) && userData.GetGamePlayed(out var gameInfo))
				{
					if ((filter.inThisGame || !gameInfo.Game.IsMe) && (filter.inOtherGame || gameInfo.Game.IsMe))
					{
						GameObject gameObject = Object.Instantiate(template, content);
						UserInviteButton component = gameObject.GetComponent<UserInviteButton>();
						component.SetFriend(userData);
						component.Click.AddListener(FriendButtonClicked);
						displayMembers.Add(gameObject);
					}
					continue;
				}
				EPersonaState state = userData.State;
				if (state == EPersonaState.k_EPersonaStateOnline || (state == EPersonaState.k_EPersonaStateBusy && filter.busy) || (state == EPersonaState.k_EPersonaStateAway && filter.away) || (state == EPersonaState.k_EPersonaStateSnooze && filter.snooze))
				{
					GameObject gameObject2 = Object.Instantiate(template, content);
					UserInviteButton component2 = gameObject2.GetComponent<UserInviteButton>();
					component2.SetFriend(userData);
					component2.Click.AddListener(FriendButtonClicked);
					displayMembers.Add(gameObject2);
				}
			}
			panel.gameObject.SetActive(value: true);
		}

		public void FriendButtonClicked(UserAndPointerData data)
		{
			inputField.text = data.user.FriendId.ToString();
			panel.gameObject.SetActive(value: false);
		}
	}
}
