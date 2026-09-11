using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace pworld.Scripts.Extensions
{
	public static class ExtGraphics
	{
		public static Color GetColorOfPixel(this RenderTexture me, Vector2 pixelCoord)
		{
			pixelCoord.y = (float)me.height - pixelCoord.y;
			Texture2D texture2D = new Texture2D(1, 1, me.graphicsFormat, TextureCreationFlags.None);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = me;
			Rect source = new Rect(new Vector2(pixelCoord.x, pixelCoord.y), Vector2.one);
			texture2D.ReadPixels(source, 0, 0);
			texture2D.Apply();
			Color pixel = texture2D.GetPixel(0, 0);
			RenderTexture.active = active;
			return pixel;
		}

		public static Texture2D PToTexture2D(this RenderTexture me, Texture2D texture2D)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = me;
			texture2D.ReadPixels(new Rect(0f, 0f, me.width, me.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			return texture2D;
		}

		public static Vector2 GetResolution(this Screen me)
		{
			return new Vector2(Screen.width, Screen.height);
		}

		public static Vector2 TransformUV(this RenderTexture me, Vector2 uv)
		{
			return uv * me.PGetSize();
		}

		public static Vector2 PGetSize(this RenderTexture me)
		{
			return new Vector2(me.width, me.height);
		}

		public static void ClearTexture(this RenderTexture me)
		{
			Graphics.Blit(new Texture2D(me.width, me.height), me);
		}
	}
}
