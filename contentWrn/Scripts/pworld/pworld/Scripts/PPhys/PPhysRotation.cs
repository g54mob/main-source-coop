using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysRotation : PPhysRotationBase
	{
		public Transform lookAt;

		public override Vector3 Target
		{
			get
			{
				return (lookAt.position - base.transform.position).normalized;
			}
			set
			{
				lookAt.transform.position = value;
			}
		}

		public override Vector3 TargetUp => Vector3.Cross(Target, base.transform.right).normalized;

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

		public override void Awake()
		{
			SetTargetOnAwake = false;
			base.Awake();
		}

		public override void Rotate(Vector3 dRot)
		{
			base.transform.Rotate(dRot, Space.World);
		}
	}
}
