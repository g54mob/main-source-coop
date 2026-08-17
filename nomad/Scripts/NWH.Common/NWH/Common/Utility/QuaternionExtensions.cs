using UnityEngine;

namespace NWH.Common.Utility
{
	public static class QuaternionExtensions
	{
		public static Quaternion Lerp(Quaternion p, Quaternion q, float t, bool shortWay)
		{
			if (shortWay && Quaternion.Dot(p, q) < 0f)
			{
				return Lerp(ScalarMultiply(p, -1f), q, t, shortWay: true);
			}
			Quaternion identity = Quaternion.identity;
			identity.x = p.x * (1f - t) + q.x * t;
			identity.y = p.y * (1f - t) + q.y * t;
			identity.z = p.z * (1f - t) + q.z * t;
			identity.w = p.w * (1f - t) + q.w * t;
			return identity;
		}

		public static Quaternion Slerp(Quaternion p, Quaternion q, float t, bool shortWay)
		{
			float num = Quaternion.Dot(p, q);
			if (shortWay && num < 0f)
			{
				return Slerp(ScalarMultiply(p, -1f), q, t, shortWay: true);
			}
			float num2 = Mathf.Acos(num);
			Quaternion p2 = ScalarMultiply(p, Mathf.Sin((1f - t) * num2));
			Quaternion q2 = ScalarMultiply(q, Mathf.Sin(t * num2));
			return ScalarMultiply(scalar: 1f / Mathf.Sin(num2), input: Add(p2, q2));
		}

		public static Quaternion ScalarMultiply(Quaternion input, float scalar)
		{
			return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
		}

		public static Quaternion Add(Quaternion p, Quaternion q)
		{
			return new Quaternion(p.x + q.x, p.y + q.y, p.z + q.z, p.w + q.w);
		}
	}
}
