using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Handlers;

namespace pworld.Scripts.PPhys.Bursted.Rotators
{
	public class PRotatorLookAt : PBurstSpringWorkerRotation
	{
		public Transform lookAt;

		public override Quaternion Target => Quaternion.LookRotation(LookAtDir, TargetUp);

		public virtual Vector3 LookAtDir => (lookAt.position - base.transform.position).normalized;

		public virtual Vector3 TargetUp => Vector3.up;

		private void Update()
		{
			UpdateTarget();
		}
	}
}
