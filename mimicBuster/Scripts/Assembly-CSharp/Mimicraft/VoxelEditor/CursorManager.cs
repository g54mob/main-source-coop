using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class CursorManager
	{
		public enum Kind
		{
			Default = 0,
			TransformIdle = 1,
			TransformAxis = 2,
			TransformEdge = 3,
			TransformDrag = 4,
			Extrude = 5,
			Paint = 6,
			LoopCut = 7,
			Eyedropper = 8
		}

		private const int Size = 32;

		public static Texture2D EyedropperIcon;

		private static Texture2D transformIdleTexture;

		private static Texture2D transformEdgeTexture;

		private static Texture2D transformDragTexture;

		private static Texture2D extrudeTexture;

		private static Texture2D loopCutTexture;

		private static Texture2D paintTexture;

		private static Color paintTextureTint;

		private static bool paintTextureBuilt;

		private static Texture2D eyedropperFallbackTexture;

		private static Kind currentKind = Kind.Default;

		private static Color currentTint;

		private static bool hasCurrentTint;

		private static readonly Dictionary<Texture2D, Texture2D> cursorSafeCopies = new Dictionary<Texture2D, Texture2D>();

		public static void Set(Kind kind, Color? tint = null)
		{
			Color color = tint ?? Color.white;
			bool flag = hasCurrentTint != tint.HasValue || (tint.HasValue && color != currentTint);
			if (kind != currentKind || flag)
			{
				currentKind = kind;
				currentTint = color;
				hasCurrentTint = tint.HasValue;
				Vector2 hotspot = Vector2.zero;
				Texture2D texture = GetTexture(kind, color, out hotspot);
				if (texture == null)
				{
					Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
				}
				else
				{
					Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
				}
			}
		}

		public static void ResetToDefault()
		{
			if (currentKind != Kind.Default)
			{
				currentKind = Kind.Default;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
		}

		private static Texture2D GetTexture(Kind kind, Color tint, out Vector2 hotspot)
		{
			hotspot = new Vector2(16f, 16f);
			switch (kind)
			{
			case Kind.TransformIdle:
				return transformIdleTexture ?? (transformIdleTexture = BuildRing(32, 9f, 2f, new Color(1f, 1f, 1f, 0.9f)));
			case Kind.TransformAxis:
				return BuildDot(32, 7f, tint);
			case Kind.TransformEdge:
				return transformEdgeTexture ?? (transformEdgeTexture = BuildRing(32, 9f, 3f, new Color(1f, 0.9f, 0.2f, 0.95f)));
			case Kind.TransformDrag:
				return transformDragTexture ?? (transformDragTexture = BuildDot(32, 10f, new Color(1f, 1f, 1f, 0.95f)));
			case Kind.Extrude:
				hotspot.y = 3f;
				return extrudeTexture ?? (extrudeTexture = BuildArrowUp(32, new Color(1f, 0.6f, 0.2f, 0.95f)));
			case Kind.Paint:
				if (!paintTextureBuilt || paintTextureTint != tint)
				{
					paintTexture = BuildDot(32, 8f, tint);
					paintTextureTint = tint;
					paintTextureBuilt = true;
				}
				return paintTexture;
			case Kind.LoopCut:
				return loopCutTexture ?? (loopCutTexture = BuildCross(32, new Color(0.9f, 0.2f, 0.85f, 0.95f)));
			case Kind.Eyedropper:
			{
				Texture2D texture2D = CursorSafe(EyedropperIcon);
				if (texture2D != null)
				{
					hotspot.y = texture2D.height;
					hotspot.x = 0f;
					return texture2D;
				}
				hotspot.y = 32f;
				hotspot.x = 0f;
				return eyedropperFallbackTexture ?? (eyedropperFallbackTexture = BuildDot(32, 8f, new Color(0.9f, 0.85f, 0.3f, 0.95f)));
			}
			default:
				return null;
			}
		}

		private static Texture2D CursorSafe(Texture2D source)
		{
			if (source == null)
			{
				return null;
			}
			if (source.format == TextureFormat.RGBA32 && source.isReadable && source.mipmapCount == 1)
			{
				return source;
			}
			if (cursorSafeCopies.TryGetValue(source, out var value))
			{
				return value;
			}
			Texture2D texture2D = BuildCursorSafeCopy(source);
			cursorSafeCopies[source] = texture2D;
			return texture2D;
		}

		private static Texture2D BuildCursorSafeCopy(Texture2D source)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture renderTexture = null;
			Texture2D texture2D = null;
			try
			{
				int num = Mathf.Max(1, source.width);
				int num2 = Mathf.Max(1, source.height);
				renderTexture = RenderTexture.GetTemporary(num, num2, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
				Graphics.Blit(source, renderTexture);
				RenderTexture.active = renderTexture;
				texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, mipChain: false, linear: false)
				{
					filterMode = source.filterMode,
					wrapMode = TextureWrapMode.Clamp
				};
				texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
				texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CursorManager] '" + source.name + "' imlec dokusuna cevrilemedi: " + ex.Message + " - prosedurel imlece dusulecek.");
				if (texture2D != null)
				{
					UnityEngine.Object.Destroy(texture2D);
					texture2D = null;
				}
			}
			finally
			{
				RenderTexture.active = active;
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
			return texture2D;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			foreach (Texture2D value in cursorSafeCopies.Values)
			{
				if (value != null)
				{
					UnityEngine.Object.Destroy(value);
				}
			}
			cursorSafeCopies.Clear();
			EyedropperIcon = null;
		}

		private static Texture2D NewTransparentTexture(int size)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, mipChain: false)
			{
				filterMode = FilterMode.Bilinear
			};
			Color[] array = new Color[size * size];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.clear;
			}
			texture2D.SetPixels(array);
			return texture2D;
		}

		private static Texture2D BuildDot(int size, float radius, Color color)
		{
			Texture2D texture2D = NewTransparentTexture(size);
			Vector2 b = new Vector2(size - 1, size - 1) * 0.5f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (Vector2.Distance(new Vector2(j, i), b) <= radius)
					{
						texture2D.SetPixel(j, i, color);
					}
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		private static Texture2D BuildRing(int size, float radius, float thickness, Color color)
		{
			Texture2D texture2D = NewTransparentTexture(size);
			Vector2 b = new Vector2(size - 1, size - 1) * 0.5f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (Mathf.Abs(Vector2.Distance(new Vector2(j, i), b) - radius) <= thickness * 0.5f)
					{
						texture2D.SetPixel(j, i, color);
					}
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		private static Texture2D BuildCross(int size, Color color)
		{
			Texture2D texture2D = NewTransparentTexture(size);
			float num = (float)(size - 1) * 0.5f;
			float num2 = 2f;
			float num3 = (float)size * 0.35f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num4 = (float)j - num;
					float num5 = (float)i - num;
					float f = (num4 + num5) * 0.70710677f;
					float f2 = (num4 - num5) * 0.70710677f;
					bool num6 = Mathf.Abs(f2) <= num2 * 0.5f && Mathf.Abs(f) <= num3;
					bool flag = Mathf.Abs(f) <= num2 * 0.5f && Mathf.Abs(f2) <= num3;
					if (num6 || flag)
					{
						texture2D.SetPixel(j, i, color);
					}
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		private static Texture2D BuildArrowUp(int size, Color color)
		{
			Texture2D texture2D = NewTransparentTexture(size);
			float num = (float)(size - 1) * 0.5f;
			float num2 = 2f;
			float num3 = (float)size * 0.25f;
			float num4 = (float)size * 0.15f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float f = (float)j - num;
					float num5 = i;
					bool num6 = Mathf.Abs(f) <= num2 && num5 <= (float)size * 0.85f;
					float value = Mathf.InverseLerp(num4, size - 1, num5);
					bool flag = num5 >= num4 && Mathf.Abs(f) <= num3 * (1f - Mathf.Clamp01(value));
					if (num6 || flag)
					{
						texture2D.SetPixel(j, i, color);
					}
				}
			}
			texture2D.Apply();
			return texture2D;
		}
	}
}
