using System.Collections.Generic;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.PhysicsVolumeModule.Scripts
{
	[DefaultExecutionOrder(-50)]
	public class Lift : MonoBehaviour
	{
		[SerializeField]
		private PhysicsInfluenceVolume _volume;

		[Header("Shaft travel")]
		[Tooltip("How far the deck travels straight up from its placed (bottom) position, in metres.")]
		[SerializeField]
		private float _travelDistance = 5f;

		[Header("Crank & brake")]
		[SerializeField]
		private CrankRotator _crank;

		[SerializeField]
		private TwoStateLever _brakeLever;

		[Tooltip("Which lever position engages the brake: crank locked, deck held.")]
		[SerializeField]
		private LeverState _brakeEngagedState = LeverState.Up;

		[Header("Floor call levers")]
		[Tooltip("Deck speed (m/s) of an automatic run answering a floor call lever.")]
		[SerializeField]
		private float _callDriveSpeed = 2f;

		[Tooltip("The floors this shaft serves: one entry per call lever, each with the height it stops at. As many as the shaft needs.")]
		[SerializeField]
		private List<LiftCallStop> _callStops = new List<LiftCallStop>();

		[Header("Load")]
		[Tooltip("Rated max load weight: full-scale for the weight gauge AND the reference the fall acceleration scales against.")]
		[SerializeField]
		private float _maxWeight = 50f;

		[Header("Automatic descent")]
		[Tooltip("Downward acceleration (m/s^2) once released, at ZERO load.")]
		[SerializeField]
		private float _fallAcceleration = 3f;

		[Tooltip("Downward acceleration (m/s^2) at full rated load — the fall accel scales up to this as weight is loaded on.")]
		[SerializeField]
		private float _ladenFallAcceleration = 10f;

		[Tooltip("Terminal downward speed (m/s) of a free fall.")]
		[SerializeField]
		private float _maxFallSpeed = 4f;

		[Tooltip("Seconds after the crank is let go (or the brake opens) before the deck begins to descend — the window to re-catch.")]
		[SerializeField]
		private float _releaseGraceSeconds = 2f;

		[Header("Manual control (crank grabbed)")]
		[Tooltip("Crank degrees per metre of lift travel — the winch ratio for both the fall-spin and manual control.")]
		[SerializeField]
		private float _crankDegreesPerMeter = 360f;

		[Tooltip("How hard the deck eases toward the crank-commanded speed when caught (m/s^2) — the gentle catch.")]
		[SerializeField]
		private float _catchDeceleration = 6f;

		[Tooltip("How hard the brake bleeds the deck to a stop (m/s^2).")]
		[SerializeField]
		private float _brakeDeceleration = 12f;

		[Header("Bottom impact")]
		[Tooltip("Downward deck speed (m/s) at the foot of the shaft above which riders are ragdolled and hurt.")]
		[SerializeField]
		private float _crashImpactSpeed = 3f;

		[Tooltip("Damage each rider takes on a hard bottom impact — applied by each rider to itself.")]
		[SerializeField]
		private float _crashDamage = 25f;

		[Tooltip("Seconds a rider stays ragdolled after a bottom impact.")]
		[SerializeField]
		private float _crashRagdollRecoverySeconds = 2f;

		[Tooltip("Impact damage (cost-reduction force) each cargo item takes on a hard bottom impact. 0 = items unharmed.")]
		[SerializeField]
		private float _crashItemDamage = 1f;

		private Vector3 _startPosition;

		private bool _hasStart;

		private Vector3 _deckWorldVelocity;

		private Vector3 _cachedShaftBase;

		private bool _hasCachedShaftBase;

		private Rigidbody _deckBody;

		private readonly HashSet<PhysicsInfluenceVolume> _hauledCarriers = new HashSet<PhysicsInfluenceVolume>();

		private readonly HashSet<PhysicsInfluenceVolume> _presentCarriers = new HashSet<PhysicsInfluenceVolume>();

		private readonly List<PhysicsInfluenceVolume> _departedCarriers = new List<PhysicsInfluenceVolume>();

		private LiftMode _mode;

		private float _shaftT;

		private float _deckVelocity;

		private float _autoDriveVelocity;

		private float _catchResidual;

		private float _releaseTimer;

		private float _driveAnchorAngle;

		private float _driveAnchorT;

		private float _residualOffsetT;

		private bool _wasControlled;

		private bool _wasAuthority;

		private bool _wasAtBottom;

		private bool _driveActive;

		private const float LIMIT_END_EPSILON = 0.004f;

		private float _bottomStopAngle;

		private float _topStopAngle;

		private bool _wasLimitAtBottom;

		private bool _wasLimitAtTop;

		private IItemCostReduceService _itemCostReduceService;

		private readonly List<Rigidbody> _cargoScratch = new List<Rigidbody>();

		private int _activeCall = -1;

		private const int NO_CALL = -1;

		private const float CALL_ARRIVAL_EPSILON_METERS = 0.01f;

		private const float AT_FLOOR_EPSILON_METERS = 0.15f;

		public PhysicsInfluenceVolume Volume => _volume;

		public LiftMode Mode => _mode;

		public IReadOnlyList<LiftCallStop> CallStops => _callStops;

		public LiftCallStop ActiveCall
		{
			get
			{
				if (_activeCall < 0 || _activeCall >= _callStops.Count)
				{
					return null;
				}
				return _callStops[_activeCall];
			}
		}

		public float MaxWeight => _maxWeight;

		public Vector3 ShaftBasePosition
		{
			get
			{
				if (!TryGetReplicatedShaftBase(out var basePosition))
				{
					if (!_hasStart)
					{
						return base.transform.position;
					}
					return _startPosition;
				}
				return basePosition;
			}
		}

		public Vector3 BottomPoint => ShaftBasePosition;

		public Vector3 TopPoint => ShaftBasePosition + Vector3.up * _travelDistance;

		public float ShaftHeight => Mathf.Abs(_travelDistance);

		public Vector3 DeckPosition
		{
			get
			{
				if (!(_deckBody != null))
				{
					return base.transform.position;
				}
				return _deckBody.position;
			}
		}

		public float AutoDriveVelocity => _autoDriveVelocity;

		[Inject]
		public void InjectDependencies(IItemCostReduceService itemCostReduceService)
		{
			_itemCostReduceService = itemCostReduceService;
		}

		public float StopShaftT(LiftCallStop stop)
		{
			if (stop != null)
			{
				return Mathf.Clamp01(stop.HeightMeters / Mathf.Max(0.0001f, ShaftHeight));
			}
			return 0f;
		}

		private float CalledShaftT()
		{
			return StopShaftT(ActiveCall);
		}

		private bool TryGetReplicatedShaftBase(out Vector3 basePosition)
		{
			if (_hasCachedShaftBase)
			{
				basePosition = _cachedShaftBase;
				return true;
			}
			basePosition = default(Vector3);
			if (_volume == null || _volume.Object == null || !_volume.Object.IsValid || !_volume.NetworkedCarrierPathAnchorSet)
			{
				return false;
			}
			basePosition = _volume.NetworkedCarrierPathAnchor;
			_cachedShaftBase = basePosition;
			_hasCachedShaftBase = true;
			return true;
		}

		public Vector3 GetShaftPoint(float t)
		{
			return Vector3.Lerp(BottomPoint, TopPoint, Mathf.Clamp01(t));
		}

		public void SetAutoDrive(float metersPerSecond)
		{
			_autoDriveVelocity = metersPerSecond;
		}

		private void Awake()
		{
			_deckBody = ((_volume != null) ? _volume.GetComponent<Rigidbody>() : GetComponent<Rigidbody>());
		}

		private void Start()
		{
			_startPosition = base.transform.position;
			_hasStart = true;
			if (_deckBody != null)
			{
				_shaftT = ProjectShaftT(_deckBody.position);
			}
			if (_volume != null)
			{
				_volume.CarrierPathConstraint = (Vector3 position) => GetShaftPoint(ProjectShaftT(position));
				_volume.CarrierVelocityProvider = () => _deckWorldVelocity;
			}
		}

		private void OnDestroy()
		{
			if (_volume != null)
			{
				_volume.CarrierPathConstraint = null;
				_volume.CarrierVelocityProvider = null;
			}
			foreach (PhysicsInfluenceVolume hauledCarrier in _hauledCarriers)
			{
				if (hauledCarrier != null)
				{
					hauledCarrier.SetCargoClusterMode(enabled: false);
				}
			}
			_hauledCarriers.Clear();
		}

		private void UpdateHauledCarriers()
		{
			if (_volume == null)
			{
				return;
			}
			_presentCarriers.Clear();
			CollectNestedCarriers(_volume, 0);
			foreach (PhysicsInfluenceVolume presentCarrier in _presentCarriers)
			{
				if (_hauledCarriers.Add(presentCarrier))
				{
					presentCarrier.SetCargoClusterMode(enabled: true);
				}
			}
			_departedCarriers.Clear();
			foreach (PhysicsInfluenceVolume hauledCarrier in _hauledCarriers)
			{
				if (hauledCarrier == null || !_presentCarriers.Contains(hauledCarrier))
				{
					_departedCarriers.Add(hauledCarrier);
				}
			}
			foreach (PhysicsInfluenceVolume departedCarrier in _departedCarriers)
			{
				if (departedCarrier != null)
				{
					departedCarrier.SetCargoClusterMode(enabled: false);
				}
				_hauledCarriers.Remove(departedCarrier);
			}
		}

		private void CollectNestedCarriers(PhysicsInfluenceVolume volume, int depth)
		{
			if (volume == null || depth > 3)
			{
				return;
			}
			foreach (Rigidbody trackedCargoBody in volume.TrackedCargoBodies)
			{
				if (!(trackedCargoBody == null))
				{
					PhysicsInfluenceVolume componentInChildren = trackedCargoBody.GetComponentInChildren<PhysicsInfluenceVolume>();
					if (!(componentInChildren == null) && !(componentInChildren == _volume) && !(componentInChildren == volume) && _presentCarriers.Add(componentInChildren))
					{
						CollectNestedCarriers(componentInChildren, depth + 1);
					}
				}
			}
		}

		private void FixedUpdate()
		{
			bool flag = _volume != null && _volume.Object != null && _volume.Object.HasStateAuthority;
			UpdateHauledCarriers();
			if (flag && !_wasAuthority)
			{
				if (_deckBody != null)
				{
					_shaftT = ProjectShaftT(_deckBody.position);
				}
				_deckVelocity = 0f;
				_catchResidual = 0f;
				_wasControlled = false;
				_wasAtBottom = _shaftT <= 0f;
				ResumeActiveCall();
			}
			_wasAuthority = flag;
			if (flag && !_volume.NetworkedCarrierPathAnchorSet)
			{
				_volume.NetworkedCarrierPathAnchor = ShaftBasePosition;
				_volume.NetworkedCarrierPathAnchorSet = true;
			}
			if (_deckBody == null || _crank == null)
			{
				_driveActive = false;
				_deckWorldVelocity = Vector3.zero;
				return;
			}
			if (_crank.Object == null || !_crank.Object.IsValid)
			{
				_driveActive = false;
				_deckWorldVelocity = Vector3.zero;
				return;
			}
			UpdateCrankTravelLimits();
			UpdateCrankMirrorSpin();
			if (!flag)
			{
				_driveActive = false;
				_deckWorldVelocity = Vector3.zero;
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			bool flag2 = _brakeLever != null && _brakeLever.Object != null && _brakeLever.Object.IsValid && _brakeLever.CurrentState == _brakeEngagedState;
			bool isGrabbed = _crank.IsGrabbed;
			UpdateActiveCall();
			LiftMode mode = _mode;
			if (_activeCall != -1)
			{
				_mode = LiftMode.Called;
				_releaseTimer = _releaseGraceSeconds;
			}
			else if (flag2)
			{
				_mode = LiftMode.Braked;
				_releaseTimer = _releaseGraceSeconds;
			}
			else if (isGrabbed)
			{
				_mode = LiftMode.Controlled;
				_releaseTimer = _releaseGraceSeconds;
			}
			else if (_autoDriveVelocity != 0f)
			{
				_mode = LiftMode.Driven;
				_releaseTimer = _releaseGraceSeconds;
			}
			else if (_releaseTimer > 0f)
			{
				_mode = LiftMode.Releasing;
			}
			else
			{
				_mode = LiftMode.Falling;
			}
			if (_mode == LiftMode.Controlled && mode != LiftMode.Controlled)
			{
				_catchResidual = _deckVelocity;
			}
			switch (_mode)
			{
			case LiftMode.Braked:
				_deckVelocity = Mathf.MoveTowards(_deckVelocity, 0f, _brakeDeceleration * fixedDeltaTime);
				break;
			case LiftMode.Releasing:
				_releaseTimer -= fixedDeltaTime;
				_deckVelocity = Mathf.MoveTowards(_deckVelocity, 0f, _brakeDeceleration * fixedDeltaTime);
				break;
			case LiftMode.Falling:
			{
				if (_shaftT <= 0f)
				{
					_deckVelocity = 0f;
					break;
				}
				float t = ((_maxWeight > 0f) ? Mathf.Clamp01(_volume.SettledWeight / _maxWeight) : 0f);
				float num = Mathf.Lerp(_fallAcceleration, _ladenFallAcceleration, t);
				_deckVelocity = Mathf.Max(_deckVelocity - num * fixedDeltaTime, 0f - _maxFallSpeed);
				break;
			}
			case LiftMode.Controlled:
				if (!_wasControlled)
				{
					AnchorAngleDrive();
				}
				_catchResidual = Mathf.MoveTowards(_catchResidual, 0f, _catchDeceleration * fixedDeltaTime);
				break;
			case LiftMode.Called:
				_deckVelocity = ((CalledShaftT() > _shaftT) ? _callDriveSpeed : (0f - _callDriveSpeed));
				break;
			case LiftMode.Driven:
				_deckVelocity = _autoDriveVelocity;
				break;
			}
			_wasControlled = _mode == LiftMode.Controlled;
			_crank.IsLocked = _mode == LiftMode.Braked;
			_crank.ExternalDriveDegreesPerSecond = ((_mode == LiftMode.Controlled) ? 0f : (_deckVelocity * _crankDegreesPerMeter));
			_driveActive = true;
		}

		private void Update()
		{
			if (!_driveActive || _deckBody == null || _crank == null)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			float num = Mathf.Max(0.0001f, ShaftHeight);
			float shaftT = _shaftT;
			if (_mode == LiftMode.Controlled)
			{
				_residualOffsetT += _catchResidual * deltaTime / num;
				float target = AngleDriveShaftT(num) + _residualOffsetT;
				float maxDelta = (_crank.MaxTurnDegreesPerSecond / Mathf.Max(0.0001f, _crankDegreesPerMeter) + Mathf.Abs(_catchResidual)) * deltaTime / num;
				_shaftT = Mathf.MoveTowards(_shaftT, target, maxDelta);
			}
			else
			{
				_shaftT += _deckVelocity * deltaTime / num;
			}
			bool flag = ((_mode == LiftMode.Controlled) ? (_shaftT < shaftT) : (_deckVelocity < 0f));
			bool flag2 = ((_mode == LiftMode.Controlled) ? (_shaftT > shaftT) : (_deckVelocity > 0f));
			bool flag3 = false;
			bool flag4 = false;
			if (_activeCall != -1)
			{
				float num2 = CalledShaftT();
				float num3 = 0.01f / num;
				if ((shaftT - num2) * (_shaftT - num2) <= 0f || Mathf.Abs(_shaftT - num2) <= num3)
				{
					_shaftT = num2;
					_deckVelocity = 0f;
					flag4 = true;
				}
			}
			if (_shaftT <= 0f && flag)
			{
				flag3 = _mode == LiftMode.Falling && !_wasAtBottom && 0f - _deckVelocity >= _crashImpactSpeed;
				_shaftT = 0f;
				_deckVelocity = 0f;
				ReanchorAngleDriveAtStop();
			}
			else if (_shaftT >= 1f && flag2)
			{
				_shaftT = 1f;
				_deckVelocity = 0f;
				ReanchorAngleDriveAtStop();
			}
			_shaftT = Mathf.Clamp01(_shaftT);
			if (flag4)
			{
				ArriveAtCall();
			}
			if (_mode == LiftMode.Controlled)
			{
				_deckVelocity = ((deltaTime > 0f) ? ((_shaftT - shaftT) * num / deltaTime) : 0f);
			}
			_wasAtBottom = _shaftT <= 0f;
			_deckWorldVelocity = ((deltaTime > 0f) ? (Vector3.up * ((_shaftT - shaftT) * num / deltaTime)) : Vector3.zero);
			Vector3 shaftPoint = GetShaftPoint(_shaftT);
			_deckBody.transform.position = shaftPoint;
			_deckBody.position = shaftPoint;
			if (flag3 && _volume != null)
			{
				_volume.SlamRiders(_crashDamage, _crashRagdollRecoverySeconds);
				DamageCargo();
			}
		}

		private void UpdateActiveCall()
		{
			for (int i = 0; i < _callStops.Count; i++)
			{
				LiftCallLever liftCallLever = _callStops[i]?.Lever;
				if (liftCallLever != null && liftCallLever.ConsumeCall())
				{
					_activeCall = i;
				}
			}
			bool flag = _activeCall != -1;
			float num = Mathf.Max(0.0001f, ShaftHeight);
			foreach (LiftCallStop callStop in _callStops)
			{
				if (!(callStop?.Lever == null))
				{
					bool flag2 = Mathf.Abs(_shaftT - StopShaftT(callStop)) * num <= 0.15f;
					callStop.Lever.SetPullBlocked(flag || flag2);
				}
			}
		}

		private void ResumeActiveCall()
		{
			_activeCall = -1;
			foreach (LiftCallStop callStop in _callStops)
			{
				callStop?.Lever?.DiscardPendingCall();
				callStop?.Lever?.SetPullBlocked(blocked: false);
			}
		}

		private void ArriveAtCall()
		{
			_activeCall = -1;
			_deckVelocity = 0f;
			if (_brakeLever != null && _brakeLever.Object != null && _brakeLever.Object.IsValid)
			{
				_brakeLever.SetState(_brakeEngagedState);
			}
		}

		private void UpdateCrankMirrorSpin()
		{
			if (!(_volume == null))
			{
				float y = _volume.DrivenCarrierVelocity.y;
				_crank.ExternalDriveDegreesPerSecond = (_crank.IsGrabbed ? 0f : (y * _crankDegreesPerMeter));
			}
		}

		private void AnchorAngleDrive()
		{
			_driveAnchorAngle = _crank.RenderAngle;
			_driveAnchorT = _shaftT;
			_residualOffsetT = 0f;
		}

		private float AngleDriveShaftT(float shaftHeight)
		{
			return _driveAnchorT + (_crank.RenderAngle - _driveAnchorAngle) / (Mathf.Max(0.0001f, _crankDegreesPerMeter) * shaftHeight);
		}

		private void ReanchorAngleDriveAtStop()
		{
			if (_mode == LiftMode.Controlled)
			{
				_catchResidual = 0f;
				AnchorAngleDrive();
			}
		}

		private float ProjectShaftT(Vector3 worldPosition)
		{
			Vector3 rhs = TopPoint - BottomPoint;
			float sqrMagnitude = rhs.sqrMagnitude;
			if (sqrMagnitude < 1E-06f)
			{
				return 0f;
			}
			return Mathf.Clamp01(Vector3.Dot(worldPosition - BottomPoint, rhs) / sqrMagnitude);
		}

		private void UpdateCrankTravelLimits()
		{
			float num = ProjectShaftT(_deckBody.position);
			float angle = _crank.Angle;
			bool flag = num <= 0.004f;
			bool flag2 = num >= 0.996f;
			if (flag && !_wasLimitAtBottom)
			{
				_bottomStopAngle = angle;
			}
			if (flag2 && !_wasLimitAtTop)
			{
				_topStopAngle = angle;
			}
			_wasLimitAtBottom = flag;
			_wasLimitAtTop = flag2;
			_crank.UseAngleLimits = true;
			_crank.MinAngleDeg = (flag ? _bottomStopAngle : (angle - num * ShaftHeight * _crankDegreesPerMeter));
			_crank.MaxAngleDeg = (flag2 ? _topStopAngle : (angle + (1f - num) * ShaftHeight * _crankDegreesPerMeter));
		}

		private void DamageCargo()
		{
			if (_itemCostReduceService == null || _crashItemDamage <= 0f || _volume == null)
			{
				return;
			}
			_cargoScratch.Clear();
			_cargoScratch.AddRange(_volume.TrackedCargoBodies);
			foreach (Rigidbody item2 in _cargoScratch)
			{
				if (item2 == null)
				{
					continue;
				}
				IItem component;
				IItem item = (item2.TryGetComponent<IItem>(out component) ? component : item2.GetComponentInParent<IItem>());
				if (item != null)
				{
					Collider componentInChildren = item2.GetComponentInChildren<Collider>();
					if (!(componentInChildren == null))
					{
						_itemCostReduceService.ProcessItemCollisionData(new ItemCollisionData(item, _crashItemDamage, item2.mass, componentInChildren), null, isIgnoreLimits: true);
					}
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			Vector3 bottomPoint = BottomPoint;
			Vector3 topPoint = TopPoint;
			Gizmos.color = new Color(1f, 0.85f, 0.2f, 1f);
			Gizmos.DrawLine(bottomPoint, topPoint);
			Gizmos.DrawSphere(bottomPoint, 0.08f);
			Gizmos.DrawSphere(topPoint, 0.08f);
			Gizmos.color = new Color(0.35f, 0.9f, 1f, 1f);
			foreach (LiftCallStop callStop in _callStops)
			{
				if (callStop != null)
				{
					Vector3 shaftPoint = GetShaftPoint(StopShaftT(callStop));
					Gizmos.DrawSphere(shaftPoint, 0.12f);
					if (callStop.Lever != null)
					{
						Gizmos.DrawLine(shaftPoint, callStop.Lever.transform.position);
					}
				}
			}
		}
	}
}
