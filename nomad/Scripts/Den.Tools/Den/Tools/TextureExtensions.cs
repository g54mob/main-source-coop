using System.Collections.Generic;
using System.IO;
using Den.Tools.Matrices;
using UnityEngine;

namespace Den.Tools
{
	public static class TextureExtensions
	{
		public enum TextureType
		{
			RGBA = 0,
			RGB = 1,
			Normal = 2,
			Monochrome = 3,
			MonochromeFloat = 4,
			Manual = 5
		}

		public static readonly HashSet<TextureFormat> uncompressedFormats = new HashSet<TextureFormat>(new TextureFormat[16]
		{
			TextureFormat.Alpha8,
			TextureFormat.ARGB32,
			TextureFormat.ARGB4444,
			TextureFormat.R16,
			TextureFormat.R8,
			TextureFormat.RFloat,
			TextureFormat.RG16,
			TextureFormat.RGB24,
			TextureFormat.RGB565,
			TextureFormat.RGB9e5Float,
			TextureFormat.RGBA32,
			TextureFormat.RGBA4444,
			TextureFormat.RGBAFloat,
			TextureFormat.RGBAHalf,
			TextureFormat.RGFloat,
			TextureFormat.RHalf
		});

		public static bool IsReadable(this Texture2D tex)
		{
			try
			{
				tex.GetPixel(0, 0);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsLinear(this Texture tex)
		{
			return false;
		}

		public static Texture2D ReadableClone(this Texture2D tex)
		{
			Texture2D texture2D = new Texture2D(tex.width, tex.height, tex.format, mipChain: true, tex.IsLinear());
			Graphics.CopyTexture(tex, texture2D);
			texture2D.Apply(updateMipmaps: false);
			return texture2D;
		}

		public static Texture2D UncompressedClone(this Texture2D tex)
		{
			int mipmapCount = tex.mipmapCount;
			Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, mipmapCount != 1, tex.IsLinear());
			for (int i = 0; i < mipmapCount; i++)
			{
				Color[] pixels = tex.GetPixels(i);
				texture2D.SetPixels(pixels, i);
			}
			texture2D.Apply(updateMipmaps: false);
			return texture2D;
		}

		public static Texture2D ResizedClone(this Texture2D tex, int newWidth, int newHeight)
		{
			Texture2D texture2D = new Texture2D(newWidth, newHeight, TextureFormat.ARGB32, mipChain: true, tex.IsLinear());
			texture2D.name = tex.name;
			Color[] pixels = tex.GetPixels();
			pixels = pixels.ResizeColorArray(tex.width, tex.height, newWidth, newHeight);
			texture2D.SetPixels(pixels);
			texture2D.Apply(updateMipmaps: true);
			return texture2D;
		}

		public static Color[] ResizeColorArray(this Color[] srcColors, int oldWidth, int oldHeight, int newWidth, int newHeight)
		{
			Color[] array = new Color[newWidth * newHeight];
			Matrix matrix = new Matrix(new CoordRect(0, 0, oldWidth, oldHeight));
			Matrix matrix2 = new Matrix(new CoordRect(0, 0, newWidth, newHeight));
			for (int i = 0; i < srcColors.Length; i++)
			{
				matrix.arr[i] = srcColors[i].r;
			}
			MatrixOps.Resize(matrix, matrix2);
			for (int j = 0; j < array.Length; j++)
			{
				array[j].r = matrix2.arr[j];
			}
			for (int k = 0; k < srcColors.Length; k++)
			{
				matrix.arr[k] = srcColors[k].g;
			}
			MatrixOps.Resize(matrix, matrix2);
			for (int l = 0; l < array.Length; l++)
			{
				array[l].g = matrix2.arr[l];
			}
			for (int m = 0; m < srcColors.Length; m++)
			{
				matrix.arr[m] = srcColors[m].b;
			}
			MatrixOps.Resize(matrix, matrix2);
			for (int n = 0; n < array.Length; n++)
			{
				array[n].b = matrix2.arr[n];
			}
			for (int num = 0; num < srcColors.Length; num++)
			{
				matrix.arr[num] = srcColors[num].a;
			}
			MatrixOps.Resize(matrix, matrix2);
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				array[num2].a = matrix2.arr[num2];
			}
			return array;
		}

