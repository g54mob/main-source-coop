using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtDataTypes
	{
		public static decimal Min(this decimal me, decimal other)
		{
			if (!(me < other))
			{
				return other;
			}
			return me;
		}

		public static decimal Max(this decimal me, decimal other)
		{
			if (!(me > other))
			{
				return other;
			}
			return me;
		}

		public static System.Numerics.Vector2 ToNumericsVec(this UnityEngine.Vector2 me)
		{
			return new System.Numerics.Vector2(me.x, me.y);
		}

		public static UnityEngine.Vector2 ToUnityVec(this System.Numerics.Vector2 me)
		{
			return new UnityEngine.Vector2(me.X, me.Y);
		}

		public static float PDistance(this UnityEngine.Vector3 me, UnityEngine.Vector3 other)
		{
			return UnityEngine.Vector3.Distance(me, other);
		}

		public static UnityEngine.Vector2 PToVec2(this float2 me)
		{
			return me;
		}

		public static int PCeilToInt(this float me)
		{
			return Mathf.CeilToInt(me);
		}

		public static float PMin(this float me, float f)
		{
			return Mathf.Min(me, f);
		}

		public static int PMin(this int me, int f)
		{
			return Mathf.Min(me, f);
		}

		public static int PMax(this int me, int f)
		{
			return Mathf.Max(me, f);
		}

		public static float PMax(this float me, float f)
		{
			return Mathf.Max(me, f);
		}

		public static UnityEngine.Vector2 PToVec2(this float me)
		{
			return new UnityEngine.Vector2(me, me);
		}

		public static List<T> PPutInList<T>(this T me)
		{
			return new List<T> { me };
		}

		public static Color PToColor(this float me)
		{
			return new Color(me, me, me, me);
		}

		public static float2 PToF2xy(this UnityEngine.Vector3 me)
		{
			return new float2(me.x, me.y);
		}

		public static float2 PToF2xz(this UnityEngine.Vector3 me)
		{
			return new float2(me.x, me.z);
		}

		public static UnityEngine.Vector3 PToV3(this float3 me)
		{
			return me;
		}

		public static float PDistance(this float2 me, float2 other)
		{
			return math.distance(me, other);
		}

		public static float2 PNormalize(this float2 me)
		{
			return math.normalize(me);
		}

		public static float3 PToF3(this UnityEngine.Vector3 me)
		{
			return me;
		}

		public static float2 PToF2(this UnityEngine.Vector2 me)
		{
			return me;
		}

		public static float3 PToF3(this float2 me)
		{
			return new float3(me.x, me.y, 0f);
		}

		public static float ToFloat(this int me)
		{
			return me;
		}

		public static int PToInt(this float me)
		{
			return (int)me;
		}

		public static UnityEngine.Vector2 ToVec2(this Vector2Int me)
		{
			return new UnityEngine.Vector2(me.x, me.y);
		}

		public static UnityEngine.Vector2 PToVec2(this UnityEngine.Vector3 me)
		{
			return me;
		}

		public static UnityEngine.Vector2 PToVec2XY0(this UnityEngine.Vector3 me)
		{
			return new UnityEngine.Vector2(me.x, me.y);
		}

		public static UnityEngine.Vector2 PToVec2XZ0(this UnityEngine.Vector3 me)
		{
			return new UnityEngine.Vector2(me.x, me.z);
		}

		public static Vector2Int ToIVec2(this float me)
		{
			return new Vector2Int((int)me, (int)me);
		}

		public static (float, float) PToFloat(this (int, int) me)
		{
			return (me.Item1, me.Item2);
		}

		public static UnityEngine.Vector2 PToVec(this (float, float) me)
		{
			return new UnityEngine.Vector2(me.Item1, me.Item2);
		}

		public static Vector2Int ToVec2Int(this (int, int) me)
		{
			return new Vector2Int(me.Item1, me.Item2);
		}

		public static float Abs(this float me)
		{
			return Mathf.Abs(me);
		}

		public static UnityEngine.Vector3 PZeroY(this UnityEngine.Vector3 me)
		{
			me.y = 0f;
			return me;
		}

		public static float2 ToF2XZ(this UnityEngine.Vector3 me)
		{
			return new float2(me.x, me.z);
		}

		public static UnityEngine.Vector3 Abs(this UnityEngine.Vector3 me)
		{
			return new UnityEngine.Vector3(Mathf.Abs(me.x), Mathf.Abs(me.y), Mathf.Abs(me.z));
		}

		public static UnityEngine.Vector3 NewY(this UnityEngine.Vector3 me, float y)
		{
			return new UnityEngine.Vector3(me.x, y, me.z);
		}

		public static UnityEngine.Vector3 ToVec3(this UnityEngine.Vector2 me)
		{
			return new UnityEngine.Vector3(me.x, me.y, 0f);
		}

		public static UnityEngine.Vector2 PToVec(this Vector2Int me)
		{
			return new UnityEngine.Vector2(me.x, me.y);
		}

		public static UnityEngine.Vector3 PToVec3xoy(this UnityEngine.Vector2 me)
		{
			return new UnityEngine.Vector3(me.x, 0f, me.y);
		}

		public static UnityEngine.Vector3 PToVec3xoy(this float2 me)
		{
			return new UnityEngine.Vector3(me.x, 0f, me.y);
		}

		public static float ToInt(this bool me)
		{
			return me ? 1 : 0;
		}

		public static float Sum(this UnityEngine.Vector3 me)
		{
			return me.x + me.y + me.z;
		}

		public static UnityEngine.Vector3 ZeroY(this UnityEngine.Vector3 me)
		{
			me.y = 0f;
			return me;
		}

		public static float PRoundToDecimals(this float me, int decimals)
		{
			return MathF.Truncate(me * MathF.Pow(10f, decimals)) / MathF.Pow(10f, decimals);
		}

		public static bool IsNaN(this UnityEngine.Vector3 me)
		{
			if (!float.IsNaN(me.x) && !float.IsNaN(me.y))
			{
				return float.IsNaN(me.z);
			}
			return true;
		}

		public static bool IsSet(this Enum input, Enum matchTo)
		{
			return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
		}

		public static string PToString(this float f)
		{
			return f.ToString(CultureInfo.InvariantCulture);
		}

		public static string PToString(this decimal d)
		{
			return d.ToString(CultureInfo.InvariantCulture);
		}

		public static float PClamp(this ref float me, float min, float max)
		{
			me = Mathf.Clamp(me, min, max);
			return me;
		}

		public static UnityEngine.Vector3 ToVec(this float me)
		{
			return UnityEngine.Vector3.one * me;
		}

		public static UnityEngine.Vector3 ToVec(this int me)
		{
			return UnityEngine.Vector3.one * me;
		}

		public static UnityEngine.Vector2 ToVec2(this int me)
		{
			return UnityEngine.Vector2.one * me;
		}

		public static UnityEngine.Vector2 ToVec2(this float me)
		{
			return UnityEngine.Vector2.one * me;
		}

		public static float PLoopMe(this ref float me, float min, float max)
		{
			while (me >= max)
			{
				me -= max;
			}
			while (me < min)
			{
				me += max;
			}
			return me;
		}

		public static int PLoopMe(this ref int me, int min, int max)
		{
			while (me >= max)
			{
				me -= max;
			}
			while (me < min)
			{
				me += max;
			}
			return me;
		}

		public static bool InRangeEx(this int me, int min, int max)
		{
			if (me < max)
			{
				return me > min;
			}
			return false;
		}

		public static bool InRangeInc(this int me, int min, int max)
		{
			if (me <= max)
			{
				return me >= min;
			}
			return false;
		}

		public static bool InRangeEx(this UnityEngine.Vector3 me, UnityEngine.Vector3 min, UnityEngine.Vector3 max)
		{
			if (me.x.InRangeEx(min.x, max.x) && me.y.InRangeEx(min.y, max.y) && me.z.InRangeEx(min.z, max.z))
			{
				return true;
			}
			return false;
		}

		public static bool InRangeEx(this float me, float min, float max)
		{
			if (me < max)
			{
				return me > min;
			}
			return false;
		}

		public static float PRndRange(this UnityEngine.Vector2 me)
		{
			return UnityEngine.Random.Range(me.x, me.y);
		}

		public static int PRndRange(this Vector2Int me)
		{
			return UnityEngine.Random.Range(me.x, me.y);
		}

		public static float PClampFloat(this UnityEngine.Vector2 me, float value)
		{
			return Mathf.Clamp(value, me.x, me.y);
		}

		public static void PClampFloat(this UnityEngine.Vector2 me, ref float value)
		{
			value = Mathf.Clamp(value, me.x, me.y);
		}
	}
}
