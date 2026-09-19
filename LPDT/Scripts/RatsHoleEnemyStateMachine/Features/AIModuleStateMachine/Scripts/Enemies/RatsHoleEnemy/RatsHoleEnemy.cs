using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.AliveEnemyCount;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Core.Settings;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.EnemyFearingModule.Scripts;
using Features.LevelGatesModule.Data;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class RatsHoleEnemy : EnemyBase<RatsHoleEnemyStateId, RatsHoleEnemyEvent>, IEnemyFearCallbackListener
	{
		private EnemyFearListenerModel _enemyFearListenerModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private PlayersGatesModelSynchronizedModel _playersGatesModel;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private INavigationService _navigationService;

		private RatsHoleEnemyTargetReacquireService _targetReacquireService;

		private RatsHoleEnemyHoleDestinationService _holeDestinationService;

		private RatsHoleEnemyHostMigrationRecovery _hostMigrationRecovery;

		private AliveEnemyCountModel _aliveEnemyCountModel;

		private FearHoleAbsorbAnimationSettings _fearHoleAbsorbAnimationSettings;

		private IFearHoleAbsorbVisualController _fearHoleAbsorbVisualController;

		private EnemyRotator _bodyRotator;

		private bool _aliveCountRegistered;

		private bool _fearing;

		private bool _escaping;

		public override EnemyType EnemyType => EnemyType.RatsHoleEnemy;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		[Inject]
		public void InjectDependencies(RatsHoleEnemyContext context, IInstantiator instantiator, EnemyFearListenerModel enemyFearListenerModel, SpawnedPlayersModel spawnedPlayersModel, PlayersGatesModelSynchronizedModel playersGatesModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, INavigationService navigationService, IReadyHoleLocator readyHoleLocator, FearHoleAbsorbAnimationSettings fearHoleAbsorbAnimationSettings, IFearHoleAbsorbVisualController fearHoleAbsorbVisualController, AliveEnemyCountModel aliveEnemyCountModel)
		{
			InjectBaseDependencies(context, instantiator);
			_enemyFearListenerModel = enemyFearListenerModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playersGatesModel = playersGatesModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_navigationService = navigationService;
			_targetReacquireService = new RatsHoleEnemyTargetReacquireService(spawnedPlayersModel, playersGatesModel, enemyPlayerAttackabilityService, navigationService);
			_holeDestinationService = new RatsHoleEnemyHoleDestinationService(readyHoleLocator);
			_hostMigrationRecovery = new RatsHoleEnemyHostMigrationRecovery();
			_fearHoleAbsorbAnimationSettings = fearHoleAbsorbAnimationSettings;
			_fearHoleAbsorbVisualController = fearHoleAbsorbVisualController;
			_aliveEnemyCountModel = aliveEnemyCountModel;
		}

		protected override StateMachine<RatsHoleEnemyStateId, RatsHoleEnemyStateId, RatsHoleEnemyEvent> BuildFsm()
		{
			StateMachine<RatsHoleEnemyStateId, RatsHoleEnemyStateId, RatsHoleEnemyEvent> stateMachine = new StateMachine<RatsHoleEnemyStateId, RatsHoleEnemyStateId, RatsHoleEnemyEvent>();
			stateMachine.AddState(RatsHoleEnemyStateId.Idle, Instantiator.Instantiate<RatsHoleEnemyIdleState>());
			stateMachine.AddState(RatsHoleEnemyStateId.Chase, Instantiator.Instantiate<RatsHoleEnemyChaseState>());
			stateMachine.AddState(RatsHoleEnemyStateId.Attack, Instantiator.Instantiate<RatsHoleEnemyAttackState>());
			stateMachine.AddState(RatsHoleEnemyStateId.Fear, Instantiator.Instantiate<RatsHoleEnemyFearState>());
			stateMachine.AddState(RatsHoleEnemyStateId.ReturnToHoleDespawn, Instantiator.Instantiate<RatsHoleEnemyReturnToHoleDespawnState>());
			stateMachine.AddState(RatsHoleEnemyStateId.Patrol, Instantiator.Instantiate<RatsHoleEnemyPatrolState>());
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnTargetAcquired, RatsHoleEnemyStateId.Idle, RatsHoleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnInAttackRange, RatsHoleEnemyStateId.Idle, RatsHoleEnemyStateId.Attack);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnInAttackRange, RatsHoleEnemyStateId.Chase, RatsHoleEnemyStateId.Attack);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnOutOfAttackRange, RatsHoleEnemyStateId.Attack, RatsHoleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnTargetAcquired, RatsHoleEnemyStateId.Attack, RatsHoleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnTargetAcquired, RatsHoleEnemyStateId.ReturnToHoleDespawn, RatsHoleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnInAttackRange, RatsHoleEnemyStateId.ReturnToHoleDespawn, RatsHoleEnemyStateId.Attack);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnTargetAcquired, RatsHoleEnemyStateId.Patrol, RatsHoleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnInAttackRange, RatsHoleEnemyStateId.Patrol, RatsHoleEnemyStateId.Attack);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnPatrolTimeout, RatsHoleEnemyStateId.Patrol, RatsHoleEnemyStateId.ReturnToHoleDespawn);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnNoMigrationTargetFound, RatsHoleEnemyStateId.Idle, RatsHoleEnemyStateId.ReturnToHoleDespawn);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnNoTargetForPatrol, RatsHoleEnemyStateId.Idle, RatsHoleEnemyStateId.Patrol);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnNoTargetForPatrol, RatsHoleEnemyStateId.Chase, RatsHoleEnemyStateId.Patrol);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnNoTargetForPatrol, RatsHoleEnemyStateId.Attack, RatsHoleEnemyStateId.Patrol);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnDamaged, RatsHoleEnemyStateId.Idle, RatsHoleEnemyStateId.Chase);
			stateMachine.AddTriggerTransition(RatsHoleEnemyEvent.OnDamaged, RatsHoleEnemyStateId.Patrol, RatsHoleEnemyStateId.Chase);
			stateMachine.AddTriggerTransitionFromAny(RatsHoleEnemyEvent.OnFear, RatsHoleEnemyStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(RatsHoleEnemyStateId.Idle);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			_fearHoleAbsorbVisualController?.ResetVisual();
			if (base.HasStateAuthority && Context is RatsHoleEnemyContext ratsHoleEnemyContext && !ratsHoleEnemyContext.HasHomePosition)
			{
				ratsHoleEnemyContext.SetHomePosition(base.transform.position);
			}
			_bodyRotator = GetComponentInChildren<EnemyRotator>(includeInactive: true);
			FearCompleted = false;
			_fearing = false;
			_escaping = false;
			_enemyFearListenerModel.RegisterFearListener(EnemyType, EnemyInstants, this);
			RegisterAliveCount();
			if (base.HasStateAuthority && !TryAggroNearestPlayer())
			{
				TriggerEvent(RatsHoleEnemyEvent.OnNoTargetForPatrol);
			}
		}

		public override void StateAuthorityChanged()
		{
			base.StateAuthorityChanged();
			if (!base.HasStateAuthority)
			{
				return;
			}
			_fearing = false;
			_escaping = false;
			_hostMigrationRecovery.Begin(base.Runner);
			if (Context is RatsHoleEnemyContext ratsHoleEnemyContext)
			{
				if (!ratsHoleEnemyContext.HasHomePosition)
				{
					ratsHoleEnemyContext.SetHomePosition(base.transform.position);
				}
				ratsHoleEnemyContext.SetPriorityPlayer(null);
				ratsHoleEnemyContext.SetIsAttackPerforming(value: false);
				ratsHoleEnemyContext.SetSmoothedVelocity(0f);
			}
			if (TryAggroNearestPlayer())
			{
				_hostMigrationRecovery.Complete();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_fearHoleAbsorbVisualController?.Kill(resetScale: false);
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType, EnemyInstants);
			UnregisterAliveCount();
			base.Despawned(runner, hasState);
		}

		private void RegisterAliveCount()
		{
			if (!_aliveCountRegistered)
			{
				_aliveEnemyCountModel.Register(EnemyType);
				_aliveCountRegistered = true;
			}
		}

		private void UnregisterAliveCount()
		{
			if (_aliveCountRegistered)
			{
				_aliveEnemyCountModel.Unregister(EnemyType);
				_aliveCountRegistered = false;
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fearing && !_escaping)
			{
				_escaping = true;
				TriggerEvent(RatsHoleEnemyEvent.OnFear);
			}
			base.FixedUpdateNetwork();
			if (base.HasStateAuthority)
			{
				UpdateBodyRotationTowardPriorityPlayer();
			}
		}

		public void SnapBodyRotationTowardPriorityPlayer()
		{
			UpdateBodyRotationTowardPriorityPlayer(snap: true);
		}

		private void UpdateBodyRotationTowardPriorityPlayer()
		{
			UpdateBodyRotationTowardPriorityPlayer(snap: false);
		}

		private void UpdateBodyRotationTowardPriorityPlayer(bool snap)
		{
			if (!(_bodyRotator == null) && Context is RatsHoleEnemyContext { VisualState: RatsHoleEnemyVisualState.Attack, PriorityPlayer: var priorityPlayer } && !(priorityPlayer?.NetworkObject == null))
			{
				Vector3 direction = priorityPlayer.NetworkObject.transform.position - base.transform.position;
				direction.y = 0f;
				if (snap)
				{
					_bodyRotator.SnapTowardsDirection(direction);
				}
				else
				{
					_bodyRotator.RotateTowardsDirection(direction);
				}
			}
		}

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				_fearing = true;
			}
		}

		public void MarkFearCompleted()
		{
			FearCompleted = true;
			_fearing = false;
			_escaping = false;
		}

		public void RequestDespawn()
		{
			if (base.HasStateAuthority && (!TryGetComponent<EnemyDeathDissolveEffect>(out var component) || !component.TryStartDeferredDespawn(EnemyDissolveReason.Despawn)))
			{
				base.Object.DespawnHierarchy();
			}
		}

		public bool IsNearHoleAbsorbStart()
		{
			if (!(Context is RatsHoleEnemyContext ratsHoleEnemyContext))
			{
				return false;
			}
			float num = ((_fearHoleAbsorbAnimationSettings != null) ? Mathf.Max(0f, _fearHoleAbsorbAnimationSettings.StartDistance) : 0f);
			float num2 = ((ratsHoleEnemyContext.NavMeshAgent != null) ? ratsHoleEnemyContext.NavMeshAgent.stoppingDistance : 0f);
			return Vector3.Distance(base.transform.position, ratsHoleEnemyContext.TargetPosition) <= num + num2;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1102443660u)]
		public void PlayHoleAbsorbVisualRpc([RpcPayload(4)] float duration, [RpcPayload(4)] float endScale, [RpcPayload(4)] int ease)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1102443660u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.RatsHoleEnemy::PlayHoleAbsorbVisualRpc(System.Single,System.Single,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(duration, 4);
						writer.Write(endScale, 4);
						writer.Write(ease, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_fearHoleAbsorbVisualController?.Play(duration, endScale, ease);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3412459069u)]
		public void PlayHoleAbsorbVisualToTargetRpc([RpcPayload(4)] float duration, [RpcPayload(4)] float endScale, [RpcPayload(4)] int ease, [RpcPayload(12)] Vector3 worldTarget)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3412459069u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.RatsHoleEnemy::PlayHoleAbsorbVisualToTargetRpc(System.Single,System.Single,System.Int32,UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(duration, 4);
						writer.Write(endScale, 4);
						writer.Write(ease, 4);
						writer.Write(worldTarget, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_fearHoleAbsorbVisualController?.Play(duration, endScale, ease, worldTarget);
		}

		public bool TryPrepareNearestHoleAbsorbDestination()
		{
			if (!(Context is RatsHoleEnemyContext context))
			{
				return false;
			}
			return _holeDestinationService.TryPrepareNearestHoleDestination(this, context);
		}

		public bool TryPrepareHomeThenNearestHoleAbsorbDestination()
		{
			if (!(Context is RatsHoleEnemyContext context))
			{
				return false;
			}
			return _holeDestinationService.TryPrepareHomeThenNearestHoleDestination(this, context);
		}

		public bool IsIdleDespawnBlockedAfterHostMigration()
		{
			return _hostMigrationRecovery.IsIdleDespawnBlocked(base.Runner);
		}

		public bool UpdateHostMigrationRecovery()
		{
			if (!_hostMigrationRecovery.IsRecovering)
			{
				return false;
			}
			if (TryAggroNearestPlayer())
			{
				_hostMigrationRecovery.Complete();
				return true;
			}
			if (_hostMigrationRecovery.ShouldFallbackToHole(base.Runner))
			{
				TriggerEvent(RatsHoleEnemyEvent.OnNoMigrationTargetFound);
				return true;
			}
			return false;
		}

		public bool IsPriorityPlayerOutsideHomeAggro()
		{
			if (!TryGetPriorityPlayerTrackingPosition(out var position))
			{
				return true;
			}
			return IsOutsideHomeAggro(position);
		}

		public bool IsOutsideHomeAggro(Vector3 playerPosition)
		{
			if (!(Context is RatsHoleEnemyContext ratsHoleEnemyContext) || !ratsHoleEnemyContext.HasHomePosition)
			{
				return false;
			}
			return GetHorizontalDistance(ratsHoleEnemyContext.HomePosition, playerPosition) > ratsHoleEnemyContext.AggroRange;
		}

		public float GetHorizontalDistanceFromHome(Vector3 playerPosition)
		{
			if (!(Context is RatsHoleEnemyContext ratsHoleEnemyContext) || !ratsHoleEnemyContext.HasHomePosition)
			{
				return float.MaxValue;
			}
			return GetHorizontalDistance(ratsHoleEnemyContext.HomePosition, playerPosition);
		}

		private float GetHorizontalDistance(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			return new Vector2(vector.x, vector.z).magnitude;
		}

		public bool IsPriorityPlayerInSafeZone()
		{
			if (!(Context is RatsHoleEnemyContext ratsHoleEnemyContext))
			{
				return false;
			}
			return _targetReacquireService.IsPlayerInSafeZone(this, ratsHoleEnemyContext, ratsHoleEnemyContext.PriorityPlayer);
		}

		public bool IsPriorityPlayerOutsideGate()
		{
			if (!(Context is RatsHoleEnemyContext { PriorityPlayer: var priorityPlayer }))
			{
				return false;
			}
			if (priorityPlayer?.NetworkObject == null)
			{
				return false;
			}
			int playerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
			if (_playersGatesModel != null && _playersGatesModel.TryGetPlayerState(playerId, out var state))
			{
				return !state.PlayerInsideGate;
			}
			return false;
		}

		public bool CanInteractWithPriorityPlayer()
		{
			if (!(Context is RatsHoleEnemyContext { PriorityPlayer: var priorityPlayer }))
			{
				return false;
			}
			if (priorityPlayer?.NetworkObject == null)
			{
				return false;
			}
			int playerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
			if (!IsPriorityPlayerOutsideGate())
			{
				return _enemyPlayerAttackabilityService.CanEnemyAttackPlayer(playerId);
			}
			return false;
		}

		public bool TryGetPriorityPlayerTrackingPosition(out Vector3 position)
		{
			position = default(Vector3);
			if (!(Context is RatsHoleEnemyContext { PriorityPlayer: var priorityPlayer }))
			{
				return false;
			}
			if (priorityPlayer?.NetworkObject == null)
			{
				return false;
			}
			PlayerRef inputAuthority = priorityPlayer.NetworkObject.InputAuthority;
			if (_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out position))
			{
				return true;
			}
			position = priorityPlayer.NetworkObject.transform.position;
			return true;
		}

		public bool IsPriorityPlayerReachable()
		{
			if (!(Context is RatsHoleEnemyContext { PriorityPlayer: var priorityPlayer } ratsHoleEnemyContext))
			{
				return false;
			}
			if (priorityPlayer?.NetworkObject == null || ratsHoleEnemyContext.NavMeshAgent == null)
			{
				return false;
			}
			PlayerReachableData reachableData;
			return _navigationService.IsPlayerOnReachablePoint(priorityPlayer.NetworkObject.InputAuthority, ratsHoleEnemyContext.NavMeshAgent, ratsHoleEnemyContext.AggroRange, out reachableData);
		}

		public void LosePriorityTarget()
		{
			PlayerDataHolder ignoredPlayer = null;
			if (Context is RatsHoleEnemyContext ratsHoleEnemyContext)
			{
				ignoredPlayer = ratsHoleEnemyContext.PriorityPlayer;
				ratsHoleEnemyContext.SetPriorityPlayer(null);
			}
			if (!TryAggroNearestPlayer(ignoredPlayer))
			{
				TriggerEvent(RatsHoleEnemyEvent.OnNoTargetForPatrol);
			}
		}

		public void LoseSafeZonePriorityTargetToPatrolFallback()
		{
			PlayerDataHolder ignoredPlayer = null;
			if (Context is RatsHoleEnemyContext ratsHoleEnemyContext)
			{
				ignoredPlayer = ratsHoleEnemyContext.PriorityPlayer;
				ratsHoleEnemyContext.SetPriorityPlayer(null);
			}
			if (!TryAggroNearestPlayer(ignoredPlayer))
			{
				TriggerEvent(RatsHoleEnemyEvent.OnNoTargetForPatrol);
			}
		}

		public bool TryReacquireAvailableTarget()
		{
			return TryAggroNearestPlayer();
		}

		public void StartNoTargetPatrol()
		{
			if (Context is RatsHoleEnemyContext ratsHoleEnemyContext)
			{
				ratsHoleEnemyContext.SetPriorityPlayer(null);
			}
			TriggerEvent(RatsHoleEnemyEvent.OnNoTargetForPatrol);
		}

		private bool TryAggroNearestPlayer(PlayerDataHolder ignoredPlayer = null)
		{
			if (!(Context is RatsHoleEnemyContext ratsHoleEnemyContext))
			{
				return false;
			}
			if (_targetReacquireService.TryFindNearest(this, ratsHoleEnemyContext, ratsHoleEnemyContext.DistanceToAttack, fromHome: false, ignoredPlayer, out var player))
			{
				ratsHoleEnemyContext.SetPriorityPlayer(player);
				TriggerEvent(RatsHoleEnemyEvent.OnInAttackRange);
				return true;
			}
			if (!_targetReacquireService.TryFindNearest(this, ratsHoleEnemyContext, ratsHoleEnemyContext.AggroRange, fromHome: true, ignoredPlayer, out var player2))
			{
				return false;
			}
			ratsHoleEnemyContext.SetPriorityPlayer(player2);
			TriggerEvent(RatsHoleEnemyEvent.OnTargetAcquired);
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}

		[NetworkRpcWeavedInvoker(1102443660u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHoleAbsorbVisualRpc_0040Invoker1102443660([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out float value2, 4);
			payloadReader.Read(out int value3, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RatsHoleEnemy)context.TargetBehaviour).PlayHoleAbsorbVisualRpc(value, value2, value3);
		}

		[NetworkRpcWeavedInvoker(3412459069u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHoleAbsorbVisualToTargetRpc_0040Invoker3412459069([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out float value2, 4);
			payloadReader.Read(out int value3, 4);
			payloadReader.Read(out Vector3 value4, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RatsHoleEnemy)context.TargetBehaviour).PlayHoleAbsorbVisualToTargetRpc(value, value2, value3, value4);
		}
	}
}
