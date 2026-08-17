using System;
using UnityEngine;

namespace NWH.Common.Vehicles
{
	[RequireComponent(typeof(WheelCollider))]
	public class WheelColliderUAPI : WheelUAPI
	{
		public GameObject wheelVisual;

		public float width = 0.3f;

		[SerializeField]
		private WheelCollider _wc;

		[SerializeField]
		private Rigidbody _rb;

		private WheelHit _wheelHit;

		private bool _isGrounded;

		private Vector3 _rbVelocity;

		private float _forwardSpeed;

		private float _sideSpeed;

		private float _inertia;

		private float _latFrictionStiffness;

		private float _latFrictionGrip;

		private float _lngFrictionStiffness;

		private float _lngFrictionGrip;

		public override float MotorTorque
		{
			get
			{
				return _wc.motorTorque;
			}
			set
			{
				_wc.motorTorque = value;
			}
		}

		public override float BrakeTorque
		{
			get
			{
				return _wc.brakeTorque;
			}
			set
			{
				_wc.brakeTorque = value;
			}
		}

		public override float SteerAngle
		{
			get
			{
				return _wc.steerAngle;
			}
			set
			{
				_wc.steerAngle = value;
			}
		}

		public override float Mass
		{
			get
			{
				return _wc.mass;
			}
			set
			{
				_wc.mass = value;
			}
		}

		public override float Inertia
		{
			get
			{
				return _inertia;
			}
			set
			{
				Mathf.Clamp(_inertia, 1E-06f, 1f / 0f);
			}
		}

		public override float Radius
		{
			get
			{
				return _wc.radius;
			}
			set
			{
				_wc.radius = value;
			}
		}

		public override float Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		public override float RPM => _wc.rpm;

		public override float AngularVelocity => _wc.rpm * ((float)Math.PI / 30f);

		public override Vector3 WheelPosition => base.transform.TransformPoint(_wc.center);

		public override float Load
		{
			get
			{
				if (!_isGrounded)
				{
					return 0f;
				}
				return _wheelHit.force;
			}
		}

		public override float MaxLoad
		{
			get
			{
				return _wc.forwardFriction.extremumValue;
			}
			set
			{
				WheelFrictionCurve forwardFriction = _wc.forwardFriction;
				forwardFriction.extremumValue = value;
				forwardFriction.asymptoteValue = forwardFriction.extremumValue * 0.7f;
				_wc.forwardFriction = forwardFriction;
			}
		}

		public override float Camber
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public override bool IsGrounded => _isGrounded;

		public override float Damage
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public override float SpringMaxLength
		{
			get
			{
				return _wc.suspensionDistance;
			}
			set
			{
				_wc.suspensionDistance = value;
			}
		}

		public override float SpringMaxForce
		{
			get
			{
				return _wc.suspensionSpring.spring;
			}
			set
			{
				JointSpring suspensionSpring = _wc.suspensionSpring;
				suspensionSpring.spring = value;
				_wc.suspensionSpring = suspensionSpring;
			}
		}

		public override float SpringForce
		{
			get
			{
				if (!_wc.isGrounded)
				{
					return 0f;
				}
				return _wheelHit.force;
			}
		}

		public override float SpringLength => 0f - _wc.center.y;

		public override float SpringCompression => SpringLength / SpringMaxLength;

		public override float DamperBumpRate
		{
			get
			{
				return _wc.suspensionSpring.damper;
			}
			set
			{
				JointSpring suspensionSpring = _wc.suspensionSpring;
				suspensionSpring.damper = value;
				_wc.suspensionSpring = suspensionSpring;
			}
		}

		public override float DamperReboundRate
		{
			get
			{
				return DamperBumpRate;
			}
			set
			{
				DamperBumpRate = value;
			}
		}

		public override float DamperForce => 0f;

		public override float LongitudinalSlip
		{
			get
			{
				if (!_isGrounded)
				{
					return 0f;
				}
				return _wheelHit.forwardSlip;
			}
		}

		public override float LongitudinalSpeed => _forwardSpeed;

		public override float LateralSlip
		{
			get
			{
				if (!_isGrounded)
				{
					return 0f;
				}
				return _wheelHit.sidewaysSlip;
			}
		}

		public override float LateralSpeed => _sideSpeed;

		public override Vector3 HitPoint
		{
			get
			{
				if (!_isGrounded)
				{
					return Vector3.zero;
				}
				return _wheelHit.point;
			}
		}

		public override GameObject WheelVisual
		{
			get
			{
				return wheelVisual;
			}
			set
			{
				wheelVisual = value;
			}
		}

		public override GameObject NonRotatingVisual
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public override Rigidbody TargetRigidbody => _rb;

		public override Vector3 HitNormal
		{
			get
			{
				if (!_isGrounded)
				{
					return Vector3.up;
				}
				return _wheelHit.normal;
			}
		}

		public override Collider HitCollider
		{
			get
			{
				if (!_isGrounded)
				{
					return null;
				}
				return _wheelHit.collider;
			}
		}

		public override float ForceApplicationPointDistance
		{
			get
			{
				return _wc.forceAppPointDistance;
			}
			set
			{
				_wc.forceAppPointDistance = value;
			}
		}

		public override FrictionPreset FrictionPreset
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public override float CounterTorque => 0f - _wc.motorTorque;

		public override float LongitudinalFrictionGrip
		{
			get
			{
				return _lngFrictionGrip;
			}
			set
			{
				_lngFrictionGrip = value;
			}
		}

		public override float LongitudinalFrictionStiffness
		{
			get
			{
				return _lngFrictionStiffness;
			}
			set
			{
				_lngFrictionStiffness = value;
			}
		}

		public override float LateralFrictionGrip
		{
			get
			{
				return _latFrictionGrip;
			}
			set
			{
				_latFrictionGrip = value;
			}
		}

		public override float LateralFrictionStiffness
		{
			get
			{
				return _latFrictionStiffness;
			}
			set
			{
				_latFrictionStiffness = value;
			}
		}

		public override float RollingResistanceTorque { get; set; }

		public override float FrictionCircleShape { get; set; }

		public override float FrictionCircleStrength { get; set; }

		public override bool AutoSimulate
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public override void Step()
		{
		}

		public override void Validate()
		{
		}

		private void Initialize()
		{
			_wc = GetComponent<WheelCollider>();
			_rb = GetComponentInParent<Rigidbody>();
			_wc.mass = 200f;
			_inertia = 0.5f * _wc.mass * _wc.radius * _wc.radius;
		}

		private void Reset()
		{
			Initialize();
		}

		private void Awake()
		{
			Initialize();
		}

		public void FixedUpdate()
		{
			_isGrounded = _wc.GetGroundHit(out _wheelHit);
			_rbVelocity = _rb.GetPointVelocity(WheelPosition);
			Vector3 vector = base.transform.InverseTransformVector(_rbVelocity);
			_forwardSpeed = vector.z;
			_sideSpeed = vector.x;
			_wc.GetWorldPose(out var pos, out var quat);
			wheelVisual.transform.SetPositionAndRotation(pos, quat);
		}
	}
}
