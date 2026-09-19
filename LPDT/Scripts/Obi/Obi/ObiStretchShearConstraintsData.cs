using System;

namespace Obi
{
	[Serializable]
	public class ObiStretchShearConstraintsData : ObiConstraints<ObiStretchShearConstraintsBatch>
	{
		public override ObiStretchShearConstraintsBatch CreateBatch(ObiStretchShearConstraintsBatch source = null)
		{
			return new ObiStretchShearConstraintsBatch();
		}
	}
}
