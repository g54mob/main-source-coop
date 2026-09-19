using System.Collections.Generic;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerResurrectionCollision : NetworkBehaviour
	{
		private readonly List<int> _playersInResurrectionRange = new List<int>();

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		private PlayerResurrectionModel _playerResurrectionModel;

		private MultiplayerModel _multiplayerModel;

		private IPlayerStateService _playerStateService;

		[Inject]
		public void InjectDependencies(PlayersStatesSynchronizer playersStatesSynchronizer, PlayerResurrectionModel playerResurrectionModel, MultiplayerModel multiplayerModel, IPlayerStateService playerStateService)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_playerResurrectionModel = playerResurrectionModel;
			_multiplayerModel = multiplayerModel;
			_playerStateService = playerStateService;
		}

		private void Awake()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += ProcessPlayerSomePlayerStateChanged;
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= ProcessPlayerSomePlayerStateChanged;
		}

		private void ProcessPlayerSomePlayerStateChanged(PlayerStateData stateData)
		{
			if (_playersInResurrectionRange.Contains(stateData.PlayerId))
			{
				if (!_playerStateService.IsPlayerAlive(stateData.PlayerId))
				{
					_playerResurrectionModel.AddResurrectionAvailablePlayer(stateData.PlayerId);
				}
				else
				{
					_playerResurrectionModel.RemoveResurrectionAvailablePlayer(stateData.PlayerId);
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer) && other.TryGetComponent<PlayerCharacterMovableBase>(out var component) && _playersStatesSynchronizer.TryGetState(component.Object.InputAuthority.PlayerId, out var _))
			{
				_playersInResurrectionRange.Add(component.Object.InputAuthority.PlayerId);
				if (!_playerStateService.IsPlayerAlive(component.Object.InputAuthority.PlayerId))
				{
					_playerResurrectionModel.AddResurrectionAvailablePlayer(component.Object.InputAuthority.PlayerId);
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer) && other.TryGetComponent<PlayerCharacterMovableBase>(out var component))
			{
				if (_playersStatesSynchronizer.TryGetState(component.Object.InputAuthority.PlayerId, out var _))
				{
					_playersInResurrectionRange.Remove(component.Object.InputAuthority.PlayerId);
				}
				_playerResurrectionModel.RemoveResurrectionAvailablePlayer(component.Object.InputAuthority.PlayerId);
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
