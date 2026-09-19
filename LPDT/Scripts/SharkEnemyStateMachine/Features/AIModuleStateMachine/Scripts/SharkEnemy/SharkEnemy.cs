using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.States;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.StateMachineDebug;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy
{
	[NetworkBehaviourWeaved(1)]
	public class SharkEnemy : NetworkBehaviour, IEnemyBehaviour, IEnemyTypeProvider, IEnemyFearCallbackListener, IStateAuthorityChanged, IPublicFacingInterface, IHfsmDebugSource<SharkStateId, SharkStateId, SharkEvent>, IHfsmDebugSource, IEnemyAttractionZoneCallbackListener
	{
		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SharkVisualState _VisualState;

		private StateMachine<SharkStateId, SharkStateId, SharkEvent> _fsm;

		private IInstantiator _instantiator;

		private SharkEnemyContext _context;

		private SharkMovementSettings _movementSettings;

		private SharkAttackSettings _attackSettings;

		private SharkWaterTargetSensor _sensor;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private PlayerDamageablesTrackModel _playerDamageables;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private EnemyAttractionZoneListenerModel _attractionZoneListenerModel;

		private SessionAnalyticsModel _sessionAnalyticsModel;

		private bool _pendingNavSnap;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe SharkVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SharkEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(SharkVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SharkEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(SharkVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => EnemyType.Shark;

		public int EnemyInstants => base.gameObject.GetInstanceID();

		public bool IsDespawnAfterFear
		{
			get
			{
				return _context.IsDespawnAfterFear;
			}
			set
			{
				_context.IsDespawnAfterFear = value;
			}
		}

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		public StateMachine<SharkStateId, SharkStateId, SharkEvent> StateMachineForDebug => _fsm;

		public event Action<IEnemyBehaviour> OnDeath;

		public event Action<Vector3> OnChangeAreaTriggered;

		[Inject]
		public void InjectDependencies(IInstantiator instantiator, SharkEnemyContext context, EnemyFearListenerModel enemyFearListenerModel, SharkMovementSettings movementSettings, SharkTargetingSettings targetingSettings, SharkAttackSettings attackSettings, SharkWaterTargetSensor sensor, PlayerDamageablesTrackModel playerDamageables, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, EnemyAttractionZoneListenerModel attractionZoneListenerModel, SessionAnalyticsModel sessionAnalyticsModel)
		{
			_instantiator = instantiator;
			_context = context;
			_attractionZoneListenerModel = attractionZoneListenerModel;
			_enemyFearListenerModel = enemyFearListenerModel;
			_movementSettings = movementSettings;
			_attackSettings = attackSettings;
			_sensor = sensor;
			_playerDamageables = playerDamageables;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_sessionAnalyticsModel = sessionAnalyticsModel;
		}

		private bool IsBackoffFinished()
		{
			return _context.BackoffElapsed >= _attackSettings.BackoffDuration;
		}

		private bool CanResumePlayerHuntAfterBackoff()
		{
			if (_context.TargetPlayer != PlayerRef.None)
			{
				return _sensor.CanResumePlayerHuntAfterBackoff(this, _context.TargetPlayer, _context.TargetObject);
			}
			return false;
		}

		private bool CanResumeMimicHuntAfterBackoff()
		{
			if (_context.TargetPlayer == PlayerRef.None)
			{
				return _sensor.CanResumeMimicHuntAfterBackoff(_context.TargetObject);
			}
			return false;
		}

		private bool CanResumeAnyHuntAfterBackoff()
		{
			if (!CanResumePlayerHuntAfterBackoff())
			{
				return CanResumeMimicHuntAfterBackoff();
			}
			return true;
		}

		private StateMachine<SharkStateId, SharkStateId, SharkEvent> BuildFsm()
		{
			StateMachine<SharkStateId, SharkStateId, SharkEvent> root = new StateMachine<SharkStateId, SharkStateId, SharkEvent>();
			root.AddState(SharkStateId.Idle, _instantiator.Instantiate<SharkIdleState>());
			root.AddState(SharkStateId.Hunt, _instantiator.Instantiate<SharkHuntState>());
			root.AddState(SharkStateId.AttackSimple, _instantiator.Instantiate<SharkAttackSimpleState>());
			root.AddState(SharkStateId.AttackLethal, _instantiator.Instantiate<SharkAttackLethalState>());
			root.AddState(SharkStateId.Backoff, _instantiator.Instantiate<SharkBackoffState>());
			root.AddState(SharkStateId.Fear, _instantiator.Instantiate<SharkFearState>());
			root.AddState(SharkStateId.MimicHunt, _instantiator.Instantiate<SharkMimicHuntState>());
			root.AddState(SharkStateId.AttractionInvestigate, _instantiator.Instantiate<SharkAttractionInvestigateState>());
			root.AddTriggerTransition(SharkEvent.OnTargetAcquired, SharkStateId.Idle, SharkStateId.Hunt);
			root.AddTriggerTransition(SharkEvent.OnMimicTargetAcquired, SharkStateId.Idle, SharkStateId.MimicHunt);
			root.AddTriggerTransition(SharkEvent.OnAttractionZoneEntered, SharkStateId.Idle, SharkStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnAttractionZoneExited, SharkStateId.AttractionInvestigate, SharkStateId.Idle, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnTargetAcquired, SharkStateId.AttractionInvestigate, SharkStateId.Hunt);
			root.AddTriggerTransition(SharkEvent.OnMimicTargetAcquired, SharkStateId.AttractionInvestigate, SharkStateId.MimicHunt);
			root.AddTriggerTransition(SharkEvent.OnFear, SharkStateId.AttractionInvestigate, SharkStateId.Fear, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnTargetLost, SharkStateId.Hunt, SharkStateId.Idle, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnTargetLost, SharkStateId.MimicHunt, SharkStateId.Idle, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnTargetLost, SharkStateId.AttackSimple, SharkStateId.Idle, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnTargetLost, SharkStateId.AttackLethal, SharkStateId.Idle, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnCommitSimpleAttack, SharkStateId.Hunt, SharkStateId.AttackSimple, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnCommitSimpleAttack, SharkStateId.MimicHunt, SharkStateId.AttackSimple, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnCommitLethalAttack, SharkStateId.Hunt, SharkStateId.AttackLethal, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnHuntWithdraw, SharkStateId.Hunt, SharkStateId.Backoff, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnAttackFinished, SharkStateId.AttackSimple, SharkStateId.Backoff, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnAttackFinished, SharkStateId.AttackLethal, SharkStateId.Backoff, null, null, null, forceInstantly: true);
			root.AddTransition(SharkStateId.Backoff, SharkStateId.Hunt, (Transition<SharkStateId> transition) => IsBackoffFinished() && CanResumePlayerHuntAfterBackoff());
			root.AddTransition(SharkStateId.Backoff, SharkStateId.MimicHunt, (Transition<SharkStateId> transition) => IsBackoffFinished() && CanResumeMimicHuntAfterBackoff());
			root.AddTransition(SharkStateId.Backoff, SharkStateId.Idle, (Transition<SharkStateId> transition) => IsBackoffFinished() && !CanResumeAnyHuntAfterBackoff());
			root.AddTriggerTransition(SharkEvent.OnFear, SharkStateId.Idle, SharkStateId.Fear, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnFear, SharkStateId.Hunt, SharkStateId.Fear, null, null, null, forceInstantly: true);
			root.AddTriggerTransition(SharkEvent.OnFear, SharkStateId.MimicHunt, SharkStateId.Fear, null, null, null, forceInstantly: true);
			root.AddTransition(SharkStateId.Fear, SharkStateId.Idle, (Transition<SharkStateId> transition) => !_context.IsFearing);
			AddFinDamagedToBackoff(SharkStateId.Idle);
			AddFinDamagedToBackoff(SharkStateId.Hunt);
			AddFinDamagedToBackoff(SharkStateId.MimicHunt);
			AddFinDamagedToBackoff(SharkStateId.AttackSimple);
			AddFinDamagedToBackoff(SharkStateId.AttackLethal);
			AddFinDamagedToBackoff(SharkStateId.Backoff);
			AddFinDamagedToBackoff(SharkStateId.AttractionInvestigate);
			root.SetStartState(SharkStateId.Idle);
			return root;
			void AddFinDamagedToBackoff(SharkStateId from)
			{
				root.AddTriggerTransition(SharkEvent.OnFinDamaged, from, SharkStateId.Backoff, null, null, null, forceInstantly: true);
			}
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority && _fsm == null)
			{
				_context.EnableNavAgentForStateAuthority(_movementSettings);
				_pendingNavSnap = !_context.TrySnapNavAgentOntoNavMesh(_movementSettings);
				_fsm = BuildFsm();
				_fsm.Init();
				if (_context.FinDamageable != null)
				{
					_context.FinDamageable.OnDamaged += OnFinDamagedHandler;
				}
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			_context.ValidateRequiredReferences();
			FearCompleted = false;
			_context.IsFearing = false;
			_attractionZoneListenerModel.RegisterListener(EnemyType, EnemyInstants, this);
			_context.InitNavAgent(this, _movementSettings);
			_context.CreateFmodInstances();
			if (base.HasStateAuthority)
			{
				_pendingNavSnap = !_context.TrySnapNavAgentOntoNavMesh(_movementSettings);
				_fsm = BuildFsm();
				_fsm.Init();
				_context.FinDamageable.OnDamaged += OnFinDamagedHandler;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			base.Despawned(runner, hasState);
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_context.IsFearing = false;
			_attractionZoneListenerModel.UnregisterListener(EnemyType, EnemyInstants);
			_context.ReleaseFmodInstances();
			if (base.HasStateAuthority)
			{
				_context.FinDamageable.OnDamaged -= OnFinDamagedHandler;
			}
			_context.DisposeNavAgent(this);
			this.OnDeath?.Invoke(this);
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			if (_pendingNavSnap && _context.TrySnapNavAgentOntoNavMesh(_movementSettings))
			{
				_pendingNavSnap = false;
			}
			if (_fsm != null)
			{
				if (_context.IsFearing)
				{
					_fsm.Trigger(SharkEvent.OnFear);
				}
				_fsm.OnLogic();
				_context.TickStartHuntSoundRetreatCheckOnHost();
			}
		}

		public void Fear()
		{
			if (base.HasStateAuthority)
			{
				_context.IsFearing = true;
			}
		}

		public void SetAreaPosition(Vector3 areaPosition)
		{
			_context.SetAreaPosition(areaPosition);
		}

		public void BindSpawnPointOccupancy(IEnemySpawnPointOccupancy occupancy)
		{
			_spawnPointOccupancyBinder.Bind(occupancy, base.Object);
		}

		public void ReleaseSpawnPointOccupancy()
		{
			_spawnPointOccupancyBinder.ReleaseBound();
		}

		public void TriggerEvent(SharkEvent ev)
		{
			if (_fsm != null)
			{
				_fsm.Trigger(ev);
			}
		}

		public void BeginPlayerHunt(PlayerRef target, NetworkObject targetObject)
		{
			_context.TargetPlayer = target;
			_context.TargetObject = targetObject;
			_sessionAnalyticsModel.RegisterEnemyTarget(target.PlayerId);
			TriggerEvent(SharkEvent.OnTargetAcquired);
		}

		public void OnAttractionZoneRaised(AttractionZoneData zone)
		{
			if (base.HasStateAuthority && _fsm != null && _fsm.ActiveStateName == SharkStateId.Idle && !((base.transform.position - zone.Origin).sqrMagnitude > zone.AttractRadius * zone.AttractRadius))
			{
				_context.SetPendingAttractionZone(zone);
				if (_context.CanReachZoneOrigin(_movementSettings))
				{
					_fsm.Trigger(SharkEvent.OnAttractionZoneEntered);
				}
			}
		}

		public void OnAttractionZoneEnded(int zoneId)
		{
		}

		public void SetVisualState(SharkVisualState state)
		{
			if (base.HasStateAuthority)
			{
				VisualState = state;
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

		public bool CanEnterPlayerBite()
		{
			if (_context.TargetPlayer == PlayerRef.None)
			{
				return true;
			}
			if (_context.TargetObject == null || !_context.TargetObject.IsValid)
			{
				return false;
			}
			return !_sensor.IsPlayerProtectedFromAttack(_context.TargetObject.transform.position, base.transform.position);
		}

		public void TryCommitPlayerBite()
		{
			if (!base.HasStateAuthority || _context.TargetPlayer == PlayerRef.None)
			{
				return;
			}
			if (_context.TargetObject == null || !_context.TargetObject.IsValid)
			{
				LosePlayerTarget();
				return;
			}
			if (_sensor.IsPlayerProtectedFromAttack(_context.TargetObject.transform.position, base.transform.position))
			{
				LosePlayerTarget();
				return;
			}
			Vector3 position = _context.TargetObject.transform.position;
			Vector3 position2 = base.transform.position;
			Vector3 to = position + Vector3.up * 1.5f;
			bool flag = _sensor.HasCoverBetween(position2 + Vector3.up * 0.3f, to);
			if (_sensor.WouldSimpleKill(_context.TargetPlayer.PlayerId, _attackSettings.SimpleDamage) && !flag)
			{
				TriggerEvent(SharkEvent.OnCommitLethalAttack);
			}
			else
			{
				TriggerEvent(SharkEvent.OnCommitSimpleAttack);
			}
		}

		public void LosePlayerTarget()
		{
			if (_context.TargetPlayer != PlayerRef.None)
			{
				_context.RecordPlayerHuntLost(_context.TargetPlayer);
			}
			TriggerEvent(SharkEvent.OnTargetLost);
		}

		public void ApplyBiteDamage(float damage, bool applyKnockup)
		{
			if (base.HasStateAuthority && TryGetBiteDamageTarget(out var dmg, out var dmgTransform))
			{
				Vector3 position = _context.FinBody.position;
				Vector3 vector = dmgTransform.position - position;
				Vector3 direction = Vector3.up + vector.normalized * _attackSettings.KnockForwardRatio;
				direction.Normalize();
				dmg.Damage(new DamageData
				{
					Damage = damage,
					Position = position,
					Direction = direction,
					DamageDealerPlayerID = ((base.Object != null) ? base.Object.StateAuthority.PlayerId : 0),
					Force = (applyKnockup ? _attackSettings.KnockUpForce : 0f),
					ForceMode = ForceMode.Impulse,
					IsStunning = false,
					Source = DamageDataSourceExtensions.ForEnemyAttack(base.transform, EnemyType.Shark.ToString(), DamageType.Melee)
				});
			}
		}

		private bool TryGetBiteDamageTarget(out IDamageable dmg, out Transform dmgTransform)
		{
			dmg = null;
			dmgTransform = null;
			if (_context.TargetPlayer != PlayerRef.None)
			{
				int playerId = _context.TargetPlayer.PlayerId;
				if (!_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(playerId))
				{
					return false;
				}
				if (!_playerDamageables.AllPlayerDamageables.TryGetValue(playerId, out dmg))
				{
					return false;
				}
				dmgTransform = dmg.Transform;
				return true;
			}
			if (_context.TargetObject == null || !_context.TargetObject.IsValid)
			{
				return false;
			}
			dmg = TryResolveDamageableOnNetworkObject(_context.TargetObject);
			if (dmg == null)
			{
				return false;
			}
			dmgTransform = dmg.Transform;
			return true;
		}

		private static IDamageable TryResolveDamageableOnNetworkObject(NetworkObject networkObject)
		{
			IDamageable component = networkObject.GetComponent<IDamageable>();
			if (component == null)
			{
				return networkObject.GetComponentInChildren<IDamageable>(includeInactive: true);
			}
			return component;
		}

		public void RequestDespawnNoItemDrop()
		{
			if (base.HasStateAuthority)
			{
				_context.DeadProcessor.IsNeedToSpawnItem = false;
				if (base.Runner != null && base.Object != null)
				{
					base.Object.DespawnHierarchy();
				}
			}
		}

		private void OnFinDamagedHandler(DamageData _)
		{
			if (_fsm != null)
			{
				_fsm.Trigger(SharkEvent.OnFinDamaged);
			}
		}

		public void RaiseAttackStartSound()
		{
			if (base.HasStateAuthority && !_context.ShouldDebounceAttackStartSound())
			{
				_context.RecordAttackStartSoundPlayed();
				PlayAttackStartSoundRpc();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 576290167u)]
		private void PlayAttackStartSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(576290167u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SharkEnemy.SharkEnemy::PlayAttackStartSoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.PlayAttackStartSound();
		}

		public void RaiseStartHuntSound()
		{
			if (base.HasStateAuthority)
			{
				PlayerRef targetPlayer = _context.TargetPlayer;
				if (!(targetPlayer == PlayerRef.None) && !_context.ShouldDebounceStartHuntSoundForPlayer(targetPlayer) && !_context.IsStartHuntSoundBlockedByRetreat())
				{
					_context.ClearPlayerHuntLostRecord();
					_context.OnStartHuntSoundPlayedOnHost();
					PlayStartHuntSoundRpc();
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1530700772u)]
		private void PlayStartHuntSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1530700772u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.SharkEnemy.SharkEnemy::PlayStartHuntSoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.PlayStartHuntSoundLocal();
		}

		public void RaiseChangeAreaTriggered(Vector3 position)
		{
			this.OnChangeAreaTriggered?.Invoke(position);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
		}

		[NetworkRpcWeavedInvoker(576290167u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayAttackStartSoundRpc_0040Invoker576290167([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SharkEnemy)context.TargetBehaviour).PlayAttackStartSoundRpc();
		}

		[NetworkRpcWeavedInvoker(1530700772u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayStartHuntSoundRpc_0040Invoker1530700772([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SharkEnemy)context.TargetBehaviour).PlayStartHuntSoundRpc();
		}
	}
}
