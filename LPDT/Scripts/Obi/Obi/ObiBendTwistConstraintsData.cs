using System;

namespace Obi
{
	[Serializable]
	public class ObiBendTwistConstraintsData : ObiConstraints<ObiBendTwistConstraintsBatch>
	{
		public override ObiBendTwistConstraintsBatch CreateBatch(ObiBendTwistConstraintsBatch source = null)
		{
			return new ObiBendTwistConstraintsBatch();
		}
	}
}
