using System;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/ui-components/friend-profile")]
	public class FriendProfile : MonoBehaviour, IUserProfile
	{
		[Serializable]
		public struct TextField
		{
			[Tooltip("The label to display the fields value with")]
			public TextMeshProUGUI label;

			[Header("Colors")]
			[Tooltip("Should the system use specific colors for each status type")]
			public bool useStatusColors;

			[Tooltip("The color to use when the status of the player is playing a game and the game being played is this game.\nOnly used when the Use Status Colors field is true.")]
			public Color inThisGame;

			[Tooltip("The color to use when the status of the player is playing a game and the game being played is *NOT* this game.\nOnly used when the Use Status Colors field is true.")]
			public Color inOtherGame;

			[Tooltip("The color to use when the status of the player is online and active.\nOnly used when the Use Status Colors field is true.")]
			public Color isOnlineActive;

			[Tooltip("The color to use when the status of the player is online and inactive.\nOnly used when the Use Status Colors field is true.")]
			public Color isOnlineInactive;

			[Tooltip("The color to use when the status of the player is offline.\nOnly used when the Use Status Colors field is true.")]
			public Color isOffline;

			public void SetValue(string value, bool inGame, bool inThisGame, EPersonaState state)
			{
				if (!(label != null))
				{
					return;
				}
				label.text = value;
				if (!useStatusColors)
				{
					return;
				}
				if (inGame)
				{
					if (inThisGame)
					{
						label.color = this.inThisGame;
					}
					else
					{
						label.color = inOtherGame;
					}
					return;
				}
				switch (state)
				{
				case EPersonaState.k_EPersonaStateOffline:
					label.color = isOffline;
					break;
				case EPersonaState.k_EPersonaStateBusy:
				case EPersonaState.k_EPersonaStateAway:
				case EPersonaState.k_EPersonaStateSnooze:
				case EPersonaState.k_EPersonaStateInvisible:
					label.color = isOnlineInactive;
					break;
				default:
					label.color = isOnlineActive;
					break;
				}
			}
		}

		[Serializable]
		public struct InputField
		{
			public TMP_InputField label;

			[Header("Colors")]
			[Tooltip("Should the system use specific colors for each status type")]
			public bool useStatusColors;

			[Tooltip("The color to use when the status of the player is playing a game and the game being played is this game.\nOnly used when the Use Status Colors field is true.")]
			public Color inThisGame;

			[Tooltip("The color to use when the status of the player is playing a game and the game being played is *NOT* this game.\nOnly used when the Use Status Colors field is true.")]
			public Color inOtherGame;

			[Tooltip("The color to use when the status of the player is online and active.\nOnly used when the Use Status Colors field is true.")]
			public Color isOnlineActive;

			[Tooltip("The color to use when the status of the player is online and inactive.\nOnly used when the Use Status Colors field is true.")]
			public Color isOnlineInactive;

			[Tooltip("The color to use when the status of the player is offline.\nOnly used when the Use Status Colors field is true.")]
			public Color isOffline;

			public void SetValue(string value, bool inGame, bool inThisGame, EPersonaState state)
			{
				if (!(label != null))
				{
					return;
				}
				label.text = value;
				if (!useStatusColors)
				{
					return;
				}
				if (inGame)
				{
					if (inThisGame)
					{
						label.textComponent.color = this.inThisGame;
					}
					else
					{
						label.textComponent.color = inOtherGame;
					}
					return;
				}
				switch (state)
				{
				case EPersonaState.k_EPersonaStateOffline:
					label.textComponent.color = isOffline;
					break;
				case EPersonaState.k_EPersonaStateBusy:
				case EPersonaState.k_EPersonaStateAway:
				case EPersonaState.k_EPersonaStateSnooze:
				case EPersonaState.k_EPersonaStateInvisible:
					label.textComponent.color = isOnlineInactive;
					break;
				default:
					label.textComponent.color = isOnlineActive;
					break;
				}
			}
		}

		[Serializable]
		public struct ImageField
		{
			public Image image;

			[Header("Colors")]
			[Tooltip("Should the system use specific colors for each status type")]
			public bool useStatusColors;

			[Tooltip("The color to use when the status of the player is playing a game and the game being played is this game.\nOnly used when the Use Status Colors field is true.")]
			public Color inThisGame;

			[Tooltip("The color to use when the status of the player is playing a game and the game being played is *NOT* this game.\nOnly used when the Use Status Colors field is true.")]
			public Color inOtherGame;

			[Tooltip("The color to use when the status of the player is online and active.\nOnly used when the Use Status Colors field is true.")]
			public Color isOnlineActive;

			[Tooltip("The color to use when the status of the player is online and inactive.\nOnly used when the Use Status Colors field is true.")]
			public Color isOnlineInactive;

			[Tooltip("The color to use when the status of the player is offline.\nOnly used when the Use Status Colors field is true.")]
			public Color isOffline;

			public void SetValue(bool inGame, bool inThisGame, EPersonaState state)
			{
				if (!(image != null) || !useStatusColors)
				{
					return;
				}
				if (inGame)
				{
					if (inThisGame)
					{
						image.color = this.inThisGame;
					}
					else
					{
						image.color = inOtherGame;
					}
					return;
				}
				switch (state)
				{
				case EPersonaState.k_EPersonaStateOffline:
					image.color = isOffline;
					break;
				case EPersonaState.k_EPersonaStateBusy:
				case EPersonaState.k_EPersonaStateAway:
				case EPersonaState.k_EPersonaStateSnooze:
				case EPersonaState.k_EPersonaStateInvisible:
					image.color = isOnlineInactive;
					break;
				default:
					image.color = isOnlineActive;
					break;
				}
			}
		}

		[Serializable]
		public struct MessageOptions
		{
			[Tooltip("V")]
			public string playingThis;

			[Tooltip("The message to be displayed when the subject is *NOT* playing this game. This is used if Name Other Game is false")]
			public string playingOther;

			[Tooltip("If true then the message will be the name of the game the player is playing if known, if false then the Playing Other message will be used.")]
			public bool nameOtherGame;

			[Tooltip("The message to display when the subject is inactive")]
			public string inactive;

			[Tooltip("The message to display when the subject is active")]
			public string active;

			[Tooltip("The message to display when the subject is offline")]
			public string offline;

			public string ToString(EPersonaState state, bool isPlaying, string gameName)
			{
				if (!string.IsNullOrEmpty(gameName))
				{
					if (nameOtherGame)
					{
						return playingOther + gameName;
					}
					return gameName;
				}
				if (isPlaying)
				{
					return playingThis;
				}
				switch (state)
				{
				case EPersonaState.k_EPersonaStateOffline:
					return offline;
				case EPersonaState.k_EPersonaStateBusy:
				case EPersonaState.k_EPersonaStateAway:
				case EPersonaState.k_EPersonaStateSnooze:
				case EPersonaState.k_EPersonaStateInvisible:
					return inactive;
				default:
					return active;
				}
			}
		}

		[SerializeField]
		[Tooltip("Should the component load the local user's avatar on Start.\nIf false you must call LoadAvatar and provide the ID of the user to load")]
		private bool useLocalUser;

		[Tooltip("If false then the display name field will get the Nickname if available and Friend name if not. If true then the display name will always be the Friend name and the nickname field will be used for nick if available.")]
		public bool appendNickname;

		public MessageOptions messageOptions = new MessageOptions
		{
			active = "Online",
			inactive = "Away",
			playingThis = "Playing",
			offline = "Offline",
			nameOtherGame = true,
			playingOther = ""
		};

		[Header("UI Elements")]
		[SerializeField]
		private RawImage avatar;

		[SerializeField]
		private TextField displayName = new TextField
		{
			useStatusColors = true,
			inThisGame = new Color(0.8862f, 0.996f, 0.7568f, 1f),
			inOtherGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			isOnlineActive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOnlineInactive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOffline = new Color(0.887f, 0.887f, 0.887f, 1f)
		};

		[SerializeField]
		private TextField nickname = new TextField
		{
			useStatusColors = false,
			inThisGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			inOtherGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			isOnlineActive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOnlineInactive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOffline = new Color(0.887f, 0.887f, 0.887f, 1f)
		};

		[SerializeField]
		private TextField statusLabel = new TextField
		{
			useStatusColors = true,
			inThisGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			inOtherGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			isOnlineActive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOnlineInactive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOffline = new Color(0.887f, 0.887f, 0.887f, 1f)
		};

		[SerializeField]
		private ImageField statusImage = new ImageField
		{
			useStatusColors = true,
			inThisGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			inOtherGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			isOnlineActive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOnlineInactive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOffline = new Color(0.887f, 0.887f, 0.887f, 1f)
		};

		[SerializeField]
		private InputField friendId = new InputField
		{
			useStatusColors = false,
			inThisGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			inOtherGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			isOnlineActive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOnlineInactive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOffline = new Color(0.887f, 0.887f, 0.887f, 1f)
		};

		[SerializeField]
		private TextField level = new TextField
		{
			useStatusColors = false,
			inThisGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			inOtherGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			isOnlineActive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOnlineInactive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOffline = new Color(0.887f, 0.887f, 0.887f, 1f)
		};

		[SerializeField]
		private ImageField panel = new ImageField
		{
			useStatusColors = true,
			inThisGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			inOtherGame = new Color(0.5686f, 0.7607f, 0.3411f, 1f),
			isOnlineActive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOnlineInactive = new Color(0.4117f, 0.7803f, 0.9254f, 1f),
			isOffline = new Color(0.887f, 0.887f, 0.887f, 1f)
		};

		[Header("Events")]
		public UnityEvent evtLoaded;

		private UserData currentUser;

		public UserData UserData
		{
			get
			{
				return currentUser;
			}
			set
			{
				Apply(value);
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
			if (App.Initialized)
			{
				if (useLocalUser)
				{
					UserData id = User.Client.Id;
					Apply(id);
				}
			}
			else
			{
				App.evtSteamInitialized.AddListener(DelayUpdate);
			}
		}

		private void DelayUpdate()
		{
			if (useLocalUser)
			{
				UserData id = User.Client.Id;
				Apply(id);
			}
			App.evtSteamInitialized.RemoveListener(DelayUpdate);
		}

		private void HandlePersonaStateChange(PersonaStateChange arg)
		{
			if (arg.SubjectId == currentUser)
			{
				UpdateUserData();
			}
		}

		public void Apply(UserData user)
		{
			currentUser = user;
			if (!currentUser.RequestInformation())
			{
				UpdateUserData();
			}
		}

		private void UpdateUserData()
		{
			if (!currentUser.IsValid)
			{
				return;
			}
			FriendGameInfo gameInfo;
			bool inGame = currentUser.GetGamePlayed(out gameInfo);
			bool inThisGame = inGame && gameInfo.Game.App == App.Client.Id;
			EPersonaState state = currentUser.State;
			if (!appendNickname)
			{
				if (nickname.label != null && nickname.label.gameObject.activeSelf)
				{
					nickname.label.gameObject.SetActive(value: false);
				}
				displayName.SetValue(currentUser.Nickname, inGame, inThisGame, state);
			}
			else
			{
				string text = currentUser.Nickname;
				string text2 = currentUser.Name;
				if (text2 != text)
				{
					if (nickname.label != null && !nickname.label.gameObject.activeSelf)
					{
						nickname.label.gameObject.SetActive(value: true);
					}
					displayName.SetValue(text2, inGame, inThisGame, state);
					nickname.SetValue(text, inGame, inThisGame, state);
				}
				else
				{
					if (nickname.label != null && nickname.label.gameObject.activeSelf)
					{
						nickname.label.gameObject.SetActive(value: false);
					}
					displayName.SetValue(text2, inGame, inThisGame, state);
				}
			}
			friendId.SetValue(currentUser.FriendId.ToString(), inGame, inThisGame, state);
			int num = currentUser.Level;
			if (num == 0)
			{
				level.SetValue("??", inGame, inThisGame, state);
				Invoke("UpdateUserData", 1f);
			}
			else
			{
				level.SetValue(num.ToString(), inGame, inThisGame, state);
			}
			panel.SetValue(inGame, inThisGame, state);
			if (avatar != null)
			{
				currentUser.LoadAvatar(delegate(Texture2D r)
				{
					avatar.texture = r;
					evtLoaded?.Invoke();
				});
			}
			if (!inThisGame)
			{
				if (!inGame)
				{
					statusLabel.SetValue(messageOptions.ToString(state, inGame, string.Empty), inGame, inThisGame, state);
					statusImage.SetValue(inGame, inThisGame, state);
					return;
				}
				App.Web.GetAppName(gameInfo.Game.App, delegate(string r, bool e)
				{
					if (!e)
					{
						statusLabel.SetValue(messageOptions.ToString(state, inGame, r), inGame, inThisGame, state);
						statusImage.SetValue(inGame, inThisGame, state);
					}
					else
					{
						statusLabel.SetValue(messageOptions.ToString(state, inGame, "Unknown"), inGame, inThisGame, state);
						statusImage.SetValue(inGame, inThisGame, state);
					}
				});
			}
			else
			{
				statusLabel.SetValue(messageOptions.ToString(state, inGame, string.Empty), inGame, inThisGame, state);
				statusImage.SetValue(inGame, inThisGame, state);
			}
		}
	}
}
