using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.Extensions
{
	public static class GraphicExtensions
	{
		public static void SetColorR(this Graphic graphic, float r)
		{
			graphic.color = graphic.color.WithR(r);
		}

		public static void SetColorG(this Graphic graphic, float g)
		{
			graphic.color = graphic.color.WithG(g);
		}

		public static void SetColorB(this Graphic graphic, float b)
		{
			graphic.color = graphic.color.WithB(b);
		}

		public static void SetColorA(this Graphic graphic, float a)
		{
			graphic.color = graphic.color.WithA(a);
		}

		public static void SetColorRg(this Graphic graphic, float r, float g)
		{
			graphic.color = graphic.color.WithRg(r, g);
		}

		public static void SetColorRb(this Graphic graphic, float r, float b)
		{
			graphic.color = graphic.color.WithRb(r, b);
		}

		public static void SetColorRa(this Graphic graphic, float r, float a)
		{
			graphic.color = graphic.color.WithRa(r, a);
		}

		public static void SetColorGb(this Graphic graphic, float g, float b)
		{
			graphic.color = graphic.color.WithGb(g, b);
		}

		public static void SetColorGa(this Graphic graphic, float g, float a)
		{
			graphic.color = graphic.color.WithGa(g, a);
		}

		public static void SetColorBa(this Graphic graphic, float b, float a)
		{
			graphic.color = graphic.color.WithBa(b, a);
		}

		public static void SetColorRGB(this Graphic graphic, float r, float g, float b)
		{
			graphic.color = graphic.color.WithRGB(r, g, b);
		}

		public static void SetColorRGB(this Graphic graphic, Color color)
		{
			graphic.color = color.WithA(graphic.color.a);
		}

		public static void SetColorRga(this Graphic graphic, float r, float g, float a)
		{
			graphic.color = graphic.color.WithRga(r, g, a);
		}

		public static void SetColorRba(this Graphic graphic, float r, float b, float a)
		{
			graphic.color = graphic.color.WithRba(r, b, a);
		}

		public static void SetColorGba(this Graphic graphic, float g, float b, float a)
		{
			graphic.color = graphic.color.WithGba(g, b, a);
		}
	}
}
