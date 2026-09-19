using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.DamageableTrackModule.Scripts;
using Features.EntitiesSoundOcclusionModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	[NetworkBehaviourWeaved(14)]
	public class MonkeyPorterEnemy : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface, IDamageable, IEnemyTrackable
	{
		private const float NetworkQuantizeFactor = 10f;

		private const float NetworkDequantizeFactor = 0.1f;

		private const float CART_FORWARD_EPSILON = 0.0001f;

		private const float CART_DROP_GRAB_MAX_AGE_SECONDS = 3f;

		private static readonly int _takeTriggerHash = Animator.StringToHash("Take");

		private static readonly int _throwTriggerHash = Animator.StringToHash("Throw");

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VisualState", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private MonkeyPorterVisualState _VisualState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Phase", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private MonkeyPorterStateId _Phase;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CarriedItemId", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _CarriedItemId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CartId", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _CartId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsCartFollowSuspended", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsCartFollowSuspended;

		[WeaverGenerated]
		[DefaultForProperty("AgentVelocityQuantized", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _AgentVelocityQuantized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Health", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Health;

		[WeaverGenerated]
		[DefaultForProperty("IsHealthInitialized", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsHealthInitialized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HomePosition", 8, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _HomePosition;

		[WeaverGenerated]
		[DefaultForProperty("IsHomeInitialized", 11, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsHomeInitialized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("FloorAnchor", 12, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _FloorAnchor;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsFloorAnchorInitialized", 13, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsFloorAnchorInitialized;

		private StateMachine<MonkeyPorterStateId, MonkeyPorterStateId, MonkeyPorterEvent> _fsm;

		private MonkeyPorterContext _context;

		private MonkeyPorterSettings _settings;

		private NetworkObject _cartObject;

		private PorterCartIntake _cartIntake;

		private MonkeyPorterCartFollower _cartFollower;

		private PorterCartSteering _cartSteering;

		private PorterBlockDetector _blockDetector;

		private PorterThreatDetector _threatDetector;

		private PorterCoverFinder _coverFinder;

		private PorterNoiseEmitter _noiseEmitter;

		private SimplePointGrabable _launchingItem;

		private Vector3 _launchStartPosition;

		private Vector3 _launchEndPosition;

		private float _launchElapsed;

		private SimplePointGrabable _carriedGrabable;

		private SimplePointGrabable _orphanCartItem;

		private Vector3 _orphanCartItemStart;

		private float _orphanCartItemElapsed;

		private RigidbodyInterpolation _carriedItemInterpolation;

		private bool _isTakingIntoCart;

		private float _takeElapsed;

		private bool _agentPlacedOnce;

		private bool _isDead;

		private bool _isHiddenInCover;

		private float _fleeCooldownLeft;

		private int _lastDamageDealerId;

		private Transform _cachedTransform;

		private IEnemyTrackingService _enemyTrackingService;

		private IEntitiesSoundOcclusionService _entitiesSoundOcclusionService;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe MonkeyPorterVisualState VisualState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(MonkeyPorterVisualState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.VisualState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(MonkeyPorterVisualState*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe MonkeyPorterStateId Phase
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (MonkeyPorterStateId)Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe NetworkId CarriedItemId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.CarriedItemId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 2);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.CarriedItemId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe NetworkId CartId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.CartId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 3);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.CartId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe bool IsCartFollowSuspended
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsCartFollowSuspended. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 4);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsCartFollowSuspended. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		private unsafe short AgentVelocityQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.AgentVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((short*)Ptr)[10];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.AgentVelocityQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[10] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 1)]
		public unsafe float Health
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.Health. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 6);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.Health. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 6) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(7, 1)]
		private unsafe bool IsHealthInitialized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsHealthInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 7);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsHealthInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 7) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(8, 3)]
		public unsafe Vector3 HomePosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.HomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 8);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.HomePosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 8) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(11, 1)]
		private unsafe bool IsHomeInitialized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsHomeInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 11);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsHomeInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 11) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(12, 1)]
		public unsafe float FloorAnchor
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.FloorAnchor. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 12);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.FloorAnchor. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 12) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(13, 1)]
		public unsafe bool IsFloorAnchorInitialized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsFloorAnchorInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 13);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterEnemy.IsFloorAnchorInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 13) = new NetworkBool(value);
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public Transform Transform => _cachedTransform ?? (_cachedTransform = base.transform);

		public Transform CartSocket => _context.CartSocket;

		public bool IsActive => !_isDead;

		public bool IsTrackable
		{
			get
			{
				if (!_isDead)
				{
					return !_isHiddenInCover;
				}
				return false;
			}
		}

		public bool IsFullHealth => Health >= _settings.MaxHealth;

		public List<DamageableTag> DamageableTags { get; private set; } = new List<DamageableTag>();

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

		public PorterBlockDetector BlockDetector => _blockDetector;

		public PorterThreatDetector ThreatDetector => _threatDetector;

		public PorterNoiseEmitter NoiseEmitter => _noiseEmitter;

		public bool IsAimedAtTarget
		{
			get
			{
				if (_cartSteering != null)
				{
					return _cartSteering.IsAimed;
				}
				return false;
			}
		}

		public bool IsCartItemAdjustFinished
		{
			get
			{
				if (!(_orphanCartItem == null))
				{
					return _orphanCartItemElapsed >= _settings.TakeIntoCartDuration;
				}
				return true;
			}
		}

		public Rigidbody CarriedItemRigidbody
		{
			get
			{
				if (!(_carriedGrabable != null) || !_context.ItemHolder.IsGrabbing)
				{
					return null;
				}
				return _carriedGrabable.Rigidbody;
			}
		}

		public Transform CarryHandSocket => _context.HandSocket;

		public float TakeProgress01
		{
			get
			{
				if (!_isTakingIntoCart || !(_settings.TakeIntoCartDuration > 0f))
				{
					return 1f;
				}
				return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_takeElapsed / _settings.TakeIntoCartDuration));
			}
		}

		public event Action<DamageData> OnDamaged;

		public event Action<bool> OnIsActiveChanged;

		[Inject]
		public void InjectDependencies(MonkeyPorterContext context, MonkeyPorterSettings settings, IEnemyTrackingService enemyTrackingService, IEntitiesSoundOcclusionService entitiesSoundOcclusionService)
		{
			_context = context;
			_settings = settings;
			_enemyTrackingService = enemyTrackingService;
			_entitiesSoundOcclusionService = entitiesSoundOcclusionService;
		}

		public void SetHiddenInCover(bool isHidden)
		{
			_isHiddenInCover = isHidden;
		}

		public void SuspendCartFollow(bool isSuspended)
		{
			if (!(base.Object == null) && base.Object.IsValid && base.HasStateAuthority && IsCartFollowSuspended != isSuspended)
			{
				IsCartFollowSuspended = isSuspended;
				if (isSuspended)
				{
					ParkCarriedItemInCart();
				}
			}
		}

		public bool TryGetCartPosition(out Vector3 position)
		{
			ResolveCart();
			if (_cartObject == null || !_cartObject.IsValid)
			{
				position = default(Vector3);
				return false;
			}
			position = _cartObject.transform.position;
			return true;
		}

		public bool TryGetCartDockPose(out Vector3 position, out Vector3 forward)
		{
			position = default(Vector3);
			forward = base.transform.forward;
			if (!TryGetCartPosition(out var position2))
			{
				return false;
			}
			Vector3 forward2 = _cartObject.transform.forward;
			forward2.y = 0f;
			if (forward2.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			forward = forward2.normalized;
			position = position2 - forward * ResolveCartFollowDistanceRaw();
			return true;
		}

		private StateMachine<MonkeyPorterStateId, MonkeyPorterStateId, MonkeyPorterEvent> BuildFsm()
		{
			StateMachine<MonkeyPorterStateId, MonkeyPorterStateId, MonkeyPorterEvent> stateMachine = new StateMachine<MonkeyPorterStateId, MonkeyPorterStateId, MonkeyPorterEvent>();
			stateMachine.AddState(MonkeyPorterStateId.Idle, new PorterIdleState(this, _context));
			stateMachine.AddState(MonkeyPorterStateId.GoToPlayers, new PorterGoToPlayersState(this, _context, _settings));
			stateMachine.AddState(MonkeyPorterStateId.WaitForItem, new PorterWaitForItemState(this, _context));
			stateMachine.AddState(MonkeyPorterStateId.CarryToBoat, new PorterCarryToBoatState(this, _context, _settings));
			stateMachine.AddState(MonkeyPorterStateId.Deliver, new PorterDeliverState(this, _context, _settings));
			stateMachine.AddState(MonkeyPorterStateId.Blocked, new PorterBlockedState(this, _context, _settings));
			stateMachine.AddState(MonkeyPorterStateId.Flee, new PorterFleeState(this, _context, _settings, _coverFinder));
			stateMachine.AddTransition(MonkeyPorterStateId.Idle, MonkeyPorterStateId.GoToPlayers, (Transition<MonkeyPorterStateId> transition) => HasAnyPlayer());
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnArrivedToPlayers, MonkeyPorterStateId.GoToPlayers, MonkeyPorterStateId.WaitForItem);
			stateMachine.AddTransition(MonkeyPorterStateId.GoToPlayers, MonkeyPorterStateId.Idle, (Transition<MonkeyPorterStateId> transition) => !HasAnyPlayer());
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnItemLoaded, MonkeyPorterStateId.WaitForItem, MonkeyPorterStateId.CarryToBoat);
			stateMachine.AddTransition(MonkeyPorterStateId.WaitForItem, MonkeyPorterStateId.GoToPlayers, (Transition<MonkeyPorterStateId> transition) => !HasCartItem() && ShouldReapproachPlayers());
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnArrivedToBoat, MonkeyPorterStateId.CarryToBoat, MonkeyPorterStateId.Deliver);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnItemLostFromCart, MonkeyPorterStateId.CarryToBoat, MonkeyPorterStateId.WaitForItem);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnItemDelivered, MonkeyPorterStateId.Deliver, MonkeyPorterStateId.GoToPlayers);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnPathBlocked, MonkeyPorterStateId.GoToPlayers, MonkeyPorterStateId.Blocked);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnPathBlocked, MonkeyPorterStateId.CarryToBoat, MonkeyPorterStateId.Blocked);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnPathCleared, MonkeyPorterStateId.Blocked, MonkeyPorterStateId.CarryToBoat, (Transition<MonkeyPorterStateId> transition) => HasCartItem());
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnPathCleared, MonkeyPorterStateId.Blocked, MonkeyPorterStateId.GoToPlayers, (Transition<MonkeyPorterStateId> transition) => !HasCartItem());
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnThreatSpotted, MonkeyPorterStateId.Idle, MonkeyPorterStateId.Flee);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnThreatSpotted, MonkeyPorterStateId.GoToPlayers, MonkeyPorterStateId.Flee);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnThreatSpotted, MonkeyPorterStateId.WaitForItem, MonkeyPorterStateId.Flee);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnThreatSpotted, MonkeyPorterStateId.CarryToBoat, MonkeyPorterStateId.Flee);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnThreatSpotted, MonkeyPorterStateId.Blocked, MonkeyPorterStateId.Flee);
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnThreatGone, MonkeyPorterStateId.Flee, MonkeyPorterStateId.CarryToBoat, (Transition<MonkeyPorterStateId> transition) => HasCartItem());
			stateMachine.AddTriggerTransition(MonkeyPorterEvent.OnThreatGone, MonkeyPorterStateId.Flee, MonkeyPorterStateId.GoToPlayers, (Transition<MonkeyPorterStateId> transition) => !HasCartItem());
			stateMachine.SetStartState(MonkeyPorterStateId.Idle);
			return stateMachine;
		}

		public override void Spawned()
		{
			base.Spawned();
			_enemyTrackingService.RegisterTarget(this);
			_context.ValidateRequiredReferences();
			SetupAgentByAuthority();
			EnsureMasterStateAuthority();
			if (IsMasterAiPeer() && base.HasStateAuthority)
			{
				InitializeAuthority();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_enemyTrackingService.UnregisterTarget(this);
			if (base.HasStateAuthority && _fsm != null)
			{
				_fsm.StateChanged -= OnFsmStateChanged;
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (!IsMasterAiPeer())
			{
				return;
			}
			EnsureMasterStateAuthority();
			if (base.HasStateAuthority)
			{
				if (IsHealthInitialized && Health <= 0f)
				{
					Die(_lastDamageDealerId);
				}
				else if (TryPlaceAgentOnNavMesh() && _fsm != null)
				{
					AgentVelocity = Mathf.Max(_context.AgentVelocity, _cartSteering.TangentialSpeed);
					_noiseEmitter.Tick(GetTickDelta(), HasCartItem());
					TickThreatDetection();
					TryPickupNearbyItem();
					UpdateCarriedItem();
					_fsm.OnLogic();
				}
			}
		}

		private void Update()
		{
			if (_cartSteering != null && _fsm != null && IsMasterAiPeer() && base.HasStateAuthority)
			{
				if (_isTakingIntoCart)
				{
					_takeElapsed += Time.deltaTime;
				}
				_cartSteering.Tick(Time.deltaTime, ResolveCartFollowDistance());
			}
		}

		private void LateUpdate()
		{
			TickItemLaunch();
			TickOrphanCartItem();
		}

		public void StateAuthorityChanged()
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				if (!IsMasterAiPeer() || !base.HasStateAuthority)
				{
					TeardownAuthority();
					SetupAgentByAuthority();
				}
				else
				{
					EnsureMasterStateAuthority();
					SetupAgentByAuthority();
					InitializeAuthority();
				}
			}
		}

		public void AttachCart(NetworkObject cartObject)
		{
			if (base.HasStateAuthority && !(cartObject == null))
			{
				if (CartId != cartObject.Id)
				{
					CartId = cartObject.Id;
				}
				CacheCart(cartObject);
				EnsureCartAuthority();
			}
		}

		public void TriggerEvent(MonkeyPorterEvent monkeyPorterEvent)
		{
			_fsm?.Trigger(monkeyPorterEvent);
		}

		public void SetAimTarget(Vector3 worldPosition)
		{
			_cartSteering?.SetAimTarget(worldPosition);
		}

		public void ClearAimTarget()
		{
			_cartSteering?.ClearAimTarget();
		}

		public void SetVisualState(MonkeyPorterVisualState state)
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

		public void TryInitializeFloorAnchor()
		{
			if (!IsFloorAnchorInitialized && base.HasStateAuthority && _context.TryResolveFloorAnchor(HomePosition, out var floorAnchor))
			{
				FloorAnchor = floorAnchor;
				IsFloorAnchorInitialized = true;
			}
		}

		public bool HasCartItem()
		{
			if (_context.ItemHolder.IsGrabbing)
			{
				return !_isTakingIntoCart;
			}
			return false;
		}

		public void PlayThrowAnimation()
		{
			PlayActionAnimation(MonkeyPorterActionAnim.Throw);
		}

		public void DeliverCarriedItem()
		{
			SimplePointGrabable carriedGrabable = _carriedGrabable;
			_context.ItemHolder.ReleaseGrab();
			if (carriedGrabable != null && carriedGrabable.NetworkObject != null && carriedGrabable.Rigidbody != null)
			{
				BeginItemLaunch(carriedGrabable);
			}
			else
			{
				RestoreItemDynamics(carriedGrabable);
			}
			ClearCarried();
		}

		public bool TryAdoptOrphanCartItem()
		{
			if (_orphanCartItem == null || _context.ItemHolder.IsGrabbing)
			{
				return false;
			}
			SimplePointGrabable orphanCartItem = _orphanCartItem;
			if (!_context.ItemHolder.TryGrab(orphanCartItem))
			{
				return false;
			}
			_orphanCartItem = null;
			_carriedGrabable = orphanCartItem;
			_isTakingIntoCart = false;
			CarriedItemId = orphanCartItem.NetworkObject.Id;
			MakeItemKinematic(orphanCartItem);
			_context.MoveCarryAnchorToCart();
			return true;
		}

		public bool TryStartCartItemAdjust()
		{
			if (_orphanCartItem != null || _context.ItemHolder.IsGrabbing)
			{
				return false;
			}
			ResolveCart();
			if (_cartIntake == null)
			{
				return false;
			}
			IPointGrabable pointGrabable = _cartIntake.TryGetDroppedItem();
			if (pointGrabable == null || pointGrabable.NetworkObject == null)
			{
				return false;
			}
			if (!pointGrabable.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component))
			{
				return false;
			}
			_cartIntake.Forget(pointGrabable);
			MakeItemKinematic(component);
			SetOrphanCartItem(component, 0f);
			PlayActionAnimation(MonkeyPorterActionAnim.Take);
			return true;
		}

		private void ParkCarriedItemInCart()
		{
			if (!(_carriedGrabable == null) && _context.ItemHolder.IsGrabbing)
			{
				SimplePointGrabable carriedGrabable = _carriedGrabable;
				_context.ItemHolder.ReleaseGrab();
				SetOrphanCartItem(carriedGrabable, _settings.TakeIntoCartDuration);
			}
		}

		private void SetOrphanCartItem(SimplePointGrabable item, float elapsed)
		{
			_orphanCartItem = item;
			_orphanCartItemStart = ((item.Rigidbody != null) ? item.Rigidbody.position : item.transform.position);
			_orphanCartItemElapsed = elapsed;
			_carriedGrabable = null;
			_isTakingIntoCart = false;
		}

		private void TickOrphanCartItem()
		{
			if (!(_orphanCartItem == null))
			{
				if (_orphanCartItem.NetworkObject == null || _cartIntake == null || _orphanCartItem.GrabbedByPlayers.Count > 0)
				{
					ReleaseOrphanCartItem();
					return;
				}
				Transform holdPoint = _cartIntake.HoldPoint;
				_orphanCartItemElapsed += Time.deltaTime;
				float num = Mathf.Max(0.01f, _settings.TakeIntoCartDuration);
				float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_orphanCartItemElapsed / num));
				_orphanCartItem.transform.SetPositionAndRotation(Vector3.Lerp(_orphanCartItemStart, holdPoint.position, t), holdPoint.rotation);
			}
		}

		private void ReleaseOrphanCartItem()
		{
			if (!(_orphanCartItem == null))
			{
				RestoreItemDynamics(_orphanCartItem);
				_orphanCartItem = null;
			}
		}

		private void TickThreatDetection()
		{
			float tickDelta = GetTickDelta();
			_threatDetector.Tick(tickDelta, _isHiddenInCover);
			if (Phase == MonkeyPorterStateId.Flee)
			{
				_fleeCooldownLeft = _settings.FleeCooldown;
				return;
			}
			_fleeCooldownLeft -= tickDelta;
			if (_threatDetector.HasThreat && (!(_fleeCooldownLeft > 0f) || _threatDetector.IsAnyThreatWithin(_settings.ThreatOverrideRadius)))
			{
				TriggerEvent(MonkeyPorterEvent.OnThreatSpotted);
			}
		}

		private void TryPickupNearbyItem()
		{
			if (_context.ItemHolder.IsGrabbing || Phase == MonkeyPorterStateId.Flee || Phase == MonkeyPorterStateId.Deliver)
			{
				return;
			}
			if (_orphanCartItem != null)
			{
				if (IsCartItemAdjustFinished)
				{
					TryAdoptOrphanCartItem();
				}
				return;
			}
			ResolveCart();
			if (!(_cartIntake == null) && !TryStartCartItemMagnet() && Phase == MonkeyPorterStateId.WaitForItem)
			{
				IPointGrabable pointGrabable = _cartIntake.TryGetDroppedItem();
				if (pointGrabable != null && !(pointGrabable.NetworkObject == null) && pointGrabable.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component) && _context.ItemHolder.TryGrab(component))
				{
					_carriedGrabable = component;
					_cartIntake.Forget(pointGrabable);
					CarriedItemId = component.NetworkObject.Id;
					MakeItemKinematic(component);
					_isTakingIntoCart = true;
					_takeElapsed = 0f;
					_context.MoveCarryAnchorToHand();
					PlayActionAnimation(MonkeyPorterActionAnim.Take);
				}
			}
		}

		private bool TryStartCartItemMagnet()
		{
			IPointGrabable pointGrabable = _cartIntake.TryGetItemRestingInCart();
			if (pointGrabable == null || pointGrabable.NetworkObject == null)
			{
				return false;
			}
			if (!WasRecentlyHeldByPlayer(pointGrabable))
			{
				return false;
			}
			if (!pointGrabable.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component))
			{
				return false;
			}
			_cartIntake.Forget(pointGrabable);
			MakeItemKinematic(component);
			SetOrphanCartItem(component, 0f);
			PlayActionAnimation(MonkeyPorterActionAnim.Take);
			return true;
		}

		private bool WasRecentlyHeldByPlayer(IPointGrabable item)
		{
			MonoItem componentInChildren = item.NetworkObject.GetComponentInChildren<MonoItem>();
			if (componentInChildren == null || componentInChildren.LastGrabTikRaw <= 0)
			{
				return false;
			}
			int num = Mathf.RoundToInt((float)base.Runner.TickRate * 3f);
			return base.Runner.Tick.Raw - componentInChildren.LastGrabTikRaw <= num;
		}

		private void UpdateCarriedItem()
		{
			if (!_context.ItemHolder.IsGrabbing)
			{
				if (_carriedGrabable != null)
				{
					RestoreItemDynamics(_carriedGrabable);
					ClearCarried();
				}
				return;
			}
			if (IsCarriedItemTakenByPlayer())
			{
				SimplePointGrabable carriedGrabable = _carriedGrabable;
				_context.ItemHolder.ReleaseGrab();
				RestoreItemDynamics(carriedGrabable);
				ClearCarried();
				return;
			}
			if (_isTakingIntoCart)
			{
				_context.MoveCarryAnchorToHand();
				if (_takeElapsed < _settings.TakeIntoCartDuration)
				{
					return;
				}
				_isTakingIntoCart = false;
			}
			_context.MoveCarryAnchorToCart();
		}

		private bool IsCarriedItemTakenByPlayer()
		{
			if (_carriedGrabable != null)
			{
				if (!(_carriedGrabable.NetworkObject == null))
				{
					return _carriedGrabable.GrabbedByPlayersCount > 0;
				}
				return true;
			}
			return false;
		}

		private void MakeItemKinematic(SimplePointGrabable grabable)
		{
			if (!(grabable == null) && !(grabable.Rigidbody == null))
			{
				grabable.ChangeRigidbodyKinematic = false;
				if (grabable.Rigidbody.interpolation != RigidbodyInterpolation.None)
				{
					_carriedItemInterpolation = grabable.Rigidbody.interpolation;
				}
				grabable.Rigidbody.interpolation = RigidbodyInterpolation.None;
				grabable.Rigidbody.linearVelocity = Vector3.zero;
				grabable.Rigidbody.angularVelocity = Vector3.zero;
				grabable.Rigidbody.isKinematic = true;
			}
		}

		private void RestoreItemDynamics(SimplePointGrabable grabable)
		{
			if (!(grabable == null) && !(grabable.Rigidbody == null))
			{
				grabable.Rigidbody.isKinematic = false;
				grabable.Rigidbody.interpolation = _carriedItemInterpolation;
				grabable.ChangeRigidbodyKinematic = true;
			}
		}

		private void BeginItemLaunch(SimplePointGrabable item)
		{
			AbortItemLaunch();
			if (!_context.HasBoatDrop)
			{
				RestoreItemDynamics(item);
				return;
			}
			Rigidbody rigidbody = item.Rigidbody;
			rigidbody.linearVelocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.isKinematic = true;
			_launchingItem = item;
			_launchElapsed = 0f;
			_launchStartPosition = rigidbody.position;
			_launchEndPosition = _context.GetBoatWorldPosition();
		}

		private void TickItemLaunch()
		{
			if (_launchingItem == null)
			{
				return;
			}
			if (_launchingItem.NetworkObject == null || _launchingItem.Rigidbody == null)
			{
				_launchingItem = null;
				return;
			}
			float num = Mathf.Max(0.01f, _settings.ThrowFlightDuration);
			_launchElapsed += Time.deltaTime;
			float num2 = Mathf.Clamp01(_launchElapsed / num);
			_launchingItem.Rigidbody.transform.position = EvaluateLaunchPoint(num2);
			if (!(num2 < 1f))
			{
				SimplePointGrabable launchingItem = _launchingItem;
				_launchingItem = null;
				RestoreItemDynamics(launchingItem);
				launchingItem.Rigidbody.linearVelocity = EvaluateLaunchVelocity(num);
			}
		}

		private Vector3 EvaluateLaunchPoint(float progress)
		{
			Vector3 result = Vector3.Lerp(_launchStartPosition, _launchEndPosition, progress);
			result.y += _settings.DeliverApexHeight * 4f * progress * (1f - progress);
			return result;
		}

		private Vector3 EvaluateLaunchVelocity(float duration)
		{
			Vector3 result = (_launchEndPosition - _launchStartPosition) / duration;
			result.y -= _settings.DeliverApexHeight * 4f / duration;
			return result;
		}

		private void AbortItemLaunch()
		{
			if (!(_launchingItem == null))
			{
				SimplePointGrabable launchingItem = _launchingItem;
				_launchingItem = null;
				RestoreItemDynamics(launchingItem);
			}
		}

		private void ClearCarried()
		{
			_carriedGrabable = null;
			_isTakingIntoCart = false;
			if (CarriedItemId != default(NetworkId))
			{
				CarriedItemId = default(NetworkId);
			}
		}

		private void ReacquireCarriedItem()
		{
			if (CarriedItemId == default(NetworkId) || _context.ItemHolder.IsGrabbing || base.Runner == null)
			{
				return;
			}
			NetworkObject networkObject = base.Runner.FindObject(CarriedItemId);
			if (networkObject == null || !networkObject.TryGetComponent<SimplePointGrabable>(out var component))
			{
				return;
			}
			if (IsCartFollowSuspended)
			{
				MakeItemKinematic(component);
				SetOrphanCartItem(component, _settings.TakeIntoCartDuration);
			}
			else if (_context.ItemHolder.TryGrab(component))
			{
				_carriedGrabable = component;
				_isTakingIntoCart = false;
				component.ChangeRigidbodyKinematic = false;
				_carriedItemInterpolation = ((!(component.Rigidbody != null)) ? RigidbodyInterpolation.Interpolate : component.Rigidbody.interpolation);
				if (component.Rigidbody != null)
				{
					component.Rigidbody.interpolation = RigidbodyInterpolation.None;
					component.Rigidbody.isKinematic = true;
				}
				_context.MoveCarryAnchorToCart();
			}
		}

		private void PlayActionAnimation(MonkeyPorterActionAnim action)
		{
			if (base.HasStateAuthority)
			{
				PlayActionAnimationRpc(action);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2434741634u)]
		private void PlayActionAnimationRpc([RpcPayload(4)] MonkeyPorterActionAnim action)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2434741634u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.MonkeyPorterEnemy::PlayActionAnimationRpc(Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data.MonkeyPorterActionAnim)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(action, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			switch (action)
			{
			case MonkeyPorterActionAnim.Take:
				_context.Animator.SetTrigger(_takeTriggerHash);
				break;
			case MonkeyPorterActionAnim.Throw:
				_context.Animator.SetTrigger(_throwTriggerHash);
				break;
			}
		}

		private void InitializeAuthority()
		{
			if (_fsm == null && IsMasterAiPeer())
			{
				if (!IsHealthInitialized)
				{
					Health = _settings.MaxHealth;
					IsHealthInitialized = true;
				}
				ResolveCart();
				EnsureCartAuthority();
				if (_cartSteering == null)
				{
					_cartSteering = new PorterCartSteering(_context, _settings);
				}
				if (_blockDetector == null)
				{
					_blockDetector = new PorterBlockDetector(_context, _settings);
				}
				if (_threatDetector == null)
				{
					_threatDetector = new PorterThreatDetector(_context, _settings, _enemyTrackingService);
				}
				if (_coverFinder == null)
				{
					_coverFinder = new PorterCoverFinder(_context, _settings);
				}
				if (_noiseEmitter == null)
				{
					_noiseEmitter = new PorterNoiseEmitter(_context, _settings, _entitiesSoundOcclusionService);
				}
				if (!IsHomeInitialized)
				{
					HomePosition = base.transform.position;
					IsHomeInitialized = true;
				}
				_fsm = BuildFsm();
				_fsm.Init();
				_fsm.StateChanged += OnFsmStateChanged;
				RestorePhaseFromNetwork();
				ReacquireCarriedItem();
			}
		}

		private void TeardownAuthority()
		{
			_cartSteering?.ClearAimTarget();
			_cartSteering?.Reset();
			AbortItemLaunch();
			ReleaseOrphanCartItem();
			RestoreItemDynamics(_carriedGrabable);
			_context.ItemHolder.ReleaseGrab();
			_carriedGrabable = null;
			_isTakingIntoCart = false;
			if (_fsm != null)
			{
				_fsm.StateChanged -= OnFsmStateChanged;
				_fsm = null;
			}
		}

		public void Damage(DamageData damage)
		{
			this.OnDamaged?.Invoke(damage);
			if (base.HasStateAuthority && !_isDead)
			{
				_lastDamageDealerId = damage.DamageDealerPlayerID;
				Health = Mathf.Max(0f, Health - damage.Damage);
			}
		}

		public void DamageRPC(float damage)
		{
			DamageRPC(damage, 0, default(DamageRpcSource));
		}

		public void DamageRPC(float damage, int dealerPlayerID)
		{
			DamageRPC(damage, dealerPlayerID, default(DamageRpcSource));
		}

		public void DamageRPC(float damage, int dealerPlayerID, DamageRpcSource rpcSource)
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				DamageRpc(damage, dealerPlayerID);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1506398947u)]
		private void DamageRpc([RpcPayload(4)] float damage, [RpcPayload(4)] int dealerPlayerID)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1506398947u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.MonkeyPorterEnemy::DamageRpc(System.Single,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(damage, 4);
						writer.Write(dealerPlayerID, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Damage(new DamageData
			{
				Damage = damage,
				DamageDealerPlayerID = dealerPlayerID
			});
		}

		public void Heal(float value, bool isSynchronize = false)
		{
			if (base.HasStateAuthority && !_isDead)
			{
				Health = Mathf.Min(_settings.MaxHealth, Health + value);
			}
		}

		public void HealToFullValue()
		{
			if (base.HasStateAuthority)
			{
				Health = _settings.MaxHealth;
			}
		}

		public void SetDamageableTags(List<DamageableTag> entityDamageableTags)
		{
			DamageableTags = entityDamageableTags;
		}

		public void AddRPCForce(float force, Vector3 direction, ForceMode forceMode)
		{
		}

		private void Die(int dealerPlayerId)
		{
			if (!_isDead)
			{
				_isDead = true;
				_lastDamageDealerId = dealerPlayerId;
				this.OnIsActiveChanged?.Invoke(obj: false);
				SpawnDeadCart();
				TeardownAuthority();
				_context.StopAgent();
				AgentVelocity = 0f;
				if (!(base.Runner == null) && !(base.Object == null) && base.Object.IsValid && (!TryGetComponent<IDeferredEnemyDespawn>(out var component) || !component.TryStartDeferredDespawn()))
				{
					base.Runner.Despawn(base.Object);
				}
			}
		}

		private void SpawnDeadCart()
		{
			ResolveCart();
			if (!(_cartObject == null) && _cartObject.IsValid)
			{
				Vector3 position = _cartObject.transform.position;
				Quaternion rotation = _cartObject.transform.rotation;
				base.Runner.Despawn(_cartObject);
				_cartObject = null;
				_cartIntake = null;
				_cartFollower = null;
				if (_settings.DeadCartPrefab != null)
				{
					base.Runner.Spawn(_settings.DeadCartPrefab, position, rotation, base.Runner.LocalPlayer);
				}
			}
		}

		private void RestorePhaseFromNetwork()
		{
			MonkeyPorterStateId monkeyPorterStateId = ((Phase == MonkeyPorterStateId.None) ? MonkeyPorterStateId.Idle : Phase);
			if (monkeyPorterStateId != MonkeyPorterStateId.Idle)
			{
				_fsm.RequestStateChange(monkeyPorterStateId, forceInstantly: true);
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

		private bool HasAnyPlayer()
		{
			Vector3 position;
			float distance;
			return _context.TryGetNearestPlayerPosition(out position, out distance);
		}

		private bool ShouldReapproachPlayers()
		{
			if (!_context.TryGetNearestPlayerPosition(out var _, out var distance))
			{
				return true;
			}
			return distance > _settings.LeashRadius;
		}

		private void SetupAgentByAuthority()
		{
			if (!base.HasStateAuthority || !(base.Runner != null) || !base.Runner.IsSharedModeMasterClient)
			{
				_context.Agent.enabled = false;
				return;
			}
			_context.SetAgentRotationControl(isAgentControlled: false);
			TryPlaceAgentOnNavMesh();
		}

		private bool TryPlaceAgentOnNavMesh()
		{
			if (_context.Agent.isOnNavMesh)
			{
				_agentPlacedOnce = true;
				return true;
			}
			if (!_context.Agent.enabled)
			{
				_context.Agent.enabled = true;
			}
			if (_context.WarpOntoNavMesh(_settings.NavMeshSampleRadius))
			{
				_agentPlacedOnce = true;
				return true;
			}
			if (!_agentPlacedOnce)
			{
				_context.Agent.enabled = false;
				return false;
			}
			return true;
		}

		private void EnsureMasterStateAuthority()
		{
			if (!(base.Runner == null) && base.Object.IsValid && base.Runner.IsSharedModeMasterClient)
			{
				if (!base.HasStateAuthority)
				{
					base.Object.RequestStateAuthority();
				}
				EnsureCartAuthority();
			}
		}

		private void EnsureCartAuthority()
		{
			if (!(base.Runner == null) && base.Runner.IsSharedModeMasterClient)
			{
				ResolveCart();
				if (!(_cartObject == null) && _cartObject.IsValid && !_cartObject.HasStateAuthority)
				{
					_cartObject.RequestStateAuthority();
				}
			}
		}

		private void ResolveCart()
		{
			if (_cartObject != null && _cartObject.IsValid)
			{
				if (_cartIntake == null)
				{
					_cartIntake = _cartObject.GetComponentInChildren<PorterCartIntake>();
				}
				if (_cartFollower == null)
				{
					_cartFollower = _cartObject.GetComponent<MonkeyPorterCartFollower>();
				}
			}
			else if (!(CartId == default(NetworkId)) && !(base.Runner == null))
			{
				NetworkObject networkObject = base.Runner.FindObject(CartId);
				if (networkObject != null)
				{
					CacheCart(networkObject);
				}
			}
		}

		private void CacheCart(NetworkObject cartObject)
		{
			_cartObject = cartObject;
			_cartIntake = cartObject.GetComponentInChildren<PorterCartIntake>();
			_cartFollower = cartObject.GetComponent<MonkeyPorterCartFollower>();
		}

		private float ResolveCartFollowDistance()
		{
			if (IsCartFollowSuspended)
			{
				return 0f;
			}
			return ResolveCartFollowDistanceRaw();
		}

		private float ResolveCartFollowDistanceRaw()
		{
			ResolveCart();
			if (!(_cartFollower != null))
			{
				return 0f;
			}
			return _cartFollower.FollowDistance;
		}

		private void OnFsmStateChanged(StateBase<MonkeyPorterStateId> state)
		{
			SetPhase(state.name);
		}

		private void SetPhase(MonkeyPorterStateId phase)
		{
			if (base.HasStateAuthority && Phase != phase)
			{
				Phase = phase;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			VisualState = _VisualState;
			Phase = _Phase;
			CarriedItemId = _CarriedItemId;
			CartId = _CartId;
			IsCartFollowSuspended = _IsCartFollowSuspended;
			AgentVelocityQuantized = _AgentVelocityQuantized;
			Health = _Health;
			IsHealthInitialized = _IsHealthInitialized;
			HomePosition = _HomePosition;
			IsHomeInitialized = _IsHomeInitialized;
			FloorAnchor = _FloorAnchor;
			IsFloorAnchorInitialized = _IsFloorAnchorInitialized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_VisualState = VisualState;
			_Phase = Phase;
			_CarriedItemId = CarriedItemId;
			_CartId = CartId;
			_IsCartFollowSuspended = IsCartFollowSuspended;
			_AgentVelocityQuantized = AgentVelocityQuantized;
			_Health = Health;
			_IsHealthInitialized = IsHealthInitialized;
			_HomePosition = HomePosition;
			_IsHomeInitialized = IsHomeInitialized;
			_FloorAnchor = FloorAnchor;
			_IsFloorAnchorInitialized = IsFloorAnchorInitialized;
		}

		[NetworkRpcWeavedInvoker(2434741634u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayActionAnimationRpc_0040Invoker2434741634([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out MonkeyPorterActionAnim value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyPorterEnemy)context.TargetBehaviour).PlayActionAnimationRpc(value);
		}

		[NetworkRpcWeavedInvoker(1506398947u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DamageRpc_0040Invoker1506398947([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonkeyPorterEnemy)context.TargetBehaviour).DamageRpc(value, value2);
		}
	}
}
