using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.SnakeModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeBodyGrabAggroSystem : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		private const float DEFAULT_GRAB_AGGRO_PIN_DURATION = 6f;

		private const int NoPendingPlayerId = -1;

		[SerializeField]
		private SnakeController _snakeController;

		[Tooltip("Debug: skip grab aggro + force-UnJoin (OnBodyPartGrabbed no-ops).")]
		[SerializeField]
		private bool _debugDisableGrabAggro;

		[Tooltip("Seconds to keep the grab before UnJoin, then enter StepAggro. 0 = UnJoin + chase same frame.")]
		[SerializeField]
		private float _forceReleaseDelay = 0.35f;

		[Tooltip("Seconds the grabbing player stays pinned as the target after chase starts. Needed because the player may be outside the snake's detection — a long-arm grab from range — and detection would otherwise clear the target on its next update. 0 = not set, falls back to the default.")]
		[SerializeField]
		private float _grabAggroPinDuration = 6f;

		private SnakeEnemy _enemy;

		private SnakeEnemyContext _context;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private LineArmsModel _lineArmsModel;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private bool _areGrabCallbacksBound;

		private bool _hasPendingGrabAggro;

		private float _pendingGrabAggroAt;

		private int _pendingAggroPlayerId = -1;

		[Inject]
		public void InjectDependencies(SnakeEnemy enemy, SnakeEnemyContext context, SpawnedPlayersModel spawnedPlayersModel, LineArmsModel lineArmsModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_enemy = enemy;
			_context = context;
			_spawnedPlayersModel = spawnedPlayersModel;
			_lineArmsModel = lineArmsModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public override void Spawned()
		{
			base.Spawned();
			TryBindGrabCallbacks();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ClearPendingGrabAggro();
			UnbindGrabCallbacks();
			base.Despawned(runner, hasState);
		}

		public void StateAuthorityChanged()
		{
			if (!base.HasStateAuthority)
			{
				ClearPendingGrabAggro();
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				_context.TickPriorityPin(base.Runner.DeltaTime);
				if (_hasPendingGrabAggro && !(base.Runner == null) && !(base.Runner.SimulationTime < _pendingGrabAggroAt))
				{
					CompleteGrabAggroAfterHold();
				}
			}
		}

		private float ResolvePinDuration()
		{
			if (!(_grabAggroPinDuration > 0f))
			{
				return 6f;
			}
			return _grabAggroPinDuration;
		}

		private void Update()
		{
			if (!_areGrabCallbacksBound)
			{
				TryBindGrabCallbacks();
			}
		}

		private void TryBindGrabCallbacks()
		{
			if (_areGrabCallbacksBound || _snakeController == null)
			{
				return;
			}
			IReadOnlyList<SimplePointGrabable> bodyPartGrabables = _snakeController.BodyPartGrabables;
			if (bodyPartGrabables == null || bodyPartGrabables.Count == 0)
			{
				return;
			}
			for (int i = 0; i < bodyPartGrabables.Count; i++)
			{
				SimplePointGrabable simplePointGrabable = bodyPartGrabables[i];
				if (!(simplePointGrabable == null))
				{
					simplePointGrabable.LocalOnGrab += OnBodyPartGrabbed;
				}
			}
			_areGrabCallbacksBound = true;
		}

		private void UnbindGrabCallbacks()
		{
			if (!_areGrabCallbacksBound || _snakeController == null)
			{
				return;
			}
			IReadOnlyList<SimplePointGrabable> bodyPartGrabables = _snakeController.BodyPartGrabables;
			if (bodyPartGrabables != null)
			{
				for (int i = 0; i < bodyPartGrabables.Count; i++)
				{
					SimplePointGrabable simplePointGrabable = bodyPartGrabables[i];
					if (!(simplePointGrabable == null))
					{
						simplePointGrabable.LocalOnGrab -= OnBodyPartGrabbed;
					}
				}
			}
			_areGrabCallbacksBound = false;
		}

		private void OnBodyPartGrabbed(int playerId)
		{
			if (!_debugDisableGrabAggro && base.HasStateAuthority && !(_enemy == null) && !(_context == null) && _enemy.CurrentStateId != SnakeStateId.Fear && _enemy.CurrentStateId != SnakeStateId.Wrap && !_context.IsWrapOrbitActive && _enemy.CurrentStateId != SnakeStateId.SafeZoneApproach && TryResolvePlayer(playerId, out var _) && _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(playerId) && _context.CanTargetPlayerForWrap(playerId) && !_context.IsPlayerOutsideGate(playerId))
			{
				_context.ClearWrapEscapeCooldown();
				ScheduleGrabAggro(playerId);
			}
		}

		private void ScheduleGrabAggro(int playerId)
		{
			_pendingAggroPlayerId = playerId;
			if (_forceReleaseDelay <= 0f || base.Runner == null)
			{
				CompleteGrabAggroAfterHold();
				return;
			}
			_hasPendingGrabAggro = true;
			_pendingGrabAggroAt = base.Runner.SimulationTime + _forceReleaseDelay;
		}

		private void CompleteGrabAggroAfterHold()
		{
			int pendingAggroPlayerId = _pendingAggroPlayerId;
			ClearPendingGrabAggro();
			ForceReleaseBodyGrabsRpc();
			if (!(_enemy == null) && !(_context == null) && _enemy.CurrentStateId != SnakeStateId.Fear && _enemy.CurrentStateId != SnakeStateId.Wrap && !_context.IsWrapOrbitActive && _enemy.CurrentStateId != SnakeStateId.SafeZoneApproach && TryResolvePlayer(pendingAggroPlayerId, out var player) && _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(pendingAggroPlayerId) && _context.CanTargetPlayerForWrap(pendingAggroPlayerId) && !_context.IsPlayerOutsideGate(pendingAggroPlayerId))
			{
				_context.PinPriorityPlayer(player, ResolvePinDuration());
				_context.ApplyStepAggroMoveSpeed();
				_enemy.TriggerEvent(SnakeEvent.OnSteppedOn);
			}
		}

		private void ClearPendingGrabAggro()
		{
			_hasPendingGrabAggro = false;
			_pendingGrabAggroAt = 0f;
			_pendingAggroPlayerId = -1;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2527590666u)]
		private void ForceReleaseBodyGrabsRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2527590666u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems.SnakeBodyGrabAggroSystem::ForceReleaseBodyGrabsRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ReleaseLocalBodyGrabs();
		}

		private void ReleaseLocalBodyGrabs()
		{
			if (_snakeController == null || _lineArmsModel == null || base.Runner == null)
			{
				return;
			}
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(base.Runner.LocalPlayer.PlayerId);
			if (allLineArmsForPlayer == null)
			{
				return;
			}
			foreach (LineArmControllerBase value in allLineArmsForPlayer.Values)
			{
				if (!(value == null))
				{
					value.UnJoinAllMatching(IsSnakeBodyGrabable);
				}
			}
		}

		private bool IsSnakeBodyGrabable(IPointGrabable grabable)
		{
			if (_snakeController != null)
			{
				return _snakeController.IsBodyPartGrabable(grabable);
			}
			return false;
		}

		private bool TryResolvePlayer(int playerId, out PlayerDataHolder player)
		{
			player = null;
			if (_spawnedPlayersModel == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player2 in _spawnedPlayersModel.Players)
			{
				if (player2.Key.PlayerId == playerId)
				{
					PlayerDataHolder value = player2.Value;
					if (value != null && !(value.NetworkObject == null))
					{
						player = value;
						return true;
					}
				}
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2527590666u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ForceReleaseBodyGrabsRpc_0040Invoker2527590666([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SnakeBodyGrabAggroSystem)context.TargetBehaviour).ForceReleaseBodyGrabsRpc();
		}
	}
}
