using UnityEngine;

namespace NWH.Common.Vehicles
{
	public abstract class WheelUAPI : MonoBehaviour
	{
		public abstract float MotorTorque { get; set; }

		public abstract float BrakeTorque { get; set; }

		public abstract float CounterTorque { get; }

		public abstract float RollingResistanceTorque { get; set; }

		public abstract float SteerAngle { get; set; }

		public abstract float Mass { get; set; }

		public abstract float Radius { get; set; }

		public abstract float Width { get; set; }

		public abstract float Inertia { get; set; }

		public abstract float RPM { get; }

		public abstract float AngularVelocity { get; }

		public abstract Vector3 WheelPosition { get; }

		public abstract float Load { get; }

		public abstract float MaxLoad { get; set; }

		public abstract bool IsGrounded { get; }

		public abstract float Damage { get; set; }

		public abstract float Camber { get; set; }

		public abstract float SpringMaxLength { get; set; }

		public abstract float SpringMaxForce { get; set; }

		public abstract float SpringForce { get; }

		public abstract float SpringLength { get; }

		public abstract float SpringCompression { get; }

		public abstract float DamperBumpRate { get; set; }

		public abstract float DamperReboundRate { get; set; }

		public abstract float DamperForce { get; }

		public abstract FrictionPreset FrictionPreset { get; set; }

		public abstract float LongitudinalFrictionGrip { get; set; }

		public abstract float LongitudinalFrictionStiffness { get; set; }

		public abstract float LateralFrictionGrip { get; set; }

		public abstract float LateralFrictionStiffness { get; set; }

		public abstract float ForceApplicationPointDistance { get; set; }

		public abstract float LongitudinalSlip { get; }

		public abstract float LongitudinalSpeed { get; }

		public virtual bool IsSkiddingLongitudinally => NormalizedLongitudinalSlip > 0.35f;

		public virtual float NormalizedLongitudinalSlip
		{
			get
			{
				float longitudinalSlip = LongitudinalSlip;
				float num = ((longitudinalSlip < 0f) ? (0f - longitudinalSlip) : longitudinalSlip);
				if (!(num < 0f))
				{
					if (!(num > 1f))
					{
						return num;
					}
					return 1f;
				}
				return 0f;
			}
		}

		public abstract float LateralSlip { get; }

		public abstract float LateralSpeed { get; }

		public virtual bool IsSkiddingLaterally => NormalizedLateralSlip > 0.35f;

		public virtual float NormalizedLateralSlip
		{
			get
			{
				float lateralSlip = LateralSlip;
				float num = ((lateralSlip < 0f) ? (0f - lateralSlip) : lateralSlip);
				if (!(num < 0f))
				{
					if (!(num > 1f))
					{
						return num;
					}
					return 1f;
				}
				return 0f;
			}
		}

		public abstract float FrictionCircleShape { get; set; }

		public abstract float FrictionCircleStrength { get; set; }

		public abstract Vector3 HitPoint { get; }

		public abstract Vector3 HitNormal { get; }

		public abstract Collider HitCollider { get; }

		public abstract GameObject WheelVisual { get; set; }

		public abstract GameObject NonRotatingVisual { get; set; }

		public abstract Rigidbody TargetRigidbody { get; }

		public abstract bool AutoSimulate { get; set; }

		public virtual void WakeFromSleep()
		{
		}

		public virtual void Step()
		{
		}

		public virtual void Validate()
		{
		}
	}
}
