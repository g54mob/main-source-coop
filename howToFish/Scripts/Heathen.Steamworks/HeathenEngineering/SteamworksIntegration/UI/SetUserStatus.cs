using System;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	public class SetUserStatus : MonoBehaviour
	{
		[Serializable]
		public class Options
		{
			[Serializable]
			public class References
			{
				public Sprite icon;

				public bool setIconColor;

				public Color iconColor;

				[Tooltip("You can use %gameName% and it will be replaced with the name of the game the player is currently playing. This is only relivent for In This Game and In Another Game options.")]
				public string message;

				public bool setMessageColor;

				public Color messageColor;

				public void Set(Image image, TextMeshProUGUI label, FriendGameInfo_t? gameInfo)
				{
					if (image != null)
					{
						image.gameObject.SetActive(value: true);
						image.sprite = icon;
						if (setIconColor)
						{
							image.color = iconColor;
						}
					}
					if (message == null)
					{
						return;
					}
					if (gameInfo.HasValue)
					{
						AppData.LoadNames(delegate
						{
							AppData appData = gameInfo.Value.m_gameID;
							label.text = message.Replace("%gameName%", appData.Name);
						});
					}
					else
					{
						label.text = message;
					}
					if (setMessageColor)
					{
						label.color = messageColor;
					}
				}
			}

			public References InThisGame = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Playing %gameName%",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References InAnotherGame = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Playing %gameName%",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References Online = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Online",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References Offline = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Offline",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References Busy = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Busy",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References Away = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Away",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References Snooze = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Snooze",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References LookingToTrade = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Looking to Trade",
				setMessageColor = false,
				messageColor = Color.white
			};

			public References LookingToPlay = new References
			{
				setIconColor = false,
				iconColor = Color.white,
				message = "Looking to Play",
				setMessageColor = false,
				messageColor = Color.white
			};
		}

		public Image icon;

		public TextMeshProUGUI message;

		public Options options;

		private UserData userData;

		public UserData UserData
		{
			get
			{
				return userData;
			}
			set
			{
				userData = value;
				Refresh();
			}
		}

		private void OnEnable()
		{
			Friends.Client.EventPersonaStateChange.AddListener(InternalPersonaStateChange);
			Friends.Client.EventFriendRichPresenceUpdate.AddListener(InternalRichPresenceUpdate);
		}

		private void OnDisable()
		{
			Friends.Client.EventPersonaStateChange.RemoveListener(InternalPersonaStateChange);
			Friends.Client.EventFriendRichPresenceUpdate.RemoveListener(InternalRichPresenceUpdate);
		}

		private void InternalRichPresenceUpdate(FriendRichPresenceUpdate arg0)
		{
			Refresh();
		}

		private void InternalPersonaStateChange(PersonaStateChange arg0)
		{
			Refresh();
		}

		public void Refresh()
		{
			if (userData.GetGamePlayed(out var gameInfo))
			{
				if (gameInfo.Game.IsMe)
				{
					options.InThisGame.Set(icon, message, gameInfo);
				}
				else
				{
					options.InAnotherGame.Set(icon, message, gameInfo);
				}
				return;
			}
			switch (userData.State)
			{
			case EPersonaState.k_EPersonaStateAway:
				options.Away.Set(icon, message, null);
				break;
			case EPersonaState.k_EPersonaStateBusy:
				options.Busy.Set(icon, message, null);
				break;
			case EPersonaState.k_EPersonaStateOnline:
				options.Online.Set(icon, message, null);
				break;
			case EPersonaState.k_EPersonaStateSnooze:
				options.Snooze.Set(icon, message, null);
				break;
			case EPersonaState.k_EPersonaStateLookingToPlay:
				options.LookingToPlay.Set(icon, message, null);
				break;
			case EPersonaState.k_EPersonaStateLookingToTrade:
				options.LookingToTrade.Set(icon, message, null);
				break;
			default:
				options.Offline.Set(icon, message, null);
				break;
			}
		}
	}
}
