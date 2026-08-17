using System;
using UnityEngine;

namespace Den.Tools.Splines
{
	[Serializable]
	public struct Node
	{
		public enum TangentType
		{
			auto = 0,
			linear = 1,
			correlated = 2,
			broken = 3
		}

		public Vector3 pos;

		public Vector3 dir;

		public TangentType type;

		public static (Vector3, Vector3) AutoTangents(Vector3 prevPos, Vector3 thisPos, Vector3 nextPos)
		{
			Vector3 vector = nextPos - thisPos;
			float magnitude = vector.magnitude;
			if (magnitude > 1E-05f)
			{
				vector /= magnitude;
			}
			else
			{
				vector = default(Vector3);
			}
			Vector3 vector2 = prevPos - thisPos;
			float magnitude2 = vector2.magnitude;
			if (magnitude2 > 1E-05f)
			{
				vector2 /= magnitude2;
			}
			else
			{
				vector2 = default(Vector3);
			}
			Vector3 normalized = (vector2 - vector).normalized;
			Vector3 vector3 = -normalized;
			vector2 = normalized.normalized * magnitude2 * 0.35f;
			vector = vector3.normalized * magnitude * 0.35f;
			return (vector2, vector);
		}

		public static (Vector3, Vector3) LinearTangents(Vector3 prevPos, Vector3 thisPos, Vector3 nextPos)
		{
			return ((prevPos - thisPos) * 0.333f, (nextPos - thisPos) * 0.333f);
		}

		public static Vector3 LinearTangent(Vector3 thisPos, Vector3 nextPos)
		{
			return (nextPos - thisPos) * 0.333f;
		}

		public static Vector3 CorrelatedOutTangent(Vector3 inDir, Vector3 outDir)
		{
			return -inDir.normalized * outDir.magnitude;
		}

		public static Vector3 CorrelatedInTangent(Vector3 inDir, Vector3 outDir)
		{
			return -outDir.normalized * inDir.magnitude;
		}
	}
}
