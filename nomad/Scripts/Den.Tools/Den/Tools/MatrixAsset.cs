using System;
using System.IO;
using Den.Tools.Matrices;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	[HelpURL("https://gitlab.com/denispahunov/mapmagic/wikis/home")]
	[CreateAssetMenu(menuName = "MapMagic/Imported Map", fileName = "Imported Map.asset", order = 113)]
	[PreferBinarySerialization]
	public class MatrixAsset : ScriptableObject
	{
		public enum Source
		{
			Raw = 0,
			Texture = 1,
			New = 2
		}

		public enum Channel
		{
			Average = 0,
			Grayscale = 1,
			Red = 2,
			Green = 3,
			Blue = 4,
			Alpha = 5
		}

		public Matrix matrix = new Matrix(new CoordRect(0, 0, 1, 1));

		public Texture2D preview;

		public Source source;

		public string rawPath;

		public Texture2D textureSource;

		public Channel channelSource;

		public static Action<MatrixAsset> OnReloaded;

		public static Action<Texture2D> OnTextureImported;

		public MatrixAsset()
		{
			OnTextureImported = (Action<Texture2D>)Delegate.Combine(OnTextureImported, (Action<Texture2D>)delegate(Texture2D tex)
			{
				if (source == Source.Texture && textureSource == tex)
				{
					Reload();
				}
			});
		}

		public static void ImportRaw(ref Matrix matrix, string path = null)
		{
			FileStream fileStream = new FileInfo(path).Open(FileMode.Open, FileAccess.Read);
			int num = (int)Mathf.Sqrt(fileStream.Length / 2);
			byte[] array = new byte[num * num * 2];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			if (matrix == null || matrix.rect.size.x != num || matrix.rect.size.z != num)
			{
				matrix = new Matrix(new CoordRect(0, 0, num, num));
			}
			matrix.ImportRaw16(array, num, num);
			Matrix matrix2 = new Matrix(matrix.rect);
			MatrixOps.FlipVertical(matrix, matrix2);
			matrix = matrix2;
		}

		public static void ImportTexture(ref Matrix matrix, Texture2D texture, Channel channel = Channel.Average)
		{
			if (!texture.IsReadable())
			{
				texture = texture.ReadableClone();
			}
			Color[] pixels = texture.GetPixels();
			if (matrix == null || matrix.rect.size.x != texture.width || matrix.rect.size.z != texture.height)
			{
				matrix = new Matrix(new CoordRect(0, 0, texture.width, texture.height));
			}
			int num = 0;
			Coord min = matrix.rect.Min;
			Coord max = matrix.rect.Max;
			for (int i = min.z; i < max.z; i++)
			{
				for (int j = min.x; j < max.x; j++)
				{
					float value = channel switch
					{
						Channel.Grayscale => 0.21f * pixels[num].r + 0.72f * pixels[num].g + 0.07f * pixels[num].b, 
						Channel.Red => pixels[num].r, 
						Channel.Green => pixels[num].g, 
						Channel.Blue => pixels[num].b, 
						Channel.Alpha => pixels[num].a, 
						_ => (pixels[num].r + pixels[num].g + pixels[num].b) / 3f, 
					};
					matrix[j, i] = value;
					num++;
				}
			}
		}

		public static void ImportArray(ref Matrix matrix, float[,] heights)
		{
			if (matrix == null || matrix.rect.size.x != heights.GetLength(1) || matrix.rect.size.z != heights.GetLength(0))
			{
				matrix = new Matrix(new CoordRect(0, 0, heights.GetLength(1), heights.GetLength(0)));
			}
			matrix.ImportHeights(heights);
		}

		public void RefreshPreview(int size = 128)
		{
			if (this.matrix != null)
			{
				Matrix matrix = this.matrix;
				preview = new Texture2D(matrix.rect.size.x, matrix.rect.size.z);
				matrix.ExportTexture(preview);
			}
			else
			{
				preview = TextureExtensions.ColorTexture(2, 2, Color.black);
			}
		}

		public void Reload()
		{
			if (source == Source.Raw)
			{
				if (rawPath != null)
				{
					ImportRaw(ref matrix, rawPath);
				}
			}
			else if (textureSource != null)
			{
				ImportTexture(ref matrix, textureSource, channelSource);
			}
			RefreshPreview(256);
			OnReloaded?.Invoke(this);
		}
	}
}
