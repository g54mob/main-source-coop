using System;

[Flags]
public enum LobbySettingFields
{
	None = 0,
	PrepSeconds = 1,
	HuntSeconds = 2,
	RoundEndSeconds = 4,
	MinHiders = 8,
	MinHunters = 0x10,
	TauntInterval = 0x400,
	SelfDamage = 0x800,
	HunterShare = 0x1000,
	MinPlayers = 0x20,
	ScoreLimit = 0x40,
	TimeLimit = 0x80,
	ExtraWarmup = 0x100,
	RespawnTime = 0x200
}
