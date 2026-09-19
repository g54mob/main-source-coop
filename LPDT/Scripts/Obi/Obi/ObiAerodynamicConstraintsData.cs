using System;

namespace Obi
{
	[Serializable]
	public class ObiAerodynamicConstraintsData : ObiConstraints<ObiAerodynamicConstraintsBatch>
	{
		public override ObiAerodynamicConstraintsBatch CreateBatch(ObiAerodynamicConstraintsBatch source = null)
		{
			return new ObiAerodynamicConstraintsBatch();
		}
	}
}
