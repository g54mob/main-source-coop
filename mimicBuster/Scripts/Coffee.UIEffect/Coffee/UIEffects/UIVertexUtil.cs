using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIEffects
{
	internal static class UIVertexUtil
	{
		public static Func<UIVertex, UIVertex, UIVertex, float, UIVertex> onLerpVertex;

		public static void Flip(List<UIVertex> verts, bool horizontal, bool vertical)
		{
			int count = verts.Count;
			for (int i = 0; i < count; i++)
			{
				UIVertex value = verts[i];
				Vector3 position = value.position;
				value.position = new Vector3(horizontal ? (0f - position.x) : position.x, vertical ? (0f - position.y) : position.y);
				verts[i] = value;
			}
		}

		public static void Flip(VertexHelper vh, bool horizontal, bool vertical)
		{
			int currentVertCount = vh.currentVertCount;
			UIVertex vertex = default(UIVertex);
			for (int i = 0; i < currentVertCount; i++)
			{
				vh.PopulateUIVertex(ref vertex, i);
				Vector3 position = vertex.position;
				vertex.position = new Vector3(horizontal ? (0f - position.x) : position.x, vertical ? (0f - position.y) : position.y);
				vh.SetUIVertex(vertex, i);
			}
		}

		public static void ExpandCapacity(List<UIVertex> verts, int multiplier)
		{
			int num = Mathf.NextPowerOfTwo(verts.Count * multiplier);
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
		}

		public static void Expand(List<UIVertex> verts, int start, int bundleSize, Vector4 expandSize, Rect bounds)
		{
			if (expandSize == Vector4.zero)
			{
				return;
			}
			for (int i = 0; i < bundleSize; i += 3)
			{
				if (bounds.Contains(verts[start + i].position) && bounds.Contains(verts[start + i + 1].position) && bounds.Contains(verts[start + i + 2].position))
				{
					continue;
				}
				GetBounds(verts, start + i, 3, out var posBounds, out var uvBounds);
				Vector4 vector = posBounds.center;
				vector.z = vector.x;
				vector.w = vector.y;
				Vector4 vector2 = uvBounds.center;
				vector2.z = vector2.x;
				vector2.w = vector2.y;
				Vector4 a = posBounds.size;
				a.z = 1f + expandSize.z / Mathf.Abs(a.x);
				a.w = 1f + expandSize.w / Mathf.Abs(a.y);
				a.x = 1f + expandSize.x / Mathf.Abs(a.x);
				a.y = 1f + expandSize.y / Mathf.Abs(a.y);
				Vector4 vector3 = vector - Vector4.Scale(a, vector);
				Vector4 vector4 = vector2 - Vector4.Scale(a, vector2);
				for (int j = 0; j < 3; j++)
				{
					UIVertex value = verts[start + i + j];
					Vector3 position = value.position;
					Vector4 uv = value.uv0;
					if (position.x < bounds.xMin)
					{
						position.x = position.x * a.x + vector3.x;
						uv.x = uv.x * a.x + vector4.x;
					}
					else if (bounds.xMax < position.x)
					{
						position.x = position.x * a.z + vector3.z;
						uv.x = uv.x * a.z + vector4.z;
					}
					if (position.y < bounds.yMin)
					{
						position.y = position.y * a.y + vector3.y;
						uv.y = uv.y * a.y + vector4.y;
					}
					else if (bounds.yMax < position.y)
					{
						position.y = position.y * a.w + vector3.w;
						uv.y = uv.y * a.w + vector4.w;
					}
					value.position = position;
					value.uv0.x = uv.x;
					value.uv0.y = uv.y;
					verts[start + i + j] = value;
				}
			}
		}

		public static UIVertex VertexLerp(UIVertex a, UIVertex b, float t)
		{
			UIVertex uIVertex = new UIVertex
			{
				position = Vector3.Lerp(a.position, b.position, t),
				normal = Vector3.Lerp(a.normal, b.normal, t),
				tangent = Vector4.Lerp(a.tangent, b.tangent, t),
				color = Color.Lerp(a.color, b.color, t),
				uv0 = Vector4.Lerp(a.uv0, b.uv0, t),
				uv1 = Vector4.Lerp(a.uv1, b.uv1, t),
				uv2 = Vector4.Lerp(a.uv2, b.uv2, t)
			};
			if (onLerpVertex != null)
			{
				uIVertex = onLerpVertex(uIVertex, a, b, t);
			}
			return uIVertex;
		}

		public static void GetBounds(List<UIVertex> verts, int start, int bundleSize, out Rect posBounds, out Rect uvBounds)
		{
			Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 max = new Vector2(float.MinValue, float.MinValue);
			Vector2 min2 = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 max2 = new Vector2(float.MinValue, float.MinValue);
			for (int i = start; i < start + bundleSize; i++)
			{
				UIVertex uIVertex = verts[i];
				UpdateMinMax(ref min, ref max, uIVertex.position);
				UpdateMinMax(ref min2, ref max2, uIVertex.uv0);
			}
			posBounds = new Rect(min.x + 0.001f, min.y + 0.001f, max.x - min.x - 0.002f, max.y - min.y - 0.002f);
			uvBounds = new Rect(min2.x, min2.y, max2.x - min2.x, max2.y - min2.y);
			static void UpdateMinMax(ref Vector2 reference, ref Vector2 reference2, Vector2 value)
			{
				if (value.x < reference.x)
				{
					reference.x = value.x;
				}
				if (reference2.x < value.x)
				{
					reference2.x = value.x;
				}
				if (value.y < reference.y)
				{
					reference.y = value.y;
				}
				if (reference2.y < value.y)
				{
					reference2.y = value.y;
				}
			}
		}
	}
}
