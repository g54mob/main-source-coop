using UnityEngine;

namespace EasyTextEffects.Editor.MyBoxCopy.Attributes
{
	public static class RangedExtensions
	{
		public static float LerpFromRange(this RangedFloat ranged, float t)
		{
			return Mathf.Lerp(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRangeUnclamped(this RangedFloat ranged, float t)
		{
			return Mathf.LerpUnclamped(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRange(this RangedInt ranged, float t)
		{
			return Mathf.Lerp(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRangeUnclamped(this RangedInt ranged, float t)
		{
			return Mathf.LerpUnclamped(ranged.Min, ranged.Max, t);
		}
	}
}
