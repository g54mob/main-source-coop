using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
	[SerializeField]
	private Image _iconImage;

	[SerializeField]
	private Image _statusImage;

	[SerializeField]
	private TMP_Text _usernameText;

	private string username;

	private CSteamID steamID;

	private bool isOnline;

	private Sprite icon;

	protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

	private void Start()
	{
		avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
	}

	public void InitializeFriendItem(string name, ulong id, bool status)
	{
		username = name;
		steamID = new CSteamID(id);
		isOnline = status;
		_usernameText.text = username;
		GetIcon();
	}

	public void InviteFriend()
	{
		SteamMatchmaking.InviteUserToLobby(SteamLobby.LobbyID, steamID);
	}

	private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
	{
		if (!(callback.m_steamID != steamID))
		{
			GetIcon();
		}
	}

	private void GetIcon()
	{
		Texture2D avatar = SteamHelper.GetAvatar(steamID);
		if ((bool)avatar)
		{
			icon = SteamHelper.ConvertTextureToSprite(avatar);
			_iconImage.sprite = icon;
		}
	}
}
