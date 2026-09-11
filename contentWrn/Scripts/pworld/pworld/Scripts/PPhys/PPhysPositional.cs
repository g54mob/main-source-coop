using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysPositional : PPhysSpringBase
	{
		public Transform transTarget;

		public override Vector3 Target
		{
			get
			{
				return transTarget.position;
			}
			set
			{
				transTarget.position = value;
			}
		}

		public override Vector3 Current
		{
			get
			{
				return base.transform.position;
			}
			set
			{
				base.transform.position = value;
			}
		}

		public override void Awake()
		{
			SetTargetOnAwake = false;
			base.Awake();
		}
	}
}
