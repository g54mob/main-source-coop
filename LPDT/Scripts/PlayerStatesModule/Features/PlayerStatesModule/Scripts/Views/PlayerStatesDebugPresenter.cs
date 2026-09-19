using Features.MultiplayerSessionServices.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.PlayerStatesModule.Scripts.Views
{
	[PublicAPI]
	public class PlayerStatesDebugPresenter : PresenterBehaviour<PlayerStatesDebugViewBase>
	{
		private readonly IPlayerStateService _playerStateService;

		private readonly MultiplayerModel _multiplayerModel;

		public PlayerStatesDebugPresenter(IPlayerStateService playerStateService, MultiplayerModel multiplayerModel)
		{
			_playerStateService = playerStateService;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			RefreshFreeFlyToggle();
			base.View.FreeFlyStateToggle.onValueChanged.AddListener(OnFreeFlyToggleChanged);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.FreeFlyStateToggle.onValueChanged.RemoveListener(OnFreeFlyToggleChanged);
		}

		private void RefreshFreeFlyToggle()
		{
			PlayerState playerState = _playerStateService.GetPlayerState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			base.View.FreeFlyStateToggle.SetIsOnWithoutNotify(playerState == PlayerState.FreeFly);
		}

		private void OnFreeFlyToggleChanged(bool isOn)
		{
			_playerStateService.ChangePlayerState((!isOn) ? PlayerState.Alive : PlayerState.FreeFly);
		}
	}
}
