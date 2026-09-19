namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ILevelQuotaInitializer
	{
		void InitializeForCurrentLevel();

		void HarvestCollectedGoldToWallet();

		void ResetRunWallet();

		void ApplyChapterStartWallet();

		void ApplyThemeBoundaryCarryCap();
	}
}