		public static Texture2D ColorTexture(int width, int height, Color color, bool linear = false)
		{
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: true, linear);
			texture2D.Colorize(color);
			return texture2D;
		}

		public static void Colorize(this Texture2D tex, Color color)
		{
			Color[] pixels = tex.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = color;
			}
			tex.SetPixels(pixels);
			tex.Apply();
		}

		public static Texture2D Clone(this Texture2D src)
		{
			Texture2D texture2D = new Texture2D(src.width, src.height, src.format, src.mipmapCount != 1);
			Graphics.CopyTexture(src, texture2D);
			texture2D.Apply(updateMipmaps: false);
			return texture2D;
		}

		public static void ClearAlpha(this Texture2D tex)
		{
			Color[] pixels = tex.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i].a = 1f;
			}
			tex.SetPixels(pixels);
			tex.Apply();
		}

		public static void ApplyGamma(this Texture2D tex, float gamma = 2.2f)
		{
			float p = 1f / gamma;
			Color[] pixels = tex.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = new Color(Mathf.Pow(pixels[i].r, p), Mathf.Pow(pixels[i].g, p), Mathf.Pow(pixels[i].b, p), pixels[i].a);
			}
			tex.SetPixels(pixels);
			tex.Apply();
		}

		public static void RestoreNormalmap(this Texture2D tex)
		{
			Color[] pixels = tex.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				Vector2 vector = new Vector2(pixels[i].g * 2f - 1f, pixels[i].a * 2f - 1f);
				float num = Mathf.Sqrt(1f - Mathf.Clamp01(Vector3.Dot(vector, vector)));
				pixels[i] = new Color(pixels[i].g, pixels[i].a, num / 2f + 0.5f, 1f);
			}
			tex.SetPixels(pixels);
			tex.Apply();
		}

		public static void Multiply(this Texture2D tex, Color color, bool multiplyAlpha = false)
		{
			Color[] pixels = tex.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i].r = pixels[i].r * color.r;
				pixels[i].g = pixels[i].g * color.g;
				pixels[i].b = pixels[i].b * color.b;
				if (multiplyAlpha)
				{
					pixels[i].a = pixels[i].a * color.a;
				}
			}
			tex.SetPixels(pixels);
			tex.Apply();
		}

		public static void SaveAsPNG(this Texture2D origTex, string savePath, bool linear = false, bool normal = false)
		{
			Texture2D texture2D = origTex;
			if (!texture2D.IsReadable())
			{
				texture2D = texture2D.ReadableClone();
			}
			if (texture2D.format.IsCompressed())
			{
				texture2D = texture2D.UncompressedClone();
			}
			if (linear)
			{
				if (texture2D == origTex)
				{
					texture2D = texture2D.Clone();
				}
				texture2D.ApplyGamma(0.45454547f);
				texture2D.Apply(updateMipmaps: false);
			}
			if (normal)
			{
				if (texture2D == origTex)
				{
					texture2D = texture2D.Clone();
				}
				texture2D.RestoreNormalmap();
				texture2D.Apply(updateMipmaps: false);
			}
			savePath = savePath.Replace(Application.dataPath, "Assets");
			File.WriteAllBytes(savePath, texture2D.EncodeToPNG());
		}

		public static Hash128 GetHash(this Texture2D tex)
		{
			return default(Hash128);
		}

		public static bool IsCompressed(this TextureFormat format)
		{
			if (uncompressedFormats.Contains(format))
			{
				return false;
			}
			return true;
		}

		public static TextureFormat AutoFormat(TextureType type, bool compressed)
		{
			if (compressed)
			{
				return TextureFormat.DXT5;
			}
			return type switch
			{
				TextureType.RGB => TextureFormat.RGB24, 
				TextureType.Normal => TextureFormat.RG16, 
				TextureType.Monochrome => TextureFormat.R8, 
				TextureType.MonochromeFloat => TextureFormat.RFloat, 
				_ => TextureFormat.RGBA32, 
			};
		}
	}
}
