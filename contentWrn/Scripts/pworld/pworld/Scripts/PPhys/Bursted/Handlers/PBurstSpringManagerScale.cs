using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public class PBurstSpringManagerScale : PBurstSpringManagerBase<SpringJobScale, SpringJobScale.SpringData, Vector3>
	{
		protected override SpringJobScale GetNewJob()
		{
			return new SpringJobScale
			{
				velocities = velocities,
				springDatas = datas,
				dt = Time.deltaTime,
				targets = targets
			};
		}
	}
}
