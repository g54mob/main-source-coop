using System;

namespace EasyTextEffects.Editor.MyBoxCopy.Attributes
{
	[Serializable]
	public struct RangedInt
	{
		public int Min;

		public int Max;

		public RangedInt(int min, int max)
		{
			Min = min;
			Max = max;
		}
	}
}
