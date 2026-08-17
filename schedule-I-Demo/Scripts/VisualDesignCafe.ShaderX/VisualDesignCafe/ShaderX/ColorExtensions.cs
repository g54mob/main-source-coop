using UnityEngine;

namespace VisualDesignCafe.ShaderX
{
	public static class ColorExtensions
	{
		public static bool Equals(this Color color, Color other, ColorComparison comparison)
		{
			bool flag = color.r == other.r;
			bool flag2 = color.g == other.g;
			bool flag3 = color.b == other.b;
			bool flag4 = color.a == other.a;
			if (comparison.HasFlag(ColorComparison.IgnoreAlpha))
			{
				flag4 = true;
			}
			return flag && flag2 && flag3 && flag4;
		}
	}
}
