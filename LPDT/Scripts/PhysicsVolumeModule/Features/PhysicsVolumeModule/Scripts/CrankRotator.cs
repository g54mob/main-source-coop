using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.PhysicsVolumeModule.Scripts
{
	[NetworkBehaviourWeaved(11)]
	public class CrankRotator : NetworkBehaviour, IVolumeUncapturable
	{
		private const float RENDER_SMOOTHING = 14f;

		private const float RENDER_CATCHUP_HEADROOM = 1.05f;

		private const float RENDER_MIN_STEP_DEG = 0.35f;

		private const float RENDER_SNAP_DEG = 120f;

		private const float RENDER_SOURCE_STOPPED_DEG_PER_SEC = 5f;

		private const float HAND_SPEED_TAU = 0.4f;

		private const float INERTIA_STOP_DEG_PER_SEC = 8f;

		private const float STRAIN_WIGGLE_SECONDS = 0.45f;

		private const float VIRTUAL_PULLER_MIN_LENGTH_DEG = 1f;

		private const float SWIPE_STRAIN_DECAY_DEG_PER_SEC = 30f;

		private const float SCREEN_TANGENT_MAX_LEAD_DEG = 45f;

		private const float SCREEN_TANGENT_LEAD_DECAY_TAU = 0.25f;

		private const float SCREEN_TANGENT_ALIGN_TAU = 0.15f;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[Tooltip("Carry spin after release: the hand's recent turn rate keeps driving the crank, bled off by drag.")]
		[SerializeField]
		private bool _inertiaEnabled;

		[Tooltip("Exponential decay rate of the carried spin (per second) — higher stops the freewheel sooner.")]
		[SerializeField]
		private float _inertiaDrag = 1.2f;

		[Tooltip("Cap on the carried spin (deg/s) a release can inherit.")]
		[SerializeField]
		private float _inertiaMaxDegreesPerSecond = 540f;

		[Tooltip("Foreign colliders never block the crank: contact generation is excluded on every crank collider (grab raycasts are queries and still hit), so a prop or body sitting in the swept arc cannot jam the wind.")]
		[SerializeField]
		private bool _collidersDoNotBlock = true;

		[Tooltip("Hard physical cap on the grabbed turn rate (deg/s) — the winch gearing. A lagging hand that catches up cannot whip the wheel through an arc in one burst; consumers see a rate-limited Angle and never need spike filtering of their own.")]
		[SerializeField]
		private float _maxTurnDegreesPerSecond = 200f;

		[Tooltip("Optional hard travel stops on the accumulated Angle (deg): NOTHING moves the crank outside [min, max] — not the hand (either input mode), not the freewheel, not an external drive or angle-follow. The freewheel dies on the stop instead of pushing through it.")]
		[SerializeField]
		private bool _useAngleLimits;

		[SerializeField]
		private float _minAngleDeg;

		[SerializeField]
		private float _maxAngleDeg = 1440f;

		[Tooltip("Spawn with the lock engaged.")]
		[SerializeField]
		private bool _startLocked;

		[Tooltip("Hand pull (m between where the hand wants the handle and where the handle is) that counts as a strain attempt on a locked crank.")]
		[SerializeField]
		private float _strainTriggerMeters = 0.35f;

		[Tooltip("Seconds between strain signals while the pull is held.")]
		[SerializeField]
		private float _strainCooldownSeconds = 1.5f;

		[Tooltip("Visual shudder amplitude (deg) played on every peer when a strain attempt fires; 0 disables.")]
		[SerializeField]
		private float _strainWiggleDeg = 2f;

		[Tooltip("While the LOCAL player holds this crank, the rendered camera locks onto the crank base while mouse-look keeps steering the hands (the grab-focus virtual aim).")]
		[SerializeField]
		private bool _focusCameraOnGrab = true;

		[Tooltip("How mouse input turns the grabbed crank: AimOrbit — the virtual aim steers the hands and the puller's orbit around the shaft is the turn input (legacy); TangentialSwipe — the aim is pinned to the crank and raw mouse swipes, projected into the wheel plane, push a free virtual hand whose angle the crank follows; ScreenTangent — the pushed hand is ANCHORED to the real handle within a small decaying lead band, so a fresh stroke maps exactly to the handle's on-screen tangent while fast circling still winds without micro-reversals.")]
		[SerializeField]
		private CrankInputMode _grabInputMode = CrankInputMode.TangentialSwipe;

		[Tooltip("Swipe mode: radius (in swipe degrees) of the virtual hand the swipes push around the wheel — the crank follows that hand's angle 1:1, exactly like the real puller drive. Smaller = more crank turn per swipe; a circular swipe of this radius maps to exactly one crank revolution per swipe circle.")]
		[SerializeField]
		private float _swipeVirtualRadiusDeg = 35f;

		[Tooltip("Swipe mode: accumulated screen-swipe degrees against the locked crank that count as one strain attempt.")]
		[SerializeField]
		private float _strainSwipeTriggerDegrees = 20f;

		[Tooltip("ScreenTangent only: turn-rate cap (deg/s) earned by swiping exactly along the handle's on-screen tangent. The live cap lerps from the base winch cap up to this as the (smoothed) alignment approaches 1 — match the motion and you wind this fast; never below the base cap.")]
		[SerializeField]
		private float _screenTangentAlignedMaxDegreesPerSecond = 400f;

		[Tooltip("ScreenTangent only: strictness of the direction match. The swipe-vs-tangent alignment (0..1) is raised to this power before it scales the applied turn and the rate cap: 1 = lenient linear falloff, higher = off-tangent swiping bleeds force fast and only a close match earns the boosted speed.")]
		[SerializeField]
		private float _screenTangentAlignmentPower = 2f;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Angle", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Angle;

		[WeaverGenerated]
		[DefaultForProperty("IsLockedInternal", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsLockedInternal;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("StrainCount", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _StrainCount;

		[WeaverGenerated]
		[DefaultForProperty("SpawnPosition", 3, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _SpawnPosition;

		[WeaverGenerated]
		[DefaultForProperty("SpawnRotation", 6, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Quaternion _SpawnRotation;

		[WeaverGenerated]
		[DefaultForProperty("HasSpawnPose", 10, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasSpawnPose;

		private const float SPEED_WINDOW_SECONDS = 0.35f;

		private float _renderAngle;

		private float _lastReportedAngle;

		private float _speedAnchorAngle;

		private float _speedAnchorTime;

		private float _windowBaselineSpeed;

		private float _handDegreesPerSecond;

		private float _inertiaDegreesPerSecond;

		private bool _wasGrabbed;

		private float _lastStrainTime = float.NegativeInfinity;

		private float _wiggleEndTime = float.NegativeInfinity;

		private Quaternion _baseLocalRotation = Quaternion.identity;

		private bool _spawnPoseApplied;

		private float _lastPullerAngle;

		private bool _hasPullerAngle;

		private int _seenStrainCount;

		private float _lockedSwipeAccumulatedDeg;

		private Vector2 _virtualPullerDirection = Vector2.right;

		private float _lastVirtualPullerAngle;

		private bool _hasVirtualPuller;

		private float _screenTangentLeadDeg;

		private float _screenTangentAlignment;

		private ICameraGrabFocusService _cameraGrabFocusService;

		private bool _focusEngaged;

		private const float REANCHOR_EPSILON_SQR_M = 1E-06f;

		private Joint[] _worldJoints;

		private Vector3[] _worldJointAnchors;

		private Transform _mount;

		private Vector3 _restLocalPosition;

		private Quaternion _restLocalRotation;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe float Angle
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.Angle. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.Angle. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe bool IsLockedInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.IsLockedInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.IsLockedInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[OnChangedRender("OnStrainRender")]
		[NetworkedWeaved(2, 1)]
		public unsafe int StrainCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.StrainCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.StrainCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 3)]
		private unsafe Vector3 SpawnPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.SpawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.SpawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 4)]
		private unsafe Quaternion SpawnRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.SpawnRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Quaternion*)(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.SpawnRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Quaternion*)(Ptr + 6) = value;
			}
		}

		[Networked]
		[OnChangedRender("TryApplySpawnPose")]
		[NetworkedWeaved(10, 1)]
		private unsafe NetworkBool HasSpawnPose
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.HasSpawnPose. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 10);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CrankRotator.HasSpawnPose. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 10) = value;
			}
		}

		public bool IsGrabbed => _grabable.GrabObject.Grabbers.Count > 0;

		public CrankInputMode GrabInputMode
		{
			get
			{
				return _grabInputMode;
			}
			set
			{
				if (_grabInputMode == value)
				{
					return;
				}
				_grabInputMode = value;
				if (_focusEngaged)
				{
					_cameraGrabFocusService.Activate(() => base.transform.position, IsSwipeInputMode());
				}
			}
		}

		public float InertiaDegreesPerSecond => _inertiaDegreesPerSecond;

		public float RenderAngle => _renderAngle;

		public float MaxTurnDegreesPerSecond
		{
			get
			{
				if (_grabInputMode != CrankInputMode.ScreenTangent)
				{
					return _maxTurnDegreesPerSecond;
				}
				return Mathf.Max(_maxTurnDegreesPerSecond, _screenTangentAlignedMaxDegreesPerSecond);
			}
		}

		public bool UseAngleLimits
		{
			get
			{
				return _useAngleLimits;
			}
			set
			{
				_useAngleLimits = value;
			}
		}

		public float MinAngleDeg
		{
			get
			{
				return _minAngleDeg;
			}
			set
			{
				_minAngleDeg = value;
			}
		}

		public float MaxAngleDeg
		{
			get
			{
				return _maxAngleDeg;
			}
			set
			{
				_maxAngleDeg = value;
			}
		}

		public bool InertiaEnabled
		{
			get
			{
				return _inertiaEnabled;
			}
			set
			{
				_inertiaEnabled = value;
			}
		}

		public bool IsLocked
		{
			get
			{
				if (!(base.Object != null) || !base.Object.IsValid)
				{
					return _startLocked;
				}
				return IsLockedInternal;
			}
			set
			{
				if (!(base.Object == null) && base.Object.IsValid)
				{
					if (base.HasStateAuthority)
					{
						IsLockedInternal = value;
					}
					else if (IsLockedInternal != value)
					{
						SetLockedRpc(value);
					}
				}
			}
		}

		public float ExternalDriveDegreesPerSecond { get; set; }

		public float ScriptedHandDegreesPerSecond { get; set; }

		public event Action<float> OnAngleChanged;

		public event Action<int> OnLockedStrain;

		[Inject]
		public void InjectDependencies(ICameraGrabFocusService cameraGrabFocusService)
		{
			_cameraGrabFocusService = cameraGrabFocusService;
		}

		private void Awake()
		{
			_baseLocalRotation = base.transform.localRotation;
			_rigidbody.maxAngularVelocity = Mathf.Max(_maxTurnDegreesPerSecond, _screenTangentAlignedMaxDegreesPerSecond) * (MathF.PI / 180f);
			if (_collidersDoNotBlock)
			{
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].excludeLayers = -1;
				}
			}
			CaptureRestPose();
			ReanchorWorldJoints();
		}

		private void FixedUpdate()
		{
			ReanchorWorldJoints();
		}

		private void CaptureRestPose()
		{
			Transform transform = _rigidbody.transform;
			_mount = transform.parent;
			_restLocalPosition = transform.localPosition;
			_restLocalRotation = transform.localRotation;
			_worldJoints = Array.FindAll(_rigidbody.GetComponents<Joint>(), (Joint joint) => joint.connectedBody == null);
			_worldJointAnchors = new Vector3[_worldJoints.Length];
			for (int num = 0; num < _worldJoints.Length; num++)
			{
				_worldJoints[num].autoConfigureConnectedAnchor = false;
				_worldJointAnchors[num] = _worldJoints[num].connectedAnchor;
			}
		}

		private void ReanchorWorldJoints()
		{
			if (_worldJoints == null)
			{
				return;
			}
			Vector3 localScale = _rigidbody.transform.localScale;
			for (int i = 0; i < _worldJoints.Length; i++)
			{
				Joint joint = _worldJoints[i];
				Vector3 vector = _restLocalPosition + _restLocalRotation * Vector3.Scale(joint.anchor, localScale);
				Vector3 vector2 = ((_mount != null) ? _mount.TransformPoint(vector) : vector);
				if (!((vector2 - _worldJointAnchors[i]).sqrMagnitude < 1E-06f))
				{
					_worldJointAnchors[i] = vector2;
					joint.connectedAnchor = vector2;
				}
			}
		}

		public override void Spawned()
		{
			if (!base.Object.NetworkTypeId.IsSceneObject)
			{
				if (base.HasStateAuthority)
				{
					SpawnPosition = base.transform.position;
					SpawnRotation = base.transform.rotation;
					HasSpawnPose = true;
					_spawnPoseApplied = true;
				}
				else
				{
					TryApplySpawnPose();
				}
			}
			_renderAngle = Angle;
			_lastReportedAngle = Angle;
			_speedAnchorAngle = Angle;
			_speedAnchorTime = Time.time;
			_windowBaselineSpeed = 0f;
			_seenStrainCount = StrainCount;
			if (base.HasStateAuthority && _startLocked)
			{
				IsLockedInternal = true;
			}
			_grabable.LocalOnGrab += OnLocalGrab;
			_grabable.LocalOnUnGrab += OnLocalUnGrab;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_grabable.LocalOnGrab -= OnLocalGrab;
			_grabable.LocalOnUnGrab -= OnLocalUnGrab;
			DisengageFocus();
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			bool isGrabbed = IsGrabbed;
			if (!isGrabbed)
			{
				ScriptedHandDegreesPerSecond = 0f;
			}
			_rigidbody.isKinematic = true;
			if (isGrabbed && IsLocked)
			{
				_inertiaDegreesPerSecond = 0f;
				_handDegreesPerSecond = 0f;
				_hasPullerAngle = false;
				_hasVirtualPuller = false;
				_screenTangentLeadDeg = 0f;
				_screenTangentAlignment = 0f;
				if (UseSwipeDrive())
				{
					DetectSwipeStrain();
				}
				else
				{
					DetectStrain();
				}
			}
			else if (isGrabbed && ScriptedHandDegreesPerSecond != 0f)
			{
				float num = Mathf.Clamp(ScriptedHandDegreesPerSecond * base.Runner.DeltaTime, (0f - _maxTurnDegreesPerSecond) * base.Runner.DeltaTime, _maxTurnDegreesPerSecond * base.Runner.DeltaTime);
				float num2 = ClampToAngleLimits(Angle + num) - Angle;
				if (num2 != 0f)
				{
					Angle += num2;
					StampAngle();
				}
			}
			else if (isGrabbed && UseSwipeDrive() && _grabInputMode == CrankInputMode.ScreenTangent)
			{
				_lockedSwipeAccumulatedDeg = 0f;
				Vector2 vector = ProjectSwipeToWheelPlane(_cameraGrabFocusService.ConsumeLookDelta());
				if (TryGetHandleRadial(out var radial))
				{
					float f = _screenTangentLeadDeg * (MathF.PI / 180f);
					Vector2 vector2 = new Vector2(radial.x * Mathf.Cos(f) - radial.y * Mathf.Sin(f), radial.x * Mathf.Sin(f) + radial.y * Mathf.Cos(f));
					Vector2 vector3 = vector2 * _swipeVirtualRadiusDeg;
					Vector2 vector4 = vector3 + vector;
					float num3 = ((vector4.sqrMagnitude >= 1f) ? Mathf.DeltaAngle(Mathf.Atan2(vector3.y, vector3.x) * 57.29578f, Mathf.Atan2(vector4.y, vector4.x) * 57.29578f) : 0f);
					float p = Mathf.Max(0.01f, _screenTangentAlignmentPower);
					if (vector != Vector2.zero)
					{
						float num4 = Mathf.Pow(Mathf.Clamp01(Mathf.Abs(Vector2.Dot(rhs: new Vector2(0f - vector2.y, vector2.x), lhs: vector.normalized))), p);
						num3 *= num4;
						_screenTangentAlignment = Mathf.Lerp(_screenTangentAlignment, num4, 1f - Mathf.Exp((0f - base.Runner.DeltaTime) / 0.15f));
					}
					else
					{
						_screenTangentAlignment *= Mathf.Exp((0f - base.Runner.DeltaTime) / 0.15f);
					}
					float num5 = _screenTangentLeadDeg + num3;
					float b = Mathf.Max(_maxTurnDegreesPerSecond, _screenTangentAlignedMaxDegreesPerSecond);
					float num6 = Mathf.Lerp(_maxTurnDegreesPerSecond, b, _screenTangentAlignment) * base.Runner.DeltaTime;
					float num7 = Mathf.Clamp(num5, 0f - num6, num6);
					num7 = ClampToAngleLimits(Angle + num7) - Angle;
					if (num7 != 0f)
					{
						Angle += num7;
						StampAngle();
					}
					_screenTangentLeadDeg = Mathf.Clamp(num5 - num7, -45f, 45f);
					if (vector == Vector2.zero)
					{
						_screenTangentLeadDeg *= Mathf.Exp((0f - base.Runner.DeltaTime) / 0.25f);
					}
					_handDegreesPerSecond = Mathf.Lerp(_handDegreesPerSecond, num7 / base.Runner.DeltaTime, 1f - Mathf.Exp((0f - base.Runner.DeltaTime) / 0.4f));
				}
				_hasVirtualPuller = false;
				_hasPullerAngle = false;
				_inertiaDegreesPerSecond = 0f;
			}
			else if (isGrabbed && UseSwipeDrive())
			{
				_lockedSwipeAccumulatedDeg = 0f;
				Vector2 vector5 = _virtualPullerDirection * _swipeVirtualRadiusDeg + ProjectSwipeToWheelPlane(_cameraGrabFocusService.ConsumeLookDelta());
				if (vector5.sqrMagnitude >= 1f)
				{
					_virtualPullerDirection = vector5.normalized;
				}
				float num8 = Mathf.Atan2(_virtualPullerDirection.y, _virtualPullerDirection.x) * 57.29578f;
				if (_hasVirtualPuller)
				{
					float num9 = _maxTurnDegreesPerSecond * base.Runner.DeltaTime;
					float num10 = Mathf.Clamp(Mathf.DeltaAngle(_lastVirtualPullerAngle, num8), 0f - num9, num9);
					num10 = ClampToAngleLimits(Angle + num10) - Angle;
					if (num10 != 0f)
					{
						Angle += num10;
						StampAngle();
					}
					_handDegreesPerSecond = Mathf.Lerp(_handDegreesPerSecond, num10 / base.Runner.DeltaTime, 1f - Mathf.Exp((0f - base.Runner.DeltaTime) / 0.4f));
				}
				_lastVirtualPullerAngle = num8;
				_hasVirtualPuller = true;
				_hasPullerAngle = false;
				_inertiaDegreesPerSecond = 0f;
			}
			else if (isGrabbed)
			{
				if (TryGetPullerSpinAngle(out var pullerAngle, out var _))
				{
					if (_hasPullerAngle)
					{
						float num11 = _maxTurnDegreesPerSecond * base.Runner.DeltaTime;
						float num12 = Mathf.Clamp(Mathf.DeltaAngle(_lastPullerAngle, pullerAngle), 0f - num11, num11);
						num12 = ClampToAngleLimits(Angle + num12) - Angle;
						Angle += num12;
						_handDegreesPerSecond = Mathf.Lerp(_handDegreesPerSecond, num12 / base.Runner.DeltaTime, 1f - Mathf.Exp((0f - base.Runner.DeltaTime) / 0.4f));
						StampAngle();
					}
					_lastPullerAngle = pullerAngle;
					_hasPullerAngle = true;
				}
				_inertiaDegreesPerSecond = 0f;
			}
			else if (!IsLocked && ExternalDriveDegreesPerSecond != 0f)
			{
				Angle = ClampToAngleLimits(Angle + ExternalDriveDegreesPerSecond * base.Runner.DeltaTime);
				StampAngle();
				_inertiaDegreesPerSecond = 0f;
			}
			else if (!IsLocked && _inertiaDegreesPerSecond != 0f)
			{
				float num13 = Angle + _inertiaDegreesPerSecond * base.Runner.DeltaTime;
				float num14 = ClampToAngleLimits(num13);
				if (num14 != num13)
				{
					_inertiaDegreesPerSecond = 0f;
				}
				Angle = num14;
				_inertiaDegreesPerSecond *= Mathf.Exp((0f - _inertiaDrag) * base.Runner.DeltaTime);
				if (Mathf.Abs(_inertiaDegreesPerSecond) < 8f)
				{
					_inertiaDegreesPerSecond = 0f;
				}
				StampAngle();
			}
			if (_wasGrabbed && !isGrabbed)
			{
				if (_inertiaEnabled && !IsLocked)
				{
					_inertiaDegreesPerSecond = Mathf.Clamp(_handDegreesPerSecond, 0f - _inertiaMaxDegreesPerSecond, _inertiaMaxDegreesPerSecond);
				}
				_handDegreesPerSecond = 0f;
				_hasPullerAngle = false;
				_hasVirtualPuller = false;
				_lockedSwipeAccumulatedDeg = 0f;
				_screenTangentLeadDeg = 0f;
				_screenTangentAlignment = 0f;
			}
			_wasGrabbed = isGrabbed;
			RaiseIfChanged();
		}

		private bool TryGetPullerSpinAngle(out float pullerAngle, out PhysGrabber puller)
		{
			pullerAngle = 0f;
			puller = null;
			Quaternion quaternion = InverseBaseWorldRotation();
			Vector3 position = base.transform.position;
			foreach (PhysGrabber grabber in _grabable.GrabObject.Grabbers)
			{
				if (!(grabber == null) && !(grabber.physGrabPointPullerPosition == null))
				{
					Vector3 vector = quaternion * (grabber.physGrabPointPullerPosition.position - position);
					if (!(new Vector2(vector.x, vector.y).sqrMagnitude < 0.0025f))
					{
						pullerAngle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
						puller = grabber;
						return true;
					}
				}
			}
			return false;
		}

		private bool UseSwipeDrive()
		{
			if (IsSwipeInputMode() && _focusEngaged && _cameraGrabFocusService != null)
			{
				return _cameraGrabFocusService.IsActive;
			}
			return false;
		}

		private bool IsSwipeInputMode()
		{
			if (_grabInputMode != CrankInputMode.TangentialSwipe)
			{
				return _grabInputMode == CrankInputMode.ScreenTangent;
			}
			return true;
		}

		private bool TryGetHandleRadial(out Vector2 radial)
		{
			radial = Vector2.right;
			Vector3 position = base.transform.position;
			foreach (PhysGrabber grabber in _grabable.GrabObject.Grabbers)
			{
				if (grabber != null && grabber.physGrabPointPullerPosition != null)
				{
					position = grabber.physGrabPointPullerPosition.position;
					break;
				}
			}
			Transform nearestHandle = _grabable.GetNearestHandle(position);
			if (nearestHandle == null)
			{
				return false;
			}
			Vector3 vector = InverseBaseWorldRotation() * (nearestHandle.position - base.transform.position);
			Vector2 vector2 = new Vector2(vector.x, vector.y);
			if (vector2.sqrMagnitude < 0.0025f)
			{
				return false;
			}
			radial = vector2.normalized;
			return true;
		}

		private Vector2 ProjectSwipeToWheelPlane(Vector2 lookDelta)
		{
			if (lookDelta == Vector2.zero)
			{
				return Vector2.zero;
			}
			Quaternion quaternion = InverseBaseWorldRotation();
			Vector3 lhs = quaternion * Vector3.up;
			Vector3 vector = quaternion * _cameraGrabFocusService.LookDirection;
			Vector3 vector2 = Vector3.Cross(lhs, vector);
			if (vector2.sqrMagnitude <= Mathf.Epsilon)
			{
				return Vector2.zero;
			}
			vector2.Normalize();
			Vector3 normalized = Vector3.Cross(vector, vector2).normalized;
			Vector3 vector3 = vector2 * lookDelta.x + normalized * (0f - lookDelta.y);
			return new Vector2(vector3.x, vector3.y);
		}

		private Quaternion InverseBaseWorldRotation()
		{
			return Quaternion.Inverse(((base.transform.parent != null) ? base.transform.parent.rotation : Quaternion.identity) * _baseLocalRotation);
		}

		private float ClampToAngleLimits(float angle)
		{
			if (!_useAngleLimits)
			{
				return angle;
			}
			return Mathf.Clamp(angle, _minAngleDeg, _maxAngleDeg);
		}

		private void DetectSwipeStrain()
		{
			_lockedSwipeAccumulatedDeg = Mathf.Max(0f, _lockedSwipeAccumulatedDeg + _cameraGrabFocusService.ConsumeLookDelta().magnitude - 30f * base.Runner.DeltaTime);
			if (!(Time.time - _lastStrainTime < _strainCooldownSeconds) && !(_lockedSwipeAccumulatedDeg < _strainSwipeTriggerDegrees))
			{
				_lockedSwipeAccumulatedDeg = 0f;
				_lastStrainTime = Time.time;
				StrainCount++;
			}
		}

		public bool DriveTowardAngle(float targetAngle, float maxDegreesPerSecond)
		{
			if (base.Object == null || !base.Object.IsValid || !base.HasStateAuthority)
			{
				return false;
			}
			if (IsLocked || IsGrabbed || ExternalDriveDegreesPerSecond != 0f || _inertiaDegreesPerSecond != 0f)
			{
				return false;
			}
			float maxDelta = Mathf.Abs(maxDegreesPerSecond) * Time.fixedDeltaTime;
			float num = Mathf.MoveTowards(Angle, ClampToAngleLimits(targetAngle), maxDelta);
			if (Mathf.Approximately(num, Angle))
			{
				return false;
			}
			Angle = num;
			StampAngle();
			RaiseIfChanged();
			return true;
		}

		public override void Render()
		{
			if (_focusEngaged && !LocalPlayerStillHolds())
			{
				DisengageFocus();
			}
			float num = Mathf.Abs(Angle - _speedAnchorAngle) / 0.35f;
			float num2 = Mathf.Max(num, _windowBaselineSpeed);
			if (Time.time - _speedAnchorTime >= 0.35f)
			{
				_windowBaselineSpeed = num;
				_speedAnchorAngle = Angle;
				_speedAnchorTime = Time.time;
			}
			if (Mathf.Abs(Angle - _renderAngle) > 120f)
			{
				_renderAngle = Angle;
			}
			else if (num2 < 5f)
			{
				_renderAngle = Mathf.Lerp(_renderAngle, Angle, 1f - Mathf.Exp(-14f * Time.deltaTime));
			}
			else
			{
				float target = Mathf.Lerp(_renderAngle, Angle, 1f - Mathf.Exp(-14f * Time.deltaTime));
				float maxDelta = Mathf.Max(num2 * Time.deltaTime * 1.05f, 0.35f);
				_renderAngle = Mathf.MoveTowards(_renderAngle, target, maxDelta);
			}
			base.transform.localRotation = _baseLocalRotation * Quaternion.AngleAxis(_renderAngle + WiggleOffsetDegrees(), Vector3.forward);
			RaiseIfChanged();
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3619479328u)]
		private void SetLockedRpc([RpcPayload(4)] bool locked)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(locked);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3619479328u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PhysicsVolumeModule.Scripts.CrankRotator::SetLockedRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(locked);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			IsLockedInternal = locked;
		}

		private void TryApplySpawnPose()
		{
			if (!_spawnPoseApplied && (bool)HasSpawnPose && !base.HasStateAuthority && !base.Object.NetworkTypeId.IsSceneObject)
			{
				_spawnPoseApplied = true;
				base.transform.SetPositionAndRotation(SpawnPosition, SpawnRotation);
				_rigidbody.position = _rigidbody.transform.position;
				_rigidbody.rotation = _rigidbody.transform.rotation;
				_baseLocalRotation = base.transform.localRotation;
				CaptureRestPose();
				ReanchorWorldJoints();
			}
		}

		private void OnLocalGrab(int playerId)
		{
			if (_focusCameraOnGrab && _cameraGrabFocusService != null && !(base.Runner == null) && playerId == base.Runner.LocalPlayer.PlayerId)
			{
				_focusEngaged = true;
				_cameraGrabFocusService.Activate(() => base.transform.position, IsSwipeInputMode());
			}
		}

		private void OnLocalUnGrab()
		{
			if (_focusEngaged && !LocalPlayerStillHolds())
			{
				DisengageFocus();
			}
		}

		private void DisengageFocus()
		{
			if (_focusEngaged)
			{
				_focusEngaged = false;
				_cameraGrabFocusService?.Deactivate();
			}
		}

		private bool LocalPlayerStillHolds()
		{
			if (base.Runner == null || !base.Runner.IsRunning)
			{
				return false;
			}
			int playerId = base.Runner.LocalPlayer.PlayerId;
			foreach (PhysGrabber grabber in _grabable.GrabObject.Grabbers)
			{
				if (grabber != null && grabber.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
		}

		private void DetectStrain()
		{
			if (Time.time - _lastStrainTime < _strainCooldownSeconds)
			{
				return;
			}
			foreach (PhysGrabber grabber in _grabable.GrabObject.Grabbers)
			{
				if (!(grabber == null) && !(grabber.physGrabPointPullerPosition == null) && grabber.physGrabPoints.TryGetValue(_grabable.GrabObject, out var value) && !(value == null) && !(Vector3.Distance(grabber.physGrabPointPullerPosition.position, value.position) < _strainTriggerMeters))
				{
					_lastStrainTime = Time.time;
					StrainCount++;
					break;
				}
			}
		}

		private void OnStrainRender()
		{
			if (StrainCount != _seenStrainCount)
			{
				_seenStrainCount = StrainCount;
				_wiggleEndTime = Time.time + 0.45f;
				this.OnLockedStrain?.Invoke(StrainCount);
			}
		}

		private float WiggleOffsetDegrees()
		{
			float num = _wiggleEndTime - Time.time;
			if (num <= 0f || _strainWiggleDeg <= 0f)
			{
				return 0f;
			}
			float num2 = 1f - num / 0.45f;
			return _strainWiggleDeg * Mathf.Sin(num2 * 4f * MathF.PI) * (1f - num2);
		}

		private void StampAngle()
		{
			base.transform.localRotation = _baseLocalRotation * Quaternion.AngleAxis(Angle, Vector3.forward);
		}

		private void RaiseIfChanged()
		{
			if (!Mathf.Approximately(Angle, _lastReportedAngle))
			{
				_lastReportedAngle = Angle;
				this.OnAngleChanged?.Invoke(Angle);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Angle = _Angle;
			IsLockedInternal = _IsLockedInternal;
			StrainCount = _StrainCount;
			SpawnPosition = _SpawnPosition;
			SpawnRotation = _SpawnRotation;
			HasSpawnPose = _HasSpawnPose;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Angle = Angle;
			_IsLockedInternal = IsLockedInternal;
			_StrainCount = StrainCount;
			_SpawnPosition = SpawnPosition;
			_SpawnRotation = SpawnRotation;
			_HasSpawnPose = HasSpawnPose;
		}

		[NetworkRpcWeavedInvoker(3619479328u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetLockedRpc_0040Invoker3619479328([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CrankRotator)context.TargetBehaviour).SetLockedRpc(value);
		}
	}
}
