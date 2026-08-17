using UnityEngine;

namespace ECM2
{
	public static class MathLib
	{
		public static float Remap(float inA, float inB, float outA, float outB, float value)
		{
			float t = Mathf.InverseLerp(inA, inB, value);
			return Mathf.Lerp(outA, outB, t);
		}

		public static float Square(float value)
		{
			return value * value;
		}

		public static Vector3 GetTangent(Vector3 direction, Vector3 normal, Vector3 up)
		{
			Vector3 otherVector = direction.perpendicularTo(up);
			return normal.perpendicularTo(otherVector);
		}

		public static Vector3 ProjectPointOnPlane(Vector3 point, Vector3 planeOrigin, Vector3 planeNormal)
		{
			Vector3 vector = Vector3.Project(point - planeOrigin, planeNormal);
			return point - vector;
		}

		public static float ClampAngle(float a, float min, float max)
		{
			while (max < min)
			{
				max += 360f;
			}
			while (a > max)
			{
				a -= 360f;
			}
			while (a < min)
			{
				a += 360f;
			}
			if (!(a > max))
			{
				return a;
			}
			if (!(a - (max + min) * 0.5f < 180f))
			{
				return min;
			}
			return max;
		}

		public static float ClampAngle(float angle)
		{
			angle %= 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle;
		}

		public static float NormalizeAngle(float angle)
		{
			angle = ClampAngle(angle);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			return angle;
		}

		private static float Clamp0360(float eulerAngles)
		{
			float num = eulerAngles - (float)Mathf.CeilToInt(eulerAngles / 360f) * 360f;
			if (num < 0f)
			{
				num += 360f;
			}
			return num;
		}

		public static float FixedTurn(float current, float target, float maxDegreesDelta)
		{
			if (maxDegreesDelta == 0f)
			{
				return Clamp0360(current);
			}
			if (maxDegreesDelta >= 360f)
			{
				return Clamp0360(target);
			}
			float num = Clamp0360(current);
			current = num;
			target = Clamp0360(target);
			num = ((current > target) ? ((!(current - target < 180f)) ? (num + Mathf.Min(target + 360f - current, Mathf.Abs(maxDegreesDelta))) : (num - Mathf.Min(current - target, Mathf.Abs(maxDegreesDelta)))) : ((!(target - current < 180f)) ? (num - Mathf.Min(current + 360f - target, Mathf.Abs(maxDegreesDelta))) : (num + Mathf.Min(target - current, Mathf.Abs(maxDegreesDelta)))));
			return Clamp0360(num);
		}

		public static float Damp(float a, float b, float lambda, float dt)
		{
			return Mathf.Lerp(a, b, 1f - Mathf.Exp((0f - lambda) * dt));
		}

		public static Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
		{
			return Vector3.Lerp(a, b, 1f - Mathf.Exp((0f - lambda) * dt));
		}
	}
}
