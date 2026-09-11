using UnityEngine.Serialization;

namespace Zorro.Core.SmallShadows
{
	internal struct CheckResult
	{
		[FormerlySerializedAs("State")]
		public bool CastShadow;

		public int Index;

		public CheckResult(bool castShadow, int index)
		{
			CastShadow = castShadow;
			Index = index;
		}
	}
}
