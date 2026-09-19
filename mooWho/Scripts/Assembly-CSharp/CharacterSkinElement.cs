using Steamworks;
using UnityEngine;

public class CharacterSkinElement : MonoBehaviour
{
	public Transform nametagPos;

	private bool initialized;

	private Sprite icon;

	protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

	public MyClient client { get; private set; }

	public CSteamID steamId { get; private set; }

	public NametagMarker nametagMarker { get; set; }

	private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
	{
		if (!(callback.m_steamID != steamId))
		{
			Texture2D avatar = SteamHelper.GetAvatar(steamId);
			if ((bool)avatar)
			{
				icon = SteamHelper.ConvertTextureToSprite(avatar);
			}
			nametagMarker.UpdatePFP(icon);
		}
	}

	public void Initialize(MyClient client, bool _isReady)
	{
		string username = (SteamManager.Initialized ? SteamFriends.GetPersonaName() : "Player");
		bool isReady = _isReady;
		steamId = (client ? new CSteamID(client.playerInfo.steamId) : (SteamManager.Initialized ? SteamUser.GetSteamID() : CSteamID.Nil));
		if (nametagMarker == null)
		{
			nametagMarker = (NametagMarker)MarkerHandler.instance.SpawnMarker(0, nametagPos.position, null);
		}
		if (client != null)
		{
			this.client = client;
			username = client.playerInfo.username;
			icon = client.icon;
		}
		if (!initialized)
		{
			initialized = true;
			if (SteamManager.Initialized)
			{
				avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
			}
			Texture2D avatar = SteamHelper.GetAvatar(steamId);
			if ((bool)avatar)
			{
				icon = SteamHelper.ConvertTextureToSprite(avatar);
			}
			nametagMarker.UpdatePFP(icon);
		}
		nametagMarker.UpdateTag(username, isReady);
		nametagMarker.UpdatePFP(icon);
	}

	private void OnDestroy()
	{
		if ((bool)nametagMarker)
		{
			nametagMarker.DestroyMarker();
		}
	}
}
