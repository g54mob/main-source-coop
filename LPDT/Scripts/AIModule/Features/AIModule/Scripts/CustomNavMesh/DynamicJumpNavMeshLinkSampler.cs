using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts.CustomNavMesh
{
	public static class DynamicJumpNavMeshLinkSampler
	{
		private static readonly List<Vector3> _hits = new List<Vector3>(16);

		private static readonly List<int> _hitIndices = new List<int>(16);

		private static readonly List<JumpLinkPair> _pairs = new List<JumpLinkPair>(16);

		public static void SamplePairs(Vector3 center, float horizontalRadius, int sampleDirections, float sampleDistance, float sampleRadius, int areaMask, int maxLinks, List<JumpLinkPair> results, List<Vector3> candidateHits = null)
		{
			results.Clear();
			candidateHits?.Clear();
			_hits.Clear();
			_hitIndices.Clear();
			_pairs.Clear();
			if (sampleDirections < 2 || maxLinks <= 0)
			{
				return;
			}
			float num = Mathf.Max(0.05f, horizontalRadius + sampleDistance);
			for (int i = 0; i < sampleDirections; i++)
			{
				float f = (float)i * MathF.PI * 2f / (float)sampleDirections;
				Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * num;
				if (NavMesh.SamplePosition(center + vector, out var hit, sampleRadius, areaMask))
				{
					_hits.Add(hit.position);
					_hitIndices.Add(i);
					candidateHits?.Add(hit.position);
				}
			}
			if (_hits.Count < 2)
			{
				return;
			}
			int num2 = sampleDirections / 2;
			for (int j = 0; j < _hits.Count; j++)
			{
				int num3 = _hitIndices[j];
				for (int k = j + 1; k < _hits.Count; k++)
				{
					int num4 = _hitIndices[k];
					int num5 = Mathf.Abs(num3 - num4);
					int num6 = Mathf.Min(num5, sampleDirections - num5);
					if (num6 >= num2 - 1)
					{
						Vector3 vector2 = _hits[j];
						Vector3 vector3 = _hits[k];
						Vector3 vector4 = (vector2 + vector3) * 0.5f;
						Vector3 vector5 = center - vector4;
						vector5.y = 0f;
						float num7 = Vector3.Distance(new Vector3(vector2.x, 0f, vector2.z), new Vector3(vector3.x, 0f, vector3.z));
						float num8 = 1f / (1f + vector5.sqrMagnitude);
						float num9 = (float)num6 / (float)num2;
						float score = num7 * num9 * num8;
						_pairs.Add(new JumpLinkPair(vector2, vector3, score));
					}
				}
			}
			_pairs.Sort((JumpLinkPair left, JumpLinkPair right) => right.Score.CompareTo(left.Score));
			for (int num10 = 0; num10 < _pairs.Count; num10++)
			{
				if (results.Count >= maxLinks)
				{
					break;
				}
				JumpLinkPair jumpLinkPair = _pairs[num10];
				if (!IsTooSimilar(jumpLinkPair, results))
				{
					results.Add(jumpLinkPair);
				}
			}
		}

		private static bool IsTooSimilar(JumpLinkPair candidate, List<JumpLinkPair> accepted)
		{
			for (int i = 0; i < accepted.Count; i++)
			{
				JumpLinkPair jumpLinkPair = accepted[i];
				if (EndpointsClose(candidate.Start, jumpLinkPair.Start, jumpLinkPair.End, 0.25f) && EndpointsClose(candidate.End, jumpLinkPair.Start, jumpLinkPair.End, 0.25f))
				{
					return true;
				}
			}
			return false;
		}

		private static bool EndpointsClose(Vector3 point, Vector3 a, Vector3 b, float minSqr)
		{
			Vector3 vector = new Vector3(point.x, 0f, point.z);
			Vector3 vector2 = new Vector3(a.x, 0f, a.z);
			Vector3 vector3 = new Vector3(b.x, 0f, b.z);
			if (!((vector - vector2).sqrMagnitude <= minSqr))
			{
				return (vector - vector3).sqrMagnitude <= minSqr;
			}
			return true;
		}
	}
}
