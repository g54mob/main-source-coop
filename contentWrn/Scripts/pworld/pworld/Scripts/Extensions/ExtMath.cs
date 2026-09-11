using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtMath
	{
		public static int PClampToSizeOf<T>(this int value, List<T> list)
		{
			return Mathf.Clamp(value, 0, list.Count - 1);
		}

		public static int PClampToSizeOf<T>(this int value, IEnumerable<T> array)
		{
			return Mathf.Clamp(value, 0, array.Count() - 1);
		}

		public static int PClampToSizeOf<T>(this int value, T[] array)
		{
			return Mathf.Clamp(value, 0, array.Length - 1);
		}

		public static bool IsBetween(this float me, float min, float max)
		{
			if (me >= min)
			{
				return me <= max;
			}
			return false;
		}

		public static bool IsEven(this int me)
		{
			return me % 2 == 0;
		}

		public static bool NearlyEqual(float a, float b, float epsilon = float.Epsilon)
		{
			float num = Math.Abs(a);
			float num2 = Math.Abs(b);
			float num3 = Math.Abs(a - b);
			if (a == b)
			{
				return true;
			}
			if (a == 0f || b == 0f || num + num2 < float.Epsilon)
			{
				return num3 < epsilon * float.Epsilon;
			}
			return num3 / (num + num2) < epsilon;
		}

		public static int RandFlip()
		{
			return 1 + UnityEngine.Random.Range(0, 2) * -2;
		}

		public static float DistanceFromPointToLine(Vector3 start, Vector3 end, Vector3 point)
		{
			return Vector3.Cross(end - start, point - start).magnitude / (end - start).magnitude;
		}

		public static float DistancePointToFiniteLine(Vector3 start, Vector3 end, Vector3 point)
		{
			Vector3 vector = end - start;
			float num = Vector3.Dot(point - start, vector) / vector.sqrMagnitude;
			Vector3 b = ((num < 0f) ? start : ((!(num > 1f)) ? (start + vector * num) : end));
			return Vector3.Distance(point, b);
		}

		public static Vector2 NearestPointOnLineDir(Vector2 linePnt, Vector2 lineDir, Vector2 pnt)
		{
			lineDir = lineDir.normalized;
			float num = Vector2.Dot(pnt - linePnt, lineDir);
			return linePnt + lineDir * num;
		}

		public static Vector2 NearestPointOnLineSegment(Vector2 origin, Vector2 end, Vector2 point)
		{
			Vector2 vector = end - origin;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float value = Vector2.Dot(point - origin, vector);
			value = Mathf.Clamp(value, 0f, magnitude);
			return origin + vector * value;
		}

		public static float Eerp(float a, float b, float t)
		{
			if (t != 0f)
			{
				if (t == 1f)
				{
					return b;
				}
				return Mathf.Pow(a, 1f - t) * Mathf.Pow(b, t);
			}
			return a;
		}

		public static float InverseEerp(float a, float b, float v)
		{
			return Mathf.Log(a / v) / Mathf.Log(a / b);
		}

		public static Color LinearToGamma(Color color)
		{
			float e = 0.45454544f;
			return new Color(Pow(color.r, e), Pow(color.g, e), Pow(color.b, e), Pow(color.a, e));
			static float Pow(float x, float p)
			{
				return Mathf.Pow(x, p);
			}
		}

		public static float Step(float x, float y)
		{
			return (x >= y) ? 1 : 0;
		}

		public static Vector3 Clamp(this Vector3 me, Vector3 min, Vector3 max)
		{
			me.x = Mathf.Clamp(me.x, min.x, max.x);
			me.z = Mathf.Clamp(me.z, min.z, max.z);
			me.y = Mathf.Clamp(me.y, min.y, max.y);
			return me;
		}

		public static Vector2 Clamp(this Vector2 me, Vector2 min, Vector2 max)
		{
			me.x = Mathf.Clamp(me.x, min.x, max.x);
			me.y = Mathf.Clamp(me.y, min.y, max.y);
			return me;
		}

		public static Vector3 GetRotationDelta(Vector3 ownForward, Vector3 targetForward)
		{
			return Vector3.Cross(ownForward, targetForward).normalized * Vector3.Angle(ownForward, targetForward);
		}

		public static float PLoop(float value, float min, float max)
		{
			if (value < min)
			{
				return ((value - min) / max + min) % 1f * max + min;
			}
			return value / max % 1f * max;
		}

		public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
		{
			float num = 1E-05f;
			bool result = false;
			float num2 = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);
			if (num2 != 0f)
			{
				float num3 = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) / num2;
				float num4 = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) / num2;
				if (shouldIncludeEndPoints)
				{
					if (num3 >= 0f + num && num3 <= 1f - num && num4 >= 0f + num && num4 <= 1f - num)
					{
						result = true;
					}
				}
				else if (num3 > 0f + num && num3 < 1f - num && num4 > 0f + num && num4 < 1f - num)
				{
					result = true;
				}
			}
			return result;
		}

		public static void FindIntersection(Vector2 line1Start, Vector2 line1End, Vector2 line2start, Vector2 line2End, out bool lines_intersect, out bool segments_intersect, out Vector2 intersection, out Vector2 close_p1, out Vector2 close_p2)
		{
			float num = line1End.x - line1Start.x;
			float num2 = line1End.y - line1Start.y;
			float num3 = line2End.x - line2start.x;
			float num4 = line2End.y - line2start.y;
			float num5 = num2 * num3 - num * num4;
			float num6 = ((line1Start.x - line2start.x) * num4 + (line2start.y - line1Start.y) * num3) / num5;
			if (float.IsInfinity(num6))
			{
				lines_intersect = false;
				segments_intersect = false;
				intersection = new Vector2(float.NaN, float.NaN);
				close_p1 = new Vector2(float.NaN, float.NaN);
				close_p2 = new Vector2(float.NaN, float.NaN);
				return;
			}
			lines_intersect = true;
			float num7 = ((line2start.x - line1Start.x) * num2 + (line1Start.y - line2start.y) * num) / (0f - num5);
			intersection = new Vector2(line1Start.x + num * num6, line1Start.y + num2 * num6);
			segments_intersect = num6 >= 0f && num6 <= 1f && num7 >= 0f && num7 <= 1f;
			if (num6 < 0f)
			{
				num6 = 0f;
			}
			else if (num6 > 1f)
			{
				num6 = 1f;
			}
			if (num7 < 0f)
			{
				num7 = 0f;
			}
			else if (num7 > 1f)
			{
				num7 = 1f;
			}
			close_p1 = new Vector2(line1Start.x + num * num6, line1Start.y + num2 * num6);
			close_p2 = new Vector2(line2start.x + num3 * num7, line2start.y + num4 * num7);
		}
	}
}
