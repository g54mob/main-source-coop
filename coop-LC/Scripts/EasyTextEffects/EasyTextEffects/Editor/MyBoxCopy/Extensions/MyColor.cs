using UnityEngine;
using UnityEngine.UI;

namespace EasyTextEffects.Editor.MyBoxCopy.Extensions
{
	public static class MyColor
	{
		private const float LightOffset = 0.0625f;

		public static Color RandomBright => new Color(Random.Range(0.4f, 1f), Random.Range(0.4f, 1f), Random.Range(0.4f, 1f));

		public static Color RandomDim => new Color(Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.8f), Random.Range(0.4f, 0.8f));

		public static Color RandomColor => new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));

		public static Color WithAlphaSetTo(this Color color, float a)
		{
			return new Color(color.r, color.g, color.b, a);
		}

		public static void SetAlpha(this Graphic graphic, float a)
		{
			graphic.color = graphic.color.WithAlphaSetTo(a);
		}

		public static void SetAlpha(this SpriteRenderer renderer, float a)
		{
			renderer.color = renderer.color.WithAlphaSetTo(a);
		}

		public static string ToHex(this Color color)
		{
			return $"#{(int)(color.r * 255f):X2}{(int)(color.g * 255f):X2}{(int)(color.b * 255f):X2}";
		}

		public static Color Lighter(this Color color)
		{
			return color.BrightnessOffset(0.0625f);
		}

		public static Color Darker(this Color color)
		{
			return color.BrightnessOffset(-0.0625f);
		}

		public static Color BrightnessOffset(this Color color, float offset)
		{
			return new Color(color.r + offset, color.g + offset, color.b + offset, color.a);
		}

		public static Color ToUnityColor(this string source)
		{
			ColorUtility.TryParseHtmlString(source, out var color);
			return color;
		}
	}
}
