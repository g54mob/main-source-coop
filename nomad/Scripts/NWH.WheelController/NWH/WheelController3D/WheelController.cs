using System;
using System.Collections.Generic;
using NWH.Common.Vehicles;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.WheelController3D
{
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(100)]
	public class WheelController : WheelUAPI
	{
		[Tooltip("    Instance of the spring.")]
		[SerializeField]
		public Spring spring = new Spring();

		[Tooltip("    Instance of the damper.")]
		[SerializeField]
		public Damper damper = new Damper();

		[Tooltip("    Instance of the wheel.")]
		[SerializeField]
		public Wheel wheel = new Wheel();

		[Tooltip("    Side (lateral) friction info.")]
		[SerializeField]
		public Friction sideFriction = new Friction();

		[Tooltip("    Forward (longitudinal) friction info.")]
		[SerializeField]
		public Friction forwardFriction = new Friction();

		[NonSerialized]
		[Tooltip("    Contains point in which wheel touches ground. Not valid if !_isGrounded.")]
		private WheelHit wheelHit;

		[Tooltip("    Current active friction preset.")]
		[SerializeField]
		private FrictionPreset activeFrictionPreset;

		[Tooltip("Motor torque applied to the wheel. Since NWH Vehicle Physics 2 the value is readonly and setting it will have no effect\r\nsince torque calculation is done inside powertrain solver.")]
		private float motorTorque;

		[Tooltip("    Brake torque applied to the wheel in Nm.")]
		private float brakeTorque;

		[Tooltip("    The amount of torque returned by the wheel.\r\n    Under perfect grip conditions this will be equal to the torque that was put down.\r\n    While in air the value will be equal to the source torque minus torque that is result of dW of the wheel.")]
		private float counterTorque;

		[Tooltip("    Current steer angle of the wheel.")]
		private float steerAngle;

		[SerializeField]
		private float camber;

		[NonSerialized]
		[Tooltip("    Tire load in Nm.")]
		private float load;

		[SerializeField]
		private float loadRating = 5400f;

		[Range(-2f, 2f)]
		[Tooltip("Amount of torque transferred from wheel to the chassis.")]
		public float chassisTorqueCoefficient = 1f;

		[Range(0f, 500f)]
		[Tooltip("    Constant torque acting similar to brake torque.\r\n    Imitates rolling resistance.")]
		public float rollingResistanceTorque = 30f;

		[Tooltip("Higher the number, higher the effect of longitudinal friction on lateral friction.\r\nIf 1, when wheels are locked up or there is wheel spin it will be impossible to steer.\r\nIf 0 doughnuts or power slides will be impossible.\r\nThe 'accurate' value is 1 but might not be desirable for arcade games.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float frictionCircleStrength = 1f;

		[Range(1f, 5f)]
		[SerializeField]
		[Tooltip("Stiffness of the friction circle. Higher shape value will have more sudden effect, but the effect will come into play with higher slip.")]
		private float frictionCirclePower = 3f;

		[Tooltip("    True if wheel touching ground.")]
		private bool _isGrounded;

		[Tooltip("    Rigidbody to which the forces will be applied.")]
		[SerializeField]
		private Rigidbody targetRigidbody;

		[Tooltip("Distance as a percentage of the max spring length. Value of 1 means that the friction force will\r\nbe applied 1 max spring length above the contact point, and value of 0 means that it will be applied at the\r\nground level. Value can be >1.\r\nCan be used instead of the anti-roll bar to prevent the vehicle from tipping over in corners\r\nand can be useful in low framerate applications where anti-roll bar might induce jitter.")]
		public float forceApplicationPointDistance = 0.8f;

		[Tooltip("Disables the motion vectors on the wheel visual to prevent artefacts due to \r\nthe wheel rotation when using PostProcessing.")]
		public bool disableMotionVectors = true;

		[Range(0.0001f, 30f)]
		[Tooltip("The speed coefficient of the spring / suspension extension when not on the ground.\r\nwheel.perceivedPowertrainInertia.e. how fast the wheels extend when in the air.\r\nThe setting of 1 will result in suspension fully extending in 1 second, 2 in 0.5s, 3 in 0.333s, etc.\r\nRecommended value is 6-10.")]
		public float suspensionExtensionSpeedCoeff = 6f;

		[Range(0f, 90f)]
		[Tooltip("The amount of wobble around the X-axis the wheel will have when fully damaged.\r\nPart of the damage visualization and does not affect handling.")]
		public float damageMaxWobbleAngle = 30f;

		[Tooltip("Scales the forces applied to other Rigidbodies. Useful for interacting\r\nwith lightweight objects and prevents them from flying away or glitching out.")]
		public float otherBodyForceScale = 1f;

		[Tooltip("Layers that will be detected by the wheel cast.")]
		public LayerMask layerMask = 1;

		[Tooltip("Layer the mesh collider of the wheel is on.")]
		public int meshColliderLayer = 2;

		[Tooltip("Experimental! Uses contacts modification API to soften or ignore collisions in the wheel forward direction.")]
		public bool useContactModification = true;

		[Tooltip("Number of friction substeps per global physics step. Has low impact on performance but higher value can improve stability.")]
		public int frictionSubsteps = 20;

		public HashSet<Collider> vehicleColliders;

		private bool _autoSimulate = true;

		private Matrix4x4 _parentWorldMatrix = Matrix4x4.identity;

		private Matrix4x4 _parentLocalMatrix = Matrix4x4.identity;

		private Matrix4x4 _initSuspensionLocalMatrix = Matrix4x4.identity;

		private Matrix4x4 _suspensionLocalMatrix = Matrix4x4.identity;

		private Matrix4x4 _suspensionWorldMatrix = Matrix4x4.identity;

		private Matrix4x4 _suspensionInvWorldMatrix = Matrix4x4.identity;

		private Vector3 _suspensionLocalPosition = Vector3.zero;

		private Vector3 _suspensionWorldPosition = Vector3.zero;

		private Quaternion _suspensionWorldRotation = Quaternion.identity;

		private Vector3 _suspensionUp = Vector3.up;

		private Vector3 _suspensionForward = Vector3.forward;

		private Vector3 _suspensionRight = Vector3.right;

		private Vector3 _suspensionLocalUp = Vector3.up;

		private Vector3 _suspensionLocalForward = Vector3.forward;

		private Vector3 _suspensionLocalRight = Vector3.right;

		private Matrix4x4 _wheelWorldMatrix = Matrix4x4.identity;

		private Vector3 _wheelWorldPosition = Vector3.zero;

		private Quaternion _wheelWorldRotation = Quaternion.identity;

		private Vector3 _wheelUp = Vector3.up;

		private Vector3 _wheelForward = Vector3.forward;

		private Vector3 _wheelRight = Vector3.right;

		private Quaternion steerRotationQuaternion = Quaternion.identity;

		private Matrix4x4 steerRotationMatrix = Matrix4x4.identity;

		private Vector3 _hitContactVelocity;

		private Vector3 _hitSurfaceVelocity;

		private Rigidbody _hitRigidbody;

		private Transform _parentTransform;

		private Vector3 _frictionForce;

		private Vector3 _suspensionForce;

		private float _damage;

		private bool _initialized;

		private float _dt;

		private float _invDt;

		private float _localAxleRotation;

		private float _localDamageRotation;

		private GroundDetectionBase _groundDetection;

		private WheelControllerManager _wheelControllerManager;

		private bool _lowSpeedReferenceIsSet;

		private Vector3 _lowSpeedReferencePosition;

		private Vector3 _zeroVector;

		private Vector3 _upVector;

		private Vector3 _castOrigin;

		private Vector3 _castDirection;

		private Vector3 _hitLocalPoint;

		private int _targetRigidbodyId;

		private bool _wakeOneFrame;

		public UnityEvent OnInitialized;

		[ShowInTelemetry]
		public override float MotorTorque
		{
			get
			{
				return motorTorque;
			}
			set
			{
				motorTorque = value;
			}
		}

		[ShowInTelemetry]
		public override float CounterTorque => counterTorque;

		[ShowInTelemetry]
		public override float BrakeTorque
		{
			get
			{
				return brakeTorque;
			}
			set
			{
				brakeTorque = value;
			}
		}

		[ShowInTelemetry]
		public override float SteerAngle
		{
			get
			{
				return steerAngle;
			}
			set
			{
				steerAngle = value;
			}
		}

		public override float Mass
		{
			get
			{
				return wheel.mass;
			}
			set
			{
				wheel.mass = Mathf.Clamp(value, 0f, 1f / 0f);
			}
		}

		public override float Radius
		{
			get
			{
				return wheel.radius;
			}
			set
			{
				wheel.radius = ((value < Mathf.Epsilon) ? Mathf.Epsilon : value);
				wheel.invRadius = 1f / wheel.radius;
			}
		}

		public override float Width
		{
			get
			{
				return wheel.width;
			}
			set
			{
				wheel.width = value;
			}
		}

		public override float Inertia
		{
			get
			{
				return wheel.inertia;
			}
			set
			{
				wheel.inertia = ((value < Mathf.Epsilon) ? Mathf.Epsilon : value);
				wheel.invInertia = 1f / wheel.inertia;
			}
		}

		[ShowInTelemetry]
		public override float RPM => wheel.rpm;

		public override float AngularVelocity => wheel.angularVelocity;

		public override Vector3 WheelPosition => _wheelWorldPosition;

		[ShowInTelemetry]
		public override float Load => load;

		public override float MaxLoad
		{
			get
			{
				return loadRating;
			}
			set
			{
				loadRating = ((value < 0f) ? 0f : value);
			}
		}

		[ShowInTelemetry]
		public override float Camber
		{
			get
			{
				return camber;
			}
			set
			{
				camber = ((value < -16f) ? (-16f) : ((value > 16f) ? 16f : value));
			}
		}

		[ShowInTelemetry]
		public override bool IsGrounded => _isGrounded;

		[ShowInTelemetry]
		public override float Damage
		{
			get
			{
				return _damage;
			}
			set
			{
				_damage = ((value < 0f) ? 0f : ((value > 1f) ? 1f : value));
			}
		}

		public override float SpringMaxLength
		{
			get
			{
				return spring.maxLength;
			}
			set
			{
				spring.maxLength = ((value < 0f) ? 0f : value);
			}
		}

		public override float SpringMaxForce
		{
			get
			{
				return spring.maxForce;
			}
			set
			{
				spring.maxForce = ((value < 0f) ? 0f : value);
			}
		}

		[ShowInTelemetry]
		public override float SpringForce => spring.force;

		[ShowInTelemetry]
		public override float SpringLength => spring.length;

		public override float SpringCompression
		{
			get
			{
				if (spring.maxLength != 0f)
				{
					return (spring.maxLength - spring.length) / spring.maxLength;
				}
				return 1f;
			}
		}

		public override float DamperBumpRate
		{
			get
			{
				return damper.bumpRate;
			}
			set
			{
				damper.bumpRate = ((value < 0f) ? 0f : value);
			}
		}

		public override float DamperReboundRate
		{
			get
			{
				return damper.reboundRate;
			}
			set
			{
				damper.reboundRate = ((value < 0f) ? 0f : value);
			}
		}

		[ShowInTelemetry]
		public override float DamperForce => damper.force;

		[ShowInTelemetry]
		public override float LongitudinalSlip => forwardFriction.slip;

		[ShowInTelemetry]
		public override float LongitudinalSpeed => forwardFriction.speed;

		[ShowInTelemetry]
		public override float LateralSlip => sideFriction.slip;

		[ShowInTelemetry]
		public override float LateralSpeed => sideFriction.speed;

		public override Vector3 HitPoint => wheelHit.point;

		public override Vector3 HitNormal => wheelHit.normal;

		public override GameObject WheelVisual
		{
			get
			{
				return wheel.rotatingContainer.gameObject;
			}
			set
			{
				wheel.rotatingContainer = value.transform;
			}
		}

		public override GameObject NonRotatingVisual
		{
			get
			{
				return wheel.nonRotatingContainer.gameObject;
			}
			set
			{
				wheel.nonRotatingContainer = value.transform;
			}
		}

		public override Rigidbody TargetRigidbody => targetRigidbody;

		public override Collider HitCollider => wheelHit.collider;

		public override float ForceApplicationPointDistance
		{
			get
			{
				return forceApplicationPointDistance;
			}
			set
			{
				forceApplicationPointDistance = value;
			}
		}

		public override FrictionPreset FrictionPreset
		{
			get
			{
				return activeFrictionPreset;
			}
			set
			{
				activeFrictionPreset = value;
			}
		}

		public override float LongitudinalFrictionGrip
		{
			get
			{
				return forwardFriction.grip;
			}
			set
			{
				forwardFriction.grip = ((value < 0f) ? 0f : value);
			}
		}

		public override float LongitudinalFrictionStiffness
		{
			get
			{
				return forwardFriction.stiffness;
			}
			set
			{
				forwardFriction.stiffness = ((value < 0f) ? 0f : value);
			}
		}

		public override float LateralFrictionGrip
		{
			get
			{
				return sideFriction.grip;
			}
			set
			{
				sideFriction.grip = ((value < 0f) ? 0f : value);
			}
		}

		public override float LateralFrictionStiffness
		{
			get
			{
				return sideFriction.stiffness;
			}
			set
			{
				sideFriction.stiffness = ((value < 0f) ? 0f : value);
			}
		}

		public override float RollingResistanceTorque
		{
			get
			{
				return rollingResistanceTorque;
			}
			set
			{
				rollingResistanceTorque = ((value < 0f) ? 0f : value);
			}
		}

		public override float FrictionCircleShape
		{
			get
			{
				return frictionCirclePower;
			}
			set
			{
				frictionCirclePower = ((value < 0f) ? 0f : value);
			}
		}

		public override float FrictionCircleStrength
		{
			get
			{
				return frictionCircleStrength;
			}
			set
			{
				frictionCircleStrength = ((value < 0f) ? 0f : value);
			}
		}

		public override bool AutoSimulate
		{
			get
			{
				return _autoSimulate;
			}
			set
			{
				_autoSimulate = value;
			}
		}

		private void OnDrawGizmosSelected()
		{
		}

		public override void Validate()
		{
			OnValidate();
		}

		public override void WakeFromSleep()
		{
			_wakeOneFrame = true;
		}

		private void Awake()
		{
			targetRigidbody = GetComponentInParent<Rigidbody>();
			_zeroVector = Vector3.zero;
			_upVector = Vector3.up;
		}

		private void Start()
		{
			SetRuntimeDefaultsIfNeeded();
			FindOrSpawnVisualContainers();
			FindOrAddWheelControllerManager();
			Initialize();
			if (spring.maxLength > 0f)
			{
				spring.length = spring.maxLength * 0.7f;
				spring.prevLength = spring.length;
			}
			_groundDetection = GetComponent<GroundDetectionBase>();
			if (_groundDetection == null)
			{
				_groundDetection = base.gameObject.AddComponent<StandardGroundDetection>();
			}
			wheelHit = default(WheelHit);
			DisableMotionVectors();
			if (useContactModification)
			{
				Physics.ContactModifyEvent += OnContactModifyEvent;
				_targetRigidbodyId = targetRigidbody.GetInstanceID();
			}
		}

		public void Initialize()
		{
			_parentTransform = TargetRigidbody.transform;
			UpdateSuspensionTransforms();
			UpdateWheelTransforms();
			if (wheel.radius < 0.01f)
			{
				wheel.radius = 0.01f;
			}
			wheel.invRadius = 1f / wheel.radius;
			if (wheel.inertia < 1E-05f)
			{
				wheel.inertia = 1E-05f;
			}
			wheel.inertia = wheel.mass * wheel.radius * wheel.radius;
			wheel.invInertia = 1f / wheel.inertia;
			if (wheel.mass < 0.01f)
			{
				wheel.mass = 0.01f;
			}
			frictionSubsteps = ((frictionSubsteps <= 0) ? 1 : frictionSubsteps);
			SetupWheelCollider();
			vehicleColliders = new HashSet<Collider>(targetRigidbody.transform.GetComponentsInChildren<Collider>(includeInactive: true));
			_initialized = true;
			OnInitialized.Invoke();
		}

		private void FixedUpdate()
		{
			if (_autoSimulate)
			{
				Step();
			}
		}

		private void OnEnable()
		{
			RegisterWithWheelControllerManager();
		}

		private void OnDisable()
		{
			DeregisterWithWheelControllerManager();
		}

		public override void Step()
		{
			if (!_initialized || !base.isActiveAndEnabled)
			{
				return;
			}
			_dt = Time.fixedDeltaTime;
			if (_dt < 1E-05f)
			{
				_dt = 1E-05f;
			}
			_invDt = 1f / _dt;
			wheel.prevAngularVelocity = wheel.angularVelocity;
			_suspensionForce = _zeroVector;
			_frictionForce = _zeroVector;
			_hitSurfaceVelocity = _zeroVector;
			_hitContactVelocity = _zeroVector;
			wheelHit.point = _zeroVector;
			wheelHit.normal = _upVector;
			load = 0f;
			spring.force = 0f;
			damper.force = 0f;
			forwardFriction.speed = 0f;
			sideFriction.speed = 0f;
			forwardFriction.slip = 0f;
			sideFriction.slip = 0f;
			_isGrounded = false;
			_parentWorldMatrix = _parentTransform.localToWorldMatrix;
			_parentLocalMatrix = _parentTransform.worldToLocalMatrix;
			UpdateSuspensionTransforms();
			UpdateWheelTransforms();
			bool num = spring.maxLength > 0f;
			float num2 = (num ? (wheel.radius * 1.1f) : (wheel.radius * 0.1f));
			float distance = (num ? (wheel.radius * 2.2f + spring.maxLength) : (wheel.radius * 0.02f + num2));
			_castOrigin.x = _suspensionWorldPosition.x + _suspensionUp.x * num2;
			_castOrigin.y = _suspensionWorldPosition.y + _suspensionUp.y * num2;
			_castOrigin.z = _suspensionWorldPosition.z + _suspensionUp.z * num2;
			_castDirection.x = 0f - _suspensionUp.x;
			_castDirection.y = 0f - _suspensionUp.y;
			_castDirection.z = 0f - _suspensionUp.z;
			if (_groundDetection.WheelCast(in _castOrigin, in _castDirection, in distance, in wheel.radius, in wheel.width, ref wheelHit, layerMask))
			{
				_isGrounded = true;
				_hitContactVelocity = targetRigidbody.GetPointVelocity(wheelHit.point);
				_hitRigidbody = wheelHit.collider?.attachedRigidbody;
				if (_hitRigidbody != null)
				{
					_hitSurfaceVelocity = _hitRigidbody.GetPointVelocity(wheelHit.point);
					_hitContactVelocity -= _hitSurfaceVelocity;
				}
				forwardFriction.speed = Vector3.Dot(_hitContactVelocity, _wheelForward);
				sideFriction.speed = Vector3.Dot(_hitContactVelocity, _wheelRight);
			}
			spring.prevLength = spring.length;
			float num3;
			if (_isGrounded)
			{
				_hitLocalPoint = _suspensionInvWorldMatrix.MultiplyPoint3x4(wheelHit.point);
				float f = Mathf.Asin(Mathf.Clamp(_hitLocalPoint.z / wheel.radius, -1f, 1f));
				num3 = Mathf.Clamp(0f - (_hitLocalPoint.y + wheel.radius * Mathf.Cos(f)), 0f, spring.maxLength);
			}
			else
			{
				num3 = spring.maxLength;
			}
			if (num3 > spring.length)
			{
				float maxDelta = suspensionExtensionSpeedCoeff * _dt;
				spring.length = Mathf.MoveTowards(spring.length, num3, maxDelta);
			}
			else
			{
				spring.length = num3;
			}
			spring.compressionVelocity = (spring.prevLength - spring.length) / _dt;
			spring.compression = ((spring.maxLength == 0f) ? 1f : ((spring.maxLength - spring.length) / spring.maxLength));
			spring.force = (_isGrounded ? (spring.maxForce * spring.forceCurve.Evaluate(spring.compression)) : 0f);
			damper.force = (_isGrounded ? damper.CalculateDamperForce(in spring.compressionVelocity) : 0f);
			if (_isGrounded)
			{
				if (spring.maxLength > 0f && spring.maxForce > 0f)
				{
					load = spring.force + damper.force;
					load = ((load < 0f) ? 0f : load);
					_suspensionForce = load * wheelHit.normal;
					targetRigidbody.AddForceAtPosition(_suspensionForce, _suspensionWorldPosition);
				}
				else
				{
					load = loadRating;
				}
			}
			UpdateWheelTransforms();
			wheel.axleAngle = wheel.axleAngle % 360f + wheel.angularVelocity * 57.29578f * _dt;
			Quaternion quaternion = Quaternion.AngleAxis(wheel.axleAngle, _wheelRight);
			Quaternion quaternion2 = Quaternion.AngleAxis(camber * ((_suspensionLocalPosition.x < 0f) ? 1f : (-1f)), _wheelForward);
			Quaternion.AngleAxis(_damage * damageMaxWobbleAngle, quaternion * (quaternion * _wheelUp));
			wheel.axleRotation = quaternion2 * quaternion * _wheelWorldRotation;
			UpdateFriction();
			Vector3 torque = Vector3.Project(Vector3.Cross(wheelHit.point - targetRigidbody.worldCenterOfMass, forwardFriction.force * _wheelRight) * chassisTorqueCoefficient, _wheelRight);
			targetRigidbody.AddTorque(torque);
			if (_isGrounded)
			{
				Vector3 position = wheelHit.point + _suspensionUp * forceApplicationPointDistance * spring.maxLength;
				targetRigidbody.AddForceAtPosition(_frictionForce, position);
				if (_hitRigidbody != null)
				{
					Vector3 force = default(Vector3);
					force.x = (0f - (_frictionForce.x + _suspensionForce.x)) * otherBodyForceScale;
					force.y = (0f - (_frictionForce.y + _suspensionForce.y)) * otherBodyForceScale;
					force.z = (0f - (_frictionForce.z + _suspensionForce.z)) * otherBodyForceScale;
					_hitRigidbody.AddForceAtPosition(force, wheelHit.point);
				}
			}
			wheel.rotatingContainer.SetPositionAndRotation(_wheelWorldPosition, wheel.axleRotation);
			wheel.nonRotatingContainer.SetPositionAndRotation(_wheelWorldPosition, quaternion2 * _wheelWorldRotation);
			wheel.meshCollider.transform.SetPositionAndRotation(_suspensionWorldPosition, quaternion2 * _suspensionWorldRotation);
		}

		private void UpdateSuspensionTransforms()
		{
			_suspensionLocalMatrix = TargetRigidbody.transform.worldToLocalMatrix * base.transform.localToWorldMatrix;
			_suspensionLocalUp.x = _suspensionLocalMatrix.m01;
			_suspensionLocalUp.y = _suspensionLocalMatrix.m11;
			_suspensionLocalUp.z = _suspensionLocalMatrix.m21;
			_suspensionLocalForward.x = _suspensionLocalMatrix.m02;
			_suspensionLocalForward.y = _suspensionLocalMatrix.m12;
			_suspensionLocalForward.z = _suspensionLocalMatrix.m22;
			_suspensionLocalRight.x = _suspensionLocalMatrix.m00;
			_suspensionLocalRight.y = _suspensionLocalMatrix.m10;
			_suspensionLocalRight.z = _suspensionLocalMatrix.m20;
			_suspensionLocalPosition.x = _suspensionLocalMatrix.m03;
			_suspensionLocalPosition.y = _suspensionLocalMatrix.m13;
			_suspensionLocalPosition.z = _suspensionLocalMatrix.m23;
			steerRotationQuaternion = Quaternion.AngleAxis(steerAngle, _suspensionLocalUp);
			float num = steerRotationQuaternion.x * steerRotationQuaternion.x;
			float num2 = steerRotationQuaternion.y * steerRotationQuaternion.y;
			float num3 = steerRotationQuaternion.z * steerRotationQuaternion.z;
			float num4 = steerRotationQuaternion.x * steerRotationQuaternion.y;
			float num5 = steerRotationQuaternion.x * steerRotationQuaternion.z;
			float num6 = steerRotationQuaternion.y * steerRotationQuaternion.z;
			float num7 = steerRotationQuaternion.w * steerRotationQuaternion.x;
			float num8 = steerRotationQuaternion.w * steerRotationQuaternion.y;
			float num9 = steerRotationQuaternion.w * steerRotationQuaternion.z;
			steerRotationMatrix.m00 = 1f - 2f * (num2 + num3);
			steerRotationMatrix.m01 = 2f * (num4 - num9);
			steerRotationMatrix.m02 = 2f * (num5 + num8);
			steerRotationMatrix.m03 = 0f;
			steerRotationMatrix.m10 = 2f * (num4 + num9);
			steerRotationMatrix.m11 = 1f - 2f * (num + num3);
			steerRotationMatrix.m12 = 2f * (num6 - num7);
			steerRotationMatrix.m13 = 0f;
			steerRotationMatrix.m20 = 2f * (num5 - num8);
			steerRotationMatrix.m21 = 2f * (num6 + num7);
			steerRotationMatrix.m22 = 1f - 2f * (num + num2);
			steerRotationMatrix.m23 = 0f;
			steerRotationMatrix.m30 = 0f;
			steerRotationMatrix.m31 = 0f;
			steerRotationMatrix.m32 = 0f;
			steerRotationMatrix.m33 = 1f;
			float num10 = (0f - wheel.rimOffset) * ((_suspensionLocalPosition.x < 0f) ? (-1f) : 1f);
			steerRotationMatrix.m03 = (_suspensionLocalPosition.x + num10) * (1f - steerRotationMatrix.m00) + _suspensionLocalPosition.y * (0f - steerRotationMatrix.m01) + _suspensionLocalPosition.z * (0f - steerRotationMatrix.m02);
			steerRotationMatrix.m13 = (_suspensionLocalPosition.x + num10) * (0f - steerRotationMatrix.m10) + _suspensionLocalPosition.y * (1f - steerRotationMatrix.m11) + _suspensionLocalPosition.z * (0f - steerRotationMatrix.m12);
			steerRotationMatrix.m23 = (_suspensionLocalPosition.x + num10) * (0f - steerRotationMatrix.m20) + _suspensionLocalPosition.y * (0f - steerRotationMatrix.m21) + _suspensionLocalPosition.z * (1f - steerRotationMatrix.m22);
			_suspensionWorldMatrix = _parentWorldMatrix * (steerRotationMatrix * _suspensionLocalMatrix);
			_suspensionInvWorldMatrix = Matrix4x4.Inverse(_suspensionWorldMatrix);
			_suspensionWorldPosition.x = _suspensionWorldMatrix.m03;
			_suspensionWorldPosition.y = _suspensionWorldMatrix.m13;
			_suspensionWorldPosition.z = _suspensionWorldMatrix.m23;
			_suspensionUp.x = _suspensionWorldMatrix.m01;
			_suspensionUp.y = _suspensionWorldMatrix.m11;
			_suspensionUp.z = _suspensionWorldMatrix.m21;
			_suspensionForward.x = _suspensionWorldMatrix.m02;
			_suspensionForward.y = _suspensionWorldMatrix.m12;
			_suspensionForward.z = _suspensionWorldMatrix.m22;
			_suspensionRight.x = _suspensionWorldMatrix.m00;
			_suspensionRight.y = _suspensionWorldMatrix.m10;
			_suspensionRight.z = _suspensionWorldMatrix.m20;
			_suspensionWorldRotation = Quaternion.LookRotation(_suspensionForward, _suspensionUp);
		}

		private void UpdateWheelTransforms()
		{
			Vector3 vector = -_suspensionUp * spring.length;
			_wheelWorldMatrix = _suspensionWorldMatrix;
			_wheelWorldMatrix.m03 += vector.x;
			_wheelWorldMatrix.m13 += vector.y;
			_wheelWorldMatrix.m23 += vector.z;
			_wheelWorldPosition.x = _wheelWorldMatrix.m03;
			_wheelWorldPosition.y = _wheelWorldMatrix.m13;
			_wheelWorldPosition.z = _wheelWorldMatrix.m23;
			_wheelUp.x = _suspensionWorldMatrix.m01;
			_wheelUp.y = _suspensionWorldMatrix.m11;
			_wheelUp.z = _suspensionWorldMatrix.m21;
			_wheelForward.x = _suspensionWorldMatrix.m02;
			_wheelForward.y = _suspensionWorldMatrix.m12;
			_wheelForward.z = _suspensionWorldMatrix.m22;
			_wheelRight.x = _suspensionWorldMatrix.m00;
			_wheelRight.y = _suspensionWorldMatrix.m10;
			_wheelRight.z = _suspensionWorldMatrix.m20;
			if (_wheelForward != _zeroVector && _wheelUp != _zeroVector)
			{
				_wheelWorldRotation = Quaternion.LookRotation(_wheelForward, _wheelUp);
			}
			else
			{
				_wheelWorldRotation = _suspensionWorldRotation;
			}
		}

		private void SetupWheelCollider()
		{
			GameObject gameObject = base.transform.Find("Collider")?.gameObject;
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			GameObject gameObject2 = new GameObject("Collider");
			if (gameObject2 == null)
			{
				Debug.LogError("Failed go spawn a wheel collider!");
				return;
			}
			Transform obj = gameObject2.transform;
			obj.SetParent(base.transform);
			obj.localPosition = Vector3.zero;
			obj.localRotation = Quaternion.identity;
			wheel.meshCollider = gameObject2.AddComponent<MeshCollider>();
			if (wheel.meshCollider == null)
			{
				Debug.LogError("Failed to add a MeshCollider to the Collider object!");
				return;
			}
			wheel.meshCollider.convex = true;
			wheel.meshCollider.gameObject.layer = meshColliderLayer;
			wheel.meshCollider.material.bounceCombine = PhysicsMaterialCombine.Minimum;
			wheel.meshCollider.material.frictionCombine = PhysicsMaterialCombine.Minimum;
			wheel.meshCollider.material.bounciness = 0f;
			wheel.meshCollider.material.staticFriction = 0f;
			wheel.meshCollider.material.dynamicFriction = 0f;
			wheel.meshCollider.hasModifiableContacts = true;
			if (wheel.meshCollider != null)
			{
				UnityEngine.Object.Destroy(wheel.meshCollider.sharedMesh);
				wheel.meshCollider.sharedMesh = WheelControllerUtility.CreateCylinderMesh(12, wheel.width * 1.05f, wheel.radius * 0.95f);
			}
			else
			{
				Debug.LogError("Failed to set up wheel collider!");
			}
		}

		private static float GetSuspensionLengthFromWheelHit(in float wheelRadius, in Vector3 hitWorldPoint, in Matrix4x4 l2wMat, ref Vector3 hitLocalPoint)
		{
			hitLocalPoint.x = l2wMat.m00 * hitWorldPoint.x + l2wMat.m01 * hitWorldPoint.y + l2wMat.m02 * hitWorldPoint.z + l2wMat.m03;
			hitLocalPoint.y = l2wMat.m10 * hitWorldPoint.x + l2wMat.m11 * hitWorldPoint.y + l2wMat.m12 * hitWorldPoint.z + l2wMat.m13;
			hitLocalPoint.z = l2wMat.m20 * hitWorldPoint.x + l2wMat.m21 * hitWorldPoint.y + l2wMat.m22 * hitWorldPoint.z + l2wMat.m23;
			float num = hitLocalPoint.z / wheelRadius;
			if (num < -1f)
			{
				num = -1f;
			}
			else if (num > 1f)
			{
				num = 1f;
			}
			float f = 0f;
			if (num >= -1f && num <= 1f)
			{
				f = Mathf.Asin(num);
			}
			return 0f - hitLocalPoint.y + wheelRadius * Mathf.Cos(f);
		}

		protected virtual void UpdateFriction()
		{
			float num = load * forwardFriction.loadFactor;
			float num2 = load * sideFriction.loadFactor;
			float num3 = Mathf.Pow(Mathf.Clamp01(num / loadRating), 0.5f) * loadRating * forwardFriction.loadFactor;
			float num4 = Mathf.Pow(Mathf.Clamp01(num2 / loadRating), 0.5f) * loadRating * sideFriction.loadFactor;
			_ = targetRigidbody.mass;
			float num5 = ((forwardFriction.speed < 0f) ? (0f - forwardFriction.speed) : forwardFriction.speed);
			float num6 = ((sideFriction.speed < 0f) ? (0f - sideFriction.speed) : sideFriction.speed);
			float num7 = 1.5f * (_dt / 0.005f);
			num7 = ((num7 < 1.5f) ? 1.5f : ((num7 > 10f) ? 10f : num7));
			float num8 = ((num5 < num7) ? num7 : num5);
			float num9 = 1f / num8;
			float num10 = Vector3.Dot(_suspensionUp, wheelHit.normal);
			num10 = ((num10 < 0f) ? 0f : num10);
			float num11 = activeFrictionPreset.BCDE.z * num3;
			float num12 = brakeTorque + rollingResistanceTorque;
			float num13 = 1f / (float)frictionSubsteps;
			float num14 = _dt * num13;
			_ = 1f / num14;
			float num15 = 0f;
			float num16 = 1f / wheel.inertia;
			float num17 = 0f;
			for (int i = 0; i < frictionSubsteps; i++)
			{
				float num18 = motorTorque * num14;
				wheel.angularVelocity += num18 * num16;
				num17 += num18;
				if (_isGrounded)
				{
					forwardFriction.slip = (0f - (wheel.angularVelocity * wheel.radius - forwardFriction.speed)) * num9 * forwardFriction.stiffness;
					float num19 = ((forwardFriction.slip >= 0f) ? 1f : (-1f));
					float time = ((forwardFriction.slip < 0f) ? (0f - forwardFriction.slip) : forwardFriction.slip);
					float num20 = (0f - num19) * activeFrictionPreset.Curve.Evaluate(time) * num3;
					float num21 = ((num20 > num11) ? num11 : ((num20 < 0f - num11) ? (0f - num11) : num20));
					float num22 = num21 * wheel.radius * num14;
					wheel.angularVelocity -= num22 * num16;
					num17 += num22;
					num15 += num21 * num13;
				}
				if (num12 > 0f)
				{
					float angularVelocity = wheel.angularVelocity;
					float num23 = ((wheel.angularVelocity >= 0f) ? 1f : (-1f)) * num12 * num14;
					wheel.angularVelocity -= num23 * num16;
					if ((wheel.angularVelocity >= 0f && angularVelocity < 0f) || (wheel.angularVelocity < 0f && angularVelocity >= 0f))
					{
						wheel.angularVelocity = 0f;
					}
					num17 -= num23;
				}
			}
			forwardFriction.force = num15;
			forwardFriction.slip = (_isGrounded ? ((0f - (wheel.angularVelocity * wheel.radius - forwardFriction.speed)) * num9 * forwardFriction.stiffness) : 0f);
			forwardFriction.slip = Mathf.Clamp(forwardFriction.slip, -1f, 1f);
			counterTorque = 0f - num17;
			sideFriction.slip = Mathf.Atan2(sideFriction.speed, num8) * 57.29578f * 0.01111f * sideFriction.stiffness;
			float num24 = ((sideFriction.slip < 0f) ? (-1f) : 1f);
			float time2 = ((sideFriction.slip < 0f) ? (0f - sideFriction.slip) : sideFriction.slip);
			float num25 = activeFrictionPreset.BCDE.z * num4;
			sideFriction.force = (0f - num24) * activeFrictionPreset.Curve.Evaluate(time2) * num4 * num10;
			if (_isGrounded && !_wakeOneFrame && num5 < 0.12f && num6 < 0.12f)
			{
				float num26 = spring.length + wheel.radius;
				Vector3 vector = _suspensionWorldPosition - _suspensionUp * num26;
				if (!_lowSpeedReferenceIsSet)
				{
					_lowSpeedReferenceIsSet = true;
					_lowSpeedReferencePosition = vector;
				}
				else
				{
					Vector3 vector2 = _lowSpeedReferencePosition - vector;
					Vector3 lhs = _invDt * load * vector2;
					if (Mathf.Abs(wheel.angularVelocity) < 0.5f)
					{
						forwardFriction.force += Vector3.Dot(lhs, _suspensionForward);
					}
					sideFriction.force += Vector3.Dot(lhs, _suspensionRight);
				}
			}
			else
			{
				_lowSpeedReferenceIsSet = false;
			}
			forwardFriction.force = ((forwardFriction.force > num11) ? num11 : ((forwardFriction.force < 0f - num11) ? (0f - num11) : forwardFriction.force));
			sideFriction.force = ((sideFriction.force > num25) ? num25 : ((sideFriction.force < 0f - num25) ? (0f - num25) : sideFriction.force));
			forwardFriction.force *= forwardFriction.grip;
			sideFriction.force *= sideFriction.grip;
			if (frictionCircleStrength > 0f)
			{
				sideFriction.force *= 1f - Mathf.Pow(Mathf.Clamp01(Mathf.Abs(forwardFriction.slip)), frictionCirclePower) * frictionCircleStrength;
			}
			_frictionForce = _suspensionRight * sideFriction.force + _suspensionForward * forwardFriction.force;
			if (_wakeOneFrame)
			{
				_wakeOneFrame = false;
			}
		}

		private void OnValidate()
		{
			if (wheel.rotatingContainer == base.gameObject.transform)
			{
				Debug.LogError(base.name + ": Visual and WheelController are the same GameObject. This will result in unknown behaviour.The controller and the visual should be separate GameObjects.");
			}
			if (!Application.isPlaying && wheel.rotatingContainer != null && wheel.rotatingContainer.GetComponentsInChildren<Collider>().Length != 0)
			{
				Debug.LogWarning(base.name + ": Visual object already contains a Collider. Visual should have no colliders attached to it or its children as they can prevent the wheel from functioning properly.");
			}
			if (targetRigidbody == null)
			{
				targetRigidbody = GetComponentInParent<Rigidbody>();
			}
			if (targetRigidbody != null)
			{
				string text = targetRigidbody.name + " > " + base.name + ":";
				float num = targetRigidbody.mass * (0f - Physics.gravity.y) * 0.05f;
				float num2 = targetRigidbody.mass * (0f - Physics.gravity.y) * 4f;
				if (loadRating < num)
				{
					Debug.LogWarning($"{text} Load rating of the tyre might be too low. This can cause the vehicle to slide around. Current: {loadRating}, min. recommended: {num}.");
				}
				else if (loadRating > num2)
				{
					Debug.LogWarning($"{text} Load rating of the tyre might be too high. This can cause the vehicle friction to not work properly. Current: {loadRating}, max. recommended: {num2}.");
				}
				if (spring.length > 0f)
				{
					float num3 = targetRigidbody.mass * (0f - Physics.gravity.y) * 0.25f;
					if (spring.maxForce < num3)
					{
						Debug.LogWarning($"{text} spring.maxForce is most likely too low for the given Rigidbody mass. Current: {spring.maxForce}, min. recommended: {num3}, recommended: {num3 * 3f}" + "With the current values the suspension might not be strong enough to support the weight of the vehicle and might bottom out.");
					}
					float fixedDeltaTime = Time.fixedDeltaTime;
					if (spring.maxLength < fixedDeltaTime)
					{
						Debug.LogWarning($"{text} spring.maxLength is shorter than recommended for the given Time.fixedDeltaTime. Current: {spring.maxLength}, min. recommended: {fixedDeltaTime}. With " + "the current values the suspension might bottom out frequently and cause a harsh ride.");
					}
				}
			}
			else
			{
				Debug.LogWarning("Target Rigidbody on " + base.name + " is null!");
			}
		}

		private void RegisterWithWheelControllerManager()
		{
			FindOrAddWheelControllerManager();
			_wheelControllerManager.Register(this);
		}

		private void DeregisterWithWheelControllerManager()
		{
			FindOrAddWheelControllerManager();
			_wheelControllerManager.Deregister(this);
		}

		private void FindOrAddWheelControllerManager()
		{
			if (_wheelControllerManager == null)
			{
				_wheelControllerManager = GetComponentInParent<WheelControllerManager>();
				if (_wheelControllerManager == null)
				{
					_wheelControllerManager = targetRigidbody.gameObject.AddComponent<WheelControllerManager>();
				}
			}
		}

		private void DisableMotionVectors()
		{
			if (disableMotionVectors && wheel.rotatingContainer != null)
			{
				MeshRenderer[] componentsInChildren = wheel.rotatingContainer.GetComponentsInChildren<MeshRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
				}
			}
		}

		private void Reset()
		{
			SetRuntimeDefaultsIfNeeded();
			FindOrSpawnVisualContainers();
			int num = 4;
			float num2 = 0f - Physics.gravity.y;
			float num3 = targetRigidbody.mass * num2 / (float)num;
			spring.maxForce = num3 * 6f;
			damper.bumpRate = num3 * 0.15f;
			damper.reboundRate = num3 * 0.15f;
			loadRating = num3 * 2f;
		}

		public void FindOrSpawnVisualContainers()
		{
			Vector3 localPosition = new Vector3(0f, (0f - spring.maxLength) * 0.5f, 0f);
			wheel.rotatingContainer = base.transform.Find("Rotating");
			if (wheel.rotatingContainer == null)
			{
				GameObject gameObject = new GameObject("Rotating");
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = localPosition;
				gameObject.transform.localRotation = Quaternion.identity;
				wheel.rotatingContainer = gameObject.transform;
			}
			wheel.nonRotatingContainer = base.transform.Find("NonRotating");
			if (wheel.nonRotatingContainer == null)
			{
				GameObject gameObject2 = new GameObject("NonRotating");
				gameObject2.transform.parent = base.transform;
				gameObject2.transform.localPosition = localPosition;
				gameObject2.transform.localRotation = Quaternion.identity;
				wheel.nonRotatingContainer = gameObject2.transform;
			}
		}

		public void SetRuntimeDefaultsIfNeeded(bool reset = false, bool findWheelVisuals = true)
		{
			if (targetRigidbody == null)
			{
				targetRigidbody = base.gameObject.GetComponentInParent<Rigidbody>();
			}
			if (wheel == null || reset)
			{
				wheel = new Wheel();
			}
			if (spring == null || reset)
			{
				spring = new Spring();
			}
			if (damper == null || reset)
			{
				damper = new Damper();
			}
			if (forwardFriction == null || reset)
			{
				forwardFriction = new Friction
				{
					grip = 1.2f,
					loadFactor = 1.8f,
					stiffness = 0.7f
				};
			}
			if (sideFriction == null || reset)
			{
				sideFriction = new Friction
				{
					grip = 1f,
					loadFactor = 1.4f,
					stiffness = 1f
				};
			}
			if (activeFrictionPreset == null || reset)
			{
				activeFrictionPreset = Resources.Load<FrictionPreset>("Wheel Controller 3D/Defaults/DefaultFrictionPreset");
			}
			if (spring.forceCurve == null || spring.forceCurve.keys.Length == 0 || reset)
			{
				spring.forceCurve = GenerateDefaultSpringCurve();
			}
		}

		private AnimationCurve GenerateDefaultSpringCurve()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.AddKey(0f, 0f);
			animationCurve.AddKey(1f, 1f);
			return animationCurve;
		}

		public void PositionToVisual()
		{
			if (wheel.rotatingContainer.childCount == 0)
			{
				Debug.LogWarning("Rotating container does not have any children assigned. Cannot position the WheelController as there is nothing to position to.");
			}
			if (wheel.rotatingContainer == null)
			{
				Debug.LogError("Wheel visual not assigned.");
				return;
			}
			Rigidbody componentInParent = GetComponentInParent<Rigidbody>();
			if (componentInParent == null)
			{
				Debug.LogError("Rigidbody not found in parent.");
				return;
			}
			int num = GetComponentInParent<Rigidbody>().GetComponentsInChildren<WheelController>().Length;
			if (num == 0)
			{
				return;
			}
			float num2 = Mathf.Clamp01(componentInParent.mass * (0f - Physics.gravity.y) / (float)num / spring.maxForce) * spring.maxLength;
			wheel.rotatingContainer.GetChild(0);
			Vector3 position = base.transform.position;
			base.transform.position = wheel.rotatingContainer.GetChild(0).position + componentInParent.transform.up * (spring.maxLength - num2);
			Vector3 vector = base.transform.position - position;
			foreach (Transform item in wheel.rotatingContainer)
			{
				item.position -= vector;
			}
			foreach (Transform item2 in wheel.nonRotatingContainer)
			{
				item2.position -= vector;
			}
		}

		private void OnContactModifyEvent(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
		{
			if (!useContactModification)
			{
				return;
			}
			if (_suspensionLocalMatrix == Matrix4x4.zero)
			{
				Debug.LogWarning("Suspension Local Matrix is not initialized.");
				return;
			}
			foreach (ModifiableContactPair item in pairs)
			{
				if (item.bodyInstanceID != _targetRigidbodyId && item.otherBodyInstanceID != _targetRigidbodyId)
				{
					continue;
				}
				_ = item.bodyInstanceID;
				_ = _targetRigidbodyId;
				for (int i = 0; i < item.contactCount; i++)
				{
					Vector3 vector = _suspensionLocalMatrix.MultiplyPoint3x4(item.GetPoint(i));
					if (!(vector.y < 0f))
					{
						continue;
					}
					Vector3 normal = item.GetNormal(i);
					if (Mathf.Abs(Vector3.Dot(_suspensionLocalMatrix.MultiplyVector(normal), _suspensionLocalForward)) > 0.3f)
					{
						float separation = item.GetSeparation(i);
						if (separation < 0f)
						{
							float num = 1f - Mathf.Clamp01((0f - vector.y) / wheel.radius);
							item.SetSeparation(i, separation * num);
						}
					}
				}
			}
		}
	}
}
