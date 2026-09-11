namespace Zorro.Core
{
	public static class AspectRatioUtility
	{
		public static AspectRatio GetAspectRatio(float aspect)
		{
			(AspectRatio, float)[] obj = new(AspectRatio, float)[5]
			{
				(AspectRatio._4x3, 1.3333334f),
				(AspectRatio._16x10, 1.6f),
				(AspectRatio._16x9, 1.7777778f),
				(AspectRatio._21x9, 2.3333333f),
				(AspectRatio._32x9, 3.5555556f)
			};
			AspectRatio result = AspectRatio.Default;
			(AspectRatio, float)[] array = obj;
			for (int i = 0; i < array.Length; i++)
			{
				(AspectRatio, float) tuple = array[i];
				if (!(aspect >= tuple.Item2 - 0.01f))
				{
					break;
				}
				result = tuple.Item1;
			}
			return result;
		}
	}
}
