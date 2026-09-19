using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class BIH
	{
		public static BIHNode[] Build(ref IBounded[] elements, int maxDepth = 10, float maxOverlap = 0.7f)
		{
			List<BIHNode> list = new List<BIHNode>
			{
				new BIHNode(0, elements.Length)
			};
			int num = 0;
			int num2 = 1;
			Queue<int> queue = new Queue<int>();
			queue.Enqueue(0);
			while (queue.Count > 0)
			{
				int index = queue.Dequeue();
				BIHNode node = list[index];
				if (node.count <= 2)
				{
					continue;
				}
				int start = node.start;
				int num3 = start + (node.count - 1);
				Aabb bounds = elements[start].GetBounds();
				for (int i = start + 1; i <= num3; i++)
				{
					bounds.Encapsulate(elements[i].GetBounds());
				}
				Vector3 vector = bounds.size;
				int num4 = (node.axis = ((!(vector.x > vector.y)) ? ((vector.y > vector.z) ? 1 : 2) : ((!(vector.x > vector.z)) ? 2 : 0)));
				float pivot = bounds.min[num4] + vector[num4] * 0.5f;
				int num5 = HoarePartition(elements, start, num3, pivot, ref node, num4);
				BIHNode item = new BIHNode(start, num5 - start + 1);
				BIHNode item2 = new BIHNode(num5 + 1, num3 - num5);
				if (((vector[num4] > 0f) ? (Mathf.Max(node.min - node.max, 0f) / vector[num4]) : 1f) <= maxOverlap && item.count > 0 && item2.count > 0)
				{
					node.firstChild = list.Count;
					list[index] = node;
					queue.Enqueue(list.Count);
					queue.Enqueue(list.Count + 1);
					list.Add(item);
					list.Add(item2);
				}
				if (--num2 == 0)
				{
					num++;
					if (num >= maxDepth)
					{
						return list.ToArray();
					}
					num2 = queue.Count;
				}
			}
			return list.ToArray();
		}

		public static int HoarePartition(IBounded[] elements, int start, int end, float pivot, ref BIHNode node, int axis)
		{
			int num = start;
			int num2 = end;
			while (num <= num2)
			{
				while (num < end && elements[num].GetBounds().center[axis] < pivot)
				{
					node.min = Mathf.Max(node.min, elements[num++].GetBounds().max[axis]);
				}
				while (num2 > start && elements[num2].GetBounds().center[axis] > pivot)
				{
					node.max = Mathf.Min(node.max, elements[num2--].GetBounds().min[axis]);
				}
				if (num <= num2)
				{
					node.min = Mathf.Max(node.min, elements[num2].GetBounds().max[axis]);
					node.max = Mathf.Min(node.max, elements[num].GetBounds().min[axis]);
					ObiUtils.Swap(ref elements[num++], ref elements[num2--]);
				}
			}
			return num2;
		}

		public static float DistanceToSurface(Triangle[] triangles, Vector3[] vertices, Vector3[] normals, in BIHNode node, in Vector3 point)
		{
			float num = float.MaxValue;
			int num2 = 1;
			for (int i = node.start; i < node.start + node.count; i++)
			{
				Triangle triangle = triangles[i];
				ObiUtils.NearestPointOnTri(in vertices[triangle.i1], in vertices[triangle.i2], in vertices[triangle.i3], in point, out var result);
				Vector3 vector = point - result;
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude < num)
				{
					Vector3 bary = Vector3.zero;
					ObiUtils.BarycentricCoordinates(in vertices[triangle.i1], in vertices[triangle.i2], in vertices[triangle.i3], in result, ref bary);
					ObiUtils.BarycentricInterpolation(in normals[triangle.i1], in normals[triangle.i2], in normals[triangle.i3], in bary, out var result2);
					num2 = (vector.x * result2.x + vector.y * result2.y + vector.z * result2.z).PureSign();
					num = sqrMagnitude;
				}
			}
			return Mathf.Sqrt(num) * (float)num2;
		}

		public static float DistanceToSurface(BIHNode[] nodes, Triangle[] triangles, Vector3[] vertices, Vector3[] normals, in Vector3 point)
		{
			if (nodes.Length != 0)
			{
				return DistanceToSurface(nodes, triangles, vertices, normals, in nodes[0], in point);
			}
			return float.MaxValue;
		}

		public static float DistanceToSurface(BIHNode[] nodes, Triangle[] triangles, Vector3[] vertices, Vector3[] normals, in BIHNode node, in Vector3 point)
		{
			if (node.firstChild >= 0)
			{
				float num = float.MaxValue;
				float num2 = point[node.axis];
				if (node.min > node.max)
				{
					if (num2 <= node.min && num2 >= node.max)
					{
						num = MinSignedDistance(DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild], in point), DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild + 1], in point));
					}
					else if (num2 > node.min)
					{
						num = DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild + 1], in point);
						if (Mathf.Abs(num) > Mathf.Abs(num2 - node.min))
						{
							num = MinSignedDistance(num, DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild], in point));
						}
					}
					else
					{
						num = DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild], in point);
						if (Mathf.Abs(num) > Mathf.Abs(node.max - num2))
						{
							num = MinSignedDistance(num, DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild + 1], in point));
						}
					}
				}
				else if (num2 > node.min && num2 < node.max)
				{
					num = DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild + 1], in point);
					if (Mathf.Abs(num) > Mathf.Abs(num2 - node.min))
					{
						num = MinSignedDistance(num, DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild], in point));
					}
				}
				else if (num2 <= node.min)
				{
					num = DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild], in point);
					if (Mathf.Abs(num) > Mathf.Abs(node.max - num2))
					{
						num = MinSignedDistance(num, DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild + 1], in point));
					}
				}
				else if (num2 >= node.max)
				{
					num = DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild + 1], in point);
					if (Mathf.Abs(num) > Mathf.Abs(num2 - node.min))
					{
						num = MinSignedDistance(num, DistanceToSurface(nodes, triangles, vertices, normals, in nodes[node.firstChild], in point));
					}
				}
				return num;
			}
			return DistanceToSurface(triangles, vertices, normals, in node, in point);
			static float MinSignedDistance(float d1, float d2)
			{
				if (!(Mathf.Abs(d1) < Mathf.Abs(d2)))
				{
					return d2;
				}
				return d1;
			}
		}
	}
}
