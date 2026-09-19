using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using Features.AIModule.Scripts;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.InteractModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StateMachineDebug;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.TeleportModule.Scripts.TeleportCommon;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	[NetworkBehaviourWeaved(5)]
	public class MonkeyEnemy : NetworkBehaviour, ITeleportable, IEnemyBehaviour, IEnemyTypeProvider, IEnemyFearCallbackListener, IStateAuthorityChanged, IPublicFacingInterface, IHfsmDebugSource<MonkeyStateId, MonkeyStateId, MonkeyEvent>, IHfsmDebugSource, IEnemyAttractionZoneCallbackListener
	{
		private const float NetworkQuantizeFactor = 10f;

		private const float NetworkDequantizeFactor = 0.1f;

		private const float HungerReductionMultiplier = 100f;

		private readonly int _moveSlowHash = Animator.StringToHash("MoveSlow");

		private readonly int _moveMiddleHash = Animator.StringToHash("MoveMiddle");

		private readonly int _hitHash = Animator.StringToHash("Hit");

		private readonly int _idleHash = Animator.StringToHash("Idle");

		private readonly int _runHash = Animator.StringToHash("Run");

		private readonly int _moveHash = Animator.StringToHash("Move");

		private readonly int _grabHash = Animator.StringToHash("Grab");

		private readonly int _attackHash = Animator.StringToHash("Attack");

		private readonly int _coinThinkHash = Animator.StringToHash("CoinThink");

		private readonly int _coinRequestHash = Animator.StringToHash("CoinRequest");

		private readonly int _fakeAttackHash = Animator.StringToHash("RangeAttacking");

		private readonly int _attackStateHash = Animator.StringToHash("CHR_Monkey_Attack_02");

		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private MonkeyVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CurrentTargetPlayerId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CurrentTargetPlayerId = -1;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsCoinVisible", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsCoinVisible;

		[WeaverGenerated]
		[DefaultForProperty("CurrentHungerQuantized", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _CurrentHungerQuantized;

		[WeaverGenerated]
		[DefaultForProperty("AgentVelocityQuantized", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _AgentVelocityQuantized;

		private StateMachine<MonkeyStateId, MonkeyStateId, MonkeyEvent> _fsm;

		private MonkeyEnemyContext _context;

		private MonkeyEnemySettings _enemySettings;

		private MonkeyMovementSettings _movementSettings;

		private MonkeyPlayerInteractionSettings _playerInteractionSettings;

		private MonkeyItemInteractionSettings _itemInteractionSettings;

		private MonkeyItemGiftSettings _itemGiftSettings;

		private MonkeyCombatSettings _combatSettings;

		private MonkeyItemSensor _itemSensor;

		private EnemyFearListenerModel _enemyFearListenerModel;

		private IInstantiator _instantiator;

		private IPlayerStateService _playerStateService;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private EnemyHeadwearModel _enemyHeadwearModel;

		private IAudioService _audioService;

		private IItemSpawnService _itemSpawnService;

		private bool _giftItemSpawnInProgress;

		private EnemyAttractionZoneListenerModel _attractionZoneListenerModel;

		[SerializeField]
		private EnemyItemHolder _giftItemHolder;

		[field: SerializeField]
		public bool IsOccupySpawnPoint { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe MonkeyVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(MonkeyVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(MonkeyVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int CurrentTargetPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.CurrentTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.CurrentTargetPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe bool IsCoinVisible
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.IsCoinVisible. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.IsCoinVisible. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe short CurrentHungerQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.CurrentHungerQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((short*)Ptr)[6];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.CurrentHungerQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[6] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe short AgentVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.AgentVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((short*)Ptr)[8];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyEnemy.AgentVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[8] = value;
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public EnemyType EnemyType => EnemyType.Monkey;

		public int EnemyInstants => base.gameObject.GetHashCode();

		public Vector3? PositionOnFearEnd { get; set; }

		public bool FearCompleted { get; set; }

		public StateMachine<MonkeyStateId, MonkeyStateId, MonkeyEvent> StateMachineForDebug => _fsm;

		public bool IsDespawnAfterFear
		{
			get
			{
				if (_context != null)
				{
					return _context.IsDespawnAfterFear;
				}
				return false;
			}
			set
			{
				if (_context != null)
				{
					_context.IsDespawnAfterFear = value;
				}
			}
		}

		public float CurrentHunger
		{
			get
			{
				return (float)CurrentHungerQuantized * 0.1f;
			}
			private set
			{
				int value2 = Mathf.RoundToInt(value * 10f);
				int max = Mathf.RoundToInt(_enemySettings.MaxHunger * 10f);
				short num = (short)Mathf.Clamp(value2, 0, max);
				if (CurrentHungerQuantized != num)
				{
					CurrentHungerQuantized = num;
					this.OnHungerChanged?.Invoke();
				}
			}
		}

		public float AgentVelocity
		{
			get
			{
				return (float)AgentVelocityQuantized * 0.1f;
			}
			private set
			{
				short num = (short)Mathf.Clamp(Mathf.RoundToInt(Mathf.Max(0f, value) * 10f), 0, 32767);
				if (AgentVelocityQuantized != num)
				{
					AgentVelocityQuantized = num;
				}
			}
		}

		public float HungerPercent
		{
			get
			{
				if (!(_enemySettings.MaxHunger > 0f))
				{
					return 0f;
				}
				return CurrentHunger / _enemySettings.MaxHunger;
			}
		}

		public bool IsFull => CurrentHunger <= _enemySettings.FullHungerThreshold;

		public bool IsStarving => CurrentHunger >= _enemySettings.StarvingHungerThreshold;

		public event Action<IEnemyBehaviour> OnDeath;

		public event Action OnAskingForCoins;

		public event Action OnHungerChanged;

		[Inject]
		public void InjectDependencies(MonkeyEnemyContext context, MonkeyEnemySettings enemySettings, MonkeyMovementSettings movementSettings, MonkeyPlayerInteractionSettings playerInteractionSettings, MonkeyItemInteractionSettings itemInteractionSettings, MonkeyItemGiftSettings itemGiftSettings, MonkeyCombatSettings combatSettings, MonkeyItemSensor itemSensor, EnemyFearListenerModel enemyFearListenerModel, IInstantiator instantiator, IPlayerStateService playerStateService, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, IAudioService audioService, IItemSpawnService itemSpawnService, EnemyAttractionZoneListenerModel attractionZoneListenerModel, EnemyHeadwearModel enemyHeadwearModel)
		{
			_context = context;
			_attractionZoneListenerModel = attractionZoneListenerModel;
			_enemySettings = enemySettings;
			_movementSettings = movementSettings;
			_playerInteractionSettings = playerInteractionSettings;
			_itemInteractionSettings = itemInteractionSettings;
			_itemGiftSettings = itemGiftSettings;
			_combatSettings = combatSettings;
			_itemSensor = itemSensor;
			_enemyFearListenerModel = enemyFearListenerModel;
			_instantiator = instantiator;
			_playerStateService = playerStateService;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_enemyHeadwearModel = enemyHeadwearModel;
			_audioService = audioService;
			_itemSpawnService = itemSpawnService;
		}

		public void ResetGiftItemSpawnState()
		{
			_giftItemSpawnInProgress = false;
		}

		public void TryAttachGiftedItem(IItem item)
		{
			if (base.HasStateAuthority && !(item?.NetworkObject == null) && item.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component))
			{
				_giftItemHolder.TryGrab(component);
			}
		}

		public void ReleaseGiftedItemGrab()
		{
			_giftItemHolder.ReleaseGrab();
		}

		public bool TryActivateGiftedItemIfEligible()
		{
			if (!base.HasStateAuthority || VisualState != MonkeyVisualState.GivingItem)
			{
				return false;
			}
			if (_context.GiftActivateItemChance <= 0f || UnityEngine.Random.value > _context.GiftActivateItemChance)
			{
				return false;
			}
			return TryActivateGiftedItem();
		}

		private bool TryActivateGiftedItem()
		{
			if (_context.GiftedItem == null || _context.GiftedItem.IsDespawned)
			{
				return false;
			}
			if (!(_context.GiftedItem is NetworkBehaviour networkBehaviour))
			{
				return false;
			}
			InteractableBase componentInChildren = networkBehaviour.GetComponentInChildren<InteractableBase>();
			if (componentInChildren == null || !componentInChildren.IsInteractable)
			{
				return false;
			}
			ReleaseGiftedItemGrab();
			componentInChildren.Interact();
			_context.DetachGiftedItem();
			return true;
		}

		private StateMachine<MonkeyStateId, MonkeyStateId, MonkeyEvent> BuildFsm()
		{
			StateMachine<MonkeyStateId, MonkeyStateId, MonkeyEvent> stateMachine = new StateMachine<MonkeyStateId, MonkeyStateId, MonkeyEvent>();
			stateMachine.AddState(MonkeyStateId.Wandering, _instantiator.Instantiate<MonkeyWanderingState>());
			stateMachine.AddState(MonkeyStateId.PlayerInteraction, BuildPlayerInteractionStateMachine());
			stateMachine.AddState(MonkeyStateId.Combat, BuildCombatStateMachine());
			stateMachine.AddState(MonkeyStateId.ItemInteraction, BuildItemInteractionStateMachine());
			stateMachine.AddState(MonkeyStateId.Stun, _instantiator.Instantiate<MonkeyStunState>());
			stateMachine.AddState(MonkeyStateId.Fear, _instantiator.Instantiate<MonkeyFearState>());
			stateMachine.AddState(MonkeyStateId.LastChance, _instantiator.Instantiate<MonkeyLastChanceState>());
			stateMachine.AddState(MonkeyStateId.Cauldroned, _instantiator.Instantiate<MonkeyCauldronedState>());
			stateMachine.AddState(MonkeyStateId.AttractionInvestigate, _instantiator.Instantiate<MonkeyAttractionInvestigateState>());
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetAcquired, MonkeyStateId.Wandering, MonkeyStateId.PlayerInteraction);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetAcquired, MonkeyStateId.Stun, MonkeyStateId.PlayerInteraction);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnCombatRequired, MonkeyStateId.Wandering, MonkeyStateId.Combat);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnCombatRequired, MonkeyStateId.PlayerInteraction, MonkeyStateId.Combat);
			stateMachine.AddTransition(MonkeyStateId.PlayerInteraction, MonkeyStateId.LastChance, (Transition<MonkeyStateId> transition) => _context.HasTarget && IsStarving);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnCombatRequired, MonkeyStateId.LastChance, MonkeyStateId.Combat);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetLost, MonkeyStateId.PlayerInteraction, MonkeyStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetLost, MonkeyStateId.LastChance, MonkeyStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetLost, MonkeyStateId.Combat, MonkeyStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetLost, MonkeyStateId.Stun, MonkeyStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnItemAcquired, MonkeyStateId.Wandering, MonkeyStateId.ItemInteraction, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnItemAcquired, MonkeyStateId.PlayerInteraction, MonkeyStateId.ItemInteraction, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnItemConsumed, MonkeyStateId.ItemInteraction, MonkeyStateId.Wandering);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnItemLost, MonkeyStateId.ItemInteraction, MonkeyStateId.Wandering);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnRecovered, MonkeyStateId.Stun, MonkeyStateId.Combat, (Transition<MonkeyStateId> transition) => _context.HasTarget && (IsStarving || _context.IsDamageAggro));
			stateMachine.AddTriggerTransition(MonkeyEvent.OnRecovered, MonkeyStateId.Stun, MonkeyStateId.PlayerInteraction, (Transition<MonkeyStateId> transition) => _context.HasTarget && !IsFull);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnRecovered, MonkeyStateId.Stun, MonkeyStateId.Wandering, (Transition<MonkeyStateId> transition) => !_context.HasTarget || IsFull);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnFear, MonkeyStateId.Wandering, MonkeyStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnFear, MonkeyStateId.PlayerInteraction, MonkeyStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnFear, MonkeyStateId.ItemInteraction, MonkeyStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnFear, MonkeyStateId.Stun, MonkeyStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetLost, MonkeyStateId.Fear, MonkeyStateId.Wandering);
			stateMachine.AddTriggerTransitionFromAny(MonkeyEvent.OnDamage, MonkeyStateId.Stun, (Transition<MonkeyStateId> transition) => !_context.IsDead, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransitionFromAny(MonkeyEvent.OnCauldroned, MonkeyStateId.Cauldroned, (Transition<MonkeyStateId> transition) => !_context.IsDead, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnRecovered, MonkeyStateId.Cauldroned, MonkeyStateId.Combat, (Transition<MonkeyStateId> transition) => _context.HasTarget && IsStarving);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnRecovered, MonkeyStateId.Cauldroned, MonkeyStateId.PlayerInteraction, (Transition<MonkeyStateId> transition) => _context.HasTarget && !IsFull);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnRecovered, MonkeyStateId.Cauldroned, MonkeyStateId.Wandering, (Transition<MonkeyStateId> transition) => !_context.HasTarget || IsFull);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnAttractionZoneEntered, MonkeyStateId.Wandering, MonkeyStateId.AttractionInvestigate, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnAttractionZoneExited, MonkeyStateId.AttractionInvestigate, MonkeyStateId.Wandering, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnTargetAcquired, MonkeyStateId.AttractionInvestigate, MonkeyStateId.PlayerInteraction);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnCombatRequired, MonkeyStateId.AttractionInvestigate, MonkeyStateId.Combat);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnItemAcquired, MonkeyStateId.AttractionInvestigate, MonkeyStateId.ItemInteraction, null, null, null, forceInstantly: true);
			stateMachine.AddTriggerTransition(MonkeyEvent.OnFear, MonkeyStateId.AttractionInvestigate, MonkeyStateId.Fear, null, null, null, forceInstantly: true);
			stateMachine.SetStartState(MonkeyStateId.Wandering);
			return stateMachine;
		}

		private HybridStateMachine<MonkeyStateId, MonkeyPlayerInteractionStateId, MonkeyEvent> BuildPlayerInteractionStateMachine()
		{
			HybridStateMachine<MonkeyStateId, MonkeyPlayerInteractionStateId, MonkeyEvent> hybridStateMachine = new HybridStateMachine<MonkeyStateId, MonkeyPlayerInteractionStateId, MonkeyEvent>();
			hybridStateMachine.AddState(MonkeyPlayerInteractionStateId.InteractionChasing, _instantiator.Instantiate<MonkeyInteractionChasingState>());
			hybridStateMachine.AddState(MonkeyPlayerInteractionStateId.RunAround, _instantiator.Instantiate<MonkeyRunAroundState>());
			hybridStateMachine.AddState(MonkeyPlayerInteractionStateId.InteractingWaiting, _instantiator.Instantiate<MonkeyInteractingWaitingState>());
			hybridStateMachine.AddState(MonkeyPlayerInteractionStateId.FakeAttacking, _instantiator.Instantiate<MonkeyFakeAttackingState>());
			hybridStateMachine.AddState(MonkeyPlayerInteractionStateId.SelectingNextInteraction, delegate
			{
				SelectNextPlayerInteractionState();
			}, null, null, null, needsExitTime: false, isGhostState: true);
			hybridStateMachine.AddTransition(MonkeyPlayerInteractionStateId.InteractionChasing, MonkeyPlayerInteractionStateId.RunAround, (Transition<MonkeyPlayerInteractionStateId> transition) => _context.GetDistanceToTarget() <= _playerInteractionSettings.InteractionCatchUpDistance);
			AddPlayerInteractionSelectionRoute(hybridStateMachine, MonkeyPlayerInteractionStateId.RunAround);
			AddPlayerInteractionSelectionRoute(hybridStateMachine, MonkeyPlayerInteractionStateId.InteractingWaiting);
			AddPlayerInteractionSelectionRoute(hybridStateMachine, MonkeyPlayerInteractionStateId.FakeAttacking);
			hybridStateMachine.AddTransition(MonkeyPlayerInteractionStateId.SelectingNextInteraction, MonkeyPlayerInteractionStateId.InteractionChasing, (Transition<MonkeyPlayerInteractionStateId> transition) => IsStarving);
			hybridStateMachine.AddTransition(MonkeyPlayerInteractionStateId.SelectingNextInteraction, MonkeyPlayerInteractionStateId.RunAround, (Transition<MonkeyPlayerInteractionStateId> transition) => _context.NextPlayerInteractionState == MonkeyPlayerInteractionStateId.RunAround);
			hybridStateMachine.AddTransition(MonkeyPlayerInteractionStateId.SelectingNextInteraction, MonkeyPlayerInteractionStateId.InteractingWaiting, (Transition<MonkeyPlayerInteractionStateId> transition) => _context.NextPlayerInteractionState == MonkeyPlayerInteractionStateId.InteractingWaiting);
			hybridStateMachine.AddTransition(MonkeyPlayerInteractionStateId.SelectingNextInteraction, MonkeyPlayerInteractionStateId.FakeAttacking, (Transition<MonkeyPlayerInteractionStateId> transition) => _context.NextPlayerInteractionState == MonkeyPlayerInteractionStateId.FakeAttacking);
			hybridStateMachine.SetStartState(MonkeyPlayerInteractionStateId.InteractionChasing);
			return hybridStateMachine;
		}

		private HybridStateMachine<MonkeyStateId, MonkeyCombatStateId, MonkeyEvent> BuildCombatStateMachine()
		{
			HybridStateMachine<MonkeyStateId, MonkeyCombatStateId, MonkeyEvent> hybridStateMachine = new HybridStateMachine<MonkeyStateId, MonkeyCombatStateId, MonkeyEvent>();
			hybridStateMachine.AddState(MonkeyCombatStateId.CombatChasing, _instantiator.Instantiate<MonkeyCombatChasingState>());
			hybridStateMachine.AddState(MonkeyCombatStateId.Attacking, _instantiator.Instantiate<MonkeyAttackingState>());
			hybridStateMachine.AddTransition(MonkeyCombatStateId.CombatChasing, MonkeyCombatStateId.Attacking, (Transition<MonkeyCombatStateId> transition) => _context.GetDistanceToTarget() <= _combatSettings.DistanceToAttack);
			hybridStateMachine.AddTriggerTransition(MonkeyEvent.OnCombatRequired, MonkeyCombatStateId.Attacking, MonkeyCombatStateId.CombatChasing);
			hybridStateMachine.SetStartState(MonkeyCombatStateId.CombatChasing);
			return hybridStateMachine;
		}

		private HybridStateMachine<MonkeyStateId, MonkeyItemInteractionStateId, MonkeyEvent> BuildItemInteractionStateMachine()
		{
			HybridStateMachine<MonkeyStateId, MonkeyItemInteractionStateId, MonkeyEvent> hybridStateMachine = new HybridStateMachine<MonkeyStateId, MonkeyItemInteractionStateId, MonkeyEvent>();
			hybridStateMachine.AddState(MonkeyItemInteractionStateId.MoveToItem, _instantiator.Instantiate<MonkeyMoveToItemState>());
			hybridStateMachine.AddState(MonkeyItemInteractionStateId.Eating, _instantiator.Instantiate<MonkeyEatingState>());
			hybridStateMachine.AddState(MonkeyItemInteractionStateId.GivingItem, _instantiator.Instantiate<MonkeyGivingItemState>());
			hybridStateMachine.AddState(MonkeyItemInteractionStateId.CoinHideout, _instantiator.Instantiate<MonkeyCoinHideoutState>());
			hybridStateMachine.AddTriggerTransition(MonkeyEvent.OnItemReached, MonkeyItemInteractionStateId.MoveToItem, MonkeyItemInteractionStateId.Eating);
			hybridStateMachine.AddTriggerTransition(MonkeyEvent.OnItemGiveRequired, MonkeyItemInteractionStateId.Eating, MonkeyItemInteractionStateId.GivingItem);
			hybridStateMachine.AddTriggerTransition(MonkeyEvent.OnItemGiveCompleted, MonkeyItemInteractionStateId.GivingItem, MonkeyItemInteractionStateId.CoinHideout);
			hybridStateMachine.AddTriggerTransition(MonkeyEvent.OnCoinHideoutRequired, MonkeyItemInteractionStateId.Eating, MonkeyItemInteractionStateId.CoinHideout);
			hybridStateMachine.SetStartState(MonkeyItemInteractionStateId.MoveToItem);
			return hybridStateMachine;
		}

		private void AddPlayerInteractionSelectionRoute(HybridStateMachine<MonkeyStateId, MonkeyPlayerInteractionStateId, MonkeyEvent> playerInteraction, MonkeyPlayerInteractionStateId from)
		{
			playerInteraction.AddTriggerTransition(MonkeyEvent.OnInteractionStepCompleted, from, MonkeyPlayerInteractionStateId.SelectingNextInteraction);
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority && _fsm == null)
			{
				CurrentHunger = _enemySettings.MaxHunger * _enemySettings.StartHungerPercent;
				_context.TargetDetector.OnTargetDetectedInRange += OnTargetDetectedHandler;
				_context.CollectItemFellEvent.OnItemFell += OnItemFellHandler;
				_context.Damageable.OnDamaged += OnDamagedHandler;
				_context.AnimationEvent.OnAttack += OnAttackAnimationEvent;
				_context.AnimationEvent.OnFakeAttack += OnFakeAttackAnimationEvent;
				_context.AnimationEvent.OnGrab += OnGrabAnimationEvent;
				_context.AnimationEvent.OnSpawnGiftItem += OnSpawnGiftItemAnimationEvent;
				_context.AnimationEvent.OnStep += OnStepAnimationEvent;
				_context.PlayerStateModel.OnSomePlayerStateChanged += OnPlayerStateChangedHandler;
				_context.PlayersGatesModel.OnPlayerInsideGateChanged += OnPlayerInsideGateChangedHandler;
				_context.StatHealthController.OnEnemyDead += OnEnemyDeadHandler;
				_fsm = BuildFsm();
				_fsm.Init();
			}
			SetupObjectByAuthority();
			if (base.HasStateAuthority && _context.Agent != null)
			{
				_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
				_context.Agent.Warp(base.transform.position);
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			_context.ValidateRequiredReferences();
			FearCompleted = false;
			_context.IsDead = false;
			_context.IsFearing = false;
			_context.EnableMovement();
			_context.CreateFmodInstances();
			_enemyFearListenerModel.RegisterFearListener(EnemyType.Monkey, base.gameObject.GetHashCode(), this);
			_attractionZoneListenerModel.RegisterListener(EnemyType, EnemyInstants, this);
			SetupObjectByAuthority();
			if (base.HasStateAuthority)
			{
				_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
				_context.Agent.Warp(base.transform.position);
			}
			if (base.HasStateAuthority)
			{
				CurrentHunger = _enemySettings.MaxHunger * _enemySettings.StartHungerPercent;
				_context.TargetDetector.OnTargetDetectedInRange += OnTargetDetectedHandler;
				_context.CollectItemFellEvent.OnItemFell += OnItemFellHandler;
				_context.Damageable.OnDamaged += OnDamagedHandler;
				_context.AnimationEvent.OnAttack += OnAttackAnimationEvent;
				_context.AnimationEvent.OnFakeAttack += OnFakeAttackAnimationEvent;
				_context.AnimationEvent.OnGrab += OnGrabAnimationEvent;
				_context.AnimationEvent.OnSpawnGiftItem += OnSpawnGiftItemAnimationEvent;
				_context.AnimationEvent.OnStep += OnStepAnimationEvent;
				_context.PlayerStateModel.OnSomePlayerStateChanged += OnPlayerStateChangedHandler;
				_context.PlayersGatesModel.OnPlayerInsideGateChanged += OnPlayerInsideGateChangedHandler;
				_context.StatHealthController.OnEnemyDead += OnEnemyDeadHandler;
				_fsm = BuildFsm();
				_fsm.Init();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			base.Despawned(runner, hasState);
			PositionOnFearEnd = base.transform.position;
			FearCompleted = true;
			_context.IsFearing = false;
			_enemyFearListenerModel.UnregisterFearListener(EnemyType.Monkey, base.gameObject.GetHashCode());
			_context.ClearTargetItem();
			_giftItemHolder.ReleaseGrab();
			_attractionZoneListenerModel.UnregisterListener(EnemyType, EnemyInstants);
			_context.ReleaseFmodInstances();
			if (base.HasStateAuthority)
			{
				_context.TargetDetector.OnTargetDetectedInRange -= OnTargetDetectedHandler;
				_context.CollectItemFellEvent.OnItemFell -= OnItemFellHandler;
				_context.Damageable.OnDamaged -= OnDamagedHandler;
				_context.AnimationEvent.OnAttack -= OnAttackAnimationEvent;
				_context.AnimationEvent.OnFakeAttack -= OnFakeAttackAnimationEvent;
				_context.AnimationEvent.OnGrab -= OnGrabAnimationEvent;
				_context.AnimationEvent.OnSpawnGiftItem -= OnSpawnGiftItemAnimationEvent;
				_context.AnimationEvent.OnStep -= OnStepAnimationEvent;
				_context.PlayerStateModel.OnSomePlayerStateChanged -= OnPlayerStateChangedHandler;
				_context.PlayersGatesModel.OnPlayerInsideGateChanged -= OnPlayerInsideGateChangedHandler;
				_context.StatHealthController.OnEnemyDead -= OnEnemyDeadHandler;
			}
			this.OnDeath?.Invoke(this);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fsm != null && !_context.IsDead)
			{
				AgentVelocity = (_context.Agent.enabled ? _context.Agent.velocity.magnitude : 0f);
				UpdateHunger();
				UpdateCooldown();
				TryAcquireHeldCoin();
				if (_context.IsFearing)
				{
					_fsm.Trigger(MonkeyEvent.OnFear);
				}
				if (_enemyHeadwearModel.IsWearing(base.Object.Id.Raw) && _fsm.ActiveStateName != MonkeyStateId.Cauldroned)
				{
					_fsm.Trigger(MonkeyEvent.OnCauldroned);
				}
				_fsm.OnLogic();
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
		}

		public void BindSpawnPointOccupancy(IEnemySpawnPointOccupancy occupancy)
		{
			_spawnPointOccupancyBinder.Bind(occupancy, base.Object);
		}

		public void ReleaseSpawnPointOccupancy()
		{
			_spawnPointOccupancyBinder.ReleaseBound();
		}

		public void Teleport(Vector3 position)
		{
			if (_context.Agent.enabled && _context.Agent.isOnNavMesh)
			{
				_context.Agent.Warp(position);
			}
		}

		public void TriggerEvent(MonkeyEvent ev)
		{
			_fsm?.Trigger(ev);
		}

		public void OnAttractionZoneRaised(AttractionZoneData zone)
		{
			if (base.HasStateAuthority && _fsm != null && _fsm.ActiveStateName == MonkeyStateId.Wandering && !_context.HasTarget && !_context.IsFearing && !((base.transform.position - zone.Origin).sqrMagnitude > zone.AttractRadius * zone.AttractRadius))
			{
				_context.SetPendingAttractionZone(zone);
				_fsm.Trigger(MonkeyEvent.OnAttractionZoneEntered);
			}
		}

		public void OnAttractionZoneEnded(int zoneId)
		{
		}

		public void SetVisualState(MonkeyVisualState state)
		{
			if (base.HasStateAuthority && VisualState != state)
			{
				VisualState = state;
			}
		}

		public void SetCurrentTargetPlayerId(int playerId)
		{
			if (base.HasStateAuthority && CurrentTargetPlayerId != playerId)
			{
				CurrentTargetPlayerId = playerId;
			}
		}

		public void SetCoinVisible(bool visible)
		{
			if (base.HasStateAuthority && IsCoinVisible != visible)
			{
				IsCoinVisible = visible;
				SetCoinVisibleRpc(visible);
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

		public void ResetStateTimer(float duration)
		{
			_context.StateElapsed = 0f;
			_context.StateDuration = duration;
		}

		public void ResetStateTimerRandom(float minDuration, float maxDuration)
		{
			ResetStateTimer(UnityEngine.Random.Range(minDuration, maxDuration));
		}

		public bool AdvanceStateTimer()
		{
			_context.StateElapsed += GetTickDelta();
			return _context.StateElapsed >= _context.StateDuration;
		}

		private void SetupObjectByAuthority()
		{
			_context.Agent.enabled = base.HasStateAuthority;
		}

		private void SelectNextPlayerInteractionState()
		{
			if (IsStarving)
			{
				_context.NextPlayerInteractionState = MonkeyPlayerInteractionStateId.InteractionChasing;
			}
			else if (CurrentHunger >= _playerInteractionSettings.FakeAttackHungerThreshold)
			{
				_context.NextPlayerInteractionState = SelectWeightedPlayerInteractionState(_playerInteractionSettings.RunAroundSelectionWeight, _playerInteractionSettings.WaitingSelectionWeight, _playerInteractionSettings.FakeAttackSelectionWeight);
			}
			else
			{
				_context.NextPlayerInteractionState = SelectWeightedPlayerInteractionState(_playerInteractionSettings.CalmRunAroundSelectionWeight, _playerInteractionSettings.CalmWaitingSelectionWeight, 0f);
			}
		}

		private MonkeyPlayerInteractionStateId SelectWeightedPlayerInteractionState(float runAroundWeight, float waitingWeight, float fakeAttackWeight)
		{
			runAroundWeight = Mathf.Max(0f, runAroundWeight);
			waitingWeight = Mathf.Max(0f, waitingWeight);
			fakeAttackWeight = Mathf.Max(0f, fakeAttackWeight);
			float num = runAroundWeight + waitingWeight + fakeAttackWeight;
			if (num <= 0f)
			{
				return MonkeyPlayerInteractionStateId.RunAround;
			}
			float num2 = UnityEngine.Random.value * num;
			if (num2 < runAroundWeight)
			{
				return MonkeyPlayerInteractionStateId.RunAround;
			}
			num2 -= runAroundWeight;
			if (num2 < waitingWeight)
			{
				return MonkeyPlayerInteractionStateId.InteractingWaiting;
			}
			return MonkeyPlayerInteractionStateId.FakeAttacking;
		}

		public bool CanEnemyInteractWithTarget()
		{
			return _context.CanEnemyInteractWithTargetPlayer();
		}

		public void LoseInterestInTarget()
		{
			_context.TargetPlayer = PlayerRef.None;
			_context.IsDamageAggro = false;
			SetCurrentTargetPlayerId(-1);
			_context.ChaseElapsed = 0f;
			_context.ChaseCooldownRemaining = _combatSettings.ChaseCooldownDuration;
			CurrentHunger = _enemySettings.MaxHunger * _enemySettings.StartHungerPercent;
			TriggerEvent(MonkeyEvent.OnTargetLost);
		}

		public void AttackRemover(int playerId)
		{
			PlayerRef playerRef = PlayerRef.None;
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer.PlayerId == playerId)
				{
					playerRef = activePlayer;
					break;
				}
			}
			if (playerRef == PlayerRef.None)
			{
				TriggerEvent(MonkeyEvent.OnRecovered);
				return;
			}
			_context.TargetPlayer = playerRef;
			_context.ChaseElapsed = 0f;
			SetCurrentTargetPlayerId(playerId);
			CurrentHunger = _enemySettings.MaxHunger;
			TriggerEvent(MonkeyEvent.OnRecovered);
		}

		public void Feed(float hungerReduction)
		{
			CurrentHunger = Mathf.Max(0f, CurrentHunger - hungerReduction);
			if (IsFull)
			{
				_context.TargetPlayer = PlayerRef.None;
				_context.IsDamageAggro = false;
				SetCurrentTargetPlayerId(-1);
				TriggerEvent(MonkeyEvent.OnTargetLost);
			}
		}

		public void MarkFearCompleted()
		{
			_context.IsFearing = false;
			FearCompleted = true;
		}

		public void RequestDespawnAfterFear()
		{
			if (base.HasStateAuthority)
			{
				_context.SimpleEnemyDeadProcessor.IsNeedToSpawnItem = false;
				_context.ClearTargetItem();
				base.Object.DespawnHierarchy();
			}
		}

		public void RaiseMoveSlowAnimation()
		{
			SetAnimatorTriggerRpc(_moveSlowHash);
		}

		public void RaiseMoveMiddleAnimation()
		{
			SetAnimatorTriggerRpc(_moveMiddleHash);
		}

		public void RaiseRunAnimation()
		{
			SetAnimatorTriggerRpc(_runHash);
		}

		public void RaiseMoveAnimation()
		{
			SetAnimatorTriggerRpc(_moveHash);
		}

		public void RaiseIdleAnimation()
		{
			SetAnimatorTriggerRpc(_idleHash);
		}

		public void RaiseHitAnimation()
		{
			SetAnimatorTriggerRpc(_hitHash);
		}

		public void RaiseGrabAnimation()
		{
			SetAnimatorTriggerRpc(_grabHash);
		}

		public void RaiseAttackAnimation()
		{
			SetAnimatorTriggerRpc(_attackHash);
		}

		public void RaiseFakeAttackAnimation()
		{
			SetAnimatorTriggerRpc(_fakeAttackHash);
		}

		public void RaiseCoinRequestAnimation()
		{
			SetAnimatorTriggerRpc(SelectWeightedCoinRequestAnimationHash());
			PlayCoinRequestSoundRpc();
			this.OnAskingForCoins?.Invoke();
		}

		private int SelectWeightedCoinRequestAnimationHash()
		{
			float num = Mathf.Max(0f, _playerInteractionSettings.CoinThinkAnimationWeight);
			float num2 = Mathf.Max(0f, _playerInteractionSettings.CoinRequestAnimationWeight);
			float num3 = num + num2;
			if (num3 <= 0f)
			{
				return _coinRequestHash;
			}
			if (!(UnityEngine.Random.value * num3 < num))
			{
				return _coinRequestHash;
			}
			return _coinThinkHash;
		}

		private bool TryRollItemGift(out NetworkBehaviour giftItemPrefab, out bool canActivateItem, out float activateItemChance, out float activateItemDelay, out Vector3 giftSpawnOffset)
		{
			giftItemPrefab = null;
			canActivateItem = false;
			activateItemChance = 0f;
			activateItemDelay = 0f;
			giftSpawnOffset = Vector3.zero;
			if (_itemGiftSettings.ItemGiftChance <= 0f || UnityEngine.Random.value > _itemGiftSettings.ItemGiftChance)
			{
				return false;
			}
			return TrySelectRandomGiftItem(out giftItemPrefab, out canActivateItem, out activateItemChance, out activateItemDelay, out giftSpawnOffset);
		}

		private bool TrySelectRandomGiftItem(out NetworkBehaviour giftItemPrefab, out bool canActivateItem, out float activateItemChance, out float activateItemDelay, out Vector3 giftSpawnOffset)
		{
			giftItemPrefab = null;
			canActivateItem = false;
			activateItemChance = 0f;
			activateItemDelay = 0f;
			giftSpawnOffset = Vector3.zero;
			List<MonkeyItemGiftEntry> giftItems = _itemGiftSettings.GiftItems;
			if (giftItems == null || giftItems.Count == 0)
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < giftItems.Count; i++)
			{
				if (giftItems[i].ItemPrefab != null)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return false;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			int num3 = 0;
			for (int j = 0; j < giftItems.Count; j++)
			{
				if (!(giftItems[j].ItemPrefab == null))
				{
					if (num3 == num2)
					{
						MonkeyItemGiftEntry monkeyItemGiftEntry = giftItems[j];
						giftItemPrefab = monkeyItemGiftEntry.ItemPrefab;
						canActivateItem = monkeyItemGiftEntry.IsCanActivateItem;
						activateItemChance = monkeyItemGiftEntry.ActivateItemChance;
						activateItemDelay = monkeyItemGiftEntry.ActivateItemDelay;
						giftSpawnOffset = monkeyItemGiftEntry.GiftSpawnOffset;
						return true;
					}
					num3++;
				}
			}
			return false;
		}

		public void RaiseTakeCoinSound()
		{
			PlayTakeCoinSoundRpc();
		}

		public void RaiseHitSound()
		{
			PlayHitSoundRpc();
		}

		public void RaiseMoveAngrySound()
		{
			PlayMoveAngrySoundRpc();
		}

		public void StopMoveAngrySound()
		{
			StopMoveAngrySoundRpc();
		}

		public void RaiseAttackJumpAnimation(float normalizedTime)
		{
			PlayAttackStateRpc(normalizedTime);
		}

		public void CompleteEatingInteraction()
		{
			if (!base.HasStateAuthority || _context.EatingInteractionCompleted)
			{
				return;
			}
			if (!_context.EatingCoinConsumed)
			{
				TryConsumeCoinDuringEating();
			}
			if (!_context.EatingCoinConsumed)
			{
				_context.ResetEatingInteractionState();
				TriggerEvent(MonkeyEvent.OnItemLost);
				return;
			}
			_context.EatingInteractionCompleted = true;
			SetCoinVisible(visible: false);
			if (_context.CoinSourcePlayer != PlayerRef.None)
			{
				if (TryRollItemGift(out var giftItemPrefab, out var canActivateItem, out var activateItemChance, out var activateItemDelay, out var giftSpawnOffset))
				{
					_context.SetPendingGiftItem(giftItemPrefab, canActivateItem, activateItemChance, activateItemDelay, giftSpawnOffset);
					_context.ResetEatingInteractionState();
					TriggerEvent(MonkeyEvent.OnItemGiveRequired);
				}
				else
				{
					_context.ResetEatingInteractionState();
					TriggerEvent(MonkeyEvent.OnCoinHideoutRequired);
				}
			}
			else
			{
				_context.ClearCoinSourcePlayer();
				_context.ResetEatingInteractionState();
				TriggerEvent(MonkeyEvent.OnItemConsumed);
			}
		}

		public void TryConsumeCoinDuringEating()
		{
			if (!_context.EatingCoinConsumed)
			{
				if (_context.IsTargetItemHeld())
				{
					_context.TryCaptureCoinSourcePlayerFromTargetItem();
				}
				if (TryConsumeCoinDuringEatingInternal(out var _))
				{
					_context.EatingCoinConsumed = true;
					SetCoinVisible(visible: true);
					ResetStateTimer(_itemInteractionSettings.CoinPocketAnimationDuration);
				}
			}
		}

		private bool TryConsumeCoinDuringEatingInternal(out bool wasHeldByPlayer)
		{
			wasHeldByPlayer = _context.IsTargetItemHeld();
			float maxDistance = _movementSettings.StoppingDistance * _itemInteractionSettings.ConsumeDistanceStoppingDistanceMultiplier;
			float hungerReduction;
			bool num = (wasHeldByPlayer ? _context.TryTakeTargetCoinFromHand(maxDistance, 100f, out hungerReduction) : _context.TryConsumeTargetItem(maxDistance, 100f, out hungerReduction));
			if (num)
			{
				Feed(hungerReduction);
			}
			return num;
		}

		private void UpdateHunger()
		{
			if (VisualState != MonkeyVisualState.Eating && VisualState != MonkeyVisualState.GivingItem && IsCoinVisible)
			{
				SetCoinVisible(visible: false);
			}
			if (IsFull || _context.HasTarget)
			{
				CurrentHunger = Mathf.Clamp(CurrentHunger + _enemySettings.HungerIncreaseRate * GetTickDelta(), 0f, _enemySettings.MaxHunger);
			}
		}

		private void UpdateCooldown()
		{
			if (!(_context.ChaseCooldownRemaining <= 0f))
			{
				_context.ChaseCooldownRemaining = Mathf.Max(0f, _context.ChaseCooldownRemaining - GetTickDelta());
			}
		}

		private void TryAcquireHeldCoin()
		{
			if (!IsStarving && VisualState != MonkeyVisualState.Attacking && VisualState != MonkeyVisualState.Eating && _itemSensor.TryAcquireHeldCoin())
			{
				TriggerEvent(MonkeyEvent.OnItemAcquired);
			}
		}

		private void OnTargetDetectedHandler(PlayerRef targetPlayer)
		{
			if (base.HasStateAuthority && !_context.HasTarget && !IsFull && !_context.IsFearing && _context.IsPlayerEligibleForTarget(targetPlayer) && _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(targetPlayer.PlayerId))
			{
				_context.TargetPlayer = targetPlayer;
				_context.ChaseElapsed = 0f;
				SetCurrentTargetPlayerId(targetPlayer.PlayerId);
				TriggerEvent(IsStarving ? MonkeyEvent.OnCombatRequired : MonkeyEvent.OnTargetAcquired);
			}
		}

		private void OnItemFellHandler(IItem item)
		{
			if (base.HasStateAuthority && VisualState != MonkeyVisualState.Attacking && _itemSensor.TryAcceptFallenItem(item, base.Runner.Tick, base.Runner.TickRate))
			{
				TriggerEvent(MonkeyEvent.OnItemAcquired);
			}
		}

		private void OnDamagedHandler(DamageData _)
		{
			if (base.HasStateAuthority && !_context.IsDead)
			{
				if (_context.TryGetNearestAlivePlayer(out var nearestPlayer))
				{
					_context.TargetPlayer = nearestPlayer;
					_context.ChaseElapsed = 0f;
					_context.IsDamageAggro = true;
					SetCurrentTargetPlayerId(nearestPlayer.PlayerId);
				}
				TriggerEvent(MonkeyEvent.OnDamage);
			}
		}

		private void OnEnemyDeadHandler()
		{
			_context.IsDead = true;
			_context.ClearTargetItem();
			ReleaseGiftedItemGrab();
		}

		private void OnPlayerStateChangedHandler(PlayerStateData data)
		{
			if (base.HasStateAuthority && _context.HasTarget && _context.TargetPlayer.PlayerId == data.PlayerId && !_playerStateService.IsPlayerAlive(data.PlayerId))
			{
				LoseInterestInTarget();
			}
		}

		private void OnPlayerInsideGateChangedHandler(int ownerId, bool playerInsideGate)
		{
			if (_context.HasTarget && _context.TargetPlayer.PlayerId == ownerId && !playerInsideGate)
			{
				LoseInterestInTarget();
			}
		}

		private void OnAttackAnimationEvent()
		{
			if (!base.HasStateAuthority || !_context.TryGetPlayerDamageable(out var damageable) || _enemyHeadwearModel.IsWearing(base.Object.Id.Raw) || !_context.HasTarget || !_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(_context.TargetPlayer.PlayerId))
			{
				return;
			}
			PlayHitSoundRpc();
			PlayHitParticleRpc();
			if (!(_context.GetDistanceToTarget() > _combatSettings.DistanceToAttack))
			{
				Vector3 normalized = (damageable.Transform.position - base.transform.position).normalized;
				normalized.y = 0f;
				if (!(Vector3.Angle(base.transform.forward, normalized) > _combatSettings.AttackAngle))
				{
					damageable.Damage(new DamageData
					{
						Damage = _context.GetStatValue(EntityStatType.Damage),
						Direction = damageable.Transform.position - base.transform.position,
						Force = _enemySettings.ForceStrength,
						ForceMode = ForceMode.Impulse,
						Source = DamageDataSourceExtensions.ForEnemyAttack(base.transform, EnemyType.Monkey.ToString(), DamageType.Melee)
					});
				}
			}
		}

		private void OnFakeAttackAnimationEvent()
		{
			if (base.HasStateAuthority)
			{
				PlayHitSoundRpc();
			}
		}

		private void OnGrabAnimationEvent()
		{
			if (base.HasStateAuthority && VisualState == MonkeyVisualState.Eating)
			{
				TryConsumeCoinDuringEating();
			}
		}

		private void OnSpawnGiftItemAnimationEvent()
		{
			if (base.HasStateAuthority && VisualState == MonkeyVisualState.GivingItem && _context.GiftedItem == null && !_giftItemSpawnInProgress)
			{
				SpawnGiftItemAsync().Forget();
			}
		}

		private async UniTaskVoid SpawnGiftItemAsync()
		{
			NetworkBehaviour pendingGiftItemPrefab = _context.PendingGiftItemPrefab;
			if (pendingGiftItemPrefab == null)
			{
				return;
			}
			_giftItemSpawnInProgress = true;
			Vector3 pendingGiftSpawnOffset = _context.PendingGiftSpawnOffset;
			try
			{
				_context.ApplyGiftGrabPointOffset(pendingGiftSpawnOffset);
				Transform transform = _context.GiftItemParent.transform;
				NetworkBehaviour networkBehaviour = await _itemSpawnService.SpawnItem(pendingGiftItemPrefab, transform.position, transform.rotation);
				if (networkBehaviour == null || !networkBehaviour.TryGetComponent<IItem>(out var component))
				{
					_context.ResetGiftGrabPointOffset();
					return;
				}
				bool pendingGiftCanActivateItem = _context.PendingGiftCanActivateItem;
				float pendingGiftActivateItemChance = _context.PendingGiftActivateItemChance;
				float pendingGiftActivateItemDelay = _context.PendingGiftActivateItemDelay;
				_context.ClearPendingGiftItem();
				TryAttachGiftedItem(component);
				_context.SetGiftedItem(component, _itemGiftSettings.ItemScaleDuration, _itemGiftSettings.ItemScaleEase);
				_context.BeginGiftItemActivationTracking(pendingGiftCanActivateItem, pendingGiftActivateItemChance, pendingGiftActivateItemDelay);
			}
			finally
			{
				_giftItemSpawnInProgress = false;
			}
		}

		private void OnStepAnimationEvent()
		{
			if (AgentVelocity > 0.1f)
			{
				_audioService.PlayOneShot(_context.StepSoundReference, _context.SoundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 624534644u)]
		private void SetAnimatorTriggerRpc([RpcPayload(4)] int animationHash)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(624534644u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::SetAnimatorTriggerRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(animationHash, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_context.Animator.SetTrigger(animationHash);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 939711574u)]
		private void PlayHitSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(939711574u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::PlayHitSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.StartInstanceWith3DAttributes(_context.HitSoundInstance, _context.SoundSourceBehaviour);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3476780353u)]
		private void PlayMoveAngrySoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3476780353u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::PlayMoveAngrySoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.MoveAngryInstance.start();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1001290209u)]
		private void StopMoveAngrySoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1001290209u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::StopMoveAngrySoundRpc()", invokeInfo, PlayerRef.None);
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
			_context.MoveAngryInstance.stop(STOP_MODE.ALLOWFADEOUT);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1990567966u)]
		private void PlayAttackStateRpc([RpcPayload(4)] float normalizedTime)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1990567966u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::PlayAttackStateRpc(System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(normalizedTime, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_context.Animator.Play(_attackStateHash, -1, normalizedTime);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1146887949u)]
		private void PlayHitParticleRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1146887949u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::PlayHitParticleRpc()", invokeInfo, PlayerRef.None);
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
			_context.HitParticle.Play();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 85978065u)]
		private void PlayCoinRequestSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(85978065u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::PlayCoinRequestSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.StartInstanceWith3DAttributes(_context.CoinRequestSoundInstance, _context.SoundSourceBehaviour);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3841422251u)]
		private void PlayTakeCoinSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3841422251u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::PlayTakeCoinSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.StartInstanceWith3DAttributes(_context.TakeCoinSoundInstance, _context.SoundSourceBehaviour);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 233248878u)]
		private void SetCoinVisibleRpc([RpcPayload(4)] bool visible)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(visible);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(233248878u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyEnemy.MonkeyEnemy::SetCoinVisibleRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(visible);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_context.CoinObject.SetActive(visible);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
			CurrentTargetPlayerId = _CurrentTargetPlayerId;
			IsCoinVisible = _IsCoinVisible;
			CurrentHungerQuantized = _CurrentHungerQuantized;
			AgentVelocityQuantized = _AgentVelocityQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
			_CurrentTargetPlayerId = CurrentTargetPlayerId;
			_IsCoinVisible = IsCoinVisible;
			_CurrentHungerQuantized = CurrentHungerQuantized;
			_AgentVelocityQuantized = AgentVelocityQuantized;
		}

		[NetworkRpcWeavedInvoker(624534644u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetAnimatorTriggerRpc_0040Invoker624534644([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).SetAnimatorTriggerRpc(value);
		}

		[NetworkRpcWeavedInvoker(939711574u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHitSoundRpc_0040Invoker939711574([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).PlayHitSoundRpc();
		}

		[NetworkRpcWeavedInvoker(3476780353u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayMoveAngrySoundRpc_0040Invoker3476780353([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).PlayMoveAngrySoundRpc();
		}

		[NetworkRpcWeavedInvoker(1001290209u)]
		[Preserve]
		[WeaverGenerated]
		protected static void StopMoveAngrySoundRpc_0040Invoker1001290209([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).StopMoveAngrySoundRpc();
		}

		[NetworkRpcWeavedInvoker(1990567966u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayAttackStateRpc_0040Invoker1990567966([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out float value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).PlayAttackStateRpc(value);
		}

		[NetworkRpcWeavedInvoker(1146887949u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHitParticleRpc_0040Invoker1146887949([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).PlayHitParticleRpc();
		}

		[NetworkRpcWeavedInvoker(85978065u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayCoinRequestSoundRpc_0040Invoker85978065([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).PlayCoinRequestSoundRpc();
		}

		[NetworkRpcWeavedInvoker(3841422251u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayTakeCoinSoundRpc_0040Invoker3841422251([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).PlayTakeCoinSoundRpc();
		}

		[NetworkRpcWeavedInvoker(233248878u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetCoinVisibleRpc_0040Invoker233248878([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyEnemy)context.TargetBehaviour).SetCoinVisibleRpc(value);
		}
	}
}
