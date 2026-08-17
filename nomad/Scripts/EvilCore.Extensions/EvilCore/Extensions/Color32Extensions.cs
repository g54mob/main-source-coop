using UnityEngine;

namespace EvilCore.Extensions
{
	public static class Color32Extensions
	{
		public static Color32 With(this Color32 color, int channel, byte value)
		{
			color[channel] = value;
			return color;
		}

		public static Color32 WithR(this Color32 color, byte r)
		{
			return color.With(0, r);
		}

		public static Color32 WithG(this Color32 color, byte g)
		{
			return color.With(1, g);
		}

		public static Color32 WithB(this Color32 color, byte b)
		{
			return color.With(2, b);
		}

		public static Color32 WithA(this Color32 color, byte a)
		{
			return color.With(3, a);
		}

		public static Color32 With(this Color32 color, int channel1, byte value1, int channel2, byte value2)
		{
			color[channel1] = value1;
			color[channel2] = value2;
			return color;
		}

		public static Color32 WithRg(this Color32 color, byte r, byte g)
		{
			return color.With(0, r, 1, g);
		}

		public static Color32 WithRb(this Color32 color, byte r, byte b)
		{
			return color.With(0, r, 2, b);
		}

		public static Color32 WithRa(this Color32 color, byte r, byte a)
		{
			return color.With(0, r, 3, a);
		}

		public static Color32 WithGb(this Color32 color, byte g, byte b)
		{
			return color.With(1, g, 2, b);
		}

		public static Color32 WithGa(this Color32 color, byte g, byte a)
		{
			return color.With(1, g, 3, a);
		}

		public static Color32 WithBa(this Color32 color, byte b, byte a)
		{
			return color.With(2, b, 3, a);
		}

		public static Color32 With(this Color32 color, int channel1, byte value1, int channel2, byte value2, int channel3, byte value3)
		{
			color[channel1] = value1;
			color[channel2] = value2;
			color[channel3] = value3;
			return color;
		}

		public static Color32 WithRGB(this Color32 color, byte r, byte g, byte b)
		{
			return color.With(0, r, 1, g, 2, b);
		}

		public static Color32 WithRga(this Color32 color, byte r, byte g, byte a)
		{
			return color.With(0, r, 1, g, 3, a);
		}

		public static Color32 WithRba(this Color32 color, byte r, byte b, byte a)
		{
			return color.With(0, r, 2, b, 3, a);
		}

		public static Color32 WithGba(this Color32 color, byte g, byte b, byte a)
		{
			return color.With(1, g, 2, b, 3, a);
		}
	}
}
