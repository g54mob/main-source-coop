using UnityEngine;

namespace StylizedGrass
{
	public class GrassColorMap : ScriptableObject
	{
		public int resolution = 1024;

		public Bounds bounds;

		public Vector4 uv;

		public Texture2D texture;

		public Texture2D customTex;

		[Tooltip("When enabled, a custom color map texture can be used")]
		public bool overrideTexture;

		public bool hasScalemap;

		public static GrassColorMap Active;

		private static readonly int _ColorMap = Shader.PropertyToID("_ColorMap");

		private static readonly int _ColorMapUV = Shader.PropertyToID("_ColorMapUV");

		private static readonly int _ColorMapParams = Shader.PropertyToID("_ColorMapParams");

		public void SetActive()
		{
			if (!texture || (overrideTexture && !customTex))
			{
				Debug.LogWarning("Tried to activate grass color map with null texture", this);
				return;
			}
			Shader.SetGlobalTexture(_ColorMap, overrideTexture ? customTex : texture);
			Shader.SetGlobalVector(_ColorMapUV, uv);
			Shader.SetGlobalVector(_ColorMapParams, new Vector4(1f, hasScalemap ? 1 : 0, 0f, 0f));
			Active = this;
		}

		public static void DisableGlobally()
		{
			Shader.SetGlobalTexture(_ColorMap, null);
			Shader.SetGlobalVector(_ColorMapUV, Vector4.zero);
			Shader.SetGlobalVector(_ColorMapParams, Vector4.zero);
			Active = null;
		}
	}
}
