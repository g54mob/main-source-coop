using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListItem : MonoBehaviour
{
	[Header("UI")]
	public TextMeshProUGUI ownerNameText;

	public TextMeshProUGUI typeText;

	public TextMeshProUGUI playerCountText;

	public TextMeshProUGUI regionText;

	public Button joinButton;

	private Lobby _lobby;

	public void Setup(Lobby lobby)
	{
		_lobby = lobby;
		if (ownerNameText != null)
		{
			ownerNameText.text = (string.IsNullOrEmpty(lobby.name) ? Localization.Get("LABEL_UNKNOWN") : lobby.name);
		}
		if (playerCountText != null)
		{
			playerCountText.text = $"{lobby.memberCount}/{lobby.maxMembers}";
		}
		if (regionText != null)
		{
			regionText.text = (string.IsNullOrEmpty(lobby.region) ? "" : lobby.region.ToUpper());
		}
		string text = (lobby.lobbyType ?? "public").ToLower();
		bool flag = (lobby.state ?? "lobby").ToLower() == "ingame";
		bool isJoinable = lobby.IsJoinable;
		if (typeText != null)
		{
			if (flag)
			{
				typeText.text = Localization.Get("ROOM_TYPE_INGAME");
			}
			else if (!(text == "inviteonly"))
			{
				if (text == "private")
				{
					typeText.text = Localization.Get("ROOM_TYPE_PRIVATE");
				}
				else
				{
					typeText.text = Localization.Get("ROOM_TYPE_PUBLIC");
				}
			}
			else
			{
				typeText.text = Localization.Get("ROOM_TYPE_INVITE_ONLY");
			}
		}
		if (joinButton != null)
		{
			joinButton.gameObject.SetActive(isJoinable);
			joinButton.onClick.RemoveAllListeners();
			joinButton.onClick.AddListener(OnJoinClicked);
		}
	}

	private void OnJoinClicked()
	{
		if (_lobby != null && !(SteamLobby.instance == null) && !SteamLobby.instance.JoinLobby(_lobby.lobbyID))
		{
			LobbyListUI.Instance?.ShowJoinFeedback(Localization.Get("LOBBY_JOIN_UNAVAILABLE"));
		}
	}
}
