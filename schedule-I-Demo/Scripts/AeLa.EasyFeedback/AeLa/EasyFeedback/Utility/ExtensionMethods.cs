using UnityEngine;

namespace AeLa.EasyFeedback.Utility
{
	internal static class ExtensionMethods
	{
		public static string WrapToClass(this string source, string topClass)
		{
			return "{\"" + topClass + "\": " + source + "}";
		}

		public static void Scale(this Texture2D tex, float scale, FilterMode filterMode = FilterMode.Trilinear, bool updateMipMaps = false)
		{
			int num = Mathf.RoundToInt((float)tex.width * scale);
			int num2 = Mathf.RoundToInt((float)tex.height * scale);
			tex.filterMode = filterMode;
			tex.Apply(updateMipMaps);
			Graphics.SetRenderTarget(new RenderTexture(num, num2, 32));
			GL.LoadPixelMatrix(0f, 1f, 1f, 0f);
			GL.Clear(clearDepth: true, clearColor: true, default(Color));
			Graphics.DrawTexture(new Rect(0f, 0f, 1f, 1f), tex);
			tex.Reinitialize(num, num2);
			tex.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0, updateMipMaps);
			tex.Apply(updateMipMaps);
		}
	}
}
