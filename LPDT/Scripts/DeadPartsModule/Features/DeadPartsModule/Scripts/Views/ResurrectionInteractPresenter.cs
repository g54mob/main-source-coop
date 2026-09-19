using System.Linq;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using PlayerCustomization;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DeadPartsModule.Scripts.Views
{
	public class ResurrectionInteractPresenter : PresenterBehaviour<ResurrectionInteractViewBase>
	{
		private readonly PlayerResurrectionModel _playerResurrectionModel;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly MultiplayerModel _multiplayerModel;

		public ResurrectionInteractPresenter(PlayerResurrectionModel playerResurrectionModel, PlayerCustomizationModel playerCustomizationModel, IPlayerStateService playerStateService, MultiplayerModel multiplayerModel)
		{
			_playerResurrectionModel = playerResurrectionModel;
			_playerCustomizationModel = playerCustomizationModel;
			_playerStateService = playerStateService;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewSet()
		{
			_playerResurrectionModel.OnResurrectionAvailablePlayerChanged += UpdateResurrectionInteractionView;
			UpdateResurrectionInteractionView();
		}

		protected override void OnDisposed()
		{
			_playerResurrectionModel.OnResurrectionAvailablePlayerChanged -= UpdateResurrectionInteractionView;
		}

		private void UpdateResurrectionInteractionView()
		{
			if (_playerStateService.IsPlayerAlive(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId) && _playerResurrectionModel.PlayersAvailableToResurrection.Count > 0)
			{
				int firstPlayerId = _playerResurrectionModel.PlayersAvailableToResurrection[0];
				PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData slotData) => slotData.PlayerId == firstPlayerId);
				if (playerCustomizationSlotData != null && base.View.ResurrectionHintText != null)
				{
					base.View.ResurrectionHintText.text = "Press E to resurrect " + playerCustomizationSlotData.Nickname;
					base.View.ResurrectionHintText.gameObject.SetActive(value: true);
				}
			}
			else if (base.View.ResurrectionHintText != null)
			{
				base.View.ResurrectionHintText.gameObject.SetActive(value: false);
			}
		}
	}
}
