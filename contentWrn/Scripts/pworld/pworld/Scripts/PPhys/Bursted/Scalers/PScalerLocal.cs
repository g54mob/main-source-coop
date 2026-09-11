using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Handlers;

namespace pworld.Scripts.PPhys.Bursted.Scalers
{
	public class PScalerLocal : PBurstSpringWorkerScale
	{
		public Vector3 target;

		public override Vector3 Target => target;

		public void Update()
		{
			UpdateTarget();
		}
	}
}
