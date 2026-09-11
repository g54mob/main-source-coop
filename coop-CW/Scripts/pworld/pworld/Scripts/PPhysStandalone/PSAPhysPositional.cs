using UnityEngine;

namespace pworld.Scripts.PPhysStandalone
{
	public class PSAPhysPositional : PSAPhysSpringBase
	{
		public Vector3 target;

		public Vector3 current;

		public override Vector3 Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public override Vector3 Current
		{
			get
			{
				return current;
			}
			set
			{
				current = value;
			}
		}

		public override void Awake()
		{
			SetTargetOnAwake = false;
			base.Awake();
		}
	}
}
