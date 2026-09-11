using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public abstract class PBurstSpringWorkerScale : PBurstSpringWorkerBase<Vector3, SpringJobScale.SpringData, SpringJobScale, PBurstSpringManagerScale>
	{
		public override PBurstSpringManagerScale Manager => PBurstSpringManagerBase<SpringJobScale, SpringJobScale.SpringData, Vector3>.Me as PBurstSpringManagerScale;

		protected override SpringJobScale.SpringData GetDefaultData()
		{
			return new SpringJobScale.SpringData
			{
				spring = 15f,
				damp = 15f
			};
		}
	}
}
