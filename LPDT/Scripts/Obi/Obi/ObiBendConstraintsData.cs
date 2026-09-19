using System;

namespace Obi
{
	[Serializable]
	public class ObiBendConstraintsData : ObiConstraints<ObiBendConstraintsBatch>
	{
		public override ObiBendConstraintsBatch CreateBatch(ObiBendConstraintsBatch source = null)
		{
			return new ObiBendConstraintsBatch();
		}
	}
}
