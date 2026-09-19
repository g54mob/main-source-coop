using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	public static class ObiUtils
	{
		[Flags]
		public enum ParticleFlags
		{
			SelfCollide = 0x1000000,
			Fluid = 0x2000000,
			OneSided = 0x4000000,
			Isolated = 0x8000000
		}

		public const float epsilon = 1E-07f;

		public const float sqrt3 = 1.7320508f;

		public const float sqrt2 = 1.4142135f;

		public const int FilterMaskBitmask = -65536;

		public const int FilterCategoryBitmask = 65535;

		public const int ParticleGroupBitmask = 16777215;

		public const int CollideWithEverything = 65535;

		public const int CollideWithNothing = 0;

		public const int MaxCategory = 15;

		public const int MinCategory = 0;

		public static readonly Color32[] colorAlphabet = new Color32[26]
		{
			new Color32(240, 163, byte.MaxValue, byte.MaxValue),
			new Color32(0, 117, 220, byte.MaxValue),
			new Color32(153, 63, 0, byte.MaxValue),
			new Color32(76, 0, 92, byte.MaxValue),
			new Color32(25, 25, 25, byte.MaxValue),
			new Color32(0, 92, 49, byte.MaxValue),
			new Color32(43, 206, 72, byte.MaxValue),
			new Color32(byte.MaxValue, 204, 153, byte.MaxValue),
			new Color32(128, 128, 128, byte.MaxValue),
			new Color32(148, byte.MaxValue, 181, byte.MaxValue),
			new Color32(143, 124, 0, byte.MaxValue),
			new Color32(157, 204, 0, byte.MaxValue),
			new Color32(194, 0, 136, byte.MaxValue),
			new Color32(0, 51, 128, byte.MaxValue),
			new Color32(byte.MaxValue, 164, 5, byte.MaxValue),
			new Color32(byte.MaxValue, 168, 187, byte.MaxValue),
			new Color32(66, 102, 0, byte.MaxValue),
			new Color32(byte.MaxValue, 0, 16, byte.MaxValue),
			new Color32(94, 241, 242, byte.MaxValue),
			new Color32(0, 153, 143, byte.MaxValue),
			new Color32(224, byte.MaxValue, 102, byte.MaxValue),
			new Color32(116, 10, byte.MaxValue, byte.MaxValue),
			new Color32(153, 0, 0, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, 128, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue),
			new Color32(byte.MaxValue, 80, 5, byte.MaxValue)
		};

		public static readonly string[] categoryNames = new string[16]
		{
			"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
			"10", "11", "12", "13", "14", "15"
		};

		public static Color RGBToAbsorption(this Color rgb)
		{
			Color result = default(Color);
			result.r = Mathf.Pow(1f - rgb.r, 2f) / (2f * rgb.r + 1E-07f);
			result.g = Mathf.Pow(1f - rgb.g, 2f) / (2f * rgb.g + 1E-07f);
			result.b = Mathf.Pow(1f - rgb.b, 2f) / (2f * rgb.b + 1E-07f);
			result.a = rgb.a;
			return result;
		}

		public static Color AbsorptionToRGB(this Color k)
		{
			Color result = default(Color);
			result.r = 1f + k.r - Mathf.Sqrt(k.r * (k.r + 2f));
			result.g = 1f + k.g - Mathf.Sqrt(k.g * (k.g + 2f));
			result.b = 1f + k.b - Mathf.Sqrt(k.b * (k.b + 2f));
			result.a = k.a;
			return result;
		}

		public static void DrawArrowGizmo(float bodyLenght, float bodyWidth, float headLenght, float headWidth)
		{
			float num = bodyLenght * 0.5f;
			float num2 = bodyWidth * 0.5f;
			Gizmos.DrawLine(new Vector3(num2, 0f, 0f - num), new Vector3(num2, 0f, num));
			Gizmos.DrawLine(new Vector3(0f - num2, 0f, 0f - num), new Vector3(0f - num2, 0f, num));
			Gizmos.DrawLine(new Vector3(0f - num2, 0f, 0f - num), new Vector3(num2, 0f, 0f - num));
			Gizmos.DrawLine(new Vector3(num2, 0f, num), new Vector3(headWidth, 0f, num));
			Gizmos.DrawLine(new Vector3(0f - num2, 0f, num), new Vector3(0f - headWidth, 0f, num));
			Gizmos.DrawLine(new Vector3(0f, 0f, num + headLenght), new Vector3(headWidth, 0f, num));
			Gizmos.DrawLine(new Vector3(0f, 0f, num + headLenght), new Vector3(0f - headWidth, 0f, num));
		}

		public static void DebugDrawBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x.SetTRS(pos, rot, scale);
			Vector3 vector = matrix4x.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
			Vector3 vector2 = matrix4x.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
			Vector3 vector3 = matrix4x.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
			Vector3 vector4 = matrix4x.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));
			Vector3 vector5 = matrix4x.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
			Vector3 vector6 = matrix4x.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
			Vector3 vector7 = matrix4x.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
			Vector3 vector8 = matrix4x.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));
			Debug.DrawLine(vector, vector2, c);
			Debug.DrawLine(vector2, vector3, c);
			Debug.DrawLine(vector3, vector4, c);
			Debug.DrawLine(vector4, vector, c);
			Debug.DrawLine(vector5, vector6, c);
			Debug.DrawLine(vector6, vector7, c);
			Debug.DrawLine(vector7, vector8, c);
			Debug.DrawLine(vector8, vector5, c);
			Debug.DrawLine(vector, vector5, c);
			Debug.DrawLine(vector2, vector6, c);
			Debug.DrawLine(vector3, vector7, c);
			Debug.DrawLine(vector4, vector8, c);
		}

		public static void DebugDrawCross(Vector3 pos, float size, Color color)
		{
			Debug.DrawLine(pos - Vector3.right * size, pos + Vector3.right * size, color);
			Debug.DrawLine(pos - Vector3.up * size, pos + Vector3.up * size, color);
			Debug.DrawLine(pos - Vector3.forward * size, pos + Vector3.forward * size, color);
		}

		public static int CeilToPowerOfTwo(this int count)
		{
			int num;
			for (num = 1; num < count; num <<= 1)
			{
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(ref T lhs, ref T rhs)
		{
			T val = lhs;
			lhs = rhs;
			rhs = val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(this T[] source, int index1, int index2)
		{
			if (source != null && index1 >= 0 && index2 >= 0 && index1 < source.Length && index2 < source.Length)
			{
				T val = source[index1];
				source[index1] = source[index2];
				source[index2] = val;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(this IList<T> list, int index1, int index2)
		{
			if (list != null && index1 >= 0 && index2 >= 0 && index1 < list.Count && index2 < list.Count)
			{
				T value = list[index1];
				list[index1] = list[index2];
				list[index2] = value;
			}
		}

		public static void ShiftLeft<T>(this T[] source, int index, int count, int positions)
		{
			for (int i = 0; i < positions; i++)
			{
				for (int j = index; j < index + count; j++)
				{
					source.Swap(j, j - 1);
				}
				index--;
			}
		}

		public static void ShiftRight<T>(this T[] source, int index, int count, int positions)
		{
			for (int i = 0; i < positions; i++)
			{
				for (int num = index + count - 1; num >= index; num--)
				{
					source.Swap(num, num + 1);
				}
				index++;
			}
		}

		public static int Unique<T>(this IList<T> list, Func<T, T, bool> equals)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (!equals(list[num], list[i]) && ++num != i)
				{
					list[num] = list[i];
				}
			}
			return num + 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool AreValid(this Bounds bounds)
		{
			if (!float.IsNaN(bounds.max.x) && !float.IsInfinity(bounds.max.x) && !float.IsNaN(bounds.max.y) && !float.IsInfinity(bounds.max.y) && !float.IsNaN(bounds.max.z) && !float.IsInfinity(bounds.max.z) && !float.IsNaN(bounds.min.x) && !float.IsInfinity(bounds.min.x) && !float.IsNaN(bounds.min.y) && !float.IsInfinity(bounds.min.y) && !float.IsNaN(bounds.min.z))
			{
				return !float.IsInfinity(bounds.min.z);
			}
			return false;
		}

		public static Bounds Transform(this Bounds b, Matrix4x4 m)
		{
			Vector4 vector = m.GetColumn(0) * b.min.x;
			Vector4 vector2 = m.GetColumn(0) * b.max.x;
			Vector4 vector3 = m.GetColumn(1) * b.min.y;
			Vector4 vector4 = m.GetColumn(1) * b.max.y;
			Vector4 vector5 = m.GetColumn(2) * b.min.z;
			Vector4 vector6 = m.GetColumn(2) * b.max.z;
			Bounds result = default(Bounds);
			Vector3 vector7 = m.GetColumn(3);
			result.SetMinMax(Vector3.Min(vector, vector2) + Vector3.Min(vector3, vector4) + Vector3.Min(vector5, vector6) + vector7, Vector3.Max(vector, vector2) + Vector3.Max(vector3, vector4) + Vector3.Max(vector5, vector6) + vector7);
			return result;
		}

		public static int CountTrailingZeroes(int x)
		{
			int num = 1;
			int num2 = 0;
			while (num2 < 32)
			{
				if ((x & num) != 0)
				{
					return num2;
				}
				num2++;
				num <<= 1;
			}
			return 32;
		}

		public static void Add(Vector3 a, Vector3 b, ref Vector3 result)
		{
			result.x = a.x + b.x;
			result.y = a.y + b.y;
			result.z = a.z + b.z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Remap(this float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Mod(float a, float b)
		{
			return a - b * Mathf.Floor(a / b);
		}

		public static Matrix4x4 Add(this Matrix4x4 a, Matrix4x4 other)
		{
			for (int i = 0; i < 16; i++)
			{
				a[i] += other[i];
			}
			return a;
		}

		public static float FrobeniusNorm(this Matrix4x4 a)
		{
			float num = 0f;
			for (int i = 0; i < 16; i++)
			{
				num += a[i] * a[i];
			}
			if (!(num > 0f))
			{
				return 0f;
			}
			return Mathf.Sqrt(num);
		}

		public static Matrix4x4 ScalarMultiply(this Matrix4x4 a, float s)
		{
			for (int i = 0; i < 16; i++)
			{
				a[i] *= s;
			}
			return a;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Diagonal(this Matrix4x4 value)
		{
			return new Vector4(value.m00, value.m11, value.m22, value.m33);
		}

		public static Vector3 ProjectPointLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point, out float mu, bool clampToSegment = true)
		{
			Vector3 lhs = point - lineStart;
			Vector3 vector = lineEnd - lineStart;
			mu = Vector3.Dot(lhs, vector) / Vector3.Dot(vector, vector);
			if (clampToSegment)
			{
				mu = Mathf.Clamp01(mu);
			}
			return lineStart + vector * mu;
		}

		public static bool LinePlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection, out Vector3 point)
		{
			point = linePoint;
			Vector3 normalized = lineDirection.normalized;
			float num = Vector3.Dot(planeNormal, normalized);
			if (Mathf.Approximately(num, 0f))
			{
				return false;
			}
			float num2 = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint)) / num;
			point = linePoint + normalized * num2;
			return true;
		}

		public static float RaySphereIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 center, float radius)
		{
			Vector3 vector = rayOrigin - center;
			float num = Vector3.Dot(rayDirection, rayDirection);
			float num2 = 2f * Vector3.Dot(vector, rayDirection);
			float num3 = Vector3.Dot(vector, vector) - radius * radius;
			float num4 = num2 * num2 - 4f * num * num3;
			if (num4 < 0f)
			{
				return -1f;
			}
			return (0f - num2 - Mathf.Sqrt(num4)) / (2f * num);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float InvMassToMass(float invMass)
		{
			return 1f / invMass;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MassToInvMass(float mass)
		{
			return 1f / Mathf.Max(mass, 1E-05f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PureSign(this float val)
		{
			return ((0f <= val) ? 1 : 0) - ((val < 0f) ? 1 : 0);
		}

		public static void NearestPointOnTri(in Vector3 p1, in Vector3 p2, in Vector3 p3, in Vector3 p, out Vector3 result)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p2.z - p1.z;
			float num4 = p3.x - p1.x;
			float num5 = p3.y - p1.y;
			float num6 = p3.z - p1.z;
			float num7 = p1.x - p.x;
			float num8 = p1.y - p.y;
			float num9 = p1.z - p.z;
			float num10 = num * num + num2 * num2 + num3 * num3;
			float num11 = num * num4 + num2 * num5 + num3 * num6;
			float num12 = num4 * num4 + num5 * num5 + num6 * num6;
			float num13 = num * num7 + num2 * num8 + num3 * num9;
			float num14 = num4 * num7 + num5 * num8 + num6 * num9;
			float num15 = num10 * num12 - num11 * num11;
			float num16 = num11 * num14 - num12 * num13;
			float num17 = num11 * num13 - num10 * num14;
			if (num16 + num17 <= num15)
			{
				if (num16 < 0f)
				{
					if (num17 < 0f)
					{
						if (num13 < 0f)
						{
							num17 = 0f;
							num16 = ((!(0f - num13 >= num10)) ? ((0f - num13) / num10) : 1f);
						}
						else
						{
							num16 = 0f;
							num17 = ((num14 >= 0f) ? 0f : ((!(0f - num14 >= num12)) ? ((0f - num14) / num12) : 1f));
						}
					}
					else
					{
						num16 = 0f;
						num17 = ((num14 >= 0f) ? 0f : ((!(0f - num14 >= num12)) ? ((0f - num14) / num12) : 1f));
					}
				}
				else if (num17 < 0f)
				{
					num17 = 0f;
					num16 = ((num13 >= 0f) ? 0f : ((!(0f - num13 >= num10)) ? ((0f - num13) / num10) : 1f));
				}
				else
				{
					float num18 = 1f / num15;
					num16 *= num18;
					num17 *= num18;
				}
			}
			else if (num16 < 0f)
			{
				float num19 = num11 + num13;
				float num20 = num12 + num14;
				if (num20 > num19)
				{
					float num21 = num20 - num19;
					float num22 = num10 - 2f * num11 + num12;
					if (num21 >= num22)
					{
						num16 = 1f;
						num17 = 0f;
					}
					else
					{
						num16 = num21 / num22;
						num17 = 1f - num16;
					}
				}
				else
				{
					num16 = 0f;
					num17 = ((num20 <= 0f) ? 1f : ((!(num14 >= 0f)) ? ((0f - num14) / num12) : 0f));
				}
			}
			else if (num17 < 0f)
			{
				float num19 = num11 + num14;
				float num20 = num10 + num13;
				if (num20 > num19)
				{
					float num21 = num20 - num19;
					float num22 = num10 - 2f * num11 + num12;
					if (num21 >= num22)
					{
						num17 = 1f;
						num16 = 0f;
					}
					else
					{
						num17 = num21 / num22;
						num16 = 1f - num17;
					}
				}
				else
				{
					num17 = 0f;
					num16 = ((num20 <= 0f) ? 1f : ((!(num13 >= 0f)) ? ((0f - num13) / num10) : 0f));
				}
			}
			else
			{
				float num21 = num12 + num14 - num11 - num13;
				if (num21 <= 0f)
				{
					num16 = 0f;
					num17 = 1f;
				}
				else
				{
					float num22 = num10 - 2f * num11 + num12;
					if (num21 >= num22)
					{
						num16 = 1f;
						num17 = 0f;
					}
					else
					{
						num16 = num21 / num22;
						num17 = 1f - num16;
					}
				}
			}
			result.x = p1.x + num16 * num + num17 * num4;
			result.y = p1.y + num16 * num2 + num17 * num5;
			result.z = p1.z + num16 * num3 + num17 * num6;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float TriangleArea(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			return Mathf.Sqrt(Vector3.Cross(p2 - p1, p3 - p1).sqrMagnitude) / 2f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float TetraVolume(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			return Vector3.Dot(Vector3.Cross(p1, p2), p3) / 6f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 TriangleNormal(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			return Vector3.Cross(p2 - p1, p3 - p1).normalized;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float TriangleAspectRatio(float a, float b, float c)
		{
			float num = (a + b + c) / 2f;
			float num2 = 8f * (num - a) * (num - b) * (num - c);
			if (num2 > 1E-07f)
			{
				return a * b * c / num2;
			}
			return 0f;
		}

		public static void BestTriangleAxisProjection(Vector3 p1, Vector3 p2, Vector3 p3, out Vector2 r1, out Vector2 r2, out Vector2 r3)
		{
			float num = TriangleArea(new Vector3(0f, p1.y, p1.z), new Vector3(0f, p2.y, p2.z), new Vector3(0f, p3.y, p3.z));
			float num2 = TriangleArea(new Vector3(p1.x, 0f, p1.z), new Vector3(p2.x, 0f, p2.z), new Vector3(p3.x, 0f, p3.z));
			float num3 = TriangleArea(new Vector3(p1.x, p1.y, 0f), new Vector3(p2.x, p2.y, 0f), new Vector3(p3.x, p3.y, 0f));
			if (num > num2 && num > num3)
			{
				r1 = new Vector2(p1.y, p1.z);
				r2 = new Vector2(p2.y, p2.z);
				r3 = new Vector2(p3.y, p3.z);
			}
			else if (num2 > num && num2 > num3)
			{
				r1 = new Vector2(p1.x, p1.z);
				r2 = new Vector2(p2.x, p2.z);
				r3 = new Vector2(p3.x, p3.z);
			}
			else
			{
				r1 = new Vector2(p1.x, p1.y);
				r2 = new Vector2(p2.x, p2.y);
				r3 = new Vector2(p3.x, p3.y);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float EllipsoidVolume(Vector3 principalRadii)
		{
			return 4.1887903f * principalRadii.x * principalRadii.y * principalRadii.z;
		}

		public static Quaternion RestDarboux(Quaternion q1, Quaternion q2)
		{
			Quaternion result = Quaternion.Inverse(q1) * q2;
			Vector4 vector = new Vector4(result.x, result.y, result.z, result.w + 1f);
			if (new Vector4(result.x, result.y, result.z, result.w - 1f).sqrMagnitude > vector.sqrMagnitude)
			{
				result = new Quaternion(result.x * -1f, result.y * -1f, result.z * -1f, result.w * -1f);
			}
			return result;
		}

		public static Vector4 AsVector4(this Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RestBendingConstraint(Vector3 positionA, Vector3 positionB, Vector3 positionC)
		{
			Vector3 vector = (positionA + positionB + positionC) / 3f;
			return (positionC - vector).magnitude;
		}

		public static IEnumerable BilateralInterleaved(int count)
		{
			int i = 0;
			while (i < count)
			{
				if (i % 2 != 0)
				{
					yield return count - count % 2 - i;
				}
				else
				{
					yield return i;
				}
				int num = i + 1;
				i = num;
			}
		}

		public static void BarycentricCoordinates(in Vector3 A, in Vector3 B, in Vector3 C, in Vector3 P, ref Vector3 bary)
		{
			Vector3 vector = C - A;
			Vector3 vector2 = B - A;
			Vector3 rhs = P - A;
			float num = Vector3.Dot(vector, vector);
			float num2 = Vector3.Dot(vector, vector2);
			float num3 = Vector3.Dot(vector, rhs);
			float num4 = Vector3.Dot(vector2, vector2);
			float num5 = Vector3.Dot(vector2, rhs);
			float num6 = num * num4 - num2 * num2;
			if ((double)Math.Abs(num6) > 1E-38)
			{
				float num7 = (num4 * num3 - num2 * num5) / num6;
				float num8 = (num * num5 - num2 * num3) / num6;
				bary = new Vector3(1f - num7 - num8, num8, num7);
			}
		}

		public static void BarycentricInterpolation(in Vector3 p1, in Vector3 p2, in Vector3 p3, in Vector3 coords, out Vector3 result)
		{
			result.x = coords.x * p1.x + coords.y * p2.x + coords.z * p3.x;
			result.y = coords.x * p1.y + coords.y * p2.y + coords.z * p3.y;
			result.z = coords.x * p1.z + coords.y * p2.z + coords.z * p3.z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float BarycentricInterpolation(float p1, float p2, float p3, Vector3 coords)
		{
			return coords[0] * p1 + coords[1] * p2 + coords[2] * p3;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float BarycentricExtrapolationScale(Vector3 coords)
		{
			return 1f / (coords[0] * coords[0] + coords[1] * coords[1] + coords[2] * coords[2]);
		}

		public static Vector3[] CalculateAngleWeightedNormals(Vector3[] vertices, int[] triangles)
		{
			Vector3[] array = new Vector3[vertices.Length];
			Dictionary<Vector3, Vector3> dictionary = new Dictionary<Vector3, Vector3>();
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 vector = vertices[triangles[i]];
				Vector3 vector2 = vertices[triangles[i + 1]];
				Vector3 vector3 = vertices[triangles[i + 2]];
				if (!dictionary.ContainsKey(vector))
				{
					dictionary[vector] = Vector3.zero;
				}
				if (!dictionary.ContainsKey(vector2))
				{
					dictionary[vector2] = Vector3.zero;
				}
				if (!dictionary.ContainsKey(vector3))
				{
					dictionary[vector3] = Vector3.zero;
				}
				Vector3 lhs = vector2 - vector;
				Vector3 rhs = vector3 - vector;
				dictionary[vector] += Vector3.Cross(lhs, rhs).normalized * Mathf.Acos(Vector3.Dot(lhs.normalized, rhs.normalized));
				lhs = vector3 - vector2;
				rhs = vector - vector2;
				dictionary[vector2] += Vector3.Cross(lhs, rhs).normalized * Mathf.Acos(Vector3.Dot(lhs.normalized, rhs.normalized));
				lhs = vector - vector3;
				rhs = vector2 - vector3;
				dictionary[vector3] += Vector3.Cross(lhs, rhs).normalized * Mathf.Acos(Vector3.Dot(lhs.normalized, rhs.normalized));
			}
			for (int j = 0; j < vertices.Length; j++)
			{
				array[j] = dictionary[vertices[j]].normalized;
			}
			return array;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int MakePhase(int group, ParticleFlags flags)
		{
			return (group & 0xFFFFFF) | (int)flags;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetGroupFromPhase(int phase)
		{
			return phase & 0xFFFFFF;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ParticleFlags GetFlagsFromPhase(int phase)
		{
			return (ParticleFlags)(phase & -16777216);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int MakeFilter(int mask, int category)
		{
			return (mask << 16) | (1 << category);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetCategoryFromFilter(int filter)
		{
			return CountTrailingZeroes(filter & 0xFFFF);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetMaskFromFilter(int filter)
		{
			return (filter & -65536) >> 16;
		}

		public static void EigenSolve(Matrix4x4 D, out Vector3 S, out Matrix4x4 V)
		{
			S = EigenValues(D);
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (S[0] - S[1] > S[1] - S[2])
			{
				vector = EigenVector(D, S[0]);
				if (S[1] - S[2] < 1E-07f)
				{
					vector2 = vector.unitOrthogonal();
				}
				else
				{
					vector2 = EigenVector(D, S[2]);
					vector2 -= vector * Vector3.Dot(vector, vector2);
					vector2 = Vector3.Normalize(vector2);
				}
				vector3 = Vector3.Cross(vector2, vector);
			}
			else
			{
				vector2 = EigenVector(D, S[2]);
				if (S[0] - S[1] < 1E-07f)
				{
					vector3 = vector2.unitOrthogonal();
				}
				else
				{
					vector3 = EigenVector(D, S[1]);
					vector3 -= vector2 * Vector3.Dot(vector2, vector3);
					vector3 = Vector3.Normalize(vector3);
				}
				vector = Vector3.Cross(vector3, vector2);
			}
			V = Matrix4x4.identity;
			V.SetColumn(0, vector);
			V.SetColumn(1, vector3);
			V.SetColumn(2, vector2);
		}

		private static Vector3 unitOrthogonal(this Vector3 input)
		{
			if (!(input.x < input.z * 1E-07f) || !(input.y < input.z * 1E-07f))
			{
				float num = 1f / Vector3.Magnitude(new Vector2(input.x, input.y));
				return new Vector3((0f - input.y) * num, input.x * num, 0f);
			}
			float num2 = 1f / Vector3.Magnitude(new Vector2(input.y, input.z));
			return new Vector3(0f, (0f - input.z) * num2, input.y * num2);
		}

		private static Vector3 EigenVector(Matrix4x4 D, float S)
		{
			Vector4 column = D.GetColumn(0);
			column[0] -= S;
			Vector4 column2 = D.GetColumn(1);
			column2[1] -= S;
			Vector4 column3 = D.GetColumn(2);
			column3[2] -= S;
			Vector3 vector = new Vector3(column2[1] * column3[2] - column3[1] * column3[1], 0f, 0f);
			Vector3 vector2 = new Vector3(column3[1] * column3[0] - column2[0] * column3[2], column[0] * column3[2] - column3[0] * column3[0], 0f);
			Vector3 vector3 = new Vector3(column2[0] * column3[1] - column2[1] * column3[0], column2[0] * column3[0] - column[0] * column3[1], column[0] * column2[1] - column2[0] * column2[0]);
			float num = vector2[0] * vector2[0];
			float num2 = vector3[0] * vector3[0];
			float num3 = vector3[1] * vector3[1];
			Vector3 vector4 = new Vector3(vector[0] * vector[0] + num + num2, num + vector2[1] * vector2[1] + num3, num2 + num3 + vector3[2] * vector3[2]);
			int num4 = 0;
			num4 = ((!(vector4[0] > vector4[1]) || !(vector4[0] > vector4[2])) ? ((vector4[1] > vector4[0] && vector4[1] > vector4[2]) ? 1 : 2) : 0);
			Vector3 vector5 = Vector3.zero;
			if (vector4[num4] < 1E-07f)
			{
				vector5[0] = 1f;
				return vector5;
			}
			switch (num4)
			{
			case 0:
				vector5[0] = vector[0];
				vector5[1] = vector2[0];
				vector5[2] = vector3[0];
				break;
			case 1:
				vector5[0] = vector2[0];
				vector5[1] = vector2[1];
				vector5[2] = vector3[1];
				break;
			default:
				vector5 = vector3;
				break;
			}
			return Vector3.Normalize(vector5);
		}

		private static Vector3 EigenValues(Matrix4x4 D)
		{
			float num = 1f / 3f;
			float num2 = 1f / 6f;
			float num3 = Mathf.Sqrt(3f);
			Vector3 vector = D.GetColumn(0);
			Vector3 vector2 = D.GetColumn(1);
			Vector3 vector3 = D.GetColumn(2);
			float num4 = num * (vector[0] + vector2[1] + vector3[2]);
			float num5 = vector[0] - num4;
			float num6 = vector2[1] - num4;
			float num7 = vector3[2] - num4;
			float num8 = vector2[0] * vector2[0];
			float num9 = vector3[0] * vector3[0];
			float num10 = vector3[1] * vector3[1];
			float num11 = 0.5f * (num5 * (num6 * num7 - num10) - num7 * num8 - num6 * num9) + vector2[0] * vector3[1] * vector[2];
			float num12 = num2 * (num5 * num5 + num6 * num6 + num7 * num7 + 2f * (num8 + num9 + num10));
			float num13 = Mathf.Sqrt(num12);
			float b = num12 * num12 * num12 - num11 * num11;
			float f = num * Mathf.Atan2(Mathf.Sqrt(Mathf.Max(0f, b)), num11);
			float num14 = Mathf.Cos(f);
			float num15 = Mathf.Sin(f);
			float num16 = num13 * num14;
			float num17 = num13 * num3 * num15;
			float num18 = num4 + 2f * num16;
			float num19 = num4 - num16 - num17;
			float num20 = num4 - num16 + num17;
			if (num18 > num19)
			{
				float num21 = num18;
				num18 = num19;
				num19 = num21;
			}
			if (num18 > num20)
			{
				float num22 = num18;
				num18 = num20;
				num20 = num22;
			}
			if (num19 > num20)
			{
				float num23 = num19;
				num19 = num20;
				num20 = num23;
			}
			return new Vector3(num20, num19, num18);
		}

		public static Vector3 GetPointCloudCentroid(List<Vector3> points)
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < points.Count; i++)
			{
				zero += points[i];
			}
			return zero / points.Count;
		}

		public static void GetPointCloudAnisotropy(List<Vector3> points, float max_anisotropy, float radius, in Vector3 hint_normal, ref Vector3 centroid, ref Quaternion orientation, ref Vector3 principal_radii)
		{
			int count = points.Count;
			if (count < 2 || radius <= 0f || max_anisotropy <= 0f)
			{
				principal_radii = Vector3.one * radius;
				orientation = Quaternion.identity;
				return;
			}
			centroid = GetPointCloudCentroid(points);
			Vector4 zero = Vector4.zero;
			Vector4 zero2 = Vector4.zero;
			Vector4 zero3 = Vector4.zero;
			Matrix4x4 zero4 = Matrix4x4.zero;
			for (int i = 0; i < count; i++)
			{
				Vector4 vector = points[i] - centroid;
				zero += vector * vector[0];
				zero2 += vector * vector[1];
				zero3 += vector * vector[2];
			}
			float a = Mathf.Max(Mathf.Max(Mathf.Abs(zero.x), Mathf.Abs(zero.y)), Mathf.Abs(zero.z));
			float b = Mathf.Max(Mathf.Max(Mathf.Abs(zero2.x), Mathf.Abs(zero2.y)), Mathf.Abs(zero2.z));
			float num = Mathf.Max(b: Mathf.Max(Mathf.Max(Mathf.Abs(zero3.x), Mathf.Abs(zero3.y)), Mathf.Abs(zero3.z)), a: Mathf.Max(a, b));
			if (num > 1E-07f)
			{
				zero /= num;
				zero2 /= num;
				zero3 /= num;
			}
			zero4.SetColumn(0, zero);
			zero4.SetColumn(1, zero2);
			zero4.SetColumn(2, zero3);
			EigenSolve(zero4, out principal_radii, out var V);
			if (Vector3.Dot(V.GetColumn(2), hint_normal) < 0f)
			{
				V.SetColumn(2, V.GetColumn(2) * -1f);
				V.SetColumn(1, V.GetColumn(1) * -1f);
			}
			num = principal_radii[0];
			principal_radii = Vector3.Max(principal_radii, Vector3.one * num / max_anisotropy) / num * radius;
			orientation = V.rotation;
		}

		public static int MergeBatches<T>(List<T> batches, int count, bool trimExcess) where T : IRenderBatch
		{
			int num = 0;
			for (int i = 1; i < count; i++)
			{
				T val = batches[num];
				if (num != i && !val.TryMergeWith(batches[i]))
				{
					batches.Swap(++num, i);
				}
			}
			num++;
			if (trimExcess && num < batches.Count)
			{
				batches.RemoveRange(num, batches.Count - num);
			}
			return num;
		}

		public static int MergeBatches<T>(List<T> batches) where T : IRenderBatch
		{
			return MergeBatches(batches, batches.Count, trimExcess: true);
		}

		public static void Concatenate(this MemoryStream ms, Vector3 v)
		{
			for (int i = 0; i < 3; i++)
			{
				byte[] bytes = BitConverter.GetBytes(v[i]);
				ms.Write(bytes, 0, bytes.Length);
			}
		}

		public static void Concatenate(this MemoryStream ms, Quaternion q)
		{
			for (int i = 0; i < 4; i++)
			{
				byte[] bytes = BitConverter.GetBytes(q[i]);
				ms.Write(bytes, 0, bytes.Length);
			}
		}

		public static void Concatenate(this MemoryStream ms, float f)
		{
			byte[] bytes = BitConverter.GetBytes(f);
			ms.Write(bytes, 0, bytes.Length);
		}

		public static void Concatenate(this MemoryStream ms, int f)
		{
			byte[] bytes = BitConverter.GetBytes(f);
			ms.Write(bytes, 0, bytes.Length);
		}

		public static uint Adler32(byte[] bytes)
		{
			uint num = 1u;
			uint num2 = 0u;
			foreach (byte b in bytes)
			{
				num = (num + b) % 65521;
				num2 = (num2 + num) % 65521;
			}
			return (num2 << 16) | num;
		}

		public unsafe static Vector3 OctDecode(float k)
		{
			uint num = *(uint*)(&k);
			Vector2 vector = new Vector2((float)(num >> 16) / 65535f, (float)(num & 0xFFFF) / 65535f);
			vector.x = vector.x * 2f - 1f;
			vector.y = vector.y * 2f - 1f;
			Vector3 value = new Vector3(vector.x, vector.y, 1f - Mathf.Abs(vector.x) - Mathf.Abs(vector.y));
			float num2 = Mathf.Max(0f - value.z, 0f);
			value.x += ((value.x >= 0f) ? (0f - num2) : num2);
			value.y += ((value.y >= 0f) ? (0f - num2) : num2);
			return Vector3.Normalize(value);
		}

		public unsafe static Vector4 UnpackFloatRGBA(float v)
		{
			int num = *(int*)(&v);
			float x = (float)((uint)(num & -16777216) >> 24) / 255f;
			float y = (float)((uint)(num & 0xFF0000) >> 16) / 255f;
			float z = (float)((uint)(num & 0xFF00) >> 8) / 255f;
			float w = (float)(uint)(num & 0xFF) / 255f;
			return new Vector4(x, y, z, w);
		}

		public unsafe static float PackFloatRGBA(Vector4 enc)
		{
			uint num = ((uint)(enc.x * 255f) << 24) + ((uint)(enc.y * 255f) << 16) + ((uint)(enc.z * 255f) << 8) + (uint)(enc.w * 255f);
			return *(float*)(&num);
		}

		public unsafe static Vector2 UnpackFloatRG(float v)
		{
			int num = *(int*)(&v);
			float x = (float)((uint)(num & -65536) >> 16) / 65535f;
			float y = (float)(uint)(num & 0xFFFF) / 65535f;
			return new Vector2(x, y);
		}

		public unsafe static float PackFloatRG(Vector2 enc)
		{
			uint num = ((uint)(enc.x * 65535f) << 16) + (uint)(enc.y * 65535f);
			return *(float*)(&num);
		}
	}
}
