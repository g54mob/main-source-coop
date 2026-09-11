using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Handlers;

namespace pworld.Scripts.PPhys.Bursted.Springs
{
	public class PSpringFollow : PBurstSpringWorkerPosition
	{
		public Transform target;

		public override Vector3 Target => target.position;

		public void Update()
		{
			UpdateTarget();
		}
	}
}
