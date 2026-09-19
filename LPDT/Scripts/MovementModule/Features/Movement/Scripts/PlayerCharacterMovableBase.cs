using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AnimationModule.Scripts;
using Features.CameraModelModule;
using Features.InputModule.Scripts.Generated;
using Features.LineArmModule.Scripts.HeavyItemData;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkInputModule.Scripts;
using Features.PhysicsInteractionModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using Fusion.Addons.Physics;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;
using Zenject;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(7)]
	public class PlayerCharacterMovableBase : CharacterMovableBase
	{
		private static readonly int _currentSpeed = Animator.StringToHash("CurrentSpeed");

		private static readonly int _isCrouch = Animator.StringToHash("IsCrouch");

		private static readonly int _isCrouchImmediately = Animator.StringToHash("CrouchImmediately");

		[SerializeField]
		private Transform _rotatoblePart;

		[SerializeField]
		private float _notLocalPlayerMass = 0.1f;

		[SerializeField]
		private LayerMask _groundLayerMask;

		[Header("References")]
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private NetworkRigidbody _networkRigidbody;

		[SerializeField]
		private StatEntityNetworkedBase<EntityStatType> _statEntity;

		[Header("Configuration")]
		[SerializeField]
		private float _acceleration;

		[SerializeField]
		private float _deceleration;

		[SerializeField]
		private float _jumpImpulse;

		[SerializeField]
		private float _jumpRaycastDistance;

		[SerializeField]
		private float _jumpDelay = 0.3f;

		[Header("Slope")]
		[SerializeField]
		private float _slopeStabilizeMinAngle = 5f;

		[SerializeField]
		private float _slopeStabilizeMaxWalkableAngle = 60f;

		[Header("Crouch")]
		[SerializeField]
		private List<Transform> _crouchAffectedTransforms;

		[SerializeField]
		private List<Transform> _raycastPoints;

		[SerializeField]
		private List<CapsuleCollider> _capsuleColliders;

		[SerializeField]
		private float _crouchDeltaHeight = 0.5f;

		[SerializeField]
		private float _crouchDuration = 0.3f;

		[SerializeField]
		private LayerMask _crouchCheckMask;

		[SerializeField]
		private CapsuleCollider _mainCapsuleCollider;

		[SerializeField]
		private float _crouchCheckRadius = 0.5f;

		[Header("Sprint & Stamina")]
		[SerializeField]
		private float _toggledSprintNoInputReleaseSeconds = 1f;

		[SerializeField]
		private float _carryingStaminaDrainRate = 15f;

		[SerializeField]
		private float _staminaRechargeDelay = 1f;

		[SerializeField]
		private float _walkFOV = 40f;

		[SerializeField]
		private float _sprintFOV = 70f;

		[SerializeField]
		private float _fovTransitionSpeed = 0.5f;

		[SerializeField]
		private float _speedSmoothTime = 0.1f;

		[SerializeField]
		private float _followForwardSpeedMultiplier = 0.2f;

		[SerializeField]
		private float _animationIdleSpeedThreshold = 0.1f;

		[SerializeField]
		private CompositeAnimator _animator;

		[SerializeField]
		private NetworkedCompositeAnimator _networkedAnimator;

		[SerializeField]
		private PlayerRagdollEntity _playerRagdollEntity;

		[SerializeField]
		private Vector3 _teleportPosition;

		public Transform FootPoint;

		[WeaverGenerated]
		[DefaultForProperty("IsKinematicInternal", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsKinematicInternal;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("PlatformCarrierId", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _PlatformCarrierId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("PlatformCarrierLocalPosition", 3, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _PlatformCarrierLocalPosition;

		private Transform _platformCarrier;

		private NetworkId _platformCarrierId;

		private Vector3 _platformLocalOffset;

		private bool _hasPlatformOffset;

		private Vector3 _velocity;

		private bool _isMovementInputEnabled = true;

		private PlayerMovableModel _playerMovableModel;

		private CameraModel _cameraModel;

		private FieldOfViewModel _fieldOfViewModel;

		private PlayerStatsConfiguration _playerStatsConfiguration;

		private HeavyItemLocalModel _heavyItemLocalModel;

		private IInputService _inputService;

		private IInputDeviceService _inputDeviceService;

		private IPlayerStaminaService _playerStaminaService;

		private IMasterOrphanAvatarCleanup _masterOrphanAvatarCleanup;

		private readonly RaycastHit[] _crouchCheckHits = new RaycastHit[32];

		private readonly RaycastHit[] _groundCheckHits = new RaycastHit[32];

		private bool _zeroInput;

		private bool _isCrouching;

		private bool _crouchPhaseEnabled;

		private bool _isCrouchTransitioning;

		private float _crouchProgress;

		private float _originalHeadHeight;

		private Dictionary<Transform, float> _originalTransformHeights;

		private Dictionary<CapsuleCollider, (float height, float centerY)> _originalColliderData;

		private bool _isSprinting;

		private bool _wantsToSprint;

		private bool _isSprintInputHeld;

		private bool _isSprintToggledByGamepad;

		private float _toggledSprintNoInputTimer;

		private bool _isCrouchInputHeld;

		private bool _isCrouchToggledByGamepad;

		private float _staminaRechargeTimer;

		private float _targetFOV;

		private bool _isForcedCrouching;

		private IStat _walkSpeedStat;

		private IStat _sprintSpeedStat;

		private IStat _staminaStat;

		private IStat _speedStat;

		private IStat _punishmentStaminaStat;

		private IStat _hiddenStaminaStat;

		private IStat _crouchSpeedStat;

		private IStat _staminaRechargeRateStat;

		private IStat _staminaRechargeRateOnPunishStat;

		private IStat _hiddenStaminaRechargeRateStat;

		private IStat _hiddenStaminaDrainRateStat;

		private IStat _staminaDrainRateStat;

		private bool _isGrounded;

		private const float SPEED_QUANTIZATION = 0.1f;

		private const float MOVEMENT_INPUT_SQR_THRESHOLD = 0.0001f;

		[WeaverGenerated]
		[DefaultForProperty("_smoothedSpeedQuantized", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int __smoothedSpeedQuantized;

		private bool _ignoreMovement;

		private bool _lockedGroundPlantActive;

		private float _speedVelocity;

		private float _smoothedSpeedContinuous;

		private bool _isSpawned;

		private bool _atomicSpawnApplied;

		private Vector2 _liveMovementInput;

		private float _visualSpeed;

		private float _currentJumpDelay;

		private bool _isJumpDelayed;

		private bool _isJumpEscaping;

		private Dictionary<Transform, float> _originalLocalY;

		public CapsuleCollider BodyCollider => _mainCapsuleCollider;

		public List<Transform> RaycastPoints => _raycastPoints;

		public Transform RotatoblePart => _rotatoblePart;

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe bool IsKinematicInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase.IsKinematicInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase.IsKinematicInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		public bool IsKinematic
		{
			get
			{
				if (!_isSpawned)
				{
					return false;
				}
				return IsKinematicInternal;
			}
			private set
			{
				IsKinematicInternal = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe NetworkId PlatformCarrierId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase.PlatformCarrierId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 2);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase.PlatformCarrierId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 3)]
		public unsafe Vector3 PlatformCarrierLocalPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase.PlatformCarrierLocalPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase.PlatformCarrierLocalPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3) = value;
			}
		}

		public Transform RefusedCarrier { get; private set; }

		[Networked]
		[NetworkedWeaved(6, 1)]
		private unsafe int _smoothedSpeedQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase._smoothedSpeedQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[6];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerCharacterMovableBase._smoothedSpeedQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[6] = value;
			}
		}

		public bool IsStandUpBlocked { get; private set; }

		public Collider StandUpBlockingCollider { get; private set; }

		public Rigidbody Rigidbody => _rigidbody;

		private bool IsReady => _cameraModel.Cameras.ContainsKey(Features.CameraModelModule.CameraType.FPCamera);

		public override bool IsGrounded => _isGrounded;

		public bool IsJumping
		{
			get
			{
				if (!_isJumpDelayed)
				{
					return _isJumpEscaping;
				}
				return true;
			}
		}

		public override event Action OnChangePosition;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel, CameraModel cameraModel, FieldOfViewModel fieldOfViewModel, PlayerStatsConfiguration playerStatsConfiguration, HeavyItemLocalModel heavyItemLocalModel, IInputService inputService, IInputDeviceService inputDeviceService, IPlayerStaminaService playerStaminaService, IMasterOrphanAvatarCleanup masterOrphanAvatarCleanup)
		{
			_playerStatsConfiguration = playerStatsConfiguration;
			_cameraModel = cameraModel;
			_fieldOfViewModel = fieldOfViewModel;
			_playerMovableModel = playerMovableModel;
			_heavyItemLocalModel = heavyItemLocalModel;
			_inputService = inputService;
			_inputDeviceService = inputDeviceService;
			_playerStaminaService = playerStaminaService;
			_masterOrphanAvatarCleanup = masterOrphanAvatarCleanup;
		}

		public override void Spawned()
		{
			base.Spawned();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			_isSpawned = true;
			if (base.HasInputAuthority)
			{
				_playerMovableModel.LocalMovable = this;
				_playerMovableModel.NetworkedAnimator = _networkedAnimator;
			}
			if (base.Runner.LocalPlayer != base.Object.StateAuthority)
			{
				_rigidbody.mass = _notLocalPlayerMass;
			}
			InitizeStats();
			_playerMovableModel.AddCharacterMovable(base.Object.InputAuthority, this);
			_targetFOV = ResolveTargetFOV();
			if (base.HasInputAuthority)
			{
				InputVector2Actions movement = _inputService.Movement;
				movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(movement.VectorChangedPerformed, new Action<Vector2>(OnLiveMovementInputChanged));
				InputVector2Actions movement2 = _inputService.Movement;
				movement2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(movement2.VectorChangedCanceled, new Action<Vector2>(OnLiveMovementInputChanged));
				InputDefaultActions crouch = _inputService.Crouch;
				crouch.Performed = (Action)Delegate.Combine(crouch.Performed, new Action(OnCrouchStarted));
				InputDefaultActions crouch2 = _inputService.Crouch;
				crouch2.Canceled = (Action)Delegate.Combine(crouch2.Canceled, new Action(OnCrouchEnded));
				InputDefaultActions sprint = _inputService.Sprint;
				sprint.Started = (Action)Delegate.Combine(sprint.Started, new Action(OnSprintingStarted));
				InputDefaultActions sprint2 = _inputService.Sprint;
				sprint2.Canceled = (Action)Delegate.Combine(sprint2.Canceled, new Action(OnSprintingEnded));
				_inputDeviceService.OnCurrentActiveDeviceChange += OnCurrentActiveDeviceChanged;
				_fieldOfViewModel.OnFieldOfViewChanged += OnFieldOfViewSettingChanged;
			}
			if (base.Runner.IsSharedModeMasterClient)
			{
				_masterOrphanAvatarCleanup.ScheduleMasterOrphanCleanup();
			}
		}

		protected override void OnDespawn()
		{
			base.OnDespawn();
			_isSpawned = false;
			_playerMovableModel.RemoveCharacterMovable(base.Object.InputAuthority);
			if ((object)_playerMovableModel.LocalMovable == this)
			{
				_playerMovableModel.LocalMovable = null;
			}
			if ((object)_playerMovableModel.NetworkedAnimator == _networkedAnimator)
			{
				_playerMovableModel.NetworkedAnimator = null;
			}
			if (base.HasInputAuthority)
			{
				InputVector2Actions movement = _inputService.Movement;
				movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(movement.VectorChangedPerformed, new Action<Vector2>(OnLiveMovementInputChanged));
				InputVector2Actions movement2 = _inputService.Movement;
				movement2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(movement2.VectorChangedCanceled, new Action<Vector2>(OnLiveMovementInputChanged));
				InputDefaultActions crouch = _inputService.Crouch;
				crouch.Performed = (Action)Delegate.Remove(crouch.Performed, new Action(OnCrouchStarted));
				InputDefaultActions crouch2 = _inputService.Crouch;
				crouch2.Canceled = (Action)Delegate.Remove(crouch2.Canceled, new Action(OnCrouchEnded));
				InputDefaultActions sprint = _inputService.Sprint;
				sprint.Started = (Action)Delegate.Remove(sprint.Started, new Action(OnSprintingStarted));
				InputDefaultActions sprint2 = _inputService.Sprint;
				sprint2.Canceled = (Action)Delegate.Remove(sprint2.Canceled, new Action(OnSprintingEnded));
				_inputDeviceService.OnCurrentActiveDeviceChange -= OnCurrentActiveDeviceChanged;
				_fieldOfViewModel.OnFieldOfViewChanged -= OnFieldOfViewSettingChanged;
			}
		}

		private void OnLiveMovementInputChanged(Vector2 input)
		{
			_liveMovementInput = input;
		}

		public void RebindToAvatar(NetworkObject newAvatar)
		{
			if (!(newAvatar == null) && newAvatar.IsValid && base.HasInputAuthority)
			{
				_playerMovableModel.LocalMovable = this;
				_playerMovableModel.NetworkedAnimator = _networkedAnimator;
				_playerMovableModel.AllCharacterMovables[base.Object.InputAuthority] = this;
			}
		}

		private int QuantizeSpeed(float value)
		{
			return Mathf.RoundToInt(value / 0.1f);
		}

		private float DequantizeSpeed(int value)
		{
			return (float)value * 0.1f;
		}

		private void InitizeStats()
		{
			_walkSpeedStat = _statEntity.GetStat(EntityStatType.WalkSpeed);
			_walkSpeedStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.WalkSpeed]);
			_sprintSpeedStat = _statEntity.GetStat(EntityStatType.SprintSpeed);
			_sprintSpeedStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.SprintSpeed]);
			_speedStat = _statEntity.GetStat(EntityStatType.Speed);
			_speedStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.WalkSpeed]);
			_crouchSpeedStat = _statEntity.GetStat(EntityStatType.CrouchSpeed);
			_crouchSpeedStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.CrouchSpeed]);
			_punishmentStaminaStat = _statEntity.GetStat(EntityStatType.PunishmentStamina);
			_punishmentStaminaStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.PunishmentStamina]);
			_staminaRechargeRateStat = _statEntity.GetStat(EntityStatType.StaminaRechargeRate);
			_staminaRechargeRateStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.StaminaRechargeRate]);
			_staminaRechargeRateOnPunishStat = _statEntity.GetStat(EntityStatType.StaminaRechargeRateOnPunish);
			_staminaRechargeRateOnPunishStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.StaminaRechargeRateOnPunish]);
			_hiddenStaminaRechargeRateStat = _statEntity.GetStat(EntityStatType.HiddenStaminaRechargeRate);
			_hiddenStaminaRechargeRateStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.HiddenStaminaRechargeRate]);
			_hiddenStaminaDrainRateStat = _statEntity.GetStat(EntityStatType.HiddenStaminaDrainRate);
			_hiddenStaminaDrainRateStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.HiddenStaminaDrainRate]);
			_staminaDrainRateStat = _statEntity.GetStat(EntityStatType.StaminaDrainRate);
			_staminaDrainRateStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.StaminaDrainRate]);
			_staminaStat = _statEntity.GetStat(EntityStatType.Stamina);
			_hiddenStaminaStat = _statEntity.GetStat(EntityStatType.HiddenStamina);
		}

		private void Awake()
		{
			_originalLocalY = new Dictionary<Transform, float>();
			foreach (Transform crouchAffectedTransform in _crouchAffectedTransforms)
			{
				if (crouchAffectedTransform != null)
				{
					_originalLocalY[crouchAffectedTransform] = crouchAffectedTransform.localPosition.y;
				}
			}
			_originalColliderData = new Dictionary<CapsuleCollider, (float, float)>();
			foreach (CapsuleCollider capsuleCollider in _capsuleColliders)
			{
				_originalColliderData[capsuleCollider] = (capsuleCollider.height, capsuleCollider.center.y);
			}
		}

		public override Vector3 GetVelocity()
		{
			return _rigidbody.linearVelocity;
		}

		public override void DisableMovement()
		{
			_ignoreMovement = true;
			_velocity = Vector3.zero;
			if (!_rigidbody.isKinematic)
			{
				_rigidbody.linearVelocity = Vector3.zero;
			}
		}

		public override void EnableMovement()
		{
			_ignoreMovement = false;
			SetLockedGroundPlantActive(active: false);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.IsValid && base.HasInputAuthority && !_atomicSpawnApplied && PlayerSpawnLock.TryConsumePendingSpawn(_networkRigidbody, out var position, out var rotation))
			{
				ChangePosition(position, rotation, isForced: true);
				_atomicSpawnApplied = true;
			}
			WritePlatformAnchor();
			if (base.HasStateAuthority)
			{
				UpdateMovementState();
			}
			if (_isJumpDelayed)
			{
				_currentJumpDelay += base.Runner.DeltaTime;
			}
			if (_currentJumpDelay >= _jumpDelay)
			{
				_isJumpDelayed = false;
				_currentJumpDelay = 0f;
			}
			if (!_crouchPhaseEnabled && _isCrouching && base.HasInputAuthority)
			{
				Crouch();
			}
			if (!GetInput<NetworkInputActions>(out var input) || !_isMovementInputEnabled)
			{
				_playerMovableModel.CurrentMovementInput = (base.IsAutomaticForwardMovement ? new Vector3(0f, 0f, 1f) : Vector3.zero);
				return;
			}
			float z = (base.IsAutomaticForwardMovement ? 1f : input.MovementInput.y);
			Vector3 currentMovementInput = new Vector3(input.MovementInput.x, GetJump(input), z);
			_playerMovableModel.CurrentMovementInput = currentMovementInput;
			MoveTowardsInput(_playerMovableModel.CurrentMovementInput);
			_velocity = _rigidbody.linearVelocity;
			_playerMovableModel.Velocity = _velocity;
			UpdateSprint();
		}

		public void SetPlatformCarrier(Transform carrier, NetworkId carrierId)
		{
			_platformCarrier = carrier;
			_platformCarrierId = carrierId;
		}

		public void SetRefusedCarrier(Transform carrier)
		{
			RefusedCarrier = carrier;
		}

		public void ClearRefusedCarrier(Transform carrier)
		{
			if (RefusedCarrier == carrier)
			{
				RefusedCarrier = null;
			}
		}

		public void ClearPlatformCarrier(Transform carrier)
		{
			if (!(_platformCarrier != carrier))
			{
				_platformCarrier = null;
				_platformCarrierId = default(NetworkId);
				_hasPlatformOffset = false;
			}
		}

		public void StagePlatformAnchor(Vector3 localOffset)
		{
			_platformLocalOffset = localOffset;
			_hasPlatformOffset = true;
		}

		public void AddCarrierYaw(float deltaDegrees)
		{
			if (base.HasInputAuthority)
			{
				if (_cameraModel.Cameras.TryGetValue(_cameraModel.ActiveCameraType, out var value))
				{
					value.AddHorizontalRotation(deltaDegrees);
				}
				if (_playerMovableModel.Rotator != null)
				{
					_playerMovableModel.Rotator.AddBodyYaw(deltaDegrees);
				}
			}
		}

		private void WritePlatformAnchor()
		{
			if (base.HasStateAuthority)
			{
				if (_platformCarrier != null && _hasPlatformOffset)
				{
					PlatformCarrierId = _platformCarrierId;
					PlatformCarrierLocalPosition = _platformLocalOffset;
				}
				else if (PlatformCarrierId.IsValid)
				{
					PlatformCarrierId = default(NetworkId);
				}
			}
		}

		private void FixedUpdate()
		{
			_isGrounded = CheckIsGrounded();
			ClearJumpEscapeOnLanding();
			ApplyGroundedSlopeStabilization();
			UpdateLockedGroundPlantConstraints();
		}

		private void ClearJumpEscapeOnLanding()
		{
			if (_isJumpEscaping && _isGrounded && _rigidbody.linearVelocity.y <= 0f && !_isJumpDelayed)
			{
				_isJumpEscaping = false;
			}
		}

		private void ApplyGroundedSlopeStabilization()
		{
			if (!base.HasStateAuthority || !_isSpawned || _playerMovableModel.IsFlying || _rigidbody.isKinematic || _playerRagdollEntity.IsSimulated || !_isGrounded || (!_ignoreMovement && _isJumpDelayed) || (!_ignoreMovement && HasActiveMovementInput()))
			{
				return;
			}
			if (!TryGetGroundHitBelowPlayer(out var hit))
			{
				if (_ignoreMovement)
				{
					ZeroHorizontalVelocity();
				}
				return;
			}
			float num = Vector3.Angle(hit.normal, Vector3.up);
			if ((_ignoreMovement || (!(num < _slopeStabilizeMinAngle) && !(num > _slopeStabilizeMaxWalkableAngle))) && (!_ignoreMovement || !(num > _slopeStabilizeMaxWalkableAngle)))
			{
				Vector3 vector = Vector3.ProjectOnPlane(Physics.gravity, hit.normal);
				if (vector.sqrMagnitude > 0.0001f)
				{
					_rigidbody.AddForce(-vector * _rigidbody.mass, ForceMode.Force);
				}
				Vector3 linearVelocity = _rigidbody.linearVelocity;
				Vector3 vector2 = Vector3.ProjectOnPlane(linearVelocity, hit.normal);
				if (!(vector2.sqrMagnitude <= 0.0001f))
				{
					_rigidbody.linearVelocity = linearVelocity - vector2;
				}
			}
		}

		private void UpdateLockedGroundPlantConstraints()
		{
			if (!base.HasStateAuthority || !_isSpawned || _rigidbody.isKinematic)
			{
				SetLockedGroundPlantActive(active: false);
				return;
			}
			bool lockedGroundPlantActive = _ignoreMovement && _isGrounded && !_playerMovableModel.IsFlying && !_playerRagdollEntity.IsSimulated;
			SetLockedGroundPlantActive(lockedGroundPlantActive);
		}

		private void SetLockedGroundPlantActive(bool active)
		{
			if (_lockedGroundPlantActive != active)
			{
				if (active)
				{
					_rigidbody.constraints |= (RigidbodyConstraints)10;
				}
				else
				{
					_rigidbody.constraints &= (RigidbodyConstraints)(-11);
				}
				_lockedGroundPlantActive = active;
			}
		}

		private void ZeroHorizontalVelocity()
		{
			Vector3 linearVelocity = _rigidbody.linearVelocity;
			if (!(linearVelocity.x * linearVelocity.x + linearVelocity.z * linearVelocity.z <= 0.0001f))
			{
				_rigidbody.linearVelocity = new Vector3(0f, linearVelocity.y, 0f);
			}
		}

		private bool HasActiveMovementInput()
		{
			if (!_isMovementInputEnabled)
			{
				return false;
			}
			if (base.IsAutomaticForwardMovement)
			{
				return true;
			}
			if (base.HasInputAuthority)
			{
				return _liveMovementInput.sqrMagnitude > 0.0001f;
			}
			return _playerMovableModel.CurrentMovementInput.sqrMagnitude > 0.0001f;
		}

		private void Update()
		{
			if (!_isSpawned || !IsReady)
			{
				return;
			}
			if (base.Object.HasStateAuthority)
			{
				UpdateStamina();
				UpdateFOV();
				float animationSpeed = GetAnimationSpeed();
				_smoothedSpeedContinuous = Mathf.SmoothDamp(_smoothedSpeedContinuous, animationSpeed, ref _speedVelocity, _speedSmoothTime);
				int num = QuantizeSpeed(_smoothedSpeedContinuous);
				if (num != _smoothedSpeedQuantized)
				{
					_smoothedSpeedQuantized = num;
				}
				_animator.SetFloat(_currentSpeed, _smoothedSpeedContinuous);
				UpdateSpeed();
			}
			else
			{
				float b = DequantizeSpeed(_smoothedSpeedQuantized);
				_visualSpeed = Mathf.Lerp(_visualSpeed, b, Time.deltaTime * 10f);
				_animator.SetFloat(_currentSpeed, _visualSpeed);
			}
		}

		private void UpdateSpeed()
		{
			if (_isSprinting)
			{
				_speedStat.OverrideValue(_sprintSpeedStat.FullValue);
			}
			else if (_isCrouching)
			{
				_speedStat.OverrideValue(_crouchSpeedStat.FullValue);
			}
			else
			{
				_speedStat.OverrideValue(_walkSpeedStat.FullValue);
			}
		}

		private float GetAnimationSpeed()
		{
			Vector3 linearVelocity = _rigidbody.linearVelocity;
			float magnitude = new Vector3(linearVelocity.x, 0f, linearVelocity.z).magnitude;
			if (!(magnitude >= _animationIdleSpeedThreshold))
			{
				return 0f;
			}
			return magnitude;
		}

		private void UpdateMovementState()
		{
			if (_isSprinting)
			{
				base.MovementState = MovementState.Sprinting;
			}
			else if (_isCrouching)
			{
				base.MovementState = MovementState.Crouching;
			}
			else
			{
				base.MovementState = MovementState.Walking;
			}
		}

		private void LateUpdate()
		{
			if (_isCrouchTransitioning)
			{
				UpdateCrouchTransition();
			}
		}

		private void UpdateSprint()
		{
			if (_isForcedCrouching)
			{
				if (_isSprinting)
				{
					StopSprint();
				}
				return;
			}
			bool flag = _playerMovableModel.CurrentMovementInput.sqrMagnitude > 0.0001f;
			UpdateToggledSprintAutoRelease(flag);
			bool flag2 = _hiddenStaminaStat.FullValue > 0f && !_isCrouching && flag;
			bool flag3 = _hiddenStaminaStat.FullValue > 0f && !_isCrouching && flag;
			if (_wantsToSprint && !_isSprinting && flag2)
			{
				StartSprint();
			}
			else if (_isSprinting && (!_wantsToSprint || !flag3))
			{
				StopSprint();
			}
		}

		private void UpdateToggledSprintAutoRelease(bool isMoving)
		{
			if (!_isSprintToggledByGamepad || isMoving)
			{
				_toggledSprintNoInputTimer = 0f;
				return;
			}
			_toggledSprintNoInputTimer += base.Runner.DeltaTime;
			if (!(_toggledSprintNoInputTimer < _toggledSprintNoInputReleaseSeconds))
			{
				_toggledSprintNoInputTimer = 0f;
				_isSprintToggledByGamepad = false;
				_wantsToSprint = false;
			}
		}

		private void StartSprint()
		{
			_isSprinting = true;
			_staminaRechargeTimer = 0f;
			_targetFOV = ResolveTargetFOV();
		}

		private void StopSprint()
		{
			_isSprinting = false;
			_staminaRechargeTimer = _staminaRechargeDelay;
			_targetFOV = ResolveTargetFOV();
		}

		private float ResolveTargetFOV()
		{
			if (!_isSprinting)
			{
				return _fieldOfViewModel.FieldOfView;
			}
			return _fieldOfViewModel.FieldOfView + (_sprintFOV - _walkFOV);
		}

		private void OnFieldOfViewSettingChanged(float fieldOfView)
		{
			_targetFOV = ResolveTargetFOV();
		}

		private void UpdateStamina()
		{
			if (_playerMovableModel.IsFlying)
			{
				return;
			}
			if ((_isSprinting || IsCarryingHeavyItem()) && !_ignoreMovement)
			{
				float num = 0f;
				if (_isSprinting)
				{
					num += _staminaDrainRateStat.FullValue;
				}
				if (IsCarryingHeavyItem())
				{
					num += _carryingStaminaDrainRate * _heavyItemLocalModel.HeavyItemMultiplier;
				}
				SubtractStaminaLogic(num);
				return;
			}
			_hiddenStaminaStat.AddValue(_hiddenStaminaRechargeRateStat.FullValue * Time.deltaTime);
			if (_hiddenStaminaStat.FullValue > _playerStatsConfiguration.PlayerStats[EntityStatType.HiddenStamina])
			{
				_hiddenStaminaStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.HiddenStamina]);
			}
			if (_staminaRechargeTimer > 0f)
			{
				_staminaRechargeTimer -= Time.deltaTime;
			}
			else if (_staminaStat.FullValue > _punishmentStaminaStat.FullValue)
			{
				_staminaStat.AddValue(_staminaRechargeRateStat.FullValue * Time.deltaTime);
			}
			else
			{
				_staminaStat.AddValue(_staminaRechargeRateOnPunishStat.FullValue * Time.deltaTime);
			}
		}

		private void UpdateFOV()
		{
			if (_cameraModel.Cameras.ContainsKey(Features.CameraModelModule.CameraType.FPCamera))
			{
				float fOV = _cameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].GetFOV();
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetFOV(Mathf.Lerp(fOV, _targetFOV, _fovTransitionSpeed * Time.deltaTime));
			}
		}

		private void SubtractStaminaLogic(float staminaDrainRate)
		{
			_playerStaminaService.SubtractStaminaLogic(Time.deltaTime, staminaDrainRate);
			if (_staminaStat.FullValue <= 0f && _hiddenStaminaStat.FullValue <= 0f)
			{
				StopSprint();
			}
			_staminaRechargeTimer = _staminaRechargeDelay;
		}

		private float GetJump(NetworkInputActions input)
		{
			if (!input.JumpPhase.IsSet(InputActionPhase.Performed) || _isCrouching)
			{
				return 0f;
			}
			if (_ignoreMovement)
			{
				return 0f;
			}
			if (_playerMovableModel.IsJumpBlocked)
			{
				return 0f;
			}
			if (!_isGrounded)
			{
				return 0f;
			}
			if (_currentJumpDelay > 0f)
			{
				return 0f;
			}
			_isJumpDelayed = true;
			_isJumpEscaping = true;
			return 1f;
		}

		public override void MoveTowardsInput(Vector3 input, bool overrideSpeed = false)
		{
			if (base.HasStateAuthority && !(_cameraModel.CameraObject == null))
			{
				if (_ignoreMovement)
				{
					input = Vector3.zero;
				}
				float mass = _rigidbody.mass;
				if (input.y > 0f)
				{
					_rigidbody.AddForce(Vector3.up * _jumpImpulse * mass, ForceMode.Force);
				}
				float fullValue = _speedStat.FullValue;
				Vector3 forward = _cameraModel.CameraObject.transform.forward;
				forward.y = 0f;
				forward = ((forward.sqrMagnitude > 0.0001f) ? forward.normalized : Vector3.forward);
				Vector3 vector = Vector3.Cross(Vector3.up, forward) * input.x + forward * input.z;
				Vector3 target = ((vector.sqrMagnitude > 0.0001f) ? (vector.normalized * fullValue) : Vector3.zero);
				if (base.IsAutomaticForwardMovement)
				{
					target = new Vector3(target.x, target.y, target.z * _followForwardSpeedMultiplier);
				}
				Vector3 linearVelocity = _rigidbody.linearVelocity;
				Vector3 current = new Vector3(linearVelocity.x, 0f, linearVelocity.z);
				float deltaTime = base.Runner.DeltaTime;
				Vector3 vector2 = Vector3.MoveTowards(maxDistanceDelta: ((vector.sqrMagnitude > 0.0001f) ? _acceleration : _deceleration) * deltaTime, current: current, target: target);
				if (!_rigidbody.isKinematic)
				{
					_rigidbody.linearVelocity = new Vector3(vector2.x, _rigidbody.linearVelocity.y, vector2.z);
					_velocity = _rigidbody.linearVelocity;
					_playerMovableModel.Velocity = _velocity;
				}
			}
		}

		public override void MoveTowardsPosition(Vector3 position)
		{
			if (!_ignoreMovement && base.HasStateAuthority)
			{
				float fullValue = _speedStat.FullValue;
				Vector3 vector = position - GetPosition();
				if (!(vector.sqrMagnitude < 0.0001f))
				{
					Vector3 vector2 = vector.normalized * fullValue;
					Vector3 linearVelocity = _rigidbody.linearVelocity;
					float deltaTime = base.Runner.DeltaTime;
					Vector3 vector3 = vector2 - linearVelocity;
					float maxLength = _acceleration * deltaTime;
					Vector3 vector4 = Vector3.ClampMagnitude(vector3, maxLength) / deltaTime;
					_rigidbody.AddForce(vector4 * _rigidbody.mass, ForceMode.Force);
					_velocity = _rigidbody.linearVelocity;
					_playerMovableModel.Velocity = _velocity;
				}
			}
		}

		public override void ChangePosition(Vector3 position, Quaternion? rotation = null, bool isForced = false)
		{
			if (!base.HasStateAuthority || PlayerSpawnLock.ShouldBlockPositionOverride(isForced) || (_ignoreMovement && !isForced))
			{
				return;
			}
			if (_playerRagdollEntity.IsSimulated)
			{
				_playerRagdollEntity.Teleport(position, rotation);
			}
			else
			{
				_rigidbody.position = position;
				if (rotation.HasValue)
				{
					_rigidbody.rotation = rotation.Value;
				}
				_rigidbody.PublishTransform();
			}
			OnChangePosition?.Invoke();
		}

		public override void Jump()
		{
			if (!_ignoreMovement && base.HasStateAuthority && !_isCrouching && Physics.Raycast(GetPosition(), Vector3.down, out var _, _jumpRaycastDistance))
			{
				_rigidbody.AddForce(Vector3.up * _jumpImpulse, ForceMode.Impulse);
				_velocity = _rigidbody.linearVelocity;
				_playerMovableModel.Velocity = _velocity;
			}
		}

		public override void Crouch()
		{
			if (!_isCrouchTransitioning && _isGrounded && !_isForcedCrouching && !_ignoreMovement && (!_isCrouching || CanStandUp()))
			{
				CrouchUtil(!_isCrouching, immediately: false);
			}
		}

		public override void TryCrouchImmediately(bool isCrouching)
		{
			if (!CanStandUp() && _isGrounded && !_isForcedCrouching)
			{
				CrouchUtil(isCrouching, immediately: true);
			}
		}

		public override void ForceCrouch(bool isCrouching)
		{
			_isForcedCrouching = isCrouching;
			if (isCrouching)
			{
				_wantsToSprint = false;
				_isSprintToggledByGamepad = false;
				_isCrouchToggledByGamepad = false;
				if (_isSprinting)
				{
					StopSprint();
				}
			}
			if (_isCrouching == isCrouching)
			{
				if (base.Object != null)
				{
					ForcedCrouchRPC(isCrouching);
				}
			}
			else if (isCrouching || CanStandUp())
			{
				CrouchUtil(isCrouching, immediately: false, syncAnimator: false);
			}
		}

		private void CrouchUtil(bool isCrouching, bool immediately, bool syncAnimator = true)
		{
			if (immediately)
			{
				if (syncAnimator)
				{
					CrouchImmediatelyRPC(isCrouching);
				}
				else
				{
					ForcedCrouchImmediatelyRPC(isCrouching);
				}
			}
			else if (base.Object != null)
			{
				if (syncAnimator)
				{
					CrouchRPC(isCrouching);
				}
				else
				{
					ForcedCrouchRPC(isCrouching);
				}
			}
		}

		public bool CanStandUp()
		{
			Vector3 position = CameraPositionTransform.position;
			float crouchCheckRadius = _crouchCheckRadius;
			Transform obj = ((RotatePoint != null) ? RotatePoint : base.transform);
			Vector3 right = obj.right;
			Vector3 forward = obj.forward;
			right.y = 0f;
			forward.y = 0f;
			if (right.sqrMagnitude < 1E-06f || forward.sqrMagnitude < 1E-06f)
			{
				right = Vector3.right;
				forward = Vector3.forward;
			}
			else
			{
				right.Normalize();
				forward.Normalize();
			}
			Collider blockingCollider;
			bool num = RayClear(position, out blockingCollider);
			Collider blockingCollider2;
			bool flag = RayClear(position - right * crouchCheckRadius, out blockingCollider2);
			Collider blockingCollider3;
			bool flag2 = RayClear(position + right * crouchCheckRadius, out blockingCollider3);
			Collider blockingCollider4;
			bool flag3 = RayClear(position + forward * crouchCheckRadius, out blockingCollider4);
			Collider blockingCollider5;
			bool flag4 = RayClear(position - forward * crouchCheckRadius, out blockingCollider5);
			bool num2 = num && flag && flag2 && flag3 && flag4;
			if (!num2)
			{
				IsStandUpBlocked = true;
				StandUpBlockingCollider = blockingCollider ?? blockingCollider2 ?? blockingCollider3 ?? blockingCollider4 ?? blockingCollider5;
				if (base.HasInputAuthority && _isCrouching && StandUpBlockingCollider != null)
				{
					NotifyStandUpBlockingResponderBlocked(StandUpBlockingCollider);
					return num2;
				}
			}
			else
			{
				if (base.HasInputAuthority && IsStandUpBlocked)
				{
					NotifyStandUpBlockingResponderUnblocked(StandUpBlockingCollider);
				}
				IsStandUpBlocked = false;
				StandUpBlockingCollider = null;
			}
			return num2;
		}

		private bool RayClear(Vector3 origin, out Collider blockingCollider)
		{
			int num = Physics.RaycastNonAlloc(origin, Vector3.up, _crouchCheckHits, _crouchDeltaHeight, _crouchCheckMask);
			blockingCollider = null;
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				Collider collider = _crouchCheckHits[i].collider;
				if (!(collider == null) && !IsRefusedCarrierCollider(collider) && !(_crouchCheckHits[i].distance >= num2))
				{
					num2 = _crouchCheckHits[i].distance;
					blockingCollider = collider;
				}
			}
			return blockingCollider == null;
		}

		private bool IsRefusedCarrierCollider(Collider candidate)
		{
			if (RefusedCarrier != null)
			{
				return candidate.transform.IsChildOf(RefusedCarrier);
			}
			return false;
		}

		private void NotifyStandUpBlockingResponderBlocked(Collider blocker)
		{
			if (!(blocker == null))
			{
				blocker.GetComponentInParent<IStandUpBlockingResponder>()?.OnLocalPlayerStandUpBlocked(base.Object.InputAuthority.PlayerId, base.transform.position, blocker);
			}
		}

		private void NotifyStandUpBlockingResponderUnblocked(Collider blocker)
		{
			if (!(blocker == null))
			{
				blocker.GetComponentInParent<IStandUpBlockingResponder>()?.OnLocalPlayerStandUpNoLongerBlocked(base.Object.InputAuthority.PlayerId, blocker);
			}
		}

		public bool IsHardCrouch()
		{
			if (!CanStandUp())
			{
				return _isCrouching;
			}
			return false;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2098769687u)]
		private void CrouchRPC([RpcPayload(4)] bool isCrouching)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isCrouching);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2098769687u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.PlayerCharacterMovableBase::CrouchRPC(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isCrouching);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_animator.SetBool(_isCrouch, isCrouching);
			_animator.SetBool(_isCrouchImmediately, value: false);
			_isCrouching = isCrouching;
			_isCrouchTransitioning = true;
			if (!isCrouching)
			{
				if (base.HasInputAuthority)
				{
					NotifyStandUpBlockingResponderUnblocked(StandUpBlockingCollider);
				}
				IsStandUpBlocked = false;
				StandUpBlockingCollider = null;
			}
			if (base.HasInputAuthority)
			{
				PlayerSessionPrefs.SaveCrouching(isCrouching);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 315714411u)]
		private void CrouchImmediatelyRPC([RpcPayload(4)] bool isCrouching)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isCrouching);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(315714411u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.PlayerCharacterMovableBase::CrouchImmediatelyRPC(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isCrouching);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_animator.SetBool(_isCrouch, isCrouching);
			_animator.SetBool(_isCrouchImmediately, value: true);
			_isCrouching = isCrouching;
			if (!isCrouching)
			{
				if (base.HasInputAuthority)
				{
					NotifyStandUpBlockingResponderUnblocked(StandUpBlockingCollider);
				}
				IsStandUpBlocked = false;
				StandUpBlockingCollider = null;
			}
			CrouchTransitionImmediately(isCrouching);
			if (base.HasInputAuthority)
			{
				PlayerSessionPrefs.SaveCrouching(isCrouching);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3128644206u)]
		private void ForcedCrouchRPC([RpcPayload(4)] bool isCrouching)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isCrouching);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3128644206u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.PlayerCharacterMovableBase::ForcedCrouchRPC(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isCrouching);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (isCrouching)
			{
				_animator.SetBool(_isCrouch, value: false);
			}
			if (_isCrouching == isCrouching)
			{
				if (!isCrouching)
				{
					return;
				}
			}
			else
			{
				_isCrouching = isCrouching;
				_isCrouchTransitioning = true;
			}
			if (!isCrouching)
			{
				if (base.HasInputAuthority)
				{
					NotifyStandUpBlockingResponderUnblocked(StandUpBlockingCollider);
				}
				IsStandUpBlocked = false;
				StandUpBlockingCollider = null;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1661697346u)]
		private void ForcedCrouchImmediatelyRPC([RpcPayload(4)] bool isCrouching)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isCrouching);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1661697346u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.PlayerCharacterMovableBase::ForcedCrouchImmediatelyRPC(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isCrouching);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (isCrouching)
			{
				_animator.SetBool(_isCrouch, value: false);
			}
			_isCrouching = isCrouching;
			if (!isCrouching)
			{
				if (base.HasInputAuthority)
				{
					NotifyStandUpBlockingResponderUnblocked(StandUpBlockingCollider);
				}
				IsStandUpBlocked = false;
				StandUpBlockingCollider = null;
			}
			CrouchTransitionImmediately(isCrouching);
		}

		public override Vector3 GetPosition()
		{
			if (_rigidbody == null || _playerRagdollEntity == null)
			{
				return Vector3.zero;
			}
			if (!_playerRagdollEntity.IsSimulated)
			{
				return _rigidbody.position;
			}
			return _playerRagdollEntity.RootPhysData.RigidBody.position;
		}

		public Vector3 GetReconnectPlacementPosition()
		{
			if (_playerRagdollEntity != null && _playerRagdollEntity.IsInitialized && _playerRagdollEntity.IsSimulated)
			{
				return _playerRagdollEntity.GetReconnectStandUpPosition();
			}
			return GetPosition();
		}

		public override void Warp(Vector3 position)
		{
			if (!PlayerSpawnLock.ShouldBlockPositionOverride())
			{
				bool ignoreMovement = _ignoreMovement;
				_ignoreMovement = true;
				_rigidbody.linearVelocity = Vector3.zero;
				_velocity = Vector3.zero;
				_networkRigidbody.Teleport(position);
				_ignoreMovement = ignoreMovement;
			}
		}

		public override bool IsMoving()
		{
			return _rigidbody.linearVelocity.magnitude > 1f;
		}

		private void CrouchTransitionImmediately(bool isCrouching)
		{
			_isCrouchTransitioning = false;
			_crouchProgress = (isCrouching ? 1f : 0f);
			float num = (isCrouching ? 1f : 0f);
			foreach (Transform crouchAffectedTransform in _crouchAffectedTransforms)
			{
				if (!(crouchAffectedTransform == null) && _originalLocalY.TryGetValue(crouchAffectedTransform, out var value))
				{
					Vector3 localPosition = crouchAffectedTransform.localPosition;
					localPosition.y = value - _crouchDeltaHeight * num;
					crouchAffectedTransform.localPosition = localPosition;
				}
			}
			foreach (CapsuleCollider capsuleCollider in _capsuleColliders)
			{
				if (!(capsuleCollider == null) && _originalColliderData.TryGetValue(capsuleCollider, out (float, float) value2))
				{
					float height;
					if (!isCrouching)
					{
						(height, _) = value2;
					}
					else
					{
						height = value2.Item1 - _crouchDeltaHeight;
					}
					capsuleCollider.height = height;
					Vector3 center = capsuleCollider.center;
					center.y = (isCrouching ? (value2.Item2 - _crouchDeltaHeight * 0.5f) : value2.Item2);
					capsuleCollider.center = center;
				}
			}
		}

		private void UpdateCrouchTransition()
		{
			float num = (_isCrouching ? 1f : 0f);
			float num2 = 1f / _crouchDuration;
			_crouchProgress = Mathf.MoveTowards(_crouchProgress, num, num2 * Time.deltaTime);
			float t = 1f - Mathf.Pow(1f - _crouchProgress, 3f);
			foreach (Transform crouchAffectedTransform in _crouchAffectedTransforms)
			{
				if (!(crouchAffectedTransform == null) && _originalLocalY.TryGetValue(crouchAffectedTransform, out var value))
				{
					Vector3 localPosition = crouchAffectedTransform.localPosition;
					float b = value - _crouchDeltaHeight;
					localPosition.y = Mathf.Lerp(value, b, t);
					crouchAffectedTransform.localPosition = localPosition;
				}
			}
			foreach (CapsuleCollider capsuleCollider in _capsuleColliders)
			{
				if (!(capsuleCollider == null) && _originalColliderData.TryGetValue(capsuleCollider, out (float, float) value2))
				{
					float b2 = value2.Item1 - _crouchDeltaHeight;
					float b3 = value2.Item2 - _crouchDeltaHeight * 0.5f;
					capsuleCollider.height = Mathf.Lerp(value2.Item1, b2, t);
					Vector3 center = capsuleCollider.center;
					center.y = Mathf.Lerp(value2.Item2, b3, t);
					capsuleCollider.center = center;
				}
			}
			if (Mathf.Approximately(_crouchProgress, num))
			{
				_isCrouchTransitioning = false;
			}
		}

		private bool CheckIsGrounded()
		{
			Vector3 position = GetPosition();
			if (!GroundRay(position, out var hit) && !GroundRay(position + Vector3.left, out hit) && !GroundRay(position + Vector3.right, out hit) && !GroundRay(position + Vector3.forward, out hit))
			{
				return GroundRay(position + Vector3.back, out hit);
			}
			return true;
		}

		private bool TryGetGroundHitBelowPlayer(out RaycastHit hit)
		{
			Vector3 position = GetPosition();
			if (GroundRay(position, out hit))
			{
				return true;
			}
			if (GroundRay(position + Vector3.left, out hit))
			{
				return true;
			}
			if (GroundRay(position + Vector3.right, out hit))
			{
				return true;
			}
			if (GroundRay(position + Vector3.forward, out hit))
			{
				return true;
			}
			return GroundRay(position + Vector3.back, out hit);
		}

		private bool GroundRay(Vector3 origin, out RaycastHit hit)
		{
			if (RefusedCarrier == null)
			{
				return Physics.Raycast(origin, Vector3.down, out hit, _jumpRaycastDistance, _groundLayerMask);
			}
			hit = default(RaycastHit);
			int num = Physics.RaycastNonAlloc(origin, Vector3.down, _groundCheckHits, _jumpRaycastDistance, _groundLayerMask);
			float num2 = float.MaxValue;
			bool result = false;
			for (int i = 0; i < num; i++)
			{
				Collider collider = _groundCheckHits[i].collider;
				if (!(collider == null) && !IsRefusedCarrierCollider(collider) && !(_groundCheckHits[i].distance >= num2))
				{
					num2 = _groundCheckHits[i].distance;
					hit = _groundCheckHits[i];
					result = true;
				}
			}
			return result;
		}

		private void OnDrawGizmos()
		{
			Vector3 position = GetPosition();
			DrawGroundCheckRay(position, Vector3.down);
			DrawGroundCheckRay(position + Vector3.left * _mainCapsuleCollider.radius * _mainCapsuleCollider.transform.localScale.x, Vector3.down);
			DrawGroundCheckRay(position + Vector3.right * _mainCapsuleCollider.radius * _mainCapsuleCollider.transform.localScale.x, Vector3.down);
			DrawGroundCheckRay(position + Vector3.forward * _mainCapsuleCollider.radius * _mainCapsuleCollider.transform.localScale.z, Vector3.down);
			DrawGroundCheckRay(position + Vector3.back * _mainCapsuleCollider.radius * _mainCapsuleCollider.transform.localScale.z, Vector3.down);
		}

		private void DrawGroundCheckRay(Vector3 origin, Vector3 direction)
		{
			RaycastHit hitInfo;
			bool num = Physics.Raycast(origin, direction, out hitInfo, _jumpRaycastDistance);
			Gizmos.color = (num ? Color.green : Color.red);
			if (num)
			{
				Gizmos.DrawLine(origin, hitInfo.point);
				Gizmos.DrawSphere(hitInfo.point, 0.05f);
			}
			else
			{
				Gizmos.DrawLine(origin, origin + direction * _jumpRaycastDistance);
			}
		}

		public void SetMovementInputEnabled(bool isEnabled)
		{
			_isMovementInputEnabled = isEnabled;
		}

		public void SetLinearVelocity(Vector3 linearVelocity)
		{
			_rigidbody.linearVelocity = linearVelocity;
			_rigidbody.angularVelocity = Vector3.zero;
			if (!(_playerRagdollEntity == null) && _playerRagdollEntity.IsInitialized)
			{
				Rigidbody rigidBody = _playerRagdollEntity.RootPhysData.RigidBody;
				if (rigidBody != null && !rigidBody.isKinematic)
				{
					rigidBody.linearVelocity = linearVelocity;
					rigidBody.angularVelocity = Vector3.zero;
				}
			}
		}

		private bool IsCarryingHeavyItem()
		{
			return _heavyItemLocalModel.IsCurrentlyGrabbedHeavyItem;
		}

		private void OnCrouchStarted()
		{
			_isCrouchInputHeld = true;
			if (_isCrouchToggledByGamepad && _isCrouching)
			{
				_isCrouchToggledByGamepad = false;
				_crouchPhaseEnabled = false;
			}
			else
			{
				_crouchPhaseEnabled = true;
				Crouch();
				_isCrouchToggledByGamepad = IsControllerActive() && _isCrouching;
			}
		}

		private void OnCrouchEnded()
		{
			_isCrouchInputHeld = false;
			if (!_isCrouchToggledByGamepad)
			{
				_crouchPhaseEnabled = false;
			}
		}

		private void OnSprintingStarted()
		{
			_isSprintInputHeld = true;
			if (!_isForcedCrouching)
			{
				if (IsControllerActive())
				{
					_wantsToSprint = !_wantsToSprint;
					_isSprintToggledByGamepad = _wantsToSprint;
				}
				else
				{
					_isSprintToggledByGamepad = false;
					_wantsToSprint = true;
				}
			}
		}

		private void OnSprintingEnded()
		{
			_isSprintInputHeld = false;
			if (!_isForcedCrouching && !_isSprintToggledByGamepad)
			{
				_wantsToSprint = false;
			}
		}

		private void OnCurrentActiveDeviceChanged(InputDevice inputDevice)
		{
			if (_inputDeviceService.IsDeviceController(inputDevice))
			{
				return;
			}
			if (_isSprintToggledByGamepad)
			{
				_isSprintToggledByGamepad = false;
				if (!_isSprintInputHeld)
				{
					_wantsToSprint = false;
				}
			}
			if (_isCrouchToggledByGamepad)
			{
				_isCrouchToggledByGamepad = false;
				if (!_isCrouchInputHeld)
				{
					_crouchPhaseEnabled = false;
				}
			}
		}

		private bool IsControllerActive()
		{
			if (!_inputDeviceService.IsCurrentActiveDeviceGamepad())
			{
				return _inputDeviceService.IsCurrentActiveDeviceJoystick();
			}
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsKinematicInternal = _IsKinematicInternal;
			PlatformCarrierId = _PlatformCarrierId;
			PlatformCarrierLocalPosition = _PlatformCarrierLocalPosition;
			_smoothedSpeedQuantized = __smoothedSpeedQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsKinematicInternal = IsKinematicInternal;
			_PlatformCarrierId = PlatformCarrierId;
			_PlatformCarrierLocalPosition = PlatformCarrierLocalPosition;
			__smoothedSpeedQuantized = _smoothedSpeedQuantized;
		}

		[NetworkRpcWeavedInvoker(2098769687u)]
		[Preserve]
		[WeaverGenerated]
		protected static void CrouchRPC_0040Invoker2098769687([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerCharacterMovableBase)context.TargetBehaviour).CrouchRPC(value);
		}

		[NetworkRpcWeavedInvoker(315714411u)]
		[Preserve]
		[WeaverGenerated]
		protected static void CrouchImmediatelyRPC_0040Invoker315714411([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerCharacterMovableBase)context.TargetBehaviour).CrouchImmediatelyRPC(value);
		}

		[NetworkRpcWeavedInvoker(3128644206u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ForcedCrouchRPC_0040Invoker3128644206([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerCharacterMovableBase)context.TargetBehaviour).ForcedCrouchRPC(value);
		}

		[NetworkRpcWeavedInvoker(1661697346u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ForcedCrouchImmediatelyRPC_0040Invoker1661697346([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerCharacterMovableBase)context.TargetBehaviour).ForcedCrouchImmediatelyRPC(value);
		}
	}
}
