using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools.Splines
{
	[Serializable]
	public class Line
	{
		public Segment[] segments = new Segment[0];

		public bool looped;

		public float length;

		public Vector3 startPos
		{
			get
			{
				return segments[0].start.pos;
			}
			set
			{
				segments[0].start.pos = value;
			}
		}

		public Vector3 endPos
		{
			get
			{
				return segments[segments.Length - 1].end.pos;
			}
			set
			{
				segments[segments.Length - 1].end.pos = value;
			}
		}

		public int NodesCount
		{
			get
			{
				if (segments.Length == 0)
				{
					return 0;
				}
				if (looped)
				{
					return segments.Length;
				}
				return segments.Length + 1;
			}
		}

		public Line()
		{
		}

		public Line(Line src)
		{
			segments = new Segment[src.segments.Length];
			Array.Copy(src.segments, segments, segments.Length);
			looped = src.looped;
			length = src.length;
		}

		public Line(Vector3 start, Vector3 end)
		{
			segments = new Segment[1]
			{
				new Segment(start, end)
			};
		}

		public Vector3 GetPoint(int segNum, float segPercent)
		{
			return segments[segNum].GetPoint(segPercent);
		}

		public void SetSegmentsCount(int count)
		{
			Array.Resize(ref segments, count);
		}

		public void InsertSegment(int index, Segment segment)
		{
			ArrayTools.Insert(ref segments, index, segment);
		}

		public void AddSegment(Segment segment)
		{
			ArrayTools.Add(ref segments, segment);
		}

		public void SetAllTangentTypes(Node.TangentType type)
		{
			for (int i = 0; i < segments.Length; i++)
			{
				segments[i].start.type = type;
				segments[i].end.type = type;
			}
		}

		public void InsertNode(int n, Node node)
		{
			InsertSegment(n - 1, new Segment(segments[n - 1].start, node));
			segments[n].start = node;
			segments[n].start.dir = -segments[n].start.dir;
		}

		public void RemoveNode(int n)
		{
			if (n == segments.Length)
			{
				ArrayTools.RemoveAt(ref segments, segments.Length - 1);
				return;
			}
			if (n == 0)
			{
				ArrayTools.RemoveAt(ref segments, 0);
				return;
			}
			Segment segment = Segment.Join(segments[n - 1], segments[n]);
			segments[n - 1] = segment;
			ArrayTools.RemoveAt(ref segments, n);
		}

		public void AddNode(Node node)
		{
			AddSegment(new Segment(segments[segments.Length - 1].end, node));
		}

		public void SetNodes(Vector3[] poses)
		{
			segments = new Segment[poses.Length - 1];
			for (int i = 0; i < segments.Length; i++)
			{
				segments[i] = new Segment(poses[i], poses[i + 1]);
			}
		}

		public Vector3 GetNodePos(int n)
		{
			if (looped)
			{
				n %= segments.Length;
			}
			if (n < segments.Length)
			{
				return segments[n].start.pos;
			}
			return segments[n - 1].end.pos;
		}

		public void SetNodePos(int n, Vector3 pos)
		{
			if (n != segments.Length)
			{
				segments[n].start.pos = pos;
			}
			if (n != 0)
			{
				segments[n - 1].end.pos = pos;
			}
		}

		public Vector3? GetNodeInTangent(int n)
		{
			if (n == 0)
			{
				return null;
			}
			return segments[n - 1].end.dir;
		}

		public Vector3? GetNodeOutTangent(int n)
		{
			if (n == segments.Length)
			{
				return null;
			}
			return segments[n].start.dir;
		}

		public void Update()
		{
			UpdatePositions();
			UpdateTangents();
			UpdateLength();
		}

		public void UpdatePositions()
		{
			for (int i = 0; i < segments.Length - 1; i++)
			{
				segments[i].end.pos = segments[i + 1].start.pos;
			}
		}

		public void UpdateTangents()
		{
			for (int i = 0; i < segments.Length + 1; i++)
			{
				UpdateTangent(i);
			}
		}

		public void UpdateTangent(int n)
		{
			if (n == 0)
			{
				Node.TangentType type = segments[n].start.type;
				if (type == Node.TangentType.auto || type == Node.TangentType.linear)
				{
					segments[n].start.dir = Node.LinearTangent(segments[n].start.pos, segments[n].end.pos);
				}
			}
			else if (n != segments.Length)
			{
				Node.TangentType type2 = segments[n].start.type;
				segments[n - 1].end.type = type2;
				if (type2 != Node.TangentType.broken)
				{
					Vector3 vector = segments[n - 1].end.dir;
					Vector3 vector2 = segments[n].start.dir;
					switch (type2)
					{
					case Node.TangentType.auto:
						(vector, vector2) = Node.AutoTangents(segments[n - 1].start.pos, segments[n - 1].end.pos, segments[n].end.pos);
						break;
					case Node.TangentType.linear:
						(vector, vector2) = Node.LinearTangents(segments[n - 1].start.pos, segments[n - 1].end.pos, segments[n].end.pos);
						break;
					case Node.TangentType.correlated:
						vector = Node.CorrelatedInTangent(vector, vector2);
						break;
					}
					segments[n - 1].end.dir = vector;
					segments[n].start.dir = vector2;
				}
			}
			else
			{
				Node.TangentType type3 = segments[n - 1].end.type;
				if (type3 == Node.TangentType.auto || type3 == Node.TangentType.linear)
				{
					segments[n - 1].end.dir = Node.LinearTangent(segments[n - 1].end.pos, segments[n - 1].start.pos);
				}
			}
		}

		public void UpdateLength(int iterations = 32, float[] tmp = null)
		{
			if (tmp == null)
			{
				tmp = new float[iterations];
			}
			length = 0f;
			for (int i = 0; i < segments.Length; i++)
			{
				segments[i].UpdateLength(iterations, tmp);
				length += segments[i].length;
			}
		}

		public void UpdateLength(int num, int iterations = 32)
		{
			segments[num].UpdateLength(iterations);
			length = 0f;
			for (int i = 0; i < segments.Length - 1; i++)
			{
				length += segments[i].length;
			}
		}

		public Vector3[] GetAllPoints(float resPerUnit = 0.1f, int minRes = 3, int maxRes = 20)
		{
			int num = 0;
			for (int i = 0; i < segments.Length; i++)
			{
				int num2 = (int)(segments[i].length * resPerUnit);
				if (num2 < minRes)
				{
					num2 = minRes;
				}
				if (num2 > maxRes)
				{
					num2 = maxRes;
				}
				num += num2;
			}
			Vector3[] array = new Vector3[num + 1];
			int num3 = 0;
			for (int j = 0; j < segments.Length; j++)
			{
				int num4 = (int)(segments[j].length * resPerUnit);
				if (num4 < minRes)
				{
					num4 = minRes;
				}
				if (num4 > maxRes)
				{
					num4 = maxRes;
				}
				for (int k = 0; k < num4; k++)
				{
					float p = 1f * (float)k / (float)num4;
					array[num3] = segments[j].GetPoint(p);
					num3++;
				}
			}
			array[array.Length - 1] = segments[segments.Length - 1].end.pos;
			return array;
		}

		public (Vector3[], Vector3[]) GetAllPointsDerivatives(float resPerUnit = 0.1f, int minRes = 3, int maxRes = 20)
		{
			int num = 0;
			for (int i = 0; i < segments.Length; i++)
			{
				int num2 = (int)(segments[i].length * resPerUnit);
				if (num2 < minRes)
				{
					num2 = minRes;
				}
				if (num2 > maxRes)
				{
					num2 = maxRes;
				}
				num += num2;
			}
			Vector3[] array = new Vector3[num + 1];
			Vector3[] array2 = new Vector3[num + 1];
			int num3 = 0;
			for (int j = 0; j < segments.Length; j++)
			{
				int num4 = (int)(segments[j].length * resPerUnit);
				if (num4 < minRes)
				{
					num4 = minRes;
				}
				if (num4 > maxRes)
				{
					num4 = maxRes;
				}
				for (int k = 0; k < num4; k++)
				{
					float p = 1f * (float)k / (float)num4;
					array[num3] = segments[j].GetPoint(p);
					array2[num3] = segments[j].GetDerivative(p);
					num3++;
				}
			}
			array[array.Length - 1] = segments[segments.Length - 1].end.pos;
			array2[array.Length - 1] = -segments[segments.Length - 1].end.dir;
			return (array, array2);
		}

		public void Split(int s, float p)
		{
			var (segment, element) = segments[s].GetSplitted(p);
			segments[s] = segment;
			ArrayTools.Insert(ref segments, s + 1, element);
		}

		public void Subdivide(int num)
		{
			Segment[] array = new Segment[segments.Length * num];
			for (int i = 0; i < segments.Length; i++)
			{
				Segment segment = segments[i];
				for (int j = 0; j < num; j++)
				{
					float p = 1f / (float)(num - j);
					(Segment, Segment) splitted = segment.GetSplitted(p);
					array[i * num + j] = splitted.Item1;
					segment = splitted.Item2;
				}
			}
			segments = array;
		}

		public (int l, int s, float p, float dist) GetClosest(Vector3 point, int initialApprox = 10, int recursiveApprox = 10)
		{
			return GetClosest(null, point, initialApprox, recursiveApprox);
		}

		public (int l, int s, float p, float dist) GetClosest(Func<Vector3, Vector3, float> distanceFn, int initialApprox = 10, int recursiveApprox = 10)
		{
			return GetClosest(distanceFn, initialApprox, recursiveApprox);
		}

		public (int l, int s, float p, float dist) GetClosest(Func<Vector3, Vector3, float> distanceFn, Vector3 point, int initialApprox = 10, int recursiveApprox = 10)
		{
			float num = 3.4028235E+38f;
			int item = 0;
			int item2 = 0;
			float item3 = 0f;
			for (int i = 0; i < segments.Length; i++)
			{
				if (distanceFn != null || segments[i].IsWithinRange(point, Mathf.Sqrt(num)))
				{
					var (num2, num3) = segments[i].GetClosest(distanceFn, point, initialApprox, recursiveApprox);
					if (num3 < num)
					{
						num = num3;
						item2 = i;
						item3 = num2;
					}
				}
			}
			return (l: item, s: item2, p: item3, dist: num);
		}

		public void Optimize(float deviation)
		{
			int num = segments.Length - 1;
			if (num <= 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				float num2 = 3.4028235E+38f;
				int num3 = -1;
				for (int j = 1; j < segments.Length; j++)
				{
					float num4 = DistanceToLine(segments[j - 1].start.pos + segments[j - 1].start.dir, segments[j].end.pos + segments[j].end.dir, segments[j].start.pos);
					if (num4 < num2)
					{
						num3 = j;
						num2 = num4;
					}
				}
				if (!(num2 > deviation))
				{
					segments[num3 - 1].end = segments[num3].end;
					ArrayTools.RemoveAt(ref segments, num3);
					UpdateTangents();
					continue;
				}
				break;
			}
		}

		private static float DistanceToLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = lineStart - lineEnd;
			float num = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
			float num2 = Mathf.Sqrt(num);
			float magnitude = (lineStart - point).magnitude;
			float magnitude2 = (lineEnd - point).magnitude;
			float num3 = (magnitude + magnitude2 + num2) / 2f;
			float num4 = Mathf.Sqrt(num3 * (num3 - magnitude2) * (num3 - magnitude) * (num3 - num2));
			float num5 = 2f / num2 * num4;
			float num6 = magnitude * magnitude - num5 * num5;
			float num7 = magnitude2 * magnitude2 - num5 * num5;
			if (num6 > num && num6 > num7)
			{
				return magnitude2;
			}
			if (num7 > num)
			{
				return magnitude;
			}
			return num5;
		}

		public void Relax(float blur, int iterations)
		{
			for (int i = 0; i < iterations; i++)
			{
				Relax(blur);
			}
		}

		public void Relax(float blur)
		{
			for (int i = 1; i < segments.Length; i++)
			{
				Vector3 vector = (segments[i - 1].start.pos + segments[i].end.pos) / 2f;
				segments[i].start.pos = vector * blur / 2f + segments[i].start.pos * (1f - blur / 2f);
				segments[i - 1].end.pos = segments[i].start.pos;
			}
		}

		public void CutByRect(Vector3 pos, Vector3 size)
		{
			for (int i = 0; i < 13; i++)
			{
				List<Segment> list = new List<Segment>();
				for (int j = 0; j < segments.Length; j++)
				{
					Vector3 vector = segments[j].ApproxMin();
					Vector3 vector2 = segments[j].ApproxMax();
					if (vector2.x < pos.x || vector.x > pos.x + size.x || vector2.z < pos.z || vector.z > pos.z + size.z)
					{
						list.Add(segments[j]);
						continue;
					}
					if (vector.x > pos.x && vector2.x < pos.x + size.x && vector.z > pos.z && vector2.z < pos.z + size.z)
					{
						list.Add(segments[j]);
						continue;
					}
					float num = segments[j].IntersectRect(pos, size);
					if (num < 0.0001f || num > 0.999f)
					{
						list.Add(segments[j]);
						continue;
					}
					var (item, item2) = segments[j].GetSplitted(num);
					list.Add(item);
					list.Add(item2);
				}
				bool num2 = segments.Length != list.Count;
				segments = list.ToArray();
				if (!num2)
				{
					break;
				}
			}
		}

		public List<Line> OuterSegmentsRemoved(Vector3 pos, Vector3 size)
		{
			List<Line> list = new List<Line>();
			List<Segment> list2 = new List<Segment>();
			for (int i = 0; i < segments.Length; i++)
			{
				Vector3 point = segments[i].GetPoint(0.5f);
				if (point.x > pos.x && point.x < pos.x + size.x && point.z > pos.z && point.z < pos.z + size.z)
				{
					list2.Add(segments[i]);
				}
				else if (list2.Count != 0)
				{
					Line item = new Line
					{
						segments = list2.ToArray()
					};
					list.Add(item);
					list2.Clear();
				}
			}
			if (list2.Count != 0)
			{
				Line item2 = new Line
				{
					segments = list2.ToArray()
				};
				list.Add(item2);
			}
			return list;
		}

		public List<Line> Clamped(Vector3 pos, Vector3 size)
		{
			Line line = new Line(this);
			line.CutByRect(pos, size);
			return line.OuterSegmentsRemoved(pos, size);
		}

		public void PushPoints(Vector3[] points, float[] ranges, bool horizontalOnly = true, float distFactor = 1f)
		{
			for (int i = 0; i < segments.Length; i++)
			{
				segments[i].PushPoints(points, ranges, horizontalOnly, distFactor);
			}
		}

		public void PushStartEnd(Vector3[] points, float[] ranges, bool horizontalOnly = true, float distFactor = 1f)
		{
			for (int i = 0; i < segments.Length; i++)
			{
				segments[i].PushStartEnd(points, ranges, horizontalOnly, distFactor);
			}
		}

		public void SplitNearPoints(Vector3[] points, float[] ranges, bool horizontalOnly = true, float startEndProximityFactor = 0f, int maxIterations = 10)
		{
			if (maxIterations == 0)
			{
				return;
			}
			List<Segment> list = new List<Segment>();
			for (int i = 0; i < segments.Length; i++)
			{
				if (!segments[i].IsNearPoints(points, ranges, out var nearPercent, out var _, horizontalOnly, startEndProximityFactor))
				{
					list.Add(segments[i]);
					continue;
				}
				var (item, item2) = segments[i].GetSplitted(nearPercent);
				list.Add(item);
				list.Add(item2);
			}
			bool num = segments.Length != list.Count;
			segments = list.ToArray();
			if (num && maxIterations > 1)
			{
				SplitNearPoints(points, ranges, horizontalOnly, startEndProximityFactor, maxIterations - 1);
			}
		}

		public static bool AreCloseToWeld(Line line1, Line line2, float threshold)
		{
			Vector3[] array = new Vector3[line2.segments.Length];
			Vector3[] array2 = new Vector3[line2.segments.Length];
			for (int i = 0; i < line2.segments.Length; i++)
			{
				array[i] = line2.segments[i].ApproxMin();
				array2[i] = line2.segments[i].ApproxMax();
			}
			for (int j = 0; j < line1.segments.Length; j++)
			{
				Vector3 vector = line1.segments[j].ApproxMin();
				vector.x -= threshold;
				vector.y -= threshold;
				vector.z -= threshold;
				Vector3 vector2 = line1.segments[j].ApproxMax();
				vector.x += threshold;
				vector.y += threshold;
				vector.z += threshold;
				for (int k = 0; k < line2.segments.Length; k++)
				{
					if (!(array[k].x > vector2.x) && !(array2[k].x < vector.x) && !(array[k].y > vector2.y) && !(array2[k].y < vector.y) && !(array[k].z > vector2.z) && !(array2[k].z < vector.z))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static Line[] WeldClose(Line line1, Line line2, float threshold)
		{
			line1.CutSegmentsForWeld(line2, threshold);
			line2.CutSegmentsForWeld(line1, threshold);
			OverlapLines(line1, line2, threshold);
			Segment[] destinationArray = new Segment[line1.segments.Length + line2.segments.Length];
			Array.Copy(line1.segments, destinationArray, line1.segments.Length);
			Array.Copy(line2.segments, 0, destinationArray, line1.segments.Length, line2.segments.Length);
			return NeatWeldSegments(destinationArray);
		}

		public void CutSegmentsForWeld(Line line2, float threshold)
		{
			for (int i = 0; i <= line2.segments.Length + 2; i++)
			{
				List<Segment> list = new List<Segment>();
				for (int j = 0; j < segments.Length; j++)
				{
					Vector3 vector = segments[j].ApproxMin() - new Vector3(threshold, threshold, threshold);
					Vector3 vector2 = segments[j].ApproxMax() + new Vector3(threshold, threshold, threshold);
					float num = -1f;
					for (int k = 0; k < line2.segments.Length + 1; k++)
					{
						Vector3 vector3 = ((k != line2.segments.Length) ? line2.segments[k].start.pos : line2.segments[k - 1].end.pos);
						if (!(vector3.x < vector.x) && !(vector3.x > vector2.x) && !(vector3.y < vector.y) && !(vector3.y > vector2.y) && !(vector3.z < vector.z) && !(vector3.z > vector2.z) && !((vector3 - segments[j].start.pos).sqrMagnitude < threshold * threshold) && !((vector3 - segments[j].end.pos).sqrMagnitude < threshold * threshold))
						{
							(float percent, float distSq) closest = segments[j].GetClosest(vector3, 5, 5);
							var (num2, _) = closest;
							if (!(closest.distSq > threshold * threshold))
							{
								num = num2;
							}
						}
					}
					if (num < 0f)
					{
						list.Add(segments[j]);
						continue;
					}
					var (item, item2) = segments[j].GetSplitted(num);
					list.Add(item);
					list.Add(item2);
				}
				if (list.Count == segments.Length)
				{
					break;
				}
				segments = list.ToArray();
				if (i == line2.segments.Length + 2)
				{
					throw new Exception("CutSegmentsForWeld reached maximum number of iterations");
				}
			}
		}

		public static Line[] NeatWeldSegments(Segment[] segments)
		{
			bool[] array = new bool[segments.Length];
			for (int i = 0; i < segments.Length; i++)
			{
				if ((segments[i].start.pos - segments[i].end.pos).sqrMagnitude < 0.0001f)
				{
					array[i] = true;
				}
			}
			for (int j = 0; j < segments.Length; j++)
			{
				if (array[j])
				{
					continue;
				}
				for (int k = 0; k < segments.Length; k++)
				{
					if (!array[k] && k != j)
					{
						if ((segments[j].start.pos - segments[k].start.pos).sqrMagnitude < 0.0001f && (segments[j].end.pos - segments[k].end.pos).sqrMagnitude < 0.0001f)
						{
							array[k] = true;
						}
						if ((segments[j].start.pos - segments[k].end.pos).sqrMagnitude < 0.0001f && (segments[j].end.pos - segments[k].start.pos).sqrMagnitude < 0.0001f)
						{
							array[k] = true;
						}
					}
				}
			}
			Dictionary<int, int> dictionary = CompileWeldIndexLut(segments, array);
			List<List<int>> list = new List<List<int>>();
			for (int l = 0; l < segments.Length; l++)
			{
				if (!array[l] && (!dictionary.ContainsKey(l * 2) || !dictionary.ContainsKey(l * 2 + 1)))
				{
					list.Add(CompileLineSegments(dictionary, array, l));
				}
			}
			Line[] array2 = new Line[list.Count];
			for (int m = 0; m < array2.Length; m++)
			{
				Segment[] array3 = new Segment[list[m].Count];
				for (int n = 0; n < array3.Length; n++)
				{
					array3[n] = segments[list[m][n]];
				}
				array2[m] = new Line
				{
					segments = array3
				};
			}
			Line[] array4 = array2;
			for (int num = 0; num < array4.Length; num++)
			{
				array4[num].AutoInverseSegments();
			}
			return array2;
		}

		private static Dictionary<int, int> CompileWeldIndexLut(Segment[] segments, bool[] usedSegments)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < segments.Length * 2; i++)
			{
				int num = i / 2;
				if (usedSegments[num])
				{
					continue;
				}
				Vector3 vector = ((i % 2 == 0) ? segments[num].start.pos : segments[num].end.pos);
				int num2 = 0;
				int value = -1;
				for (int j = 0; j < segments.Length; j++)
				{
					if (!usedSegments[j] && j != num)
					{
						if ((vector - segments[j].start.pos).sqrMagnitude < 0.0001f)
						{
							value = j * 2;
							num2++;
						}
						if ((vector - segments[j].end.pos).sqrMagnitude < 0.0001f)
						{
							value = j * 2 + 1;
							num2++;
						}
					}
				}
				if (num2 == 1)
				{
					dictionary.Add(i, value);
				}
			}
			return dictionary;
		}

		private static List<int> CompileLineSegments(Dictionary<int, int> weldLut, bool[] usedSegments, int startSegNum)
		{
			List<int> list = new List<int>();
			list.Add(startSegNum);
			usedSegments[startSegNum] = true;
			int num = startSegNum * 2;
			int num2 = startSegNum * 2 + 1;
			int num3 = -1;
			if (weldLut.ContainsKey(num))
			{
				num3 = num;
			}
			if (weldLut.ContainsKey(num2))
			{
				num3 = num2;
			}
			if (num3 == -1)
			{
				return list;
			}
			for (int i = 0; i <= usedSegments.Length; i++)
			{
				int num4 = weldLut[num3];
				int num5 = num4 / 2;
				num = num5 * 2;
				num2 = num5 * 2 + 1;
				num3 = ((num4 == num) ? num2 : num);
				if (usedSegments[num5])
				{
					throw new Exception("CompileLineSegments trying to add used segment to line");
				}
				list.Add(num5);
				usedSegments[num5] = true;
				if (!weldLut.ContainsKey(num3))
				{
					break;
				}
				if (i == usedSegments.Length)
				{
					throw new Exception("CompileLineSegments reached maximum number of iterations");
				}
			}
			return list;
		}

		public static void OverlapLines(Line line1, Line line2, float threshold)
		{
			line1.OverlapTo(line2, threshold, 0.5f);
			line2.OverlapTo(line1, threshold);
			line1.OverlapTo(line2, threshold);
		}

		public void OverlapTo(Line refLine, float threshold, float moveFactor = 1f)
		{
			for (int i = 0; i < segments.Length + 1; i++)
			{
				Vector3 vector = ((i != segments.Length) ? segments[i].start.pos : segments[i - 1].end.pos);
				int closestNodeIndex = refLine.GetClosestNodeIndex(vector, threshold);
				if (closestNodeIndex >= 0)
				{
					Vector3 nodePos = refLine.GetNodePos(closestNodeIndex);
					SetNodePos(i, nodePos * moveFactor + vector * (1f - moveFactor));
				}
			}
		}

		public static bool AreCodirected(Line line1, Line line2)
		{
			(int, int) closestNodeIndexes = GetClosestNodeIndexes(line1, line2);
			int item = closestNodeIndexes.Item1;
			int item2 = closestNodeIndexes.Item2;
			Vector3 nodePos = line1.GetNodePos((item > 0) ? (item - 1) : item);
			Vector3 nodePos2 = line1.GetNodePos((item < line1.NodesCount - 1) ? (item + 1) : (line1.NodesCount - 1));
			Vector3 nodePos3 = line2.GetNodePos((item2 > 0) ? (item2 - 1) : item2);
			Vector3 nodePos4 = line2.GetNodePos((item2 < line2.NodesCount - 1) ? (item2 + 1) : (line2.NodesCount - 1));
			float sqrMagnitude = (nodePos - nodePos3).sqrMagnitude;
			float sqrMagnitude2 = (nodePos2 - nodePos4).sqrMagnitude;
			float sqrMagnitude3 = (nodePos2 - nodePos3).sqrMagnitude;
			float sqrMagnitude4 = (nodePos - nodePos4).sqrMagnitude;
			if (sqrMagnitude < sqrMagnitude3 && sqrMagnitude < sqrMagnitude4 && sqrMagnitude < sqrMagnitude2)
			{
				return true;
			}
			if (sqrMagnitude2 < sqrMagnitude3 && sqrMagnitude2 < sqrMagnitude4 && sqrMagnitude2 < sqrMagnitude)
			{
				return true;
			}
			if (sqrMagnitude3 < sqrMagnitude && sqrMagnitude3 < sqrMagnitude2 && sqrMagnitude3 < sqrMagnitude4)
			{
				return false;
			}
			if (sqrMagnitude4 < sqrMagnitude && sqrMagnitude4 < sqrMagnitude2 && sqrMagnitude4 < sqrMagnitude3)
			{
				return false;
			}
			throw new Exception("Failed to determine codirection");
		}

		private static (int, int) GetClosestNodeIndexes(Line line1, Line line2, float maxThreshold = 3.4028235E+38f, float minThreshold = -1f, bool[] ignore1 = null, bool[] ignore2 = null)
		{
			int item = -1;
			int item2 = -1;
			float num = 3.4028235E+38f;
			for (int i = 0; i < line1.segments.Length + 1; i++)
			{
				if (ignore1 != null && ignore1[i])
				{
					continue;
				}
				Vector3 vector = ((i != line1.segments.Length) ? line1.segments[i].start.pos : line1.segments[i - 1].end.pos);
				int closestNodeIndex = line2.GetClosestNodeIndex(vector, minThreshold, maxThreshold, ignore2);
				if (closestNodeIndex >= 0)
				{
					Vector3 nodePos = line2.GetNodePos(closestNodeIndex);
					float sqrMagnitude = (vector - nodePos).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						item = i;
						item2 = closestNodeIndex;
					}
				}
			}
			return (item, item2);
		}

		public int GetClosestNodeIndex(Vector3 point, float maxThreshold = 3.4028235E+38f, float minThreshold = 0f, bool[] ignore = null)
		{
			float num = 3.4028235E+38f;
			int result = -1;
			for (int i = 0; i < segments.Length + 1; i++)
			{
				if (ignore == null || !ignore[i])
				{
					Vector3 vector = ((i != segments.Length) ? segments[i].start.pos : segments[i - 1].end.pos);
					float num2 = (point.x - vector.x) * (point.x - vector.x) + (point.y - vector.y) * (point.y - vector.y) + (point.z - vector.z) * (point.z - vector.z);
					if (num2 < num && num2 >= minThreshold * minThreshold && num2 < maxThreshold * maxThreshold)
					{
						num = num2;
						result = i;
					}
				}
			}
			return result;
		}

		public IEnumerable<int> NodeIndexesWithinRange(Vector3 point, float threshold)
		{
			for (int s = 0; s < segments.Length + 1; s++)
			{
				Vector3 vector = ((s != segments.Length) ? segments[s].start.pos : segments[s - 1].end.pos);
				if ((point.x - vector.x) * (point.x - vector.x) + (point.y - vector.y) * (point.y - vector.y) + (point.z - vector.z) * (point.z - vector.z) < threshold * threshold)
				{
					yield return s;
				}
			}
		}

		private void AutoInverseSegments()
		{
			if (segments.Length == 1)
			{
				return;
			}
			if ((segments[0].start.pos - segments[1].start.pos).sqrMagnitude < 0.0001f || (segments[0].start.pos - segments[1].end.pos).sqrMagnitude < 0.0001f)
			{
				segments[0] = segments[0].Inverted;
			}
			for (int i = 1; i < segments.Length; i++)
			{
				if ((segments[i].end.pos - segments[i - 1].end.pos).sqrMagnitude < 0.0001f)
				{
					segments[i] = segments[i].Inverted;
				}
			}
		}
	}
}
