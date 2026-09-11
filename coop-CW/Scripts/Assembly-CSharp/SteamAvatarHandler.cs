using System.Collections.Generic;
using Photon.Realtime;
using Steamworks;
using UnityEngine;

public class SteamAvatarHandler : RetrievableSingleton<SteamAvatarHandler>
{
	private HashSet<ulong> m_currentlyRequestedAvatars = new HashSet<ulong>();

	private Callback<PersonaStateChange_t> m_PersonaStateChange;

	protected override void OnCreated()
	{
		base.OnCreated();
		m_PersonaStateChange = Callback<PersonaStateChange_t>.Create(OnPersonaStateChange);
	}

	private void OnPersonaStateChange(PersonaStateChange_t param)
	{
		if (m_currentlyRequestedAvatars.Contains(param.m_ulSteamID))
		{
			m_currentlyRequestedAvatars.Remove(param.m_ulSteamID);
		}
	}

	public static bool HasAvatarForPlayer(Photon.Realtime.Player player)
	{
		if (!TryGetSteamIDForPlayer(player, out var steamID))
		{
			return false;
		}
		if (RequestSteamAvatar(steamID))
		{
			return true;
		}
		return false;
	}

	public static bool TryGetSteamIDForPlayer(Photon.Realtime.Player player, out CSteamID steamID)
	{
		steamID = default(CSteamID);
		if (!player.CustomProperties.ContainsKey("SteamID"))
		{
			return false;
		}
		if (!ulong.TryParse((string)player.CustomProperties["SteamID"], out var result))
		{
			return false;
		}
		steamID = new CSteamID(result);
		return true;
	}

	public static bool TryGetAvatarForPlayer(Photon.Realtime.Player player, out Sprite icon)
	{
		icon = null;
		if (!TryGetSteamIDForPlayer(player, out var steamID))
		{
			return false;
		}
		int mediumFriendAvatar = SteamFriends.GetMediumFriendAvatar(steamID);
		if (!SteamUtils.GetImageSize(mediumFriendAvatar, out var pnWidth, out var pnHeight))
		{
			return false;
		}
		uint num = pnWidth * pnHeight * 4;
		byte[] array = new byte[num];
		if (!SteamUtils.GetImageRGBA(mediumFriendAvatar, array, (int)num))
		{
			return false;
		}
		Texture2D texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false);
		texture2D.LoadRawTextureData(array);
		texture2D.Apply();
		icon = Sprite.Create(texture2D, new Rect(0f, 0f, pnWidth, pnHeight), new Vector2(0.5f, 0.5f));
		return true;
	}

	private static bool RequestSteamAvatar(CSteamID steamID)
	{
		bool result = !SteamFriends.RequestUserInformation(steamID, bRequireNameOnly: false);
		RetrievableSingleton<SteamAvatarHandler>.Instance.m_currentlyRequestedAvatars.Add(steamID.m_SteamID);
		return result;
	}
}
