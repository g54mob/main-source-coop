using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	public static class CoordinatesExtensions
	{
		public static bool InRange(this Rect rect, Vector2 pos)
		{
			return (rect.center - pos).sqrMagnitude < rect.width / 2f * (rect.width / 2f);
		}

		public static Vector3 ToDir(this float angle)
		{
			return new Vector3(Mathf.Sin(angle * ((float)Math.PI / 180f)), 0f, Mathf.Cos(angle * ((float)Math.PI / 180f)));
		}

		public static float ToAngle(this Vector3 dir)
		{
			return Mathf.Atan2(dir.x, dir.z) * 57.29578f;
		}

		public static Vector3 Mul(this Vector3 v, Vector3 m)
		{
			return new Vector3(v.x *= m.x, v.y *= m.y, v.z *= m.z);
		}

		public static Vector3 Div(this Vector3 v, Vector3 m)
		{
			return new Vector3(v.x /= m.x, v.y /= m.y, v.z /= m.z);
		}

		public static Vector3 V3(this Vector2 v2)
		{
			return new Vector3(v2.x, 0f, v2.y);
		}

		public static Vector2 V2(this Vector3 v3)
		{
			return new Vector2(v3.x, v3.z);
		}

		public static Vector3 ToV3(this float f)
		{
			return new Vector3(f, f, f);
		}

		public static Vector4 ToV4(this Rect r)
		{
			return new Vector4(r.x, r.y, r.width, r.height);
		}

		public static Quaternion EulerToQuat(this Vector3 v)
		{
			Quaternion identity = Quaternion.identity;
			identity.eulerAngles = v;
			return identity;
		}

		public static Quaternion EulerToQuat(this float f)
		{
			Quaternion identity = Quaternion.identity;
			identity.eulerAngles = new Vector3(0f, f, 0f);
			return identity;
		}

		public static Vector3 Direction(this float angle)
		{
			return new Vector3(Mathf.Sin(angle * ((float)Math.PI / 180f)), 0f, Mathf.Cos(angle * ((float)Math.PI / 180f)));
		}

		public static float Angle(this Vector3 dir)
		{
			return Mathf.Atan2(dir.x, dir.z) * 57.29578f;
		}

		public static Rect Clamp(this Rect r, float p)
		{
			return new Rect(r.x, r.y, r.width * p, r.height);
		}

		public static Rect ClampFromLeft(this Rect r, float p)
		{
			return new Rect(r.x + r.width * (1f - p), r.y, r.width * p, r.height);
		}

		public static Rect Clamp(this Rect r, int p)
		{
			return new Rect(r.x, r.y, p, r.height);
		}

		public static Rect ClampFromLeft(this Rect r, int p)
		{
			return new Rect(r.x + (r.width - (float)p), r.y, p, r.height);
		}

		public static Vector3 Pow(this Vector3 v, float pow)
		{
			return new Vector3(Mathf.Pow(v.x, pow), Mathf.Pow(v.y, pow), Mathf.Pow(v.z, pow));
		}

		public static Rect Intersect(Rect r1, Rect r2)
		{
			Rect result = new Rect(0f, 0f, 0f, 0f);
			result.x = Mathf.Max(r1.x, r2.x);
			result.y = Mathf.Max(r1.y, r2.y);
			result.max = new Vector2(Mathf.Min(r1.max.x, r2.max.x), Mathf.Min(r1.max.y, r2.max.y));
			if (result.size.x < 0f)
			{
				result.size = new Vector2(0f, result.size.y);
			}
			if (result.size.y < 0f)
			{
				result.size = new Vector2(result.size.y, 0f);
			}
			return result;
		}

		public static Rect Intersect(Rect r1, CoordRect r2)
		{
			Rect result = new Rect(0f, 0f, 0f, 0f);
			result.x = Mathf.Max(r1.x, r2.offset.x);
			result.y = Mathf.Max(r1.y, r2.offset.z);
			result.max = new Vector2(Mathf.Min(r1.max.x, r2.offset.x + r2.size.x), Mathf.Min(r1.max.y, r2.offset.z + r2.size.z));
			if (result.size.x < 0f)
			{
				result.size = new Vector2(0f, result.size.y);
			}
			if (result.size.y < 0f)
			{
				result.size = new Vector2(result.size.y, 0f);
			}
			return result;
		}

		public static Rect ToRect(this Vector3 center, float range)
		{
			return new Rect(center.x - range, center.z - range, range * 2f, range * 2f);
		}

		public static Vector3 Average(this Vector3[] vecs)
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < vecs.Length; i++)
			{
				zero += vecs[i];
			}
			return zero / vecs.Length;
		}

		public static bool Intersects(this Rect r1, Rect r2)
		{
			Vector2 min = r1.min;
			Vector2 max = r1.max;
			Vector2 min2 = r2.min;
			Vector2 max2 = r2.max;
			if (max2.x < min.x || min2.x > max.x || max2.y < min.y || min2.y > max.y)
			{
				return false;
			}
			return true;
		}

		public static bool Intersects(this Rect r1, Vector2 pos, Vector2 size)
		{
			Vector2 min = r1.min;
			Vector2 max = r1.max;
			Vector2 vector = pos;
			Vector2 vector2 = pos + size;
			if (vector2.x < min.x || vector.x > max.x || vector2.y < min.y || vector.y > max.y)
			{
				return false;
			}
			return true;
		}

		public static bool Intersects(this Rect r1, Rect[] rects)
		{
			for (int i = 0; i < rects.Length; i++)
			{
				if (r1.Intersects(rects[i]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool Contains(this Rect r1, Rect r2)
		{
			Vector2 min = r1.min;
			Vector2 max = r1.max;
			Vector2 min2 = r2.min;
			Vector2 max2 = r2.max;
			if (min2.x > min.x && max2.x < max.x && min2.y > min.y && max2.y < max.y)
			{
				return true;
			}
			return false;
		}

		public static bool Contains(this Rect r, Vector2 pos, Vector2 size)
		{
			Vector2 min = r.min;
			Vector2 max = r.max;
			Vector2 vector = pos;
			Vector2 vector2 = pos + size;
			if (vector.x > min.x && vector2.x < max.x && vector.y > min.y && vector2.y < max.y)
			{
				return true;
			}
			return false;
		}

		public static bool ContainsOrIntersects(this Rect r1, Rect r2)
		{
			Vector2 min = r1.min;
			Vector2 max = r1.max;
			Vector2 min2 = r2.min;
			Vector2 max2 = r2.max;
			if (min2.x > max.x || min2.y > max.y || max2.x < min.x || max2.y < min.y)
			{
				return false;
			}
			return true;
		}

		public static bool ContainsOrIntersects(this Rect r1, Rect r2, float padding)
		{
			Vector2 min = r1.min;
			Vector2 max = r1.max;
			Vector2 min2 = r2.min;
			Vector2 max2 = r2.max;
			if (min2.x > max.x + padding || min2.y > max.y + padding || max2.x < min.x - padding || max2.y < min.y - padding)
			{
				return false;
			}
			return true;
		}

		public static Rect Extended(this Rect r, float f)
		{
			return new Rect(r.x - f, r.y - f, r.width + f * 2f, r.height + f * 2f);
		}

		public static Rect Extended(this Rect rect, float r, float l, float t, float b)
		{
			return new Rect(rect.x - l, rect.y - t, rect.width + r + l, rect.height + t + b);
		}

		public static Rect Extended(this Rect rect, RectOffset a)
		{
			return new Rect(rect.x - (float)a.left, rect.y - (float)a.top, rect.width + (float)a.right + (float)a.left, rect.height + (float)a.top + (float)a.bottom);
		}

		public static Rect Encapsulate(this Rect r, Rect n)
		{
			if (n.xMin < r.xMin)
			{
				r.xMin = n.xMin;
			}
			if (n.yMin < r.yMin)
			{
				r.yMin = n.yMin;
			}
			if (n.xMax > r.xMax)
			{
				r.xMax = n.xMax;
			}
			if (n.yMax > r.yMax)
			{
				r.yMax = n.yMax;
			}
			return r;
		}

		public static Rect TurnNonNegative(this Rect r)
		{
			float width = r.width;
			if (width < 0f)
			{
				r.width = 0f - width;
				r.x += width;
			}
			float height = r.height;
			if (height < 0f)
			{
				r.height = 0f - height;
				r.y += height;
			}
			return r;
		}

		public static float DistToRectCenter(this Vector3 pos, float offsetX, float offsetZ, float size)
		{
			float num = pos.x - (offsetX + size / 2f);
			float num2 = pos.z - (offsetZ + size / 2f);
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		public static float DistToRectAxisAligned(this Vector3 pos, float offsetX, float offsetZ, float size)
		{
			float num = offsetX - pos.x;
			float num2 = pos.x - offsetX - size;
			float num3 = ((num >= 0f) ? num : ((!(num2 >= 0f)) ? 0f : num2));
			float num4 = offsetZ - pos.z;
			float num5 = pos.z - offsetZ - size;
			float num6 = ((num4 >= 0f) ? num4 : ((!(num5 >= 0f)) ? 0f : num5));
			if (num3 > num6)
			{
				return num3;
			}
			return num6;
		}

		public static float DistToRectCenter(this Vector3[] poses, float offsetX, float offsetZ, float size)
		{
			float num = 200000000f;
			for (int i = 0; i < poses.Length; i++)
			{
				float num2 = poses[i].DistToRectCenter(offsetX, offsetZ, size);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		public static float DistToRectAxisAligned(this Vector3[] poses, float offsetX, float offsetZ, float size)
		{
			float num = 200000000f;
			for (int i = 0; i < poses.Length; i++)
			{
				float num2 = poses[i].DistToRectAxisAligned(offsetX, offsetZ, size);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		public static float DistAxisAligned(this Vector3 center, Vector3 pos)
		{
			float num = center.x - pos.x;
			if (num < 0f)
			{
				num = 0f - num;
			}
			float num2 = center.z - pos.z;
			if (num2 < 0f)
			{
				num2 = 0f - num2;
			}
			if (num > num2)
			{
				return num;
			}
			return num2;
		}

		[Obsolete("Use Coord.Round(v)")]
		public static Coord RoundToCoord(this Vector2 pos)
		{
			int num = (int)(pos.x + 0.5f);
			if (pos.x < 0f)
			{
				num--;
			}
			int num2 = (int)(pos.y + 0.5f);
			if (pos.y < 0f)
			{
				num2--;
			}
			return new Coord(num, num2);
		}

		[Obsolete("Use Coord.Floor(v/cellSize)")]
		public static Coord FloorToCoord(this Vector3 pos, float cellSize)
		{
			return new Coord(Mathf.FloorToInt(pos.x / cellSize), Mathf.FloorToInt(pos.z / cellSize));
		}

		[Obsolete("Use Coord.Ceil(v/cellSize)")]
		public static Coord CeilToCoord(this Vector3 pos, float cellSize)
		{
			return new Coord(Mathf.CeilToInt(pos.x / cellSize), Mathf.CeilToInt(pos.z / cellSize));
		}

		[Obsolete("Use Coord.Round(v/cellSize)")]
		public static Coord RoundToCoord(this Vector3 pos, float cellSize)
		{
			return new Coord(Mathf.RoundToInt(pos.x / cellSize), Mathf.RoundToInt(pos.z / cellSize));
		}

		public static CoordRect ToCoordRect(this Vector3 pos, float range, float cellSize)
		{
			Coord coord = new Coord(Mathf.FloorToInt((pos.x - range) / cellSize), Mathf.FloorToInt((pos.z - range) / cellSize));
			Coord coord2 = new Coord(Mathf.FloorToInt((pos.x + range) / cellSize), Mathf.FloorToInt((pos.z + range) / cellSize)) + 1;
			return new CoordRect(coord, coord2 - coord);
		}

		public static CoordRect GetHeightRect(this Terrain terrain)
		{
			float num = terrain.terrainData.size.x / (float)terrain.terrainData.heightmapResolution;
			int num2 = (int)(terrain.transform.localPosition.x / num + 0.5f);
			if (terrain.transform.localPosition.x < 0f)
			{
				num2--;
			}
			int num3 = (int)(terrain.transform.localPosition.z / num + 0.5f);
			if (terrain.transform.localPosition.z < 0f)
			{
				num3--;
			}
			return new CoordRect(num2, num3, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
		}

		public static Rect GetWorldRect(this Terrain terrain)
		{
			return new Rect(terrain.transform.position.x, terrain.transform.position.z, terrain.terrainData.size.x, terrain.terrainData.size.z);
		}

		public static float[,] SafeGetHeights(this TerrainData data, int offsetX, int offsetZ, int sizeX, int sizeZ)
		{
			if (offsetX < 0)
			{
				sizeX += offsetX;
				offsetX = 0;
			}
			if (offsetZ < 0)
			{
				sizeZ += offsetZ;
				offsetZ = 0;
			}
			int heightmapResolution = data.heightmapResolution;
			if (sizeX + offsetX > heightmapResolution)
			{
				sizeX = heightmapResolution - offsetX;
			}
			if (sizeZ + offsetZ > heightmapResolution)
			{
				sizeZ = heightmapResolution - offsetZ;
			}
			return data.GetHeights(offsetX, offsetZ, sizeX, sizeZ);
		}

		public static float[,,] SafeGetAlphamaps(this TerrainData data, int offsetX, int offsetZ, int sizeX, int sizeZ)
		{
			if (offsetX < 0)
			{
				sizeX += offsetX;
				offsetX = 0;
			}
			if (offsetZ < 0)
			{
				sizeZ += offsetZ;
				offsetZ = 0;
			}
			int alphamapResolution = data.alphamapResolution;
			if (sizeX + offsetX > alphamapResolution)
			{
				sizeX = alphamapResolution - offsetX;
			}
			if (sizeZ + offsetZ > alphamapResolution)
			{
				sizeZ = alphamapResolution - offsetZ;
			}
			return data.GetAlphamaps(offsetX, offsetZ, sizeX, sizeZ);
		}

		public static float GetInterpolated(this float[,] array, float x, float z)
		{
			int num = (int)x;
			if (x < 0f)
			{
				num--;
			}
			int num2 = num + 1;
			int num3 = (int)z;
			if (z < 0f)
			{
				num3--;
			}
			int num4 = num3 + 1;
			float num5 = array[num, num3];
			float num6 = array[num, num4];
			float num7 = array[num2, num3];
			float num8 = array[num2, num4];
			float num9 = x - (float)num;
			float num10 = z - (float)num3;
			float num11 = num5 * (1f - num9) + num6 * num9;
			float num12 = num7 * (1f - num9) + num8 * num9;
			return num11 * (1f - num10) + num12 * num10;
		}

		public static bool Equal(Vector3 v1, Vector3 v2)
		{
			if (Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y))
			{
				return Mathf.Approximately(v1.z, v2.z);
			}
			return false;
		}

		public static bool Equal(Ray r1, Ray r2)
		{
			if (Equal(r1.origin, r2.origin))
			{
				return Equal(r1.direction, r2.direction);
			}
			return false;
		}

		public static Vector3[] InverseTransformPoint(this Transform tfm, Vector3[] points)
		{
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = tfm.InverseTransformPoint(points[i]);
			}
			return points;
		}

		public static Vector3 GetCenter(this Vector3[] poses)
		{
			if (poses.Length == 0)
			{
				return default(Vector3);
			}
			if (poses.Length == 1)
			{
				return poses[0];
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < poses.Length; i++)
			{
				num += poses[i].x;
				num2 += poses[i].y;
				num3 += poses[i].z;
			}
			return new Vector3(num / (float)poses.Length, num2 / (float)poses.Length, num3 / (float)poses.Length);
		}

		public static bool Approximately(Rect r1, Rect r2)
		{
			if (Mathf.Approximately(r1.x, r2.x) && Mathf.Approximately(r1.y, r2.y) && Mathf.Approximately(r1.width, r2.width))
			{
				return Mathf.Approximately(r1.height, r2.height);
			}
			return false;
		}

		public static IEnumerable<Vector3> CircleAround(this Vector3 center, float radius, int numPoints, bool endWhereStart = false)
		{
			float radianStep = (float)Math.PI * 2f / (float)numPoints;
			if (endWhereStart)
			{
				numPoints++;
			}
			for (int i = 0; i < numPoints; i++)
			{
				float f = (float)i * radianStep;
				Vector3 vector = new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f));
				yield return center + vector * radius;
			}
		}

		public static bool IntersectsLine(this Rect rect, Vector2 from, Vector2 to)
		{
			Vector2 position = rect.position;
			Vector2 max = rect.max;
			if ((from.x < position.x && to.x < position.x) || (from.x > max.x && to.x > max.x) || (from.y < position.y && to.y < position.y) || (from.y > max.y && to.y > max.y))
			{
				return false;
			}
			if ((from.x < max.x && to.x < max.x) || (from.x > position.x && to.x > position.x) || (from.y < max.y && to.y < max.y) || (from.y > position.y && to.y > position.y))
			{
				return true;
			}
			Vector2 point = new Vector2(position.x, max.y);
			Vector2 point2 = new Vector2(max.x, position.y);
			bool flag = position.Handness(from, to) > 0f;
			bool flag2 = max.Handness(from, to) > 0f;
			bool flag3 = point.Handness(from, to) > 0f;
			bool flag4 = point2.Handness(from, to) > 0f;
			if (flag && flag2 && flag3 && flag4)
			{
				return false;
			}
			if (!flag && !flag2 && !flag3 && !flag4)
			{
				return false;
			}
			return true;
		}

		public static float Handness(this Vector2 point, Vector2 from, Vector2 to)
		{
			return (point.x - from.x) * (to.y - from.y) - (point.y - from.y) * (to.x - from.x);
		}

		public static float DistanceToLine(this Vector3 point, Vector3 lineStart, Vector3 lineEnd)
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

		public static void ClampLine(this Rect rect, ref Vector2 from, ref Vector2 to, bool checkBothWays = true)
		{
			_ = from - to;
			Vector2 position = rect.position;
			Vector2 max = rect.max;
			if (from.x < position.x)
			{
				float num = (position.x - from.x) / (to.x - from.x);
				from.y = (to.y - from.y) * num + from.y;
				from.x = position.x;
			}
			if (from.x > max.x)
			{
				float num2 = (max.x - from.x) / (to.x - from.x);
				from.y = (to.y - from.y) * num2 + from.y;
				from.x = max.x;
			}
			if (from.y < position.y)
			{
				float num3 = (position.y - from.y) / (to.y - from.y);
				from.x = (to.x - from.x) * num3 + from.x;
				from.y = position.y;
			}
			if (from.y > max.y)
			{
				float num4 = (max.y - from.y) / (to.y - from.y);
				from.x = (to.x - from.x) * num4 + from.x;
				from.y = max.y;
			}
			if (checkBothWays)
			{
				rect.ClampLine(ref to, ref from, checkBothWays = false);
			}
		}
	}
}
