using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools.Splines
{
	[Serializable]
	public class SplineSys : ICloneable
	{
		public Line[] lines = new Line[0];

		public bool guiDrawNodes = true;

		public bool guiDrawSegments = true;

		public bool guiDrawDots;

		public int guiDotsCount = 10;

		public bool guiDotsEquidist;

		public int NodesCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < lines.Length; i++)
				{
					num += lines[i].NodesCount;
				}
				return num;
			}
		}

		public SplineSys()
		{
		}

		public SplineSys(SplineSys src)
		{
			CopyLinesFrom(src.lines);
			guiDrawNodes = src.guiDrawNodes;
			guiDrawSegments = src.guiDrawSegments;
			guiDrawDots = src.guiDrawDots;
			guiDotsCount = src.guiDotsCount;
			guiDotsEquidist = src.guiDotsEquidist;
		}

		public object Clone()
		{
			return new SplineSys(this);
		}

		public Vector3 GetPoint((int l, int s, float p) loc)
		{
			return lines[loc.l].segments[loc.s].GetPoint(loc.p);
		}

		public Vector3 GetPoint(int l, int s, float p)
		{
			return lines[l].segments[s].GetPoint(p);
		}

		public Vector3 GetDerivative(int l, int s, float p)
		{
			return lines[l].segments[s].GetDerivative(p);
		}

		public Vector3 GetDerivative((int l, int s, float p) loc)
		{
			return lines[loc.l].segments[loc.s].GetDerivative(loc.p);
		}

		public void AddLine(Line line)
		{
			ArrayTools.Add(ref lines, line);
		}

		public Line AddLine(Vector3 start, Vector3 end)
		{
			Line line = new Line(start, end);
			ArrayTools.Add(ref lines, line);
			return line;
		}

		public void AddLines(Line[] otherLines)
		{
			ArrayTools.AddRange(ref lines, otherLines);
		}

		public void CopyLinesFrom(Line[] otherLines)
		{
			lines = new Line[otherLines.Length];
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = new Line(otherLines[i]);
			}
		}

		public void RemoveLine(int l)
		{
			ArrayTools.RemoveAt(ref lines, l);
		}

		public static SplineSys CreateDefault()
		{
			SplineSys splineSys = new SplineSys();
			splineSys.lines = new Line[1]
			{
				new Line()
			};
			splineSys.lines[0].segments = new Segment[1]
			{
				new Segment(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f))
			};
			return splineSys;
		}

		public void Update()
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].Update();
			}
		}

		public void UpdateTangents()
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].UpdateTangents();
			}
		}

		public void UpdateLength()
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].UpdateLength();
			}
		}

		public Vector3[][] GetAllPoints(float resPerUnit = 0.1f, int minRes = 3, int maxRes = 20)
		{
			Vector3[][] array = new Vector3[lines.Length][];
			for (int i = 0; i < lines.Length; i++)
			{
				array[i] = lines[i].GetAllPoints(resPerUnit, minRes, maxRes);
			}
			return array;
		}

		public (int l, int s, float p, float dist) GetClosest(Vector3 point, int initialApprox = 10, int recursiveApprox = 10)
		{
			return GetClosest(null, point, initialApprox, recursiveApprox);
		}

		public (int l, int s, float p, float dist) GetClosest(Func<Vector3, Vector3, float> distanceFn, int initialApprox = 10, int recursiveApprox = 10)
		{
			return GetClosest(distanceFn, default(Vector3), initialApprox, recursiveApprox);
		}

		public (int l, int s, float p, float dist) GetClosest(Func<Vector3, Vector3, float> distanceFn, Vector3 point, int initialApprox = 10, int recursiveApprox = 10)
		{
			float num = 3.4028235E+38f;
			int item = 0;
			int item2 = 0;
			float item3 = 0f;
			for (int i = 0; i < lines.Length; i++)
			{
				var (num2, num3, num4, num5) = lines[i].GetClosest(distanceFn, point, initialApprox, recursiveApprox);
				if (num5 < num)
				{
					num = num5;
					item2 = num3;
					item3 = num4;
					item = num2;
				}
			}
			return (l: item, s: item2, p: item3, dist: num);
		}

		public float Handness(Vector3 point)
		{
			(int l, int s, float p, float dist) closest = GetClosest(point);
			int item = closest.l;
			int item2 = closest.s;
			float item3 = closest.p;
			Vector3 point2 = GetPoint(item, item2, item3);
			Vector3 derivative = GetDerivative(item, item2, item3);
			return CoordinatesExtensions.Handness(from: new Vector2(point2.x, point2.z), to: new Vector2(point2.x + derivative.x, point2.z + derivative.z), point: new Vector2(point.x, point.z));
		}

		public void Optimize(float deviation)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].Optimize(deviation);
			}
		}

		public void Subdivide(int num, bool equiDistance = false)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].Subdivide(num);
			}
		}

		public void Relax(float blur, int iterations)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].Relax(blur, iterations);
			}
		}

		public void CutByRect(Vector3 pos, Vector3 size)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].CutByRect(pos, size);
			}
		}

		public void RemoveOuterSegments(Vector3 pos, Vector3 size)
		{
			List<Line> list = new List<Line>();
			for (int i = 0; i < lines.Length; i++)
			{
				list.AddRange(lines[i].OuterSegmentsRemoved(pos, size));
			}
			lines = list.ToArray();
		}

		public void Clamp(Vector3 pos, Vector3 size)
		{
			CutByRect(pos, size);
			RemoveOuterSegments(pos, size);
		}

		public void PushPoints(Vector3[] points, float[] ranges, bool horizontalOnly = true, float distFactor = 1f)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].PushPoints(points, ranges, horizontalOnly, distFactor);
			}
		}

		public void PushStartEnd(Vector3[] points, float[] ranges, bool horizontalOnly = true, float distFactor = 1f)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].PushStartEnd(points, ranges, horizontalOnly, distFactor);
			}
		}

		public void SplitNearPoints(Vector3[] points, float[] ranges, bool horizontalOnly = true, float startEndProximityFactor = 0f, int maxIterations = 10)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i].SplitNearPoints(points, ranges, horizontalOnly, startEndProximityFactor, maxIterations);
			}
		}

		public void WeldCloseLines(float threshold)
		{
			List<Line> list = new List<Line>();
			list.AddRange(lines);
			int num = 0;
			while (true)
			{
				num++;
				if (num == lines.Length + 1)
				{
					break;
				}
				int num2 = 0;
				int i;
				Line[] array;
				while (true)
				{
					if (num2 < list.Count)
					{
						for (i = 0; i < num2; i++)
						{
							Line line = list[num2];
							Line line2 = list[i];
							if (!Line.AreCloseToWeld(line, line2, threshold))
							{
								continue;
							}
							array = Line.WeldClose(new Line(line), new Line(line2), threshold);
							if (array.Length != 1)
							{
								if (array.Length != 2 || array[0].segments.Length + array[1].segments.Length != line.segments.Length + line2.segments.Length)
								{
									goto end_IL_00f1;
								}
								list[num2] = array[0];
								list[i] = array[1];
							}
						}
						num2++;
						continue;
					}
					lines = list.ToArray();
					return;
					continue;
					end_IL_00f1:
					break;
				}
				list.RemoveAt(num2);
				list.RemoveAt(i);
				list.AddRange(array);
			}
			throw new Exception("WeldCloseLines reached maximum iterations");
		}
	}
}
