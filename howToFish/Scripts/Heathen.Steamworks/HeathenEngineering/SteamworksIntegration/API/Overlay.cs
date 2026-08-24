using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.API
{
	public static class Overlay
	{
		public static class Client
		{
			private static bool isShowing = false;

			private static ENotificationPosition notificationPosition = ENotificationPosition.k_EPositionBottomRight;

			private static Vector2Int notificationInset = Vector2Int.zero;

			private static GameOverlayActivatedEvent eventGameOverlayActivated = new GameOverlayActivatedEvent();

			private static GameServerChangeRequestedEvent eventGameServerChangeRequested = new GameServerChangeRequestedEvent();

			private static GameLobbyJoinRequestedEvent eventGameLobbyJoinRequested = new GameLobbyJoinRequestedEvent();

			private static GameRichPresenceJoinRequestedEvent eventGameRichPresenceJoinRequest = new GameRichPresenceJoinRequestedEvent();

			private static Callback<GameOverlayActivated_t> m_GameOverlayActivated_t;

			private static Callback<GameServerChangeRequested_t> m_GameServerChangeRequested_t;

			private static Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested_t;

			private static Callback<GameRichPresenceJoinRequested_t> m_GameRichPresenceJoinRequested_t;

			public static bool IsEnabled => SteamUtils.IsOverlayEnabled();

			public static bool IsShowing => isShowing;

			public static ENotificationPosition NotificationPosition
			{
				get
				{
					return notificationPosition;
				}
				set
				{
					notificationPosition = value;
					SteamUtils.SetOverlayNotificationPosition(notificationPosition);
				}
			}

			public static Vector2Int NotificationInset
			{
				get
				{
					return notificationInset;
				}
				set
				{
					notificationInset = value;
					SteamUtils.SetOverlayNotificationInset(value.x, value.y);
				}
			}

			public static GameOverlayActivatedEvent EventGameOverlayActivated
			{
				get
				{
					if (m_GameOverlayActivated_t == null)
					{
						m_GameOverlayActivated_t = Callback<GameOverlayActivated_t>.Create(delegate(GameOverlayActivated_t r)
						{
							isShowing = r.m_bActive == 1;
							eventGameOverlayActivated.Invoke(isShowing);
						});
					}
					return eventGameOverlayActivated;
				}
			}

			public static GameServerChangeRequestedEvent EventGameServerChangeRequested
			{
				get
				{
					if (m_GameServerChangeRequested_t == null)
					{
						m_GameServerChangeRequested_t = Callback<GameServerChangeRequested_t>.Create(delegate(GameServerChangeRequested_t r)
						{
							eventGameServerChangeRequested.Invoke(r.m_rgchServer, r.m_rgchPassword);
						});
					}
					return eventGameServerChangeRequested;
				}
			}

			public static GameLobbyJoinRequestedEvent EventGameLobbyJoinRequested
			{
				get
				{
					if (m_GameLobbyJoinRequested_t == null)
					{
						m_GameLobbyJoinRequested_t = Callback<GameLobbyJoinRequested_t>.Create(delegate(GameLobbyJoinRequested_t r)
						{
							eventGameLobbyJoinRequested.Invoke(r.m_steamIDLobby, r.m_steamIDFriend);
						});
					}
					return eventGameLobbyJoinRequested;
				}
			}

			public static GameRichPresenceJoinRequestedEvent EventGameRichPresenceJoinRequested
			{
				get
				{
					if (m_GameRichPresenceJoinRequested_t == null)
					{
						m_GameRichPresenceJoinRequested_t = Callback<GameRichPresenceJoinRequested_t>.Create(delegate(GameRichPresenceJoinRequested_t r)
						{
							eventGameRichPresenceJoinRequest.Invoke(r.m_steamIDFriend, r.m_rgchConnect);
						});
					}
					return eventGameRichPresenceJoinRequest;
				}
			}

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Init()
			{
				eventGameOverlayActivated = new GameOverlayActivatedEvent();
				eventGameServerChangeRequested = new GameServerChangeRequestedEvent();
				eventGameLobbyJoinRequested = new GameLobbyJoinRequestedEvent();
				eventGameRichPresenceJoinRequest = new GameRichPresenceJoinRequestedEvent();
				m_GameOverlayActivated_t = null;
				m_GameServerChangeRequested_t = null;
				m_GameLobbyJoinRequested_t = null;
				m_GameRichPresenceJoinRequested_t = null;
			}

			public static void RegisterEvents()
			{
				if (m_GameOverlayActivated_t == null)
				{
					m_GameOverlayActivated_t = Callback<GameOverlayActivated_t>.Create(delegate(GameOverlayActivated_t r)
					{
						isShowing = r.m_bActive == 1;
						eventGameOverlayActivated.Invoke(isShowing);
					});
				}
				if (m_GameServerChangeRequested_t == null)
				{
					m_GameServerChangeRequested_t = Callback<GameServerChangeRequested_t>.Create(delegate(GameServerChangeRequested_t r)
					{
						eventGameServerChangeRequested.Invoke(r.m_rgchServer, r.m_rgchPassword);
					});
				}
				if (m_GameLobbyJoinRequested_t == null)
				{
					m_GameLobbyJoinRequested_t = Callback<GameLobbyJoinRequested_t>.Create(delegate(GameLobbyJoinRequested_t r)
					{
						eventGameLobbyJoinRequested.Invoke(r.m_steamIDLobby, r.m_steamIDFriend);
					});
				}
			}

			public static void Activate(string dialog)
			{
				SteamFriends.ActivateGameOverlay(dialog);
			}

			public static void Activate(OverlayDialog dialog)
			{
				SteamFriends.ActivateGameOverlay(dialog.ToString());
			}

			public static void ActivateInviteDialog(CSteamID lobbyId)
			{
				SteamFriends.ActivateGameOverlayInviteDialog(lobbyId);
			}

			public static void ActivateInviteDialog(string connectionString)
			{
				SteamFriends.ActivateGameOverlayInviteDialogConnectString(connectionString);
			}

			public static void ActivateRemotePlayInviteDialog(CSteamID lobbyId)
			{
				SteamFriends.ActivateGameOverlayRemotePlayTogetherInviteDialog(lobbyId);
			}

			public static void Activate(AppData appID, EOverlayToStoreFlag flag)
			{
				SteamFriends.ActivateGameOverlayToStore(appID, flag);
			}

			public static void Activate(string dialog, CSteamID steamId)
			{
				SteamFriends.ActivateGameOverlayToUser(dialog, steamId);
			}

			public static void Activate(FriendDialog dialog, CSteamID steamId)
			{
				SteamFriends.ActivateGameOverlayToUser(dialog.ToString(), steamId);
			}

			public static void ActivateWebPage(string url)
			{
				SteamFriends.ActivateGameOverlayToWebPage(url);
			}
		}
	}
}
