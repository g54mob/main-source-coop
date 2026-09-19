using System;
using Features.CameraModelModule;
using Features.DeadPartsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using Zenject;

namespace Features.PlayerSkinModule.Scripts
{
	public class LocalPlayerSkinSystem : IInitializable, IDisposable
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IPlayerStateSkinSwitchService _playerStateSkinSwitchService;

		private readonly SkinChangedEvent _skinChangedEvent;

		private readonly SkinModel _skinModel;

		private readonly CameraModel _cameraModel;

		private readonly CameraTransitionSkinUpdateRequest _cameraTransitionSkinUpdateRequest;

		private readonly PlayerDeadPartTypes _playerDeadPartTypes;

		private readonly LocalBodyHideSuspensionModel _localBodyHideSuspensionModel;

		private bool _isDead;

		private bool _isSkinRegistered;

		public LocalPlayerSkinSystem(PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, IPlayerStateSkinSwitchService playerStateSkinSwitchService, SkinChangedEvent skinChangedEvent, SkinModel skinModel, CameraModel cameraModel, CameraTransitionSkinUpdateRequest cameraTransitionSkinUpdateRequest, PlayerDeadPartTypes playerDeadPartTypes, LocalBodyHideSuspensionModel localBodyHideSuspensionModel)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_playerStateSkinSwitchService = playerStateSkinSwitchService;
			_skinChangedEvent = skinChangedEvent;
			_skinModel = skinModel;
			_cameraModel = cameraModel;
			_cameraTransitionSkinUpdateRequest = cameraTransitionSkinUpdateRequest;
			_playerDeadPartTypes = playerDeadPartTypes;
			_localBodyHideSuspensionModel = localBodyHideSuspensionModel;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnStateChanged;
			_skinChangedEvent.OnSkinChanged += OnSkinChanged;
			_cameraTransitionSkinUpdateRequest.OnCameraTransitionSkinUpdateRequested += OnCameraTransitionSkinUpdateRequested;
			_cameraModel.OnActiveCameraChanged += OnActiveCameraChanged;
			_skinModel.OnPlayerSkinUnregistered += OnPlayerSkinUnregistered;
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (_playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				_isDead = IsDeadState(state);
			}
			if (_skinModel.SkinVisibility.ContainsKey(playerId))
			{
				_isSkinRegistered = true;
				ApplyAllKnownPlayerSkinStates();
				ApplyLocalSkin();
			}
			else
			{
				_skinModel.OnPlayerSkinRegistered += OnPlayerSkinRegistered;
			}
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnStateChanged;
			_skinChangedEvent.OnSkinChanged -= OnSkinChanged;
			_cameraTransitionSkinUpdateRequest.OnCameraTransitionSkinUpdateRequested -= OnCameraTransitionSkinUpdateRequested;
			_cameraModel.OnActiveCameraChanged -= OnActiveCameraChanged;
			_skinModel.OnPlayerSkinRegistered -= OnPlayerSkinRegistered;
			_skinModel.OnPlayerSkinUnregistered -= OnPlayerSkinUnregistered;
		}

		private void OnPlayerSkinUnregistered(int playerId)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_isSkinRegistered = false;
				_skinModel.OnPlayerSkinRegistered -= OnPlayerSkinRegistered;
				_skinModel.OnPlayerSkinRegistered += OnPlayerSkinRegistered;
			}
		}

		private void OnPlayerSkinRegistered(int playerId)
		{
			ApplySkinStateForPlayer(playerId);
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_skinModel.OnPlayerSkinRegistered -= OnPlayerSkinRegistered;
				_isSkinRegistered = true;
				ApplyLocalSkin();
			}
		}

		private void OnStateChanged(PlayerStateData data)
		{
			bool num = data.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (num)
			{
				_isDead = IsDeadState(data.PlayerState);
			}
			if (num && ShouldLocalMainBodyBeVisible(data.PlayerState))
			{
				SetLocalMainBodyVisible(isVisible: true);
			}
			ApplySkinStateForPlayer(data.PlayerId);
			if (num)
			{
				ApplyLocalSkin();
				if (!ShouldLocalMainBodyBeVisible(data.PlayerState))
				{
					SetLocalMainBodyVisible(isVisible: false);
				}
				else if (_isDead)
				{
					EnforceLocalDeadLegsHidden();
				}
			}
		}

		private void OnActiveCameraChanged(CameraType cameraType)
		{
			if (!_isSkinRegistered)
			{
				return;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (_playersStatesSynchronizer.TryGetState(playerId, out var state) && state == PlayerState.PreDeadCrouch)
			{
				if (!ShouldLocalMainBodyBeVisible(state))
				{
					SetLocalMainBodyVisible(isVisible: false);
					return;
				}
				SetLocalMainBodyVisible(isVisible: true);
				EnforceLocalDeadLegsHidden();
			}
		}

		private void EnforceLocalDeadLegsHidden()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (_skinModel.BaseCharacterVisibility.TryGetValue(playerId, out var value))
			{
				value.DisableRenderObject();
			}
			if (_skinModel.BottomCharacterVisibility.TryGetValue(playerId, out var value2))
			{
				value2.DisableRenderObject();
			}
			if (_skinModel.SkinVisibility.TryGetValue(playerId, out var value3))
			{
				value3.ApplyDeadSkinLocal();
			}
		}

		private bool ShouldLocalMainBodyBeVisible(PlayerState state)
		{
			if (state == PlayerState.PreDeadCrouch && _cameraModel.ActiveCameraType == CameraType.FPCamera)
			{
				return _localBodyHideSuspensionModel.IsSuspended;
			}
			return true;
		}

		private void SetLocalMainBodyVisible(bool isVisible)
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_skinModel.AllCharacterVisibility.TryGetValue(playerId, out var value))
			{
				return;
			}
			if (isVisible)
			{
				value.EnableRenderObject();
				return;
			}
			value.DisableRenderObject();
			if (_skinModel.SkinVisibility.TryGetValue(playerId, out var value2))
			{
				value2.HideAllSkinRenderObjects();
			}
		}

		private void OnCameraTransitionSkinUpdateRequested(bool playerVisible)
		{
			ApplySkin(playerVisible);
		}

		private void OnSkinChanged(int playerId)
		{
			ApplySkinStateForPlayer(playerId);
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				ApplyLocalSkin();
			}
		}

		private void ApplyAllKnownPlayerSkinStates()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				ApplySkinStateForPlayer(activePlayer.PlayerId);
			}
		}

		private void ApplySkinStateForPlayer(int playerId)
		{
			if (_skinModel.SkinVisibility.ContainsKey(playerId) && _playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				bool flag = IsDeadState(state);
				bool hasBottomPart = _playerDeadPartTypes.HasConnectedPart(playerId);
				_playerStateSkinSwitchService.ApplyPlayerSkinState(playerId, flag, hasBottomPart);
				if (flag)
				{
					EnforceBottomHidden(playerId);
				}
			}
		}

		private void EnforceBottomHidden(int playerId)
		{
			if (_skinModel.BottomCharacterVisibility.TryGetValue(playerId, out var value))
			{
				value.DisableRenderObject();
			}
		}

		private void ApplyLocalSkin()
		{
			ApplySkin();
		}

		private void ApplySkin(bool? playerVisible = null)
		{
			if (_isSkinRegistered)
			{
				int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
				bool isBodyVisible = playerVisible ?? (_cameraModel.ActiveCameraType == CameraType.TPCamera);
				_playerStateSkinSwitchService.SwitchSkin(playerId, _isDead, isBodyVisible);
				EnforceLocalStateVisibility(playerId);
			}
		}

		private void EnforceLocalStateVisibility(int localPlayerId)
		{
			if (_playersStatesSynchronizer.TryGetState(localPlayerId, out var state))
			{
				if (!ShouldLocalMainBodyBeVisible(state))
				{
					SetLocalMainBodyVisible(isVisible: false);
				}
				else if (IsDeadState(state))
				{
					EnforceLocalDeadLegsHidden();
				}
			}
		}

		private static bool IsDeadState(PlayerState state)
		{
			if (state != PlayerState.Dead)
			{
				return state == PlayerState.PreDeadCrouch;
			}
			return true;
		}
	}
}
