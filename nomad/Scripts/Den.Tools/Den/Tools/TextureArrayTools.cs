using System;
using UnityEngine;

namespace Den.Tools
{
	public static class TextureArrayTools
	{
		public static void SetTexture(this Texture2DArray dstArr, Texture2D src, int dstCh, bool apply = true)
		{
			if (dstArr.depth <= dstCh)
			{
				throw new IndexOutOfRangeException("Trying to set channel (" + dstCh + ") >= depth (" + dstArr.depth + ")");
			}
			if (src.width == dstArr.width && src.height == dstArr.height && src.format == dstArr.format)
			{
				Graphics.CopyTexture(src, 0, dstArr, dstCh);
				if (apply)
				{
					dstArr.Apply(updateMipmaps: false);
				}
				return;
			}
			if (!src.IsReadable())
			{
				src = src.ReadableClone();
			}
			if (src.format.IsCompressed())
			{
				src = src.UncompressedClone();
			}
			if (src.width != dstArr.width || src.height != dstArr.height)
			{
				src = src.ResizedClone(dstArr.width, dstArr.height);
			}
			if (dstArr.format.IsCompressed())
			{
				src.Compress(highQuality: true);
			}
			src.Apply(updateMipmaps: false);
			Graphics.CopyTexture(src, 0, dstArr, dstCh);
			if (apply)
			{
				dstArr.Apply(updateMipmaps: false);
			}
		}

		public static void SetTextureAlpha(this Texture2DArray dstArr, Texture2D src, Texture2D alpha, int dstCh, bool apply = true)
		{
			if (alpha == null)
			{
				dstArr.SetTexture(src, dstCh, apply);
				return;
			}
			if (!src.IsReadable())
			{
				src = src.ReadableClone();
			}
			if (src.format.IsCompressed())
			{
				src = src.UncompressedClone();
			}
			if (src.width != dstArr.width || src.height != dstArr.height)
			{
				src = src.ResizedClone(dstArr.width, dstArr.height);
			}
			if (!alpha.IsReadable())
			{
				alpha = alpha.ReadableClone();
			}
			if (alpha.width != dstArr.width || alpha.height != dstArr.height)
			{
				alpha = alpha.ResizedClone(dstArr.width, dstArr.height);
			}
			Texture2D texture2D = new Texture2D(src.width, src.height, TextureFormat.RGBA32, mipChain: true, src.IsLinear());
			int mipmapCount = src.mipmapCount;
			for (int i = 0; i < mipmapCount; i++)
			{
				Color[] pixels = src.GetPixels(i);
				Color[] pixels2 = alpha.GetPixels(i);
				for (int j = 0; j < pixels.Length; j++)
				{
					pixels[j] = new Color(pixels[j].r, pixels[j].g, pixels[j].b, pixels2[j].r * 0.3f + pixels2[j].r * 0.6f + pixels2[j].r * 0.1f);
				}
				texture2D.SetPixels(pixels, i);
			}
			texture2D.Apply(updateMipmaps: false);
			dstArr.SetTexture(texture2D, dstCh, apply);
		}

		public static Texture2D GetTexture(this Texture2DArray srcArr, int srcCh, bool readable = true)
		{
			Texture2D texture2D = new Texture2D(srcArr.width, srcArr.height, srcArr.format, mipChain: true, srcArr.IsLinear());
			Graphics.CopyTexture(srcArr, srcCh, texture2D, 0);
			texture2D.Apply(updateMipmaps: false, !readable);
			return texture2D;
		}

		public static Texture2D[] GetTextures(this Texture2DArray srcArr)
		{
			Texture2D[] array = new Texture2D[srcArr.depth];
			for (int i = 0; i < srcArr.depth; i++)
			{
				array[i] = srcArr.GetTexture(i);
			}
			return array;
		}

		public static Color GetPixel(this Texture2DArray srcArr, int x, int y, int ch)
		{
			return srcArr.GetTexture(ch).GetPixel(x, y);
		}

