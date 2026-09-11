using System;
using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public abstract class PBurstSpringWorkerPosition : PBurstSpringWorkerBase<Vector3, SpringJobPosition.SpringData, SpringJobPosition, PBurstSpringManagerPosition>
	{
		public override PBurstSpringManagerPosition Manager => PBurstSpringManagerBase<SpringJobPosition, SpringJobPosition.SpringData, Vector3>.Me as PBurstSpringManagerPosition;

		protected override SpringJobPosition.SpringData GetDefaultData()
		{
			throw new NotImplementedException();
		}
	}
}
