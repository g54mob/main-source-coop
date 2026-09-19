using Features.CameraModelModule;
using Features.InputModule.Scripts.Generated;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;

namespace Features.SessionManagementModule.Models
{
	public sealed class SessionPlayerControl : ISessionPlayerControl
	{
		private readonly IInputService _inputService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly CameraModel _cameraModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ISessionPlayerPresence _sessionPlayerPresence;

		public SessionPlayerControl(IInputService inputService, PlayerMovableModel playerMovableModel, CameraModel cameraModel, IPlayerStateService playerStateService, MultiplayerModel multiplayerModel, ISessionPlayerPresence sessionPlayerPresence)
		{
			_inputService = inputService;
			_playerMovableModel = playerMovableModel;
			_cameraModel = cameraModel;
			_playerStateService = playerStateService;
			_multiplayerModel = multiplayerModel;
			_sessionPlayerPresence = sessionPlayerPresence;
		}

		public void LockControlls()
		{
			_cameraModel.AddCameraInputLockReason(LockCameraInputReasonEnum.SessionLoading);
			_playerMovableModel.AddLockMovementReason(LockMovementReasonEnum.SessionLoading);
		}

		public void UnlockControlls()
		{
			_inputService.EnableMovementMap();
			_cameraModel.RemoveCameraInputLockReason(LockCameraInputReasonEnum.SessionLoading);
			_playerMovableModel.RemoveLockMovementReason(LockMovementReasonEnum.SessionLoading);
		}

		public void ApplyPersistedLifeState()
		{
			PlayerState persistedLifeState = (PlayerState)_sessionPlayerPresence.PersistedLifeState;
			PlayerState playerState = ((persistedLifeState != PlayerState.Dead && persistedLifeState != PlayerState.PreDeadCrouch) ? PlayerState.Alive : persistedLifeState);
			if (playerState == PlayerState.Alive && persistedLifeState != PlayerState.None && _playerStateService.IsLocalPlayerHealthDepleted())
			{
				playerState = PlayerState.PreDeadCrouch;
			}
			_playerStateService.ChangePlayerState(playerState);
		}

		public void MirrorLifeStateToPersistence()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			_sessionPlayerPresence.PersistedLifeState = (int)_playerStateService.GetPlayerState(playerId);
		}
	}
}
