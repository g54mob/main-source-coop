using System;

namespace Obi
{
	[Serializable]
	public class ObiDistanceConstraintsData : ObiConstraints<ObiDistanceConstraintsBatch>
	{
		public override ObiDistanceConstraintsBatch CreateBatch(ObiDistanceConstraintsBatch source = null)
		{
			return new ObiDistanceConstraintsBatch();
		}
	}
}
