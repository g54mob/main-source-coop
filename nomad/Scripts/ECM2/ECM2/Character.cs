using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECM2
{
	[RequireComponent(typeof(CharacterMovement))]
	public class Character : MonoBehaviour
	{
		public enum MovementMode
		{
			None = 0,
			Walking = 1,
			Falling = 2,
			Flying = 3,
			Swimming = 4,
			Custom = 5
		}

		public enum RotationMode
		{
			None = 0,
			OrientRotationToMovement = 1,
			OrientRotationToViewDirection = 2,
			OrientWithRootMotion = 3,
			Custom = 4
		}

		public delegate void PhysicsVolumeChangedEventHandler(PhysicsVolume newPhysicsVolume);

		public delegate void MovementModeChangedEventHandler(MovementMode prevMovementMode, int prevCustomMode);

		public delegate void CustomMovementModeUpdateEventHandler(float deltaTime);

		public delegate void CustomRotationModeUpdateEventHandler(float deltaTime);

		public delegate void BeforeSimulationUpdateEventHandler(float deltaTime);

		public delegate void AfterSimulationUpdateEventHandler(float deltaTime);

		public delegate void CharacterMovementUpdateEventHandler(float deltaTime);

		public delegate void CollidedEventHandler(ref CollisionResult collisionResult);

		public delegate void FoundGroundEventHandler(ref FindGroundResult foundGround);

		public delegate void LandedEventHandled(Vector3 landingVelocity);

		public delegate void CrouchedEventHandler();

		public delegate void UnCrouchedEventHandler();

		public delegate void JumpedEventHandler();

		public delegate void ReachedJumpApexEventHandler();

		[Space(15f)]
		[Tooltip("The Character's current rotation mode.")]
		[SerializeField]
		private RotationMode _rotationMode;

		[Tooltip("Change in rotation per second (Deg / s).\nUsed when rotation mode is OrientRotationToMovement or OrientRotationToViewDirection.")]
		[SerializeField]
		private float _rotationRate;

		[Space(15f)]
		[Tooltip("The Character's default movement mode. Used at player startup.")]
		[SerializeField]
		private MovementMode _startingMovementMode;

		[Space(15f)]
		[Tooltip("The maximum ground speed when walking.\nAlso determines maximum lateral speed when falling.")]
		[SerializeField]
		private float _maxWalkSpeed;

		[Tooltip("The ground speed that we should accelerate up to when walking at minimum analog stick tilt.")]
		[SerializeField]
		private float _minAnalogWalkSpeed;

		[Tooltip("Max Acceleration (rate of change of velocity).")]
		[SerializeField]
		private float _maxAcceleration;

		[Tooltip("Deceleration when walking and not applying acceleration.\nThis is a constant opposing force that directly lowers velocity by a constant value.")]
		[SerializeField]
		private float _brakingDecelerationWalking;

		[Tooltip("Setting that affects movement control.\nHigher values allow faster changes in direction.\nIf useSeparateBrakingFriction is false, also affects the ability to stop more quickly when braking (whenever acceleration is zero).")]
		[SerializeField]
		private float _groundFriction;

		[Space(15f)]
		[Tooltip("Is the character able to crouch ?")]
		[SerializeField]
		private bool _canEverCrouch;

		[Tooltip("If canEverCrouch == true, determines the character height when crouched.")]
		[SerializeField]
		private float _crouchedHeight;

		[Tooltip("If canEverCrouch == true, determines the character height when un crouched.")]
		[SerializeField]
		private float _unCrouchedHeight;

		[Tooltip("The maximum ground speed while crouched.")]
		[SerializeField]
		private float _maxWalkSpeedCrouched;

		[Space(15f)]
		[Tooltip("The maximum vertical velocity a Character can reach when falling. Eg: Terminal velocity.")]
		[SerializeField]
		private float _maxFallSpeed;

		[Tooltip("Lateral deceleration when falling and not applying acceleration.")]
		[SerializeField]
		private float _brakingDecelerationFalling;

		[Tooltip("Friction to apply to lateral movement when falling. \nIf useSeparateBrakingFriction is false, also affects the ability to stop more quickly when braking (whenever acceleration is zero).")]
		[SerializeField]
		private float _fallingLateralFriction;

		[Range(0f, 1f)]
		[Tooltip("When falling, amount of lateral movement control available to the Character.\n0 = no control, 1 = full control at max acceleration.")]
		[SerializeField]
		private float _airControl;

		[Space(15f)]
		[Tooltip("Is the character able to jump ?")]
		[SerializeField]
		private bool _canEverJump;

		[Tooltip("Can jump while crouching ?")]
		[SerializeField]
		private bool _canJumpWhileCrouching;

		[Tooltip("The max number of jumps the Character can perform.")]
		[SerializeField]
		private int _jumpMaxCount;

		[Tooltip("Initial velocity (instantaneous vertical velocity) when jumping.")]
		[SerializeField]
		private float _jumpImpulse;

		[Tooltip("The maximum time (in seconds) to hold the jump. eg: Variable height jump.")]
		[SerializeField]
		private float _jumpMaxHoldTime;

		[Tooltip("How early before hitting the ground you can trigger a jump (in seconds).")]
		[SerializeField]
		private float _jumpMaxPreGroundedTime;

		[Tooltip("How long after leaving the ground you can trigger a jump (in seconds).")]
		[SerializeField]
		private float _jumpMaxPostGroundedTime;

		[Space(15f)]
		[Tooltip("The maximum flying speed.")]
		[SerializeField]
		private float _maxFlySpeed;

		[Tooltip("Deceleration when flying and not applying acceleration.")]
		[SerializeField]
		private float _brakingDecelerationFlying;

		[Tooltip("Friction to apply to movement when flying.")]
		[SerializeField]
		private float _flyingFriction;

		[Space(15f)]
		[Tooltip("The maximum swimming speed.")]
		[SerializeField]
		private float _maxSwimSpeed;

		[Tooltip("Deceleration when swimming and not applying acceleration.")]
		[SerializeField]
		private float _brakingDecelerationSwimming;

		[Tooltip("Friction to apply to movement when swimming.")]
		[SerializeField]
		private float _swimmingFriction;

		[Tooltip("Water buoyancy ratio. 1 = Neutral Buoyancy, 0 = No Buoyancy.")]
		[SerializeField]
		private float _buoyancy;

		[Tooltip("This Character's gravity.")]
		[Space(15f)]
		[SerializeField]
		private Vector3 _gravity;

		[Tooltip("The degree to which this object is affected by gravity.\nCan be negative allowing to change gravity direction.")]
		[SerializeField]
		private float _gravityScale;

		[Space(15f)]
		[Tooltip("Should animation determines the Character's movement ?")]
		[SerializeField]
		private bool _useRootMotion;

		[Space(15f)]
		[Tooltip("Whether the Character moves with the moving platform it is standing on.")]
		[SerializeField]
		private bool _impartPlatformMovement;

		[Tooltip("Whether the Character receives the changes in rotation of the platform it is standing on.")]
		[SerializeField]
		private bool _impartPlatformRotation;

		[Tooltip("If true, impart the platform's velocity when jumping or falling off it.")]
		[SerializeField]
		private bool _impartPlatformVelocity;

		[Space(15f)]
		[Tooltip("If enabled, the player will interact with dynamic rigidbodies when walking into them.")]
		[SerializeField]
		private bool _enablePhysicsInteraction;

		[Tooltip("Should apply push force to characters when walking into them ?")]
		[SerializeField]
		private bool _applyPushForceToCharacters;

		[Tooltip("Should apply a downward force to rigidbodies we stand on ?")]
		[SerializeField]
		private bool _applyStandingDownwardForce;

		[Space(15f)]
		[Tooltip("This Character's mass (in Kg).Determines how the character interact against other characters or dynamic rigidbodies if enablePhysicsInteraction == true.")]
		[SerializeField]
		private float _mass;

		[Tooltip("Force applied to rigidbodies when walking into them (due to mass and relative velocity) is scaled by this amount.")]
		[SerializeField]
		private float _pushForceScale;

		[Tooltip("Force applied to rigidbodies we stand on (due to mass and gravity) is scaled by this amount.")]
		[SerializeField]
		private float _standingDownwardForceScale;

		[Space(15f)]
		[Tooltip("Reference to the Player's Camera.\nIf assigned, the Character's movement will be relative to this camera, otherwise movement will be relative to world axis.")]
		[SerializeField]
		private Camera _camera;

		protected readonly List<PhysicsVolume> _physicsVolumes = new List<PhysicsVolume>();

		private Coroutine _lateFixedUpdateCoroutine;

		private bool _enableAutoSimulation = true;

		private Transform _transform;

		private CharacterMovement _characterMovement;

		private Animator _animator;

		private RootMotionController _rootMotionController;

		private Transform _cameraTransform;

		private MovementMode _movementMode;

		private int _customMovementMode;

		private bool _useSeparateBrakingFriction;

		private float _brakingFriction;

		private bool _useSeparateBrakingDeceleration;

		private float _brakingDeceleration;

		private Vector3 _movementDirection = Vector3.zero;

		private Vector3 _rotationInput = Vector3.zero;

		private Vector3 _desiredVelocity = Vector3.zero;

		protected bool _isCrouched;

		protected bool _isJumping;

		private float _jumpInputHoldTime;

		private float _jumpForceTimeRemaining;

		private int _jumpCurrentCount;

		protected float _fallingTime;

		public Camera camera
		{
			get
			{
				return _camera;
			}
			set
			{
				_camera = value;
			}
		}

		public Transform cameraTransform
		{
			get
			{
				if (_camera != null)
				{
					_cameraTransform = _camera.transform;
				}
				return _cameraTransform;
			}
		}

		public new Transform transform => _transform;

		public CharacterMovement characterMovement => _characterMovement;

		public Animator animator => _animator;

		public RootMotionController rootMotionController => _rootMotionController;

		public float rotationRate
		{
			get
			{
				return _rotationRate;
			}
			set
			{
				_rotationRate = value;
			}
		}

		public RotationMode rotationMode
		{
			get
			{
				return _rotationMode;
			}
			set
			{
				_rotationMode = value;
			}
		}

		public float maxWalkSpeed
		{
			get
			{
				return _maxWalkSpeed;
			}
			set
			{
				_maxWalkSpeed = Mathf.Max(0f, value);
			}
		}

		public float minAnalogWalkSpeed
		{
			get
			{
				return _minAnalogWalkSpeed;
			}
			set
			{
				_minAnalogWalkSpeed = Mathf.Max(0f, value);
			}
		}

		public float maxAcceleration
		{
			get
			{
				return _maxAcceleration;
			}
			set
			{
				_maxAcceleration = Mathf.Max(0f, value);
			}
		}

		public float brakingDecelerationWalking
		{
			get
			{
				return _brakingDecelerationWalking;
			}
			set
			{
				_brakingDecelerationWalking = Mathf.Max(0f, value);
			}
		}

		public float groundFriction
		{
			get
			{
				return _groundFriction;
			}
			set
			{
				_groundFriction = Mathf.Max(0f, value);
			}
		}

		public bool canEverCrouch
		{
			get
			{
				return _canEverCrouch;
			}
			set
			{
				_canEverCrouch = value;
			}
		}

		public float crouchedHeight
		{
			get
			{
				return _crouchedHeight;
			}
			set
			{
				_crouchedHeight = Mathf.Max(0f, value);
			}
		}

		public float unCrouchedHeight
		{
			get
			{
				return _unCrouchedHeight;
			}
			set
			{
				_unCrouchedHeight = Mathf.Max(0f, value);
			}
		}

		public float maxWalkSpeedCrouched
		{
			get
			{
				return _maxWalkSpeedCrouched;
			}
			set
			{
				_maxWalkSpeedCrouched = Mathf.Max(0f, value);
			}
		}

		public bool crouchInputPressed { get; protected set; }

		public float maxFallSpeed
		{
			get
			{
				return _maxFallSpeed;
			}
			set
			{
				_maxFallSpeed = Mathf.Max(0f, value);
			}
		}

		public float brakingDecelerationFalling
		{
			get
			{
				return _brakingDecelerationFalling;
			}
			set
			{
				_brakingDecelerationFalling = Mathf.Max(0f, value);
			}
		}

		public float fallingLateralFriction
		{
			get
			{
				return _fallingLateralFriction;
			}
			set
			{
				_fallingLateralFriction = Mathf.Max(0f, value);
			}
		}

		public float fallingTime => _fallingTime;

		public float airControl
		{
			get
			{
				return _airControl;
			}
			set
			{
				_airControl = Mathf.Clamp01(value);
			}
		}

		public bool canEverJump
		{
			get
			{
				return _canEverJump;
			}
			set
			{
				_canEverJump = value;
			}
		}

		public bool canJumpWhileCrouching
		{
			get
			{
				return _canJumpWhileCrouching;
			}
			set
			{
				_canJumpWhileCrouching = value;
			}
		}

		public int jumpMaxCount
		{
			get
			{
				return _jumpMaxCount;
			}
			set
			{
				_jumpMaxCount = Mathf.Max(1, value);
			}
		}

		public float jumpImpulse
		{
			get
			{
				return _jumpImpulse;
			}
			set
			{
				_jumpImpulse = Mathf.Max(0f, value);
			}
		}

		public float jumpMaxHoldTime
		{
			get
			{
				return _jumpMaxHoldTime;
			}
			set
			{
				_jumpMaxHoldTime = Mathf.Max(0f, value);
			}
		}

		public float jumpMaxPreGroundedTime
		{
			get
			{
				return _jumpMaxPreGroundedTime;
			}
			set
			{
				_jumpMaxPreGroundedTime = Mathf.Max(0f, value);
			}
		}

		public float jumpMaxPostGroundedTime
		{
			get
			{
				return _jumpMaxPostGroundedTime;
			}
			set
			{
				_jumpMaxPostGroundedTime = Mathf.Max(0f, value);
			}
		}

		public float jumpInputHoldTime
		{
			get
			{
				return _jumpInputHoldTime;
			}
			protected set
			{
				_jumpInputHoldTime = Mathf.Max(0f, value);
			}
		}

		public float jumpForceTimeRemaining
		{
			get
			{
				return _jumpForceTimeRemaining;
			}
			protected set
			{
				_jumpForceTimeRemaining = Mathf.Max(0f, value);
			}
		}

		public int jumpCurrentCount
		{
			get
			{
				return _jumpCurrentCount;
			}
			protected set
			{
				_jumpCurrentCount = Mathf.Max(0, value);
			}
		}

		public bool notifyJumpApex { get; set; }

		public bool jumpInputPressed { get; protected set; }

		public float maxFlySpeed
		{
			get
			{
				return _maxFlySpeed;
			}
			set
			{
				_maxFlySpeed = Mathf.Max(0f, value);
			}
		}

		public float brakingDecelerationFlying
		{
			get
			{
				return _brakingDecelerationFlying;
			}
			set
			{
				_brakingDecelerationFlying = Mathf.Max(0f, value);
			}
		}

		public float flyingFriction
		{
			get
			{
				return _flyingFriction;
			}
			set
			{
				_flyingFriction = Mathf.Max(0f, value);
			}
		}

		public float maxSwimSpeed
		{
			get
			{
				return _maxSwimSpeed;
			}
			set
			{
				_maxSwimSpeed = Mathf.Max(0f, value);
			}
		}

		public float brakingDecelerationSwimming
		{
			get
			{
				return _brakingDecelerationSwimming;
			}
			set
			{
				_brakingDecelerationSwimming = Mathf.Max(0f, value);
			}
		}

		public float swimmingFriction
		{
			get
			{
				return _swimmingFriction;
			}
			set
			{
				_swimmingFriction = Mathf.Max(0f, value);
			}
		}

		public float buoyancy
		{
			get
			{
				return _buoyancy;
			}
			set
			{
				_buoyancy = Mathf.Max(0f, value);
			}
		}

		public bool useSeparateBrakingFriction
		{
			get
			{
				return _useSeparateBrakingFriction;
			}
			set
			{
				_useSeparateBrakingFriction = value;
			}
		}

		public float brakingFriction
		{
			get
			{
				return _brakingFriction;
			}
			set
			{
				_brakingFriction = Mathf.Max(0f, value);
			}
		}

		public bool useSeparateBrakingDeceleration
		{
			get
			{
				return _useSeparateBrakingDeceleration;
			}
			set
			{
				_useSeparateBrakingDeceleration = value;
			}
		}

		public float brakingDeceleration
		{
			get
			{
				return _brakingDeceleration;
			}
			set
			{
				_brakingDeceleration = value;
			}
		}

		public Vector3 gravity
		{
			get
			{
				return _gravity * _gravityScale;
			}
			set
			{
				_gravity = value;
			}
		}

		public float gravityScale
		{
			get
			{
				return _gravityScale;
			}
			set
			{
				_gravityScale = value;
			}
		}

		public bool useRootMotion
		{
			get
			{
				return _useRootMotion;
			}
			set
			{
				_useRootMotion = value;
			}
		}

		public bool enablePhysicsInteraction
		{
			get
			{
				return _enablePhysicsInteraction;
			}
			set
			{
				_enablePhysicsInteraction = value;
				if ((bool)_characterMovement)
				{
					_characterMovement.enablePhysicsInteraction = _enablePhysicsInteraction;
				}
			}
		}

		public bool applyPushForceToCharacters
		{
			get
			{
				return _applyPushForceToCharacters;
			}
			set
			{
				_applyPushForceToCharacters = value;
				if ((bool)_characterMovement)
				{
					_characterMovement.physicsInteractionAffectsCharacters = _applyPushForceToCharacters;
				}
			}
		}

		public bool applyStandingDownwardForce
		{
			get
			{
				return _applyStandingDownwardForce;
			}
			set
			{
				_applyStandingDownwardForce = value;
			}
		}

		public float mass
		{
			get
			{
				return _mass;
			}
			set
			{
				_mass = Mathf.Max(1E-07f, value);
				if ((bool)_characterMovement && (bool)_characterMovement.rigidbody)
				{
					_characterMovement.rigidbody.mass = _mass;
				}
			}
		}

		public float pushForceScale
		{
			get
			{
				return _pushForceScale;
			}
			set
			{
				_pushForceScale = Mathf.Max(0f, value);
				if ((bool)_characterMovement)
				{
					_characterMovement.pushForceScale = _pushForceScale;
				}
			}
		}

		public float standingDownwardForceScale
		{
			get
			{
				return _standingDownwardForceScale;
			}
			set
			{
				_standingDownwardForceScale = Mathf.Max(0f, value);
			}
		}

		public bool impartPlatformVelocity
		{
			get
			{
				return _impartPlatformVelocity;
			}
			set
			{
				_impartPlatformVelocity = value;
				if ((bool)_characterMovement)
				{
					_characterMovement.impartPlatformVelocity = _impartPlatformVelocity;
				}
			}
		}

		public bool impartPlatformMovement
		{
			get
			{
				return _impartPlatformMovement;
			}
			set
			{
				_impartPlatformMovement = value;
				if ((bool)_characterMovement)
				{
					_characterMovement.impartPlatformMovement = _impartPlatformMovement;
				}
			}
		}

		public bool impartPlatformRotation
		{
			get
			{
				return _impartPlatformRotation;
			}
			set
			{
				_impartPlatformRotation = value;
				if ((bool)_characterMovement)
				{
					_characterMovement.impartPlatformRotation = _impartPlatformRotation;
				}
			}
		}

		public Vector3 position => characterMovement.position;

		public Quaternion rotation => characterMovement.rotation;

		public Vector3 velocity => characterMovement.velocity;

		public float speed => characterMovement.velocity.magnitude;

		public float radius => characterMovement.radius;

		public float height => characterMovement.height;

		public MovementMode movementMode => _movementMode;

		public int customMovementMode => _customMovementMode;

		public PhysicsVolume physicsVolume { get; protected set; }

		public bool enableAutoSimulation
		{
			get
			{
				return _enableAutoSimulation;
			}
			set
			{
				_enableAutoSimulation = value;
				EnableAutoSimulationCoroutine(_enableAutoSimulation);
			}
		}

		public bool isPaused { get; private set; }

		public event PhysicsVolumeChangedEventHandler PhysicsVolumeChanged;

		public event MovementModeChangedEventHandler MovementModeChanged;

		public event CustomMovementModeUpdateEventHandler CustomMovementModeUpdated;

		public event CustomRotationModeUpdateEventHandler CustomRotationModeUpdated;

		public event BeforeSimulationUpdateEventHandler BeforeSimulationUpdated;

		public event AfterSimulationUpdateEventHandler AfterSimulationUpdated;

		public event CharacterMovementUpdateEventHandler CharacterMovementUpdated;

		public event CollidedEventHandler Collided;

		public event FoundGroundEventHandler FoundGround;

		public event LandedEventHandled Landed;

		public event CrouchedEventHandler Crouched;

		public event UnCrouchedEventHandler UnCrouched;

		public event JumpedEventHandler Jumped;

		public event ReachedJumpApexEventHandler ReachedJumpApex;

		protected virtual void OnCustomMovementMode(float deltaTime)
		{
			this.CustomMovementModeUpdated?.Invoke(deltaTime);
		}

		protected virtual void OnCustomRotationMode(float deltaTime)
		{
			this.CustomRotationModeUpdated?.Invoke(deltaTime);
		}

		protected virtual void OnBeforeSimulationUpdate(float deltaTime)
		{
			this.BeforeSimulationUpdated?.Invoke(deltaTime);
		}

		protected virtual void OnAfterSimulationUpdate(float deltaTime)
		{
			this.AfterSimulationUpdated?.Invoke(deltaTime);
		}

		protected virtual void OnCharacterMovementUpdated(float deltaTime)
		{
			this.CharacterMovementUpdated?.Invoke(deltaTime);
		}

		protected virtual void OnCollided(ref CollisionResult collisionResult)
		{
			this.Collided?.Invoke(ref collisionResult);
		}

		protected virtual void OnFoundGround(ref FindGroundResult foundGround)
		{
			this.FoundGround?.Invoke(ref foundGround);
		}

		protected virtual void OnLanded(Vector3 landingVelocity)
		{
			this.Landed?.Invoke(landingVelocity);
		}

		protected virtual void OnCrouched()
		{
			this.Crouched?.Invoke();
		}

		protected virtual void OnUnCrouched()
		{
			this.UnCrouched?.Invoke();
		}

		protected virtual void OnJumped()
		{
			this.Jumped?.Invoke();
		}

		protected virtual void OnReachedJumpApex()
		{
			this.ReachedJumpApex?.Invoke();
		}

		public Vector3 GetGravityVector()
		{
			return gravity;
		}

		public Vector3 GetGravityDirection()
		{
			return gravity.normalized;
		}

		public float GetGravityMagnitude()
		{
			return gravity.magnitude;
		}

		public void SetGravityVector(Vector3 newGravityVector)
		{
			_gravity = newGravityVector;
		}

		private void EnableAutoSimulationCoroutine(bool enable)
		{
			if (enable)
			{
				if (_lateFixedUpdateCoroutine != null)
				{
					StopCoroutine(_lateFixedUpdateCoroutine);
				}
				_lateFixedUpdateCoroutine = StartCoroutine(LateFixedUpdate());
			}
			else if (_lateFixedUpdateCoroutine != null)
			{
				StopCoroutine(_lateFixedUpdateCoroutine);
			}
		}

		protected virtual void CacheComponents()
		{
			_transform = GetComponent<Transform>();
			_characterMovement = GetComponent<CharacterMovement>();
			_animator = GetComponentInChildren<Animator>();
			_rootMotionController = GetComponentInChildren<RootMotionController>();
			characterMovement.impartPlatformMovement = _impartPlatformMovement;
			characterMovement.impartPlatformRotation = _impartPlatformRotation;
			characterMovement.impartPlatformVelocity = _impartPlatformVelocity;
			characterMovement.enablePhysicsInteraction = _enablePhysicsInteraction;
			characterMovement.physicsInteractionAffectsCharacters = _applyPushForceToCharacters;
			characterMovement.pushForceScale = _pushForceScale;
			mass = _mass;
		}

		protected virtual void SetPhysicsVolume(PhysicsVolume newPhysicsVolume)
		{
			if (!(newPhysicsVolume == physicsVolume))
			{
				OnPhysicsVolumeChanged(newPhysicsVolume);
				physicsVolume = newPhysicsVolume;
			}
		}

		protected virtual void OnPhysicsVolumeChanged(PhysicsVolume newPhysicsVolume)
		{
			if ((bool)newPhysicsVolume && newPhysicsVolume.waterVolume)
			{
				SetMovementMode(MovementMode.Swimming);
			}
			else if (IsInWaterPhysicsVolume() && newPhysicsVolume == null && IsSwimming())
			{
				SetMovementMode(MovementMode.Falling);
			}
			this.PhysicsVolumeChanged?.Invoke(newPhysicsVolume);
		}

		protected virtual void UpdatePhysicsVolume(PhysicsVolume newPhysicsVolume)
		{
			Vector3 worldCenter = characterMovement.worldCenter;
			if ((bool)newPhysicsVolume && newPhysicsVolume.boxCollider.ClosestPoint(worldCenter) == worldCenter)
			{
				SetPhysicsVolume(newPhysicsVolume);
			}
			else
			{
				SetPhysicsVolume(null);
			}
		}

		protected virtual void AddPhysicsVolume(Collider other)
		{
			if (other.TryGetComponent<PhysicsVolume>(out var component) && !_physicsVolumes.Contains(component))
			{
				_physicsVolumes.Insert(0, component);
			}
		}

		protected virtual void RemovePhysicsVolume(Collider other)
		{
			if (other.TryGetComponent<PhysicsVolume>(out var component) && _physicsVolumes.Contains(component))
			{
				_physicsVolumes.Remove(component);
			}
		}

		protected virtual void UpdatePhysicsVolumes()
		{
			PhysicsVolume newPhysicsVolume = null;
			int num = -2147483648;
			int i = 0;
			for (int count = _physicsVolumes.Count; i < count; i++)
			{
				PhysicsVolume physicsVolume = _physicsVolumes[i];
				if (physicsVolume.priority > num)
				{
					num = physicsVolume.priority;
					newPhysicsVolume = physicsVolume;
				}
			}
			UpdatePhysicsVolume(newPhysicsVolume);
		}

		public virtual bool IsInWaterPhysicsVolume()
		{
			if ((bool)physicsVolume)
			{
				return physicsVolume.waterVolume;
			}
			return false;
		}

		public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
		{
			characterMovement.AddForce(force, forceMode);
		}

		public void AddExplosionForce(float forceMagnitude, Vector3 origin, float explosionRadius, ForceMode forceMode = ForceMode.Force)
		{
			characterMovement.AddExplosionForce(forceMagnitude, origin, explosionRadius, forceMode);
		}

		public void LaunchCharacter(Vector3 launchVelocity, bool overrideVerticalVelocity = false, bool overrideLateralVelocity = false)
		{
			characterMovement.LaunchCharacter(launchVelocity, overrideVerticalVelocity, overrideLateralVelocity);
		}

		public void DetectCollisions(bool detectCollisions)
		{
			characterMovement.detectCollisions = detectCollisions;
		}

		public void IgnoreCollision(Collider otherCollider, bool ignore = true)
		{
			characterMovement.IgnoreCollision(otherCollider, ignore);
		}

		public void IgnoreCollision(Rigidbody otherRigidbody, bool ignore = true)
		{
			characterMovement.IgnoreCollision(otherRigidbody, ignore);
		}

		public void CapsuleIgnoreCollision(Collider otherCollider, bool ignore = true)
		{
			characterMovement.CapsuleIgnoreCollision(otherCollider, ignore);
		}

		public void PauseGroundConstraint(float seconds = 0.1f)
		{
			characterMovement.PauseGroundConstraint(seconds);
		}

		public void EnableGroundConstraint(bool enable)
		{
			characterMovement.constrainToGround = enable;
		}

		public bool WasOnGround()
		{
			return characterMovement.wasOnGround;
		}

		public bool IsOnGround()
		{
			return characterMovement.isOnGround;
		}

		public bool WasOnWalkableGround()
		{
			return characterMovement.wasOnWalkableGround;
		}

		public bool IsOnWalkableGround()
		{
			return characterMovement.isOnWalkableGround;
		}

		public bool WasGrounded()
		{
			return characterMovement.wasGrounded;
		}

		public bool IsGrounded()
		{
			return characterMovement.isGrounded;
		}

		public CharacterMovement GetCharacterMovement()
		{
			return characterMovement;
		}

		public Animator GetAnimator()
		{
			return animator;
		}

		public RootMotionController GetRootMotionController()
		{
			return rootMotionController;
		}

		public PhysicsVolume GetPhysicsVolume()
		{
			return physicsVolume;
		}

		public Vector3 GetPosition()
		{
			return characterMovement.position;
		}

		public void SetPosition(Vector3 position, bool updateGround = false)
		{
			characterMovement.SetPosition(position, updateGround);
		}

		public void TeleportPosition(Vector3 newPosition, bool interpolating = true, bool updateGround = false)
		{
			if (interpolating)
			{
				characterMovement.interpolation = RigidbodyInterpolation.None;
			}
			characterMovement.SetPosition(newPosition, updateGround);
			if (interpolating)
			{
				characterMovement.interpolation = RigidbodyInterpolation.Interpolate;
			}
		}

		public Quaternion GetRotation()
		{
			return characterMovement.rotation;
		}

		public void SetRotation(Quaternion newRotation)
		{
			characterMovement.rotation = newRotation;
		}

		public void TeleportRotation(Quaternion newRotation, bool interpolating = true)
		{
			if (interpolating)
			{
				characterMovement.interpolation = RigidbodyInterpolation.None;
			}
			characterMovement.SetRotation(newRotation);
			if (interpolating)
			{
				characterMovement.interpolation = RigidbodyInterpolation.Interpolate;
			}
		}

		public virtual Vector3 GetUpVector()
		{
			return transform.up;
		}

		public virtual Vector3 GetRightVector()
		{
			return transform.right;
		}

		public virtual Vector3 GetForwardVector()
		{
			return transform.forward;
		}

		public virtual void RotateTowards(Vector3 worldDirection, float deltaTime, bool updateYawOnly = true)
		{
			Vector3 upVector = GetUpVector();
			if (updateYawOnly)
			{
				worldDirection = Vector3.ProjectOnPlane(worldDirection, upVector);
			}
			if (!(worldDirection == Vector3.zero))
			{
				Quaternion to = Quaternion.LookRotation(worldDirection, upVector);
				characterMovement.rotation = Quaternion.RotateTowards(rotation, to, rotationRate * deltaTime);
			}
		}

		protected virtual void RotateWithRootMotion()
		{
			if (useRootMotion && (bool)rootMotionController)
			{
				characterMovement.rotation = rootMotionController.ConsumeRootMotionRotation() * characterMovement.rotation;
			}
		}

		public Vector3 GetVelocity()
		{
			return characterMovement.velocity;
		}

		public void SetVelocity(Vector3 newVelocity)
		{
			characterMovement.velocity = newVelocity;
		}

		public float GetSpeed()
		{
			return characterMovement.velocity.magnitude;
		}

		public float GetRadius()
		{
			return characterMovement.radius;
		}

		public float GetHeight()
		{
			return characterMovement.height;
		}

		public Vector3 GetMovementDirection()
		{
			return _movementDirection;
		}

		public void SetMovementDirection(Vector3 movementDirection)
		{
			_movementDirection = movementDirection;
		}

		public virtual void SetYaw(float value)
		{
			characterMovement.rotation = Quaternion.Euler(0f, value, 0f);
		}

		public virtual void AddYawInput(float value)
		{
			_rotationInput.y += value;
		}

		public virtual void AddPitchInput(float value)
		{
			_rotationInput.x += value;
		}

		public virtual void AddRollInput(float value)
		{
			_rotationInput.z += value;
		}

		protected virtual void ConsumeRotationInput()
		{
			if (_rotationInput != Vector3.zero)
			{
				characterMovement.rotation *= Quaternion.Euler(_rotationInput);
				_rotationInput = Vector3.zero;
			}
		}

		public MovementMode GetMovementMode()
		{
			return _movementMode;
		}

		public int GetCustomMovementMode()
		{
			return _customMovementMode;
		}

		public void SetMovementMode(MovementMode newMovementMode, int newCustomMode = 0)
		{
			if (newMovementMode != _movementMode || (newMovementMode == MovementMode.Custom && newCustomMode != _customMovementMode))
			{
				MovementMode prevMovementMode = _movementMode;
				int prevCustomMode = _customMovementMode;
				_movementMode = newMovementMode;
				_customMovementMode = newCustomMode;
				OnMovementModeChanged(prevMovementMode, prevCustomMode);
			}
		}

		protected virtual void OnMovementModeChanged(MovementMode prevMovementMode, int prevCustomMode)
		{
			switch (movementMode)
			{
			case MovementMode.None:
				characterMovement.velocity = Vector3.zero;
				characterMovement.ClearAccumulatedForces();
				break;
			case MovementMode.Walking:
				ResetJumpState();
				if (prevMovementMode == MovementMode.Flying || prevMovementMode == MovementMode.Swimming)
				{
					characterMovement.constrainToGround = true;
				}
				OnLanded(characterMovement.landedVelocity);
				break;
			case MovementMode.Falling:
				if (prevMovementMode == MovementMode.Flying || prevMovementMode == MovementMode.Swimming)
				{
					characterMovement.constrainToGround = true;
				}
				break;
			case MovementMode.Flying:
			case MovementMode.Swimming:
				ResetJumpState();
				characterMovement.constrainToGround = false;
				break;
			}
			if (!IsFalling())
			{
				_fallingTime = 0f;
			}
			this.MovementModeChanged?.Invoke(prevMovementMode, prevCustomMode);
		}

		public virtual bool IsWalking()
		{
			return _movementMode == MovementMode.Walking;
		}

		public virtual bool IsFalling()
		{
			return _movementMode == MovementMode.Falling;
		}

		public virtual bool IsFlying()
		{
			return _movementMode == MovementMode.Flying;
		}

		public virtual bool IsSwimming()
		{
			return _movementMode == MovementMode.Swimming;
		}

		public virtual float GetMaxSpeed()
		{
			switch (_movementMode)
			{
			case MovementMode.Walking:
				if (!IsCrouched())
				{
					return maxWalkSpeed;
				}
				return maxWalkSpeedCrouched;
			case MovementMode.Falling:
				return maxWalkSpeed;
			case MovementMode.Swimming:
				return maxSwimSpeed;
			case MovementMode.Flying:
				return maxFlySpeed;
			default:
				return 0f;
			}
		}

		public virtual float GetMinAnalogSpeed()
		{
			MovementMode movementMode = _movementMode;
			if ((uint)(movementMode - 1) <= 1u)
			{
				return minAnalogWalkSpeed;
			}
			return 0f;
		}

		public virtual float GetMaxAcceleration()
		{
			if (IsFalling())
			{
				return maxAcceleration * airControl;
			}
			return maxAcceleration;
		}

		public virtual float GetMaxBrakingDeceleration()
		{
			switch (_movementMode)
			{
			case MovementMode.Walking:
				return brakingDecelerationWalking;
			case MovementMode.Falling:
				if (!characterMovement.isOnGround)
				{
					return brakingDecelerationFalling;
				}
				return 0f;
			case MovementMode.Swimming:
				return brakingDecelerationSwimming;
			case MovementMode.Flying:
				return brakingDecelerationFlying;
			default:
				return 0f;
			}
		}

		protected virtual float ComputeAnalogInputModifier(Vector3 desiredVelocity)
		{
			float maxSpeed = GetMaxSpeed();
			if (desiredVelocity.sqrMagnitude > 0f && maxSpeed > 1E-08f)
			{
				return Mathf.Clamp01(desiredVelocity.magnitude / maxSpeed);
			}
			return 0f;
		}

		public virtual Vector3 ApplyVelocityBraking(Vector3 velocity, float friction, float maxBrakingDeceleration, float deltaTime)
		{
			if (velocity.isZero() || deltaTime < 1E-06f)
			{
				return velocity;
			}
			bool flag = friction == 0f;
			bool flag2 = maxBrakingDeceleration == 0f;
			if (flag && flag2)
			{
				return velocity;
			}
			Vector3 rhs = velocity;
			Vector3 vector = (flag2 ? Vector3.zero : ((0f - maxBrakingDeceleration) * velocity.normalized));
			float num = deltaTime;
			while (num >= 1E-06f)
			{
				float num2 = ((num > 1f / 33f && !flag) ? Mathf.Min(1f / 33f, num * 0.5f) : num);
				num -= num2;
				velocity += ((0f - friction) * velocity + vector) * num2;
				if (Vector3.Dot(velocity, rhs) <= 0f)
				{
					return Vector3.zero;
				}
			}
			float sqrMagnitude = velocity.sqrMagnitude;
			if (sqrMagnitude <= 1E-05f || (!flag2 && sqrMagnitude <= 0.1f))
			{
				return Vector3.zero;
			}
			return velocity;
		}

		public virtual Vector3 CalcVelocity(Vector3 velocity, Vector3 desiredVelocity, float friction, bool isFluid, float deltaTime)
		{
			if (deltaTime < 1E-06f)
			{
				return velocity;
			}
			float magnitude = desiredVelocity.magnitude;
			Vector3 vector = ((magnitude > 0f) ? (desiredVelocity / magnitude) : Vector3.zero);
			float num = ComputeAnalogInputModifier(desiredVelocity);
			Vector3 vector2 = GetMaxAcceleration() * num * vector;
			float num2 = Mathf.Max(GetMinAnalogSpeed(), GetMaxSpeed() * num);
			bool num3 = vector2.isZero();
			bool flag = velocity.isExceeding(num2);
			if (num3 || flag)
			{
				Vector3 rhs = velocity;
				velocity = ApplyVelocityBraking(friction: useSeparateBrakingFriction ? brakingFriction : friction, maxBrakingDeceleration: useSeparateBrakingDeceleration ? brakingDeceleration : GetMaxBrakingDeceleration(), velocity: velocity, deltaTime: deltaTime);
				if (flag && velocity.sqrMagnitude < num2.square() && Vector3.Dot(vector2, rhs) > 0f)
				{
					velocity = rhs.normalized * num2;
				}
			}
			else
			{
				Vector3 normalized = vector2.normalized;
				float magnitude2 = velocity.magnitude;
				velocity -= (velocity - normalized * magnitude2) * Mathf.Min(friction * deltaTime, 1f);
			}
			if (isFluid)
			{
				velocity *= 1f - Mathf.Min(friction * deltaTime, 1f);
			}
			if (!num3)
			{
				float maxLength = (velocity.isExceeding(num2) ? velocity.magnitude : num2);
				velocity += vector2 * deltaTime;
				velocity = velocity.clampedTo(maxLength);
			}
			return velocity;
		}

		public virtual Vector3 ConstrainInputVector(Vector3 inputVector)
		{
			Vector3 vector = -GetGravityDirection();
			if (!Mathf.Approximately(Vector3.Dot(inputVector, vector), 0f) && (IsWalking() || IsFalling()))
			{
				inputVector = Vector3.ProjectOnPlane(inputVector, vector);
			}
			return characterMovement.ConstrainVectorToPlane(inputVector);
		}

		protected virtual void CalcDesiredVelocity(float deltaTime)
		{
			Vector3 vector = Vector3.ClampMagnitude(GetMovementDirection(), 1f);
			Vector3 inputVector = ((useRootMotion && (bool)rootMotionController) ? rootMotionController.ConsumeRootMotionVelocity(deltaTime) : (vector * GetMaxSpeed()));
			_desiredVelocity = ConstrainInputVector(inputVector);
		}

		public virtual Vector3 GetDesiredVelocity()
		{
			return _desiredVelocity;
		}

		public float GetSignedSlopeAngle()
		{
			Vector3 movementDirection = GetMovementDirection();
			if (movementDirection.isZero() || !IsOnGround())
			{
				return 0f;
			}
			return Mathf.Asin(Vector3.Dot(Vector3.ProjectOnPlane(movementDirection, characterMovement.groundNormal).normalized, -GetGravityDirection())) * 57.29578f;
		}

		protected virtual void ApplyDownwardsForce()
		{
			Rigidbody groundRigidbody = characterMovement.groundRigidbody;
			if ((bool)groundRigidbody && !groundRigidbody.isKinematic)
			{
				Vector3 vector = mass * GetGravityVector();
				groundRigidbody.AddForceAtPosition(vector * standingDownwardForceScale, GetPosition());
			}
		}

		protected virtual void WalkingMovementMode(float deltaTime)
		{
			if (useRootMotion && (bool)rootMotionController)
			{
				characterMovement.velocity = GetDesiredVelocity();
			}
			else
			{
				characterMovement.velocity = CalcVelocity(characterMovement.velocity, GetDesiredVelocity(), groundFriction, isFluid: false, deltaTime);
			}
			if (applyStandingDownwardForce)
			{
				ApplyDownwardsForce();
			}
		}

		public virtual bool IsCrouched()
		{
			return _isCrouched;
		}

		public virtual void Crouch()
		{
			crouchInputPressed = true;
		}

		public virtual void UnCrouch()
		{
			crouchInputPressed = false;
		}

		protected virtual bool IsCrouchAllowed()
		{
			if (canEverCrouch)
			{
				return IsWalking();
			}
			return false;
		}

		protected virtual bool CanUnCrouch()
		{
			return !characterMovement.CheckHeight(_unCrouchedHeight);
		}

		protected virtual void CheckCrouchInput()
		{
			if (!_isCrouched && crouchInputPressed && IsCrouchAllowed())
			{
				_isCrouched = true;
				characterMovement.SetHeight(_crouchedHeight);
				OnCrouched();
			}
			else if (_isCrouched && (!crouchInputPressed || !IsCrouchAllowed()) && CanUnCrouch())
			{
				_isCrouched = false;
				characterMovement.SetHeight(_unCrouchedHeight);
				OnUnCrouched();
			}
		}

		protected virtual void FallingMovementMode(float deltaTime)
		{
			Vector3 vector = GetDesiredVelocity();
			Vector3 vector2 = -GetGravityDirection();
			if (IsOnGround() && !IsOnWalkableGround())
			{
				Vector3 groundNormal = characterMovement.groundNormal;
				if (Vector3.Dot(vector, groundNormal) < 0f)
				{
					Vector3 normalized = Vector3.ProjectOnPlane(groundNormal, vector2).normalized;
					vector = Vector3.ProjectOnPlane(vector, normalized);
				}
				vector2 = Vector3.ProjectOnPlane(vector2, groundNormal).normalized;
			}
			Vector3 vector3 = Vector3.Project(characterMovement.velocity, vector2);
			Vector3 vector4 = characterMovement.velocity - vector3;
			vector4 = CalcVelocity(vector4, vector, fallingLateralFriction, isFluid: false, deltaTime);
			vector3 += gravity * deltaTime;
			float num = maxFallSpeed;
			if ((bool)physicsVolume)
			{
				num = physicsVolume.maxFallSpeed;
			}
			if (Vector3.Dot(vector3, vector2) < 0f - num)
			{
				vector3 = Vector3.ClampMagnitude(vector3, num);
			}
			characterMovement.velocity = vector4 + vector3;
			_fallingTime += deltaTime;
		}

		public virtual bool IsJumping()
		{
			return _isJumping;
		}

		public virtual void Jump()
		{
			jumpInputPressed = true;
		}

		public virtual void StopJumping()
		{
			jumpInputPressed = false;
			jumpInputHoldTime = 0f;
			ResetJumpState();
		}

		protected virtual void ResetJumpState()
		{
			if (!IsFalling())
			{
				jumpCurrentCount = 0;
			}
			jumpForceTimeRemaining = 0f;
			_isJumping = false;
		}

		public virtual bool IsJumpProvidingForce()
		{
			return jumpForceTimeRemaining > 0f;
		}

		public virtual float GetMaxJumpHeight()
		{
			float gravityMagnitude = GetGravityMagnitude();
			if (gravityMagnitude > 0.0001f)
			{
				return jumpImpulse * jumpImpulse / (2f * gravityMagnitude);
			}
			return 0f;
		}

		public virtual float GetMaxJumpHeightWithJumpTime()
		{
			return GetMaxJumpHeight() + jumpImpulse * jumpMaxHoldTime;
		}

		protected virtual bool IsJumpAllowed()
		{
			if (!canJumpWhileCrouching && IsCrouched())
			{
				return false;
			}
			if (canEverJump)
			{
				if (!IsWalking())
				{
					return IsFalling();
				}
				return true;
			}
			return false;
		}

		protected virtual bool CanJump()
		{
			bool flag = IsJumpAllowed();
			if (flag)
			{
				if (!_isJumping || jumpMaxHoldTime <= 0f)
				{
					if (jumpCurrentCount == 0)
					{
						flag = jumpInputHoldTime <= jumpMaxPreGroundedTime;
						if (flag)
						{
							jumpInputHoldTime = 0f;
						}
					}
					else
					{
						flag = jumpCurrentCount < jumpMaxCount && jumpInputHoldTime == 0f;
					}
				}
				else
				{
					flag = jumpInputPressed && jumpInputHoldTime < jumpMaxHoldTime && (jumpCurrentCount < jumpMaxCount || (_isJumping && jumpCurrentCount == jumpMaxCount));
				}
			}
			return flag;
		}

		protected virtual bool DoJump()
		{
			Vector3 vector = -GetGravityDirection();
			if (characterMovement.isConstrainedToPlane && Mathf.Approximately(Vector3.Dot(characterMovement.GetPlaneConstraintNormal(), vector), 1f))
			{
				return false;
			}
			float num = Mathf.Max(Vector3.Dot(characterMovement.velocity, vector), jumpImpulse);
			characterMovement.velocity = Vector3.ProjectOnPlane(characterMovement.velocity, vector) + vector * num;
			return true;
		}

		protected virtual void CheckJumpInput()
		{
			if (jumpInputPressed)
			{
				if (jumpCurrentCount == 0 && IsFalling() && fallingTime > jumpMaxPostGroundedTime)
				{
					jumpCurrentCount++;
				}
				bool flag = CanJump() && DoJump();
				if (flag && !_isJumping)
				{
					jumpCurrentCount++;
					jumpForceTimeRemaining = jumpMaxHoldTime;
					characterMovement.PauseGroundConstraint();
					SetMovementMode(MovementMode.Falling);
					OnJumped();
				}
				_isJumping = flag;
			}
		}

		protected virtual void UpdateJumpTimers(float deltaTime)
		{
			if (jumpInputPressed)
			{
				jumpInputHoldTime += deltaTime;
			}
			if (jumpForceTimeRemaining > 0f)
			{
				jumpForceTimeRemaining -= deltaTime;
				if (jumpForceTimeRemaining <= 0f)
				{
					ResetJumpState();
				}
			}
		}

		protected virtual void NotifyJumpApex()
		{
			if (notifyJumpApex && !(Vector3.Dot(GetVelocity(), -GetGravityDirection()) >= 0f))
			{
				notifyJumpApex = false;
				OnReachedJumpApex();
			}
		}

		protected virtual void FlyingMovementMode(float deltaTime)
		{
			if (useRootMotion && (bool)rootMotionController)
			{
				characterMovement.velocity = GetDesiredVelocity();
				return;
			}
			float friction = (IsInWaterPhysicsVolume() ? physicsVolume.friction : flyingFriction);
			characterMovement.velocity = CalcVelocity(characterMovement.velocity, GetDesiredVelocity(), friction, isFluid: true, deltaTime);
		}

		public virtual float CalcImmersionDepth()
		{
			float result = 0f;
			if (IsInWaterPhysicsVolume())
			{
				float num = characterMovement.height;
				if (num == 0f || buoyancy == 0f)
				{
					result = 1f;
				}
				else
				{
					Vector3 vector = -GetGravityDirection();
					Vector3 origin = GetPosition() + vector * num;
					Vector3 direction = -vector;
					result = ((!physicsVolume.boxCollider.Raycast(new Ray(origin, direction), out var hitInfo, num)) ? 1f : (1f - Mathf.InverseLerp(0f, num, hitInfo.distance)));
				}
			}
			return result;
		}

		protected virtual void SwimmingMovementMode(float deltaTime)
		{
			float num = CalcImmersionDepth();
			float num2 = buoyancy * num;
			Vector3 vector = GetDesiredVelocity();
			Vector3 vector2 = characterMovement.velocity;
			Vector3 vector3 = -GetGravityDirection();
			float num3 = Vector3.Dot(vector2, vector3);
			if (num3 > maxSwimSpeed * 0.33f && num2 > 0f)
			{
				num3 = Mathf.Max(maxSwimSpeed * 0.33f, num3 * num * num);
				vector2 = Vector3.ProjectOnPlane(vector2, vector3) + vector3 * num3;
			}
			else if (num < 0.65f)
			{
				float b = Vector3.Dot(vector, vector3);
				vector = Vector3.ProjectOnPlane(vector, vector3) + vector3 * Mathf.Min(0.1f, b);
			}
			if (useRootMotion && (bool)rootMotionController)
			{
				Vector3 vector4 = Vector3.Project(vector2, vector3);
				vector2 = Vector3.ProjectOnPlane(vector, vector3) + vector4;
			}
			else
			{
				float friction = (IsInWaterPhysicsVolume() ? (physicsVolume.friction * num) : (swimmingFriction * num));
				vector2 = CalcVelocity(vector2, vector, friction, isFluid: true, deltaTime);
			}
			vector2 += gravity * ((1f - num2) * deltaTime);
			characterMovement.velocity = vector2;
		}

		protected virtual void CustomMovementMode(float deltaTime)
		{
			OnCustomMovementMode(deltaTime);
		}

		public RotationMode GetRotationMode()
		{
			return _rotationMode;
		}

		public void SetRotationMode(RotationMode rotationMode)
		{
			_rotationMode = rotationMode;
		}

		protected virtual void UpdateRotation(float deltaTime)
		{
			if (_rotationMode != RotationMode.None)
			{
				if (_rotationMode == RotationMode.OrientRotationToMovement)
				{
					bool updateYawOnly = IsWalking() || IsFalling();
					RotateTowards(_movementDirection, deltaTime, updateYawOnly);
				}
				else if (_rotationMode == RotationMode.OrientRotationToViewDirection && camera != null)
				{
					bool updateYawOnly2 = IsWalking() || IsFalling();
					RotateTowards(cameraTransform.forward, deltaTime, updateYawOnly2);
				}
				else if (_rotationMode == RotationMode.OrientWithRootMotion)
				{
					RotateWithRootMotion();
				}
				else if (_rotationMode == RotationMode.Custom)
				{
					CustomRotationMode(deltaTime);
				}
			}
		}

		protected virtual void CustomRotationMode(float deltaTime)
		{
			OnCustomRotationMode(deltaTime);
		}

		private void BeforeSimulationUpdate(float deltaTime)
		{
			if (IsWalking() && !IsGrounded())
			{
				SetMovementMode(MovementMode.Falling);
			}
			if (IsFalling() && IsGrounded())
			{
				SetMovementMode(MovementMode.Walking);
			}
			UpdatePhysicsVolumes();
			CheckCrouchInput();
			CheckJumpInput();
			UpdateJumpTimers(deltaTime);
			OnBeforeSimulationUpdate(deltaTime);
		}

		private void SimulationUpdate(float deltaTime)
		{
			CalcDesiredVelocity(deltaTime);
			switch (_movementMode)
			{
			case MovementMode.Walking:
				WalkingMovementMode(deltaTime);
				break;
			case MovementMode.Falling:
				FallingMovementMode(deltaTime);
				break;
			case MovementMode.Flying:
				FlyingMovementMode(deltaTime);
				break;
			case MovementMode.Swimming:
				SwimmingMovementMode(deltaTime);
				break;
			case MovementMode.Custom:
				CustomMovementMode(deltaTime);
				break;
			}
			UpdateRotation(deltaTime);
			ConsumeRotationInput();
		}

		private void AfterSimulationUpdate(float deltaTime)
		{
			NotifyJumpApex();
			OnAfterSimulationUpdate(deltaTime);
		}

		private void CharacterMovementUpdate(float deltaTime)
		{
			characterMovement.Move(deltaTime);
			OnCharacterMovementUpdated(deltaTime);
			if (!useRootMotion && (bool)rootMotionController)
			{
				rootMotionController.FlushAccumulatedDeltas();
			}
		}

		public void Simulate(float deltaTime)
		{
			if (!isPaused)
			{
				BeforeSimulationUpdate(deltaTime);
				SimulationUpdate(deltaTime);
				AfterSimulationUpdate(deltaTime);
				CharacterMovementUpdate(deltaTime);
			}
		}

		private void OnLateFixedUpdate()
		{
			Simulate(Time.deltaTime);
		}

		public bool IsPaused()
		{
			return isPaused;
		}

		public void Pause(bool pause, bool clearState = true)
		{
			isPaused = pause;
			characterMovement.collider.enabled = !isPaused;
			if (clearState)
			{
				_movementDirection = Vector3.zero;
				_rotationInput = Vector3.zero;
				characterMovement.velocity = Vector3.zero;
				characterMovement.ClearAccumulatedForces();
			}
		}

		protected virtual void Reset()
		{
			_rotationMode = RotationMode.OrientRotationToMovement;
			_rotationRate = 540f;
			_startingMovementMode = MovementMode.Walking;
			_maxWalkSpeed = 5f;
			_minAnalogWalkSpeed = 0f;
			_maxAcceleration = 20f;
			_brakingDecelerationWalking = 20f;
			_groundFriction = 8f;
			_canEverCrouch = true;
			_crouchedHeight = 1.25f;
			_unCrouchedHeight = 2f;
			_maxWalkSpeedCrouched = 3f;
			_maxFallSpeed = 40f;
			_brakingDecelerationFalling = 0f;
			_fallingLateralFriction = 0.3f;
			_airControl = 0.3f;
			_canEverJump = true;
			_canJumpWhileCrouching = true;
			_jumpMaxCount = 1;
			_jumpImpulse = 5f;
			_jumpMaxHoldTime = 0f;
			_jumpMaxPreGroundedTime = 0f;
			_jumpMaxPostGroundedTime = 0f;
			_maxFlySpeed = 10f;
			_brakingDecelerationFlying = 0f;
			_flyingFriction = 1f;
			_maxSwimSpeed = 3f;
			_brakingDecelerationSwimming = 0f;
			_swimmingFriction = 0f;
			_buoyancy = 1f;
			_gravity = new Vector3(0f, -9.81f, 0f);
			_gravityScale = 1f;
			_useRootMotion = false;
			_impartPlatformVelocity = false;
			_impartPlatformMovement = false;
			_impartPlatformRotation = false;
			_enablePhysicsInteraction = false;
			_applyPushForceToCharacters = false;
			_applyStandingDownwardForce = false;
			_mass = 1f;
			_pushForceScale = 1f;
			_standingDownwardForceScale = 1f;
		}

		protected virtual void OnValidate()
		{
			rotationRate = _rotationRate;
			maxWalkSpeed = _maxWalkSpeed;
			minAnalogWalkSpeed = _minAnalogWalkSpeed;
			maxAcceleration = _maxAcceleration;
			brakingDecelerationWalking = _brakingDecelerationWalking;
			groundFriction = _groundFriction;
			crouchedHeight = _crouchedHeight;
			unCrouchedHeight = _unCrouchedHeight;
			maxWalkSpeedCrouched = _maxWalkSpeedCrouched;
			maxFallSpeed = _maxFallSpeed;
			brakingDecelerationFalling = _brakingDecelerationFalling;
			fallingLateralFriction = _fallingLateralFriction;
			airControl = _airControl;
			jumpMaxCount = _jumpMaxCount;
			jumpImpulse = _jumpImpulse;
			jumpMaxHoldTime = _jumpMaxHoldTime;
			jumpMaxPreGroundedTime = _jumpMaxPreGroundedTime;
			jumpMaxPostGroundedTime = _jumpMaxPostGroundedTime;
			maxFlySpeed = _maxFlySpeed;
			brakingDecelerationFlying = _brakingDecelerationFlying;
			flyingFriction = _flyingFriction;
			maxSwimSpeed = _maxSwimSpeed;
			brakingDecelerationSwimming = _brakingDecelerationSwimming;
			swimmingFriction = _swimmingFriction;
			buoyancy = _buoyancy;
			gravityScale = _gravityScale;
			useRootMotion = _useRootMotion;
			if (_characterMovement == null)
			{
				_characterMovement = GetComponent<CharacterMovement>();
			}
			impartPlatformVelocity = _impartPlatformVelocity;
			impartPlatformMovement = _impartPlatformMovement;
			impartPlatformRotation = _impartPlatformRotation;
			enablePhysicsInteraction = _enablePhysicsInteraction;
			applyPushForceToCharacters = _applyPushForceToCharacters;
			applyPushForceToCharacters = _applyPushForceToCharacters;
			mass = _mass;
			pushForceScale = _pushForceScale;
			standingDownwardForceScale = _standingDownwardForceScale;
		}

		protected virtual void Awake()
		{
			CacheComponents();
			SetMovementMode(_startingMovementMode);
		}

		protected virtual void OnEnable()
		{
			characterMovement.Collided += OnCollided;
			characterMovement.FoundGround += OnFoundGround;
			if (_enableAutoSimulation)
			{
				EnableAutoSimulationCoroutine(enable: true);
			}
		}

		protected virtual void OnDisable()
		{
			characterMovement.Collided -= OnCollided;
			characterMovement.FoundGround -= OnFoundGround;
			if (_enableAutoSimulation)
			{
				EnableAutoSimulationCoroutine(enable: false);
			}
		}

		protected virtual void Start()
		{
			if (_startingMovementMode == MovementMode.Walking)
			{
				characterMovement.SetPosition(transform.position, updateGround: true);
			}
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			AddPhysicsVolume(other);
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			RemovePhysicsVolume(other);
		}

		private IEnumerator LateFixedUpdate()
		{
			WaitForFixedUpdate waitTime = new WaitForFixedUpdate();
			while (true)
			{
				yield return waitTime;
				OnLateFixedUpdate();
			}
		}
	}
}
