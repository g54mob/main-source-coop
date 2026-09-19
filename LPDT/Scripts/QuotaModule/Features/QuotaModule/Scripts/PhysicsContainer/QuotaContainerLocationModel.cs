using UnityEngine;

namespace Features.QuotaModule.Scripts.PhysicsContainer
{
	public class QuotaContainerLocationModel
	{
		public Transform Drop { get; private set; }

		public Transform Approach { get; private set; }

		public bool HasContainer => Drop != null;

		public bool HasApproach => Approach != null;

		public void SetContainer(Transform drop, Transform approach)
		{
			Drop = drop;
			Approach = approach;
		}

		public void ClearContainer(Transform drop)
		{
			if (!(Drop != drop))
			{
				Drop = null;
				Approach = null;
			}
		}
	}
}
