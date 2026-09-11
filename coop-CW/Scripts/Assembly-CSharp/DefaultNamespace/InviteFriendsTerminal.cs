using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
	public class InviteFriendsTerminal : Interactable
	{
		public TextMeshProUGUI text;

		private string m_InviteFriendsText;

		private string m_GameFullText;

		private string m_GameStartedText;

		private string m_OfflineText;

		private bool IsOffline => PhotonNetwork.OfflineMode;

		private bool IsGameFull => PlayerHandler.instance.players.Count > 4;

		private bool IsGameStarted => SurfaceNetworkHandler.HasStarted;

		protected override void Awake()
		{
			base.Awake();
			base.gameObject.layer = LayerMask.NameToLayer("Interactable");
			LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
			OnLanguageChanged();
		}

		private void OnDestroy()
		{
			LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
		}

		private void OnLanguageChanged()
		{
			m_InviteFriendsText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.InviteFriends);
			m_GameFullText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.GameFull);
			m_GameStartedText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.GameStarted);
			m_OfflineText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Offline);
		}

		private void Update()
		{
			if (text != null)
			{
				text.text = (IsValid(Player.localPlayer) ? m_InviteFriendsText : m_OfflineText);
			}
			if (IsGameStarted)
			{
				hoverText = m_GameStartedText;
			}
			else if (IsGameFull)
			{
				hoverText = m_GameFullText;
			}
			else
			{
				hoverText = m_InviteFriendsText;
			}
		}

		public override bool IsValid(Player player)
		{
			if (!IsGameFull && !IsGameStarted)
			{
				return !IsOffline;
			}
			return false;
		}

		public override void Interact(Player player)
		{
			Debug.Log("Tried to interacted with terminal");
			if (!IsValid(player))
			{
				Debug.Log($"Cant invite friends: IsGameFull={IsGameFull}, IsGameStarted={IsGameStarted}");
				return;
			}
			Debug.Log("Try open invite friends");
			if (MainMenuHandler.SteamLobbyHandler != null)
			{
				MainMenuHandler.SteamLobbyHandler.InviteScreen();
				if (PhotonNetwork.InRoom)
				{
					Photon.Realtime.Room currentRoom = PhotonNetwork.CurrentRoom;
					Debug.Log("In Current Room: " + currentRoom.Name);
				}
				else
				{
					Debug.LogError("User is not in a room");
				}
			}
		}
	}
}