		public static void FillTexture(this Texture2DArray srcArr, Texture2D dst, int srcCh)
		{
			if (srcArr.depth <= srcCh)
			{
				throw new IndexOutOfRangeException("Trying to get channel (" + srcCh + ") >= depth (" + srcArr.depth + ")");
			}
			if (srcArr.format == dst.format && srcArr.width == dst.width && srcArr.height == dst.height)
			{
				Graphics.CopyTexture(srcArr, srcCh, dst, 0);
				dst.Apply(updateMipmaps: false);
				return;
			}
			Texture2D texture2D = srcArr.GetTexture(srcCh);
			if (texture2D.format.IsCompressed())
			{
				texture2D = texture2D.UncompressedClone();
			}
			if (texture2D.width != dst.width || texture2D.height != dst.height)
			{
				texture2D = texture2D.ResizedClone(dst.width, dst.height);
			}
			if (dst.format.IsCompressed())
			{
				texture2D.Compress(highQuality: true);
			}
			texture2D.Apply(updateMipmaps: false);
			Graphics.CopyTexture(texture2D, dst);
			dst.Apply(updateMipmaps: false);
		}

		public static void CopyTexture(Texture2DArray srcArr, int srcCh, Texture2DArray dstArr, int dstCh)
		{
			CopyTextures(srcArr, srcCh, dstArr, dstCh, 1);
		}

		public static void CopyTextures(Texture2DArray srcArr, Texture2DArray dstArr, int length)
		{
			CopyTextures(srcArr, 0, dstArr, 0, length);
		}

		public static void CopyTextures(Texture2DArray srcArr, int srcIndex, Texture2DArray dstArr, int dstIndex, int length)
		{
			if (srcArr.format == dstArr.format && srcArr.width == dstArr.width && srcArr.height == dstArr.height)
			{
				for (int i = 0; i < length; i++)
				{
					Graphics.CopyTexture(srcArr, srcIndex + i, dstArr, dstIndex + i);
				}
				dstArr.Apply(updateMipmaps: false);
				return;
			}
			Texture2D texture2D = new Texture2D(dstArr.width, dstArr.height, dstArr.format, mipChain: true, srcArr.IsLinear());
			for (int j = 0; j < length; j++)
			{
				srcArr.FillTexture(texture2D, srcIndex + j);
				Graphics.CopyTexture(texture2D, 0, dstArr, dstIndex + j);
			}
			dstArr.Apply(updateMipmaps: false);
		}

		public static void Add(ref Texture2DArray texArr, Texture2D tex)
		{
			Texture2DArray newArr = Add(texArr, tex);
			Rewrite(ref texArr, newArr);
		}

		public static Texture2DArray Add(Texture2DArray texArr, Texture2D tex)
		{
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, texArr.depth + 1, texArr.format, mipChain: true, texArr.IsLinear());
			texture2DArray.name = texArr.name;
			CopyTextures(texArr, texture2DArray, texArr.depth);
			texture2DArray.SetTexture(tex, texArr.depth, apply: false);
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static void Insert(ref Texture2DArray texArr, int pos, Texture2D tex)
		{
			Texture2DArray newArr = Insert(texArr, pos, tex);
			Rewrite(ref texArr, newArr);
		}

		public static Texture2DArray Insert(Texture2DArray texArr, int pos, Texture2D tex)
		{
			bool linear = texArr.IsLinear();
			if (texArr == null || texArr.depth == 0)
			{
				texArr = new Texture2DArray(tex.width, tex.height, 1, texArr.format, mipChain: true, linear);
				texArr.filterMode = FilterMode.Trilinear;
				texArr.SetTexture(tex, 0, apply: false);
				return texArr;
			}
			if (pos > texArr.depth || pos < 0)
			{
				pos = texArr.depth;
			}
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, texArr.depth + 1, texArr.format, mipChain: true, linear);
			texture2DArray.name = texArr.name;
			if (pos != 0)
			{
				CopyTextures(texArr, texture2DArray, pos);
			}
			if (pos != texArr.depth)
			{
				CopyTextures(texArr, pos, texture2DArray, pos + 1, texArr.depth - pos);
			}
			if (tex != null)
			{
				texture2DArray.SetTexture(tex, pos, apply: false);
			}
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static Texture2DArray InsertRange(Texture2DArray texArr, int pos, Texture2DArray addArr)
		{
			if (pos > texArr.depth || pos < 0)
			{
				pos = texArr.depth;
			}
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, texArr.depth + addArr.depth, texArr.format, mipChain: true, texArr.IsLinear());
			texture2DArray.name = texArr.name;
			if (pos != 0)
			{
				CopyTextures(texArr, texture2DArray, pos);
			}
			CopyTextures(addArr, 0, texture2DArray, pos, addArr.depth);
			if (pos != texArr.depth)
			{
				CopyTextures(texArr, pos, texture2DArray, pos + addArr.depth, texArr.depth - pos);
			}
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static void Switch(ref Texture2DArray texArr, int num1, int num2)
		{
			texArr.Switch(num1, num2);
			Rewrite(ref texArr, texArr);
		}

