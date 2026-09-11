using System;

namespace EasyTextEffects.Editor.MyBoxCopy.Types
{
	[Serializable]
	public struct MinMaxInt
	{
		public int Min;

		public int Max;

		public MinMaxInt(int min, int max)
		{
			Min = min;
			Max = max;
		}
	}
}
