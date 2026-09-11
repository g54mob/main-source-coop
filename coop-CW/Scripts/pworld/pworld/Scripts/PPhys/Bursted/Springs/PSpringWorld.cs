using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Handlers;

namespace pworld.Scripts.PPhys.Bursted.Springs
{
	public class PSpringWorld : PBurstSpringWorkerPosition
	{
		public Vector3 target;

		public override Vector3 Target => target;

		public void Update()
		{
			UpdateTarget();
		}
	}
}
