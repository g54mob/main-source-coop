public static class LobbySettingSets
{
	public const LobbySettingFields PropHunt = LobbySettingFields.PrepSeconds | LobbySettingFields.HuntSeconds | LobbySettingFields.RoundEndSeconds | LobbySettingFields.TauntInterval | LobbySettingFields.SelfDamage | LobbySettingFields.HunterShare;

	public const LobbySettingFields Deathmatch = LobbySettingFields.ScoreLimit | LobbySettingFields.TimeLimit | LobbySettingFields.ExtraWarmup | LobbySettingFields.RespawnTime;

	public const LobbySettingFields All = LobbySettingFields.PrepSeconds | LobbySettingFields.HuntSeconds | LobbySettingFields.RoundEndSeconds | LobbySettingFields.TauntInterval | LobbySettingFields.SelfDamage | LobbySettingFields.HunterShare | LobbySettingFields.ScoreLimit | LobbySettingFields.TimeLimit | LobbySettingFields.ExtraWarmup | LobbySettingFields.RespawnTime;
}
