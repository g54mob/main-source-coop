using Features.MultiplayerSessionServices.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DeadPartsModule.Scripts.Views
{
	[PublicAPI]
	public class PlayerResurrectionDebugPresenter : PresenterBehaviour<PlayerResurrectionDebugViewBase>
	{
		private readonly IPlayerResurrectionService _playerResurrectionService;

		private readonly MultiplayerModel _multiplayerModel;

		public PlayerResurrectionDebugPresenter(IPlayerResurrectionService playerResurrectionService, MultiplayerModel multiplayerModel)
		{
			_playerResurrectionService = playerResurrectionService;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.ResurrectionButton.onClick.AddListener(ResurrectPlayer);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.ResurrectionButton.onClick.AddListener(ResurrectPlayer);
		}

		private void ResurrectPlayer()
		{
			_playerResurrectionService.ResurrectPlayer(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, restoreHp: true, _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
		}
	}
}
