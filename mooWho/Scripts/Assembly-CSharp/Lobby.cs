using System;
using Steamworks;

[Serializable]
public class Lobby
{
	public CSteamID lobbyID;

	public string name;

	public string joinCode;

	public string lobbyType;

	public string region;

	public int memberCount;

	public int maxMembers;

	public string state;

	public bool IsJoinable
	{
		get
		{
			if ((lobbyType ?? "public").ToLower() == "public" && (state ?? "lobby").ToLower() != "ingame")
			{
				if (maxMembers > 0)
				{
					return memberCount < maxMembers;
				}
				return true;
			}
			return false;
		}
	}

	public Lobby(CSteamID lobbyID, string name)
	{
		this.lobbyID = lobbyID;
		this.name = name;
	}
}
