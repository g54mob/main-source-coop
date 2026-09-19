using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkInputModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine.InputSystem;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerResurrectionInteraction : NetworkBehaviour
	{
		private MultiplayerModel _multiplayerModel;

		private PlayerResurrectionModel _playerResurrectionModel;

		private IPlayerStateService _playerStateService;

		private IPlayerResurrectionService _playerResurrectionService;

		[Inject]
		public void InjectDependencies(MultiplayerModel multiplayerModel, PlayerResurrectionModel playerResurrectionModel, IPlayerStateService playerStateService, IPlayerResurrectionService playerResurrectionService)
		{
			_multiplayerModel = multiplayerModel;
			_playerResurrectionModel = playerResurrectionModel;
			_playerStateService = playerStateService;
			_playerResurrectionService = playerResurrectionService;
		}

		public override void FixedUpdateNetwork()
		{
			if (!(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer) && GetInput<NetworkInputActions>(out var input) && input.ItemInteractPhase.IsSet(InputActionPhase.Started) && _playerResurrectionModel.PlayersAvailableToResurrection.Count > 0 && _playerStateService.IsPlayerAlive(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				int playerId = _playerResurrectionModel.PlayersAvailableToResurrection[0];
				_playerResurrectionService.ResurrectPlayer(playerId, restoreHp: true, _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
