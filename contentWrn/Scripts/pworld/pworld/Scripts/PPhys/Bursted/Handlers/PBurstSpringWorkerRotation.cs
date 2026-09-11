using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public abstract class PBurstSpringWorkerRotation : PBurstSpringWorkerBase<Quaternion, SpringJobRotation.SpringData, SpringJobRotation, PBurstSpringManagerRotation>
	{
		public override PBurstSpringManagerRotation Manager => PBurstSpringManagerBase<SpringJobRotation, SpringJobRotation.SpringData, Quaternion>.Me as PBurstSpringManagerRotation;

		protected override SpringJobRotation.SpringData GetDefaultData()
		{
			return new SpringJobRotation.SpringData
			{
				spring = 15f,
				damp = 15f
			};
		}
	}
}
