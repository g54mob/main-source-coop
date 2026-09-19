using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Enemy;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems;
using Features.EnemyFearingModule.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeEnemy : EnemyBase<SnakeStateId, SnakeEvent>, IEnemyFearCallbackListener
	{
		private const int FearFleePositionAttempts = 20;

		private const float FearFleeMinDistanceFromCenterFactor = 0.35f;

		private const float FearFleeAverageAvoidDistanceWeight = 5f;

		private const float FearFleeTooClosePenaltyRadius = 3f;

		private const float FearFleeTooClosePenaltyWeight = 10f;

		[SerializeField]
		private SnakeWrapStruggleSystem _wrapStruggleSystem;

		[SerializeField]
		private SnakeAudioController _audioController;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private INavigationService _navigationService;

		private readonly List<Vector3> _fearAvoidPositions = new List<Vector3>();

		private bool _fearing;

		private bool _escaping;

		public override EnemyType EnemyType => EnemyType.Snake;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public bool IsDespawnAfterFear { get; set; } = true;

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		[Inject]
		public void InjectDependencies(SnakeEnemyContext context, IInstantiator instantiator, EnemyFearListenerModel enemyFearListenerModel, SpawnedPlayersModel spawnedPlayersModel, INavigationService navigationService)
		{
			InjectBaseDependencies(context, instantiator);
			_enemyFearListenerModel = enemyFearListenerModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_navigationService = navigationService;
		}

		protected override StateMachine<SnakeStateId, SnakeStateId, SnakeEvent> BuildFsm()
		{
			StateMachine<SnakeStateId, SnakeStateId, SnakeEvent> stateMachine = new StateMachine<SnakeStateId, SnakeStateId, SnakeEvent>();
			stateMachine.AddState(SnakeStateId.Idle, Instantiator.Instantiate<SnakeIdleState>());
			stateMachine.AddState(SnakeStateId.Fear, Instantiator.Instantiate<SnakeFearState>());
			stateMachine.AddState(SnakeStateId.StepAggro, Instantiator.Instantiate<SnakeStepAggroState>());
			stateMachine.AddState(SnakeStateId.Wrap, Instantiator.Instantiate<SnakeWrapState>());
			stateMachine.AddState(SnakeStateId.SafeZoneApproach, Instantiator.Instantiate<SnakeSafeZoneApproachState>());
			stateMachine.AddTriggerTransition(SnakeEvent.OnTargetAcquired, SnakeStateId.Idle, SnakeStateId.StepAggro);
			stateMachine.AddTriggerTransitionFromAny(SnakeEvent.OnDamaged, SnakeStateId.StepAggro, (Transition<SnakeStateId> transition) => base.CurrentStateId != SnakeStateId.Fear && base.CurrentStateId != SnakeStateId.Wrap && base.CurrentStateId != SnakeStateId.SafeZoneApproach, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransitionFromAny(SnakeEvent.OnSteppedOn, SnakeStateId.StepAggro, (Transition<SnakeStateId> transition) => base.CurrentStateId != SnakeStateId.Fear && base.CurrentStateId != SnakeStateId.Wrap && base.CurrentStateId != SnakeStateId.SafeZoneApproach, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SnakeEvent.OnStepAggroDisengaged, SnakeStateId.StepAggro, SnakeStateId.Idle, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SnakeEvent.OnReachedWrapTarget, SnakeStateId.StepAggro, SnakeStateId.Wrap, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SnakeEvent.OnSafeZoneApproach, SnakeStateId.StepAggro, SnakeStateId.SafeZoneApproach, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SnakeEvent.OnSafeZoneApproachFinished, SnakeStateId.SafeZoneApproach, SnakeStateId.Idle, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SnakeEvent.OnWrapFinished, SnakeStateId.Wrap, SnakeStateId.Idle, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransitionFromAny(SnakeEvent.OnFear, SnakeStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(SnakeEvent.OnFearEscapeCompleted, SnakeStateId.Fear, SnakeStateId.Idle, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(SnakeStateId.Idle);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_fearing = false;
			_escaping = false;
			_enemyFearListenerModel.RegisterFearListener(EnemyType, EnemyInstants, this);
		}

		public override void StateAuthorityChanged()
		{
			base.StateAuthorityChanged();
			if (base.HasStateAuthority)
			{
				_wrapStruggleSystem.ForceAbortOrphanedWrap();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType, EnemyInstants);
			if (Context is SnakeEnemyContext snakeEnemyContext)
			{
				snakeEnemyContext.ReleaseSafeZoneApproachPoint();
			}
			if (_audioController != null)
			{
				_audioController.StopAllImmediate();
			}
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fearing && !_escaping)
			{
				_escaping = true;
				TriggerEvent(SnakeEvent.OnFear);
			}
			base.FixedUpdateNetwork();
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
			if (base.HasStateAuthority)
			{
				base.Object.DespawnHierarchy();
			}
		}

		public void PlayUnderTableAttackSound()
		{
			if (base.HasStateAuthority && !(base.Object == null) && base.Object.IsValid)
			{
				RpcPlayUnderTableAttackSound();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true, Key = 1026483187u)]
		private void RpcPlayUnderTableAttackSound()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1026483187u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.SnakeEnemy::RpcPlayUnderTableAttackSound()", invokeInfo, PlayerRef.None);
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
			if (_audioController != null)
			{
				_audioController.PlayUnderTableAttack();
			}
		}

		public float GetTickDelta()
		{
			if (!(base.Runner != null))
			{
				return Time.fixedDeltaTime;
			}
			return base.Runner.DeltaTime;
		}

		public bool TryPrepareFearFleeDestination()
		{
			if (!(Context is SnakeEnemyContext { NavMeshAgent: var navMeshAgent } snakeEnemyContext))
			{
				return false;
			}
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				return false;
			}
			float fearFleeRadius = snakeEnemyContext.FearFleeRadius;
			CollectFearAvoidPositions();
			Vector3 bestPosition = default(Vector3);
			bool flag = false;
			if (_fearAvoidPositions.Count > 0 && _navigationService.TryGetRandomSafeNavmeshPosition(base.transform.position, fearFleeRadius, 20, _fearAvoidPositions, navMeshAgent.agentTypeID, -1, out bestPosition, fearFleeRadius * 0.35f, 1f, 5f, 3f, 10f) && HasCompletePath(navMeshAgent, bestPosition))
			{
				flag = true;
			}
			if (!flag && TryGetOppositeFearFleePosition(fearFleeRadius, out bestPosition) && HasCompletePath(navMeshAgent, bestPosition))
			{
				flag = true;
			}
			if (!flag && _navigationService.TryGetRandomNavmeshPosition(base.transform.position, fearFleeRadius, out bestPosition) && HasCompletePath(navMeshAgent, bestPosition))
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			snakeEnemyContext.SetTargetPosition(bestPosition);
			snakeEnemyContext.SetTargetPositionCompleted(isCompleted: false);
			snakeEnemyContext.NeedToFindTargetPosition = false;
			navMeshAgent.stoppingDistance = 0f;
			navMeshAgent.isStopped = false;
			navMeshAgent.ResetPath();
			navMeshAgent.SetDestination(bestPosition);
			return true;
		}

		private void CollectFearAvoidPositions()
		{
			_fearAvoidPositions.Clear();
			if (_spawnedPlayersModel?.Players == null)
			{
				return;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				TryAddFearAvoidPosition(player.Value);
			}
		}

		private void TryAddFearAvoidPosition(PlayerDataHolder holder)
		{
			if (!(holder?.NetworkObject == null) && holder.NetworkObject.IsValid)
			{
				PlayerRef inputAuthority = holder.NetworkObject.InputAuthority;
				if (_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
				{
					_fearAvoidPositions.Add(position);
				}
				else
				{
					_fearAvoidPositions.Add(holder.NetworkObject.transform.position);
				}
			}
		}

		private bool TryGetOppositeFearFleePosition(float fleeRadius, out Vector3 fleeTarget)
		{
			fleeTarget = default(Vector3);
			if (_fearAvoidPositions.Count == 0)
			{
				return false;
			}
			Vector3 position = base.transform.position;
			Vector3 vector = _fearAvoidPositions[0];
			float num = GetHorizontalDistanceSqr(position, vector);
			for (int i = 1; i < _fearAvoidPositions.Count; i++)
			{
				float horizontalDistanceSqr = GetHorizontalDistanceSqr(position, _fearAvoidPositions[i]);
				if (!(horizontalDistanceSqr >= num))
				{
					num = horizontalDistanceSqr;
					vector = _fearAvoidPositions[i];
				}
			}
			Vector3 vector2 = position - vector;
			vector2.y = 0f;
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = UnityEngine.Random.insideUnitSphere;
			}
			vector2.y = 0f;
			vector2.Normalize();
			Vector3 point = position + vector2 * fleeRadius;
			return _navigationService.TryGetPointOnNavMeshProjected(point, fleeRadius, out fleeTarget);
		}

		private static bool HasCompletePath(NavMeshAgent agent, Vector3 destination)
		{
			if (agent == null || !agent.isOnNavMesh)
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!agent.CalculatePath(destination, navMeshPath))
			{
				return false;
			}
			return navMeshPath.status == NavMeshPathStatus.PathComplete;
		}

		private static float GetHorizontalDistanceSqr(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			return new Vector2(vector.x, vector.z).sqrMagnitude;
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

		[NetworkRpcWeavedInvoker(1026483187u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcPlayUnderTableAttackSound_0040Invoker1026483187([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SnakeEnemy)context.TargetBehaviour).RpcPlayUnderTableAttackSound();
		}
	}
}
