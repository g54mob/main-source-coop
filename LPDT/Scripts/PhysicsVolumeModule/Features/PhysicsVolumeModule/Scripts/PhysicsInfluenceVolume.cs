using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.CustomSynchronizersModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.HeadwearModule.Scripts;
using Features.Movement.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting;

namespace Features.PhysicsVolumeModule.Scripts
{
	[NetworkBehaviourWeaved(3886)]
	public class PhysicsInfluenceVolume : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface, IGrabPassengerPolicy, ICargoControlRank, IRiderCarrierVolume
	{
		[StructLayout(LayoutKind.Explicit, Size = 28)]
		[NetworkStructWeaved(7)]
		public struct VolumeLockData : INetworkStruct
		{
			[FieldOffset(0)]
			public Vector3 LocalPosition;

			[FieldOffset(12)]
			public Quaternion LocalRotation;
		}

		[StructLayout(LayoutKind.Explicit, Size = 28)]
		[NetworkStructWeaved(7)]
		public struct CargoSeatData : INetworkStruct
		{
			[FieldOffset(0)]
			public Vector3 LocalPosition;

			[FieldOffset(12)]
			public Quaternion LocalRotation;
		}

		private sealed class TrackedBody
		{
			public Rigidbody Body;

			public NetworkObject NetworkObject;

			public Transform PartRoot;

			public IPointGrabable Grabable;

			public Headwear Headwear;

			public PhysicsSynchronizer BoneSynchronizer;

			public bool IsPartBone;

			public float RestTimer;

			public int HandOffAttempts;

			public bool IsResting;

			public bool WasKinematic;

			public bool IsSupported;

			public bool HadFrictionSupport;

			public bool HandVelocityShed;

			public bool IsCeded;

			public bool RigIgnored;

			public PhysicsInfluenceVolume CededTo;

			public bool ProxyKinematicForced;

			public bool ProxyCarrierDecoupled;

			public int KinematicStreak;

			public Vector3 LocalPosition;

			public Quaternion LocalRotation;

			public Collider[] IgnoredColliders;

			public CartAngularLock CartLock;

			public bool CartLockProbed;

			public bool CartAngleUnlockedBySlide;

			public float CartSlideLevelTimer;

			public bool CartPhysicsOverridden;

			public float CartOriginalLinearDamping;

			public float CartOriginalAngularDamping;

			public float CartOriginalMaxDepenetration;

			public bool IsNestedCarrier;

			public bool HasInfluenceSeat;

			public Vector3 InfluenceSeatLocal;

			public float SeatStallSeconds;

			public float PreviousSeatError;

			public bool WasRidingInfluence;

			public bool HasGrabStamp;

			public Vector3 GrabStampLocalPosition;

			public Quaternion GrabStampLocalRotation;

			public bool GrabStampWasKinematic;

			public bool IsPinnedToGrabStamp;

			public Vector3 GrabStampRenderLocalPosition;

			public Quaternion GrabStampRenderLocalRotation;

			public bool IsEasingToSimulation;

			public float EaseStartedAt;

			public float RestFloorDrop;

			public bool HasRestFloorDrop;
		}

		private sealed class RiderLock
		{
			public Rigidbody Body;

			public NetworkObject NetworkObject;

			public Vector3 RenderLocalOffset;

			public bool HasRenderOffset;
		}

		private struct RenderContinuityEntry
		{
			public Vector3 WorldError;

			public float StartedAt;

			public Vector3 LastIncomingWorld;

			public Vector3 LastIncomingLocal;

			public bool HasLast;

			public float LastSeenTrackedAt;

			public IPointGrabable Grabable;
		}

		private struct CargoSeatGraceEntry
		{
			public CargoRenderSeat Seat;

			public float DroppedAt;

			public IPointGrabable Grabable;

			public NetworkObject NetworkObject;
		}

		private struct CargoRenderSeat
		{
			public Vector3 LocalPosition;

			public Quaternion LocalRotation;

			public Vector3 LastStampedPosition;

			public float DivergedSince;

			public float SeatedAt;
		}

		private struct BoneRenderSeat
		{
			public Vector3 LocalPosition;

			public Quaternion LocalRotation;
		}

		private sealed class KinematicNeighbor
		{
			public Collider[] Colliders;

			public int Sweeps;

			public bool Decoupled;
		}

		private struct SeatRenderState
		{
			public Vector3 LocalPosition;

			public Quaternion LocalRotation;

			public Vector3 WorldPosition;

			public Quaternion WorldRotation;

			public float LastRenderedAt;
		}

		private struct SeatReleaseBridge
		{
			public Vector3 PositionOffset;

			public Quaternion RotationOffset;

			public float StartedAt;
		}

		private struct AuthorityHandoff
		{
			public Vector3 HeldLocalPosition;

			public Quaternion HeldLocalRotation;

			public float LostAt;
		}

		private struct SeatInterpBuffer
		{
			public Vector3 StartPosition;

			public Quaternion StartRotation;

			public Vector3 TargetPosition;

			public Quaternion TargetRotation;

			public float TargetAt;

			public float BlendSeconds;
		}

		private const int MAX_HANDOFF_ATTEMPTS = 3;

		private const float CARGO_CARRY_MAX_RELATIVE_SPEED = 3f;

		private const float CARRIER_MOVING_MIN_SPEED = 0.5f;

		private const float CARGO_INFLUENCE_LERP_RATE = 30f;

		private const float INFLUENCE_SEAT_DEADZONE = 0.08f;

		private const float INFLUENCE_SEAT_STEP_MAX = 0.05f;

		private const float CARGO_RIDE_ESCAPE_SPEED = 1.5f;

		private const float INFLUENCE_ROCK_DAMP_RATE = 6f;

		private const float INFLUENCE_SEAT_STALL_ERROR = 0.35f;

		private const float INFLUENCE_SEAT_STALL_SECONDS = 0.5f;

		private const float OUTER_BASIS_HOLD_SECONDS = 0.25f;

		private const float OUTER_BASIS_BRIDGE_SECONDS = 0.25f;

		private const float OUTER_BASIS_JUMP_DISTANCE = 0.5f;

		private const float CARGO_INFLUENCE_MIN_CARRIER_SPEED = 0.3f;

		private const float CLUSTER_DAMAGE_SUPPRESS_SECONDS = 1f;

		private const int MAX_AUTHORITY_REQUESTS_PER_TICK = 10;

		private const float CARGO_INFLUENCE_RECENT_MOTION_SECONDS = 1f;

		private const float CARGO_RIDE_MAX_UPWARD_SPEED = 0.6f;

		private const float CARGO_RIDE_TUMBLE_DAMP_RATE = 8f;

		private const float CARGO_RIDE_TUMBLE_MIN_RELATIVE_SPIN = 1f;

		[SerializeField]
		private PhysicsVolumeRegion _region;

		[Header("Floor")]
		[Tooltip("The floor this platform declares: a plane sitting ON the deck surface, its up vector the surface normal. Riders and their loose parts rest on it and must never end up behind it. Authored per platform — nothing infers it from geometry, because a platform knows which of its parts is walkable deck and which is rail, wheel or frame.")]
		[SerializeField]
		private Transform _floorPlane;

		[Tooltip("Half-size of the floor along the plane's own right (X) and forward (Y) axes, in metres. The plane is bounded: past these edges there is no floor, so a point out there is off the deck rather than standing on it. Measured in world metres regardless of the plane's scale.")]
		[SerializeField]
		private Vector2 _floorHalfExtents = new Vector2(2.5f, 4f);

		[Header("Cargo carry")]
		[Tooltip("RestLock (default): capture resting cargo and drive it kinematically 1:1 — rigid and exact (best for objects that must ride perfectly locked). PhysicalInfluence: never lock — keep the cargo a dynamic rigidbody and continuously pull its velocity toward the carrier, so it rides on real physics (friction + walls), adapted from the legacy cart but without its authority-seize (stays grabbable while driven).")]
		[SerializeField]
		private CargoCarryStrategy _cargoStrategy = CargoCarryStrategy.RestLock;

		[SerializeField]
		private float _restVelocityThreshold = 0.35f;

		[SerializeField]
		private float _restSeconds = 0.4f;

		[Tooltip("RestLock only. The authored sleep zone: cargo may only go kinematic while the BOTTOM of its colliders is inside this sub-region, so it reads as touching the deck regardless of how tall the body is. Author it as a thin slab on the deck surface. Leave empty to allow the lock anywhere in the detection region, which is the legacy behaviour.")]
		[SerializeField]
		private PhysicsVolumeLockZone _lockZone;

		[Header("Control rank (volume-vs-volume)")]
		[Tooltip("Dominance tier for volume-vs-volume arbitration: a higher rank may capture a lower rank's carrier body; a lower rank never captures a higher one. Cauldron 0, cart 1, platform 2, lift 3.")]
		[SerializeField]
		private int _controlRank;

		[Tooltip("Equal-rank arbitration: NeverControl — peers never capture each other (cauldrons, carts); NestBySize — bigger region encloses smaller (the platform family's legacy nesting).")]
		[SerializeField]
		private VolumeSameRankPolicy _sameRankPolicy = VolumeSameRankPolicy.NestBySize;

		[Header("Rider transfer")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _riderLinearTransfer = 1f;

		[Range(0f, 1f)]
		[SerializeField]
		private float _riderAngularTransfer = 1f;

		[Header("Ejection (velocity-change trigger)")]
		[SerializeField]
		private bool _ejectOnVelocityChange;

		[SerializeField]
		private float _ejectionVelocityChange = 3.5f;

		[SerializeField]
		private float _ejectionCooldownSeconds = 1f;

		[SerializeField]
		private float _ejectionRagdollRecoverySeconds = 2.5f;

		[Header("Exit region")]
		[Tooltip("Optional outward padding, in world metres, applied ONLY to the exit test: a tracked body counts as departed when it leaves the region grown by this much, while capture/entry still uses the core region. Lets a volume hold cargo riding pressed against its boundary (a yanked pot's coins at the bowl wall) without overextending the volume to capture objects outside the collider.")]
		[SerializeField]
		private float _exitRegionPadding;

		[Header("Tip release (tilt trigger)")]
		[SerializeField]
		private bool _releaseContentsWhenTipped = true;

		[SerializeField]
		private float _maxTiltAngle = 60f;

		[SerializeField]
		private float _tipRagdollRecoverySeconds = 2.5f;

		[Header("Cart slide (tilt trigger)")]
		[SerializeField]
		private bool _slideCartsWhenTilted;

		[Tooltip("Deck tilt (deg) where carts stop being held and the downhill influence begins.")]
		[SerializeField]
		private float _cartSlideStartAngle = 8f;

		[Tooltip("Deck tilt (deg) where the downhill influence reaches full strength.")]
		[SerializeField]
		private float _cartSlideFullAngle = 30f;

		[Tooltip("Downhill speed (m/s, deck-relative) blended in at the start angle.")]
		[SerializeField]
		private float _cartSlideGentleSpeed = 0.3f;

		[Tooltip("Downhill speed (m/s, deck-relative) blended in at the full angle.")]
		[SerializeField]
		private float _cartSlideFullSpeed = 2.5f;

		[Tooltip("How fast the cart's velocity blends toward the downhill target, 1/s.")]
		[SerializeField]
		private float _cartSlideAcceleration = 2.5f;

		[Tooltip("Seconds the deck must stay below the start angle before a freed cart is re-held and its angle re-locked — hysteresis so a swing's zero-crossings don't re-level the cart every period.")]
		[SerializeField]
		private float _cartSlideRelockSeconds = 1.5f;

		[Tooltip("Override the cart's damping/mass/depenetration into a realistic rigid body while slide-driven.")]
		[SerializeField]
		private bool _cartSlideRealisticPhysics = true;

		[Tooltip("Rigidbody linear damping while slide-driven (CartV2's authored 2.5 floats like syrup).")]
		[SerializeField]
		private float _cartSlideLinearDamping = 0.05f;

		[Tooltip("Rigidbody angular damping while slide-driven.")]
		[SerializeField]
		private float _cartSlideAngularDamping = 0.1f;

		[Tooltip("Depenetration speed cap while slide-driven, m/s — keeps the stamped, swinging deck from popping an overlapped cart through its own walls or roof.")]
		[SerializeField]
		private float _cartSlideMaxDepenetrationVelocity = 2f;

		[WeaverGenerated]
		[DefaultForProperty("Locks", 0, 1933)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SerializableDictionary<NetworkId, VolumeLockData> _Locks;

		[WeaverGenerated]
		[DefaultForProperty("CargoSeats", 1933, 1933)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SerializableDictionary<NetworkId, CargoSeatData> _CargoSeats;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedCarrierPosition", 3866, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedCarrierPosition;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedCarrierVelocity", 3869, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedCarrierVelocity;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedCarrierVelocityTick", 3872, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _NetworkedCarrierVelocityTick;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedCarrierRotation", 3873, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Quaternion _NetworkedCarrierRotation;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedCarrierAngularVelocity", 3877, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedCarrierAngularVelocity;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("NetworkedCarrierPathAnchor", 3880, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedCarrierPathAnchor;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("NetworkedCarrierPathAnchorSet", 3883, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _NetworkedCarrierPathAnchorSet;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("NetworkedCarrierPathT", 3884, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _NetworkedCarrierPathT;

		private const float RIDER_RENDER_MAX_LOCAL_STEP = 0.07f;

		private const float RIDER_RENDER_SNAP_DISTANCE = 0.5f;

		private const float SUPPORT_PROBE_DISTANCE = 2.5f;

		private const float OBSERVER_RAGDOLL_GLUE_GRACE = 0.15f;

		private const float OBSERVER_RAGDOLL_SEAT_DEADBAND = 0.2f;

		private const float OBSERVER_SEAT_OFFSET_CATCHUP = 1f;

		private const float OBSERVER_GLUE_CARRIER_MOTION = 0.0008f;

		private static readonly RaycastHit[] _supportHits = new RaycastHit[32];

		private static int _cartShellLayer = -1;

		private readonly Dictionary<Rigidbody, Vector3> _constraintPhysicsPrevious = new Dictionary<Rigidbody, Vector3>();

		private readonly Dictionary<Rigidbody, Vector3> _constraintRenderPrevious = new Dictionary<Rigidbody, Vector3>();

		private readonly List<Rigidbody> _constraintScratch = new List<Rigidbody>();

		private readonly Dictionary<Rigidbody, TrackedBody> _tracked = new Dictionary<Rigidbody, TrackedBody>();

		private readonly Dictionary<Rigidbody, Collider[]> _seamDecoupled = new Dictionary<Rigidbody, Collider[]>();

		private readonly Dictionary<Rigidbody, Collider[]> _deferredCargo = new Dictionary<Rigidbody, Collider[]>();

		private readonly Dictionary<PlayerCharacterMovableBase, int> _riderRefs = new Dictionary<PlayerCharacterMovableBase, int>();

		private readonly Dictionary<Rigidbody, PlayerCharacterMovableBase> _bodyToPlayer = new Dictionary<Rigidbody, PlayerCharacterMovableBase>();

		private readonly Dictionary<PlayerCharacterMovableBase, RagdollEntity> _playerRagdolls = new Dictionary<PlayerCharacterMovableBase, RagdollEntity>();

		private readonly HashSet<PlayerCharacterMovableBase> _ragdollCarried = new HashSet<PlayerCharacterMovableBase>();

		private const float RAGDOLL_FLOOR_TAIL = 0.1f;

		private readonly Dictionary<PlayerCharacterMovableBase, float> _ragdollFloorTailUntil = new Dictionary<PlayerCharacterMovableBase, float>();

		private readonly HashSet<PlayerCharacterMovableBase> _ragdollModeScratch = new HashSet<PlayerCharacterMovableBase>();

		private readonly Dictionary<PlayerCharacterMovableBase, Vector3> _observerRagdollCorrections = new Dictionary<PlayerCharacterMovableBase, Vector3>();

		private readonly Dictionary<PlayerCharacterMovableBase, Vector3> _observerRagdollSeatCache = new Dictionary<PlayerCharacterMovableBase, Vector3>();

		private readonly Dictionary<PlayerCharacterMovableBase, float> _observerRagdollGlueUntil = new Dictionary<PlayerCharacterMovableBase, float>();

		private readonly Dictionary<PlayerCharacterMovableBase, Vector3> _observerRagdollSeatOffset = new Dictionary<PlayerCharacterMovableBase, Vector3>();

		private Vector3 _observerGlueLastCarrierPosition;

		private Quaternion _observerGlueLastCarrierRotation = Quaternion.identity;

		private int _observerGlueLastCarrierFrame = -1;

		private readonly HashSet<PlayerCharacterMovableBase> _observerRagdollScratch = new HashSet<PlayerCharacterMovableBase>();

		private readonly Dictionary<PlayerCharacterMovableBase, Collider[]> _decoupledPlayers = new Dictionary<PlayerCharacterMovableBase, Collider[]>();

		private readonly Dictionary<PlayerCharacterMovableBase, RiderLock> _riderLocks = new Dictionary<PlayerCharacterMovableBase, RiderLock>();

		private readonly Dictionary<PlayerCharacterMovableBase, Vector3> _retainedRagdollSeat = new Dictionary<PlayerCharacterMovableBase, Vector3>();

		private readonly Dictionary<NetworkObject, IPointGrabable> _replicatedGrabables = new Dictionary<NetworkObject, IPointGrabable>();

		private readonly Dictionary<PlayerCharacterMovableBase, float> _carryExcludedUntil = new Dictionary<PlayerCharacterMovableBase, float>();

		private readonly List<PlayerCharacterMovableBase> _carryExcludeScratch = new List<PlayerCharacterMovableBase>();

		private const float FLOOR_EXIT_SKIN = 0.35f;

		private const float CEDED_FLOOR_SLACK = 0.08f;

		private const float RISING_CARRIER_MIN_SPEED = 0.1f;

		private const float CARGO_FLOOR_STEP_SLACK = 0.002f;

		private readonly List<Rigidbody> _floorSkinEscapeScratch = new List<Rigidbody>();

		private const float MAX_TRANSPORT_EXTRAPOLATION = 0.3f;

		private const float ARC_EXTRAPOLATION_MIN_ANGLE = 0.01f;

		private const int TICK_PIPELINE_FLOOR_TICKS = 3;

		private const float FOLLOW_TAU = 0.08f;

		private const float TELEPORT_DISTANCE = 25f;

		private const float CARRIER_FOLLOW_MAX_ANGLE_SPEED = 360f;

		private const float CARRIER_FOLLOW_ANGLE_CATCHUP_GAIN = 5f;

		private const float TILT_FOLLOW_TAU_HELD = 0.25f;

		private const float TILT_FOLLOW_TAU_FREE = 0.08f;

		private const float TILT_FOLLOW_MAX_ANGLE_SPEED = 360f;

		private const float TILT_FOLLOW_ANGLE_CATCHUP_GAIN = 5f;

		private bool _followerActive;

		public Func<Vector3, Vector3> CarrierPathConstraint;

		public Func<Vector3> CarrierVelocityProvider;

		private const float CONSTRAINED_TARGET_CORRECTION_TAU = 0.25f;

		private Vector3 _deadReckonedTarget;

		private bool _deadReckonValid;

		private readonly Dictionary<Rigidbody, Collider> _boneColliders = new Dictionary<Rigidbody, Collider>();

		private readonly Dictionary<Rigidbody, float> _boneBottomOffsets = new Dictionary<Rigidbody, float>();

		private readonly List<Rigidbody> _boneCachePruneScratch = new List<Rigidbody>();

		private const float BONE_FLOOR_FOOTPRINT_MARGIN = 0.25f;

		private bool _frameBasisFromOuter;

		private Vector3 _frameBasisPosition;

		private Quaternion _frameBasisRotation = Quaternion.identity;

		private const int MAX_NESTED_BASIS_DEPTH = 4;

		private static int _basisResolveDepth;

		private Vector3 _predictedPosition;

		private Quaternion _predictedRotation;

		private int _lastSeenCarrierTick;

		private float _carrierStateReceivedAt;

		private Vector3 _lastInactiveRenderedPosition;

		private Quaternion _lastInactiveRenderedRotation = Quaternion.identity;

		private float _lastInactiveRenderedAt = float.NegativeInfinity;

		private readonly Dictionary<Rigidbody, CargoRenderSeat> _cargoRenderSeats = new Dictionary<Rigidbody, CargoRenderSeat>();

		private readonly Dictionary<Rigidbody, float> _ownedChurnLastStampTime = new Dictionary<Rigidbody, float>();

		private const float OWNED_CHURN_REENGAGE_SECONDS = 0.2f;

		private readonly Dictionary<Rigidbody, float> _seatRowAbsentSince = new Dictionary<Rigidbody, float>();

		private const float SEAT_ROW_ABSENT_RELEASE_SECONDS = 0.15f;

		private readonly Dictionary<Rigidbody, RenderContinuityEntry> _renderContinuity = new Dictionary<Rigidbody, RenderContinuityEntry>();

		private readonly List<Rigidbody> _renderContinuityPruneScratch = new List<Rigidbody>();

		private readonly List<Rigidbody> _renderContinuityKeysScratch = new List<Rigidbody>();

		private const float DEPARTURE_BRIDGE_SECONDS = 0.25f;

		private const float DEPARTURE_BRIDGE_MAX_ERROR_M = 2f;

		private const float DEPARTURE_SNAP_ENGAGE_M = 0.3f;

		private const float DEPARTURE_WATCH_SECONDS = 1.5f;

		private const float CARRIER_AUTHORITY_SETTLE_SECONDS = 1f;

		private int _lastCarrierAuthorityId = -1;

		private float _carrierAuthorityChangedAt = float.NegativeInfinity;

		private const float CARGO_SEAT_SNAP_DISTANCE = 0.25f;

		private const float CARGO_SEAT_SNAP_ANGLE = 10f;

		private const float CARGO_SEAT_TRACK_TAU = 0.4f;

		private const float CARGO_SEAT_MAX_TRACK_STEP = 0.002f;

		private const float CARGO_SEAT_MAX_TRACK_ANGLE_STEP = 0.5f;

		private const float CARGO_SEAT_CALM_SPEED = 0.05f;

		private const float CARGO_SEAT_CALM_ANGLE_SPEED = 3f;

		private const float CARGO_SEAT_MOTION_TAU = 0.25f;

		private const float CARGO_SEAT_CALM_DWELL = 0.6f;

		private const float CARGO_SEAT_CALM_SPEED_HELD = 0.015f;

		private const float CARGO_SEAT_CALM_ANGLE_SPEED_HELD = 1f;

		private const float CARGO_SEAT_CALM_DWELL_HELD = 3f;

		private const float CARGO_SEAT_FULL_CALM_FRACTION = 0.4f;

		private const float CARGO_SEAT_RECONCILE_EASE_IN_SECONDS = 0.5f;

		private const float CARGO_SEAT_RECONCILE_EASE_OUT_SECONDS = 0.1f;

		private float _cargoSeatReconcileWeight;

		private float _carrierLinearSpeedInstant;

		private float _carrierAngularSpeedInstant;

		private const float CARGO_SEAT_GRACE_SECONDS = 0f;

		private const float CARGO_SEAT_ENTRY_EASE_SECONDS = 0.35f;

		private const float CARRIER_HANDBACK_EASE_SECONDS = 0.35f;

		private const float CARGO_SEAT_DIVERGENCE_DISTANCE = 0.5f;

		private const float CARGO_SEAT_DIVERGENCE_SECONDS = 0.7f;

		private readonly Dictionary<Rigidbody, CargoSeatGraceEntry> _cargoSeatGrace = new Dictionary<Rigidbody, CargoSeatGraceEntry>();

		private readonly List<Rigidbody> _cargoSeatGracePruneScratch = new List<Rigidbody>();

		private readonly List<Rigidbody> _cargoSeatGraceKeysScratch = new List<Rigidbody>();

		private Vector3 _lastRawCarrierPosition;

		private Quaternion _lastRawCarrierRotation = Quaternion.identity;

		private Vector3 _lastPredictedPositionForCalm;

		private Quaternion _lastPredictedRotationForCalm = Quaternion.identity;

		private Vector3 _lastComposedOuterBasisOffset;

		private Quaternion _lastComposedOuterBasisRotationOffset = Quaternion.identity;

		private float _lastComposedOuterBasisAt = -999f;

		private Vector3 _outerBasisBridgeFromOffset;

		private Quaternion _outerBasisBridgeFromRotationOffset = Quaternion.identity;

		private float _outerBasisBridgeStartAt = -999f;

		private float _carrierLinearSpeedSmoothed;

		private float _carrierAngularSpeedSmoothed;

		private float _carrierCalmStreak;

		private readonly HashSet<NetworkObject> _ragdollCargoParts = new HashSet<NetworkObject>();

		private readonly Dictionary<NetworkObject, PhysicsSynchronizer[]> _partSynchronizers = new Dictionary<NetworkObject, PhysicsSynchronizer[]>();

		private NetworkTRSP _carrierTrsp;

		private const float BONE_SEAT_FOLLOW_RATE = 6f;

		private const float BONE_SEAT_HARD_SNAP_DISTANCE = 0.35f;

		private readonly Dictionary<PhysicsSynchronizer, BoneRenderSeat> _boneRenderSeats = new Dictionary<PhysicsSynchronizer, BoneRenderSeat>();

		private readonly List<PhysicsSynchronizer> _boneSeatPruneScratch = new List<PhysicsSynchronizer>();

		private Vector3 _lastFixedPosition;

		private Vector3 _carrierVelocity;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedSettledWeight", 3885, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _NetworkedSettledWeight;

		private float _measuredWeight;

		private readonly HashSet<Rigidbody> _weightSettled = new HashSet<Rigidbody>();

		private readonly HashSet<Rigidbody> _weightSettledNext = new HashSet<Rigidbody>();

		private Vector3 _carrierFixedDelta;

		private Vector3 _previousCarrierVelocity;

		private float _carrierLastMovingTime = float.NegativeInfinity;

		private float _lastEjectionTime = float.NegativeInfinity;

		private int _velocityTracerWarmup;

		private int _influenceRequestCounter;

		private bool _lockedByOuterThisTick;

		private CargoSimulationCluster _cluster;

		private readonly List<Rigidbody> _clusterMembers = new List<Rigidbody>();

		private readonly List<Rigidbody> _clusterDeparted = new List<Rigidbody>();

		private readonly List<Collider> _clusterColliderScratch = new List<Collider>();

		private CancellationTokenSource _lifetimeCts;

		[Tooltip("The carrier's solid ride-surface colliders — exactly the ones the virtual rig clones and that content is decoupled from. Assign explicitly so unrelated child colliders (grabbables, cosmetic props) are NOT swept in. Left empty, it falls back to scanning all child colliders.")]
		[SerializeField]
		private Collider[] _carrierColliders;

		[Tooltip("Slot a rider to this carrier's floor with a hard positional set instead of the ordinary orbit. For a pot deep enough that a passenger belongs AT the bottom rather than merely on it. The slot releases on a jump, on tilt past the release angle, and on ragdoll (which covers downed and dead). Off by default: on an open deck a rider must be free to walk about.")]
		[SerializeField]
		private bool _slotRidersToFloor;

		[Tooltip("Carrier tilt, in degrees, past which a slotted rider is released back to ordinary physics.")]
		[SerializeField]
		private float _riderSlotReleaseAngle = 20f;

		[Tooltip("Hand-authored stand-in shapes for the virtual collider rig. Left empty, the rig clones the carrier colliders above. Assign colliders here to give cargo a purpose-built ride surface — thicker walls, a simpler box, a deeper lip — independent of the carrier's real shape. They are DISABLED at build time and never collide with anything themselves; only the rig's clone is live.")]
		[SerializeField]
		private Collider[] _virtualColliderShapes;

		[Tooltip("Optional authored box whose ticked faces cargo may never cross. Left empty, one is looked up in the children. It decides nothing about what the volume carries — only which ways out are shut.")]
		[SerializeField]
		private PhysicsVolumeConstraintShape _constraintBox;

		private Rigidbody _carrierBody;

		private IPointGrabable _carrierGrabable;

		private VirtualColliderRig _virtualColliderRig;

		private static readonly List<PhysicsInfluenceVolume> _allVolumes = new List<PhysicsInfluenceVolume>();

		private bool _wasTipped;

		private readonly List<Rigidbody> _tipShedScratch = new List<Rigidbody>();

		private readonly List<Rigidbody> _tipReclaimScratch = new List<Rigidbody>();

		private readonly HashSet<PlayerCharacterMovableBase> _tipRagdolled = new HashSet<PlayerCharacterMovableBase>();

		private readonly List<PlayerCharacterMovableBase> _tipScratch = new List<PlayerCharacterMovableBase>();

		private bool _isWearableHeadwear;

		private const float CARGO_GRAB_EASE_SECONDS = 0.4f;

		private bool _grabPinActive;

		private const float CARRIER_ABOVE_PROBE_DISTANCE = 0.35f;

		private const float CARRIER_FLOOR_FOOT_TOLERANCE = 0.3f;

		private const float SHED_ACCELERATION = 2.5f;

		private const float SHED_SUSTAIN_SECONDS = 0.5f;

		private readonly Dictionary<PlayerCharacterMovableBase, float> _restingOnPlayerSeconds = new Dictionary<PlayerCharacterMovableBase, float>();

		private const float RIDER_DEPARTURE_SUSTAIN_SECONDS = 0.25f;

		private const float RIDER_DEPARTURE_PADDING = 0.75f;

		private readonly Dictionary<PlayerCharacterMovableBase, float> _riderAbsentSeconds = new Dictionary<PlayerCharacterMovableBase, float>();

		private static readonly Dictionary<Rigidbody, List<PhysicsInfluenceVolume>> _trackerIndex = new Dictionary<Rigidbody, List<PhysicsInfluenceVolume>>();

		private static readonly Stack<List<PhysicsInfluenceVolume>> _seatIndexListPool = new Stack<List<PhysicsInfluenceVolume>>();

		private static readonly List<PhysicsInfluenceVolume> _orderedVolumes = new List<PhysicsInfluenceVolume>();

		private static readonly Comparison<PhysicsInfluenceVolume> _innerFirstComparison = CompareInnerFirst;

		private static float _seatIndexStampedAt = float.NegativeInfinity;

		private static bool _isSeatIndexDirty = true;

		private static bool _isSeatIndexIterating;

		private int _seatOrderIndex = -1;

		private Vector3 _stampDepenetrationOffset;

		private const float STAMP_DEPENETRATION_SLACK = 0.02f;

		private const float STAMP_DEPENETRATION_MAX_STEP = 0.5f;

		private readonly List<Rigidbody> _partReleaseScratch = new List<Rigidbody>();

		private bool _enrollingPart;

		private const int MIN_PART_BONES = 4;

		private readonly Dictionary<Rigidbody, int> _exitVetoStreak = new Dictionary<Rigidbody, int>();

		private readonly Dictionary<Rigidbody, float> _exitVetoSince = new Dictionary<Rigidbody, float>();

		private readonly List<Rigidbody> _handoffScratch = new List<Rigidbody>();

		private readonly Dictionary<NetworkId, float> _localControlSince = new Dictionary<NetworkId, float>();

		private const float ROW_RETIRE_STREAM_OUTSIDE_M = 0.3f;

		private readonly Dictionary<PlayerCharacterMovableBase, IPointGrabable> _playerGrabables = new Dictionary<PlayerCharacterMovableBase, IPointGrabable>();

		private const int KINEMATIC_DECOUPLE_REFRESH_TICKS = 30;

		private const float KINEMATIC_NEIGHBOR_SKIRT_M = 0.6f;

		private const int KINEMATIC_NEIGHBOR_MIN_SWEEPS = 3;

		private static readonly Collider[] _neighborOverlapScratch = new Collider[256];

		private static readonly List<Rigidbody> _neighborRetireScratch = new List<Rigidbody>();

		private readonly Dictionary<Rigidbody, KinematicNeighbor> _kinematicNeighbors = new Dictionary<Rigidbody, KinematicNeighbor>();

		private int _neighborSweepCountdown;

		private const float GRAB_HOLD_GRACE_SECONDS = 0.25f;

		private float _lastHeldAt = -999f;

		private const float AUTHORITY_HANDOFF_EASE_TAU = 0.12f;

		private const float AUTHORITY_HANDOFF_EASE_MAX_SECONDS = 0.6f;

		private bool _authorityHandoffActive;

		private bool _carrierHandbackActive;

		private bool _carrierHandbackOffsetResolved;

		private float _carrierHandbackStartedAt;

		private Vector3 _carrierHandbackFromPosition;

		private Quaternion _carrierHandbackFromRotation = Quaternion.identity;

		private Vector3 _carrierHandbackOffset;

		private Quaternion _carrierHandbackRotationOffset = Quaternion.identity;

		private float _authorityHandoffStartedAt = float.NegativeInfinity;

		private Vector3 _authorityHandoffFromPosition;

		private Quaternion _authorityHandoffFromRotation = Quaternion.identity;

		private readonly List<NetworkId> _cargoSeatPruneIds = new List<NetworkId>();

		private readonly Dictionary<NetworkId, float> _seatStaleSince = new Dictionary<NetworkId, float>();

		private const float SEAT_STALE_HOLD_SECONDS = 0.4f;

		private const float EXIT_VETO_MAX_SECONDS = 0.5f;

		private const float HANDOFF_EASE_SECONDS = 0.4f;

		private const float HANDOFF_SEED_FRESH_SECONDS = 0.25f;

		private const float SEAT_FLIP_STABLE_DISTANCE = 0.3f;

		private const float HANDOFF_SEED_MAX_DIVERGENCE = 0.75f;

		private const float SEAT_STATE_PRUNE_SECONDS = 2f;

		private readonly Dictionary<NetworkId, AuthorityHandoff> _authorityHandoffs = new Dictionary<NetworkId, AuthorityHandoff>();

		private readonly Dictionary<NetworkId, SeatRenderState> _ownedSeatPoses = new Dictionary<NetworkId, SeatRenderState>();

		private readonly Dictionary<NetworkId, int> _lastSeenSeatAuthority = new Dictionary<NetworkId, int>();

		private const float SEAT_SNAPSHOT_INTERVAL_MIN = 0.02f;

		private const float SEAT_SNAPSHOT_INTERVAL_MAX = 0.15f;

		private readonly Dictionary<NetworkId, SeatInterpBuffer> _seatInterpBuffers = new Dictionary<NetworkId, SeatInterpBuffer>();

		private readonly Dictionary<NetworkId, SeatRenderState> _seatRenderStates = new Dictionary<NetworkId, SeatRenderState>();

		private readonly Dictionary<NetworkId, SeatReleaseBridge> _seatReleaseBridges = new Dictionary<NetworkId, SeatReleaseBridge>();

		private const float SEAT_RELEASE_CAPTURE_SECONDS = 0.1f;

		private const float SEAT_RELEASE_BRIDGE_SECONDS = 0f;

		private readonly List<NetworkId> _seatRenderStatePruneScratch = new List<NetworkId>();

		private readonly List<KeyValuePair<NetworkId, SeatRenderState>> _seatWarmScratch = new List<KeyValuePair<NetworkId, SeatRenderState>>();

		private static int _seatChainDepth;

		private readonly Dictionary<NetworkId, PhysicsSynchronizer> _seatSynchronizerCache = new Dictionary<NetworkId, PhysicsSynchronizer>();

		private readonly Dictionary<NetworkId, PhysicsInfluenceVolume> _hostedVolumeCache = new Dictionary<NetworkId, PhysicsInfluenceVolume>();

		private Vector3 _renderedSeatBasisPosition;

		private Quaternion _renderedSeatBasisRotation = Quaternion.identity;

		private float _renderedSeatBasisAt = float.NegativeInfinity;

		public bool IsCargoClusterMode { get; private set; }

		public static CargoSimulationClusterService ClusterService { get; set; }

		private CargoCarryStrategy EffectiveCargoStrategy
		{
			get
			{
				if (!IsCargoClusterMode)
				{
					return _cargoStrategy;
				}
				return CargoCarryStrategy.RestLock;
			}
		}

		[Networked]
		[Capacity(100)]
		[NetworkedWeaved(0, 1933)]
		[NetworkedWeavedDictionary(193, 1, 7, typeof(ElementReaderWriterUnmanaged<NetworkId, MetaConstant1>), typeof(ElementReaderWriterUnmanaged<VolumeLockData, MetaConstant7>))]
		private unsafe NetworkDictionary<NetworkId, VolumeLockData> Locks
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.Locks. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkDictionary<NetworkId, VolumeLockData>((int*)((byte*)Ptr + 0), 193, ElementReaderWriterUnmanaged<NetworkId, MetaConstant1>.GetInstance(), ElementReaderWriterUnmanaged<VolumeLockData, MetaConstant7>.GetInstance());
			}
		}

		[Networked]
		[Capacity(100)]
		[NetworkedWeaved(1933, 1933)]
		[NetworkedWeavedDictionary(193, 1, 7, typeof(ElementReaderWriterUnmanaged<NetworkId, MetaConstant1>), typeof(ElementReaderWriterUnmanaged<CargoSeatData, MetaConstant7>))]
		private unsafe NetworkDictionary<NetworkId, CargoSeatData> CargoSeats
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.CargoSeats. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkDictionary<NetworkId, CargoSeatData>(Ptr + 1933, 193, ElementReaderWriterUnmanaged<NetworkId, MetaConstant1>.GetInstance(), ElementReaderWriterUnmanaged<CargoSeatData, MetaConstant7>.GetInstance());
			}
		}

