using UnityEngine;

namespace NWH.Common.Utility
{
	public static class AnimationCurveExtensions
	{
		public static AnimationCurve MakeSmooth(this AnimationCurve inCurve)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			Vector2 vector = default(Vector2);
			Vector2 vector2 = default(Vector2);
			for (int i = 0; i < inCurve.keys.Length; i++)
			{
				float inTangent = 0f;
				float outTangent = 0f;
				bool flag = false;
				bool flag2 = false;
				Keyframe key = inCurve[i];
				if (i == 0)
				{
					inTangent = 0f;
					flag = true;
				}
				if (i == inCurve.keys.Length - 1)
				{
					outTangent = 0f;
					flag2 = true;
				}
				if (!flag)
				{
					vector.x = inCurve.keys[i - 1].time;
					vector.y = inCurve.keys[i - 1].value;
					vector2.x = inCurve.keys[i].time;
					vector2.y = inCurve.keys[i].value;
					Vector2 vector3 = vector2 - vector;
					inTangent = vector3.y / vector3.x;
				}
				if (!flag2)
				{
					vector.x = inCurve.keys[i].time;
					vector.y = inCurve.keys[i].value;
					vector2.x = inCurve.keys[i + 1].time;
					vector2.y = inCurve.keys[i + 1].value;
					Vector2 vector3 = vector2 - vector;
					outTangent = vector3.y / vector3.x;
				}
				key.inTangent = inTangent;
				key.outTangent = outTangent;
				animationCurve.AddKey(key);
			}
			return animationCurve;
		}

		public static float[] GenerateCurveArray(this AnimationCurve self, int resolution = 256)
		{
			float[] array = new float[resolution];
			for (int i = 0; i < resolution; i++)
			{
				array[i] = self.Evaluate((float)i / (float)resolution);
			}
			return array;
		}
	}
}
