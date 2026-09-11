using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysRotationLocal : PPhysRotationBase
	{
		public Vector3 mUp = Vector3.up;

		public override Vector3 Target
		{
			get
			{
				return base.transform.parent.TransformDirection(_target);
			}
			set
			{
				_target = value;
			}
		}

		public override Vector3 TargetUp => base.transform.parent.TransformDirection(mUp);

		public override Quaternion CurrentRotation
		{
			get
			{
				return base.transform.localRotation;
			}
			set
			{
				base.transform.localRotation = value;
			}
		}

		public override void Rotate(Vector3 dRot)
		{
			dRot = base.transform.InverseTransformVector(dRot);
			base.transform.Rotate(dRot, Space.Self);
		}
	}
}
