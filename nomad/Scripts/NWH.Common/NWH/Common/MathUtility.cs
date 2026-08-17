namespace NWH.Common
{
	public static class MathUtility
	{
		public static void ClampWithRemainder(ref float x, in float range, out float remainder)
		{
			if (x > range)
			{
				remainder = x - range;
				x = range;
			}
			else if (x < 0f - range)
			{
				remainder = x + range;
				x = 0f - range;
			}
			else
			{
				remainder = 0f;
			}
		}
	}
}
