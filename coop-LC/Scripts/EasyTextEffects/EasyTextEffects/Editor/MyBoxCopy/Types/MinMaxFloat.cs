using System;

namespace EasyTextEffects.Editor.MyBoxCopy.Types
{
	[Serializable]
	public struct MinMaxFloat
	{
		public float Min;

		public float Max;

		public MinMaxFloat(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}
}
