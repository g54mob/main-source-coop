using System;

namespace Obi
{
	[Serializable]
	public class ObiTetherConstraintsData : ObiConstraints<ObiTetherConstraintsBatch>
	{
		public override ObiTetherConstraintsBatch CreateBatch(ObiTetherConstraintsBatch source = null)
		{
			return new ObiTetherConstraintsBatch();
		}
	}
}
