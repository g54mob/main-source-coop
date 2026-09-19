using System;
using Features.MultiplayerSessionServices.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.GameModeModule.Scripts.Views
{
	public class GameModePresenter : PresenterBehaviour<GameModeViewBase>
	{
		private readonly GameModeModel _gameModeModel;

		private readonly MultiplayerModel _multiplayerModel;

		public GameModePresenter(GameModeModel gameModeModel, MultiplayerModel multiplayerModel)
		{
			_gameModeModel = gameModeModel;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnGameModeChanged += ChangeGameMode;
			base.View.SetGameModeContainerActive(_multiplayerModel.NetworkRunner.IsSharedModeMasterClient);
			base.View.RefreshGameModeDropdown(_gameModeModel.CurrentGameMode);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OnGameModeChanged -= ChangeGameMode;
		}

		private void ChangeGameMode(string selectedGameModeText)
		{
			if (Enum.TryParse<GameModeType>(selectedGameModeText, out var result))
			{
				_gameModeModel.CurrentGameMode = result;
				base.View.RefreshGameModeDropdown(_gameModeModel.CurrentGameMode);
			}
		}
	}
}
