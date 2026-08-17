using System;
using System.Threading.Tasks;
using UnityEngine;

namespace VisualDesignCafe.Nature
{
	[Serializable]
	public class ShaderResources
	{
		private const int _perlinNoiseResolution = 2048;

		private const float _perlinNoiseScale = 256f;

		private const float _perlinNoiseWhiteBalance = 1.2f;

		[SerializeField]
		private Texture2D _perlinNoise;

		public Texture2D PerlinNoise
		{
			get
			{
				if (_perlinNoise == null)
				{
					_perlinNoise = GeneratePerlinNoiseTexture();
				}
				return _perlinNoise;
			}
		}

		public void SetFloatingOrigin(double x, double z)
		{
			Shader.SetGlobalVector("g_FloatingOriginOffset_Color", new Vector4((float)x, (float)z, 0f, 0f));
		}

		internal void Initialize()
		{
			Shader.SetGlobalTexture("g_PerlinNoise", PerlinNoise);
			Shader.SetGlobalFloat("g_PerlinNoiseScale", 0.00390625f);
		}

		internal void Destroy()
		{
			if (_perlinNoise != null)
			{
				UnityEngine.Object.DestroyImmediate(_perlinNoise);
			}
		}

		private Texture2D GeneratePerlinNoiseTexture()
		{
			Texture2D texture2D = new Texture2D(2048, 2048, SystemInfo.SupportsTextureFormat(TextureFormat.R8) ? TextureFormat.R8 : TextureFormat.ARGB32, mipChain: false);
			Color[] pixels = new Color[texture2D.width * texture2D.height];
			float textureWidth = texture2D.width;
			float textureHeight = texture2D.height;
			int textureWidthInt = texture2D.width;
			int textureHeightInt = texture2D.height;
			int blockSize = SystemInfo.processorCount;
			int toExclusive = (int)textureHeight / blockSize + 1;
			Parallel.For(0, toExclusive, new ParallelOptions
			{
				MaxDegreeOfParallelism = SystemInfo.processorCount
			}, delegate(int block)
			{
				Color white = Color.white;
				for (int i = 0; i < blockSize; i++)
				{
					int num = block * blockSize + i;
					if (num < textureHeightInt)
					{
						for (int j = 0; j < textureWidthInt; j++)
						{
							float num2 = Mathf.PerlinNoise((float)j / textureWidth * 256f, (float)num / textureHeight * 256f);
							white.b = (white.g = (white.r = Mathf.Clamp01(num2 * 1.2f)));
							pixels[j + num * textureWidthInt] = white;
						}
					}
				}
			});
			texture2D.name = "Perlin Noise";
			texture2D.SetPixels(pixels);
			texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			return texture2D;
		}
	}
}
