using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.CoroutineUtils.Scripts;
using Features.GameUpdaterModule;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.PlayerStatesModule.Scripts
{
	public class PlayerStunHandleSystem : IInitializable, IDisposable
	{
		private static readonly int IsTemporalStunInProgressHash = Animator.StringToHash("IsTemporalStunInProgress");

		private static readonly int IsStunFromThrowHash = Animator.StringToHash("IsStunFromThrow");

		private const float PLAYER_VELOCITY_THRESHOLD = 0.1f;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly IPlayerStateService _playerStateService;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly PlayerStatesConfiguration _playerStatesConfiguration;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly PlayersRagdollModel _playersRagdollModel;

		private readonly ILocalPlayerThrowService _localPlayerThrowService;

		private CancellationTokenSource _stunCts;

		private IStat _hiddenStaminaStat;

		private IStat _healthStat;

		private bool _isVelocityCheckEnabled;

		public PlayerStunHandleSystem(MultiplayerModel multiplayerModel, PlayerMovableModel playerMovableModel, IGameUpdater gameUpdater, IPlayerStateService playerStateService, SpawnedEntityStatsModel spawnedEntityStatsModel, PlayerStatesConfiguration playerStatesConfiguration, ICoroutineRunner coroutineRunner, PlayersRagdollModel playersRagdollModel, ILocalPlayerThrowService localPlayerThrowService)
		{
			_multiplayerModel = multiplayerModel;
			_playerMovableModel = playerMovableModel;
			_gameUpdater = gameUpdater;
			_playerStateService = playerStateService;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_playerStatesConfiguration = playerStatesConfiguration;
			_coroutineRunner = coroutineRunner;
			_playersRagdollModel = playersRagdollModel;
			_localPlayerThrowService = localPlayerThrowService;
		}

		public void Initialize()
		{
			_localPlayerThrowService.OnThrowEntered += StartVelocityCheck;
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= StartCheckForPermanentStun;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += StartCheckForPermanentStun;
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				StartCheckForPermanentStun(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
		}

		public void Dispose()
		{
			_localPlayerThrowService.OnThrowEntered -= StartVelocityCheck;
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= StartCheckForPermanentStun;
			if (_healthStat != null)
			{
				_healthStat.OnReachedMinValue -= SetPlayerPermanentStunState;
			}
			if (_hiddenStaminaStat != null)
			{
				_hiddenStaminaStat.OnFullValueChanged -= TryStunPlayerByHiddenStamina;
			}
			CancelStunTimer();
		}

		private void StartVelocityCheck()
		{
			_coroutineRunner.StartCoroutine(StartCheckForVelocityDelayed());
			_gameUpdater.OnUpdate += OnUpdate;
		}

		private void OnUpdate()
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				Debug.Log($"{ragdoll.PlayerId} - {ragdoll.IsActiveRagdoll}");
			}
			if (!(_playerMovableModel.MovablePhysics == null) && !(_playerMovableModel.MovablePhysics.RootRb == null) && !(_playerMovableModel.MovablePhysics.RootRb.linearVelocity.magnitude > 0.1f) && _isVelocityCheckEnabled)
			{
				_gameUpdater.OnUpdate -= OnUpdate;
				_isVelocityCheckEnabled = false;
				if (_healthStat.FullValue > 0f)
				{
					StunDurationPreset preset = _localPlayerThrowService.ConsumePendingStunPreset();
					AddStunReasonTemporary(_playerStatesConfiguration.ResolveStunDuration(preset));
					_localPlayerThrowService.ExitThrow(isGoingToDead: false);
					_playerStateService.ChangePlayerState(PlayerState.Alive);
				}
				else
				{
					_localPlayerThrowService.ExitThrow(isGoingToDead: true);
					_playerStateService.ChangePlayerState(PlayerState.PreDeadCrouch);
				}
			}
		}

		private void StartCheckForPermanentStun(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				if (_healthStat != null)
				{
					_healthStat.OnReachedMinValue -= SetPlayerPermanentStunState;
				}
				if (_hiddenStaminaStat != null)
				{
					_hiddenStaminaStat.OnFullValueChanged -= TryStunPlayerByHiddenStamina;
				}
				_healthStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.Health);
				_healthStat.OnReachedMinValue += SetPlayerPermanentStunState;
				_hiddenStaminaStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.HiddenStamina);
				_hiddenStaminaStat.OnFullValueChanged += TryStunPlayerByHiddenStamina;
			}
		}

		private void SetPlayerPermanentStunState()
		{
			_playerStateService.ChangePlayerState(PlayerState.PreDeadCrouch);
		}

		private IEnumerator StartCheckForVelocityDelayed()
		{
			yield return new WaitForFixedUpdate();
			_isVelocityCheckEnabled = true;
		}

		private void TryStunPlayerByHiddenStamina(float staminaValue)
		{
			if (_hiddenStaminaStat.MaxValue != 0f && !IsLocalRagdollSimulated() && staminaValue <= 0f)
			{
				AddStunReasonTemporary(_playerStatesConfiguration.StaminaStunTemporalDuration);
			}
		}

		private void AddStunReasonTemporary(float duration)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				CancelStunTimer();
				ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.Stun);
				_stunCts = new CancellationTokenSource();
				Debug.Log("Ragdoller!");
				WaitAndRemoveStunReason(duration, ragdoll, _stunCts.Token).Forget();
			}
		}

		private async UniTaskVoid WaitAndRemoveStunReason(float duration, PlayerRagdollEntity ragdoll, CancellationToken ct, bool withRagdoll = true)
		{
			try
			{
				await UniTask.WaitForSeconds(duration, ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
				_playerMovableModel.NetworkedAnimator.SetBool(IsTemporalStunInProgressHash, boolValue: false);
				_playerMovableModel.NetworkedAnimator.SetBool(IsStunFromThrowHash, boolValue: false);
				if (withRagdoll)
				{
					ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.Stun);
				}
			}
			catch (OperationCanceledException)
			{
				_playerMovableModel.NetworkedAnimator.SetBool(IsTemporalStunInProgressHash, boolValue: false);
				_playerMovableModel.NetworkedAnimator.SetBool(IsStunFromThrowHash, boolValue: false);
			}
		}

		private void CancelStunTimer()
		{
			_stunCts?.Cancel();
			_stunCts?.Dispose();
			_stunCts = null;
		}

		private bool IsLocalRagdollSimulated()
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				return ragdoll.IsSimulated;
			}
			return false;
		}
	}
}
