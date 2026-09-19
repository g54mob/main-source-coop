using UnityEngine;

namespace PlayerCustomization.LobbyAvatar.Systems
{
	public static class LobbyAvatarColor
	{
		public static int Pack(Color color)
		{
			Color32 color2 = color;
			return (color2.r << 24) | (color2.g << 16) | (color2.b << 8) | color2.a;
		}

		public static Color Unpack(int packed)
		{
			byte r = (byte)((packed >> 24) & 0xFF);
			byte g = (byte)((packed >> 16) & 0xFF);
			byte b = (byte)((packed >> 8) & 0xFF);
			byte a = (byte)(packed & 0xFF);
			return new Color32(r, g, b, a);
		}
	}
}
