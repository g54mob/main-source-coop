using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using TMPro;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class SetUserName : MonoBehaviour
	{
		private TextMeshProUGUI label;

		[SerializeField]
		[Tooltip("Should the component load the local user's name on Start.\nIf false you must call SetName and provide the ID of the user to load")]
		private bool useLocalUser;

		[SerializeField]
		[Tooltip("Should we show the profile name (set by the user this represents) or the nick name (set by the local user for this user)")]
		private bool showNickname;

		private UserData currentUser;

		public bool ShowNickname
		{
			get
			{
				return showNickname;
			}
			set
			{
				showNickname = value;
				SetName(currentUser);
			}
		}

		public UserData UserData
		{
			get
			{
				return currentUser;
			}
			set
			{
				SetName(value);
			}
		}

		private void OnEnable()
		{
			Friends.Client.EventPersonaStateChange.AddListener(HandlePersonaStateChange);
		}

		private void OnDisable()
		{
			Friends.Client.EventPersonaStateChange.RemoveListener(HandlePersonaStateChange);
		}

		private void Start()
		{
			label = GetComponent<TextMeshProUGUI>();
			if (useLocalUser)
			{
				if (App.Initialized)
				{
					SetName(UserData.Me);
				}
				else
				{
					App.evtSteamInitialized.AddListener(HandleSteamInitalized);
				}
			}
		}

		private void HandleSteamInitalized()
		{
			if (useLocalUser)
			{
				SetName(UserData.Me);
			}
			App.evtSteamInitialized.RemoveListener(HandleSteamInitalized);
		}

		private void HandlePersonaStateChange(PersonaStateChange arg)
		{
			UserData userData = arg.SubjectId;
			if ((Friends.Client.PersonaChangeHasFlag(arg.Flags, EPersonaChange.k_EPersonaChangeName) || Friends.Client.PersonaChangeHasFlag(arg.Flags, EPersonaChange.k_EPersonaChangeNickname)) && userData == currentUser)
			{
				if (showNickname)
				{
					label.text = userData.Nickname;
				}
				else
				{
					label.text = userData.Name;
				}
			}
		}

		public void SetName(UserData user)
		{
			if (label == null)
			{
				label = GetComponent<TextMeshProUGUI>();
			}
			if (!(label == null))
			{
				currentUser = user;
				Friends.Client.RequestUserInformation(user, nameOnly: false);
				if (showNickname)
				{
					label.text = user.Nickname;
				}
				else
				{
					label.text = user.Name;
				}
			}
		}
	}
}
