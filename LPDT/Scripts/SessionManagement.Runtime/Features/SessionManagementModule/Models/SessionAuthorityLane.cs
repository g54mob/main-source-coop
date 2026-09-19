namespace Features.SessionManagementModule.Models
{
	public enum SessionAuthorityLane
	{
		None = 0,
		LobbyReleaseRoster = 1,
		LobbyLoadFirstLevel = 2,
		LevelEnterOpenGates = 3,
		LevelEnterOpenGlobalScope = 4,
		LevelToShop = 5,
		LevelEndRun = 6,
		ShopOpenScope = 7,
		ShopReset = 8,
		LobbyOpenRunScope = 9,
		LobbyCloseRunScope = 10,
		LevelEnterRegisterAvatars = 11,
		LevelEnterSelectWeather = 12,
		LevelEnterSpawnContent = 13,
		LevelInitializeQuota = 14,
		LevelExitCloseGlobalScope = 15,
		LobbyExitMarkSessionStarted = 16,
		LobbyEnterMarkSessionNotStarted = 17,
		LevelEnterActivate = 18,
		LevelExitDeactivate = 19,
		LevelExitHarvestGold = 20,
		LevelExitStragglerDamage = 21,
		LobbyEnterActivate = 22,
		LobbyExitDeactivate = 23,
		ShopEnterActivate = 24,
		ShopExitDeactivate = 25,
		LevelEnterResetStatistics = 26,
		LevelEnterActivateGamePhases = 27,
		LobbyEnterResetWallet = 28,
		LobbyExitApplyChapterStartWallet = 29,
		LevelExitApplyThemeBoundaryCap = 30,
		LevelExitFlushEnemySpawnAnalytics = 31
	}
}
