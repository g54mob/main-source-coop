using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public abstract class PPhysRotationBase : PPhysSpringBase
	{
		public abstract Vector3 TargetUp { get; }

		public abstract Quaternion CurrentRotation { get; set; }

		public override Vector3 Current
		{
			get
			{
				return base.transform.forward;
			}
			set
			{
				base.transform.rotation = Quaternion.LookRotation(value, TargetUp);
			}
		}

		protected override void PhysicsStep(float dt)
		{
			Vector3 vector = Vector3.Cross(base.transform.up, TargetUp).normalized * Vector3.Angle(base.transform.up, TargetUp);
			Vector3 vector2 = Vector3.Cross(base.transform.forward, Target).normalized * Vector3.Angle(base.transform.forward, Target);
			velocity = FRILerp.PLerp(velocity, (vector2 + vector) * spring, damp, dt);
			Rotate(velocity * dt);
		}

		public abstract void Rotate(Vector3 dRot);
	}
}
