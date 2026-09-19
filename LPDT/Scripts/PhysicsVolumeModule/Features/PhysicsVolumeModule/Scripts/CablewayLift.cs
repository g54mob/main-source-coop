using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[DefaultExecutionOrder(-50)]
	public class CablewayLift : MonoBehaviour
	{
		private const int CRANK_FILTER_WINDOW = 5;

		private const float LIMIT_END_METERS = 0.05f;

		private const float LIMIT_END_EXIT_METERS = 0.25f;

		[SerializeField]
		private PhysicsInfluenceVolume _volume;

		private readonly HashSet<PhysicsInfluenceVolume> _hauledCarriers = new HashSet<PhysicsInfluenceVolume>();

		[Header("Rope path")]
		[Tooltip("Rope anchor the cabin departs from — the station end, where the shore crank stands.")]
		[SerializeField]
		private Transform _pointA;

		[Tooltip("Rope anchor the cabin travels to.")]
		[SerializeField]
		private Transform _pointB;

		[Tooltip("How deep the rope sags below the straight A-B line at mid-span, in metres. The cabin follows the sag.")]
		[SerializeField]
		private float _ropeSag = 0.8f;

		[Header("Cranks")]
		[Tooltip("The crank aboard the cabin.")]
		[SerializeField]
		private CrankRotator _cabinCrank;

		[Tooltip("Shore/station cranks (e.g. at PointA) — any one can rope the cabin back; grabbing two or more cranks (cabin or station) locks them all.")]
		[SerializeField]
		private CrankRotator[] _stationCranks;

		[Tooltip("Flip every station crank's winding direction (a station winch reeling IN pulls the cabin toward A).")]
		[SerializeField]
		private bool _invertStationCrank = true;

		[Tooltip("Crank degrees per metre of cabin travel — the winch ratio for all cranks.")]
		[SerializeField]
		private float _crankDegreesPerMeter = 360f;

		[Header("Motion")]
		[Tooltip("How hard the cabin bleeds to a stop when no hand drives it (m/s^2).")]
		[SerializeField]
		private float _coastDeceleration = 2f;

		[Tooltip("How hard the fighting winches bleed the cabin to a halt when two or more cranks are grabbed (m/s^2).")]
		[SerializeField]
		private float _stuckDeceleration = 12f;

		[Tooltip("How hard the cabin eases from coasting momentum toward the crank command when caught (m/s^2).")]
		[SerializeField]
		private float _catchDeceleration = 6f;

		[Tooltip("Rope-pull propagation lag (seconds): crank turns accumulate as pending rope pull and the cabin eases into it, the way a real pull propagates through the winch and rope instead of moving the cabin the same frame.")]
		[SerializeField]
		private float _driveResponseTau = 0.25f;

		[Tooltip("Outlier ceiling on a single tick's crank turn, in degrees: a hand re-snapping on the physics handle can jump the networked angle far beyond any honest wind in one tick — turns above this are cut to the ceiling before they enter the winch.")]
		[SerializeField]
		private float _maxCrankDegreesPerTick = 12f;

		[Header("Hanging dynamics")]
		[Tooltip("The rope attach point the cabin swings from (e.g. the WinchAnchor at the hanger top). When set, the pendulum arm length is measured from its height above the cabin origin at startup — place THIS to define the rotation anchor. Unset = the fixed hanger length below.")]
		[SerializeField]
		private Transform _ropePivot;

		[Tooltip("Fallback pendulum arm length when no rope pivot is assigned: how far above the cabin origin the pivot hangs. Longer = slower, wider swings.")]
		[SerializeField]
		private float _hangerLength = 2f;

		[Tooltip("Pendulum gravity (m/s^2) — the restoring pull that swings the hanging cabin back under its pivot.")]
		[SerializeField]
		private float _pendulumGravity = 9.81f;

		[Tooltip("Pendulum damping (1/s) — how fast a swing dies out. Lower = the cabin keeps rocking longer.")]
		[SerializeField]
		private float _pendulumDamping = 0.7f;

		[Tooltip("Acceleration deadband (m/s^2): excitation below this is ignored, so ordinary uneven cranking never rocks the cabin (the stir's oscillation sits near the pendulum's resonance and would pump it up). Real stops decelerate far harder and pass through.")]
		[SerializeField]
		private float _accelDeadband = 1.5f;

		[Tooltip("How much of the cabin's own drive acceleration feeds the swing (1 = physical: a hard stop throws the full travel momentum into a forward swing).")]
		[SerializeField]
		private float _accelSwingCoupling = 1f;

		[Tooltip("Velocity low-pass (seconds) for the dynamics excitation. The crank command is recomputed per network tick and is jittery tick-to-tick; without smoothing every tick reads as a huge one-frame acceleration and the pendulum thrashes. Real stops persist across frames, so their momentum still transfers in full.")]
		[SerializeField]
		private float _accelSmoothingTau = 0.12f;

		[Tooltip("Hard cap on the swing angle, in degrees — the hanger's mechanical stop.")]
		[SerializeField]
		private float _maxSwingDegrees = 70f;

		[Tooltip("Rope-stretch bounce spring (1/s^2): hard stops slam the rope and the cabin dips and rebounds vertically.")]
		[SerializeField]
		private float _bounceStiffness = 40f;

		[Tooltip("Rope-stretch bounce damping (1/s).")]
		[SerializeField]
		private float _bounceDamping = 5f;

		[Tooltip("Fraction of the cabin's retained vertical momentum that feeds the rope-stretch bounce (1 = physical). Signed and slack-scaled: max at mid-span, zero at the taut anchors.")]
		[SerializeField]
		private float _bounceAccelCoupling = 1f;

		[Tooltip("Hard cap on the vertical bounce, in metres.")]
		[SerializeField]
		private float _maxBounce = 0.5f;

		private Rigidbody _cabinBody;

		private CablewayMode _mode;

		private float _pathT;

		private float _cabinVelocity;

		private float _autoDriveVelocity;

		private float _catchResidual;

		private float _lastCabinCrankAngle;

		private float _lastStationCrankAngle;

		private float _cabinStartStopAngle;

		private float _cabinEndStopAngle;

		private bool _isCabinStartLatched;

		private bool _isCabinEndLatched;

		private float _stationStartStopAngle;

		private float _stationEndStopAngle;

		private bool _isStationStartLatched;

		private bool _isStationEndLatched;

		private bool _isParkedAtStart;

		private bool _isParkedAtEnd;

		private float[] _lastStationCrankAngles = Array.Empty<float>();

		private bool _cranksBaselined;

		private readonly float[] _crankDeltaSamples = new float[5];

		private int _crankSampleCount;

		private int _lastDrivingCrankId = -1;

		private float _pendingTravel;

		private bool _wasAuthority;

		private bool _driveActive;

		private float _swingAngle;

		private float _swingVelocity;

		private float _bounceOffset;

		private float _bounceVelocity;

		private float _smoothedVelocity;

		private float _lastSmoothedVelocity;

		private Quaternion _baseRotation = Quaternion.identity;

		private Transform _pathPointMarker;

		public PhysicsInfluenceVolume Volume => _volume;

		public CablewayMode Mode => _mode;

		public Vector3 PathStart
		{
			get
			{
				if (!(_pointA != null))
				{
					return base.transform.position;
				}
				return _pointA.position;
			}
		}

		public Vector3 PathEnd
		{
			get
			{
				if (!(_pointB != null))
				{
					return base.transform.position;
				}
				return _pointB.position;
			}
		}

		public float PathLength => Vector3.Distance(PathStart, PathEnd);

		public Vector3 CabinPosition
		{
			get
			{
				if (!(_cabinBody != null))
				{
					return base.transform.position;
				}
				return _cabinBody.position;
			}
		}

		public float AutoDriveVelocity => _autoDriveVelocity;

		public Vector3 GetPathPoint(float t)
		{
			t = Mathf.Clamp01(t);
			float num = _ropeSag * 4f * t * (1f - t);
			return Vector3.Lerp(PathStart, PathEnd, t) + Vector3.down * num;
		}

		public void SetAutoDrive(float metersPerSecond)
		{
			_autoDriveVelocity = metersPerSecond;
		}

		private void Awake()
		{
			_cabinBody = ((_volume != null) ? _volume.GetComponent<Rigidbody>() : GetComponent<Rigidbody>());
			if (_cabinBody != null)
			{
				_baseRotation = _cabinBody.transform.rotation;
				if (_ropePivot != null)
				{
					_hangerLength = Mathf.Max(0.1f, _ropePivot.position.y - _cabinBody.transform.position.y);
				}
			}
		}

		private void UpdateHauledCarriers()
		{
			if (_volume == null)
			{
				return;
			}
			foreach (Rigidbody trackedCargoBody in _volume.TrackedCargoBodies)
			{
				if (!(trackedCargoBody == null))
				{
					PhysicsInfluenceVolume componentInChildren = trackedCargoBody.GetComponentInChildren<PhysicsInfluenceVolume>();
					if (!(componentInChildren == null) && !(componentInChildren == _volume) && !_hauledCarriers.Contains(componentInChildren))
					{
						componentInChildren.SetCargoClusterMode(enabled: true);
						_hauledCarriers.Add(componentInChildren);
					}
				}
			}
		}

		private void OnDisable()
		{
			foreach (PhysicsInfluenceVolume hauledCarrier in _hauledCarriers)
			{
				if (hauledCarrier != null)
				{
					hauledCarrier.SetCargoClusterMode(enabled: false);
				}
			}
			_hauledCarriers.Clear();
		}

		private void Start()
		{
			if (_cabinBody != null)
			{
				_pathT = ProjectPathT(_cabinBody.position);
			}
		}

		private void FixedUpdate()
		{
			bool flag = _volume != null && _volume.Object != null && _volume.Object.HasStateAuthority;
			UpdateHauledCarriers();
			if (flag && !_wasAuthority)
			{
				if (_cabinBody != null)
				{
					_pathT = ProjectPathT(_cabinBody.position);
				}
				_cabinVelocity = 0f;
				_cranksBaselined = false;
				_pendingTravel = 0f;
				_crankSampleCount = 0;
				_lastDrivingCrankId = -1;
				_swingAngle = 0f;
				_swingVelocity = 0f;
				_bounceOffset = 0f;
				_bounceVelocity = 0f;
				_smoothedVelocity = 0f;
				_lastSmoothedVelocity = 0f;
			}
			_wasAuthority = flag;
			UpdateCrankTravelLimits(flag);
			if (!flag || _cabinBody == null)
			{
				_driveActive = false;
				return;
			}
			bool flag2 = IsCrankReady(_cabinCrank);
			EnsureStationAngleBuffer();
			int num = ((_stationCranks != null) ? _stationCranks.Length : 0);
			bool flag3 = flag2;
			bool flag4 = flag2 && _cabinCrank.IsGrabbed;
			int num2 = (flag4 ? 1 : 0);
			int num3 = -1;
			for (int i = 0; i < num; i++)
			{
				if (!IsCrankReady(_stationCranks[i]))
				{
					continue;
				}
				flag3 = true;
				if (_stationCranks[i].IsGrabbed)
				{
					num2++;
					if (num3 < 0)
					{
						num3 = i;
					}
				}
			}
			if (!flag3)
			{
				_driveActive = false;
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			CablewayMode mode = _mode;
			if (num2 >= 2)
			{
				_mode = CablewayMode.Stuck;
			}
			else if (num2 == 1)
			{
				_mode = CablewayMode.Controlled;
			}
			else if (_autoDriveVelocity != 0f)
			{
				_mode = CablewayMode.Driven;
			}
			else
			{
				_mode = CablewayMode.Idle;
			}
			if (_mode == CablewayMode.Controlled && mode != CablewayMode.Controlled)
			{
				_catchResidual = _cabinVelocity;
				_crankSampleCount = 0;
			}
			bool flag5 = _mode == CablewayMode.Controlled && flag4;
			bool flag6 = _mode == CablewayMode.Controlled && !flag4 && num3 >= 0;
			int num4 = (flag6 ? num3 : (-1));
			int num5 = ((!flag5) ? (flag6 ? (1 + num4) : (-1)) : 0);
			if (!_cranksBaselined || !flag5)
			{
				_lastCabinCrankAngle = (flag2 ? _cabinCrank.Angle : 0f);
			}
			for (int j = 0; j < num; j++)
			{
				bool flag7 = flag6 && j == num4;
				if (!_cranksBaselined || !flag7)
				{
					_lastStationCrankAngles[j] = (IsCrankReady(_stationCranks[j]) ? _stationCranks[j].Angle : 0f);
				}
			}
			_cranksBaselined = true;
			switch (_mode)
			{
			case CablewayMode.Idle:
				_cabinVelocity = ((_pendingTravel != 0f) ? DrainPendingTravel(fixedDeltaTime) : Mathf.MoveTowards(_cabinVelocity, 0f, _coastDeceleration * fixedDeltaTime));
				break;
			case CablewayMode.Stuck:
				_pendingTravel = 0f;
				_cabinVelocity = Mathf.MoveTowards(_cabinVelocity, 0f, _stuckDeceleration * fixedDeltaTime);
				break;
			case CablewayMode.Controlled:
			{
				float delta;
				if (flag5)
				{
					delta = _cabinCrank.Angle - _lastCabinCrankAngle;
					_lastCabinCrankAngle = _cabinCrank.Angle;
				}
				else
				{
					CrankRotator crankRotator = _stationCranks[num4];
					delta = (crankRotator.Angle - _lastStationCrankAngles[num4]) * (_invertStationCrank ? (-1f) : 1f);
					_lastStationCrankAngles[num4] = crankRotator.Angle;
				}
				if (num5 != _lastDrivingCrankId)
				{
					_crankSampleCount = 0;
				}
				_lastDrivingCrankId = num5;
				_pendingTravel += FilterCrankDelta(delta) / Mathf.Max(0.0001f, _crankDegreesPerMeter);
				_catchResidual = Mathf.MoveTowards(_catchResidual, 0f, _catchDeceleration * fixedDeltaTime);
				_cabinVelocity = DrainPendingTravel(fixedDeltaTime) + _catchResidual;
				break;
			}
			case CablewayMode.Driven:
				_cabinVelocity = _autoDriveVelocity;
				break;
			}
			if (flag2)
			{
				_cabinCrank.IsLocked = _mode == CablewayMode.Stuck;
			}
			for (int k = 0; k < num; k++)
			{
				if (IsCrankReady(_stationCranks[k]))
				{
					_stationCranks[k].IsLocked = _mode == CablewayMode.Stuck;
				}
			}
			float num6 = _cabinVelocity * _crankDegreesPerMeter;
			float num7 = num6 * (_invertStationCrank ? (-1f) : 1f);
			if (flag2)
			{
				_cabinCrank.ExternalDriveDegreesPerSecond = (flag5 ? 0f : num6);
			}
			for (int l = 0; l < num; l++)
			{
				if (IsCrankReady(_stationCranks[l]))
				{
					bool flag8 = flag6 && l == num4;
					_stationCranks[l].ExternalDriveDegreesPerSecond = (flag8 ? 0f : num7);
				}
			}
			_driveActive = true;
		}

		private void Update()
		{
			if (_driveActive && !(_cabinBody == null))
			{
				float deltaTime = Time.deltaTime;
				float num = Mathf.Max(0.0001f, PathLength);
				_pathT += _cabinVelocity * deltaTime / num;
				if (_pathT <= 0f && _cabinVelocity < 0f)
				{
					_pathT = 0f;
					_cabinVelocity = 0f;
					_pendingTravel = Mathf.Max(0f, _pendingTravel);
				}
				else if (_pathT >= 1f && _cabinVelocity > 0f)
				{
					_pathT = 1f;
					_cabinVelocity = 0f;
					_pendingTravel = Mathf.Min(0f, _pendingTravel);
				}
				_pathT = Mathf.Clamp01(_pathT);
				if (_pathPointMarker == null)
				{
					GameObject gameObject = new GameObject("CablewayPathPoint");
					gameObject.transform.SetParent(base.transform, worldPositionStays: false);
					_pathPointMarker = gameObject.transform;
				}
				_pathPointMarker.position = GetPathPoint(_pathT);
				float t = 1f - Mathf.Exp((0f - deltaTime) / Mathf.Max(0.01f, _accelSmoothingTau));
				_smoothedVelocity = Mathf.Lerp(_smoothedVelocity, _cabinVelocity, t);
				float value = ((deltaTime > 0f) ? ((_smoothedVelocity - _lastSmoothedVelocity) / deltaTime) : 0f);
				_lastSmoothedVelocity = _smoothedVelocity;
				value = Mathf.Clamp(value, -25f, 25f);
				value = Mathf.Sign(value) * Mathf.Max(0f, Mathf.Abs(value) - _accelDeadband);
				float num2 = Mathf.Max(0.1f, _hangerLength);
				float num3 = ((0f - _pendulumGravity) * Mathf.Sin(_swingAngle) + _accelSwingCoupling * value * Mathf.Cos(_swingAngle)) / num2 - _pendulumDamping * _swingVelocity;
				_swingVelocity += num3 * deltaTime;
				_swingAngle += _swingVelocity * deltaTime;
				float num4 = _maxSwingDegrees * (MathF.PI / 180f);
				if (Mathf.Abs(_swingAngle) > num4)
				{
					_swingAngle = Mathf.Clamp(_swingAngle, 0f - num4, num4);
					_swingVelocity = 0f;
				}
				float num5 = (PathEnd.y - PathStart.y - 4f * _ropeSag * (1f - 2f * _pathT)) / num;
				float num6 = 4f * _pathT * (1f - _pathT);
				_bounceVelocity += ((0f - _bounceStiffness) * _bounceOffset - _bounceDamping * _bounceVelocity + value * num5 * _bounceAccelCoupling * num6) * deltaTime;
				_bounceOffset = Mathf.Clamp(_bounceOffset + _bounceVelocity * deltaTime, 0f - _maxBounce, _maxBounce);
				Vector3 vector = PathEnd - PathStart;
				vector.y = 0f;
				vector = ((vector.sqrMagnitude > 1E-06f) ? vector.normalized : Vector3.forward);
				Vector3 axis = Vector3.Cross(Vector3.up, vector);
				Quaternion quaternion = Quaternion.AngleAxis(_swingAngle * 57.29578f, axis);
				Vector3 position = GetPathPoint(_pathT) + Vector3.up * num2 + quaternion * (Vector3.down * (num2 + _bounceOffset));
				Quaternion rotation = quaternion * _baseRotation;
				_cabinBody.transform.position = position;
				_cabinBody.transform.rotation = rotation;
				_cabinBody.position = position;
				_cabinBody.rotation = rotation;
			}
		}

		private float FilterCrankDelta(float delta)
		{
			delta = Mathf.Clamp(delta, 0f - _maxCrankDegreesPerTick, _maxCrankDegreesPerTick);
			if (_crankSampleCount < 5)
			{
				_crankDeltaSamples[_crankSampleCount] = delta;
				_crankSampleCount++;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					_crankDeltaSamples[i] = _crankDeltaSamples[i + 1];
				}
				_crankDeltaSamples[4] = delta;
			}
			float num = 0f;
			for (int j = 0; j < _crankSampleCount; j++)
			{
				num += _crankDeltaSamples[j];
			}
			return num / 5f;
		}

		private float DrainPendingTravel(float dt)
		{
			if (dt <= 0f)
			{
				return 0f;
			}
			float num = _pendingTravel * (1f - Mathf.Exp((0f - dt) / Mathf.Max(0.01f, _driveResponseTau)));
			_pendingTravel -= num;
			return num / dt;
		}

		private void UpdateCrankTravelLimits(bool isAuthority)
		{
			if (_cabinBody == null)
			{
				return;
			}
			float pathLength = PathLength;
			float num;
			if (isAuthority)
			{
				num = _pathT;
				if (_volume != null && _volume.Object != null && _volume.Object.IsValid)
				{
					_volume.NetworkedCarrierPathT = num;
				}
			}
			else
			{
				num = ((_volume != null && _volume.Object != null && _volume.Object.IsValid) ? Mathf.Clamp01(_volume.NetworkedCarrierPathT) : _pathT);
			}
			float num2 = num * pathLength;
			float num3 = (1f - num) * pathLength;
			_isParkedAtStart = num2 <= (_isParkedAtStart ? 0.25f : 0.05f);
			_isParkedAtEnd = num3 <= (_isParkedAtEnd ? 0.25f : 0.05f);
			bool isParkedAtStart = _isParkedAtStart;
			bool isParkedAtEnd = _isParkedAtEnd;
			ApplyCrankTravelLimits(_cabinCrank, num, pathLength, isParkedAtStart, isParkedAtEnd, 1f, ref _cabinStartStopAngle, ref _cabinEndStopAngle, ref _isCabinStartLatched, ref _isCabinEndLatched);
			CrankRotator[] stationCranks = _stationCranks;
			foreach (CrankRotator crank in stationCranks)
			{
				ApplyCrankTravelLimits(crank, num, pathLength, isParkedAtStart, isParkedAtEnd, _invertStationCrank ? (-1f) : 1f, ref _stationStartStopAngle, ref _stationEndStopAngle, ref _isStationStartLatched, ref _isStationEndLatched);
			}
		}

		private void ApplyCrankTravelLimits(CrankRotator crank, float t, float length, bool atStart, bool atEnd, float towardEndSign, ref float startStopAngle, ref float endStopAngle, ref bool isStartLatched, ref bool isEndLatched)
		{
			if (IsCrankReady(crank))
			{
				float angle = crank.Angle;
				if (!atStart)
				{
					isStartLatched = false;
				}
				else if (!isStartLatched)
				{
					startStopAngle = angle;
					isStartLatched = true;
				}
				if (!atEnd)
				{
					isEndLatched = false;
				}
				else if (!isEndLatched)
				{
					endStopAngle = angle;
					isEndLatched = true;
				}
				float a = (atStart ? startStopAngle : (angle - towardEndSign * t * length * _crankDegreesPerMeter));
				float b = (atEnd ? endStopAngle : (angle + towardEndSign * (1f - t) * length * _crankDegreesPerMeter));
				crank.UseAngleLimits = true;
				crank.MinAngleDeg = Mathf.Min(a, b);
				crank.MaxAngleDeg = Mathf.Max(a, b);
			}
		}

		private static bool IsCrankReady(CrankRotator crank)
		{
			if (crank != null && crank.Object != null)
			{
				return crank.Object.IsValid;
			}
			return false;
		}

		private void EnsureStationAngleBuffer()
		{
			int num = ((_stationCranks != null) ? _stationCranks.Length : 0);
			if (_lastStationCrankAngles.Length != num)
			{
				_lastStationCrankAngles = ((num == 0) ? Array.Empty<float>() : new float[num]);
				_cranksBaselined = false;
			}
		}

		private float ProjectPathT(Vector3 worldPosition)
		{
			Vector3 rhs = PathEnd - PathStart;
			float sqrMagnitude = rhs.sqrMagnitude;
			if (sqrMagnitude < 1E-06f)
			{
				return 0f;
			}
			return Mathf.Clamp01(Vector3.Dot(worldPosition - PathStart, rhs) / sqrMagnitude);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(1f, 0.85f, 0.2f, 1f);
			Vector3 vector = GetPathPoint(0f);
			for (int i = 1; i <= 24; i++)
			{
				Vector3 pathPoint = GetPathPoint((float)i / 24f);
				Gizmos.DrawLine(vector, pathPoint);
				vector = pathPoint;
			}
			Gizmos.DrawSphere(PathStart, 0.08f);
			Gizmos.DrawSphere(PathEnd, 0.08f);
		}
	}
}