		public static void Switch(this Texture2DArray texArr, int num1, int num2)
		{
			if (num1 >= 0 && num1 < texArr.depth && num2 >= 0 && num2 < texArr.depth)
			{
				Texture2D texture = texArr.GetTexture(num1);
				CopyTexture(texArr, num2, texArr, num1);
				texArr.SetTexture(texture, num2);
			}
		}

		public static void Clear(this Texture2DArray texArr, int chNum)
		{
			Texture2D src = new Texture2D(texArr.width, texArr.height, texArr.format, mipChain: true, texArr.IsLinear());
			texArr.SetTexture(src, chNum);
		}

		public static void RemoveAt(ref Texture2DArray texArr, int num)
		{
			Texture2DArray newArr = RemoveAt(texArr, num);
			Rewrite(ref texArr, newArr);
		}

		public static Texture2DArray RemoveAt(Texture2DArray texArr, int num)
		{
			if (num >= texArr.depth || num < 0)
			{
				return texArr;
			}
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, texArr.depth - 1, texArr.format, mipChain: true, texArr.IsLinear());
			texture2DArray.name = texArr.name;
			if (num != 0)
			{
				CopyTextures(texArr, texture2DArray, num);
			}
			if (num != texArr.depth)
			{
				CopyTextures(texArr, num + 1, texture2DArray, num, texture2DArray.depth - num);
			}
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static void ChangeCount(ref Texture2DArray texArr, int newSize)
		{
			Texture2DArray newArr = ChangeCount(texArr, newSize);
			Rewrite(ref texArr, newArr);
		}

		public static Texture2DArray ChangeCount(Texture2DArray texArr, int newSize)
		{
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, newSize, texArr.format, mipChain: true, texArr.IsLinear());
			texture2DArray.name = texArr.name;
			int length = ((newSize < texArr.depth) ? newSize : texArr.depth);
			CopyTextures(texArr, texture2DArray, length);
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static Texture2DArray ResizedClone(this Texture2DArray texArr, int newWidth, int newHeight)
		{
			Texture2DArray texture2DArray = new Texture2DArray(newWidth, newHeight, texArr.depth, texArr.format, mipChain: true, texArr.IsLinear());
			texture2DArray.name = texArr.name;
			for (int i = 0; i < texArr.depth; i++)
			{
				CopyTexture(texArr, i, texture2DArray, i);
			}
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static Texture2DArray FormattedClone(this Texture2DArray texArr, TextureFormat format)
		{
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, texArr.depth, format, mipChain: true, texArr.IsLinear());
			texture2DArray.name = texArr.name;
			int depth = texArr.depth;
			for (int i = 0; i < depth; i++)
			{
				CopyTexture(texArr, i, texture2DArray, i);
			}
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static Texture2DArray LinearClone(this Texture2DArray texArr, bool linear)
		{
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, texArr.depth, texArr.format, mipChain: true, linear);
			texture2DArray.name = texArr.name;
			int depth = texArr.depth;
			for (int i = 0; i < depth; i++)
			{
				CopyTexture(texArr, i, texture2DArray, i);
			}
			texture2DArray.Apply(updateMipmaps: false);
			return texture2DArray;
		}

		public static Texture2DArray WritableClone(this Texture2DArray texArr)
		{
			Texture2DArray texture2DArray = new Texture2DArray(texArr.width, texArr.height, texArr.depth, texArr.format, mipChain: true, texArr.IsLinear());
			for (int i = 0; i < texArr.depth; i++)
			{
				CopyTexture(texArr, i, texture2DArray, i);
			}
			texture2DArray.Apply(updateMipmaps: true);
			return texture2DArray;
		}

		public static int GetMipMapCount(this Texture2DArray texArr)
		{
			for (int i = 0; i < 100; i++)
			{
				try
				{
					texArr.GetPixels(0, i);
				}
				catch
				{
					return i;
				}
			}
			return -1;
		}

		public static bool IsReadWrite(this Texture2DArray texArr)
		{
			try
			{
				texArr.SetPixels(null, 0);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static void Rewrite(ref Texture2DArray texArr, Texture2DArray newArr)
		{
		}
	}
}
