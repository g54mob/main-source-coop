using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtColor
	{
		public static Vector4 PToVec4(this Color me)
		{
			return new Vector4(me.r, me.g, me.b, me.a);
		}

		public static Color LinearToGamma(Color color)
		{
			float e = 0.45454544f;
			return new Color(pow(color.r, e), pow(color.g, e), pow(color.b, e), pow(color.a, e));
			static float pow(float x, float p)
			{
				return Mathf.Pow(x, p);
			}
		}

		public static Color ToGamma(this Color me)
		{
			return LinearToGamma(me);
		}

		public static Color32 ToGamma(this Color32 me)
		{
			return LinearToGamma(me);
		}

		public static Color32 Black32()
		{
			return new Color32(0, 0, 0, 0);
		}

		public static Vector4 PToVec4(this Color32 me)
		{
			return new Vector4((float)(int)me.r / 255f, (float)(int)me.g / 255f, (float)(int)me.b / 255f, (float)(int)me.a / 255f);
		}
	}
}
