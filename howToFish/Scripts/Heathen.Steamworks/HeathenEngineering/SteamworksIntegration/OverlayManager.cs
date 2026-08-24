using System.Collections.Generic;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[DisallowMultipleComponent]
	public class OverlayManager : MonoBehaviour
	{
		public enum ManagedEvents
		{
			JoinGameRequested = 0,
			LobbyInviteAccepted = 1,
			OverlayActivated = 2,
			ServerChangeRequested = 3
		}

		[SerializeField]
		private List<ManagedEvents> m_Delegates;

		[SerializeField]
		private ENotificationPosition notificationPosition = ENotificationPosition.k_EPositionBottomRight;

		[SerializeField]
		private Vector2Int notificationInset = Vector2Int.zero;

		public GameOverlayActivatedEvent evtOverlayActivated;

		public GameLobbyJoinRequestedEvent evtGameLobbyJoinRequested;

		public GameServerChangeRequestedEvent evtGameServerChangeRequested;

		public GameRichPresenceJoinRequestedEvent evtRichPresenceJoinRequested;

		public ENotificationPosition NotificationPosition
		{
			get
			{
				return Overlay.Client.NotificationPosition;
			}
			set
			{
				Overlay.Client.NotificationPosition = value;
			}
		}

		public Vector2Int NotificationInset
		{
			get
			{
				return Overlay.Client.NotificationInset;
			}
			set
			{
				Overlay.Client.NotificationInset = value;
			}
		}

		public bool IsShowing => Overlay.Client.IsShowing;

		public bool IsEnabled => Overlay.Client.IsEnabled;

		private void OnEnable()
		{
			if (App.Initialized)
			{
				EnabledProcess();
			}
			else
			{
				App.evtSteamInitialized.AddListener(EnabledProcess);
			}
		}

		private void EnabledProcess()
		{
			NotificationPosition = notificationPosition;
			NotificationInset = notificationInset;
			Overlay.Client.EventGameOverlayActivated.AddListener(evtOverlayActivated.Invoke);
			Overlay.Client.EventGameServerChangeRequested.AddListener(evtGameServerChangeRequested.Invoke);
			Overlay.Client.EventGameLobbyJoinRequested.AddListener(evtGameLobbyJoinRequested.Invoke);
			Overlay.Client.EventGameRichPresenceJoinRequested.AddListener(evtRichPresenceJoinRequested.Invoke);
		}

		private void OnDisable()
		{
			Overlay.Client.EventGameOverlayActivated.RemoveListener(evtOverlayActivated.Invoke);
			Overlay.Client.EventGameServerChangeRequested.RemoveListener(evtGameServerChangeRequested.Invoke);
			Overlay.Client.EventGameLobbyJoinRequested.RemoveListener(evtGameLobbyJoinRequested.Invoke);
			Overlay.Client.EventGameRichPresenceJoinRequested.RemoveListener(evtRichPresenceJoinRequested.Invoke);
		}

		public void Open(string dialog)
		{
			Overlay.Client.Activate(dialog);
		}

		public void Open(OverlayDialog dialog)
		{
			Overlay.Client.Activate(dialog);
		}

		public void OpenLobbyInvite(CSteamID lobbyId)
		{
			Overlay.Client.ActivateInviteDialog(lobbyId);
		}

		public void OpenConnectStringInvite(string connectionString)
		{
			Overlay.Client.ActivateInviteDialog(connectionString);
		}

		public void OpenRemotePlayInvite(CSteamID lobbyId)
		{
			Overlay.Client.ActivateRemotePlayInviteDialog(lobbyId);
		}

		public void OpenStore(AppData appID, EOverlayToStoreFlag flag)
		{
			Overlay.Client.Activate(appID, flag);
		}

		public void OpenUser(string dialog, CSteamID steamId)
		{
			Overlay.Client.Activate(dialog, steamId);
		}

		public void OpenUser(FriendDialog dialog, CSteamID steamId)
		{
			Overlay.Client.Activate(dialog.ToString(), steamId);
		}

		public void OpenWebPage(string url)
		{
			Overlay.Client.ActivateWebPage(url);
		}
	}
}
