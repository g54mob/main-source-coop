using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class ASDF
	{
		private static readonly Vector4[] corners = new Vector4[8]
		{
			new Vector4(-1f, -1f, -1f, -1f),
			new Vector4(-1f, -1f, 1f, -1f),
			new Vector4(-1f, 1f, -1f, -1f),
			new Vector4(-1f, 1f, 1f, -1f),
			new Vector4(1f, -1f, -1f, -1f),
			new Vector4(1f, -1f, 1f, -1f),
			new Vector4(1f, 1f, -1f, -1f),
			new Vector4(1f, 1f, 1f, -1f)
		};

		private static readonly Vector4[] samples = new Vector4[19]
		{
			new Vector4(0f, 0f, 0f, 0f),
			new Vector4(1f, 0f, 0f, 0f),
			new Vector4(-1f, 0f, 0f, 0f),
			new Vector4(0f, 1f, 0f, 0f),
			new Vector4(0f, -1f, 0f, 0f),
			new Vector4(0f, 0f, 1f, 0f),
			new Vector4(0f, 0f, -1f, 0f),
			new Vector4(0f, -1f, -1f, 0f),
			new Vector4(0f, -1f, 1f, 0f),
			new Vector4(0f, 1f, -1f, 0f),
			new Vector4(0f, 1f, 1f, 0f),
			new Vector4(-1f, 0f, -1f, 0f),
			new Vector4(-1f, 0f, 1f, 0f),
			new Vector4(1f, 0f, -1f, 0f),
			new Vector4(1f, 0f, 1f, 0f),
			new Vector4(-1f, -1f, 0f, 0f),
			new Vector4(-1f, 1f, 0f, 0f),
			new Vector4(1f, -1f, 0f, 0f),
			new Vector4(1f, 1f, 0f, 0f)
		};

		private const float sqrt3 = 1.73205f;

		public static IEnumerator Build(float maxError, int maxDepth, Vector3[] vertexPositions, int[] triangleIndices, List<DFNode> nodes, int yieldAfterNodeCount = 32)
		{
			if (maxDepth <= 0 || nodes == null || vertexPositions == null || vertexPositions.Length == 0 || triangleIndices == null || triangleIndices.Length == 0)
			{
				yield break;
			}
			IBounded[] elements = new IBounded[triangleIndices.Length / 3];
			for (int i = 0; i < elements.Length; i++)
			{
				int num = triangleIndices[i * 3];
				int num2 = triangleIndices[i * 3 + 1];
				int num3 = triangleIndices[i * 3 + 2];
				elements[i] = new Triangle(num, num2, num3, vertexPositions[num], vertexPositions[num2], vertexPositions[num3]);
			}
			BIHNode[] bih = BIH.Build(ref elements);
			Triangle[] tris = Array.ConvertAll(elements, (IBounded x) => (Triangle)(object)x);
			Vector3[] angleNormals = ObiUtils.CalculateAngleWeightedNormals(vertexPositions, triangleIndices);
			Bounds bounds = new Bounds(vertexPositions[0], Vector3.zero);
			for (int num4 = 1; num4 < vertexPositions.Length; num4++)
			{
				bounds.Encapsulate(vertexPositions[num4]);
			}
			bounds.Expand(0.2f);
			int depth = 0;
			int nodesToNextLevel = 1;
			Vector4 center = bounds.center;
			Vector3 extents = bounds.extents;
			center[3] = Mathf.Max(extents[0], Math.Max(extents[1], extents[2]));
			nodes.Clear();
			nodes.Add(new DFNode(center));
			Queue<int> queue = new Queue<int>();
			queue.Enqueue(0);
			int processedNodeCount = 0;
			while (queue.Count > 0)
			{
				int index = queue.Dequeue();
				DFNode value = nodes[index];
				for (int num5 = 0; num5 < 8; num5++)
				{
					Vector4 vector = value.center + corners[num5] * value.center[3];
					vector[3] = 0f;
					float value2 = BIH.DistanceToSurface(bih, tris, vertexPositions, angleNormals, (Vector3)vector);
					if (num5 < 4)
					{
						value.distancesA[num5] = value2;
					}
					else
					{
						value.distancesB[num5 - 4] = value2;
					}
				}
				if (depth < maxDepth && Mathf.Abs(BIH.DistanceToSurface(bih, tris, vertexPositions, angleNormals, (Vector3)value.center)) < value.center[3] * 1.73205f)
				{
					float num6 = 0f;
					for (int num7 = 0; num7 < samples.Length; num7++)
					{
						Vector4 vector2 = value.center + samples[num7] * value.center[3];
						float num8 = BIH.DistanceToSurface(bih, tris, vertexPositions, angleNormals, (Vector3)vector2);
						float num9 = value.Sample(vector2);
						float num10 = num8 - num9;
						num6 += num10 * num10;
					}
					num6 /= (float)samples.Length;
					if (num6 > maxError)
					{
						value.firstChild = nodes.Count;
						for (int num11 = 0; num11 < 8; num11++)
						{
							queue.Enqueue(nodes.Count);
							nodes.Add(new DFNode(value.center + corners[num11] * value.center[3] * 0.5f));
						}
					}
					int num12 = nodesToNextLevel - 1;
					nodesToNextLevel = num12;
					if (num12 == 0)
					{
						depth++;
						nodesToNextLevel = queue.Count;
					}
				}
				nodes[index] = value;
				if (nodes.Count - processedNodeCount >= yieldAfterNodeCount)
				{
					processedNodeCount = nodes.Count;
					yield return null;
				}
			}
		}

		public static float Sample(List<DFNode> nodes, Vector3 position)
		{
			if (nodes != null && nodes.Count > 0)
			{
				Queue<int> queue = new Queue<int>();
				queue.Enqueue(0);
				while (queue.Count > 0)
				{
					DFNode dFNode = nodes[queue.Dequeue()];
					if (dFNode.firstChild > -1)
					{
						queue.Enqueue(dFNode.firstChild + dFNode.GetOctant(position));
						continue;
					}
					return dFNode.Sample(position);
				}
			}
			return 0f;
		}
	}
}
