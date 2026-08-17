using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIEffects
{
	public static class ShadowUtil
	{
		public static Func<UIVertex, float, UIVertex> onMarkAsShadow;

		public static void DoShadow(List<UIVertex> verts, Vector2[] vectors, Vector2 distance, int iteration, float fade)
		{
			UIVertexUtil.ExpandCapacity(verts, 1 + vectors.Length);
			distance = new Vector2(Mathf.Clamp(distance.x, -600f, 600f), Mathf.Clamp(distance.y, -600f, 600f));
			int count = verts.Count;
			int start = 0;
			int end = count;
			Vector2 zero = Vector2.zero;
			float num = fade;
			for (int i = 0; i < iteration; i++)
			{
				zero += distance / (i + 1);
				ApplyShadow(verts, vectors, ref start, ref end, zero, num);
				num *= 0.75f;
			}
			for (int j = 0; j < verts.Count - count; j++)
			{
				if (onMarkAsShadow != null)
				{
					verts[j] = onMarkAsShadow(verts[j], 2f);
				}
			}
		}

		public static void DoMirror(List<UIVertex> verts, Vector2 distance, float scale, float fade, RectTransform root)
		{
			UIVertexUtil.ExpandCapacity(verts, 2);
			distance = new Vector2(Mathf.Clamp(distance.x, -600f, 600f), Mathf.Clamp(distance.y, -600f, 600f));
			int count = verts.Count;
			Rect rect = root.rect;
			float y = root.pivot.y;
			float height = rect.height;
			float x = distance.x;
			float offset = distance.y - (scale + 1f) * y * height;
			Vector2 range = new Vector2(rect.yMin, rect.yMax);
			ApplyMirror(verts, count, x, range, scale, offset, fade);
			for (int i = 0; i < verts.Count - count; i++)
			{
				if (onMarkAsShadow != null)
				{
					verts[i] = onMarkAsShadow(verts[i], 4f);
				}
			}
		}

		private static void ApplyMirror(List<UIVertex> verts, int count, float rate, Vector2 range, float scale, float offset, float alpha)
		{
			rate = Mathf.Clamp01(rate);
			int start = 0;
			int end = count;
			ApplyShadowZeroAlloc(verts, ref start, ref end, 0f, 0f, alpha);
			for (int i = 0; i < count; i += 6)
			{
				UIVertex uIVertex = verts[i];
				float num = Mathf.InverseLerp(range.x, range.y, uIVertex.position.y);
				UIVertex uIVertex2 = verts[i + 1];
				float num2 = Mathf.InverseLerp(range.x, range.y, uIVertex2.position.y);
				UIVertex uIVertex3 = verts[i + 2];
				float num3 = Mathf.InverseLerp(range.x, range.y, uIVertex3.position.y);
				UIVertex uIVertex4 = verts[i + 4];
				float num4 = Mathf.InverseLerp(range.x, range.y, uIVertex4.position.y);
				uIVertex.color.a = (byte)(Mathf.InverseLerp(rate, 0f, num) * (float)(int)uIVertex.color.a);
				uIVertex2.color.a = (byte)(Mathf.InverseLerp(rate, 0f, num2) * (float)(int)uIVertex2.color.a);
				uIVertex3.color.a = (byte)(Mathf.InverseLerp(rate, 0f, num3) * (float)(int)uIVertex3.color.a);
				uIVertex4.color.a = (byte)(Mathf.InverseLerp(rate, 0f, num4) * (float)(int)uIVertex4.color.a);
				if (num < rate && rate < num2)
				{
					float t = (rate - num) / (num2 - num);
					uIVertex2 = UIVertexUtil.VertexLerp(uIVertex, uIVertex2, t);
				}
				if (num4 < rate && rate < num3)
				{
					float t2 = (rate - num4) / (num3 - num4);
					uIVertex3 = UIVertexUtil.VertexLerp(uIVertex4, uIVertex3, t2);
				}
				uIVertex.position.y = (0f - uIVertex.position.y) * scale + offset;
				uIVertex2.position.y = (0f - uIVertex2.position.y) * scale + offset;
				uIVertex3.position.y = (0f - uIVertex3.position.y) * scale + offset;
				uIVertex4.position.y = (0f - uIVertex4.position.y) * scale + offset;
				int index = i;
				UIVertex value = (verts[i + 5] = uIVertex);
				verts[index] = value;
				verts[i + 1] = uIVertex2;
				int index2 = i + 2;
				value = (verts[i + 3] = uIVertex3);
				verts[index2] = value;
				verts[i + 4] = uIVertex4;
			}
		}

		private static void ApplyShadow(List<UIVertex> verts, Vector2[] vectors, ref int start, ref int end, Vector2 distance, float alpha)
		{
			float x = distance.x;
			float y = distance.y;
			for (int i = 0; i < vectors.Length; i++)
			{
				float x2 = x * vectors[i].x;
				float y2 = y * vectors[i].y;
				ApplyShadowZeroAlloc(verts, ref start, ref end, x2, y2, alpha);
			}
		}

		private static void ApplyShadowZeroAlloc(List<UIVertex> verts, ref int start, ref int end, float x, float y, float alpha)
		{
			int num = end - start;
			for (int i = 0; i < num; i++)
			{
				verts.Add(verts[end - num + i]);
				UIVertex value = verts[start + i];
				value.position.x += x;
				value.position.y += y;
				value.color.a = (byte)(alpha * (float)(int)value.color.a);
				verts[start + i] = value;
			}
			start = end;
			end = verts.Count;
		}
	}
}
