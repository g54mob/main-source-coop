using System;

namespace Obi
{
	[Serializable]
	public class ObiVolumeConstraintsData : ObiConstraints<ObiVolumeConstraintsBatch>
	{
		public override ObiVolumeConstraintsBatch CreateBatch(ObiVolumeConstraintsBatch source = null)
		{
			return new ObiVolumeConstraintsBatch();
		}
	}
}
