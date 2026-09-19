using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Systems;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Settings;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.DamageableTrackModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.TeleportModule.Scripts.TeleportCommon;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	[NetworkBehaviourWeaved(2)]
	public class CoinRobSwarmEnemy : NetworkBehaviour, ICoinRobSwarmHost, IEnemyBehaviour, IEnemyTypeProvider, ITeleportable, IEnemyFearCallbackListener, IEnemyStateProvider, IStateAuthorityChanged, IPublicFacingInterface
	{
		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private CoinRobSwarmVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SyncedChasePlayer", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private PlayerRef _SyncedChasePlayer;

		private StateMachine<CoinRobSwarmStateId, CoinRobSwarmStateId, CoinRobSwarmEvent> _fsm;

		private CoinRobSwarmEnemyContext _context;

		private CoinRobSwarmAnimatorPresenter _presenter;

		private CoinRobSwarmsDataHolder _swarmsDataHolder;

		private INavigationService _navigationService;

		private IPlayerStateService _playerStateService;

		private CoinRobSwarmFormationService _formationService;

		private CoinRobSwarmSpawnPositionService _spawnPositionService;

		private CoinRobTargetSensor _targetSensor;

		private CoinRobPlayerAttackValidator _playerAttackValidator;

		private MonoItem _chaseTargetMonoItem;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private PlayerReboundModel _playerReboundModel;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private SessionAnalyticsModel _sessionAnalyticsModel;

		private EnemyHeadwearModel _enemyHeadwearModel;

		private CoinRobSwarmEnemySettings _swarmSettings;

		private CoinRobChaseSettings _chaseSettings;

		private CoinRobCombatSettings _combatSettings;

		private CoinRobStealSettings _stealSettings;

		private FearHoleAbsorbAnimationSettings _fearHoleAbsorbAnimationSettings;

		private IRatsHoleSpawnService _ratsHoleSpawnService;

		private RatsHoleRegistry _ratsHoleRegistry;

		private CoinRobCauldronSettings _cauldronSettings;

		private bool _isExtendingSwarm;

		private bool _isSubscribedToPlayerLifecycle;

		private bool _pendingChasePlayerLossFlee;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe CoinRobSwarmVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobSwarmEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(CoinRobSwarmVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobSwarmEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(CoinRobSwarmVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe PlayerRef SyncedChasePlayer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobSwarmEnemy.SyncedChasePlayer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(PlayerRef*)(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CoinRobSwarmEnemy.SyncedChasePlayer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(PlayerRef*)(Ptr + 1) = value;
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => EnemyType.CoinRobSwarm;

		public int EnemyInstants => base.gameObject.GetHashCode();

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

		public CoinRobSwarmStateId CurrentStateId { get; private set; }

		public bool IsInWanderingState => CurrentStateId == CoinRobSwarmStateId.Wandering;

		public bool HasActiveMembers
		{
			get
			{
				if (_context != null)
				{
					return _context.HasActiveMembers;
				}
				return false;
			}
		}

		public bool CanReactToCoinFall
		{
			get
			{
				if (base.HasStateAuthority)
				{
					return IsInWanderingState;
				}
				return false;
			}
		}

		public CoinRobSwarmEnemyContext Context => _context;

		public CoinRobCombatSettings CombatSettings => _combatSettings;

		EnemyState IEnemyStateProvider.CurrentState => MapToLegacyEnemyState(CurrentStateId);

		private bool IsSwarmHostAlive
		{
			get
			{
				if (base.Object != null)
				{
					return base.Object.IsValid;
				}
				return false;
			}
		}

		private int UnitsCount => _context.UnitsCount;

		public event Action<IEnemyBehaviour> OnDeath;

		public bool IsNearChaseTarget(Vector3 worldPosition)
		{
			if (!HasActiveMembers || _context.SwarmKing == null)
			{
				return false;
			}
			float num = Mathf.Max(_chaseSettings.AttackRange, _context.SwarmKing.StoppingDistance + 0.15f);
			return Vector3.Distance(_context.SwarmKing.transform.position, worldPosition) <= num;
		}

		[Inject]
		public void InjectDependencies(CoinRobSwarmEnemyContext context, CoinRobSwarmAnimatorPresenter presenter, CoinRobSwarmsDataHolder swarmsDataHolder, CoinRobSwarmEnemySettings swarmSettings, CoinRobChaseSettings chaseSettings, CoinRobCombatSettings combatSettings, CoinRobStealSettings stealSettings, FearHoleAbsorbAnimationSettings fearHoleAbsorbAnimationSettings, CoinRobCauldronSettings cauldronSettings, CoinRobSwarmFormationService formationService, CoinRobSwarmSpawnPositionService spawnPositionService, CoinRobTargetSensor targetSensor, CoinRobPlayerAttackValidator playerAttackValidator, INavigationService navigationService, IPlayerStateService playerStateService, EnemyFearListenerModel enemyFearListenerModel, SpawnedPlayersModel spawnedPlayersModel, PlayerReboundModel playerReboundModel, PlayerDamageablesTrackModel playerDamageablesTrackModel, SessionAnalyticsModel sessionAnalyticsModel, IRatsHoleSpawnService ratsHoleSpawnService, RatsHoleRegistry ratsHoleRegistry, EnemyHeadwearModel enemyHeadwearModel)
		{
			_context = context;
			_presenter = presenter;
			_swarmsDataHolder = swarmsDataHolder;
			_swarmSettings = swarmSettings;
			_chaseSettings = chaseSettings;
			_combatSettings = combatSettings;
			_stealSettings = stealSettings;
			_fearHoleAbsorbAnimationSettings = fearHoleAbsorbAnimationSettings;
			_cauldronSettings = cauldronSettings;
			_formationService = formationService;
			_spawnPositionService = spawnPositionService;
			_targetSensor = targetSensor;
			_playerAttackValidator = playerAttackValidator;
			_navigationService = navigationService;
			_playerStateService = playerStateService;
			_enemyFearListenerModel = enemyFearListenerModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerReboundModel = playerReboundModel;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_sessionAnalyticsModel = sessionAnalyticsModel;
			_ratsHoleSpawnService = ratsHoleSpawnService;
			_ratsHoleRegistry = ratsHoleRegistry;
			_enemyHeadwearModel = enemyHeadwearModel;
		}

		private StateMachine<CoinRobSwarmStateId, CoinRobSwarmStateId, CoinRobSwarmEvent> BuildFsm()
		{
			StateMachine<CoinRobSwarmStateId, CoinRobSwarmStateId, CoinRobSwarmEvent> stateMachine = new StateMachine<CoinRobSwarmStateId, CoinRobSwarmStateId, CoinRobSwarmEvent>();
			stateMachine.AddState(CoinRobSwarmStateId.Wandering, new CoinRobSwarmWanderingState(this, _context, _formationService, _swarmSettings));
			stateMachine.AddState(CoinRobSwarmStateId.Chasing, new CoinRobSwarmChasingState(this, _context, _formationService, _chaseSettings, _navigationService));
			stateMachine.AddState(CoinRobSwarmStateId.Attacking, new CoinRobSwarmAttackingState(this, _context, _targetSensor, _formationService, _chaseSettings));
			stateMachine.AddState(CoinRobSwarmStateId.Stealing, new CoinRobSwarmStealingState(this, _context, _targetSensor, _stealSettings, _chaseSettings, _presenter, _formationService));
			stateMachine.AddState(CoinRobSwarmStateId.PlayerAttacking, new CoinRobSwarmPlayerAttackingState(this, _context, _formationService, _combatSettings, _chaseSettings, _swarmSettings, _navigationService, _spawnedPlayersModel, _playerStateService, _presenter, _playerAttackValidator));
			stateMachine.AddState(CoinRobSwarmStateId.RunAway, new CoinRobSwarmRunAwayState(this, _context, _formationService, _chaseSettings, _combatSettings));
			stateMachine.AddState(CoinRobSwarmStateId.Fear, new CoinRobSwarmFearState(this, _context, _formationService, _chaseSettings, _fearHoleAbsorbAnimationSettings, _ratsHoleRegistry));
			stateMachine.AddState(CoinRobSwarmStateId.Cauldroned, new CoinRobSwarmCauldronedState(this, _context, _formationService, _chaseSettings, _combatSettings, _cauldronSettings, _enemyHeadwearModel));
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnChaseRequested, CoinRobSwarmStateId.Wandering, CoinRobSwarmStateId.Chasing);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnChaseRequested, CoinRobSwarmStateId.Attacking, CoinRobSwarmStateId.Chasing);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnReachChasePoint, CoinRobSwarmStateId.Chasing, CoinRobSwarmStateId.Attacking);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnItemsMarkedForSteal, CoinRobSwarmStateId.Attacking, CoinRobSwarmStateId.Stealing);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnPlayerAttackStarted, CoinRobSwarmStateId.Wandering, CoinRobSwarmStateId.PlayerAttacking);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnPlayerAttackStarted, CoinRobSwarmStateId.Chasing, CoinRobSwarmStateId.PlayerAttacking);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnPlayerAttackStarted, CoinRobSwarmStateId.Attacking, CoinRobSwarmStateId.PlayerAttacking);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnPlayerAttackStarted, CoinRobSwarmStateId.RunAway, CoinRobSwarmStateId.PlayerAttacking);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnPlayerAttackCancelled, CoinRobSwarmStateId.PlayerAttacking, CoinRobSwarmStateId.Wandering);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnRunAway, CoinRobSwarmStateId.Chasing, CoinRobSwarmStateId.RunAway);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnRunAway, CoinRobSwarmStateId.Attacking, CoinRobSwarmStateId.RunAway);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnRunAway, CoinRobSwarmStateId.PlayerAttacking, CoinRobSwarmStateId.RunAway);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnRunAwayComplete, CoinRobSwarmStateId.RunAway, CoinRobSwarmStateId.Wandering);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnPlayerAttackStarted, CoinRobSwarmStateId.Stealing, CoinRobSwarmStateId.PlayerAttacking);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnRunAway, CoinRobSwarmStateId.Stealing, CoinRobSwarmStateId.RunAway);
			stateMachine.AddTriggerTransitionFromAny(CoinRobSwarmEvent.OnFear, CoinRobSwarmStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(CoinRobSwarmEvent.OnFearEscapeCompleted, CoinRobSwarmStateId.Fear, CoinRobSwarmStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransitionFromAny(CoinRobSwarmEvent.OnCauldroned, CoinRobSwarmStateId.Cauldroned, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(CoinRobSwarmStateId.Wandering);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			FearCompleted = false;
			_context.IsFearing = false;
			_context.CurrentFsmStateId = CoinRobSwarmStateId.Wandering;
			_context.BindReactivePlayerAttackQueued(delegate
			{
				OnReactivePlayerAttackQueued();
			});
			_enemyFearListenerModel.RegisterFearListener(EnemyType.CoinRobSwarm, base.gameObject.GetHashCode(), this);
			EnsureMasterStateAuthority();
			if (IsMasterAiPeer() && base.HasStateAuthority)
			{
				InitializeAuthority();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			base.Despawned(runner, hasState);
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType.CoinRobSwarm, base.gameObject.GetHashCode());
			if (base.HasStateAuthority)
			{
				_swarmsDataHolder.ActiveCoinRobSwarms.Remove(this);
				_context.DisableReactiveAggro();
				UnsubscribePlayerLifecycleEvents();
				if (_fsm != null)
				{
					_fsm.StateChanged -= OnFsmStateChanged;
				}
			}
			this.OnDeath?.Invoke(this);
		}

		public override void FixedUpdateNetwork()
		{
			if (!IsMasterAiPeer())
			{
				return;
			}
			EnsureMasterStateAuthority();
			if (!base.HasStateAuthority || _fsm == null)
			{
				return;
			}
			if (!_context.HasActiveMembers && VisualState != CoinRobSwarmVisualState.Wandering)
			{
				TryRebuildLocalMemberContext();
			}
			if (!TryHandleChasePlayerUnavailable())
			{
				if (IsKingCauldroned() && CurrentStateId != CoinRobSwarmStateId.Cauldroned)
				{
					Trigger(CoinRobSwarmEvent.OnCauldroned);
				}
				TryResolveReactivePlayerAttackRequest();
				_fsm.OnLogic();
				_presenter.SyncUnitsCarryLocomotion();
				UpdateKingRotation();
			}
		}

		private void OnReactivePlayerAttackQueued()
		{
			if (base.HasStateAuthority && _fsm != null)
			{
				TryResolveReactivePlayerAttackRequest();
			}
		}

		public void StateAuthorityChanged()
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				if (!IsMasterAiPeer())
				{
					TeardownAuthority();
					return;
				}
				if (!base.HasStateAuthority)
				{
					TeardownAuthority();
					return;
				}
				EnsureMasterStateAuthority();
				InitializeAuthority();
			}
		}

		private bool IsMasterAiPeer()
		{
			if (base.Runner != null)
			{
				return base.Runner.IsSharedModeMasterClient;
			}
			return false;
		}

		private bool IsKingCauldroned()
		{
			if (_context.SwarmKing != null && _context.SwarmKing.Object != null)
			{
				return _enemyHeadwearModel.IsWearing(_context.SwarmKing.Object.Id.Raw);
			}
			return false;
		}

		private void EnsureMasterStateAuthority()
		{
			if (!(base.Runner == null) && base.Object.IsValid && base.Runner.IsSharedModeMasterClient)
			{
				if (!base.HasStateAuthority)
				{
					base.Object.RequestStateAuthority();
				}
				EnsureSwarmMembersStateAuthority();
			}
		}

		private void EnsureSwarmMembersStateAuthority()
		{
			if (base.Runner == null || !base.Runner.IsSharedModeMasterClient)
			{
				return;
			}
			if (_context.SwarmKing != null)
			{
				NetworkObject networkObject = _context.SwarmKing.Object;
				if (networkObject != null && networkObject.IsValid && !networkObject.HasStateAuthority)
				{
					networkObject.RequestStateAuthority();
				}
			}
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				NetworkObject networkObject2 = swarmUnit?.Object;
				if (networkObject2 != null && networkObject2.IsValid && !networkObject2.HasStateAuthority)
				{
					networkObject2.RequestStateAuthority();
				}
			}
		}

		private void InitializeAuthority()
		{
			if (!IsMasterAiPeer())
			{
				return;
			}
			_ratsHoleSpawnService.EnsureAllSpawned();
			if (_fsm == null)
			{
				if (!_swarmsDataHolder.ActiveCoinRobSwarms.Contains(this))
				{
					_swarmsDataHolder.ActiveCoinRobSwarms.Add(this);
				}
				_fsm = BuildFsm();
				_fsm.Init();
				_fsm.StateChanged += OnFsmStateChanged;
				SubscribePlayerLifecycleEvents();
				TryRebuildLocalMemberContext();
				RestoreChasePlayerFromNetwork();
				EnsureSwarmMembersStateAuthority();
				if (ShouldFleeOnAuthorityRestore())
				{
					_pendingChasePlayerLossFlee = false;
					EnterRunAwayState();
				}
				else
				{
					CoinRobSwarmStateId coinRobSwarmStateId = ResolveRestoreState();
					if (coinRobSwarmStateId == CoinRobSwarmStateId.Wandering)
					{
						ClearChasePlayerIfUnavailable();
						CurrentStateId = CoinRobSwarmStateId.Wandering;
						_context.CurrentFsmStateId = CoinRobSwarmStateId.Wandering;
						_fsm.RequestStateChange(CoinRobSwarmStateId.Wandering, forceInstantly: true);
						SetVisualState(CoinRobSwarmVisualState.Wandering);
						_presenter.ApplyFsmVisualState(CoinRobSwarmVisualState.Wandering);
						RefreshFormationForRestoredState(CoinRobSwarmStateId.Wandering);
					}
					else
					{
						CurrentStateId = coinRobSwarmStateId;
						_context.CurrentFsmStateId = coinRobSwarmStateId;
						_fsm.RequestStateChange(coinRobSwarmStateId, forceInstantly: true);
						SetVisualState((CoinRobSwarmVisualState)coinRobSwarmStateId);
						_presenter.ApplyFsmVisualState((CoinRobSwarmVisualState)coinRobSwarmStateId);
						RefreshFormationForRestoredState(coinRobSwarmStateId);
					}
				}
			}
			TryEnsureInitialSwarmMembers();
		}

		private void TryEnsureInitialSwarmMembers()
		{
			if (base.HasStateAuthority && !(_context.SwarmKing != null))
			{
				EnsureInitialSwarmMembersAsync().Forget();
			}
		}

		private async UniTaskVoid EnsureInitialSwarmMembersAsync()
		{
			if (base.HasStateAuthority && !(_context.SwarmKing != null))
			{
				await TryExtendSwarm();
				if (IsSwarmHostAlive && base.HasStateAuthority && !(_context.SwarmKing == null) && IsInWanderingState)
				{
					RefreshFormationForRestoredState(CoinRobSwarmStateId.Wandering);
				}
			}
		}

		private CoinRobSwarmStateId ResolveRestoreState()
		{
			if (_context.CurrentFsmStateId != CoinRobSwarmStateId.Wandering)
			{
				return _context.CurrentFsmStateId;
			}
			if (VisualState != CoinRobSwarmVisualState.Wandering)
			{
				return (CoinRobSwarmStateId)VisualState;
			}
			return CoinRobSwarmStateId.Wandering;
		}

		private bool ShouldFleeOnAuthorityRestore()
		{
			if (_pendingChasePlayerLossFlee)
			{
				return true;
			}
			if (_context.HasMembersCarryingLoot())
			{
				return true;
			}
			if (VisualState == CoinRobSwarmVisualState.RunAway || VisualState == CoinRobSwarmVisualState.Stealing)
			{
				return true;
			}
			PlayerRef effectiveChasePlayer = GetEffectiveChasePlayer();
			if (effectiveChasePlayer != PlayerRef.None && IsChasePlayerUnavailable(effectiveChasePlayer))
			{
				return true;
			}
			if (VisualState != CoinRobSwarmVisualState.PlayerAttacking)
			{
				return _context.CurrentFsmStateId == CoinRobSwarmStateId.PlayerAttacking;
			}
			return true;
		}

		private bool TryHandleChasePlayerUnavailable()
		{
			PlayerRef effectiveChasePlayer = GetEffectiveChasePlayer();
			if (effectiveChasePlayer == PlayerRef.None || !IsChasePlayerUnavailable(effectiveChasePlayer))
			{
				return false;
			}
			if (CurrentStateId == CoinRobSwarmStateId.PlayerAttacking)
			{
				EnterRunAwayState();
				return true;
			}
			ClearChasePlayerState();
			return false;
		}

		private void ClearChasePlayerIfUnavailable()
		{
			PlayerRef effectiveChasePlayer = GetEffectiveChasePlayer();
			if (effectiveChasePlayer != PlayerRef.None && IsChasePlayerUnavailable(effectiveChasePlayer))
			{
				ClearChasePlayerState();
			}
		}

		private void ClearChasePlayerState()
		{
			_context.ChasePlayer = PlayerRef.None;
			if (base.HasStateAuthority)
			{
				SyncedChasePlayer = PlayerRef.None;
			}
		}

		private void EnterRunAwayState()
		{
			BeginRunAway();
			if (_fsm != null && CurrentStateId != CoinRobSwarmStateId.RunAway)
			{
				_fsm.RequestStateChange(CoinRobSwarmStateId.RunAway, forceInstantly: true);
			}
		}

		private bool IsChasePlayerUnavailable(PlayerRef chasePlayer)
		{
			if (chasePlayer == PlayerRef.None)
			{
				return false;
			}
			if (base.Runner == null || !base.Runner.IsRunning)
			{
				return true;
			}
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (!(activePlayer == chasePlayer))
				{
					continue;
				}
				goto IL_0065;
			}
			return true;
			IL_0065:
			if (!_playerStateService.IsPlayerAlive(chasePlayer.PlayerId))
			{
				return true;
			}
			if (!_spawnedPlayersModel.Players.TryGetValue(chasePlayer, out var value))
			{
				return true;
			}
			if (!(value.NetworkObject == null))
			{
				return !value.NetworkObject.IsValid;
			}
			return true;
		}

		private void RefreshFormationForRestoredState(CoinRobSwarmStateId stateId)
		{
			switch (stateId)
			{
			case CoinRobSwarmStateId.RunAway:
			case CoinRobSwarmStateId.Cauldroned:
				RefreshRunAwayFormation();
				break;
			case CoinRobSwarmStateId.Chasing:
				if (_context.ChasePosition.sqrMagnitude > 0.0001f)
				{
					_formationService.SetSwarmDestination(_context.ChasePosition);
				}
				break;
			case CoinRobSwarmStateId.Wandering:
				_formationService.MoveSwarmToPatrolAroundKing();
				break;
			}
		}

		private void RefreshRunAwayFormation()
		{
			if (_context.RunAwayPosition.sqrMagnitude < 0.0001f || Vector3.Distance(_context.RunAwayStartPosition, _context.RunAwayPosition) < 0.0001f)
			{
				_context.RunAwayStartPosition = _context.SwarmCenter;
				_context.RunAwayPosition = _formationService.ResolveRunAwayDestination(_context.RunAwayStartPosition);
			}
			_formationService.SetRunAwayDestination(_context.RunAwayPosition);
		}

		private void TeardownAuthority()
		{
			if (_fsm != null)
			{
				UnsubscribePlayerLifecycleEvents();
				_swarmsDataHolder.ActiveCoinRobSwarms.Remove(this);
				_context.DisableReactiveAggro();
				_fsm.StateChanged -= OnFsmStateChanged;
				_fsm = null;
			}
		}

		private void SubscribePlayerLifecycleEvents()
		{
			if (!_isSubscribedToPlayerLifecycle)
			{
				_spawnedPlayersModel.OnPlayerUnregistered += HandleChasePlayerUnregistered;
				_playerReboundModel.OnPlayerRebound += HandleChasePlayerRebound;
				_isSubscribedToPlayerLifecycle = true;
			}
		}

		private void UnsubscribePlayerLifecycleEvents()
		{
			if (_isSubscribedToPlayerLifecycle)
			{
				_spawnedPlayersModel.OnPlayerUnregistered -= HandleChasePlayerUnregistered;
				_playerReboundModel.OnPlayerRebound -= HandleChasePlayerRebound;
				_isSubscribedToPlayerLifecycle = false;
			}
		}

		private void HandleChasePlayerUnregistered(PlayerRef playerRef)
		{
			if (IsReactiveAttackTargetPlayer(playerRef))
			{
				_context.ClearPendingReactivePlayerAttack();
				if (playerRef.PlayerId == _context.LastMemberDamageDealerPlayerId)
				{
					_context.LastMemberDamageDealerPlayerId = 0;
				}
				TryFleeFromChasePlayerLoss();
			}
		}

		private void HandleChasePlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			if (IsTrackingChasePlayer(playerRef) && CurrentStateId == CoinRobSwarmStateId.PlayerAttacking)
			{
				TryFleeFromChasePlayerLoss();
			}
		}

		private void TryFleeFromChasePlayerLoss()
		{
			if (IsMasterAiPeer())
			{
				_pendingChasePlayerLossFlee = true;
				EnsureMasterStateAuthority();
				if (base.HasStateAuthority)
				{
					ApplyChasePlayerLossFlee();
				}
			}
		}

		private void ApplyChasePlayerLossFlee()
		{
			if (_fsm == null)
			{
				InitializeAuthority();
				return;
			}
			_pendingChasePlayerLossFlee = false;
			EnterRunAwayState();
		}

		private PlayerRef GetEffectiveChasePlayer()
		{
			if (_context.ChasePlayer != PlayerRef.None)
			{
				return _context.ChasePlayer;
			}
			return SyncedChasePlayer;
		}

		private bool IsTrackingChasePlayer(PlayerRef playerRef)
		{
			if (!(_context.ChasePlayer == playerRef))
			{
				return SyncedChasePlayer == playerRef;
			}
			return true;
		}

		private bool IsReactiveAttackTargetPlayer(PlayerRef playerRef)
		{
			if (IsTrackingChasePlayer(playerRef))
			{
				return true;
			}
			if (_context.IsReadyForPlayerAttack && _context.PendingPlayerAttackPlayerId > 0)
			{
				return playerRef.PlayerId == _context.PendingPlayerAttackPlayerId;
			}
			return false;
		}

		private void RestoreChasePlayerFromNetwork()
		{
			if (_context.ChasePlayer == PlayerRef.None && SyncedChasePlayer != PlayerRef.None)
			{
				_context.ChasePlayer = SyncedChasePlayer;
			}
		}

		private void TryRebuildLocalMemberContext()
		{
			if (_context.HasActiveMembers || base.Runner == null || !base.Object.IsValid)
			{
				return;
			}
			NetworkId id = base.Object.Id;
			foreach (NetworkObject allNetworkObject in base.Runner.GetAllNetworkObjects())
			{
				if (allNetworkObject == null || !allNetworkObject.IsValid || !allNetworkObject.TryGetComponent<CoinRobBehaviour>(out var component) || component.SwarmHostNetworkId != id)
				{
					continue;
				}
				if (component.IsSwarmKing)
				{
					if (_context.SwarmKing == null)
					{
						AttachSwarmKing(component);
					}
				}
				else
				{
					RegisterSwarmUnit(component);
				}
			}
		}

		private void AttachSwarmKing(CoinRobBehaviour swarmKing)
		{
			_context.SwarmKing = swarmKing;
			swarmKing.SetAutoRotationActive(isActive: false);
			_context.SwarmKingRotator = swarmKing.GetComponentInChildren<EnemyRotator>();
			swarmKing.OnDeactivated -= OnKingDeactivated;
			swarmKing.OnDeactivated += OnKingDeactivated;
			RegisterSwarmMemberAggro(swarmKing);
		}

		private void BindSwarmMemberHost(CoinRobBehaviour member)
		{
			if (!(member == null))
			{
				member.BindSwarmHost(base.Object.Id);
			}
		}

		public async UniTask<bool> TryExtendSwarm()
		{
			if (!IsSwarmHostAlive || !base.HasStateAuthority)
			{
				return false;
			}
			while (_isExtendingSwarm)
			{
				await UniTask.Yield();
				if (!IsSwarmHostAlive)
				{
					return false;
				}
			}
			if (UnitsCount >= _swarmSettings.MaxUnitsInSwarmCount)
			{
				return false;
			}
			_isExtendingSwarm = true;
			try
			{
				bool isFirstActivation = _context.SwarmKing == null;
				if (isFirstActivation)
				{
					await SpawnSwarmKing();
					if (_context.SwarmKing == null)
					{
						return false;
					}
				}
				if (!IsSwarmHostAlive)
				{
					return false;
				}
				int unitsToSpawn = ((!isFirstActivation) ? 1 : ResolveInitialUnitsToSpawn());
				bool spawnedAny = false;
				for (int i = 0; i < unitsToSpawn; i++)
				{
					if (UnitsCount >= _swarmSettings.MaxUnitsInSwarmCount)
					{
						break;
					}
					if (!(await TrySpawnSwarmUnit()))
					{
						break;
					}
					spawnedAny = true;
				}
				return spawnedAny;
			}
			finally
			{
				_isExtendingSwarm = false;
			}
		}

		private int ResolveInitialUnitsToSpawn()
		{
			int num = Mathf.Clamp(_swarmSettings.InitialUnitsCount, 0, _swarmSettings.MaxUnitsInSwarmCount);
			if (num <= 0)
			{
				return 1;
			}
			return num;
		}

		private async UniTask<bool> TrySpawnSwarmUnit()
		{
			if (!_spawnPositionService.TryGetReachableUnitSpawnPosition(out var spawnPosition))
			{
				return false;
			}
			NetworkRunner runner = base.Runner;
			NetworkObject networkObject = await base.Runner.SpawnAsync(_swarmSettings.CoinRobPrefab, spawnPosition);
			if (networkObject == null)
			{
				return false;
			}
			if (!IsSwarmHostAlive)
			{
				DespawnOrphanedMember(runner, networkObject);
				return false;
			}
			CoinRobBehaviour component = networkObject.GetComponent<CoinRobBehaviour>();
			if (component == null)
			{
				return false;
			}
			RegisterSwarmUnit(component);
			_formationService.PlayInitialAnimation(component);
			return true;
		}

		private void RegisterSwarmUnit(CoinRobBehaviour swarmUnit)
		{
			if (swarmUnit == null)
			{
				return;
			}
			foreach (CoinRobBehaviour swarmUnit2 in _context.SwarmUnits)
			{
				if (swarmUnit2 == swarmUnit)
				{
					return;
				}
			}
			_context.AddUnit(swarmUnit);
			swarmUnit.OnDeactivated += OnUnitDeactivated;
			RegisterSwarmMemberAggro(swarmUnit);
			BindSwarmMemberHost(swarmUnit);
		}

		public void PrepareCoinChase(Vector3 target, IItem chaseItem)
		{
			_context.ChasePosition = target;
			_context.ChaseTargetItem = chaseItem;
			_context.IsPatrolWaiting = false;
			_context.WanderingTimer = 0f;
			HookChaseItemDespawn(chaseItem);
		}

		public void CancelCoinChase()
		{
			UnhookChaseItemDespawn();
			_context.ChaseTargetItem = null;
		}

		public void ChaseTarget(Vector3 target, IItem chaseItem = null)
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			if (_fsm == null)
			{
				CancelCoinChase();
				return;
			}
			if (_formationService == null)
			{
				CancelCoinChase();
				return;
			}
			if (!HasActiveMembers)
			{
				CancelCoinChase();
				return;
			}
			PrepareCoinChase(target, chaseItem);
			switch (CurrentStateId)
			{
			case CoinRobSwarmStateId.Wandering:
			case CoinRobSwarmStateId.Attacking:
				Trigger(CoinRobSwarmEvent.OnChaseRequested);
				break;
			default:
				CancelCoinChase();
				return;
			case CoinRobSwarmStateId.Chasing:
				break;
			}
			_formationService.SetSwarmDestination(target);
		}

		private void HookChaseItemDespawn(IItem chaseItem)
		{
			if (_chaseTargetMonoItem != null)
			{
				if (chaseItem == _chaseTargetMonoItem)
				{
					return;
				}
				UnhookChaseItemDespawn();
			}
			if (chaseItem != null && !(chaseItem.NetworkObject == null) && chaseItem.NetworkObject.TryGetComponent<MonoItem>(out var component))
			{
				_chaseTargetMonoItem = component;
				_chaseTargetMonoItem.OnDespawn += HandleChaseItemDespawned;
			}
		}

		private void UnhookChaseItemDespawn()
		{
			if (!(_chaseTargetMonoItem == null))
			{
				_chaseTargetMonoItem.OnDespawn -= HandleChaseItemDespawned;
				_chaseTargetMonoItem = null;
			}
		}

		private void HandleChaseItemDespawned(IItem item)
		{
			if (base.HasStateAuthority && _context.ChaseTargetItem != null && _context.ChaseTargetItem == item)
			{
				CancelCoinChase();
				if (CurrentStateId != CoinRobSwarmStateId.Stealing)
				{
					BeginRunAway();
					Trigger(CoinRobSwarmEvent.OnRunAway);
				}
			}
		}

		public void Teleport(Vector3 position)
		{
			if (base.HasStateAuthority)
			{
				_formationService.TeleportSwarm(position);
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

		public void Fear()
		{
			if (base.HasStateAuthority && (CurrentStateId != CoinRobSwarmStateId.Fear || !_context.IsFearing))
			{
				_context.FearInterruptedStateId = CurrentStateId;
				_context.IsFearing = true;
				if (_fsm != null)
				{
					Trigger(CoinRobSwarmEvent.OnFear);
				}
			}
		}

		public void MarkFearCompleted()
		{
			_context.IsFearing = false;
			FearCompleted = true;
		}

		public void Trigger(CoinRobSwarmEvent ev)
		{
			_fsm?.Trigger(ev);
		}

		public void BeginPlayerAttack(PlayerRef player)
		{
			_context.ChasePlayer = player;
			SyncedChasePlayer = player;
			_sessionAnalyticsModel.RegisterEnemyTarget(player.PlayerId);
		}

		public void BeginWandering()
		{
			ClearChasePlayerState();
			_context.ChaseTargetItem = null;
		}

		public void CancelPlayerAttackForFear()
		{
			ClearChasePlayerState();
			_context.IsAttackPerforming = false;
			if (_context.SwarmKing != null)
			{
				_context.SwarmKing.OnAttackPreformed -= DealDamageToChasePlayer;
			}
		}

		public bool TryResolveReactivePlayerAttackRequest()
		{
			if (!_context.IsReadyForPlayerAttack)
			{
				return false;
			}
			int pendingPlayerAttackPlayerId = _context.PendingPlayerAttackPlayerId;
			if (!TryBeginReactivePlayerAttack(pendingPlayerAttackPlayerId))
			{
				return false;
			}
			_context.IsReadyForPlayerAttack = false;
			_context.PendingPlayerAttackPlayerId = 0;
			return true;
		}

		public bool TryBeginReactivePlayerAttack(int playerId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (_fsm == null)
			{
				return false;
			}
			if (!HasActiveMembers)
			{
				return false;
			}
			PlayerRef playerRef = PlayerRef.None;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == playerId)
				{
					playerRef = player.Key;
					break;
				}
			}
			if (playerRef == PlayerRef.None)
			{
				return false;
			}
			if (!_playerAttackValidator.CanEngageReactivePlayerAttack(playerRef))
			{
				return false;
			}
			if (CurrentStateId == CoinRobSwarmStateId.PlayerAttacking)
			{
				return false;
			}
			if (CurrentStateId == CoinRobSwarmStateId.Cauldroned || IsKingCauldroned())
			{
				return false;
			}
			_context.IsProvokedByMemberDamage = true;
			_context.ActiveStealTasks.Clear();
			CancelCoinChase();
			BeginPlayerAttack(playerRef);
			Trigger(CoinRobSwarmEvent.OnPlayerAttackStarted);
			return true;
		}

		public void BeginRunAway()
		{
			_context.ChaseTargetItem = null;
			ClearChasePlayerState();
			_context.RunAwayStartPosition = _context.SwarmCenter;
			_context.RunAwayPosition = _formationService.ResolveRunAwayDestination(_context.RunAwayStartPosition);
			_formationService.SetRunAwayDestination(_context.RunAwayPosition);
		}

		public void DealDamageToChasePlayer()
		{
			if (_context.SwarmKing != null)
			{
				_context.SwarmKing.OnAttackPreformed -= DealDamageToChasePlayer;
			}
			if (_spawnedPlayersModel.Players.TryGetValue(_context.ChasePlayer, out var value) && value.NetworkObject != null && !_playerAttackValidator.CanKingAttackPlayer(_context.ChasePlayer, _context.SwarmKing))
			{
				return;
			}
			if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(_context.ChasePlayer.PlayerId, out var value2) && _spawnedPlayersModel.Players.TryGetValue(_context.ChasePlayer, out var value3))
			{
				Transform transform = ((_context.SwarmKing != null) ? _context.SwarmKing.transform : base.transform);
				Vector3 direction = value3.NetworkObject.transform.position - transform.position;
				direction.y = 0f;
				if (direction.sqrMagnitude > 0.0001f)
				{
					direction.Normalize();
				}
				else
				{
					direction = transform.forward;
				}
				value2.Damage(new DamageData
				{
					Damage = _combatSettings.Damage,
					Position = transform.position,
					Direction = direction,
					DamageDealerPlayerID = base.Object.StateAuthority.PlayerId,
					Force = _combatSettings.ForceStrength,
					ForceMode = ForceMode.Impulse,
					Source = DamageDataSourceExtensions.ForEnemyAttack(transform, EnemyType.CoinRobSwarm.ToString(), DamageType.Melee)
				});
			}
			_context.IsAttackPerforming = false;
		}

		public void DespawnSwarm()
		{
			DespawnSwarmInternal(isDissolveDespawn: false);
		}

		public void DespawnSwarmWithDissolve()
		{
			DespawnSwarmInternal(isDissolveDespawn: true);
		}

		private void DespawnSwarmInternal(bool isDissolveDespawn)
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit != null && swarmUnit.Object != null)
				{
					DespawnMember(swarmUnit, isDissolveDespawn);
				}
			}
			if (_context.SwarmKing != null && _context.SwarmKing.Object != null)
			{
				DespawnMember(_context.SwarmKing, isDissolveDespawn);
			}
			_context.ClearSwarm();
			_swarmsDataHolder.ActiveCoinRobSwarms.Remove(this);
			base.Runner.Despawn(base.Object);
		}

		private void DespawnMember(CoinRobBehaviour member, bool isDissolveDespawn)
		{
			if (!isDissolveDespawn || !member.TryGetComponent<EnemyDeathDissolveEffect>(out var component) || !component.TryStartDeferredDespawn(EnemyDissolveReason.Despawn))
			{
				base.Runner.Despawn(member.Object);
			}
		}

		public void SetVisualState(CoinRobSwarmVisualState state)
		{
			if (base.HasStateAuthority && VisualState != state)
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

		private async UniTask SpawnSwarmKing()
		{
			Vector3 value = _navigationService.ValidatePointOnNavmesh(_context.SwarmCenter, _swarmSettings.SwarmRadius);
			NetworkRunner runner = base.Runner;
			NetworkObject networkObject = await runner.SpawnAsync(_swarmSettings.CoinRobKingPrefab, value);
			if (networkObject == null)
			{
				return;
			}
			if (!IsSwarmHostAlive)
			{
				DespawnOrphanedMember(runner, networkObject);
				return;
			}
			CoinRobBehaviour component = networkObject.GetComponent<CoinRobBehaviour>();
			if (!(component == null))
			{
				AttachSwarmKing(component);
				_formationService.PlayInitialAnimation(component);
				BindSwarmMemberHost(component);
			}
		}

		private static void DespawnOrphanedMember(NetworkRunner runner, NetworkObject member)
		{
			if (!(runner == null) && runner.IsRunning && !(member == null))
			{
				runner.Despawn(member);
			}
		}

		private void RegisterSwarmMemberAggro(CoinRobBehaviour member)
		{
			if (member.TryGetComponent<CoinRobMemberReactiveAggroSystem>(out var component))
			{
				component.Bind(_context);
			}
			if (member.TryGetComponent<CoinRobSwarmChaseAggroSystem>(out var component2))
			{
				component2.Bind(_context, _combatSettings);
			}
		}

		public void RequestReactivePlayerAttackFromMember(int playerId)
		{
			if (playerId > 0)
			{
				if (base.HasStateAuthority)
				{
					_context.QueueReactivePlayerAttack(playerId);
				}
				else
				{
					RequestReactivePlayerAttackRpc(playerId);
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 77832696u)]
		private void RequestReactivePlayerAttackRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(77832696u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.CoinRobSwarmEnemy::RequestReactivePlayerAttackRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_context.QueueReactivePlayerAttack(playerId);
		}

		private void OnUnitDeactivated(CoinRobBehaviour unit)
		{
			unit.OnDeactivated -= OnUnitDeactivated;
			_presenter.ClearMemberLocomotionState(unit);
			_context.RemoveUnit(unit);
			int num = unit.LastDamageDealerPlayerId;
			if (num <= 0)
			{
				num = _context.LastMemberDamageDealerPlayerId;
			}
			else
			{
				_context.LastMemberDamageDealerPlayerId = num;
			}
			if (base.HasStateAuthority && num > 0)
			{
				_context.QueueReactivePlayerAttack(num);
			}
		}

		private void OnKingDeactivated(CoinRobBehaviour king)
		{
			king.OnDeactivated -= OnKingDeactivated;
			_presenter.ClearMemberLocomotionState(king);
			if (king.TryGetComponent<SimpleEnemyDeadProcessor>(out var component))
			{
				component.ProcessEnemyDeath();
			}
			DespawnSwarmInternal(isDissolveDespawn: true);
		}

		private void OnFsmStateChanged(StateBase<CoinRobSwarmStateId> state)
		{
			CurrentStateId = state.name;
			_context.CurrentFsmStateId = CurrentStateId;
			CoinRobSwarmVisualState currentStateId = (CoinRobSwarmVisualState)CurrentStateId;
			SetVisualState(currentStateId);
			_presenter.ApplyFsmVisualState(currentStateId);
		}

		private void UpdateKingRotation()
		{
			if (!(_context.SwarmKing == null) && !(_context.SwarmKingRotator == null))
			{
				PlayerDataHolder value;
				if (CurrentStateId == CoinRobSwarmStateId.Fear || _context.ChasePlayer == PlayerRef.None)
				{
					_context.SwarmKingRotator.RotateTowardsDirection(_context.SwarmKing.Velocity);
				}
				else if (_spawnedPlayersModel.Players.TryGetValue(_context.ChasePlayer, out value))
				{
					Vector3 position = value.NetworkObject.transform.position;
					_context.SwarmKingRotator.RotateTowardsDirection(position - _context.SwarmKing.transform.position);
				}
			}
		}

		private static EnemyState MapToLegacyEnemyState(CoinRobSwarmStateId stateId)
		{
			return stateId switch
			{
				CoinRobSwarmStateId.Wandering => EnemyState.Wandering, 
				CoinRobSwarmStateId.Chasing => EnemyState.Chasing, 
				CoinRobSwarmStateId.Attacking => EnemyState.Attacking, 
				CoinRobSwarmStateId.Stealing => EnemyState.Stealing, 
				CoinRobSwarmStateId.PlayerAttacking => EnemyState.PlayerAttacking, 
				CoinRobSwarmStateId.RunAway => EnemyState.RunAway, 
				CoinRobSwarmStateId.Fear => EnemyState.Fear, 
				CoinRobSwarmStateId.Cauldroned => EnemyState.RunAway, 
				_ => EnemyState.None, 
			};
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
			SyncedChasePlayer = _SyncedChasePlayer;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
			_SyncedChasePlayer = SyncedChasePlayer;
		}

		[NetworkRpcWeavedInvoker(77832696u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestReactivePlayerAttackRpc_0040Invoker77832696([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CoinRobSwarmEnemy)context.TargetBehaviour).RequestReactivePlayerAttackRpc(value);
		}
	}
}
