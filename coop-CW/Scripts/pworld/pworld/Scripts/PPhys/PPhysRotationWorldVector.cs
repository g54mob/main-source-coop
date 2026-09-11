using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysRotationWorldVector : PPhysRotationBase
	{
		public override Vector3 TargetUp => Vector3.up;

		public override Quaternion CurrentRotation
		{
			get
			{
				return base.transform.rotation;
			}
			set
			{
				base.transform.rotation = value;
			}
		}

		public override void Rotate(Vector3 dRot)
		{
			base.transform.Rotate(dRot, Space.World);
		}
	}
}
