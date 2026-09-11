using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public class PBurstSpringManagerRotation : PBurstSpringManagerBase<SpringJobRotation, SpringJobRotation.SpringData, Quaternion>
	{
		protected override SpringJobRotation GetNewJob()
		{
			return new SpringJobRotation
			{
				velocities = velocities,
				springDatas = datas,
				dt = Time.deltaTime,
				targets = targets
			};
		}
	}
}