		[Networked]
		[NetworkedWeaved(3866, 3)]
		private unsafe Vector3 NetworkedCarrierPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3866);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3866) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3869, 3)]
		private unsafe Vector3 NetworkedCarrierVelocity
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3869);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3869) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3872, 1)]
		private unsafe int NetworkedCarrierVelocityTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierVelocityTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[3872];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierVelocityTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3872] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3873, 4)]
		private unsafe Quaternion NetworkedCarrierRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Quaternion*)(Ptr + 3873);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Quaternion*)(Ptr + 3873) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3877, 3)]
		private unsafe Vector3 NetworkedCarrierAngularVelocity
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierAngularVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3877);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierAngularVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3877) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3880, 3)]
		public unsafe Vector3 NetworkedCarrierPathAnchor
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPathAnchor. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3880);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPathAnchor. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3880) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3883, 1)]
		public unsafe NetworkBool NetworkedCarrierPathAnchorSet
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPathAnchorSet. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 3883);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPathAnchorSet. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3883) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3884, 1)]
		public unsafe float NetworkedCarrierPathT
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPathT. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 3884);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedCarrierPathT. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 3884) = value;
			}
		}

		public bool HasRiders
		{
			get
			{
				if (_riderRefs.Count <= 0)
				{
					return _ragdollCarried.Count > 0;
				}
				return true;
			}
		}

		private Vector3 EffectiveCarrierVelocity
		{
			get
			{
				if (CarrierVelocityProvider == null || (!(base.Object == null) && base.Object.IsValid && !base.Object.HasStateAuthority))
				{
					return _carrierVelocity;
				}
				return CarrierVelocityProvider();
			}
		}

		public Vector3 CarrierVelocity => _carrierVelocity;

		public Vector3 DrivenCarrierVelocity
		{
			get
			{
				if (!(base.Object != null) || !base.Object.IsValid || base.Object.HasStateAuthority)
				{
					return EffectiveCarrierVelocity;
				}
				return NetworkedCarrierVelocity;
			}
		}

		public float SettledWeight
		{
			get
			{
				if (!IsNetworked)
				{
					return _measuredWeight;
				}
				return NetworkedSettledWeight;
			}
		}

		[Networked]
		[NetworkedWeaved(3885, 1)]
		private unsafe float NetworkedSettledWeight
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedSettledWeight. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 3885);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsInfluenceVolume.NetworkedSettledWeight. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 3885) = value;
			}
		}

		public IReadOnlyCollection<Rigidbody> TrackedCargoBodies => _tracked.Keys;

		public int ClusterReleaseCount { get; private set; }

		public bool SlotsRidersToFloor => _slotRidersToFloor;

		public float RiderSlotReleaseAngle => _riderSlotReleaseAngle;

		private bool IsNetworked
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

		private bool IsCarrierSimulatedHere
		{
			get
			{
				if (_carrierBody != null)
				{
					return !_carrierBody.isKinematic;
				}
				return false;
			}
		}

		private bool IsTipped
		{
			get
			{
				if (_releaseContentsWhenTipped)
				{
					return DeckTiltDegrees > _maxTiltAngle;
				}
				return false;
			}
		}

		private float DeckTiltDegrees => Vector3.Angle(base.transform.up, Vector3.up);

		public static IReadOnlyList<PhysicsInfluenceVolume> ActiveVolumes => _allVolumes;

		public float RegionMeasure
		{
			get
			{
				if (!(_region != null))
				{
					return float.MaxValue;
				}
				return _region.WorldMeasure;
			}
		}

		private long ArbitrationKey
		{
			get
			{
				if (!IsNetworked)
				{
					return GetInstanceID();
				}
				return base.Object.Id.Raw;
			}
		}

		public Rigidbody CarrierBody => _carrierBody;

		public float CargoFloorWorldY
		{
			get
			{
				if (!(_region != null))
				{
					return float.NegativeInfinity;
				}
				return _region.FloorWorldY;
			}
		}

		public Transform DeclaredFloorPlane => _floorPlane;

		public Vector2 DeclaredFloorHalfExtents => _floorHalfExtents;

		public bool CarrierIsPoseStamped
		{
			get
			{
				if (_carrierBody != null)
				{
					return _carrierBody.isKinematic;
				}
				return false;
			}
		}

		public void SetCargoClusterMode(bool enabled)
		{
			if (IsCargoClusterMode == enabled)
			{
				return;
			}
			IsCargoClusterMode = enabled;
			if (!enabled)
			{
				if (_cargoStrategy == CargoCarryStrategy.RestLock)
				{
					return;
				}
				foreach (TrackedBody value in _tracked.Values)
				{
					if (!(value.Body == null))
					{
						if (value.IsResting || value.Body.isKinematic)
						{
							Unlock(value);
						}
						if (value.IgnoredColliders != null)
						{
							SetContentCarrierDecoupled(value.IgnoredColliders, decoupled: false);
						}
						value.RigIgnored = false;
					}
				}
				if (_virtualColliderRig != null)
				{
					_virtualColliderRig.Dispose();
					_virtualColliderRig = null;
				}
				return;
			}
			foreach (TrackedBody value2 in _tracked.Values)
			{
				if (!(value2.Body == null) && value2.IgnoredColliders != null)
				{
					EnsureRig().IncludeContentLayers(ContentLayerMask(value2.IgnoredColliders));
					if (IsActivelyGrabbed(value2.Grabable) || IsWornHeadwear(value2))
					{
						SetBodyInteractsWithRealCarrier(value2);
					}
					else
					{
						SetContentCarrierDecoupled(value2.IgnoredColliders, decoupled: true);
					}
				}
			}
		}

		public bool HasRiderOtherThan(IReadOnlyList<int> excludedPlayerIds)
		{
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				if (!IsExcludedRider(key, excludedPlayerIds))
				{
					return true;
				}
			}
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				if (!IsExcludedRider(item, excludedPlayerIds))
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsExcludedRider(PlayerCharacterMovableBase rider, IReadOnlyList<int> excludedPlayerIds)
		{
			if (rider == null || rider.Object == null)
			{
				return false;
			}
			int playerId = rider.Object.StateAuthority.PlayerId;
			for (int i = 0; i < excludedPlayerIds.Count; i++)
			{
				if (excludedPlayerIds[i] == playerId)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsRiderInShape(Rigidbody body, float riderHeight)
		{
			if (_constraintBox == null || body == null)
			{
				return false;
			}
			return _constraintBox.ContainsColumnLocal(_constraintBox.ToLocal(body.position), riderHeight);
		}

		public bool TryGetRiderSeatWorld(out Vector3 seatWorld)
		{
			seatWorld = Vector3.zero;
			if (_constraintBox == null)
			{
				return false;
			}
			if (!_constraintBox.TryGetFloorCentreLocal(out var floorCentreLocal))
			{
				return false;
			}
			seatWorld = _constraintBox.ToWorld(floorCentreLocal);
			return true;
		}

		private static bool IsCargoDespawned(TrackedBody tracked)
		{
			if (tracked.NetworkObject != null)
			{
				return !tracked.NetworkObject.IsValid;
			}
			return false;
		}

		private void Awake()
		{
			_carrierBody = GetComponent<Rigidbody>();
			_carrierGrabable = GetComponent<IPointGrabable>();
			_isWearableHeadwear = GetComponent<Headwear>() != null;
			if (_region == null)
			{
				_region = GetComponentInChildren<PhysicsVolumeRegion>();
			}
			if (_region == null)
			{
				Debug.LogError("[PhysicsInfluenceVolume] no PhysicsVolumeRegion assigned or found under '" + base.name + "'", this);
				base.enabled = false;
				return;
			}
			if (_constraintBox == null)
			{
				_constraintBox = GetComponentInChildren<PhysicsVolumeConstraintShape>();
			}
			if (_lockZone == null)
			{
				_lockZone = GetComponentInChildren<PhysicsVolumeLockZone>();
			}
			_lastFixedPosition = base.transform.position;
		}

		private void Start()
		{
			if (_region != null && EffectiveCargoStrategy != CargoCarryStrategy.None)
			{
				EnsureRig();
			}
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			if (!_allVolumes.Contains(this))
			{
				_allVolumes.Add(this);
			}
			InvalidateSeatScanIndexes();
			if (!(_region == null))
			{
				_region.OnBodyEntered += HandleBodyEntered;
				_region.OnBodyExited += HandleBodyExited;
				_region.OnSeamEntered += HandleSeamEntered;
				_region.OnSeamExited += HandleSeamExited;
				_lifetimeCts = new CancellationTokenSource();
			}
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_allVolumes.Remove(this);
			_seatOrderIndex = -1;
			InvalidateSeatScanIndexes();
			if (_allVolumes.Count == 0)
			{
				ReleaseSeatIndex(_trackerIndex);
				_orderedVolumes.Clear();
			}
			if (_region != null)
			{
				_region.OnBodyEntered -= HandleBodyEntered;
				_region.OnBodyExited -= HandleBodyExited;
				_region.OnSeamEntered -= HandleSeamEntered;
				_region.OnSeamExited -= HandleSeamExited;
			}
			_lifetimeCts?.Cancel();
			_lifetimeCts?.Dispose();
			_lifetimeCts = null;
			if (_cluster != null)
			{
				ClusterService?.Release(this);
				_cluster = null;
			}
			IsCargoClusterMode = false;
			RestoreAllKinematicNeighbors();
			ReleaseAll();
			if (_virtualColliderRig != null)
			{
				_virtualColliderRig.Dispose();
				_virtualColliderRig = null;
			}
		}

		private void FixedUpdate()
		{
			Vector3 position = base.transform.position;
			_carrierFixedDelta = position - _lastFixedPosition;
			_previousCarrierVelocity = _carrierVelocity;
			_carrierVelocity = _carrierFixedDelta / Time.fixedDeltaTime;
			_lastFixedPosition = position;
			TraceVelocityChange();
			_influenceRequestCounter = 0;
			_lockedByOuterThisTick = IsLockedByOuterVolume();
			RecoupleSimulatedNeighbors();
			SweepKinematicNeighbors();
			if (_region != null)
			{
				_floorSkinEscapeScratch.Clear();
				foreach (KeyValuePair<Rigidbody, TrackedBody> item in _tracked)
				{
					Rigidbody key = item.Key;
					if (key != null && _region.Sees(key))
					{
						continue;
					}
					if (key != null && WithinExitPadding(key))
					{
						ClearExitVeto(key);
					}
					else if (!(key != null) || !item.Value.IsPartBone || !PartSeamVisible(item.Value.PartRoot))
					{
						bool flag = ExitVetoExhausted(key, item.Value);
						if (!flag && key != null && !IsActivelyGrabbed(item.Value.Grabable) && !IsWornHeadwear(item.Value) && HasActiveSolidCollider(item.Value.IgnoredColliders) && (IsMidHardConstraintCrossing(key) || (!IsTipped && _region.ContainsWithFloorSkin(key.worldCenterOfMass, 0.35f))))
						{
							NoteExitVeto(key, "floor-skin");
						}
						else if (!flag && ShouldRetainProxyGluedCargo(key, item.Value))
						{
							NoteExitVeto(key, "proxy-glue");
						}
						else
						{
							_floorSkinEscapeScratch.Add(key);
						}
					}
				}
				foreach (Rigidbody item2 in _floorSkinEscapeScratch)
				{
					TrackedBody trackedBody = _tracked[item2];
					_seatRowAbsentSince.Remove(item2);
					_tracked.Remove(item2);
					InvalidateSeatScanIndexes();
					DropCargoSeatWithGrace(trackedBody);
					Release(trackedBody);
					if (item2 == null || !_region.SeamSees(item2))
					{
						SetContentCarrierDecoupled(trackedBody.IgnoredColliders, decoupled: false);
					}
					else
					{
						_seamDecoupled[item2] = trackedBody.IgnoredColliders;
					}
				}
			}
			foreach (TrackedBody value2 in _tracked.Values)
			{
				if (value2.Body == null || IsCargoDespawned(value2))
				{
					continue;
				}
				if (IsActivelyGrabbed(value2.Grabable) || IsWornHeadwear(value2))
				{
					if (value2.IsResting)
					{
						Unlock(value2);
					}
					ReleaseCartSlideClaim(value2);
					SetBodyInteractsWithRealCarrier(value2);
					value2.RestTimer = 0f;
					value2.HandOffAttempts = 0;
					value2.HasInfluenceSeat = false;
					value2.WasRidingInfluence = false;
					value2.HasRestFloorDrop = false;
					if (IsActivelyGrabbed(value2.Grabable))
					{
						UpdateProxyCargoKinematics(value2);
					}
					if (EffectiveCargoStrategy != CargoCarryStrategy.PhysicalInfluence)
					{
						continue;
					}
					if (value2.Grabable != null)
					{
						value2.Grabable.IsAuthorityRequested = true;
					}
					if (HasLocalControl(value2))
					{
						RestoreOriginalMass(value2);
						if (!value2.HandVelocityShed && !value2.Body.isKinematic)
						{
							value2.HandVelocityShed = true;
							value2.Body.linearVelocity = Vector3.zero;
							value2.Body.angularVelocity = Vector3.zero;
						}
					}
					continue;
				}
				value2.HandVelocityShed = false;
				if (TryGetInnerContainingVolume(value2.Body, out var _))
				{
					_handoffScratch.Add(value2.Body);
					continue;
				}
				if (TryGetCedingVolume(value2.Body, out var inner2))
				{
					value2.IsCeded = true;
					value2.CededTo = inner2;
					if (value2.IsResting)
					{
						Unlock(value2);
					}
					SetBodyInteractsWithRealCarrier(value2);
					value2.RestTimer = 0f;
					value2.HandOffAttempts = 0;
					value2.IsSupported = true;
					value2.HadFrictionSupport = false;
					continue;
				}
				value2.IsCeded = false;
				value2.CededTo = null;
				if (value2.RigIgnored)
				{
					RestoreRigCollision(value2);
					SetContentCarrierDecoupled(value2.IgnoredColliders, decoupled: true);
				}
				if (_slideCartsWhenTilted && TryGetCartLock(value2, out var cartLock) && UpdateCartSlideState(value2))
				{
					if (value2.IsResting)
					{
						Unlock(value2);
						SetContentCarrierDecoupled(value2.IgnoredColliders, decoupled: false);
					}
					value2.RestTimer = 0f;
					value2.HandOffAttempts = 0;
					value2.HasInfluenceSeat = false;
					DriveCartSlide(value2, cartLock);
					continue;
				}
				if (IsTipped)
				{
					if (value2.IsResting)
					{
						Unlock(value2);
						SetContentCarrierDecoupled(value2.IgnoredColliders, decoupled: false);
					}
					value2.RestTimer = 0f;
					value2.HandOffAttempts = 0;
					value2.HasInfluenceSeat = false;
					_tipShedScratch.Add(value2.Body);
					continue;
				}
				if (value2.IsResting && !HasLocalControl(value2))
				{
					value2.IsResting = false;
					value2.RestTimer = 0f;
					UnpublishLock(value2);
				}
				if (EffectiveCargoStrategy == CargoCarryStrategy.PhysicalInfluence)
				{
					DrivePhysicalInfluence(value2);
					continue;
				}
				if (HasInnerTrackingVolume(value2.Body))
				{
					value2.RestTimer = 0f;
					continue;
				}
				if (!value2.IsResting && HasLocalControl(value2) && TryGetPublishedLock(value2, out var published))
				{
					if (IsAtPublishedPose(value2, published))
					{
						AdoptLock(value2, published);
						continue;
					}
					UnpublishLock(value2);
					if (value2.Body.isKinematic && !IsWornHeadwear(value2))
					{
						value2.RestTimer = 0f;
						value2.Body.isKinematic = false;
					}
				}
				if (value2.IsResting || value2.Body.isKinematic || !HasLocalControl(value2))
				{
					continue;
				}
				bool isCarrierSimulatedHere = IsCarrierSimulatedHere;
				value2.IsSupported = true;
				value2.HadFrictionSupport = isCarrierSimulatedHere;
				if (!isCarrierSimulatedHere)
				{
					if (IsCarriedByCarrierAuthority(value2))
					{
						value2.Body.position += _carrierFixedDelta;
					}
				}
				else
				{
					Vector3 carrierVelocity = _carrierVelocity;
					Vector3 linearVelocity = value2.Body.linearVelocity;
					Vector3 vector = carrierVelocity;
					vector.y = 0f;
					Vector3 vector2 = linearVelocity - carrierVelocity;
					vector2.y = 0f;
					if (vector.magnitude >= 0.5f && vector2.magnitude <= 3f)
					{
						value2.Body.linearVelocity = new Vector3(carrierVelocity.x, linearVelocity.y, carrierVelocity.z);
					}
				}
				if ((isCarrierSimulatedHere ? (value2.Body.linearVelocity - _carrierBody.linearVelocity).magnitude : value2.Body.linearVelocity.magnitude) <= _restVelocityThreshold)
				{
					value2.RestTimer += Time.fixedDeltaTime;
				}
				else
				{
					value2.RestTimer = 0f;
					value2.HandOffAttempts = 0;
				}
				if (value2.RestTimer >= _restSeconds && IsInLockZone(value2))
				{
					if (value2.BoneSynchronizer != null)
					{
						TryCaptureRagdollPartAsWhole(value2);
					}
					else
					{
						CaptureOrHandOff(value2);
					}
				}
			}
			for (int i = 0; i < _tipShedScratch.Count; i++)
			{
				Rigidbody rigidbody = _tipShedScratch[i];
				if (!(rigidbody == null) && _tracked.TryGetValue(rigidbody, out var value))
				{
					Collider[] ignoredColliders = value.IgnoredColliders;
					ReleaseCargoTo(rigidbody);
					SetContentCarrierDecoupled(ignoredColliders, decoupled: false);
				}
			}
			_tipShedScratch.Clear();
			UpdateTipCustody();
			for (int j = 0; j < _handoffScratch.Count; j++)
			{
				Rigidbody rigidbody2 = _handoffScratch[j];
				if (!(rigidbody2 == null) && _tracked.ContainsKey(rigidbody2) && TryGetInnerContainingVolume(rigidbody2, out var inner3))
				{
					ReleaseCargoTo(rigidbody2);
					inner3.HandleBodyEntered(rigidbody2);
				}
			}
			_handoffScratch.Clear();
			PublishCargoSeats();
			UpdateRagdollModes();
			CarryRagdolls();
			PinCarriedRagdollRootsToSeat();
			ApplyTipRelease();
			ApplyRiderVetoes();
			ReleaseDepartedRiders();
			ShedCarrierRestingOnPlayers();
			ExpireCarryExclusions();
			UpdateSettledWeight();
		}

		private bool IsCarrierRestingOnPlayer(PlayerCharacterMovableBase player)
		{
			if (player == null || player.BodyCollider == null || player.Rigidbody == null)
			{
				return false;
			}
			if (_carrierGrabable == null)
			{
				return false;
			}
			if (IsActivelyGrabbed(_carrierGrabable))
			{
				return false;
			}
			if (_carrierBody == null)
			{
				return false;
			}
			Bounds bounds = player.BodyCollider.bounds;
			int num = Physics.RaycastNonAlloc(new Vector3(bounds.center.x, bounds.max.y - 0.05f, bounds.center.z), Vector3.up, _supportHits, 0.35f, -5, QueryTriggerInteraction.Ignore);
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (_supportHits[i].rigidbody == _carrierBody)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (TryGetCarrierFloorY(player.Rigidbody, out var floorY) && Mathf.Abs(bounds.min.y - floorY) <= 0.3f)
			{
				return false;
			}
			return true;
		}

		private void ShedCarrierRestingOnPlayers()
		{
			if (_riderRefs.Count == 0)
			{
				return;
			}
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				if (!IsCarrierRestingOnPlayer(key))
				{
					_restingOnPlayerSeconds.Remove(key);
					continue;
				}
				_restingOnPlayerSeconds.TryGetValue(key, out var value);
				value += Time.fixedDeltaTime;
				_restingOnPlayerSeconds[key] = value;
				if (!(value < 0.5f))
				{
					if (_riderLocks.ContainsKey(key))
					{
						ClearRiderOn(key);
						_restingOnPlayerSeconds[key] = value;
					}
					if (!(_carrierBody == null) && !_carrierBody.isKinematic && (!IsNetworked || base.Object.HasStateAuthority))
					{
						Vector3 vector = _carrierBody.worldCenterOfMass - key.Rigidbody.worldCenterOfMass;
						vector.y = 0f;
						Vector3 vector2 = ((vector.sqrMagnitude > 0.0001f) ? vector.normalized : key.transform.forward);
						_carrierBody.AddForce(vector2 * (2.5f * _carrierBody.mass), ForceMode.Force);
					}
				}
			}
		}

		private void ApplyRiderVetoes()
		{
			if (_riderRefs.Count == 0 && _ragdollCarried.Count == 0)
			{
				return;
			}
			_tipScratch.Clear();
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				if (key != null && key.RefusedCarrier == base.transform)
				{
					_tipScratch.Add(key);
				}
			}
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				if (item != null && item.RefusedCarrier == base.transform && !_tipScratch.Contains(item))
				{
					_tipScratch.Add(item);
				}
			}
			foreach (PlayerCharacterMovableBase item2 in _tipScratch)
			{
				ReleaseRiderCompletely(item2);
			}
		}

		private void ReleaseRiderCompletely(PlayerCharacterMovableBase player)
		{
			if (!(player == null))
			{
				_riderRefs.Remove(player);
				_ragdollCarried.Remove(player);
				_ragdollFloorTailUntil.Remove(player);
				_playerRagdolls.Remove(player);
				_retainedRagdollSeat.Remove(player);
				_riderAbsentSeconds.Remove(player);
				RecouplePlayer(player);
				ClearRiderOn(player);
			}
		}

		private bool IsPlayerPresentInRegion(PlayerCharacterMovableBase player)
		{
			foreach (PlayerCharacterMovableBase value in _bodyToPlayer.Values)
			{
				if (value == player)
				{
					return true;
				}
			}
			if (IsBodyWithinRegion(player.Rigidbody, 0.75f))
			{
				return true;
			}
			RagdollEntity ragdoll = GetRagdoll(player);
			if (ragdoll == null)
			{
				return false;
			}
			return IsBodyWithinRegion(ragdoll.RootPhysData.RigidBody, 0.75f);
		}

		private void ReleaseDepartedRiders()
		{
			if (_riderRefs.Count == 0 && _ragdollCarried.Count == 0 && _riderLocks.Count == 0)
			{
				_riderAbsentSeconds.Clear();
				return;
			}
			_ragdollModeScratch.Clear();
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				_ragdollModeScratch.Add(key);
			}
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				_ragdollModeScratch.Add(item);
			}
			foreach (PlayerCharacterMovableBase key2 in _riderLocks.Keys)
			{
				_ragdollModeScratch.Add(key2);
			}
			_tipScratch.Clear();
			foreach (PlayerCharacterMovableBase item2 in _ragdollModeScratch)
			{
				if (item2 == null)
				{
					continue;
				}
				if (IsPlayerPresentInRegion(item2))
				{
					_riderAbsentSeconds.Remove(item2);
					continue;
				}
				_riderAbsentSeconds.TryGetValue(item2, out var value);
				value += Time.fixedDeltaTime;
				_riderAbsentSeconds[item2] = value;
				if (value >= 0.25f)
				{
					_tipScratch.Add(item2);
				}
			}
			foreach (PlayerCharacterMovableBase item3 in _tipScratch)
			{
				ReleaseRiderCompletely(item3);
			}
		}

		private void DrivePhysicalInfluence(TrackedBody tracked)
		{
			if (!IsNetworked)
			{
				if (!_lockedByOuterThisTick && !tracked.Body.isKinematic)
				{
					ApplyCartMass(tracked);
					ApplyPhysicalCargoInfluence(tracked);
				}
				return;
			}
			UpdateProxyCargoKinematics(tracked);
			SetProxyCargoCarrierDecoupled(tracked);
			if (!base.Object.HasStateAuthority)
			{
				if (tracked.NetworkObject != null && tracked.NetworkObject.HasStateAuthority && tracked.Grabable != null)
				{
					tracked.Grabable.IsAuthorityRequested = false;
				}
			}
			else
			{
				if (_lockedByOuterThisTick)
				{
					return;
				}
				if (tracked.NetworkObject != null && !tracked.NetworkObject.HasStateAuthority)
				{
					RequestCargoAuthority(tracked);
					return;
				}
				if (tracked.Body.isKinematic)
				{
					tracked.Body.isKinematic = false;
					ShedMigratedCargoVelocity(tracked.Body);
				}
				if (tracked.Grabable != null)
				{
					tracked.Grabable.IsAuthorityRequested = true;
				}
				ApplyCartMass(tracked);
				if (tracked.IsNestedCarrier && !tracked.HasInfluenceSeat)
				{
					Vector3 vector = tracked.Body.linearVelocity - EffectiveCarrierVelocity;
					vector.y = 0f;
					if (vector.magnitude <= _restVelocityThreshold)
					{
						tracked.InfluenceSeatLocal = base.transform.InverseTransformPoint(tracked.Body.position);
						tracked.HasInfluenceSeat = true;
					}
				}
				ApplyPhysicalCargoInfluence(tracked);
			}
		}

		private void SetProxyCargoCarrierDecoupled(TrackedBody tracked)
		{
			if (!(tracked.NetworkObject == null))
			{
				bool flag = !tracked.NetworkObject.HasStateAuthority;
				if (flag != tracked.ProxyCarrierDecoupled)
				{
					tracked.ProxyCarrierDecoupled = flag;
					SetContentCarrierDecoupled(tracked.IgnoredColliders, flag);
				}
			}
		}

		private void UpdateProxyCargoKinematics(TrackedBody tracked)
		{
			if (tracked.NetworkObject == null || tracked.Body == null)
			{
				return;
			}
			if (tracked.NetworkObject.HasStateAuthority)
			{
				if (!tracked.ProxyKinematicForced)
				{
					return;
				}
				tracked.ProxyKinematicForced = false;
				if (tracked.BoneSynchronizer != null)
				{
					PhysicsSynchronizer[] partSynchronizers = GetPartSynchronizers(tracked.NetworkObject);
					foreach (PhysicsSynchronizer physicsSynchronizer in partSynchronizers)
					{
						if (physicsSynchronizer != null && physicsSynchronizer.Rigidbody != null && physicsSynchronizer.Rigidbody.isKinematic)
						{
							physicsSynchronizer.Rigidbody.isKinematic = false;
						}
					}
				}
				else
				{
					if (tracked.Body.isKinematic)
					{
						tracked.Body.isKinematic = false;
					}
					ShedMigratedCargoVelocity(tracked.Body);
				}
			}
			else if (tracked.BoneSynchronizer != null)
			{
				PhysicsSynchronizer[] partSynchronizers = GetPartSynchronizers(tracked.NetworkObject);
				foreach (PhysicsSynchronizer physicsSynchronizer2 in partSynchronizers)
				{
					if (physicsSynchronizer2 != null && physicsSynchronizer2.Rigidbody != null && !physicsSynchronizer2.Rigidbody.isKinematic)
					{
						physicsSynchronizer2.Rigidbody.isKinematic = true;
					}
				}
				tracked.ProxyKinematicForced = true;
			}
			else if (!tracked.Body.isKinematic)
			{
				tracked.Body.isKinematic = true;
				tracked.ProxyKinematicForced = true;
			}
		}

		private void ShedMigratedCargoVelocity(Rigidbody body)
		{
			if (!(body == null) && !body.isKinematic)
			{
				body.linearVelocity = new Vector3(_carrierVelocity.x, Mathf.Min(body.linearVelocity.y, 0f), _carrierVelocity.z);
				body.angularVelocity = Vector3.zero;
			}
		}

		private static bool OutsideVolumeKinematic(TrackedBody tracked)
		{
			if (!(tracked.NetworkObject != null) || !tracked.NetworkObject.IsValid)
			{
				return tracked.WasKinematic;
			}
			return !tracked.NetworkObject.HasStateAuthority;
		}

		private void RestoreForcedProxyKinematic(TrackedBody tracked)
		{
			if (!tracked.ProxyKinematicForced)
			{
				return;
			}
			tracked.ProxyKinematicForced = false;
			bool flag = OutsideVolumeKinematic(tracked);
			if (tracked.BoneSynchronizer == null)
			{
				if (tracked.Body != null && tracked.Body.isKinematic != flag)
				{
					tracked.Body.isKinematic = flag;
				}
				return;
			}
			foreach (TrackedBody value in _tracked.Values)
			{
				if (value != tracked && value.NetworkObject == tracked.NetworkObject)
				{
					return;
				}
			}
			PhysicsSynchronizer[] partSynchronizers = GetPartSynchronizers(tracked.NetworkObject);
			foreach (PhysicsSynchronizer physicsSynchronizer in partSynchronizers)
			{
				if (physicsSynchronizer != null && physicsSynchronizer.Rigidbody != null && physicsSynchronizer.Rigidbody.isKinematic != flag)
				{
					physicsSynchronizer.Rigidbody.isKinematic = flag;
				}
			}
			_partSynchronizers.Remove(tracked.NetworkObject);
		}

		private void ApplyCartMass(TrackedBody tracked)
		{
			IPointGrabable grabable = tracked.Grabable;
			if (grabable != null && grabable.WeightResetOnCart && !(tracked.Body == null) && !(grabable.CartMass <= 0f) && !Mathf.Approximately(tracked.Body.mass, grabable.CartMass))
			{
				tracked.Body.mass = grabable.CartMass;
			}
		}

		private void RestoreOriginalMass(TrackedBody tracked)
		{
			IPointGrabable grabable = tracked.Grabable;
			if (grabable != null && grabable.WeightResetOnCart && !(tracked.Body == null) && !(grabable.OriginalMass <= 0f) && !Mathf.Approximately(tracked.Body.mass, grabable.OriginalMass))
			{
				tracked.Body.mass = grabable.OriginalMass;
			}
		}

		private void RequestCargoAuthority(TrackedBody tracked)
		{
			IPointGrabable grabable = tracked.Grabable;
			if (grabable != null && !grabable.IsAuthorityRequested && _influenceRequestCounter < 10)
			{
				_influenceRequestCounter++;
				grabable.IsAuthorityRequested = true;
				grabable.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
			}
		}

		private void ApplyPhysicalCargoInfluence(TrackedBody tracked)
		{
			Rigidbody body = tracked.Body;
			Vector3 carrierVelocity = _carrierVelocity;
			Vector3 vector = body.linearVelocity - carrierVelocity;
			vector.y = 0f;
			if (tracked.WasRidingInfluence && vector.magnitude > 1.5f)
			{
				Vector3 vector2 = Vector3.ClampMagnitude(vector, 1.5f);
				body.linearVelocity = new Vector3(carrierVelocity.x + vector2.x, Mathf.Min(body.linearVelocity.y, 1.5f), carrierVelocity.z + vector2.z);
				vector = body.linearVelocity - carrierVelocity;
				vector.y = 0f;
			}
			tracked.WasRidingInfluence = vector.magnitude <= 1.5f;
			bool flag = _carrierVelocity.magnitude > 0.3f;
			if (flag)
			{
				_carrierLastMovingTime = Time.time;
			}
			bool flag2 = Time.time - _carrierLastMovingTime <= 1f;
			if (!flag && !flag2)
			{
				return;
			}
			bool flag3 = vector.magnitude <= 3f;
			if (!flag && (!flag3 || vector.magnitude <= _restVelocityThreshold))
			{
				return;
			}
			Vector3 b = new Vector3(carrierVelocity.x, body.linearVelocity.y, carrierVelocity.z);
			if (tracked.IsNestedCarrier && tracked.HasInfluenceSeat && flag3)
			{
				Vector3 vector3 = base.transform.TransformPoint(tracked.InfluenceSeatLocal) - body.position;
				vector3.y = 0f;
				float magnitude = vector3.magnitude;
				if (magnitude > 0.08f)
				{
					if (magnitude > 0.35f && magnitude >= tracked.PreviousSeatError - 0.001f)
					{
						tracked.SeatStallSeconds += Time.fixedDeltaTime;
					}
					else
					{
						tracked.SeatStallSeconds = 0f;
					}
					if (tracked.SeatStallSeconds > 0.5f)
					{
						tracked.HasInfluenceSeat = false;
						tracked.SeatStallSeconds = 0f;
					}
					else
					{
						body.position += Vector3.ClampMagnitude(vector3, 0.05f);
					}
				}
				else
				{
					tracked.SeatStallSeconds = 0f;
				}
				tracked.PreviousSeatError = magnitude;
			}
			if (tracked.IsNestedCarrier && flag3)
			{
				Vector3 angularVelocity = body.angularVelocity;
				float t = 6f * Time.fixedDeltaTime;
				angularVelocity.x = Mathf.Lerp(angularVelocity.x, 0f, t);
				angularVelocity.z = Mathf.Lerp(angularVelocity.z, 0f, t);
				body.angularVelocity = angularVelocity;
			}
			body.linearVelocity = Vector3.Lerp(body.linearVelocity, b, 30f * Time.fixedDeltaTime);
			if (flag3)
			{
				Vector3 linearVelocity = body.linearVelocity;
				float num = Mathf.Max(0f, carrierVelocity.y) + 0.6f;
				if (linearVelocity.y > num)
				{
					linearVelocity.y = num;
					body.linearVelocity = linearVelocity;
				}
				Vector3 vector4 = ((_carrierBody != null && !_carrierBody.isKinematic) ? _carrierBody.angularVelocity : Vector3.zero);
				if ((body.angularVelocity - vector4).magnitude > 1f)
				{
					body.angularVelocity = Vector3.Lerp(body.angularVelocity, vector4, 8f * Time.fixedDeltaTime);
				}
			}
		}

		private void UpdateSettledWeight()
		{
			if (IsNetworked && !base.Object.HasStateAuthority)
			{
				return;
			}
			if (_region == null)
			{
				_measuredWeight = 0f;
				return;
			}
			_weightSettledNext.Clear();
			float num = 0f;
			foreach (Rigidbody body in _region.Bodies)
			{
				if (!(body == null) && (_weightSettled.Contains(body) || (body.linearVelocity - EffectiveCarrierVelocity).magnitude <= _restVelocityThreshold))
				{
					_weightSettledNext.Add(body);
					num += body.mass;
				}
			}
			_weightSettled.Clear();
			foreach (Rigidbody item in _weightSettledNext)
			{
				_weightSettled.Add(item);
			}
			_measuredWeight = num;
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.HasStateAuthority)
			{
				NetworkedSettledWeight = _measuredWeight;
				NetworkedCarrierPosition = ((_carrierBody != null) ? _carrierBody.position : base.transform.position);
				NetworkedCarrierVelocity = EffectiveCarrierVelocity;
				NetworkedCarrierVelocityTick = base.Runner.Tick;
				NetworkedCarrierRotation = ((_carrierBody != null) ? _carrierBody.rotation : base.transform.rotation);
				NetworkedCarrierAngularVelocity = ((_carrierBody != null && !_carrierBody.isKinematic) ? _carrierBody.angularVelocity : Vector3.zero);
			}
		}

		private void SyncCargoCluster()
		{
			bool flag = !IsNetworked || base.Object == null || !base.Object.IsValid || base.Object.HasStateAuthority;
			if (!IsCargoClusterMode || !flag || ClusterService == null || _carrierBody == null)
			{
				if (_cluster != null)
				{
					ClusterService?.Release(this);
					_cluster = null;
				}
				return;
			}
			if (_cluster == null)
			{
				CollectColliders(_carrierBody.transform, _clusterColliderScratch);
				_cluster = ClusterService.Acquire(this, base.transform, _clusterColliderScratch);
				if (_cluster == null)
				{
					return;
				}
			}
			_cluster.SetCarrierRotation(base.transform.rotation);
			_clusterMembers.Clear();
			foreach (TrackedBody value in _tracked.Values)
			{
				if (value.Body == null || !HasLocalControl(value) || IsActivelyGrabbed(value.Grabable) || IsWornHeadwear(value))
				{
					continue;
				}
				_clusterMembers.Add(value.Body);
				value.Grabable?.SuppressCollisionDamageFor(1f);
				Vector3 localPosition;
				Quaternion localRotation;
				if (!_cluster.Contains(value.Body))
				{
					CollectColliders(value.Body.transform, _clusterColliderScratch);
					_cluster.Add(value.Body, base.transform, _clusterColliderScratch);
					if (!value.IsResting)
					{
						Lock(value);
					}
				}
				else if (_cluster.TryGetCarrierLocalPose(value.Body, out localPosition, out localRotation))
				{
					value.LocalPosition = localPosition;
					value.LocalRotation = localRotation;
					PublishLock(value);
				}
			}
			_cluster.PruneMissing(_clusterMembers);
			if (!_cluster.TryCollectDeparted(_clusterDeparted))
			{
				return;
			}
			foreach (Rigidbody item in _clusterDeparted)
			{
				if (!(item == null) && _tracked.ContainsKey(item))
				{
					ClusterReleaseCount++;
					ReleaseCargoTo(item);
				}
			}
		}

		public bool TryGetCarrierFloor(out float floorY, out string lowest, out int measured)
		{
			floorY = 0f;
			lowest = null;
			measured = 0;
			if (_carrierBody == null)
			{
				return false;
			}
			CollectColliders(_carrierBody.transform, _clusterColliderScratch);
			return CargoSimulationCluster.LowestPointIn(base.transform, _clusterColliderScratch, out floorY, out lowest, out measured);
		}

		public bool TryDescribeBasketShapes(ICollection<string> real, ICollection<string> copied)
		{
			if (_carrierBody == null)
			{
				return false;
			}
			CollectColliders(_carrierBody.transform, _clusterColliderScratch);
			CargoSimulationCluster.DescribeShapesIn(base.transform, _clusterColliderScratch, real);
			if (_cluster != null)
			{
				return _cluster.TryDescribeBasketShapes(copied);
			}
			return false;
		}

		public bool TryGetClusterBasketFloor(out float floorY, out string lowest, out int measured)
		{
			floorY = 0f;
			lowest = null;
			measured = 0;
			if (_cluster != null)
			{
				return _cluster.TryGetBasketFloor(out floorY, out lowest, out measured);
			}
			return false;
		}

		public bool TryGetBasketExtents(out Vector3 realSize, out Vector3 clusterSize, out Vector3 carrierScale)
		{
			realSize = Vector3.zero;
			clusterSize = Vector3.zero;
			carrierScale = base.transform.lossyScale;
			if (_cluster == null || !_cluster.TryGetBasketExtents(out clusterSize) || _carrierBody == null)
			{
				return false;
			}
			CollectColliders(_carrierBody.transform, _clusterColliderScratch);
			if (!CargoSimulationCluster.ExtentsIn(base.transform, _clusterColliderScratch, out var bounds))
			{
				return false;
			}
			realSize = bounds.size;
			return true;
		}

		public bool TryGetCargoExtents(out Vector3 realSize, out Vector3 clusterSize)
		{
			realSize = Vector3.zero;
			clusterSize = Vector3.zero;
			if (_cluster == null || !_cluster.TryGetFirstCargoExtents(out clusterSize))
			{
				return false;
			}
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!(value.Body == null) && value.IsResting)
				{
					CollectColliders(value.Body.transform, _clusterColliderScratch);
					if (CargoSimulationCluster.ExtentsIn(value.Body.transform, _clusterColliderScratch, out var bounds))
					{
						realSize = bounds.size;
						return true;
					}
				}
			}
			return false;
		}

		public bool TryGetFirstTrackedState(out bool isResting, out bool hasGrabable, out int grabbedByPlayers, out int grabbedBySomething)
		{
			isResting = false;
			hasGrabable = false;
			grabbedByPlayers = 0;
			grabbedBySomething = 0;
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!(value.Body == null))
				{
					isResting = value.IsResting;
					hasGrabable = value.Grabable != null;
					if (value.Grabable != null)
					{
						grabbedByPlayers = value.Grabable.GrabbedByPlayersCount;
						grabbedBySomething = value.Grabable.GrabbedBySomethingCount;
					}
					return true;
				}
			}
			return false;
		}

		public bool TryGetCargoInBasket(out Vector3 cargoLocal, out Vector3 basketCentre, out Vector3 basketSize)
		{
			cargoLocal = Vector3.zero;
			basketCentre = Vector3.zero;
			basketSize = Vector3.zero;
			if (_cluster != null)
			{
				return _cluster.TryGetFirstCargoInBasket(out cargoLocal, out basketCentre, out basketSize);
			}
			return false;
		}

		public bool TryGetClusterStatus(out Vector3 origin, out int bodies, out int basketColliders, out int clusterId)
		{
			origin = Vector3.zero;
			bodies = 0;
			basketColliders = 0;
			clusterId = 0;
			if (_cluster == null)
			{
				return false;
			}
			origin = _cluster.Origin;
			bodies = _cluster.BodyCount;
			basketColliders = _cluster.BasketColliderCount;
			clusterId = _cluster.Id;
			return true;
		}

		private static void CollectColliders(Transform root, List<Collider> into)
		{
			into.Clear();
			root.GetComponentsInChildren(includeInactive: true, into);
		}

		private void PublishCarrierFloorToCarts()
		{
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!(value.Body == null) && value.Body.TryGetComponent<CartGrabObject>(out var component))
				{
					if (TryGetCarrierFloorY(value.Body, out var floorY))
					{
						component.SetCarrierFloor(floorY);
					}
					else
					{
						component.ClearCarrierFloor();
					}
				}
			}
		}

		private void ApplyRestingLockPhysics()
		{
			foreach (TrackedBody value in _tracked.Values)
			{
				if (value.IsResting && !(value.Body == null) && HasLocalControl(value) && !IsActivelyGrabbed(value.Grabable) && !IsWornHeadwear(value))
				{
					value.Body.position = base.transform.TransformPoint(value.LocalPosition);
					value.Body.rotation = base.transform.rotation * value.LocalRotation;
					if (!value.Body.isKinematic)
					{
						value.Body.linearVelocity = Vector3.zero;
						value.Body.angularVelocity = Vector3.zero;
					}
				}
			}
		}

		internal void TickPhysicsApply()
		{
			PublishCarrierFloorToCarts();
			SyncCargoCluster();
			ApplyRemoteCarrierPhysics();
			ApplyRestingLockPhysics();
			ApplyReplicatedLockPhysics();
			ApplyReplicatedCargoSeatPhysics();
			_virtualColliderRig?.Follow(_stampDepenetrationOffset);
			ApplyRiderLockPhysics();
			EnforceCargoFloor();
			EnforceConstraintBox(_constraintPhysicsPrevious, killVelocity: true);
		}

		private void EnforceCargoFloor()
		{
			if (_region == null)
			{
				return;
			}
			float floorWorldY = _region.FloorWorldY;
			bool flag = EffectiveCarrierVelocity.y > 0.1f;
			float b = Mathf.Max(0f, EffectiveCarrierVelocity.y) * Time.fixedDeltaTime + 0.002f;
			foreach (TrackedBody value in _tracked.Values)
			{
				Rigidbody body = value.Body;
				if (body == null || body.isKinematic || !HasLocalControl(value) || IsActivelyGrabbed(value.Grabable) || IsWornHeadwear(value))
				{
					continue;
				}
				float num = LowestSolidY(value.IgnoredColliders, body);
				if (!flag)
				{
					if ((body.linearVelocity - EffectiveCarrierVelocity).magnitude <= _restVelocityThreshold)
					{
						value.RestFloorDrop = Mathf.Max(0f, floorWorldY - num);
						value.HasRestFloorDrop = true;
					}
					continue;
				}
				float num2 = (value.HasRestFloorDrop ? (floorWorldY - value.RestFloorDrop) : floorWorldY);
				if (value.IsCeded && value.CededTo != null && value.CededTo.CarrierIsPoseStamped)
				{
					num2 = Mathf.Max(num2, value.CededTo.CargoFloorWorldY - 0.08f);
				}
				if (!(num >= num2))
				{
					Vector3 position = body.position;
					position.y += Mathf.Min(num2 - num, b);
					body.position = position;
					Vector3 linearVelocity = body.linearVelocity;
					if (linearVelocity.y < 0f)
					{
						linearVelocity.y = 0f;
						body.linearVelocity = linearVelocity;
					}
				}
			}
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				RagdollEntity ragdoll = GetRagdoll(item);
				if (ragdoll == null || !ragdoll.HasStateAuthority || (!ragdoll.IsSimulated && !ragdoll.IsBlendingOut))
				{
					continue;
				}
				IReadOnlyList<Rigidbody> bones = ragdoll.Bones;
				for (int i = 0; i < bones.Count; i++)
				{
					Rigidbody rigidbody = bones[i];
					if (rigidbody == null || !_region.ContainsXZ(rigidbody.position, 0.25f))
					{
						continue;
					}
					float num3 = rigidbody.position.y - BoneBottomOffset(rigidbody);
					if (num3 >= floorWorldY)
					{
						continue;
					}
					Vector3 position2 = rigidbody.position;
					position2.y += floorWorldY - num3;
					rigidbody.position = position2;
					if (!rigidbody.isKinematic)
					{
						Vector3 linearVelocity2 = rigidbody.linearVelocity;
						if (linearVelocity2.y < 0f)
						{
							linearVelocity2.y = 0f;
							rigidbody.linearVelocity = linearVelocity2;
						}
					}
				}
			}
		}

		private float BoneBottomOffset(Rigidbody bone)
		{
			if (!_boneColliders.TryGetValue(bone, out var value) || value == null || !value.enabled || value.isTrigger)
			{
				PruneDeadBoneCaches();
				value = null;
				Collider[] componentsInChildren = bone.GetComponentsInChildren<Collider>();
				foreach (Collider collider in componentsInChildren)
				{
					if (collider != null && collider.enabled && !collider.isTrigger)
					{
						value = collider;
						break;
					}
				}
				_boneColliders[bone] = value;
			}
			if (value == null)
			{
				return 0f;
			}
			float num = Mathf.Max(0f, bone.position.y - value.bounds.min.y);
			_boneBottomOffsets[bone] = num;
			return num;
		}

		private void PruneDeadBoneCaches()
		{
			if (_boneColliders.Count < 64)
			{
				return;
			}
			_boneCachePruneScratch.Clear();
			foreach (KeyValuePair<Rigidbody, Collider> boneCollider in _boneColliders)
			{
				if (boneCollider.Key == null)
				{
					_boneCachePruneScratch.Add(boneCollider.Key);
				}
			}
			foreach (Rigidbody item in _boneCachePruneScratch)
			{
				_boneColliders.Remove(item);
				_boneBottomOffsets.Remove(item);
			}
		}

		private static bool HasActiveSolidCollider(Collider[] colliders)
		{
			if (colliders == null)
			{
				return false;
			}
			foreach (Collider collider in colliders)
			{
				if (collider != null && collider.enabled && !collider.isTrigger && collider.gameObject.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
		}

		private static float LowestSolidY(Collider[] colliders, Rigidbody fallback)
		{
			float num = float.MaxValue;
			if (colliders != null)
			{
				foreach (Collider collider in colliders)
				{
					if (collider != null && collider.enabled && !collider.isTrigger)
					{
						num = Mathf.Min(num, collider.bounds.min.y);
					}
				}
			}
			if (num != float.MaxValue)
			{
				return num;
			}
			return fallback.worldCenterOfMass.y;
		}

		public bool IsLockingCargo(NetworkId id)
		{
			VolumeLockData value;
			if (IsNetworked && Locks.Count > 0)
			{
				return Locks.TryGet(id, out value);
			}
			return false;
		}

		public bool TracksCargo(Rigidbody body)
		{
			if (body != null)
			{
				return _tracked.ContainsKey(body);
			}
			return false;
		}

		public bool RegionContains(Rigidbody body)
		{
			if (body != null && _region != null)
			{
				return _region.ContainsWithFloorSkin(body.worldCenterOfMass, 0f);
			}
			return false;
		}

		private bool TryGetHandlingVolume(Rigidbody body, out PhysicsInfluenceVolume holder)
		{
			holder = null;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.TracksCargo(body))
				{
					holder = physicsInfluenceVolume;
					return true;
				}
			}
			return false;
		}

		private void ReleaseCargoTo(Rigidbody body)
		{
			if (_tracked.TryGetValue(body, out var value))
			{
				_exitVetoStreak.Remove(body);
				_exitVetoSince.Remove(body);
				_seatRowAbsentSince.Remove(body);
				_tracked.Remove(body);
				InvalidateSeatScanIndexes();
				DropCargoSeatWithGrace(value, keepStamping: false);
				Release(value);
				_deferredCargo[body] = value.IgnoredColliders;
			}
		}

		private void OfferReleasedCargo(Rigidbody body)
		{
			if (body == null)
			{
				return;
			}
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume._deferredCargo.ContainsKey(body) && !physicsInfluenceVolume.TracksCargo(body) && !(physicsInfluenceVolume._region == null) && physicsInfluenceVolume._region.ContainsWithFloorSkin(body.worldCenterOfMass, 0f))
				{
					physicsInfluenceVolume.HandleBodyEntered(body);
					if (physicsInfluenceVolume.TracksCargo(body))
					{
						break;
					}
				}
			}
		}

		public bool IsBodyWithinRegion(Rigidbody body, float padding)
		{
			if (body != null && _region != null)
			{
				return _region.ContainsPadded(body.worldCenterOfMass, padding);
			}
			return false;
		}

		public bool OwnsCargo(Rigidbody body)
		{
			if (body == null || !_tracked.ContainsKey(body))
			{
				return false;
			}
			if (IsCededToInnerVolume(body))
			{
				return false;
			}
			return !IsLockedByOuterVolume();
		}

		public bool ControlsOver(PhysicsInfluenceVolume other)
		{
			if (other == null || other == this)
			{
				return false;
			}
			if (_controlRank != other._controlRank)
			{
				return _controlRank > other._controlRank;
			}
			if (_sameRankPolicy != VolumeSameRankPolicy.NestBySize || other._sameRankPolicy != VolumeSameRankPolicy.NestBySize)
			{
				return false;
			}
			float regionMeasure = RegionMeasure;
			float regionMeasure2 = other.RegionMeasure;
			if (regionMeasure != regionMeasure2)
			{
				return regionMeasure > regionMeasure2;
			}
			return ArbitrationKey > other.ArbitrationKey;
		}

		public bool ControlsOverCarrierOf(GameObject candidate)
		{
			if (candidate == null)
			{
				return false;
			}
			PhysicsInfluenceVolume physicsInfluenceVolume = candidate.GetComponentInParent<PhysicsInfluenceVolume>();
			if (physicsInfluenceVolume == null)
			{
				physicsInfluenceVolume = candidate.GetComponentInChildren<PhysicsInfluenceVolume>();
			}
			if (!(physicsInfluenceVolume == null) && !(physicsInfluenceVolume == this))
			{
				return ControlsOver(physicsInfluenceVolume);
			}
			return true;
		}

		public bool IsInnerTo(PhysicsInfluenceVolume other)
		{
			if (other == null || other == this)
			{
				return false;
			}
			if (_controlRank != other._controlRank)
			{
				return _controlRank < other._controlRank;
			}
			float regionMeasure = RegionMeasure;
			float regionMeasure2 = other.RegionMeasure;
			if (regionMeasure != regionMeasure2)
			{
				return regionMeasure < regionMeasure2;
			}
			return ArbitrationKey < other.ArbitrationKey;
		}

		private bool IsCededToInnerVolume(Rigidbody body)
		{
			PhysicsInfluenceVolume inner;
			return TryGetCedingVolume(body, out inner);
		}

		private static bool IsCargoOfBodysOwnVolume(Rigidbody body, PhysicsInfluenceVolume candidate)
		{
			if (body == null || candidate == null || candidate.CarrierBody == null)
			{
				return false;
			}
			PhysicsInfluenceVolume component = body.GetComponent<PhysicsInfluenceVolume>();
			if (component != null)
			{
				return component.TracksCargo(candidate.CarrierBody);
			}
			return false;
		}

		private static int CompareInnerFirst(PhysicsInfluenceVolume a, PhysicsInfluenceVolume b)
		{
			if ((object)a == b)
			{
				return 0;
			}
			if (a.IsInnerTo(b))
			{
				return -1;
			}
			if (!b.IsInnerTo(a))
			{
				return 0;
			}
			return 1;
		}

		private static void InvalidateSeatScanIndexes()
		{
			_isSeatIndexDirty = true;
		}

		private static void EnsureSeatScanIndexes()
		{
			if (_isSeatIndexIterating || (!_isSeatIndexDirty && _seatIndexStampedAt == Time.fixedTime))
			{
				return;
			}
			ReleaseSeatIndex(_trackerIndex);
			_orderedVolumes.Clear();
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				if (_allVolumes[i] != null)
				{
					_orderedVolumes.Add(_allVolumes[i]);
				}
			}
			_orderedVolumes.Sort(_innerFirstComparison);
			for (int j = 0; j < _orderedVolumes.Count; j++)
			{
				_orderedVolumes[j]._seatOrderIndex = j;
			}
			for (int k = 0; k < _allVolumes.Count; k++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[k];
				if (physicsInfluenceVolume == null || !physicsInfluenceVolume.isActiveAndEnabled)
				{
					continue;
				}
				foreach (Rigidbody key in physicsInfluenceVolume._tracked.Keys)
				{
					if (!(key == null))
					{
						SeatIndexBucket(_trackerIndex, key).Add(physicsInfluenceVolume);
					}
				}
			}
			_seatIndexStampedAt = Time.fixedTime;
			_isSeatIndexDirty = false;
		}

		private static List<PhysicsInfluenceVolume> SeatIndexBucket<TKey>(Dictionary<TKey, List<PhysicsInfluenceVolume>> index, TKey key)
		{
			if (index.TryGetValue(key, out var value))
			{
				return value;
			}
			return index[key] = ((_seatIndexListPool.Count > 0) ? _seatIndexListPool.Pop() : new List<PhysicsInfluenceVolume>());
		}

		private static void ReleaseSeatIndex<TKey>(Dictionary<TKey, List<PhysicsInfluenceVolume>> index)
		{
			foreach (KeyValuePair<TKey, List<PhysicsInfluenceVolume>> item in index)
			{
				item.Value.Clear();
				_seatIndexListPool.Push(item.Value);
			}
			index.Clear();
		}

		private bool HasInnerTrackingVolume(Rigidbody body)
		{
			if (body == null)
			{
				return false;
			}
			EnsureSeatScanIndexes();
			if (!_trackerIndex.TryGetValue(body, out var value))
			{
				return false;
			}
			_isSeatIndexIterating = true;
			try
			{
				for (int i = 0; i < value.Count; i++)
				{
					PhysicsInfluenceVolume physicsInfluenceVolume = value[i];
					if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.IsInnerTo(this) && !IsCargoOfBodysOwnVolume(body, physicsInfluenceVolume))
					{
						return true;
					}
				}
			}
			finally
			{
				_isSeatIndexIterating = false;
			}
			return false;
		}

		private bool IsLockedByAnotherVolume(NetworkId id)
		{
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.IsLockingCargo(id))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryGetInnerContainingVolume(Rigidbody body, out PhysicsInfluenceVolume inner)
		{
			inner = null;
			if (body == null)
			{
				return false;
			}
			PhysicsInfluenceVolume component = body.GetComponent<PhysicsInfluenceVolume>();
			EnsureSeatScanIndexes();
			int seatOrderIndex = _seatOrderIndex;
			if (seatOrderIndex < 0 || seatOrderIndex >= _orderedVolumes.Count || (object)_orderedVolumes[seatOrderIndex] != this)
			{
				return false;
			}
			for (int i = 0; i < seatOrderIndex; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _orderedVolumes[i];
				if (!(physicsInfluenceVolume == null) && (object)physicsInfluenceVolume != component && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.RegionContains(body) && !IsCargoOfBodysOwnVolume(body, physicsInfluenceVolume) && !IsCarrierLockedHere(physicsInfluenceVolume))
				{
					inner = physicsInfluenceVolume;
					return true;
				}
			}
			return false;
		}

		private bool TryGetCedingVolume(Rigidbody body, out PhysicsInfluenceVolume inner)
		{
			inner = null;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.TracksCargo(body) && !IsCargoOfBodysOwnVolume(body, physicsInfluenceVolume) && physicsInfluenceVolume.IsInnerTo(this) && !IsCarrierLockedHere(physicsInfluenceVolume))
				{
					inner = physicsInfluenceVolume;
					return true;
				}
			}
			return false;
		}

		private bool IsCarrierLockedHere(PhysicsInfluenceVolume inner)
		{
			VolumeLockData data;
			if (inner.IsNetworked)
			{
				return TryGetCargoLock(inner.Object.Id, out data);
			}
			Rigidbody carrierBody = inner.CarrierBody;
			if (carrierBody != null && _tracked.TryGetValue(carrierBody, out var value))
			{
				return value.IsResting;
			}
			return false;
		}

		private bool IsLockedByOuterVolume()
		{
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.IsCarrierLockedHere(this))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetCargoLock(NetworkId id, out VolumeLockData data)
		{
			data = default(VolumeLockData);
			if (IsNetworked)
			{
				return Locks.TryGet(id, out data);
			}
			return false;
		}

		public bool TryGetCargoSeat(NetworkId id, out CargoSeatData data)
		{
			data = default(CargoSeatData);
			if (IsNetworked)
			{
				return CargoSeats.TryGet(id, out data);
			}
			return false;
		}

		private void GetReplicatedSeatBasis(out Vector3 position, out Quaternion rotation)
		{
			if (IsNetworked && !base.Object.HasStateAuthority && _basisResolveDepth < 4)
			{
				NetworkId id = base.Object.Id;
				PhysicsInfluenceVolume physicsInfluenceVolume = null;
				for (int i = 0; i < _allVolumes.Count; i++)
				{
					PhysicsInfluenceVolume physicsInfluenceVolume2 = _allVolumes[i];
					if (!(physicsInfluenceVolume2 == this) && !(physicsInfluenceVolume2 == null) && physicsInfluenceVolume2.isActiveAndEnabled && physicsInfluenceVolume2.IsNetworked && (physicsInfluenceVolume2.TryGetCargoLock(id, out var _) || physicsInfluenceVolume2.TryGetCargoSeat(id, out var _)) && (physicsInfluenceVolume == null || physicsInfluenceVolume2.RegionMeasure < physicsInfluenceVolume.RegionMeasure || (physicsInfluenceVolume2.RegionMeasure == physicsInfluenceVolume.RegionMeasure && physicsInfluenceVolume2.ArbitrationKey < physicsInfluenceVolume.ArbitrationKey)))
					{
						physicsInfluenceVolume = physicsInfluenceVolume2;
					}
				}
				if (physicsInfluenceVolume != null)
				{
					PhysicsInfluenceVolume physicsInfluenceVolume3 = physicsInfluenceVolume;
					CargoSeatData data3 = default(CargoSeatData);
					VolumeLockData data4;
					bool num = physicsInfluenceVolume3.TryGetCargoLock(id, out data4);
					bool flag = !num && physicsInfluenceVolume3.TryGetCargoSeat(id, out data3);
					Vector3 point = (num ? data4.LocalPosition : data3.LocalPosition);
					Quaternion quaternion = (num ? data4.LocalRotation : data3.LocalRotation);
					if (flag && physicsInfluenceVolume3._seatRenderStates.TryGetValue(id, out var value) && Time.time - value.LastRenderedAt <= 0.1f)
					{
						point = value.LocalPosition;
						quaternion = value.LocalRotation;
					}
					_basisResolveDepth++;
					try
					{
						Vector3 position2;
						Quaternion rotation2;
						if (Time.time - physicsInfluenceVolume3._renderedSeatBasisAt <= 0.1f)
						{
							position2 = physicsInfluenceVolume3._renderedSeatBasisPosition;
							rotation2 = physicsInfluenceVolume3._renderedSeatBasisRotation;
						}
						else
						{
							physicsInfluenceVolume3.GetReplicatedSeatBasis(out position2, out rotation2);
						}
						position = Matrix4x4.TRS(position2, rotation2, physicsInfluenceVolume3.transform.lossyScale).MultiplyPoint3x4(point);
						rotation = rotation2 * quaternion;
					}
					finally
					{
						_basisResolveDepth--;
					}
					Vector3 vector = base.transform.position + _lastComposedOuterBasisOffset;
					_ = _lastComposedOuterBasisRotationOffset * base.transform.rotation;
					bool flag2 = Time.time - _outerBasisBridgeStartAt < 0.25f;
					if (!flag2 && Time.time - _lastComposedOuterBasisAt <= 0.35f && (position - vector).magnitude > 0.5f)
					{
						_outerBasisBridgeFromOffset = _lastComposedOuterBasisOffset;
						_outerBasisBridgeFromRotationOffset = _lastComposedOuterBasisRotationOffset;
						_outerBasisBridgeStartAt = Time.time;
						flag2 = true;
					}
					if (flag2)
					{
						float t = SmoothStep01((Time.time - _outerBasisBridgeStartAt) / 0.25f);
						position = Vector3.Lerp(base.transform.position + _outerBasisBridgeFromOffset, position, t);
						rotation = Quaternion.Slerp(_outerBasisBridgeFromRotationOffset * base.transform.rotation, rotation, t);
					}
					_lastComposedOuterBasisOffset = position - base.transform.position;
					_lastComposedOuterBasisRotationOffset = rotation * Quaternion.Inverse(base.transform.rotation);
					_lastComposedOuterBasisAt = Time.time;
					return;
				}
				if (Time.time - _lastComposedOuterBasisAt <= 0.25f)
				{
					position = base.transform.position + _lastComposedOuterBasisOffset;
					rotation = _lastComposedOuterBasisRotationOffset * base.transform.rotation;
					return;
				}
			}
			if (_followerActive)
			{
				position = _predictedPosition;
				rotation = _predictedRotation;
			}
			else
			{
				position = base.transform.position;
				rotation = base.transform.rotation;
			}
		}

		private void RefreshFromOuterCarrier()
		{
			if (!IsNetworked || base.Object.HasStateAuthority)
			{
				return;
			}
			NetworkId id = base.Object.Id;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if (!(physicsInfluenceVolume == this) && physicsInfluenceVolume.IsNetworked && physicsInfluenceVolume.TryGetCargoLock(id, out var data))
				{
					physicsInfluenceVolume.GetCarrierRenderPose(out var position, out var rotation);
					Vector3 position2 = Matrix4x4.TRS(position, rotation, physicsInfluenceVolume.transform.lossyScale).MultiplyPoint3x4(data.LocalPosition);
					base.transform.SetPositionAndRotation(position2, rotation * data.LocalRotation);
					break;
				}
			}
		}

		private bool TryGetOuterTrackingVolume(out PhysicsInfluenceVolume outer)
		{
			outer = null;
			if (_carrierBody == null || !IsNetworked || base.Object.HasStateAuthority || IsActivelyGrabbed(_carrierGrabable))
			{
				return false;
			}
			float regionMeasure = RegionMeasure;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if (!(physicsInfluenceVolume == this) && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.IsNetworked && physicsInfluenceVolume.TracksCargo(_carrierBody) && !(physicsInfluenceVolume.RegionMeasure < regionMeasure) && (physicsInfluenceVolume.RegionMeasure != regionMeasure || physicsInfluenceVolume.ArbitrationKey > ArbitrationKey) && (outer == null || physicsInfluenceVolume.RegionMeasure < outer.RegionMeasure || (physicsInfluenceVolume.RegionMeasure == outer.RegionMeasure && physicsInfluenceVolume.ArbitrationKey < outer.ArbitrationKey)))
				{
					outer = physicsInfluenceVolume;
				}
			}
			return outer != null;
		}

		private bool IsOuterTrackedCarrier()
		{
			PhysicsInfluenceVolume outer;
			return TryGetOuterTrackingVolume(out outer);
		}

		private bool TryResolveRenderBasis(out Vector3 position, out Quaternion rotation, out bool fromOuter)
		{
			fromOuter = false;
			if (_followerActive)
			{
				position = _predictedPosition;
				rotation = _predictedRotation;
				return true;
			}
			if (TryGetAuthorityHandoffEase(out position, out rotation))
			{
				return true;
			}
			position = default(Vector3);
			rotation = Quaternion.identity;
			if (_basisResolveDepth >= 4)
			{
				return false;
			}
			if (!TryGetOuterTrackingVolume(out var outer))
			{
				return false;
			}
			_basisResolveDepth++;
			try
			{
				fromOuter = outer.TryGetCargoRenderPose(_carrierBody, out position, out rotation);
				return fromOuter;
			}
			finally
			{
				_basisResolveDepth--;
			}
		}

		public bool TryGetCargoRenderPose(Rigidbody body, out Vector3 position, out Quaternion rotation)
		{
			position = default(Vector3);
			rotation = Quaternion.identity;
			if (body == null)
			{
				return false;
			}
			if (IsNetworked && _tracked.TryGetValue(body, out var value) && value.NetworkObject != null && CargoSeats.TryGet(value.NetworkObject.Id, out var value2))
			{
				Vector3 position2;
				Quaternion rotation2;
				if (Time.time - _renderedSeatBasisAt <= 0.1f)
				{
					position2 = _renderedSeatBasisPosition;
					rotation2 = _renderedSeatBasisRotation;
				}
				else
				{
					GetReplicatedSeatBasis(out position2, out rotation2);
				}
				Vector3 localPosition = value2.LocalPosition;
				Quaternion localRotation = value2.LocalRotation;
				if (_seatRenderStates.TryGetValue(value.NetworkObject.Id, out var value3) && Time.time - value3.LastRenderedAt <= 0.1f)
				{
					localPosition = value3.LocalPosition;
					localRotation = value3.LocalRotation;
				}
				position = Matrix4x4.TRS(position2, rotation2, base.transform.lossyScale).MultiplyPoint3x4(localPosition);
				rotation = rotation2 * localRotation;
				return true;
			}
			if (EffectiveCargoStrategy != CargoCarryStrategy.PhysicalInfluence)
			{
				return false;
			}
			if (!_cargoRenderSeats.TryGetValue(body, out var value4))
			{
				return false;
			}
			if (!TryResolveRenderBasis(out var position3, out var rotation3, out var _))
			{
				return false;
			}
			position = position3 + rotation3 * value4.LocalPosition;
			rotation = rotation3 * value4.LocalRotation;
			return true;
		}

		private bool IsCarriedByOuterVolume()
		{
			if (!IsNetworked)
			{
				return false;
			}
			NetworkId id = base.Object.Id;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if (!(physicsInfluenceVolume == this) && physicsInfluenceVolume.IsNetworked && physicsInfluenceVolume.IsLockingCargo(id))
				{
					return true;
				}
			}
			return false;
		}

		private void ApplyRemoteCarrierPhysics()
		{
			_stampDepenetrationOffset = Vector3.zero;
			if (!(_carrierBody == null) && IsNetworked && !base.Object.HasStateAuthority && !IsCarriedByOuterVolume())
			{
				if (!_carrierBody.isKinematic)
				{
					_carrierBody.isKinematic = true;
				}
				Vector3 stampPosition = base.transform.position;
				DepenetrateStampFromHeldCarriers(ref stampPosition);
				_stampDepenetrationOffset = stampPosition - base.transform.position;
				_carrierBody.position = stampPosition;
				_carrierBody.rotation = base.transform.rotation;
			}
		}

		private void DepenetrateStampFromHeldCarriers(ref Vector3 stampPosition)
		{
			float regionMeasure = RegionMeasure;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if (physicsInfluenceVolume == this || physicsInfluenceVolume._carrierBody == null || physicsInfluenceVolume._carrierBody.isKinematic || !IsActivelyGrabbed(physicsInfluenceVolume._carrierGrabable) || physicsInfluenceVolume.RegionMeasure <= regionMeasure || physicsInfluenceVolume.TracksCargo(_carrierBody))
				{
					continue;
				}
				Collider[] array = SolidCarrierColliders();
				Collider[] array2 = physicsInfluenceVolume.SolidCarrierColliders();
				foreach (Collider collider in array)
				{
					if (collider == null || !collider.enabled || collider.isTrigger)
					{
						continue;
					}
					foreach (Collider collider2 in array2)
					{
						if (!(collider2 == null) && collider2.enabled && !collider2.isTrigger && Physics.ComputePenetration(collider, collider.transform.position + vector, collider.transform.rotation, collider2, collider2.transform.position, collider2.transform.rotation, out var direction, out var distance) && distance > 0.02f)
						{
							vector += direction * distance;
						}
					}
				}
			}
			if (!(vector == Vector3.zero))
			{
				if (vector.magnitude > 0.5f)
				{
					vector = vector.normalized * 0.5f;
				}
				stampPosition += vector;
			}
		}

		private Collider[] SolidCarrierColliders()
		{
			if (_carrierColliders == null || _carrierColliders.Length == 0)
			{
				_carrierColliders = GetComponentsInChildren<Collider>(includeInactive: true);
			}
			return _carrierColliders;
		}

		private void HandleSeamEntered(Rigidbody body)
		{
			if (body.TryGetComponent<VirtualColliderRig>(out var _) || body.GetComponentInParent<IVolumeUncapturable>() != null)
			{
				return;
			}
			PlayerCharacterMovableBase componentInParent = body.GetComponentInParent<PlayerCharacterMovableBase>();
			if (componentInParent != null)
			{
				RegisterRider(body, componentInParent);
			}
			else
			{
				if (IsNavigatingAgent(body))
				{
					return;
				}
				if (IsDeadPartBone(body))
				{
					if (PartHasTrackedBone(body.transform.root))
					{
						HandleBodyEntered(body);
					}
				}
				else if (EffectiveCargoStrategy == CargoCarryStrategy.RestLock && !(body.GetComponentInParent<Headwear>() != null))
				{
					PhysicsInfluenceVolume component2 = body.GetComponent<PhysicsInfluenceVolume>();
					if ((!(component2 != null) || !(component2 != this) || ControlsOver(component2)) && !_seamDecoupled.ContainsKey(body) && !IsActivelyGrabbed(body.GetComponentInParent<IPointGrabable>()))
					{
						Collider[] componentsInChildren = body.GetComponentsInChildren<Collider>(includeInactive: true);
						EnsureRig().IncludeContentLayers(ContentLayerMask(componentsInChildren));
						SetContentCarrierDecoupled(componentsInChildren, decoupled: true);
						_seamDecoupled[body] = componentsInChildren;
					}
				}
			}
		}

		private void HandleSeamExited(Rigidbody body)
		{
			if (_bodyToPlayer.TryGetValue(body, out var value))
			{
				_bodyToPlayer.Remove(body);
				if (value.Rigidbody == null || body == value.Rigidbody)
				{
					UnregisterRider(value);
				}
				return;
			}
			if (_tracked.TryGetValue(body, out var value2) && value2.IsPartBone && !PartSeamVisible(value2.PartRoot))
			{
				_partReleaseScratch.Clear();
				foreach (KeyValuePair<Rigidbody, TrackedBody> item in _tracked)
				{
					if (item.Value.PartRoot == value2.PartRoot)
					{
						_partReleaseScratch.Add(item.Key);
					}
				}
				foreach (Rigidbody item2 in _partReleaseScratch)
				{
					HandleBodyExited(item2);
				}
			}
			if (_seamDecoupled.TryGetValue(body, out var value3))
			{
				_seamDecoupled.Remove(body);
				if (!(body != null) || !_tracked.ContainsKey(body))
				{
					SetContentCarrierDecoupled(value3, decoupled: false);
				}
			}
		}

		private static bool IsDeadPartBone(Rigidbody body)
		{
			if (ResolveBoneSynchronizer(body) == null)
			{
				return false;
			}
			Transform root = body.transform.root;
			if (root.GetComponentInChildren<PhysicsInfluenceVolume>(includeInactive: true) != null)
			{
				return false;
			}
			if (root.GetComponentInChildren<PlayerCharacterMovableBase>(includeInactive: true) != null)
			{
				return false;
			}
			if (root.GetComponentInChildren<Joint>(includeInactive: true) == null)
			{
				return false;
			}
			int num = 0;
			Rigidbody[] componentsInChildren = root.GetComponentsInChildren<Rigidbody>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (ResolveBoneSynchronizer(componentsInChildren[i]) != null && ++num >= 4)
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsNavigatingAgent(Rigidbody body)
		{
			NavMeshAgent component;
			return body.TryGetComponent<NavMeshAgent>(out component);
		}

		private bool PartHasTrackedBone(Transform partRoot)
		{
			if (partRoot == null)
			{
				return false;
			}
			foreach (KeyValuePair<Rigidbody, TrackedBody> item in _tracked)
			{
				if (item.Value.PartRoot == partRoot)
				{
					return true;
				}
			}
			return false;
		}

		private bool PartSeamVisible(Transform partRoot)
		{
			if (partRoot == null || _region == null)
			{
				return false;
			}
			foreach (KeyValuePair<Rigidbody, TrackedBody> item in _tracked)
			{
				if (!(item.Value.PartRoot != partRoot) && item.Key != null && _region.SeamSees(item.Key))
				{
					return true;
				}
			}
			return false;
		}

		private int ExitVetoStreakOf(Rigidbody body)
		{
			if (body == null)
			{
				return 0;
			}
			_exitVetoStreak.TryGetValue(body, out var value);
			return value;
		}

		private float SecondsVetoed(Rigidbody body)
		{
			if (body == null || !_exitVetoSince.TryGetValue(body, out var value))
			{
				return 0f;
			}
			return Time.time - value;
		}

		private bool HasSimulatedLocally(TrackedBody tracked)
		{
			if (tracked.NetworkObject == null)
			{
				return true;
			}
			NetworkId id = tracked.NetworkObject.Id;
			if (!_localControlSince.TryGetValue(id, out var value))
			{
				_localControlSince[id] = Time.time;
				return false;
			}
			return Time.time - value >= Time.fixedDeltaTime;
		}

		private bool ExitVetoExhausted(Rigidbody body, TrackedBody tracked)
		{
			if (SecondsVetoed(body) <= 0.5f)
			{
				return false;
			}
			if (!(tracked.NetworkObject == null))
			{
				return !CargoSeats.ContainsKey(tracked.NetworkObject.Id);
			}
			return true;
		}

		private bool IsCarriedByCarrierAuthority(TrackedBody tracked)
		{
			if (!IsNetworked || base.Object == null || !base.Object.IsValid || base.Object.HasStateAuthority)
			{
				return true;
			}
			if (tracked.NetworkObject == null)
			{
				return true;
			}
			if (!IsSeatPublishable(tracked))
			{
				return true;
			}
			return CargoSeats.ContainsKey(tracked.NetworkObject.Id);
		}

		private bool IsSeatPublishable(TrackedBody tracked)
		{
			if (tracked.Body != null && tracked.NetworkObject != null && !tracked.IsResting && tracked.BoneSynchronizer == null && !tracked.IsCeded && !IsActivelyGrabbed(tracked.Grabable) && !IsWornHeadwear(tracked) && !IsTipped && !tracked.CartAngleUnlockedBySlide && !HasInnerTrackingVolume(tracked.Body))
			{
				return !IsLockedByAnotherVolume(tracked.NetworkObject.Id);
			}
			return false;
		}

		private void ApplyRenderContinuityGuard()
		{
			foreach (TrackedBody value3 in _tracked.Values)
			{
				if (!(value3.Body == null) && value3.Body.isKinematic && !(value3.NetworkObject == null) && !value3.NetworkObject.HasStateAuthority && !(value3.BoneSynchronizer != null) && !IsActivelyGrabbed(value3.Grabable) && !IsWornHeadwear(value3))
				{
					if (!_renderContinuity.TryGetValue(value3.Body, out var value))
					{
						value = default(RenderContinuityEntry);
					}
					value.LastSeenTrackedAt = Time.time;
					value.Grabable = value3.Grabable;
					_renderContinuity[value3.Body] = value;
				}
			}
			if (_renderContinuity.Count == 0)
			{
				return;
			}
			GetReplicatedSeatBasis(out var position, out var rotation);
			Quaternion quaternion = Quaternion.Inverse(rotation);
			_renderContinuityPruneScratch.Clear();
			_renderContinuityKeysScratch.Clear();
			_renderContinuityKeysScratch.AddRange(_renderContinuity.Keys);
			foreach (Rigidbody item in _renderContinuityKeysScratch)
			{
				RenderContinuityEntry value2 = _renderContinuity[item];
				float num = Time.time - value2.LastSeenTrackedAt;
				float num2 = Time.time - value2.StartedAt;
				if (item == null || (num > 1.5f && num2 >= 0.25f))
				{
					_renderContinuityPruneScratch.Add(item);
					continue;
				}
				if (IsActivelyGrabbed(value2.Grabable))
				{
					_renderContinuityPruneScratch.Add(item);
					continue;
				}
				Vector3 position2 = item.transform.position;
				Vector3 vector = quaternion * (position2 - position);
				if (value2.HasLast)
				{
					Vector3 vector2 = position2 - value2.LastIncomingWorld;
					Vector3 vector3 = vector - value2.LastIncomingLocal;
					if (vector2.magnitude > 0.3f && vector3.magnitude > 0.3f && vector2.magnitude <= 2f)
					{
						float num3 = ((num2 >= 0.25f) ? 0f : (1f - SmoothStep01(num2 / 0.25f)));
						Vector3 vector4 = value2.LastIncomingWorld + value2.WorldError * num3;
						value2.WorldError = vector4 - position2;
						value2.StartedAt = Time.time;
						num2 = 0f;
					}
				}
				value2.LastIncomingWorld = position2;
				value2.LastIncomingLocal = vector;
				value2.HasLast = true;
				float num4 = ((num2 >= 0.25f) ? 0f : (1f - SmoothStep01(num2 / 0.25f)));
				if (num4 > 0f)
				{
					item.transform.position = position2 + value2.WorldError * num4;
				}
				_renderContinuity[item] = value2;
			}
			foreach (Rigidbody item2 in _renderContinuityPruneScratch)
			{
				_renderContinuity.Remove(item2);
			}
		}

		private bool OwnerRetiredSeatRow(TrackedBody tracked)
		{
			if (!IsNetworked || base.Object == null || !base.Object.IsValid || base.Object.HasStateAuthority)
			{
				return false;
			}
			if (tracked.Body == null || tracked.NetworkObject == null || HasLocalControl(tracked))
			{
				return false;
			}
			if (!IsSeatPublishable(tracked))
			{
				return false;
			}
			if (Time.time - _carrierAuthorityChangedAt < 1f)
			{
				return false;
			}
			if (ResolveHostedVolume(tracked.NetworkObject.Id, tracked.NetworkObject) != null)
			{
				return false;
			}
			if (TryGetCargoLock(tracked.NetworkObject.Id, out var _))
			{
				_seatRowAbsentSince.Remove(tracked.Body);
				return false;
			}
			if (CargoSeats.ContainsKey(tracked.NetworkObject.Id))
			{
				_seatRowAbsentSince.Remove(tracked.Body);
				return false;
			}
			if (!_seatRowAbsentSince.TryGetValue(tracked.Body, out var value))
			{
				_seatRowAbsentSince[tracked.Body] = Time.time;
				value = Time.time;
			}
			PhysicsSynchronizer physicsSynchronizer = ResolveSeatSynchronizer(tracked.NetworkObject);
			if (physicsSynchronizer != null && physicsSynchronizer.HasValidNetworkPose && _region != null && _region.DistanceOutside(physicsSynchronizer.Position) > 0.3f)
			{
				return true;
			}
			return Time.time - value > 0.15f;
		}

		private bool WithinExitPadding(Rigidbody body)
		{
			if (_exitRegionPadding > 0f && body != null && _region != null)
			{
				return _region.ContainsPadded(body.worldCenterOfMass, _exitRegionPadding);
			}
			return false;
		}

		private void ClearExitVeto(Rigidbody body)
		{
			_exitVetoStreak.Remove(body);
			_exitVetoSince.Remove(body);
		}

		private void NoteExitVeto(Rigidbody body, string veto)
		{
			if (!(body == null))
			{
				if (!_exitVetoSince.ContainsKey(body))
				{
					_exitVetoSince[body] = Time.time;
				}
				_exitVetoStreak.TryGetValue(body, out var value);
				value++;
				_exitVetoStreak[body] = value;
			}
		}

		private bool IsMidHardConstraintCrossing(Rigidbody body)
		{
			if (body == null || _constraintBox == null || !_constraintBox.HasHardSurface || IsTipped)
			{
				return false;
			}
			if (_constraintBox.ContainsWithinHardSurfaces(_constraintBox.ToLocal(body.worldCenterOfMass)))
			{
				return true;
			}
			if (_constraintPhysicsPrevious.TryGetValue(body, out var value))
			{
				return _constraintBox.ContainsWithinHardSurfaces(value);
			}
			return false;
		}

		private void HandleBodyEntered(Rigidbody body)
		{
			if (body.TryGetComponent<VirtualColliderRig>(out var _) || body.GetComponentInParent<IVolumeUncapturable>() != null || body.GetComponentInParent<PlayerCharacterMovableBase>() != null || IsNavigatingAgent(body) || IsCartShell(body) || _tracked.ContainsKey(body))
			{
				return;
			}
			if (IsTipped)
			{
				if (!_deferredCargo.ContainsKey(body))
				{
					_deferredCargo.Add(body, null);
				}
				return;
			}
			PhysicsInfluenceVolume component2 = body.GetComponent<PhysicsInfluenceVolume>();
			if (component2 != null && component2 != this && !ControlsOver(component2))
			{
				return;
			}
			if (TryGetHandlingVolume(body, out var holder))
			{
				if (!IsInnerTo(holder))
				{
					if (!_deferredCargo.ContainsKey(body))
					{
						_deferredCargo.Add(body, null);
					}
					return;
				}
				holder.ReleaseCargoTo(body);
			}
			_deferredCargo.Remove(body);
			_exitVetoStreak.Remove(body);
			_exitVetoSince.Remove(body);
			Collider[] componentsInChildren = body.GetComponentsInChildren<Collider>(includeInactive: true);
			IPointGrabable componentInParent = body.GetComponentInParent<IPointGrabable>();
			InvalidateSeatScanIndexes();
			_tracked.Add(body, new TrackedBody
			{
				Body = body,
				NetworkObject = body.GetComponentInParent<NetworkObject>(),
				PartRoot = body.transform.root,
				Grabable = componentInParent,
				Headwear = body.GetComponentInParent<Headwear>(),
				BoneSynchronizer = ResolveBoneSynchronizer(body),
				IsPartBone = IsDeadPartBone(body),
				IgnoredColliders = componentsInChildren,
				IsNestedCarrier = (component2 != null && component2 != this)
			});
			if (_tracked[body].IsPartBone && !_enrollingPart)
			{
				_enrollingPart = true;
				Rigidbody[] componentsInChildren2 = body.transform.root.GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody rigidbody in componentsInChildren2)
				{
					if (rigidbody != body && ResolveBoneSynchronizer(rigidbody) != null)
					{
						HandleBodyEntered(rigidbody);
					}
				}
				_enrollingPart = false;
			}
			if (EffectiveCargoStrategy == CargoCarryStrategy.PhysicalInfluence)
			{
				componentInParent?.PhysicsResolutionController?.SetPhysicsResolutionInContainer();
				return;
			}
			EnsureRig().IncludeContentLayers(ContentLayerMask(componentsInChildren));
			if (IsActivelyGrabbed(componentInParent) || IsWornHeadwear(_tracked[body]))
			{
				SetBodyInteractsWithRealCarrier(_tracked[body]);
			}
			else
			{
				SetContentCarrierDecoupled(componentsInChildren, decoupled: true);
			}
		}

		private void HandleBodyExited(Rigidbody body)
		{
			if (_tracked.TryGetValue(body, out var value))
			{
				if (WithinExitPadding(body))
				{
					ClearExitVeto(body);
					return;
				}
				if (value.IsPartBone && body != null && PartSeamVisible(value.PartRoot))
				{
					NoteExitVeto(body, "part-seam");
					return;
				}
				bool flag = ExitVetoExhausted(body, value);
				if (!flag && body != null && !IsActivelyGrabbed(value.Grabable) && !IsWornHeadwear(value) && HasActiveSolidCollider(value.IgnoredColliders) && IsMidHardConstraintCrossing(body))
				{
					NoteExitVeto(body, "hard-constraint");
					return;
				}
				if (!flag && body != null && !IsTipped && !IsActivelyGrabbed(value.Grabable) && !IsWornHeadwear(value) && HasActiveSolidCollider(value.IgnoredColliders) && _region != null && _region.ContainsWithFloorSkin(body.worldCenterOfMass, 0.35f))
				{
					NoteExitVeto(body, "floor-skin");
					return;
				}
				if (!flag && ShouldRetainProxyGluedCargo(body, value))
				{
					NoteExitVeto(body, "proxy-glue");
					return;
				}
				_exitVetoStreak.Remove(body);
				_exitVetoSince.Remove(body);
				_seatRowAbsentSince.Remove(body);
				_tracked.Remove(body);
				InvalidateSeatScanIndexes();
				DropCargoSeatWithGrace(value);
				Release(value);
				if (body == null || _region == null || !_region.SeamSees(body))
				{
					SetContentCarrierDecoupled(value.IgnoredColliders, decoupled: false);
				}
				else
				{
					_seamDecoupled[body] = value.IgnoredColliders;
				}
				OfferReleasedCargo(body);
			}
			else
			{
				if (!_deferredCargo.TryGetValue(body, out var value2))
				{
					return;
				}
				_deferredCargo.Remove(body);
				if (value2 != null)
				{
					if (body == null || _region == null || !_region.SeamSees(body))
					{
						SetContentCarrierDecoupled(value2, decoupled: false);
					}
					else
					{
						_seamDecoupled[body] = value2;
					}
				}
			}
		}

		private bool ShouldRetainProxyGluedCargo(Rigidbody body, TrackedBody tracked)
		{
			if (!IsNetworked || base.Object.HasStateAuthority || EffectiveCargoStrategy != CargoCarryStrategy.PhysicalInfluence)
			{
				return false;
			}
			if (body == null || !body.isKinematic || tracked.BoneSynchronizer != null)
			{
				return false;
			}
			if (IsTipped || IsActivelyGrabbed(tracked.Grabable) || IsWornHeadwear(tracked))
			{
				return false;
			}
			return _cargoRenderSeats.ContainsKey(body);
		}

		private void DropCargoSeatWithGrace(TrackedBody tracked, bool keepStamping = true)
		{
			Rigidbody body = tracked.Body;
			if (!(body == null))
			{
				_cargoSeatGrace.Remove(body);
				_cargoRenderSeats.Remove(body);
			}
		}

		private void RegisterRider(Rigidbody body, PlayerCharacterMovableBase player)
		{
			_bodyToPlayer[body] = player;
			if ((player.Rigidbody != null && body != player.Rigidbody) || IsCarryExcluded(player) || player.RefusedCarrier == base.transform)
			{
				return;
			}
			_riderRefs.TryGetValue(player, out var value);
			_riderRefs[player] = value + 1;
			if (value <= 0 && !IsCarrierRestingOnPlayer(player))
			{
				DecouplePlayer(player);
				RagdollEntity ragdoll = GetRagdoll(player);
				if (ragdoll != null && ragdoll.IsSimulated)
				{
					_ragdollCarried.Add(player);
				}
				else
				{
					AttachRider(body, player);
				}
			}
		}

		public bool IsRiddenByPlayer(int playerId)
		{
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				if (key != null && key.Object != null && key.Object.StateAuthority.PlayerId == playerId)
				{
					return true;
				}
			}
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				if (item != null && item.Object != null && item.Object.StateAuthority.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
		}

		public bool HoldsRiderSeat(PlayerCharacterMovableBase player)
		{
			if (player != null)
			{
				if (!_riderLocks.ContainsKey(player))
				{
					return _ragdollCarried.Contains(player);
				}
				return true;
			}
			return false;
		}

		private bool RiderCededToInnerVolume(PlayerCharacterMovableBase player)
		{
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.IsInnerTo(this) && physicsInfluenceVolume.HoldsRiderSeat(player))
				{
					return true;
				}
			}
			return false;
		}

		private void AttachRider(Rigidbody fallbackBody, PlayerCharacterMovableBase player)
		{
			if (IsTipped || player.RefusedCarrier == base.transform || RiderCededToInnerVolume(player) || IsCarrierRestingOnPlayer(player))
			{
				return;
			}
			Rigidbody rigidbody = ((player.Rigidbody != null) ? player.Rigidbody : fallbackBody);
			if (!(rigidbody == null))
			{
				if (!_decoupledPlayers.ContainsKey(player))
				{
					DecouplePlayer(player);
				}
				VolumeRider volumeRider = rigidbody.GetComponent<VolumeRider>();
				if (volumeRider == null)
				{
					volumeRider = rigidbody.gameObject.AddComponent<VolumeRider>();
				}
				volumeRider.SetCarrier(base.transform, this, _riderLinearTransfer, _riderAngularTransfer, player);
				_riderLocks[player] = new RiderLock
				{
					Body = rigidbody,
					NetworkObject = player.Object
				};
				if (IsNetworked)
				{
					player.SetPlatformCarrier(base.transform, base.Object.Id);
				}
			}
		}

		private void UnregisterRider(PlayerCharacterMovableBase player)
		{
			if (!_riderRefs.TryGetValue(player, out var value))
			{
				return;
			}
			value--;
			if (value > 0)
			{
				_riderRefs[player] = value;
				return;
			}
			_riderRefs.Remove(player);
			if (!_ragdollCarried.Contains(player))
			{
				_playerRagdolls.Remove(player);
				_retainedRagdollSeat.Remove(player);
				RecouplePlayer(player);
				ClearRiderOn(player);
			}
		}

		private void ClearRiderOn(PlayerCharacterMovableBase player)
		{
			if (!(player == null))
			{
				_restingOnPlayerSeconds.Remove(player);
				_riderLocks.Remove(player);
				player.ClearPlatformCarrier(base.transform);
				Rigidbody rigidbody = player.Rigidbody;
				VolumeRider volumeRider = ((rigidbody != null) ? rigidbody.GetComponent<VolumeRider>() : null);
				if (volumeRider != null)
				{
					volumeRider.ClearCarrier(base.transform);
				}
			}
		}

		private void UpdateRagdollModes()
		{
			if (_riderRefs.Count == 0 && _ragdollCarried.Count == 0 && _bodyToPlayer.Count == 0)
			{
				return;
			}
			_ragdollModeScratch.Clear();
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				_ragdollModeScratch.Add(key);
			}
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				_ragdollModeScratch.Add(item);
			}
			foreach (PlayerCharacterMovableBase value in _bodyToPlayer.Values)
			{
				_ragdollModeScratch.Add(value);
			}
			foreach (PlayerCharacterMovableBase item2 in _ragdollModeScratch)
			{
				RagdollEntity ragdoll = GetRagdoll(item2);
				if (ragdoll == null)
				{
					continue;
				}
				bool flag = _ragdollCarried.Contains(item2);
				if (IsActivelyGrabbed(GetPlayerGrabable(item2)))
				{
					if (flag)
					{
						ReleaseRagdollCarry(item2);
					}
				}
				else if (ragdoll.IsSimulated && !flag)
				{
					if (IsCarryExcluded(item2) || item2.RefusedCarrier == base.transform || RiderCededToInnerVolume(item2))
					{
						continue;
					}
					if (item2.Object != null && item2.Object.HasInputAuthority && item2.PlatformCarrierId == base.Object.Id)
					{
						_retainedRagdollSeat[item2] = item2.PlatformCarrierLocalPosition;
					}
					else if (item2.Object != null && item2.Object.HasInputAuthority && !_retainedRagdollSeat.ContainsKey(item2))
					{
						Rigidbody rigidbody = ((ragdoll.RootPhysData.RigidBody != null) ? ragdoll.RootPhysData.RigidBody : item2.Rigidbody);
						if (rigidbody != null)
						{
							GetCarrierRenderPose(out var position, out var rotation);
							Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, base.transform.lossyScale);
							_retainedRagdollSeat[item2] = matrix4x.inverse.MultiplyPoint3x4(rigidbody.position);
						}
					}
					_ragdollCarried.Add(item2);
					ClearRiderOn(item2);
					if (IsNetworked && _retainedRagdollSeat.ContainsKey(item2))
					{
						item2.SetPlatformCarrier(base.transform, base.Object.Id);
					}
					RefreshPlayerDecoupling(item2);
				}
				else if (!ragdoll.IsSimulated && flag)
				{
					_ragdollCarried.Remove(item2);
					_ragdollFloorTailUntil.Remove(item2);
					if (!IsPlayerPresentInRegion(item2))
					{
						ReleaseRiderCompletely(item2);
						continue;
					}
					AttachRider(item2.Rigidbody, item2);
					RestoreRetainedSeat(item2);
				}
			}
		}

		private void RestoreRetainedSeat(PlayerCharacterMovableBase player)
		{
			if (!_retainedRagdollSeat.TryGetValue(player, out var value))
			{
				return;
			}
			_retainedRagdollSeat.Remove(player);
			if (!(player.Object == null) && player.Object.HasInputAuthority && !(player.Rigidbody == null))
			{
				GetCarrierRenderPose(out var position, out var rotation);
				Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, base.transform.lossyScale);
				Vector3 vector = matrix4x.MultiplyPoint3x4(value);
				vector.y = Mathf.Max(vector.y, MinSeatWorldY(player));
				value = matrix4x.inverse.MultiplyPoint3x4(vector);
				RagdollEntity ragdoll = GetRagdoll(player);
				if (ragdoll != null)
				{
					ragdoll.Teleport(vector);
				}
				player.Rigidbody.position = vector;
				player.StagePlatformAnchor(value);
			}
		}

		private float MinSeatWorldY(PlayerCharacterMovableBase player)
		{
			if (_region == null || player.Rigidbody == null)
			{
				return float.NegativeInfinity;
			}
			CapsuleCollider bodyCollider = player.BodyCollider;
			float b = ((bodyCollider != null) ? (player.Rigidbody.position.y - bodyCollider.bounds.min.y) : 0f);
			return _region.FloorWorldY + Mathf.Max(0f, b);
		}

		private void CarryRagdolls()
		{
			if (IsTipped || _ragdollCarried.Count == 0 || _carrierFixedDelta == Vector3.zero || IsCarrierSimulatedHere)
			{
				return;
			}
			foreach (KeyValuePair<Rigidbody, PlayerCharacterMovableBase> item in _bodyToPlayer)
			{
				if (!_ragdollCarried.Contains(item.Value))
				{
					continue;
				}
				Rigidbody key = item.Key;
				if (!(key == null) && !key.isKinematic)
				{
					RagdollEntity ragdoll = GetRagdoll(item.Value);
					if (!(ragdoll == null) && ragdoll.HasStateAuthority)
					{
						key.position += _carrierFixedDelta;
					}
				}
			}
		}

		private void PinCarriedRagdollRootsToSeat()
		{
			if (IsTipped || _retainedRagdollSeat.Count == 0 || IsCarrierSimulatedHere)
			{
				return;
			}
			GetCarrierRenderPose(out var position, out var rotation);
			Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, base.transform.lossyScale);
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				if (!_retainedRagdollSeat.TryGetValue(item, out var value))
				{
					continue;
				}
				RagdollEntity ragdoll = GetRagdoll(item);
				if (ragdoll == null || !ragdoll.HasStateAuthority)
				{
					continue;
				}
				Vector3 position2 = matrix4x.MultiplyPoint3x4(value);
				Rigidbody rigidbody = item.Rigidbody;
				if (rigidbody != null)
				{
					rigidbody.position = position2;
					rigidbody.linearVelocity = Vector3.zero;
					rigidbody.PublishTransform();
					item.StagePlatformAnchor(value);
				}
				Rigidbody rigidBody = ragdoll.RootPhysData.RigidBody;
				if (rigidBody != null)
				{
					Vector3 position3 = rigidBody.position;
					rigidBody.position = new Vector3(position2.x, position3.y, position2.z);
					if (!rigidBody.isKinematic)
					{
						rigidBody.linearVelocity = new Vector3(0f, rigidBody.linearVelocity.y, 0f);
					}
				}
			}
		}

		private void ApplyCarriedRagdollPoses()
		{
			if (_retainedRagdollSeat.Count == 0 || IsCarrierSimulatedHere)
			{
				return;
			}
			GetCarrierRenderPose(out var position, out var rotation);
			Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, base.transform.lossyScale);
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				if (!_retainedRagdollSeat.TryGetValue(item, out var value))
				{
					continue;
				}
				RagdollEntity ragdoll = GetRagdoll(item);
				if (!(ragdoll == null) && ragdoll.HasStateAuthority)
				{
					Vector3 position2 = matrix4x.MultiplyPoint3x4(value);
					Rigidbody rigidbody = item.Rigidbody;
					if (rigidbody != null)
					{
						rigidbody.transform.position = position2;
					}
					Rigidbody rigidBody = ragdoll.RootPhysData.RigidBody;
					if (rigidBody != null)
					{
						Vector3 position3 = rigidBody.transform.position;
						rigidBody.transform.position = new Vector3(position2.x, position3.y, position2.z);
					}
				}
			}
		}

		private void ApplyObserverCarriedRagdollPoses()
		{
			if (!IsNetworked || (_ragdollCarried.Count == 0 && _observerRagdollGlueUntil.Count == 0))
			{
				return;
			}
			GetCarrierRenderPose(out var position, out var rotation);
			Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, base.transform.lossyScale);
			bool flag = _observerGlueLastCarrierFrame != Time.frameCount - 1 || (position - _observerGlueLastCarrierPosition).sqrMagnitude > 6.3999994E-07f || Quaternion.Angle(rotation, _observerGlueLastCarrierRotation) > 0.01f;
			_observerGlueLastCarrierPosition = position;
			_observerGlueLastCarrierRotation = rotation;
			_observerGlueLastCarrierFrame = Time.frameCount;
			_observerRagdollCorrections.Clear();
			_observerRagdollScratch.Clear();
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				_observerRagdollScratch.Add(item);
			}
			foreach (PlayerCharacterMovableBase key in _observerRagdollGlueUntil.Keys)
			{
				_observerRagdollScratch.Add(key);
			}
			foreach (PlayerCharacterMovableBase item2 in _observerRagdollScratch)
			{
				if (item2 == null || item2.Object == null || item2.Object.HasInputAuthority)
				{
					_observerRagdollGlueUntil.Remove(item2);
					_observerRagdollSeatOffset.Remove(item2);
					continue;
				}
				RagdollEntity ragdoll = GetRagdoll(item2);
				if (ragdoll == null || ragdoll.RootPhysData.RigidBody == null)
				{
					_observerRagdollGlueUntil.Remove(item2);
					_observerRagdollSeatOffset.Remove(item2);
				}
				else
				{
					if (ragdoll.HasStateAuthority)
					{
						continue;
					}
					float value;
					if (ragdoll.IsSimulated || ragdoll.IsBlendingOut)
					{
						_observerRagdollGlueUntil[item2] = Time.time + 0.15f;
					}
					else if (!_observerRagdollGlueUntil.TryGetValue(item2, out value) || Time.time >= value)
					{
						_observerRagdollGlueUntil.Remove(item2);
						_observerRagdollSeatOffset.Remove(item2);
						continue;
					}
					if (item2.PlatformCarrierId == base.Object.Id)
					{
						_observerRagdollSeatCache[item2] = item2.PlatformCarrierLocalPosition;
					}
					if (!_observerRagdollSeatCache.TryGetValue(item2, out var value2))
					{
						continue;
					}
					Rigidbody rigidBody = ragdoll.RootPhysData.RigidBody;
					Vector3 vector = matrix4x.inverse.MultiplyPoint3x4(rigidBody.position) - value2;
					if (!flag)
					{
						_observerRagdollSeatOffset[item2] = vector;
						continue;
					}
					if (!_observerRagdollSeatOffset.TryGetValue(item2, out var value3))
					{
						value3 = vector;
						_observerRagdollSeatOffset[item2] = value3;
					}
					else if ((vector - value3).sqrMagnitude > 0.040000003f)
					{
						value3 = Vector3.MoveTowards(value3, vector, 1f * Time.deltaTime);
						_observerRagdollSeatOffset[item2] = value3;
					}
					Vector3 vector2 = matrix4x.MultiplyPoint3x4(value2 + value3);
					rigidBody.transform.position = vector2;
					Vector3 position2 = rigidBody.position;
					if (!ragdoll.IsBlendingOut)
					{
						_observerRagdollCorrections[item2] = vector2 - position2;
					}
				}
			}
			if (_observerRagdollCorrections.Count == 0)
			{
				return;
			}
			float cargoFloorWorldY = CargoFloorWorldY;
			foreach (KeyValuePair<PlayerCharacterMovableBase, Vector3> observerRagdollCorrection in _observerRagdollCorrections)
			{
				RagdollEntity ragdoll2 = GetRagdoll(observerRagdollCorrection.Key);
				if (ragdoll2 == null)
				{
					continue;
				}
				Rigidbody rigidBody2 = ragdoll2.RootPhysData.RigidBody;
				IReadOnlyList<Rigidbody> bones = ragdoll2.Bones;
				for (int i = 0; i < bones.Count; i++)
				{
					Rigidbody rigidbody = bones[i];
					if (rigidbody == null || rigidbody == rigidBody2)
					{
						continue;
					}
					Vector3 vector3 = rigidbody.position + observerRagdollCorrection.Value;
					if (_region != null && _region.ContainsXZ(vector3, 0.25f))
					{
						if (!_boneBottomOffsets.TryGetValue(rigidbody, out var value4))
						{
							value4 = BoneBottomOffset(rigidbody);
						}
						if (vector3.y - value4 < cargoFloorWorldY)
						{
							vector3.y = cargoFloorWorldY + value4;
						}
					}
					rigidbody.transform.position = vector3;
				}
			}
		}

		private void ApplyRiderLockPhysics()
		{
			if (!IsNetworked)
			{
				return;
			}
			RefreshFromOuterCarrier();
			foreach (KeyValuePair<PlayerCharacterMovableBase, RiderLock> riderLock in _riderLocks)
			{
				PlayerCharacterMovableBase key = riderLock.Key;
				RiderLock value = riderLock.Value;
				if (!(key == null) && !(value.Body == null) && !IsRiderControlledHere(value.NetworkObject) && !(key.PlatformCarrierId != base.Object.Id))
				{
					value.Body.position = base.transform.TransformPoint(key.PlatformCarrierLocalPosition);
				}
			}
		}

		private void ApplyRiderLockPoses()
		{
			if (!IsNetworked)
			{
				return;
			}
			RefreshFromOuterCarrier();
			foreach (KeyValuePair<PlayerCharacterMovableBase, RiderLock> riderLock in _riderLocks)
			{
				PlayerCharacterMovableBase key = riderLock.Key;
				RiderLock value = riderLock.Value;
				if (key == null || value.Body == null || IsRiderControlledHere(value.NetworkObject))
				{
					value.HasRenderOffset = false;
					continue;
				}
				if (key.PlatformCarrierId != base.Object.Id)
				{
					value.HasRenderOffset = false;
					continue;
				}
				Vector3 platformCarrierLocalPosition = key.PlatformCarrierLocalPosition;
				if (!value.HasRenderOffset || (platformCarrierLocalPosition - value.RenderLocalOffset).sqrMagnitude > 0.25f)
				{
					value.RenderLocalOffset = platformCarrierLocalPosition;
					value.HasRenderOffset = true;
				}
				else
				{
					value.RenderLocalOffset = Vector3.MoveTowards(value.RenderLocalOffset, platformCarrierLocalPosition, 0.07f);
				}
				value.Body.transform.position = base.transform.TransformPoint(value.RenderLocalOffset);
			}
		}

		private static bool IsRiderControlledHere(NetworkObject networkObject)
		{
			if (!(networkObject == null))
			{
				return networkObject.HasInputAuthority;
			}
			return true;
		}

		private void TraceVelocityChange()
		{
			if (!_ejectOnVelocityChange)
			{
				return;
			}
			if (_velocityTracerWarmup < 10)
			{
				_velocityTracerWarmup++;
			}
			else if ((!IsNetworked || base.Object.HasStateAuthority) && !(Time.time - _lastEjectionTime < _ejectionCooldownSeconds) && !((_carrierVelocity - _previousCarrierVelocity).magnitude < _ejectionVelocityChange))
			{
				if (IsNetworked)
				{
					EjectRPC(_previousCarrierVelocity);
				}
				else
				{
					Eject(_previousCarrierVelocity);
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3510912813u)]
		private void EjectRPC([RpcPayload(12)] Vector3 escapeVelocity)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3510912813u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PhysicsVolumeModule.Scripts.PhysicsInfluenceVolume::EjectRPC(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(escapeVelocity, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Eject(escapeVelocity);
		}

		private void Eject(Vector3 escapeVelocity)
		{
			_lastEjectionTime = Time.time;
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!(value.Body == null) && HasLocalControl(value) && !IsActivelyGrabbed(value.Grabable) && !IsWornHeadwear(value) && !value.IsCeded)
				{
					bool isResting = value.IsResting;
					if (value.IsResting)
					{
						Unlock(value);
					}
					value.RestTimer = 0f;
					if (!value.Body.isKinematic && (isResting || !value.HadFrictionSupport))
					{
						value.Body.linearVelocity += escapeVelocity;
					}
				}
			}
			foreach (KeyValuePair<PlayerCharacterMovableBase, int> riderRef in _riderRefs)
			{
				if (!IsCarrierGrabber(riderRef.Key))
				{
					EjectRider(riderRef.Key, escapeVelocity, RagdollSimulationReasonEnum.Stun, _ejectionRagdollRecoverySeconds, (_lifetimeCts != null) ? _lifetimeCts.Token : CancellationToken.None).Forget();
				}
			}
		}

		public void SlamRiders(float damage, float ragdollRecoverySeconds)
		{
			if (IsNetworked)
			{
				SlamRidersRPC(damage, ragdollRecoverySeconds);
			}
			else
			{
				ApplySlam(damage, ragdollRecoverySeconds);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3629481157u)]
		private void SlamRidersRPC([RpcPayload(4)] float damage, [RpcPayload(4)] float ragdollRecoverySeconds)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3629481157u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PhysicsVolumeModule.Scripts.PhysicsInfluenceVolume::SlamRidersRPC(System.Single,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(damage, 4);
						writer.Write(ragdollRecoverySeconds, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplySlam(damage, ragdollRecoverySeconds);
		}

		private void ApplySlam(float damage, float ragdollRecoverySeconds)
		{
			foreach (KeyValuePair<PlayerCharacterMovableBase, int> riderRef in _riderRefs)
			{
				if (!IsCarrierGrabber(riderRef.Key))
				{
					EjectRider(riderRef.Key, Vector3.zero, RagdollSimulationReasonEnum.Stun, ragdollRecoverySeconds, (_lifetimeCts != null) ? _lifetimeCts.Token : CancellationToken.None, damage).Forget();
				}
			}
		}

		private void UpdateTipCustody()
		{
			bool isTipped = IsTipped;
			if (isTipped == _wasTipped)
			{
				return;
			}
			_wasTipped = isTipped;
			if (isTipped || _region == null)
			{
				return;
			}
			_tipReclaimScratch.Clear();
			_tipReclaimScratch.AddRange(_deferredCargo.Keys);
			for (int i = 0; i < _tipReclaimScratch.Count; i++)
			{
				Rigidbody rigidbody = _tipReclaimScratch[i];
				if (!(rigidbody == null) && !_tracked.ContainsKey(rigidbody) && _region.ContainsWithFloorSkin(rigidbody.worldCenterOfMass, 0f))
				{
					HandleBodyEntered(rigidbody);
				}
			}
			_tipReclaimScratch.Clear();
		}

		private void ApplyTipRelease()
		{
			if (IsTipped)
			{
				if (_isWearableHeadwear)
				{
					return;
				}
				_tipScratch.Clear();
				_tipScratch.AddRange(_riderRefs.Keys);
				{
					foreach (PlayerCharacterMovableBase item in _tipScratch)
					{
						if (!IsCarrierGrabber(item) && _tipRagdolled.Add(item))
						{
							RagdollEntity ragdoll = GetRagdoll(item);
							if (ragdoll != null && ragdoll.HasStateAuthority)
							{
								ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.PlatformTipped);
							}
						}
					}
					return;
				}
			}
			if (_tipRagdolled.Count == 0)
			{
				return;
			}
			_tipScratch.Clear();
			_tipScratch.AddRange(_tipRagdolled);
			_tipRagdolled.Clear();
			foreach (PlayerCharacterMovableBase item2 in _tipScratch)
			{
				ReleaseTipRagdoll(item2, (_lifetimeCts != null) ? _lifetimeCts.Token : CancellationToken.None).Forget();
			}
		}

		private async UniTaskVoid ReleaseTipRagdoll(PlayerCharacterMovableBase player, CancellationToken token)
		{
			RagdollEntity ragdoll = GetRagdoll(player);
			if (!(ragdoll == null) && ragdoll.HasStateAuthority && !(await UniTask.Delay(TimeSpan.FromSeconds(_tipRagdollRecoverySeconds), DelayType.DeltaTime, PlayerLoopTiming.Update, token).SuppressCancellationThrow()) && !(ragdoll == null) && !IsTipped)
			{
				ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.PlatformTipped);
			}
		}

		private bool IsCarrierGrabber(PlayerCharacterMovableBase player)
		{
			if (_carrierGrabable == null || player == null || player.Object == null)
			{
				return false;
			}
			return _carrierGrabable.GrabbedByPlayers.Contains(player.Object.StateAuthority.PlayerId);
		}

		public bool IsPassengerGrabber(int playerId)
		{
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				if (key != null && key.Object != null && key.Object.StateAuthority.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
		}

		private async UniTaskVoid EjectRider(PlayerCharacterMovableBase player, Vector3 escapeVelocity, RagdollSimulationReasonEnum reason, float recoverySeconds, CancellationToken token, float damage = 0f)
		{
			RagdollEntity ragdoll = GetRagdoll(player);
			if (ragdoll == null || !ragdoll.HasStateAuthority)
			{
				return;
			}
			if (damage > 0f)
			{
				player.GetComponentInChildren<PlayerDamageable>(includeInactive: true)?.DamageRPC(damage);
			}
			bool forced = false;
			if (!ragdoll.IsSimulated)
			{
				ragdoll.AddSimulationReason(reason);
				forced = true;
			}
			float deadline = Time.time + 0.5f;
			while (!ragdoll.IsSimulated && Time.time < deadline)
			{
				if (await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token).SuppressCancellationThrow())
				{
					return;
				}
			}
			if (ragdoll.IsSimulated && escapeVelocity != Vector3.zero)
			{
				Rigidbody[] componentsInChildren = ragdoll.GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody rigidbody in componentsInChildren)
				{
					if (!rigidbody.isKinematic)
					{
						rigidbody.linearVelocity += escapeVelocity;
					}
				}
			}
			if (forced && !(await UniTask.Delay(TimeSpan.FromSeconds(recoverySeconds), DelayType.DeltaTime, PlayerLoopTiming.Update, token).SuppressCancellationThrow()) && !(ragdoll == null))
			{
				ragdoll.RemoveSimulationReason(reason);
			}
		}

		private bool IsCarryExcluded(PlayerCharacterMovableBase player)
		{
			if (player != null && _carryExcludedUntil.TryGetValue(player, out var value))
			{
				return Time.time < value;
			}
			return false;
		}

		private void ExpireCarryExclusions()
		{
			if (_carryExcludedUntil.Count == 0)
			{
				return;
			}
			_carryExcludeScratch.Clear();
			foreach (KeyValuePair<PlayerCharacterMovableBase, float> item in _carryExcludedUntil)
			{
				if (item.Key == null || Time.time >= item.Value)
				{
					_carryExcludeScratch.Add(item.Key);
				}
			}
			foreach (PlayerCharacterMovableBase item2 in _carryExcludeScratch)
			{
				_carryExcludedUntil.Remove(item2);
			}
		}

		public bool IsCarryingRagdoll(PlayerCharacterMovableBase player)
		{
			if (player != null)
			{
				return _ragdollCarried.Contains(player);
			}
			return false;
		}

		public void ExcludeFromCarry(PlayerCharacterMovableBase player, float seconds)
		{
			if (!(player == null))
			{
				ExpireCarryExclusions();
				_carryExcludedUntil[player] = Time.time + seconds;
				EvictFromCarry(player);
			}
		}

		private void EvictFromCarry(PlayerCharacterMovableBase player)
		{
			_ragdollCarried.Remove(player);
			_ragdollFloorTailUntil.Remove(player);
			_retainedRagdollSeat.Remove(player);
			_riderRefs.Remove(player);
			RecouplePlayer(player);
			ClearRiderOn(player);
		}

		private IPointGrabable GetPlayerGrabable(PlayerCharacterMovableBase player)
		{
			if (player == null)
			{
				return null;
			}
			if (_playerGrabables.TryGetValue(player, out var value))
			{
				return value;
			}
			IPointGrabable componentInChildren = player.GetComponentInChildren<IPointGrabable>(includeInactive: true);
			_playerGrabables[player] = componentInChildren;
			return componentInChildren;
		}

		private void ReleaseRagdollCarry(PlayerCharacterMovableBase player)
		{
			_observerRagdollGlueUntil.Remove(player);
			_observerRagdollSeatCache.Remove(player);
			_observerRagdollSeatOffset.Remove(player);
			EvictFromCarry(player);
		}

		private RagdollEntity GetRagdoll(PlayerCharacterMovableBase player)
		{
			if (player == null)
			{
				return null;
			}
			if (_playerRagdolls.TryGetValue(player, out var value))
			{
				return value;
			}
			RagdollEntity componentInChildren = player.GetComponentInChildren<RagdollEntity>(includeInactive: true);
			_playerRagdolls[player] = componentInChildren;
			return componentInChildren;
		}

		public static PhysicsInfluenceVolume FindDeclaredFloorUnder(Vector3 worldPoint, float margin, float maxHeight)
		{
			PhysicsInfluenceVolume result = null;
			float num = float.PositiveInfinity;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if (!(physicsInfluenceVolume == null) && physicsInfluenceVolume.TryGetHeightAboveDeclaredFloor(worldPoint, out var height) && !(height < 0f - margin) && !(height > maxHeight) && !(height >= num))
				{
					result = physicsInfluenceVolume;
					num = height;
				}
			}
			return result;
		}

		public bool TryGetHeightAboveDeclaredFloor(Vector3 worldPoint, out float height)
		{
			height = 0f;
			if (_floorPlane == null)
			{
				return false;
			}
			GetFloorCoordinates(worldPoint, out var alongRight, out var alongForward, out var aboveFace);
			if (Mathf.Abs(alongRight) > _floorHalfExtents.x || Mathf.Abs(alongForward) > _floorHalfExtents.y)
			{
				return false;
			}
			height = aboveFace;
			return true;
		}

		public bool IsOnDeclaredFloor(Vector3 worldPoint, float margin)
		{
			if (TryGetHeightAboveDeclaredFloor(worldPoint, out var height))
			{
				return height >= 0f - margin;
			}
			return false;
		}

		public bool IsOffDeclaredFloor(Vector3 worldPoint, float margin)
		{
			if (_floorPlane == null)
			{
				return false;
			}
			return !IsOnDeclaredFloor(worldPoint, margin);
		}

		public bool TryGetDeclaredFloorPoint(Vector3 worldPoint, out Vector3 floorPoint)
		{
			floorPoint = worldPoint;
			if (_floorPlane == null)
			{
				return false;
			}
			GetFloorCoordinates(worldPoint, out var alongRight, out var alongForward, out var _);
			alongRight = Mathf.Clamp(alongRight, 0f - _floorHalfExtents.x, _floorHalfExtents.x);
			alongForward = Mathf.Clamp(alongForward, 0f - _floorHalfExtents.y, _floorHalfExtents.y);
			floorPoint = _floorPlane.position + _floorPlane.right * alongRight + _floorPlane.forward * alongForward;
			return true;
		}

		private void GetFloorCoordinates(Vector3 worldPoint, out float alongRight, out float alongForward, out float aboveFace)
		{
			Vector3 lhs = worldPoint - _floorPlane.position;
			alongRight = Vector3.Dot(lhs, _floorPlane.right);
			alongForward = Vector3.Dot(lhs, _floorPlane.forward);
			aboveFace = Vector3.Dot(lhs, _floorPlane.up);
		}

		public bool TryGetCarrierFloorY(Rigidbody body, out float floorY)
		{
			floorY = 0f;
			if (body == null)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(body.worldCenterOfMass, Vector3.down, _supportHits, 2.5f, -5, QueryTriggerInteraction.Ignore);
			Transform transform = ((_region != null) ? _region.CarrierRoot : base.transform);
			bool flag = false;
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < num; i++)
			{
				if (!(_supportHits[i].rigidbody == body))
				{
					Transform transform2 = _supportHits[i].collider.transform;
					if ((!(transform2 != transform) || transform2.IsChildOf(transform)) && (!flag || _supportHits[i].distance < raycastHit.distance))
					{
						raycastHit = _supportHits[i];
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			floorY = raycastHit.point.y;
			return true;
		}

		private bool IsInLockZone(TrackedBody tracked)
		{
			if (_lockZone == null)
			{
				return true;
			}
			Rigidbody body = tracked.Body;
			if (body == null)
			{
				return true;
			}
			Vector3 worldCenterOfMass = body.worldCenterOfMass;
			worldCenterOfMass.y = LowestSolidY(tracked.IgnoredColliders, body);
			return _lockZone.Contains(worldCenterOfMass);
		}

		private void CaptureOrHandOff(TrackedBody tracked)
		{
			if (!IsNetworked || base.Object.HasStateAuthority || tracked.NetworkObject == null || tracked.Grabable == null || tracked.HandOffAttempts >= 3)
			{
				Lock(tracked);
				return;
			}
			tracked.HandOffAttempts++;
			tracked.RestTimer = 0f;
			tracked.Grabable.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
		}

		private void TryCaptureRagdollPartAsWhole(TrackedBody trigger)
		{
			Transform partRoot = trigger.PartRoot;
			if (partRoot == null)
			{
				CaptureOrHandOff(trigger);
				return;
			}
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!(value.PartRoot != partRoot) && !(value.BoneSynchronizer == null) && !value.IsResting && (value.Body == null || value.Body.isKinematic || !HasLocalControl(value) || value.RestTimer < _restSeconds))
				{
					return;
				}
			}
			if (IsNetworked && !base.Object.HasStateAuthority && trigger.Grabable != null && trigger.HandOffAttempts < 3)
			{
				trigger.HandOffAttempts++;
				trigger.RestTimer = 0f;
				trigger.Grabable.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
				return;
			}
			foreach (TrackedBody value2 in _tracked.Values)
			{
				if (!(value2.PartRoot != partRoot) && !(value2.BoneSynchronizer == null) && !value2.IsResting)
				{
					Lock(value2);
				}
			}
		}

		public void StateAuthorityChanged()
		{
			if (!IsNetworked || !base.Object.HasStateAuthority)
			{
				return;
			}
			foreach (KeyValuePair<NetworkId, VolumeLockData> @lock in Locks)
			{
				if (base.Runner.TryFindObject(@lock.Key, out var networkObject) && !(networkObject == null) && !networkObject.HasStateAuthority)
				{
					IPointGrabable replicatedGrabable = GetReplicatedGrabable(networkObject);
					if (replicatedGrabable != null && !IsActivelyGrabbed(replicatedGrabable))
					{
						replicatedGrabable.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
					}
				}
			}
		}

		private void Lock(TrackedBody tracked)
		{
			tracked.LocalPosition = base.transform.InverseTransformPoint(tracked.Body.position);
			tracked.LocalRotation = Quaternion.Inverse(base.transform.rotation) * tracked.Body.rotation;
			LockAt(tracked);
		}

		private bool IsAtPublishedPose(TrackedBody tracked, VolumeLockData published)
		{
			Vector3 vector = base.transform.TransformPoint(published.LocalPosition);
			if ((tracked.Body.position - vector).sqrMagnitude > 0.0625f)
			{
				return false;
			}
			Quaternion b = base.transform.rotation * published.LocalRotation;
			return Quaternion.Angle(tracked.Body.rotation, b) <= 10f;
		}

		private void AdoptLock(TrackedBody tracked, VolumeLockData published)
		{
			tracked.LocalPosition = published.LocalPosition;
			tracked.LocalRotation = published.LocalRotation;
			LockAt(tracked);
		}

		private void LockAt(TrackedBody tracked)
		{
			tracked.IsResting = true;
			tracked.RestTimer = 0f;
			tracked.WasKinematic = tracked.Body.isKinematic;
			tracked.Body.isKinematic = true;
			RestoreRigCollision(tracked);
			SetContentCarrierDecoupled(tracked.IgnoredColliders, decoupled: true);
			if (tracked.BoneSynchronizer != null)
			{
				tracked.BoneSynchronizer.ForcePoseResync();
			}
			PublishLock(tracked);
		}

		private bool TryGetPublishedLock(TrackedBody tracked, out VolumeLockData published)
		{
			published = default(VolumeLockData);
			if (IsNetworked && tracked.NetworkObject != null)
			{
				return Locks.TryGet(tracked.NetworkObject.Id, out published);
			}
			return false;
		}

		private VirtualColliderRig EnsureRig()
		{
			if (_virtualColliderRig != null)
			{
				return _virtualColliderRig;
			}
			if (_carrierColliders == null || _carrierColliders.Length == 0)
			{
				_carrierColliders = GetComponentsInChildren<Collider>(includeInactive: true);
			}
			_virtualColliderRig = VirtualColliderRig.Create(base.transform, _carrierColliders, TakeVirtualShapeSources());
			return _virtualColliderRig;
		}

		private Collider[] TakeVirtualShapeSources()
		{
			if (_virtualColliderShapes == null || _virtualColliderShapes.Length == 0)
			{
				return null;
			}
			Collider[] virtualColliderShapes = _virtualColliderShapes;
			foreach (Collider collider in virtualColliderShapes)
			{
				if (collider != null)
				{
					collider.enabled = false;
				}
			}
			return _virtualColliderShapes;
		}

		private void SetBodyInteractsWithRealCarrier(TrackedBody tracked)
		{
			if (EffectiveCargoStrategy == CargoCarryStrategy.RestLock && !(_virtualColliderRig == null))
			{
				tracked.RigIgnored = true;
				SetContentCarrierDecoupled(tracked.IgnoredColliders, decoupled: false);
				_virtualColliderRig.SetContentIgnored(tracked.IgnoredColliders, ignored: true);
			}
		}

		private void RestoreRigCollision(TrackedBody tracked)
		{
			if (tracked.RigIgnored)
			{
				tracked.RigIgnored = false;
				_virtualColliderRig?.SetContentIgnored(tracked.IgnoredColliders, ignored: false);
			}
		}

		private void SweepKinematicNeighbors()
		{
			if (EffectiveCargoStrategy != CargoCarryStrategy.PhysicalInfluence || --_neighborSweepCountdown > 0)
			{
				return;
			}
			_neighborSweepCountdown = 30;
			if (!IsCarrierSimulatedHere)
			{
				RestoreAllKinematicNeighbors();
				return;
			}
			if (_carrierColliders == null || _carrierColliders.Length == 0)
			{
				_carrierColliders = GetComponentsInChildren<Collider>(includeInactive: true);
			}
			Bounds bounds = default(Bounds);
			bool flag = false;
			Collider[] carrierColliders = _carrierColliders;
			foreach (Collider collider in carrierColliders)
			{
				if (!(collider == null) && !collider.isTrigger && collider.enabled)
				{
					if (!flag)
					{
						bounds = collider.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(collider.bounds);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			bounds.Expand(1.2f);
			int num = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _neighborOverlapScratch, Quaternion.identity);
			for (int j = 0; j < num; j++)
			{
				Rigidbody rigidbody = ((_neighborOverlapScratch[j] != null) ? _neighborOverlapScratch[j].attachedRigidbody : null);
				if (rigidbody == null || rigidbody == _carrierBody || !rigidbody.isKinematic || _tracked.ContainsKey(rigidbody) || rigidbody.GetComponentInChildren<PhysicsInfluenceVolume>(includeInactive: true) != null || rigidbody.GetComponentInParent<PhysicsInfluenceVolume>() != null || rigidbody.GetComponentInParent<PlayerCharacterMovableBase>() != null || rigidbody.GetComponentInParent<IPointGrabable>() == null)
				{
					continue;
				}
				if (!_kinematicNeighbors.TryGetValue(rigidbody, out var value))
				{
					value = new KinematicNeighbor
					{
						Colliders = rigidbody.GetComponentsInChildren<Collider>(includeInactive: true)
					};
					_kinematicNeighbors.Add(rigidbody, value);
				}
				value.Sweeps++;
				if (value.Sweeps >= 3)
				{
					if (!value.Decoupled)
					{
						value.Decoupled = true;
					}
					SetContentCarrierDecoupled(value.Colliders, decoupled: true);
				}
			}
			_neighborRetireScratch.Clear();
			foreach (KeyValuePair<Rigidbody, KinematicNeighbor> kinematicNeighbor in _kinematicNeighbors)
			{
				if (kinematicNeighbor.Key == null)
				{
					_neighborRetireScratch.Add(kinematicNeighbor.Key);
				}
				else
				{
					if (kinematicNeighbor.Key.isKinematic && !_tracked.ContainsKey(kinematicNeighbor.Key) && bounds.Contains(kinematicNeighbor.Key.worldCenterOfMass))
					{
						continue;
					}
					if (_tracked.TryGetValue(kinematicNeighbor.Key, out var value2))
					{
						if (kinematicNeighbor.Value.Decoupled && !value2.ProxyCarrierDecoupled)
						{
							value2.ProxyCarrierDecoupled = true;
						}
						_neighborRetireScratch.Add(kinematicNeighbor.Key);
					}
					else
					{
						if (kinematicNeighbor.Value.Decoupled)
						{
							SetContentCarrierDecoupled(kinematicNeighbor.Value.Colliders, decoupled: false);
						}
						_neighborRetireScratch.Add(kinematicNeighbor.Key);
					}
				}
			}
			foreach (Rigidbody item in _neighborRetireScratch)
			{
				_kinematicNeighbors.Remove(item);
			}
		}

		private void RecoupleSimulatedNeighbors()
		{
			if (_kinematicNeighbors.Count == 0)
			{
				return;
			}
			_neighborRetireScratch.Clear();
			foreach (KeyValuePair<Rigidbody, KinematicNeighbor> kinematicNeighbor in _kinematicNeighbors)
			{
				if (!(kinematicNeighbor.Key != null) || !kinematicNeighbor.Key.isKinematic)
				{
					if (kinematicNeighbor.Key != null && kinematicNeighbor.Value.Decoupled)
					{
						SetContentCarrierDecoupled(kinematicNeighbor.Value.Colliders, decoupled: false);
					}
					_neighborRetireScratch.Add(kinematicNeighbor.Key);
				}
			}
			foreach (Rigidbody item in _neighborRetireScratch)
			{
				_kinematicNeighbors.Remove(item);
			}
		}

		private void RestoreAllKinematicNeighbors()
		{
			if (_kinematicNeighbors.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<Rigidbody, KinematicNeighbor> kinematicNeighbor in _kinematicNeighbors)
			{
				if (kinematicNeighbor.Key != null && kinematicNeighbor.Value.Decoupled)
				{
					SetContentCarrierDecoupled(kinematicNeighbor.Value.Colliders, decoupled: false);
				}
			}
			_kinematicNeighbors.Clear();
		}

		private void SetContentCarrierDecoupled(Collider[] contentColliders, bool decoupled)
		{
			if (contentColliders == null)
			{
				return;
			}
			if (_carrierColliders == null || _carrierColliders.Length == 0)
			{
				_carrierColliders = GetComponentsInChildren<Collider>(includeInactive: true);
			}
			Collider[] carrierColliders = _carrierColliders;
			foreach (Collider collider in carrierColliders)
			{
				if (collider == null || collider.isTrigger || !collider.enabled || !collider.gameObject.activeInHierarchy)
				{
					continue;
				}
				foreach (Collider collider2 in contentColliders)
				{
					if (collider2 != null && !collider2.isTrigger && collider2.enabled && collider2.gameObject.activeInHierarchy)
					{
						Physics.IgnoreCollision(collider2, collider, decoupled);
					}
				}
			}
		}

		private void DecouplePlayer(PlayerCharacterMovableBase player)
		{
			Collider[] componentsInChildren = player.GetComponentsInChildren<Collider>(includeInactive: true);
			_decoupledPlayers[player] = componentsInChildren;
			EnsureRig().IncludeContentLayers(ContentLayerMask(componentsInChildren));
			SetContentCarrierDecoupled(componentsInChildren, decoupled: true);
		}

		private void RecouplePlayer(PlayerCharacterMovableBase player)
		{
			if (_decoupledPlayers.TryGetValue(player, out var value))
			{
				SetContentCarrierDecoupled(value, decoupled: false);
				_decoupledPlayers.Remove(player);
			}
		}

		private void RefreshPlayerDecoupling(PlayerCharacterMovableBase player)
		{
			if (_decoupledPlayers.ContainsKey(player))
			{
				Collider[] componentsInChildren = player.GetComponentsInChildren<Collider>(includeInactive: true);
				_decoupledPlayers[player] = componentsInChildren;
				EnsureRig().IncludeContentLayers(ContentLayerMask(componentsInChildren));
				SetContentCarrierDecoupled(componentsInChildren, decoupled: true);
			}
		}

		private static int ContentLayerMask(Collider[] contentColliders)
		{
			int num = 0;
			foreach (Collider collider in contentColliders)
			{
				if (collider != null && !collider.isTrigger)
				{
					num |= 1 << collider.gameObject.layer;
				}
			}
			return num;
		}

		private void PublishLock(TrackedBody tracked)
		{
			if (IsNetworked && base.Object.HasStateAuthority && !(tracked.NetworkObject == null))
			{
				Locks.Set(tracked.NetworkObject.Id, new VolumeLockData
				{
					LocalPosition = tracked.LocalPosition,
					LocalRotation = tracked.LocalRotation
				});
				InvalidateSeatScanIndexes();
			}
		}

		private void UnpublishLock(TrackedBody tracked)
		{
			if (IsNetworked && base.Object.HasStateAuthority && !(tracked.NetworkObject == null))
			{
				Locks.Remove(tracked.NetworkObject.Id);
				InvalidateSeatScanIndexes();
			}
		}

		private static bool TryGetCartLock(TrackedBody tracked, out CartAngularLock cartLock)
		{
			if (!tracked.CartLockProbed)
			{
				tracked.CartLockProbed = true;
				if (tracked.Body != null)
				{
					tracked.Body.TryGetComponent<CartAngularLock>(out tracked.CartLock);
				}
			}
			cartLock = tracked.CartLock;
			return cartLock != null;
		}

		private bool UpdateCartSlideState(TrackedBody tracked)
		{
			if (DeckTiltDegrees > _cartSlideStartAngle)
			{
				tracked.CartSlideLevelTimer = 0f;
				return true;
			}
			if (!tracked.CartAngleUnlockedBySlide)
			{
				return false;
			}
			tracked.CartSlideLevelTimer += Time.fixedDeltaTime;
			if (tracked.CartSlideLevelTimer < _cartSlideRelockSeconds)
			{
				return true;
			}
			ReleaseCartSlideClaim(tracked);
			return false;
		}

		private void DriveCartSlide(TrackedBody tracked, CartAngularLock cartLock)
		{
			if (tracked.Body.isKinematic || !HasLocalControl(tracked))
			{
				return;
			}
			if (cartLock.IsAngleLocked)
			{
				cartLock.UnlockAngle();
				tracked.CartAngleUnlockedBySlide = true;
			}
			if (_cartSlideRealisticPhysics && !tracked.CartPhysicsOverridden)
			{
				tracked.CartPhysicsOverridden = true;
				tracked.CartOriginalLinearDamping = tracked.Body.linearDamping;
				tracked.CartOriginalAngularDamping = tracked.Body.angularDamping;
				tracked.CartOriginalMaxDepenetration = tracked.Body.maxDepenetrationVelocity;
				tracked.Body.linearDamping = _cartSlideLinearDamping;
				tracked.Body.angularDamping = _cartSlideAngularDamping;
				tracked.Body.maxDepenetrationVelocity = _cartSlideMaxDepenetrationVelocity;
				RestoreOriginalMass(tracked);
			}
			float deckTiltDegrees = DeckTiltDegrees;
			if (!(deckTiltDegrees <= _cartSlideStartAngle))
			{
				Vector3 vector = Vector3.ProjectOnPlane(Vector3.down, base.transform.up);
				if (!(vector.sqrMagnitude < 0.0001f))
				{
					float t = Mathf.InverseLerp(_cartSlideStartAngle, Mathf.Max(_cartSlideStartAngle + 1f, _cartSlideFullAngle), deckTiltDegrees);
					Vector3 vector2 = _carrierVelocity + vector.normalized * Mathf.Lerp(_cartSlideGentleSpeed, _cartSlideFullSpeed, t);
					Vector3 linearVelocity = tracked.Body.linearVelocity;
					tracked.Body.linearVelocity = Vector3.Lerp(linearVelocity, new Vector3(vector2.x, linearVelocity.y, vector2.z), _cartSlideAcceleration * Time.fixedDeltaTime);
				}
			}
		}

		private void ReleaseCartSlideClaim(TrackedBody tracked)
		{
			if (tracked.CartPhysicsOverridden)
			{
				tracked.CartPhysicsOverridden = false;
				if (tracked.Body != null)
				{
					tracked.Body.linearDamping = tracked.CartOriginalLinearDamping;
					tracked.Body.angularDamping = tracked.CartOriginalAngularDamping;
					tracked.Body.maxDepenetrationVelocity = tracked.CartOriginalMaxDepenetration;
				}
			}
			if (tracked.CartAngleUnlockedBySlide)
			{
				tracked.CartAngleUnlockedBySlide = false;
				tracked.CartSlideLevelTimer = 0f;
				if (tracked.CartLock != null && !tracked.CartLock.IsAngleLocked)
				{
					tracked.CartLock.LockAngle();
				}
			}
		}

		private void Unlock(TrackedBody tracked)
		{
			tracked.IsResting = false;
			tracked.RestTimer = 0f;
			tracked.Body.isKinematic = tracked.WasKinematic;
			UnpublishLock(tracked);
		}

		private static bool IsActivelyGrabbed(IPointGrabable grabable)
		{
			if (grabable != null)
			{
				if (grabable.GrabbedByPlayersCount <= 0)
				{
					return grabable.GrabbedBySomethingCount > 0;
				}
				return true;
			}
			return false;
		}

		private static bool IsSeatPublisher(TrackedBody tracked)
		{
			return !IsCartShell(tracked.Body);
		}

		private static bool IsCartShell(Rigidbody body)
		{
			if (_cartShellLayer < 0)
			{
				_cartShellLayer = LayerMask.NameToLayer("CartVirtualCollider");
			}
			if (body != null)
			{
				return body.gameObject.layer == _cartShellLayer;
			}
			return false;
		}

		private static bool IsWornHeadwear(TrackedBody tracked)
		{
			if (tracked.Headwear != null)
			{
				return tracked.Headwear.IsWorn;
			}
			return false;
		}

		private IPointGrabable GetReplicatedGrabable(NetworkObject networkObject)
		{
			if (!_replicatedGrabables.TryGetValue(networkObject, out var value))
			{
				value = networkObject.GetComponentInChildren<IPointGrabable>(includeInactive: true);
				_replicatedGrabables[networkObject] = value;
			}
			return value;
		}

		private void Release(TrackedBody tracked)
		{
			if (IsNetworked && base.Object.HasStateAuthority && tracked.NetworkObject != null)
			{
				CargoSeats.Remove(tracked.NetworkObject.Id);
			}
			RestoreRigCollision(tracked);
			ReleaseCartSlideClaim(tracked);
			if (tracked.Body == null)
			{
				return;
			}
			if (tracked.Body.TryGetComponent<CartGrabObject>(out var component))
			{
				component.ClearCarrierFloor();
			}
			if (EffectiveCargoStrategy == CargoCarryStrategy.PhysicalInfluence || _cargoStrategy == CargoCarryStrategy.PhysicalInfluence)
			{
				tracked.HasInfluenceSeat = false;
				tracked.WasRidingInfluence = false;
				if (HasLocalControl(tracked))
				{
					RestoreOriginalMass(tracked);
				}
				RestoreForcedProxyKinematic(tracked);
				tracked.ProxyCarrierDecoupled = false;
				tracked.Grabable?.PhysicsResolutionController?.RestoreDefaultResolution();
				if (tracked.Grabable != null && !IsCargoDespawned(tracked))
				{
					tracked.Grabable.IsAuthorityRequested = false;
				}
			}
			if (IsActivelyGrabbed(tracked.Grabable) || IsWornHeadwear(tracked))
			{
				if (tracked.IsResting)
				{
					Unlock(tracked);
				}
			}
			else if (tracked.IsResting)
			{
				UnpublishLock(tracked);
				tracked.Body.isKinematic = OutsideVolumeKinematic(tracked);
				if (!tracked.Body.isKinematic)
				{
					tracked.Body.linearVelocity = EffectiveCarrierVelocity;
				}
			}
			else if (!tracked.Body.isKinematic && HasLocalControl(tracked) && tracked.IsSupported && !tracked.HadFrictionSupport && !tracked.IsCeded && (_region == null || !_region.SeamSees(tracked.Body)))
			{
				tracked.Body.linearVelocity += EffectiveCarrierVelocity;
			}
		}

		private void ReleaseAll()
		{
			List<Rigidbody> list = new List<Rigidbody>(_tracked.Keys);
			foreach (TrackedBody value in _tracked.Values)
			{
				Release(value);
				SetContentCarrierDecoupled(value.IgnoredColliders, decoupled: false);
			}
			_tracked.Clear();
			InvalidateSeatScanIndexes();
			foreach (Rigidbody item in list)
			{
				OfferReleasedCargo(item);
			}
			foreach (Collider[] value2 in _seamDecoupled.Values)
			{
				SetContentCarrierDecoupled(value2, decoupled: false);
			}
			_seamDecoupled.Clear();
			foreach (Collider[] value3 in _deferredCargo.Values)
			{
				if (value3 != null)
				{
					SetContentCarrierDecoupled(value3, decoupled: false);
				}
			}
			_deferredCargo.Clear();
			_cargoRenderSeats.Clear();
			foreach (PlayerCharacterMovableBase key in _riderRefs.Keys)
			{
				if (!_ragdollCarried.Contains(key))
				{
					ClearRiderOn(key);
				}
			}
			if (_decoupledPlayers.Count > 0)
			{
				foreach (PlayerCharacterMovableBase item2 in new List<PlayerCharacterMovableBase>(_decoupledPlayers.Keys))
				{
					RecouplePlayer(item2);
				}
			}
			_riderRefs.Clear();
			_bodyToPlayer.Clear();
			_playerRagdolls.Clear();
			_ragdollCarried.Clear();
			_ragdollFloorTailUntil.Clear();
			_observerRagdollSeatCache.Clear();
			_observerRagdollGlueUntil.Clear();
			_observerRagdollSeatOffset.Clear();
			_replicatedGrabables.Clear();
			_riderLocks.Clear();
			_retainedRagdollSeat.Clear();
			_carryExcludedUntil.Clear();
			_restingOnPlayerSeconds.Clear();
			_seatRenderStates.Clear();
			_seatReleaseBridges.Clear();
			_seatInterpBuffers.Clear();
			_authorityHandoffs.Clear();
			_ownedSeatPoses.Clear();
			_lastSeenSeatAuthority.Clear();
			foreach (PlayerCharacterMovableBase item3 in _tipRagdolled)
			{
				RagdollEntity ragdoll = GetRagdoll(item3);
				if (ragdoll != null && ragdoll.HasStateAuthority)
				{
					ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.PlatformTipped);
				}
			}
			_tipRagdolled.Clear();
		}

		private void ApplyGrabStampPins()
		{
			bool flag = IsLocalHaulOfCarrier();
			if (flag && !_grabPinActive)
			{
				CaptureGrabStamps();
			}
			else if (!flag && _grabPinActive)
			{
				ReleaseGrabStampPins();
			}
			_grabPinActive = flag;
			EaseReleasedCargoIntoSimulation();
			if (!flag)
			{
				return;
			}
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!value.HasGrabStamp || value.Body == null || value.NetworkObject == null)
				{
					continue;
				}
				if (value.NetworkObject.HasStateAuthority)
				{
					HandBackToSimulation(value);
				}
				else
				{
					if (IsActivelyGrabbed(value.Grabable))
					{
						continue;
					}
					if (IsSeatDrawnThisFrame(value))
					{
						value.IsPinnedToGrabStamp = false;
						continue;
					}
					if (!value.IsPinnedToGrabStamp)
					{
						value.IsPinnedToGrabStamp = true;
					}
					if (!value.Body.isKinematic)
					{
						value.Body.isKinematic = true;
					}
					else
					{
						value.Body.linearVelocity = Vector3.zero;
						value.Body.angularVelocity = Vector3.zero;
					}
					GetGrabStampBasis(out var basisPosition, out var basisRotation);
					Vector3 position = basisPosition + basisRotation * value.GrabStampLocalPosition;
					value.Body.position = position;
					value.Body.rotation = basisRotation * value.GrabStampLocalRotation;
					Vector3 position2 = base.transform.position + base.transform.rotation * value.GrabStampRenderLocalPosition;
					Quaternion rotation = base.transform.rotation * value.GrabStampRenderLocalRotation;
					value.Body.transform.SetPositionAndRotation(position2, rotation);
				}
			}
		}

		private void HandBackToSimulation(TrackedBody tracked)
		{
			if (tracked.Body == null)
			{
				return;
			}
			if (!tracked.IsPinnedToGrabStamp)
			{
				if (!tracked.IsEasingToSimulation && tracked.NetworkObject != null && _seatRenderStates.TryGetValue(tracked.NetworkObject.Id, out var value) && value.LastRenderedAt != Time.time && Time.time - value.LastRenderedAt <= 0.4f)
				{
					tracked.Body.isKinematic = false;
					Vector3 vector = ((_carrierBody != null && !_carrierBody.isKinematic) ? _carrierBody.angularVelocity : NetworkedCarrierAngularVelocity);
					Vector3 vector2 = ((_carrierBody != null) ? _carrierBody.position : base.transform.position);
					tracked.Body.linearVelocity = EffectiveCarrierVelocity + Vector3.Cross(vector, tracked.Body.position - vector2);
					tracked.Body.angularVelocity = vector;
					tracked.IsEasingToSimulation = true;
					tracked.EaseStartedAt = Time.time;
				}
			}
			else if (tracked.NetworkObject != null && !tracked.NetworkObject.HasStateAuthority)
			{
				tracked.IsPinnedToGrabStamp = false;
				tracked.HasGrabStamp = false;
			}
			else
			{
				GetGrabStampBasis(out var basisPosition, out var basisRotation);
				Vector3 vector3 = basisPosition + basisRotation * tracked.GrabStampLocalPosition;
				tracked.Body.position = vector3;
				tracked.Body.rotation = basisRotation * tracked.GrabStampLocalRotation;
				tracked.Body.isKinematic = false;
				tracked.Body.transform.SetPositionAndRotation(base.transform.position + base.transform.rotation * tracked.GrabStampRenderLocalPosition, base.transform.rotation * tracked.GrabStampRenderLocalRotation);
				Vector3 vector4 = ((_carrierBody != null && !_carrierBody.isKinematic) ? _carrierBody.angularVelocity : NetworkedCarrierAngularVelocity);
				Vector3 vector5 = ((_carrierBody != null) ? _carrierBody.position : base.transform.position);
				Vector3 linearVelocity = EffectiveCarrierVelocity + Vector3.Cross(vector4, vector3 - vector5);
				tracked.Body.linearVelocity = linearVelocity;
				tracked.Body.angularVelocity = vector4;
				tracked.IsPinnedToGrabStamp = false;
				tracked.IsEasingToSimulation = true;
				tracked.EaseStartedAt = Time.time;
			}
		}

		private void GetGrabStampBasis(out Vector3 basisPosition, out Quaternion basisRotation)
		{
			if (_carrierBody != null)
			{
				basisPosition = _carrierBody.position;
				basisRotation = _carrierBody.rotation;
			}
			else
			{
				basisPosition = base.transform.position;
				basisRotation = base.transform.rotation;
			}
		}

		private bool IsLocalHaulOfCarrier()
		{
			if (IsCarrierHeldLocally())
			{
				_lastHeldAt = Time.time;
				return true;
			}
			return Time.time - _lastHeldAt <= 0.25f;
		}

		private bool IsCarrierHeldLocally()
		{
			if (_carrierGrabable == null || base.Runner == null)
			{
				return false;
			}
			return _carrierGrabable.GrabbedByPlayers?.Contains(base.Runner.LocalPlayer.PlayerId) ?? false;
		}

		private bool IsSeatDrawnThisFrame(TrackedBody tracked)
		{
			if (IsNetworked && tracked.NetworkObject != null && _seatRenderStates.TryGetValue(tracked.NetworkObject.Id, out var value))
			{
				return value.LastRenderedAt == Time.time;
			}
			return false;
		}

		private bool HasAnyPinnedCargo()
		{
			foreach (TrackedBody value in _tracked.Values)
			{
				if (value.IsPinnedToGrabStamp)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsGrabStampCandidate(TrackedBody tracked)
		{
			if (tracked.Body == null || tracked.NetworkObject == null)
			{
				return false;
			}
			if (tracked.IsPartBone || tracked.IsNestedCarrier || tracked.IsCeded)
			{
				return false;
			}
			if (IsActivelyGrabbed(tracked.Grabable) || IsWornHeadwear(tracked))
			{
				return false;
			}
			return !_bodyToPlayer.ContainsKey(tracked.Body);
		}

		private void StampUnstampedCargo()
		{
			GetGrabStampBasis(out var basisPosition, out var basisRotation);
			Quaternion quaternion = Quaternion.Inverse(basisRotation);
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!value.HasGrabStamp && !value.IsEasingToSimulation && IsGrabStampCandidate(value))
				{
					value.GrabStampLocalPosition = quaternion * (value.Body.position - basisPosition);
					value.GrabStampLocalRotation = quaternion * value.Body.rotation;
					value.GrabStampWasKinematic = value.Body.isKinematic;
					value.HasGrabStamp = true;
					value.IsPinnedToGrabStamp = false;
				}
			}
		}

		private void CaptureGrabStamps()
		{
			GetGrabStampBasis(out var basisPosition, out var basisRotation);
			Quaternion quaternion = Quaternion.Inverse(basisRotation);
			foreach (TrackedBody value in _tracked.Values)
			{
				if (IsGrabStampCandidate(value))
				{
					value.GrabStampLocalPosition = quaternion * (value.Body.position - basisPosition);
					value.GrabStampLocalRotation = quaternion * value.Body.rotation;
					Quaternion quaternion2 = Quaternion.Inverse(base.transform.rotation);
					value.GrabStampRenderLocalPosition = quaternion2 * (value.Body.transform.position - base.transform.position);
					value.GrabStampRenderLocalRotation = quaternion2 * value.Body.transform.rotation;
					value.HasGrabStamp = true;
					value.GrabStampWasKinematic = value.Body.isKinematic;
					value.HasGrabStamp = true;
					value.IsPinnedToGrabStamp = false;
				}
			}
		}

		private void ReleaseGrabStampPins()
		{
			foreach (TrackedBody value in _tracked.Values)
			{
				if (value.HasGrabStamp)
				{
					if (value.IsPinnedToGrabStamp)
					{
						HandBackToSimulation(value);
						continue;
					}
					value.IsPinnedToGrabStamp = false;
					value.HasGrabStamp = false;
				}
			}
		}

		private void EaseReleasedCargoIntoSimulation()
		{
			foreach (TrackedBody value in _tracked.Values)
			{
				if (!value.IsEasingToSimulation)
				{
					continue;
				}
				if (value.Body == null)
				{
					value.IsEasingToSimulation = false;
					value.HasGrabStamp = false;
					continue;
				}
				float num = 1f - (Time.time - value.EaseStartedAt) / 0.4f;
				if (num <= 0f || IsActivelyGrabbed(value.Grabable))
				{
					value.IsEasingToSimulation = false;
					value.HasGrabStamp = false;
					continue;
				}
				Vector3 lhs = ((_carrierBody != null && !_carrierBody.isKinematic) ? _carrierBody.angularVelocity : NetworkedCarrierAngularVelocity);
				Vector3 vector = ((_carrierBody != null) ? _carrierBody.position : base.transform.position);
				Vector3 b = EffectiveCarrierVelocity + Vector3.Cross(lhs, value.Body.position - vector);
				value.Body.linearVelocity = Vector3.Lerp(value.Body.linearVelocity, b, num);
			}
		}

		private bool HasLocalControl(TrackedBody tracked)
		{
			if (!(tracked.NetworkObject == null))
			{
				return tracked.NetworkObject.HasStateAuthority;
			}
			return true;
		}

		internal void TickEarlyPredict()
		{
			AdvanceCarrierPrediction();
		}

		internal void TickLateApply()
		{
			ApplyProxyCargoRenderPoses();
			RestampCarrierPrediction();
			ApplyRestingPoses();
			ApplyReplicatedLocks();
			ApplyReplicatedCargoSeats();
			ApplyRiderLockPoses();
			ApplyCarriedRagdollPoses();
			ApplyObserverCarriedRagdollPoses();
			EnforceCarriedRagdollRenderFloor();
			ApplyRemoteCarrierPhysics();
			EnforceConstraintBox(_constraintRenderPrevious, killVelocity: false);
			ApplyGrabStampPins();
			ApplyRenderContinuityGuard();
		}

		private void EnforceConstraintBox(Dictionary<Rigidbody, Vector3> previousLocal, bool killVelocity)
		{
			if (_constraintBox == null || !_constraintBox.HasHardSurface)
			{
				return;
			}
			_constraintScratch.Clear();
			foreach (Rigidbody key in previousLocal.Keys)
			{
				_constraintScratch.Add(key);
			}
			foreach (Rigidbody item in _constraintScratch)
			{
				if (item == null || !_tracked.ContainsKey(item))
				{
					previousLocal.Remove(item);
				}
			}
			bool isTipped = IsTipped;
			foreach (TrackedBody value2 in _tracked.Values)
			{
				Rigidbody body = value2.Body;
				if (body == null)
				{
					continue;
				}
				if (isTipped || IsActivelyGrabbed(value2.Grabable) || IsWornHeadwear(value2))
				{
					previousLocal.Remove(body);
					continue;
				}
				Vector3 vector = _constraintBox.ToLocal(body.worldCenterOfMass);
				if (!previousLocal.TryGetValue(body, out var value))
				{
					previousLocal[body] = vector;
					continue;
				}
				if (!_constraintBox.TryConstrain(value, vector, out var constrainedLocal, out var outwardWorld))
				{
					previousLocal[body] = vector;
					continue;
				}
				Vector3 vector2 = _constraintBox.ToWorld(constrainedLocal) - body.worldCenterOfMass;
				body.position += vector2;
				previousLocal[body] = constrainedLocal;
				if (killVelocity && !body.isKinematic)
				{
					float num = Vector3.Dot(body.linearVelocity, outwardWorld);
					if (num > 0f)
					{
						body.linearVelocity -= outwardWorld * num;
					}
				}
			}
		}

		private void EnforceCarriedRagdollRenderFloor()
		{
			if (_region == null)
			{
				return;
			}
			float floorWorldY = _region.FloorWorldY;
			foreach (PlayerCharacterMovableBase item in _ragdollCarried)
			{
				RagdollEntity ragdoll = GetRagdoll(item);
				if (!(ragdoll == null))
				{
					float value;
					if (ragdoll.IsSimulated || ragdoll.IsBlendingOut)
					{
						_ragdollFloorTailUntil[item] = Time.time + 0.1f;
					}
					else if (!_ragdollFloorTailUntil.TryGetValue(item, out value) || Time.time > value)
					{
						continue;
					}
					IReadOnlyList<Rigidbody> bones = ragdoll.Bones;
					for (int i = 0; i < bones.Count; i++)
					{
						ClampBoneRenderAboveFloor(bones[i], floorWorldY, requireDynamic: false);
					}
				}
			}
			foreach (KeyValuePair<Rigidbody, PlayerCharacterMovableBase> item2 in _bodyToPlayer)
			{
				if (!_ragdollCarried.Contains(item2.Value) && (!(item2.Value != null) || !(item2.Key == item2.Value.Rigidbody)))
				{
					ClampBoneRenderAboveFloor(item2.Key, floorWorldY, requireDynamic: false);
				}
			}
		}

		private void ClampBoneRenderAboveFloor(Rigidbody bone, float floorY, bool requireDynamic)
		{
			if (bone == null || (requireDynamic && bone.isKinematic))
			{
				return;
			}
			Vector3 position = bone.transform.position;
			if (_region.ContainsXZ(position, 0.25f))
			{
				if (!_boneBottomOffsets.TryGetValue(bone, out var value))
				{
					value = BoneBottomOffset(bone);
				}
				float num = position.y - value;
				if (!(num >= floorY))
				{
					position.y += floorY - num;
					bone.transform.position = position;
				}
			}
		}

		private void AdvanceCarrierPrediction()
		{
			if (!IsNetworked || base.Object.HasStateAuthority || _carrierBody == null || !_carrierBody.isKinematic || NetworkedCarrierVelocityTick <= 0 || IsCarriedByOuterVolume() || IsOuterTrackedCarrier())
			{
				if (_followerActive && IsNetworked && _carrierBody != null && base.Object.HasStateAuthority)
				{
					_authorityHandoffActive = true;
					_authorityHandoffStartedAt = Time.time;
					_authorityHandoffFromPosition = _predictedPosition;
					_authorityHandoffFromRotation = _predictedRotation;
				}
				if (_followerActive && !_authorityHandoffActive)
				{
					_carrierHandbackActive = true;
					_carrierHandbackOffsetResolved = false;
					_carrierHandbackStartedAt = Time.time;
					_carrierHandbackFromPosition = _predictedPosition;
					_carrierHandbackFromRotation = _predictedRotation;
				}
				_followerActive = false;
				_deadReckonValid = false;
				return;
			}
			_authorityHandoffActive = false;
			_carrierHandbackActive = false;
			if (!_followerActive)
			{
				_followerActive = true;
				bool flag = Time.time - _lastInactiveRenderedAt <= 0.5f;
				_predictedPosition = (flag ? _lastInactiveRenderedPosition : base.transform.position);
				_predictedRotation = (flag ? _lastInactiveRenderedRotation : base.transform.rotation);
			}
			if (NetworkedCarrierVelocityTick != _lastSeenCarrierTick)
			{
				_lastSeenCarrierTick = NetworkedCarrierVelocityTick;
				_carrierStateReceivedAt = Time.time;
			}
			float num = 1f - Mathf.Exp((0f - Time.deltaTime) / 0.08f);
			Vector3 vector = NetworkedCarrierPosition;
			if (CarrierPathConstraint != null)
			{
				if (!_deadReckonValid)
				{
					_deadReckonedTarget = vector;
					_deadReckonValid = true;
				}
				_deadReckonedTarget += NetworkedCarrierVelocity * Time.deltaTime;
				float t = 1f - Mathf.Exp((0f - Time.deltaTime) / 0.25f);
				_deadReckonedTarget = Vector3.Lerp(_deadReckonedTarget, vector, t);
				_deadReckonedTarget = CarrierPathConstraint(_deadReckonedTarget);
				vector = _deadReckonedTarget;
			}
			if ((vector - _predictedPosition).magnitude > 25f)
			{
				_predictedPosition = vector;
			}
			else
			{
				_predictedPosition = Vector3.Lerp(_predictedPosition, vector, num);
			}
			if (CarrierPathConstraint != null)
			{
				_predictedPosition = CarrierPathConstraint(_predictedPosition);
			}
			base.transform.position = _predictedPosition;
			Vector3 vector2 = NetworkedCarrierAngularVelocity * 0f;
			float num2 = vector2.magnitude * 57.29578f;
			DecomposeSwingTwist(((num2 > 0.01f) ? (Quaternion.AngleAxis(num2, vector2.normalized) * NetworkedCarrierRotation) : NetworkedCarrierRotation) * Quaternion.Inverse(_predictedRotation), out var swing, out var twist);
			float num3 = Quaternion.Angle(Quaternion.identity, twist);
			float num4 = Quaternion.Angle(Quaternion.identity, swing);
			float maxDegreesDelta = Mathf.Min(num3 * num, (360f + num3 * 5f) * Time.deltaTime);
			float num5 = (IsActivelyGrabbed(_carrierGrabable) ? 0.25f : 0.08f);
			float num6 = 1f - Mathf.Exp((0f - Time.deltaTime) / num5);
			float maxDegreesDelta2 = Mathf.Min(num4 * num6, (360f + num4 * 5f) * Time.deltaTime);
			Quaternion quaternion = Quaternion.RotateTowards(Quaternion.identity, twist, maxDegreesDelta);
			Quaternion quaternion2 = Quaternion.RotateTowards(Quaternion.identity, swing, maxDegreesDelta2);
			_predictedRotation = Quaternion.Normalize(quaternion2 * quaternion * _predictedRotation);
			base.transform.rotation = _predictedRotation;
		}

		private static Vector3 ExtrapolateAlongArc(Vector3 velocity, Vector3 angularVelocity, float time)
		{
			float magnitude = angularVelocity.magnitude;
			float num = magnitude * time;
			if (num < 0.01f)
			{
				return velocity * time;
			}
			Vector3 vector = angularVelocity / magnitude;
			Vector3 vector2 = Vector3.Dot(velocity, vector) * vector;
			Vector3 vector3 = velocity - vector2;
			return vector2 * time + Mathf.Sin(num) / magnitude * vector3 + (1f - Mathf.Cos(num)) / magnitude * Vector3.Cross(vector, vector3);
		}

		private bool TryGetAuthorityHandoffEase(out Vector3 position, out Quaternion rotation)
		{
			position = default(Vector3);
			rotation = Quaternion.identity;
			if (!_authorityHandoffActive)
			{
				return false;
			}
			if (_carrierBody == null || !IsNetworked || !base.Object.HasStateAuthority)
			{
				_authorityHandoffActive = false;
				return false;
			}
			float num = Time.time - _authorityHandoffStartedAt;
			float num2 = Mathf.Exp((0f - num) / 0.12f);
			if (num > 0.6f || num2 < 0.02f)
			{
				_authorityHandoffActive = false;
				return false;
			}
			position = Vector3.Lerp(_carrierBody.position, _authorityHandoffFromPosition, num2);
			rotation = Quaternion.Slerp(_carrierBody.rotation, _authorityHandoffFromRotation, num2);
			return true;
		}

		private static void DecomposeSwingTwist(Quaternion delta, out Quaternion swing, out Quaternion twist)
		{
			float num = Mathf.Sqrt(delta.y * delta.y + delta.w * delta.w);
			if (num < 0.0001f)
			{
				twist = Quaternion.identity;
				swing = delta;
			}
			else
			{
				twist = new Quaternion(0f, delta.y / num, 0f, delta.w / num);
				swing = delta * Quaternion.Inverse(twist);
			}
		}

		private bool ApplyCarrierHandbackEase(ref Vector3 position, ref Quaternion rotation)
		{
			if (!_carrierHandbackActive)
			{
				return false;
			}
			float num = (Time.time - _carrierHandbackStartedAt) / 0.35f;
			if (num >= 1f)
			{
				_carrierHandbackActive = false;
				return false;
			}
			if (!_carrierHandbackOffsetResolved)
			{
				_carrierHandbackOffsetResolved = true;
				_carrierHandbackOffset = _carrierHandbackFromPosition - position;
				_carrierHandbackRotationOffset = _carrierHandbackFromRotation * Quaternion.Inverse(rotation);
			}
			float num2 = 1f - SmoothStep01(num);
			position += _carrierHandbackOffset * num2;
			rotation = Quaternion.Slerp(rotation, _carrierHandbackRotationOffset * rotation, num2);
			return true;
		}

		private void RestampCarrierPrediction()
		{
			if (!_followerActive && _frameBasisFromOuter)
			{
				Vector3 frameBasisPosition = _frameBasisPosition;
				Quaternion frameBasisRotation = _frameBasisRotation;
				base.transform.SetPositionAndRotation(frameBasisPosition, frameBasisRotation);
				_lastInactiveRenderedPosition = frameBasisPosition;
				_lastInactiveRenderedRotation = frameBasisRotation;
				_lastInactiveRenderedAt = Time.time;
			}
			else if (!_followerActive)
			{
				if (TryGetAuthorityHandoffEase(out var position, out var rotation))
				{
					base.transform.SetPositionAndRotation(position, rotation);
					_lastInactiveRenderedPosition = position;
					_lastInactiveRenderedRotation = rotation;
					_lastInactiveRenderedAt = Time.time;
					return;
				}
				Vector3 position2 = base.transform.position;
				Quaternion rotation2 = base.transform.rotation;
				if (ApplyCarrierHandbackEase(ref position2, ref rotation2))
				{
					base.transform.SetPositionAndRotation(position2, rotation2);
				}
				_lastInactiveRenderedPosition = position2;
				_lastInactiveRenderedRotation = rotation2;
				_lastInactiveRenderedAt = Time.time;
			}
			else
			{
				base.transform.SetPositionAndRotation(_predictedPosition, _predictedRotation);
			}
		}

		private static float CalmScore(float value, float limit)
		{
			if (limit <= 0f)
			{
				return 0f;
			}
			float num = limit * 0.4f;
			if (value <= num)
			{
				return 1f;
			}
			if (value >= limit)
			{
				return 0f;
			}
			return SmoothStep01(1f - (value - num) / (limit - num));
		}

		private static float SmoothStep01(float t)
		{
			return t * t * (3f - 2f * t);
		}

		private void ApplyProxyCargoRenderPoses()
		{
			Vector3 position = default(Vector3);
			Quaternion rotation = Quaternion.identity;
			int num;
			if (EffectiveCargoStrategy == CargoCarryStrategy.PhysicalInfluence)
			{
				num = (TryResolveRenderBasis(out position, out rotation, out _frameBasisFromOuter) ? 1 : 0);
				if (num != 0 && _frameBasisFromOuter)
				{
					ApplyCarrierHandbackEase(ref position, ref rotation);
				}
			}
			else
			{
				num = 0;
			}
			if (num == 0)
			{
				_frameBasisFromOuter = false;
				if (_cargoRenderSeats.Count > 0)
				{
					_cargoRenderSeats.Clear();
				}
				if (_cargoSeatGrace.Count > 0)
				{
					_cargoSeatGrace.Clear();
				}
				_cargoSeatReconcileWeight = 0f;
				return;
			}
			_frameBasisPosition = position;
			_frameBasisRotation = rotation;
			if (_cargoSeatGrace.Count > 0)
			{
				_cargoSeatGracePruneScratch.Clear();
				foreach (KeyValuePair<Rigidbody, CargoSeatGraceEntry> item in _cargoSeatGrace)
				{
					if (item.Key == null || Time.time - item.Value.DroppedAt > 0f)
					{
						_cargoSeatGracePruneScratch.Add(item.Key);
					}
				}
				foreach (Rigidbody item2 in _cargoSeatGracePruneScratch)
				{
					_cargoSeatGrace.Remove(item2);
				}
			}
			Vector3 position2 = base.transform.position;
			Quaternion rotation2 = base.transform.rotation;
			Quaternion quaternion = Quaternion.Inverse(rotation2);
			float magnitude = (position2 - _lastRawCarrierPosition).magnitude;
			float num2 = Quaternion.Angle(rotation2, _lastRawCarrierRotation);
			bool flag = magnitude <= 0.25f && num2 <= 10f;
			_lastRawCarrierPosition = position2;
			_lastRawCarrierRotation = rotation2;
			if (Time.deltaTime > 0f)
			{
				float magnitude2 = (position - _lastPredictedPositionForCalm).magnitude;
				float num3 = Quaternion.Angle(rotation, _lastPredictedRotationForCalm);
				float t = 1f - Mathf.Exp((0f - Time.deltaTime) / 0.25f);
				_carrierLinearSpeedSmoothed = Mathf.Lerp(_carrierLinearSpeedSmoothed, magnitude2 / Time.deltaTime, t);
				_carrierAngularSpeedSmoothed = Mathf.Lerp(_carrierAngularSpeedSmoothed, num3 / Time.deltaTime, t);
				_carrierLinearSpeedInstant = magnitude2 / Time.deltaTime;
				_carrierAngularSpeedInstant = num3 / Time.deltaTime;
			}
			_lastPredictedPositionForCalm = position;
			_lastPredictedRotationForCalm = rotation;
			bool flag2 = IsActivelyGrabbed(_carrierGrabable);
			float value = Mathf.Max(_carrierLinearSpeedSmoothed, _carrierLinearSpeedInstant);
			float num4 = Mathf.Min(b: CalmScore(Mathf.Max(_carrierAngularSpeedSmoothed, _carrierAngularSpeedInstant), flag2 ? 1f : 3f), a: CalmScore(value, flag2 ? 0.015f : 0.05f));
			bool flag3 = flag && num4 > 0f;
			_carrierCalmStreak = (flag3 ? (_carrierCalmStreak + Time.deltaTime) : 0f);
			float num5 = (flag2 ? 3f : 0.6f);
			float num6 = ((num5 <= 0f) ? 1f : SmoothStep01(Mathf.Clamp01(_carrierCalmStreak / num5)));
			float num7 = ((!_frameBasisFromOuter && flag) ? (num4 * num6) : 0f);
			float num8 = ((num7 > _cargoSeatReconcileWeight) ? 0.5f : 0.1f);
			_cargoSeatReconcileWeight = Mathf.MoveTowards(_cargoSeatReconcileWeight, num7, Time.deltaTime / num8);
			float cargoSeatReconcileWeight = _cargoSeatReconcileWeight;
			int num9 = ((base.Object != null && base.Object.IsValid) ? base.Object.StateAuthority.PlayerId : (-1));
			if (num9 != _lastCarrierAuthorityId)
			{
				_lastCarrierAuthorityId = num9;
				_carrierAuthorityChangedAt = Time.time;
			}
			foreach (TrackedBody value9 in _tracked.Values)
			{
				if (value9.Body == null)
				{
					continue;
				}
				CargoRenderSeat value2;
				CargoSeatData value4;
				if (IsActivelyGrabbed(value9.Grabable) || IsWornHeadwear(value9))
				{
					_cargoRenderSeats.Remove(value9.Body);
					_cargoSeatGrace.Remove(value9.Body);
				}
				else if (!IsCarriedByCarrierAuthority(value9) && (_region == null || !_region.Sees(value9.Body)) && SecondsVetoed(value9.Body) > 0.5f)
				{
					_cargoRenderSeats.Remove(value9.Body);
					_cargoSeatGrace.Remove(value9.Body);
				}
				else if (value9.IsCeded)
				{
					_cargoRenderSeats.Remove(value9.Body);
					_cargoSeatGrace.Remove(value9.Body);
				}
				else if (!value9.Body.isKinematic && HasLocalControl(value9) && value9.WasRidingInfluence && IsActivelyGrabbed(_carrierGrabable) && !IsLocalHaulOfCarrier() && !IsActivelyGrabbed(value9.Grabable) && _cargoRenderSeats.TryGetValue(value9.Body, out value2))
				{
					if (!_ownedChurnLastStampTime.TryGetValue(value9.Body, out var value3) || Time.time - value3 > 0.2f)
					{
						Quaternion quaternion2 = Quaternion.Inverse(rotation);
						value2.LocalPosition = quaternion2 * (value9.Body.transform.position - position);
						value2.LocalRotation = quaternion2 * value9.Body.transform.rotation;
					}
					_ownedChurnLastStampTime[value9.Body] = Time.time;
					Vector3 vector = position + rotation * value2.LocalPosition;
					Quaternion rotation3 = rotation * value2.LocalRotation;
					value9.Body.transform.SetPositionAndRotation(vector, rotation3);
					value9.Body.position = vector;
					value9.Body.rotation = rotation3;
					value9.Body.linearVelocity = Vector3.zero;
					value9.Body.angularVelocity = Vector3.zero;
					value2.LastStampedPosition = vector;
					_cargoRenderSeats[value9.Body] = value2;
				}
				else if (OwnerRetiredSeatRow(value9))
				{
					_cargoRenderSeats.Remove(value9.Body);
					_cargoSeatGrace.Remove(value9.Body);
				}
				else if (IsNetworked && value9.NetworkObject != null && CargoSeats.TryGet(value9.NetworkObject.Id, out value4))
				{
					_cargoRenderSeats[value9.Body] = new CargoRenderSeat
					{
						LocalPosition = value4.LocalPosition,
						LocalRotation = value4.LocalRotation,
						LastStampedPosition = value9.Body.position,
						DivergedSince = 0f
					};
					_cargoSeatGrace.Remove(value9.Body);
				}
				else
				{
					if (value9.BoneSynchronizer != null)
					{
						continue;
					}
					bool flag4 = HasLocalControl(value9);
					if (!value9.Body.isKinematic && !flag4 && IsActivelyGrabbed(_carrierGrabable) && !IsActivelyGrabbed(value9.Grabable) && _cargoRenderSeats.TryGetValue(value9.Body, out var value5))
					{
						Vector3 vector2 = position + rotation * value5.LocalPosition;
						Quaternion rotation4 = rotation * value5.LocalRotation;
						value9.Body.transform.SetPositionAndRotation(vector2, rotation4);
						value9.Body.position = vector2;
						value9.Body.rotation = rotation4;
						value9.Body.linearVelocity = Vector3.zero;
						value9.Body.angularVelocity = Vector3.zero;
						value5.LastStampedPosition = vector2;
						_cargoRenderSeats[value9.Body] = value5;
						continue;
					}
					if (!value9.Body.isKinematic || flag4)
					{
						Transform transform = value9.Body.transform;
						if (!((transform.position - position).sqrMagnitude > 25f))
						{
							Quaternion quaternion3 = Quaternion.Inverse(rotation);
							CargoRenderSeat value6 = new CargoRenderSeat
							{
								LocalPosition = quaternion3 * (transform.position - position),
								LocalRotation = quaternion3 * transform.rotation,
								LastStampedPosition = transform.position
							};
							_cargoRenderSeats[value9.Body] = value6;
						}
						continue;
					}
					Transform transform2 = value9.Body.transform;
					CargoRenderSeat value7;
					bool flag5 = _cargoRenderSeats.TryGetValue(value9.Body, out value7);
					if (!flag5 && _cargoSeatGrace.TryGetValue(value9.Body, out var value8))
					{
						_cargoSeatGrace.Remove(value9.Body);
						if (Time.time - value8.DroppedAt <= 0f)
						{
							value7 = value8.Seat;
							flag5 = true;
						}
					}
					if (!_frameBasisFromOuter && flag5 && transform2.position != value7.LastStampedPosition)
					{
						if ((quaternion * (transform2.position - position2) - value7.LocalPosition).magnitude > 0.5f)
						{
							if (value7.DivergedSince <= 0f)
							{
								value7.DivergedSince = Time.time;
							}
							else if (Time.time - value7.DivergedSince > 0.7f)
							{
								_cargoRenderSeats.Remove(value9.Body);
								_cargoSeatGrace.Remove(value9.Body);
								continue;
							}
						}
						else
						{
							value7.DivergedSince = 0f;
						}
					}
					if (!flag5)
					{
						Quaternion quaternion4 = Quaternion.Inverse(rotation);
						value7.LocalPosition = quaternion4 * (transform2.position - position);
						value7.LocalRotation = quaternion4 * transform2.rotation;
						value7.SeatedAt = Time.time;
					}
					else if (transform2.position != value7.LastStampedPosition && cargoSeatReconcileWeight > 0f)
					{
						Vector3 b = quaternion * (transform2.position - position2);
						Quaternion b2 = quaternion * transform2.rotation;
						float t2 = (1f - Mathf.Exp((0f - Time.deltaTime) / 0.4f)) * cargoSeatReconcileWeight;
						Vector3 target = Vector3.Lerp(value7.LocalPosition, b, t2);
						value7.LocalPosition = Vector3.MoveTowards(value7.LocalPosition, target, 0.002f * cargoSeatReconcileWeight);
						Quaternion to = Quaternion.Slerp(value7.LocalRotation, b2, t2);
						value7.LocalRotation = Quaternion.RotateTowards(value7.LocalRotation, to, 0.5f * cargoSeatReconcileWeight);
					}
					Vector3 vector3 = position + rotation * value7.LocalPosition;
					Quaternion quaternion5 = rotation * value7.LocalRotation;
					float num10 = SmoothStep01(Mathf.Clamp01((Time.time - value7.SeatedAt) / 0.35f));
					if (num10 < 1f)
					{
						vector3 = Vector3.Lerp(transform2.position, vector3, num10);
						quaternion5 = Quaternion.Slerp(transform2.rotation, quaternion5, num10);
					}
					if ((transform2.position - vector3).sqrMagnitude < 1E-10f)
					{
						value7.LastStampedPosition = vector3;
						_cargoRenderSeats[value9.Body] = value7;
						continue;
					}
					transform2.SetPositionAndRotation(vector3, quaternion5);
					value9.Body.position = vector3;
					value9.Body.rotation = quaternion5;
					value7.LastStampedPosition = vector3;
					_cargoRenderSeats[value9.Body] = value7;
				}
			}
			StampGracedCargoSeats(position2, quaternion, position, rotation);
			ApplyProxyRagdollCargoRenderPoses();
		}

		private void StampGracedCargoSeats(Vector3 rawPosition, Quaternion inverseRawRotation, Vector3 basisPosition, Quaternion basisRotation)
		{
			_cargoSeatGrace.Clear();
		}

		private void ApplyProxyRagdollCargoRenderPoses()
		{
			_ragdollCargoParts.Clear();
			foreach (TrackedBody value2 in _tracked.Values)
			{
				if (!(value2.Body == null) && !(value2.BoneSynchronizer == null) && !(value2.NetworkObject == null) && !value2.NetworkObject.HasStateAuthority && !HasLocalControl(value2) && !IsActivelyGrabbed(value2.Grabable) && !IsWornHeadwear(value2))
				{
					_ragdollCargoParts.Add(value2.NetworkObject);
				}
			}
			if (_ragdollCargoParts.Count == 0)
			{
				if (_boneRenderSeats.Count > 0)
				{
					_boneRenderSeats.Clear();
				}
			}
			else
			{
				if (!TryGetNetworkedCarrierPose(out var position, out var rotation))
				{
					return;
				}
				PruneStaleBoneSeats();
				float t = 1f - Mathf.Exp(-6f * Time.deltaTime);
				Quaternion quaternion = Quaternion.Inverse(rotation);
				foreach (NetworkObject ragdollCargoPart in _ragdollCargoParts)
				{
					PhysicsSynchronizer[] partSynchronizers = GetPartSynchronizers(ragdollCargoPart);
					foreach (PhysicsSynchronizer physicsSynchronizer in partSynchronizers)
					{
						if (!(physicsSynchronizer == null) && !(physicsSynchronizer.Rigidbody == null) && physicsSynchronizer.HasValidNetworkPose)
						{
							Rigidbody rigidbody = physicsSynchronizer.Rigidbody;
							Vector3 vector = quaternion * (physicsSynchronizer.Position - position);
							Quaternion quaternion2 = quaternion * ((Quaternion)physicsSynchronizer.Rotation).normalized;
							if (!_boneRenderSeats.TryGetValue(physicsSynchronizer, out var value) || (vector - value.LocalPosition).magnitude > 0.35f)
							{
								value.LocalPosition = vector;
								value.LocalRotation = quaternion2;
							}
							else
							{
								value.LocalPosition = Vector3.Lerp(value.LocalPosition, vector, t);
								value.LocalRotation = Quaternion.Slerp(value.LocalRotation, quaternion2, t);
							}
							_boneRenderSeats[physicsSynchronizer] = value;
							Vector3 vector2 = _predictedPosition + _predictedRotation * value.LocalPosition;
							Quaternion rotation2 = _predictedRotation * value.LocalRotation;
							if (!((rigidbody.transform.position - vector2).sqrMagnitude < 1E-10f))
							{
								rigidbody.transform.SetPositionAndRotation(vector2, rotation2);
								rigidbody.position = vector2;
								rigidbody.rotation = rotation2;
							}
						}
					}
				}
			}
		}

		private void PruneStaleBoneSeats()
		{
			if (_boneRenderSeats.Count == 0)
			{
				return;
			}
			_boneSeatPruneScratch.Clear();
			foreach (PhysicsSynchronizer key in _boneRenderSeats.Keys)
			{
				if (key == null || !_ragdollCargoParts.Contains(key.Object))
				{
					_boneSeatPruneScratch.Add(key);
				}
			}
			foreach (PhysicsSynchronizer item in _boneSeatPruneScratch)
			{
				_boneRenderSeats.Remove(item);
			}
		}

		private PhysicsSynchronizer[] GetPartSynchronizers(NetworkObject part)
		{
			if (!_partSynchronizers.TryGetValue(part, out var value))
			{
				value = part.GetComponentsInChildren<PhysicsSynchronizer>(includeInactive: true);
				_partSynchronizers[part] = value;
			}
			return value;
		}

		private bool TryGetNetworkedCarrierPose(out Vector3 position, out Quaternion rotation)
		{
			position = default(Vector3);
			rotation = default(Quaternion);
			if (!IsNetworked)
			{
				return false;
			}
			if (_carrierTrsp == null)
			{
				_carrierTrsp = GetComponentInParent<NetworkTRSP>();
				if (_carrierTrsp == null)
				{
					return false;
				}
			}
			position = _carrierTrsp.Data.Position;
			rotation = _carrierTrsp.Data.Rotation.normalized;
			return true;
		}

		private static PhysicsSynchronizer ResolveBoneSynchronizer(Rigidbody body)
		{
			PhysicsSynchronizer component = body.GetComponent<PhysicsSynchronizer>();
			if (component != null)
			{
				return component;
			}
			NetworkObject componentInParent = body.GetComponentInParent<NetworkObject>();
			if (componentInParent == null)
			{
				return null;
			}
			PhysicsSynchronizer[] componentsInChildren = componentInParent.GetComponentsInChildren<PhysicsSynchronizer>(includeInactive: true);
			foreach (PhysicsSynchronizer physicsSynchronizer in componentsInChildren)
			{
				if (physicsSynchronizer.Rigidbody == body)
				{
					return physicsSynchronizer;
				}
			}
			return null;
		}

		public void GetCarrierRenderPose(out Vector3 position, out Quaternion rotation)
		{
			if (_followerActive)
			{
				position = _predictedPosition;
				rotation = _predictedRotation;
			}
			else
			{
				position = base.transform.position;
				rotation = base.transform.rotation;
			}
		}

		private bool TryGetLockingVolume(NetworkId id, out PhysicsInfluenceVolume volume, out VolumeLockData data)
		{
			data = default(VolumeLockData);
			volume = null;
			for (int i = 0; i < _allVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = _allVolumes[i];
				if ((object)physicsInfluenceVolume != this && !(physicsInfluenceVolume == null) && physicsInfluenceVolume.isActiveAndEnabled && physicsInfluenceVolume.TryGetCargoLock(id, out data))
				{
					volume = physicsInfluenceVolume;
					return true;
				}
			}
			return false;
		}

		private PhysicsSynchronizer ResolveSeatSynchronizer(NetworkObject networkObject)
		{
			if (_seatSynchronizerCache.TryGetValue(networkObject.Id, out var value))
			{
				return value;
			}
			value = networkObject.GetComponent<PhysicsSynchronizer>();
			_seatSynchronizerCache[networkObject.Id] = value;
			return value;
		}

		private bool IsTrackedByNetworkId(NetworkId id)
		{
			foreach (TrackedBody value in _tracked.Values)
			{
				if (value.NetworkObject != null && value.NetworkObject.Id == id)
				{
					return true;
				}
			}
			return false;
		}

		private PhysicsInfluenceVolume ResolveHostedVolume(NetworkId id, NetworkObject networkObject)
		{
			if (_hostedVolumeCache.TryGetValue(id, out var value))
			{
				return value;
			}
			value = networkObject.GetComponentInChildren<PhysicsInfluenceVolume>();
			if (value == this)
			{
				value = null;
			}
			_hostedVolumeCache[id] = value;
			return value;
		}

		private void GetRenderedSeatBasis(out Vector3 position, out Quaternion rotation)
		{
			GetReplicatedSeatBasis(out position, out rotation);
			_renderedSeatBasisPosition = position;
			_renderedSeatBasisRotation = rotation;
			_renderedSeatBasisAt = Time.time;
		}

		private void PublishCargoSeats()
		{
			if (!IsNetworked || !base.Object.HasStateAuthority)
			{
				return;
			}
			foreach (TrackedBody value5 in _tracked.Values)
			{
				if (value5.NetworkObject == null)
				{
					continue;
				}
				if (!IsSeatPublisher(value5) || !IsSeatPublishable(value5))
				{
					CargoSeats.Remove(value5.NetworkObject.Id);
					continue;
				}
				if (!HasLocalControl(value5))
				{
					NetworkId id = value5.NetworkObject.Id;
					if (!_seatStaleSince.TryGetValue(id, out var value))
					{
						_seatStaleSince[id] = Time.time;
					}
					else if (Time.time - value > 0.4f)
					{
						CargoSeats.Remove(id);
					}
					_localControlSince.Remove(id);
					continue;
				}
				if (!HasSimulatedLocally(value5))
				{
					NetworkId id2 = value5.NetworkObject.Id;
					if (_seatStaleSince.TryGetValue(id2, out var value2) && Time.time - value2 > 0.4f)
					{
						CargoSeats.Remove(id2);
					}
					continue;
				}
				_seatStaleSince.Remove(value5.NetworkObject.Id);
				Vector3 vector = ((_carrierBody != null) ? _carrierBody.position : base.transform.position);
				Quaternion quaternion = Quaternion.Inverse((_carrierBody != null) ? _carrierBody.rotation : base.transform.rotation);
				Vector3 lossyScale = base.transform.lossyScale;
				Vector3 vector2 = quaternion * (value5.Body.position - vector);
				CargoSeatData value3 = new CargoSeatData
				{
					LocalPosition = new Vector3((lossyScale.x != 0f) ? (vector2.x / lossyScale.x) : vector2.x, (lossyScale.y != 0f) ? (vector2.y / lossyScale.y) : vector2.y, (lossyScale.z != 0f) ? (vector2.z / lossyScale.z) : vector2.z),
					LocalRotation = quaternion * value5.Body.rotation
				};
				if (!CargoSeats.TryGet(value5.NetworkObject.Id, out var value4) || (value3.LocalPosition - value4.LocalPosition).magnitude > 0.02f || Quaternion.Angle(value3.LocalRotation, value4.LocalRotation) > 2f)
				{
					PhysicsSynchronizer physicsSynchronizer = ResolveSeatSynchronizer(value5.NetworkObject);
					if (physicsSynchronizer != null)
					{
						physicsSynchronizer.ForcePoseResync();
					}
				}
				CargoSeats.Set(value5.NetworkObject.Id, value3);
			}
			_cargoSeatPruneIds.Clear();
			foreach (KeyValuePair<NetworkId, CargoSeatData> cargoSeat in CargoSeats)
			{
				bool flag = false;
				foreach (TrackedBody value6 in _tracked.Values)
				{
					if (value6.NetworkObject != null && value6.NetworkObject.Id == cargoSeat.Key)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					_cargoSeatPruneIds.Add(cargoSeat.Key);
				}
			}
			foreach (NetworkId cargoSeatPruneId in _cargoSeatPruneIds)
			{
				CargoSeats.Remove(cargoSeatPruneId);
				_seatStaleSince.Remove(cargoSeatPruneId);
				_localControlSince.Remove(cargoSeatPruneId);
			}
		}

		private void ApplyReplicatedCargoSeats()
		{
			if (!IsNetworked)
			{
				return;
			}
			bool flag = false;
			foreach (TrackedBody value7 in _tracked.Values)
			{
				if (value7.NetworkObject != null && value7.NetworkObject.HasStateAuthority)
				{
					flag = true;
					break;
				}
			}
			if (CargoSeats.Count == 0 && _seatRenderStates.Count == 0 && !flag && _ownedSeatPoses.Count == 0)
			{
				return;
			}
			GetRenderedSeatBasis(out var position, out var rotation);
			Matrix4x4 basisMatrix = Matrix4x4.TRS(position, rotation, base.transform.lossyScale);
			Matrix4x4 inverse = basisMatrix.inverse;
			Quaternion quaternion = Quaternion.Inverse(rotation);
			if (flag)
			{
				foreach (TrackedBody value8 in _tracked.Values)
				{
					if (!(value8.NetworkObject == null) && value8.NetworkObject.HasStateAuthority)
					{
						SeatRenderState value = new SeatRenderState
						{
							LocalPosition = inverse.MultiplyPoint3x4(value8.NetworkObject.transform.position),
							LocalRotation = quaternion * value8.NetworkObject.transform.rotation,
							LastRenderedAt = Time.time
						};
						_ownedSeatPoses[value8.NetworkObject.Id] = value;
						_authorityHandoffs.Remove(value8.NetworkObject.Id);
					}
				}
			}
			foreach (KeyValuePair<NetworkId, CargoSeatData> cargoSeat in CargoSeats)
			{
				if (!base.Runner.TryFindObject(cargoSeat.Key, out var networkObject) || networkObject == null || networkObject.HasStateAuthority || IsActivelyGrabbed(GetReplicatedGrabable(networkObject)) || IsLockedByAnotherVolume(cargoSeat.Key))
				{
					continue;
				}
				int playerId = networkObject.StateAuthority.PlayerId;
				if (_seatInterpBuffers.TryGetValue(cargoSeat.Key, out var value2) && (value2.TargetPosition - value2.StartPosition).sqrMagnitude < 0.09f && _lastSeenSeatAuthority.TryGetValue(cargoSeat.Key, out var value3) && value3 != playerId && !_authorityHandoffs.ContainsKey(cargoSeat.Key) && _seatRenderStates.TryGetValue(cargoSeat.Key, out var value4) && Time.time - value4.LastRenderedAt <= 0.25f)
				{
					_authorityHandoffs[cargoSeat.Key] = new AuthorityHandoff
					{
						HeldLocalPosition = value4.LocalPosition,
						HeldLocalRotation = value4.LocalRotation,
						LostAt = Time.time
					};
				}
				_lastSeenSeatAuthority[cargoSeat.Key] = playerId;
				Vector3 localPosition = cargoSeat.Value.LocalPosition;
				Quaternion localRotation = cargoSeat.Value.LocalRotation;
				Vector3 localPosition2 = localPosition;
				Quaternion localRotation2 = localRotation;
				InterpolateSeatLocal(cargoSeat.Key, ref localPosition2, ref localRotation2);
				ApplyHandoffEase(cargoSeat.Key, ref localPosition, ref localRotation);
				ClampComposedSeatInsideHardSurfaces(cargoSeat.Value.LocalPosition, ref localPosition, basisMatrix);
				_seatRenderStates.TryGetValue(cargoSeat.Key, out var value5);
				value5.LocalPosition = localPosition;
				value5.LocalRotation = localRotation;
				value5.WorldPosition = basisMatrix.MultiplyPoint3x4(localPosition);
				value5.WorldRotation = rotation * localRotation;
				value5.LastRenderedAt = Time.time;
				_seatRenderStates[cargoSeat.Key] = value5;
				_seatReleaseBridges.Remove(cargoSeat.Key);
				networkObject.transform.SetPositionAndRotation(value5.WorldPosition, value5.WorldRotation);
				PhysicsInfluenceVolume physicsInfluenceVolume = ResolveHostedVolume(cargoSeat.Key, networkObject);
				if (physicsInfluenceVolume != null && physicsInfluenceVolume.isActiveAndEnabled && _seatChainDepth < 3)
				{
					_seatChainDepth++;
					try
					{
						physicsInfluenceVolume.ApplyReplicatedCargoSeats();
					}
					finally
					{
						_seatChainDepth--;
					}
				}
			}
			_seatWarmScratch.Clear();
			foreach (KeyValuePair<NetworkId, SeatRenderState> seatRenderState in _seatRenderStates)
			{
				SeatRenderState value6 = seatRenderState.Value;
				if (value6.LastRenderedAt != Time.time && base.Runner.TryFindObject(seatRenderState.Key, out var networkObject2) && !(networkObject2 == null) && !networkObject2.HasStateAuthority)
				{
					if (!IsActivelyGrabbed(GetReplicatedGrabable(networkObject2)) && TryGetLockingVolume(seatRenderState.Key, out var volume, out var data))
					{
						Vector3 vector = volume.transform.TransformPoint(data.LocalPosition);
						Quaternion quaternion2 = volume.transform.rotation * data.LocalRotation;
						value6.LocalPosition = inverse.MultiplyPoint3x4(vector);
						value6.LocalRotation = quaternion * quaternion2;
						value6.WorldPosition = vector;
						value6.WorldRotation = quaternion2;
						value6.LastRenderedAt = Time.time;
						_seatReleaseBridges.Remove(seatRenderState.Key);
						_seatWarmScratch.Add(new KeyValuePair<NetworkId, SeatRenderState>(seatRenderState.Key, value6));
					}
					else
					{
						_ = _seatChainDepth;
					}
				}
			}
			foreach (KeyValuePair<NetworkId, SeatRenderState> item in _seatWarmScratch)
			{
				_seatRenderStates[item.Key] = item.Value;
			}
			if (_seatRenderStates.Count > 0)
			{
				_seatRenderStatePruneScratch.Clear();
				foreach (KeyValuePair<NetworkId, SeatRenderState> seatRenderState2 in _seatRenderStates)
				{
					if (Time.time - seatRenderState2.Value.LastRenderedAt > 2f)
					{
						_seatRenderStatePruneScratch.Add(seatRenderState2.Key);
					}
				}
				foreach (NetworkId item2 in _seatRenderStatePruneScratch)
				{
					_seatRenderStates.Remove(item2);
					_seatReleaseBridges.Remove(item2);
					_lastSeenSeatAuthority.Remove(item2);
					_seatInterpBuffers.Remove(item2);
				}
			}
			if (_ownedSeatPoses.Count > 0)
			{
				_seatRenderStatePruneScratch.Clear();
				foreach (KeyValuePair<NetworkId, SeatRenderState> ownedSeatPose in _ownedSeatPoses)
				{
					if (Time.time - ownedSeatPose.Value.LastRenderedAt > 2f)
					{
						_seatRenderStatePruneScratch.Add(ownedSeatPose.Key);
					}
				}
				foreach (NetworkId item3 in _seatRenderStatePruneScratch)
				{
					_ownedSeatPoses.Remove(item3);
				}
			}
			if (_authorityHandoffs.Count <= 0)
			{
				return;
			}
			_seatRenderStatePruneScratch.Clear();
			foreach (KeyValuePair<NetworkId, AuthorityHandoff> authorityHandoff in _authorityHandoffs)
			{
				if (Time.time - authorityHandoff.Value.LostAt > 2f)
				{
					_seatRenderStatePruneScratch.Add(authorityHandoff.Key);
				}
			}
			foreach (NetworkId item4 in _seatRenderStatePruneScratch)
			{
				_authorityHandoffs.Remove(item4);
			}
		}

		private void InterpolateSeatLocal(NetworkId id, ref Vector3 localPosition, ref Quaternion localRotation)
		{
			if (!_seatInterpBuffers.TryGetValue(id, out var value))
			{
				value = new SeatInterpBuffer
				{
					StartPosition = localPosition,
					StartRotation = localRotation,
					TargetPosition = localPosition,
					TargetRotation = localRotation,
					TargetAt = Time.time,
					BlendSeconds = 0.02f
				};
				_seatInterpBuffers[id] = value;
				return;
			}
			float t = Mathf.Clamp01((Time.time - value.TargetAt) / value.BlendSeconds);
			if ((localPosition - value.TargetPosition).sqrMagnitude > 1E-08f || Quaternion.Angle(localRotation, value.TargetRotation) > 0.01f)
			{
				value.StartPosition = Vector3.Lerp(value.StartPosition, value.TargetPosition, t);
				value.StartRotation = Quaternion.Slerp(value.StartRotation, value.TargetRotation, t);
				value.BlendSeconds = Mathf.Clamp(Time.time - value.TargetAt, 0.02f, 0.15f);
				value.TargetPosition = localPosition;
				value.TargetRotation = localRotation;
				value.TargetAt = Time.time;
				t = 0f;
			}
			_seatInterpBuffers[id] = value;
			localPosition = Vector3.Lerp(value.StartPosition, value.TargetPosition, t);
			localRotation = Quaternion.Slerp(value.StartRotation, value.TargetRotation, t);
		}

		private void ClampComposedSeatInsideHardSurfaces(Vector3 publishedLocal, ref Vector3 composedLocal, Matrix4x4 basisMatrix)
		{
			if (_constraintBox == null || !_constraintBox.HasHardSurface)
			{
				return;
			}
			Vector3 vector = _constraintBox.ToLocal(basisMatrix.MultiplyPoint3x4(publishedLocal));
			if (_constraintBox.ContainsWithinHardSurfaces(vector))
			{
				Vector3 currentLocal = _constraintBox.ToLocal(basisMatrix.MultiplyPoint3x4(composedLocal));
				if (_constraintBox.TryConstrain(vector, currentLocal, out var constrainedLocal, out var _))
				{
					Vector3 vector2 = basisMatrix.inverse.MultiplyPoint3x4(_constraintBox.ToWorld(constrainedLocal));
					composedLocal = vector2;
				}
			}
		}

		private void ApplyHandoffEase(NetworkId id, ref Vector3 localPosition, ref Quaternion localRotation)
		{
			if (!_authorityHandoffs.TryGetValue(id, out var value))
			{
				if (!_ownedSeatPoses.TryGetValue(id, out var value2) || Time.time - value2.LastRenderedAt > 0.25f || (value2.LocalPosition - localPosition).sqrMagnitude > 0.5625f)
				{
					return;
				}
				value = new AuthorityHandoff
				{
					HeldLocalPosition = value2.LocalPosition,
					HeldLocalRotation = value2.LocalRotation,
					LostAt = Time.time
				};
				_authorityHandoffs[id] = value;
				_ownedSeatPoses.Remove(id);
			}
			float num = (Time.time - value.LostAt) / 0.4f;
			if (num >= 1f)
			{
				_authorityHandoffs.Remove(id);
				return;
			}
			float t = num * num;
			localPosition = Vector3.Lerp(value.HeldLocalPosition, localPosition, t);
			localRotation = Quaternion.Slerp(value.HeldLocalRotation, localRotation, t);
		}

		private void ApplyReplicatedCargoSeatPhysics()
		{
			if (!IsNetworked || CargoSeats.Count == 0)
			{
				return;
			}
			Vector3 position;
			Quaternion rotation;
			if (Time.time - _renderedSeatBasisAt <= 0.1f)
			{
				position = _renderedSeatBasisPosition;
				rotation = _renderedSeatBasisRotation;
			}
			else
			{
				GetReplicatedSeatBasis(out position, out rotation);
			}
			Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, base.transform.lossyScale);
			foreach (KeyValuePair<NetworkId, CargoSeatData> cargoSeat in CargoSeats)
			{
				if (base.Runner.TryFindObject(cargoSeat.Key, out var networkObject) && !(networkObject == null) && !networkObject.HasStateAuthority && !IsActivelyGrabbed(GetReplicatedGrabable(networkObject)) && !IsLockedByAnotherVolume(cargoSeat.Key) && networkObject.TryGetComponent<Rigidbody>(out var component))
				{
					Vector3 localPosition = cargoSeat.Value.LocalPosition;
					Quaternion localRotation = cargoSeat.Value.LocalRotation;
					component.position = matrix4x.MultiplyPoint3x4(localPosition);
					component.rotation = rotation * localRotation;
				}
			}
		}

		private void ApplyReplicatedLocks()
		{
			if (!IsNetworked || Locks.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<NetworkId, VolumeLockData> @lock in Locks)
			{
				if (!base.Runner.TryFindObject(@lock.Key, out var networkObject) || networkObject == null || networkObject.HasStateAuthority || IsActivelyGrabbed(GetReplicatedGrabable(networkObject)))
				{
					continue;
				}
				Vector3 localPosition = @lock.Value.LocalPosition;
				Quaternion localRotation = @lock.Value.LocalRotation;
				ApplyHandoffEase(@lock.Key, ref localPosition, ref localRotation);
				Vector3 position = base.transform.TransformPoint(localPosition);
				Quaternion rotation = base.transform.rotation * localRotation;
				networkObject.transform.SetPositionAndRotation(position, rotation);
				PhysicsInfluenceVolume physicsInfluenceVolume = ResolveHostedVolume(@lock.Key, networkObject);
				if (physicsInfluenceVolume != null && physicsInfluenceVolume.isActiveAndEnabled && _seatChainDepth < 3)
				{
					_seatChainDepth++;
					try
					{
						physicsInfluenceVolume.ApplyReplicatedCargoSeats();
					}
					finally
					{
						_seatChainDepth--;
					}
				}
			}
		}

		private void ApplyReplicatedLockPhysics()
		{
			if (!IsNetworked || Locks.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<NetworkId, VolumeLockData> @lock in Locks)
			{
				if (base.Runner.TryFindObject(@lock.Key, out var networkObject) && !(networkObject == null) && !networkObject.HasStateAuthority && !IsActivelyGrabbed(GetReplicatedGrabable(networkObject)) && networkObject.TryGetComponent<Rigidbody>(out var component))
				{
					component.position = base.transform.TransformPoint(@lock.Value.LocalPosition);
					component.rotation = base.transform.rotation * @lock.Value.LocalRotation;
				}
			}
		}

		private void ApplyRestingPoses()
		{
			foreach (TrackedBody value in _tracked.Values)
			{
				if (value.IsResting && !(value.Body == null) && HasLocalControl(value))
				{
					Vector3 position = base.transform.TransformPoint(value.LocalPosition);
					Quaternion rotation = base.transform.rotation * value.LocalRotation;
					value.Body.transform.SetPositionAndRotation(position, rotation);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkBehaviourUtils.InitializeNetworkDictionary(Locks, _Locks, "Locks");
			NetworkBehaviourUtils.InitializeNetworkDictionary(CargoSeats, _CargoSeats, "CargoSeats");
			NetworkedCarrierPosition = _NetworkedCarrierPosition;
			NetworkedCarrierVelocity = _NetworkedCarrierVelocity;
			NetworkedCarrierVelocityTick = _NetworkedCarrierVelocityTick;
			NetworkedCarrierRotation = _NetworkedCarrierRotation;
			NetworkedCarrierAngularVelocity = _NetworkedCarrierAngularVelocity;
			NetworkedCarrierPathAnchor = _NetworkedCarrierPathAnchor;
			NetworkedCarrierPathAnchorSet = _NetworkedCarrierPathAnchorSet;
			NetworkedCarrierPathT = _NetworkedCarrierPathT;
			NetworkedSettledWeight = _NetworkedSettledWeight;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			NetworkBehaviourUtils.CopyFromNetworkDictionary(Locks, ref _Locks);
			NetworkBehaviourUtils.CopyFromNetworkDictionary(CargoSeats, ref _CargoSeats);
			_NetworkedCarrierPosition = NetworkedCarrierPosition;
			_NetworkedCarrierVelocity = NetworkedCarrierVelocity;
			_NetworkedCarrierVelocityTick = NetworkedCarrierVelocityTick;
			_NetworkedCarrierRotation = NetworkedCarrierRotation;
			_NetworkedCarrierAngularVelocity = NetworkedCarrierAngularVelocity;
			_NetworkedCarrierPathAnchor = NetworkedCarrierPathAnchor;
			_NetworkedCarrierPathAnchorSet = NetworkedCarrierPathAnchorSet;
			_NetworkedCarrierPathT = NetworkedCarrierPathT;
			_NetworkedSettledWeight = NetworkedSettledWeight;
		}

		[NetworkRpcWeavedInvoker(3510912813u)]
		[Preserve]
		[WeaverGenerated]
		protected static void EjectRPC_0040Invoker3510912813([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PhysicsInfluenceVolume)context.TargetBehaviour).EjectRPC(value);
		}

		[NetworkRpcWeavedInvoker(3629481157u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SlamRidersRPC_0040Invoker3629481157([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out float value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PhysicsInfluenceVolume)context.TargetBehaviour).SlamRidersRPC(value, value2);
		}
	}
}
