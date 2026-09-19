using UnityEngine;

namespace Features.StoreModule.Scripts
{
	public static class StoreRewardColorPacking
	{
		public static int Pack(Color color)
		{
			Color32 color2 = color;
			return (color2.a << 24) | (color2.r << 16) | (color2.g << 8) | color2.b;
		}

		public static Color Unpack(int packed)
		{
			byte a = (byte)(packed >> 24);
			byte r = (byte)(packed >> 16);
			byte g = (byte)(packed >> 8);
			byte b = (byte)packed;
			return new Color32(r, g, b, a);
		}
	}
}
