using System;
using System.Collections.Generic;
using UnityEngine;

namespace Febucci.UI.Core
{
	public static class TextUtilities
	{
		public const int verticesPerChar = 4;

		internal const int fakeRandomsCount = 25;

		internal static Vector3[] fakeRandoms;

		private static bool initialized;

		internal static void Initialize()
		{
			if (!initialized)
			{
				initialized = true;
				List<Vector3> list = new List<Vector3>();
				for (float num = 0f; num < 360f; num += 14f)
				{
					float f = num * ((float)Math.PI / 180f);
					list.Add(new Vector3(Mathf.Sin(f), Mathf.Cos(f)).normalized);
				}
				fakeRandoms = new Vector3[25];
				for (int i = 0; i < fakeRandoms.Length; i++)
				{
					int index = UnityEngine.Random.Range(0, list.Count);
					fakeRandoms[i] = list[index];
					list.RemoveAt(index);
				}
			}
		}

		public static Vector3 RotateAround(this Vector3 vec, Vector2 center, float rotDegrees)
		{
			rotDegrees *= (float)Math.PI / 180f;
			float num = vec.x - center.x;
			float num2 = vec.y - center.y;
			float num3 = num * Mathf.Cos(rotDegrees) - num2 * Mathf.Sin(rotDegrees);
			float num4 = num * Mathf.Sin(rotDegrees) + num2 * Mathf.Cos(rotDegrees);
			vec.x = num3 + center.x;
			vec.y = num4 + center.y;
			return vec;
		}

		public static void MoveChar(this Vector3[] vec, Vector3 dir)
		{
			for (byte b = 0; b < vec.Length; b++)
			{
				vec[b] += dir;
			}
		}

		public static void SetChar(this Vector3[] vec, Vector3 pos)
		{
			for (byte b = 0; b < vec.Length; b++)
			{
				vec[b] = pos;
			}
		}

		public static void LerpUnclamped(this Vector3[] vec, Vector3 target, float pct)
		{
			for (byte b = 0; b < vec.Length; b++)
			{
				vec[b] = Vector3.LerpUnclamped(vec[b], target, pct);
			}
		}

		public static Vector3 GetMiddlePos(this Vector3[] vec)
		{
			return (vec[0] + vec[2]) / 2f;
		}

		public static void RotateChar(this Vector3[] vec, float angle)
		{
			Vector3 middlePos = vec.GetMiddlePos();
			for (byte b = 0; b < vec.Length; b++)
			{
				vec[b] = vec[b].RotateAround(middlePos, angle);
			}
		}

		public static void RotateChar(this Vector3[] vec, float angle, Vector3 pivot)
		{
			for (byte b = 0; b < vec.Length; b++)
			{
				vec[b] = vec[b].RotateAround(pivot, angle);
			}
		}

		public static void SetColor(this Color32[] col, Color32 target)
		{
			for (byte b = 0; b < col.Length; b++)
			{
				col[b] = target;
			}
		}

		public static void LerpUnclamped(this Color32[] col, Color32 target, float pct)
		{
			for (byte b = 0; b < col.Length; b++)
			{
				col[b] = Color32.LerpUnclamped(col[b], target, pct);
			}
		}

		public static float CalculateCurveDuration(this AnimationCurve curve)
		{
			if (curve.keys.Length != 0)
			{
				return curve.keys[curve.length - 1].time;
			}
			return 0f;
		}

		public static bool IsTagLongEnough(string tag)
		{
			return tag.Length >= 3;
		}
	}
}
