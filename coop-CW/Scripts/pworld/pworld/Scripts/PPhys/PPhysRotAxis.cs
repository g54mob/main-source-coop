using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysRotAxis : PPhysRotationBase
	{
		public Transform inSpaceOf;

		public Transform lookAt;

		private Vector3 current;

		public override Vector3 Target
		{
			get
			{
				return (lookAt.position - inSpaceOf.position).normalized;
			}
			set
			{
				lookAt.forward = value;
			}
		}

		public override Vector3 TargetUp => inSpaceOf.TransformDirection(Vector3.forward);

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
