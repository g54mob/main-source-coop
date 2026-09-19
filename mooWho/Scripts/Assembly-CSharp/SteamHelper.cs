using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public static class SteamHelper
{
	public static Texture2D GetAvatar(CSteamID steamID)
	{
		if (!SteamManager.Initialized)
		{
			return null;
		}
		int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(steamID);
		if (largeFriendAvatar == -1)
		{
			return null;
		}
		Texture2D texture2D = null;
		if (SteamUtils.GetImageSize(largeFriendAvatar, out var pnWidth, out var pnHeight))
		{
			byte[] array = new byte[pnWidth * pnHeight * 4];
			if (SteamUtils.GetImageRGBA(largeFriendAvatar, array, (int)(pnWidth * pnHeight * 4)))
			{
				texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false, linear: false);
				texture2D.LoadRawTextureData(array);
				texture2D.Apply();
			}
		}
		return texture2D;
	}

	public static Sprite ConvertTextureToSprite(Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
	}

	public static List<CSteamID> GetMembersInLobby()
	{
		List<CSteamID> list = new List<CSteamID>();
		int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(SteamLobby.LobbyID);
		for (int i = 0; i < numLobbyMembers; i++)
		{
			CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyID, i);
			list.Add(lobbyMemberByIndex);
		}
		return list;
	}
}
