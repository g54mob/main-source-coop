namespace EvilCore.UI.MainMenu
{
	public interface IMainMenuUIManager
	{
		void ShowMainPanel();

		void ShowJoinGame();

		void ShowCreateGame();

		void ShowSinglePlayer();

		void ShowSettings();

		void ShowLoadGame();

		void ShowLoadingGame();

		void ShowNetworkError();

		void HideAll();
	}
}
