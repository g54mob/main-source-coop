#define DEBUG
using System;
using UnityEngine;

namespace Fusion
{
	public class PhysicsBody3D : AbstractPhysicsBody
	{
		private readonly Rigidbody _rb;

		public Rigidbody Rigidbody => _rb;

		public override Vector3 LinearVelocity
		{
			get
			{
				return _rb.velocity;
			}
			set
			{
				_rb.velocity = value;
			}
		}

		public override Vector3 AngularVelocity
		{
			get
			{
				return _rb.angularVelocity;
			}
			set
			{
				_rb.angularVelocity = value;
			}
		}

		public override Vector3 Position
		{
			get
			{
				return _rb.position;
			}
			set
			{
				_rb.position = value;
			}
		}

		public override Quaternion Rotation
		{
			get
			{
				return _rb.rotation;
			}
			set
			{
				_rb.rotation = value;
			}
		}

		public override Vector3 Gravity => Physics.gravity;

		public override float Drag
		{
			get
			{
				return _rb.drag;
			}
			set
			{
				_rb.drag = value;
			}
		}

		public override float AngularDrag
		{
			get
			{
				return _rb.angularDrag;
			}
			set
			{
				_rb.angularDrag = value;
			}
		}

		public override float Mass
		{
			get
			{
				return _rb.mass;
			}
			set
			{
				_rb.mass = value;
			}
		}

		public override int EncodedConstraints
		{
			get
			{
				return (int)_rb.constraints;
			}
			set
			{
				_rb.constraints = (RigidbodyConstraints)value;
			}
		}

		public override bool Kinematic
		{
			get
			{
				return _rb.isKinematic;
			}
			set
			{
				_rb.isKinematic = value;
			}
		}

		public override Vector3 InertiaTensor
		{
			get
			{
				return _rb.inertiaTensor;
			}
			set
			{
				_rb.inertiaTensor = value;
			}
		}

		public override BodyInterpolation Interpolation
		{
			get
			{
				return (BodyInterpolation)_rb.interpolation;
			}
			set
			{
				_rb.interpolation = (RigidbodyInterpolation)value;
			}
		}

		public override Transform Transform => _rb.transform;

		public override Component Component => _rb;

		public override bool Valid => _rb;

		public override bool Sleeping
		{
			get
			{
				return _rb.IsSleeping();
			}
			set
			{
				if (value)
				{
					_rb.Sleep();
				}
				else
				{
					_rb.WakeUp();
				}
			}
		}

		public override NetworkRigidbodyFlags Flags
		{
			get
			{
				NetworkRigidbodyFlags networkRigidbodyFlags = (NetworkRigidbodyFlags)0;
				if (_rb.isKinematic)
				{
					networkRigidbodyFlags |= NetworkRigidbodyFlags.IsKinematic;
				}
				if (_rb.IsSleeping())
				{
					networkRigidbodyFlags |= NetworkRigidbodyFlags.IsSleeping;
				}
				return networkRigidbodyFlags;
			}
		}

		public PhysicsBody3D(Rigidbody rb)
		{
			_rb = rb;
			Assert.Check(condition: true, "(int)BodyInterpolation.None == (int)RigidbodyInterpolation.None");
			Assert.Check(condition: true, "(int)BodyInterpolation.Interpolate == (int)RigidbodyInterpolation.Interpolate");
			Assert.Check(condition: true, "(int)BodyInterpolation.Extrapolate == (int)RigidbodyInterpolation.Extrapolate");
			Assert.Check(Enum.GetValues(typeof(BodyInterpolation)).Length == Enum.GetValues(typeof(RigidbodyInterpolation)).Length, "Enum.GetValues(typeof(BodyInterpolation)).Length == Enum.GetValues(typeof(RigidbodyInterpolation)).Length");
		}

		public override void AddForce(Vector3 force)
		{
			_rb.AddForce(force);
		}

		public override void AddTorque(Vector3 torque)
		{
			_rb.AddTorque(torque);
		}
	}
}
