using System;

[Serializable]
public struct PlayerInfoData
{
	public string username;

	public ulong steamId;

	public PlayerInfoData(string username, ulong steamId)
	{
		this.username = username;
		this.steamId = steamId;
	}
}
