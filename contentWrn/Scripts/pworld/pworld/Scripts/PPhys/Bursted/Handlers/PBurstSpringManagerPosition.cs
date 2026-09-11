using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public class PBurstSpringManagerPosition : PBurstSpringManagerBase<SpringJobPosition, SpringJobPosition.SpringData, Vector3>
	{
		protected override SpringJobPosition GetNewJob()
		{
			return new SpringJobPosition
			{
				dt = Time.deltaTime,
				springDatas = datas,
				targets = targets,
				velocities = velocities
			};
		}
	}
}
