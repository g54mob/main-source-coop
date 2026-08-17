using UnityEngine;

namespace EvilCore.Extensions
{
	public static class RenderTextureExtensions
	{
		public static Texture2D ToTexture2D(this RenderTexture renderTexture, TextureFormat format)
		{
			Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, format, mipChain: false);
			renderTexture.WriteToTexture2D(texture2D);
			return texture2D;
		}

		public static void WriteToTexture2D(this RenderTexture renderTexture, Texture2D texture)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = renderTexture;
			texture.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
			texture.Apply();
			RenderTexture.active = active;
		}

		public static Sprite ToSprite(this RenderTexture renderTexture, TextureFormat format)
		{
			Texture2D texture2D = renderTexture.ToTexture2D(format);
			return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
		}

		public static void WriteToSprite(this RenderTexture renderTexture, Sprite sprite)
		{
			renderTexture.WriteToTexture2D(sprite.texture);
		}
	}
}
