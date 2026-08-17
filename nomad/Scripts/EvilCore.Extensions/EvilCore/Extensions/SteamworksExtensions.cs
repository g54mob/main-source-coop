using Steamworks.Data;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class SteamworksExtensions
	{
		public static Sprite ToSprite(this Image image)
		{
			Texture2D texture2D = new Texture2D((int)image.Width, (int)image.Height, TextureFormat.RGBA32, mipChain: false);
			for (int i = 0; i < image.Width; i++)
			{
				for (int j = 0; j < image.Height; j++)
				{
					Steamworks.Data.Color pixel = image.GetPixel(i, j);
					texture2D.SetPixel(i, (int)image.Height - j, new UnityEngine.Color((float)(int)pixel.r / 255f, (float)(int)pixel.g / 255f, (float)(int)pixel.b / 255f, (float)(int)pixel.a / 255f));
				}
			}
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
		}
	}
}
