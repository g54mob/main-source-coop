using System;
using System.Runtime.InteropServices;
using Den.Tools.Tasks;
using UnityEngine;

namespace Den.Tools.Matrices
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Matrix
	{
		[StructLayout(LayoutKind.Explicit)]
		public class FloatToBytes
		{
			[FieldOffset(0)]
			public float f;

			[FieldOffset(0)]
			public byte b0;

			[FieldOffset(1)]
			public byte b1;

			[FieldOffset(2)]
			public byte b2;

			[FieldOffset(3)]
			public byte b3;
		}

		public CoordRect rect;

		public int count;

		public float[] arr;

		public const bool native = true;

		public static Action<Matrix, string> onPreview;

		public float this[int x, int z]
		{
			get
			{
				return arr[(z - rect.offset.z) * rect.size.x + x - rect.offset.x];
			}
			set
			{
				arr[(z - rect.offset.z) * rect.size.x + x - rect.offset.x] = value;
			}
		}

		public float this[Coord c]
		{
			get
			{
				return arr[(c.z - rect.offset.z) * rect.size.x + c.x - rect.offset.x];
			}
			set
			{
				arr[(c.z - rect.offset.z) * rect.size.x + c.x - rect.offset.x] = value;
			}
		}

		public Matrix()
		{
			arr = new float[0];
			rect = new CoordRect(0, 0, 0, 0);
			count = 0;
		}

		public Matrix(int offsetX, int offsetZ, int sizeX, int sizeZ, float[] array = null)
		{
			rect = new CoordRect(offsetX, offsetZ, sizeX, sizeZ);
			count = rect.Count;
			DefineArray(array);
		}

		public Matrix(CoordRect rect, float[] array = null)
		{
			this.rect = rect;
			count = rect.Count;
			DefineArray(array);
		}

		public Matrix(Coord offset, Coord size, float[] array = null)
		{
			rect = new CoordRect(offset, size);
			count = rect.Count;
			DefineArray(array);
		}

		public Matrix(Matrix src, float[] array = null)
		{
			rect = src.rect;
			count = rect.Count;
			DefineArray(array);
			Array.Copy(src.arr, arr, arr.Length);
		}

		public Matrix(Texture2D texture, int channel = -1)
		{
			rect = new CoordRect(0, 0, texture.width, texture.height);
			count = rect.Count;
			arr = new float[count];
			Color[] pixels = texture.GetPixels();
			ImportColors(pixels, texture.width, texture.height, channel);
		}

		protected void DefineArray(float[] array = null)
		{
			int num = rect.size.x * rect.size.z;
			if (array != null)
			{
				if (array.Length < num)
				{
					throw new Exception("Array length: " + array.Length + " is lower then matrix capacity: " + num);
				}
				arr = array;
			}
			else
			{
				arr = new float[num];
			}
		}

		public float GetFloored(float fx, float fz)
		{
			int num = (int)fx;
			if (fx < 0f)
			{
				num--;
			}
			int num2 = (int)fz;
			if (fz < 0f)
			{
				num2--;
			}
			return arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x];
		}

		public float GetInterpolated(float fx, float fz, bool roundToShort = false)
		{
			int num = (int)fx;
			if (fx < 0f)
			{
				num--;
			}
			if (num == rect.offset.x + rect.size.x)
			{
				num--;
			}
			int num2 = (int)fz;
			if (fz < 0f)
			{
				num2--;
			}
			if (num2 == rect.offset.z + rect.size.z)
			{
				num2--;
			}
			float num3 = fx - (float)num;
			float num4 = fz - (float)num2;
			if (num < rect.offset.x)
			{
				num = rect.offset.x;
			}
			if (num2 < rect.offset.z)
			{
				num2 = rect.offset.z;
			}
			int num5 = num + 1;
			if (num5 >= rect.offset.x + rect.size.x)
			{
				num5 = rect.offset.x + rect.size.x - 1;
			}
			int num6 = num2 + 1;
			if (num6 >= rect.offset.z + rect.size.z)
			{
				num6 = rect.offset.z + rect.size.z - 1;
			}
			float num7 = arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x];
			float num8 = arr[(num2 - rect.offset.z) * rect.size.x + num5 - rect.offset.x];
			float num9 = num7 * (1f - num3) + num8 * num3;
			float num10 = arr[(num6 - rect.offset.z) * rect.size.x + num - rect.offset.x];
			float num11 = arr[(num6 - rect.offset.z) * rect.size.x + num5 - rect.offset.x];
			float num12 = num10 * (1f - num3) + num11 * num3;
			return num9 * (1f - num4) + num12 * num4;
		}

		public float GetRelative(float rx, float rz)
		{
			float fx = rx * (float)rect.size.x + (float)rect.offset.x;
			float fz = rz * (float)rect.size.z + (float)rect.offset.z;
			return GetInterpolated(fx, fz);
		}

		public float GetRelative(float rx, float rz, CoordRect customRect)
		{
			float fx = rx * (float)customRect.size.x + (float)customRect.offset.x;
			float fz = rz * (float)customRect.size.z + (float)customRect.offset.z;
			return GetInterpolated(fx, fz);
		}

		public float GetRelativeWithRotation(float sx, float sz, Vector2D rotationDirection, Vector2D rotationPivot, CoordRect customRect)
		{
			float num = sx - rotationPivot.x;
			float num2 = sz - rotationPivot.z;
			Vector2D vector2D = rotationDirection * num;
			Vector2D vector2D2 = new Vector2D(0f - rotationDirection.z, rotationDirection.x) * num2;
			Vector2D vector2D3 = vector2D + vector2D2;
			vector2D3.x += rotationPivot.x;
			vector2D3.z += rotationPivot.z;
			if (vector2D3.x < 0f || vector2D3.x > 1f)
			{
				return 0f;
			}
			if (vector2D3.z < 0f || vector2D3.z > 1f)
			{
				return 0f;
			}
			float fx = vector2D3.x * (float)customRect.size.x + (float)customRect.offset.x;
			float fz = vector2D3.z * (float)customRect.size.z + (float)customRect.offset.z;
			return GetInterpolated(fx, fz);
		}

		public void ImportTexture(Texture2D tex, int channel = -1)
		{
			ImportTexture(tex, rect.offset, channel);
		}

		public void ImportTexture(Texture2D tex, Coord texOffset, int channel = -1)
		{
			Coord size = new Coord(tex.width, tex.height);
			CoordRect coordRect = CoordRect.Intersected(rect, new CoordRect(texOffset, size));
			Color[] pixels = tex.GetPixels(coordRect.offset.x - texOffset.x, coordRect.offset.z - texOffset.z, coordRect.size.x, coordRect.size.z);
			ImportColors(pixels, coordRect.offset, coordRect.size, channel);
		}

		public void ImportTextureRaw(Texture2D tex, int channel = 0)
		{
			ImportTextureRaw(tex, rect.offset, channel);
		}

		public void ImportTextureRaw(Texture2D tex, Coord texOffset, int channel = 0)
		{
			Coord coord = new Coord(tex.width, tex.height);
			TextureFormat format = tex.format;
			if (format != TextureFormat.RGBA32 && format != TextureFormat.ARGB32 && format != TextureFormat.RGB24 && format != TextureFormat.R8 && format != TextureFormat.R16)
			{
				throw new Exception("Matrix export: raw texture format is not supported");
			}
			byte[] rawTextureData = tex.GetRawTextureData();
			switch (format)
			{
			case TextureFormat.RGBA32:
				ImportRawBytes(rawTextureData, texOffset, coord, channel, 4);
				break;
			case TextureFormat.ARGB32:
				channel++;
				if (channel == 5)
				{
					channel = 0;
				}
				ImportRawBytes(rawTextureData, texOffset, coord, channel, 4);
				break;
			case TextureFormat.RGB24:
				ImportRawBytes(rawTextureData, texOffset, coord, channel, 3);
				break;
			case TextureFormat.R8:
				ImportRawBytes(rawTextureData, texOffset, coord, 0, 1);
				break;
			case TextureFormat.R16:
				ImportRaw16(rawTextureData, texOffset, coord);
				break;
			}
		}

		public void ImportColors(Color[] colors, int width, int height, int channel = -1)
		{
			ImportColors(colors, rect.offset, new Coord(width, height), channel);
		}

		public void ImportColors(Color[] colors, Coord colorsSize, int channel = -1)
		{
			ImportColors(colors, rect.offset, colorsSize, channel);
		}

		public void ImportColors(Color[] colors, Coord colorsOffset, Coord colorsSize, int channel = -1)
		{
			CoordRect coordRect = CoordRect.Intersected(rect, new CoordRect(colorsOffset, colorsSize));
			Coord min = coordRect.Min;
			Coord max = coordRect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - rect.offset.z) * rect.size.x + i - rect.offset.x;
					int num2 = (j - colorsOffset.z) * colorsSize.x + i - colorsOffset.x;
					float num3 = channel switch
					{
						0 => colors[num2].r, 
						1 => colors[num2].g, 
						2 => colors[num2].b, 
						3 => colors[num2].a, 
						_ => (colors[num2].r + colors[num2].g + colors[num2].b) / 3f, 
					};
					arr[num] = num3;
				}
			}
		}

		public void ImportRawBytes(byte[] bytes, int width, int height, int start, int step)
		{
			ImportRawBytes(bytes, rect.offset, new Coord(width, height), start, step);
		}

		public void ImportRawBytes(byte[] bytes, Coord bytesSize, int start, int step)
		{
			ImportRawBytes(bytes, rect.offset, bytesSize, start, step);
		}

		public void ImportRawBytes(byte[] bytes, Coord bytesOffset, Coord bytesSize, int start, int step)
		{
			if (bytes.Length != bytesSize.x * bytesSize.z * step && ((float)bytes.Length < (float)(bytesSize.x * bytesSize.z * step) * 1.3f || (float)bytes.Length > (float)(bytesSize.x * bytesSize.z * step) * 1.3666f))
			{
				throw new Exception("Array count does not match texture dimensions");
			}
			ImportRawBytes(this, bytes, bytes.Length, bytesOffset, bytesSize, start, step);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ImportRawBytes")]
		public static extern void ImportRawBytes(Matrix thism, byte[] bytes, int bytesLength, Coord bytesOffset, Coord bytesSize, int start, int step);

		public void ImportRaw16(byte[] bytes, int width, int height)
		{
			ImportRaw16(bytes, rect.offset, new Coord(width, height));
		}

		public void ImportRaw16(byte[] bytes, Coord texSize)
		{
			ImportRaw16(bytes, rect.offset, texSize);
		}

		public void ImportRaw16(byte[] bytes, Coord texOffset, Coord texSize)
		{
			if (texSize.x * texSize.z * 2 > bytes.Length)
			{
				throw new Exception("Array count does not match texture dimensions");
			}
			ImportRaw16(this, bytes, bytes.Length, texOffset, texSize);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ImportRaw16")]
		private static extern void ImportRaw16(Matrix thism, byte[] bytes, int bytesLength, Coord texOffset, Coord texSize);

		public void ImportRawFloat(byte[] bytes, int width, int height, float mult = 1f)
		{
			ImportRawFloat(bytes, new Coord(width, height), rect.offset, mult);
		}

		public void ImportRawFloat(byte[] bytes, Coord texSize, float mult = 1f)
		{
			ImportRawFloat(bytes, texSize, rect.offset, mult);
		}

		public void ImportRawFloat(byte[] bytes, Coord texOffset, Coord texSize, float mult = 1f)
		{
			if (texSize.x * texSize.z * 4 > bytes.Length)
			{
				throw new Exception("Array count does not match texture dimensions");
			}
			ImportRawFloat(this, bytes, bytes.Length, texOffset, texSize, mult);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ImportRawFloat")]
		public static extern void ImportRawFloat(Matrix thism, byte[] bytes, int bytesLength, Coord texOffset, Coord texSize, float mult = 1f);

		public void ImportHeights(float[,] heights)
		{
			ImportHeights(heightsSize: new Coord(heights.GetLength(1), heights.GetLength(0)), thism: this, heights: heights, heightsOffset: rect.offset);
		}

		public void ImportHeights(float[,] heights, Coord heightsOffset)
		{
			Coord heightsSize = new Coord(heights.GetLength(1), heights.GetLength(0));
			ImportHeights(this, heights, heightsOffset, heightsSize);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ImportHeights")]
		private static extern void ImportHeights(Matrix thism, float[,] heights, Coord heightsOffset, Coord heightsSize);

		public void ImportHeightStrips(float[][,] heights)
		{
			ImportHeightStrips(heights, rect.offset);
		}

		public void ImportHeightStrips(float[][,] heights, Coord heightsOffset)
		{
			int num = 0;
			for (int i = 0; i < heights.Length; i++)
			{
				ImportHeights(heights[i], new Coord(0, num));
				num += heights[i].GetLength(0);
			}
		}

		public void ImportSplats(float[,,] splats, int channel)
		{
			ImportSplats(splatsSize: new Coord(splats.GetLength(1), splats.GetLength(0)), thism: this, splats: splats, splatsOffset: rect.offset, numChannels: splats.GetLength(2), channel: channel);
		}

		public void ImportSplats(float[,,] splats, Coord heightsOffset, int channel)
		{
			Coord splatsSize = new Coord(splats.GetLength(1), splats.GetLength(0));
			ImportSplats(this, splats, heightsOffset, splatsSize, splats.GetLength(2), channel);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ImportSplats")]
		private static extern void ImportSplats(Matrix thism, float[,,] splats, Coord splatsOffset, Coord splatsSize, int numChannels, int channel);

		public void ImportDetail(int[,] detail, float density = 1f)
		{
			ImportDetail(detailSize: new Coord(detail.GetLength(1), detail.GetLength(0)), thism: this, detail: detail, detailOffset: rect.offset, density: density);
		}

		public void ImportDetail(int[,] detail, Coord detailOffset, float density = 1f)
		{
			Coord detailSize = new Coord(detail.GetLength(1), detail.GetLength(0));
			ImportDetail(this, detail, detailOffset, detailSize, density);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ImportDetail")]
		private static extern void ImportDetail(Matrix thism, int[,] detail, Coord detailOffset, Coord detailSize, float density = 1f);

		public void ImportData(TerrainData data, int channel = -1)
		{
			ImportData(data, rect.offset, channel = -1);
		}

		public void ImportData(TerrainData data, Coord dataOffset, int channel = -1)
		{
			int num = ((channel == -1) ? data.heightmapResolution : data.alphamapResolution);
			Coord size = new Coord(num, num);
			CoordRect coordRect = CoordRect.Intersected(rect, new CoordRect(dataOffset, size));
			if (coordRect.size.x != 0 && coordRect.size.z != 0)
			{
				if (channel == -1)
				{
					float[,] heights = data.GetHeights(coordRect.offset.x - dataOffset.x, coordRect.offset.z - dataOffset.z, coordRect.size.x, coordRect.size.z);
					ImportHeights(heights, coordRect.offset);
				}
				else
				{
					float[,,] alphamaps = data.GetAlphamaps(coordRect.offset.x - dataOffset.x, coordRect.offset.z - dataOffset.z, coordRect.size.x, coordRect.size.z);
					ImportSplats(alphamaps, coordRect.offset, channel);
				}
			}
		}

		public void ExportTexture(Texture2D tex, int channel = -1)
		{
			ExportTexture(tex, rect.offset, channel);
		}

		public void ExportTexture(Texture2D tex, Coord texOffset, int channel = -1)
		{
			Coord size = new Coord(tex.width, tex.height);
			CoordRect coordRect = CoordRect.Intersected(rect, new CoordRect(texOffset, size));
			Color[] colors = ((channel >= 0) ? tex.GetPixels(coordRect.offset.x - texOffset.x, coordRect.offset.z - texOffset.z, coordRect.size.x, coordRect.size.z) : new Color[coordRect.size.x * coordRect.size.z]);
			ExportColors(colors, coordRect.offset, coordRect.size, channel);
			tex.SetPixels(coordRect.offset.x - texOffset.x, coordRect.offset.z - texOffset.z, coordRect.size.x, coordRect.size.z, colors);
			tex.Apply();
		}

		public void ExportTextureRaw(Texture2D tex)
		{
			if (tex.width != rect.size.x || tex.height != rect.size.z)
			{
				throw new Exception("Matrix export: matrix size and texture resolution mismatch (tex:" + tex.width + "*" + tex.height + " matrix:" + rect.size.x + "*" + rect.size.z + ")");
			}
			byte[] array;
			switch (tex.format)
			{
			case TextureFormat.RGBA32:
				array = new byte[rect.Count * 4];
				ExportRawBytes(array, rect.offset, rect.size, 0, 4);
				ExportRawBytes(array, rect.offset, rect.size, 1, 4);
				ExportRawBytes(array, rect.offset, rect.size, 2, 4);
				break;
			case TextureFormat.ARGB32:
				array = new byte[rect.Count * 4];
				ExportRawBytes(array, rect.offset, rect.size, 1, 4);
				ExportRawBytes(array, rect.offset, rect.size, 2, 4);
				ExportRawBytes(array, rect.offset, rect.size, 3, 4);
				break;
			case TextureFormat.RGB24:
				array = new byte[rect.Count * 3];
				ExportRawBytes(array, rect.offset, rect.size, 0, 3);
				ExportRawBytes(array, rect.offset, rect.size, 1, 3);
				ExportRawBytes(array, rect.offset, rect.size, 2, 3);
				break;
			case TextureFormat.R8:
				array = new byte[rect.Count];
				ExportRawBytes(array, rect.offset, rect.size, 0, 1);
				break;
			case TextureFormat.R16:
				array = new byte[rect.Count * 2];
				ExportRaw16(array, rect.offset, rect.size);
				break;
			case TextureFormat.RFloat:
				array = new byte[rect.Count * 4];
				ExportRawFloat(array, rect.offset, rect.size);
				break;
			default:
				throw new Exception("Matrix export: raw texture format is not supported (" + tex.format.ToString() + ")");
			}
			tex.LoadRawTextureData(array);
			tex.Apply();
		}

		public void ExportTextureRaw(Texture2D tex, Coord texOffset, int channel = -1)
		{
			TextureFormat format = tex.format;
			if (format != TextureFormat.RGBA32 && format != TextureFormat.ARGB32 && format != TextureFormat.RGB24 && format != TextureFormat.R8 && format != TextureFormat.R16 && format != TextureFormat.RFloat)
			{
				throw new Exception("Matrix export: raw texture format is not supported");
			}
			Coord coord = new Coord(tex.width, tex.height);
			byte[] rawTextureData = tex.GetRawTextureData();
			switch (format)
			{
			case TextureFormat.RGBA32:
				ExportRawBytes(rawTextureData, texOffset, coord, channel, 4);
				break;
			case TextureFormat.ARGB32:
				channel++;
				if (channel == 5)
				{
					channel = 0;
				}
				ExportRawBytes(rawTextureData, texOffset, coord, channel, 4);
				break;
			case TextureFormat.RGB24:
				ExportRawBytes(rawTextureData, texOffset, coord, channel, 3);
				break;
			case TextureFormat.R8:
				ExportRawBytes(rawTextureData, texOffset, coord, 0, 1);
				break;
			case TextureFormat.R16:
				ExportRaw16(rawTextureData, texOffset, coord);
				break;
			case TextureFormat.RFloat:
				ExportRawFloat(rawTextureData, texOffset, coord);
				break;
			}
			tex.LoadRawTextureData(rawTextureData);
			tex.Apply();
		}

		public void ExportColors(Color[] colors, int width, int height, int channel = -1, bool markOutrange = true, Matrix mask = null)
		{
			ExportColors(colors, rect.offset, new Coord(width, height), channel, markOutrange, mask);
		}

		public void ExportColors(Color[] colors, Coord colorsSize, int channel = -1, bool markOutrange = true, Matrix mask = null)
		{
			ExportColors(colors, rect.offset, colorsSize, channel, markOutrange, mask);
		}

		public void ExportColors(Color[] colors, Coord colorsOffset, Coord colorsSize, int channel = -1, bool markOutrange = true, Matrix mask = null)
		{
			if (colors.Length != colorsSize.x * colorsSize.z)
			{
				throw new Exception("Array count does not match texture dimensions");
			}
			CoordRect coordRect = CoordRect.Intersected(rect, new CoordRect(colorsOffset, colorsSize));
			Coord min = coordRect.Min;
			Coord max = coordRect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - rect.offset.z) * rect.size.x + i - rect.offset.x;
					int num2 = (j - colorsOffset.z) * colorsSize.x + i - colorsOffset.x;
					float num3 = arr[num];
					if (mask != null)
					{
						num3 *= mask.arr[num];
					}
					if (num3 > 1f && markOutrange)
					{
						colors[num2] = new Color(0f, 1f, 0f, 0f);
						continue;
					}
					if (num3 < 0f && markOutrange)
					{
						colors[num2] = new Color(1f, 0f, 0f, 0f);
						continue;
					}
					switch (channel)
					{
					case 0:
						colors[num2].r = num3;
						continue;
					case 1:
						colors[num2].g = num3;
						continue;
					case 2:
						colors[num2].b = num3;
						continue;
					case 3:
						colors[num2].a = num3;
						continue;
					}
					colors[num2].r = num3;
					colors[num2].g = num3;
					colors[num2].b = num3;
					colors[num2].a = num3;
				}
			}
		}

		public void ExportRawBytes(byte[] bytes, int width, int height, int start, int step)
		{
			ExportRawBytes(bytes, new Coord(width, height), rect.offset, start, step);
		}

		public void ExportRawBytes(byte[] bytes, Coord bytesSize, int start, int step)
		{
			ExportRawBytes(bytes, bytesSize, rect.offset, start, step);
		}

		public void ExportRawBytes(byte[] bytes, Coord bytesOffset, Coord bytesSize, int start, int step)
		{
			if (bytes.Length != bytesSize.x * bytesSize.z * step && ((float)bytes.Length < (float)(bytesSize.x * bytesSize.z * step) * 1.3f || (float)bytes.Length > (float)(bytesSize.x * bytesSize.z * step) * 1.3666f))
			{
				throw new Exception("Array count does not match texture dimensions");
			}
			ExportRawBytes(this, bytes, bytes.Length, bytesOffset, bytesSize, start, step);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ExportRawBytes")]
		public static extern void ExportRawBytes(Matrix thism, byte[] bytes, int bytesLength, Coord bytesOffset, Coord bytesSize, int start, int step);

		public void ExportRaw16(byte[] bytes, int width, int height)
		{
			ExportRaw16(bytes, new Coord(width, height), rect.offset);
		}

		public void ExportRaw16(byte[] bytes, Coord texSize)
		{
			ExportRaw16(bytes, texSize, rect.offset);
		}

		public void ExportRaw16(byte[] bytes, Coord texOffset, Coord texSize)
		{
			if (texSize.x * texSize.z * 2 != bytes.Length)
			{
				throw new Exception("Array count does not match texture dimensions");
			}
			ExportRaw16(this, bytes, bytes.Length, texOffset, texSize);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ExportRaw16")]
		private static extern void ExportRaw16(Matrix thism, byte[] bytes, int bytesLength, Coord texOffset, Coord texSize);

		public void ExportRawFloat(byte[] bytes, int width, int height, float mult = 1f)
		{
			ExportRawFloat(bytes, new Coord(width, height), rect.offset, mult);
		}

		public void ExportRawFloat(byte[] bytes, Coord texSize, float mult = 1f)
		{
			ExportRawFloat(bytes, texSize, rect.offset, mult);
		}

		public void ExportRawFloat(byte[] bytes, Coord texOffset, Coord texSize, float mult = 1f)
		{
			if (texSize.x * texSize.z * 4 != bytes.Length)
			{
				throw new Exception("Array count does not match texture dimensions");
			}
			ExportRawFloat(this, bytes, bytes.Length, texOffset, texSize, mult);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ExportRawFloat")]
		private static extern int ExportRawFloat(Matrix thism, byte[] bytes, int bytesLength, Coord texOffset, Coord texSize, float mult = 1f);

		public void ExportHeights(float[,] heights)
		{
			ExportHeights(heightsSize: new Coord(heights.GetLength(1), heights.GetLength(0)), thism: this, heights: heights, heightsOffset: rect.offset);
		}

		public void ExportHeights(float[,] heights, Coord heightsOffset)
		{
			Coord heightsSize = new Coord(heights.GetLength(1), heights.GetLength(0));
			ExportHeights(this, heights, heightsOffset, heightsSize);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ExportHeights")]
		private static extern void ExportHeights(Matrix thism, float[,] heights, Coord heightsOffset, Coord heightsSize);

		public void ExportHeightStrips(float[][,] heights)
		{
			ExportHeightStrips(heights, rect.offset);
		}

		public void ExportHeightStrips(float[][,] heights, Coord heightsOffset)
		{
			int num = 0;
			for (int i = 0; i < heights.Length; i++)
			{
				ExportHeights(heights[i], new Coord(0, num));
				num += heights[i].GetLength(0);
			}
		}

		public void ExportSplats(float[,,] splats, int channel)
		{
			ExportSplats(splatsSize: new Coord(splats.GetLength(1), splats.GetLength(0)), thism: this, splats: splats, splatsOffset: rect.offset, numChannels: splats.GetLength(2), channel: channel);
		}

		public void ExportSplats(float[,,] splats, Coord heightsOffset, int channel)
		{
			Coord splatsSize = new Coord(splats.GetLength(1), splats.GetLength(0));
			ExportSplats(this, splats, heightsOffset, splatsSize, splats.GetLength(2), channel);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Matrix_ExportSplats")]
		private static extern void ExportSplats(Matrix thism, float[,,] splats, Coord splatsOffset, Coord splatsSize, int numChannels, int channel);

		public void ExportDetail(int[,] detail, int channel, Noise random, float density = 1f)
		{
			ExportDetail(detail, rect.offset, channel, random, density);
		}

		public void ExportDetail(int[,] detail, Coord detailOffset, int channel, Noise random, float density = 1f)
		{
			Coord size = new Coord(detail.GetLength(1), detail.GetLength(0));
			CoordRect c = new CoordRect(detailOffset, size);
			CoordRect coordRect = CoordRect.Intersected(rect, c);
			Coord min = coordRect.Min;
			Coord max = coordRect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - rect.offset.z) * rect.size.x + i - rect.offset.x;
					int num2 = i - c.offset.x;
					int num3 = j - c.offset.z;
					float num4 = arr[num];
					if (i < max.x - 1)
					{
						_ = arr[num + 1];
					}
					if (j < max.z - 1)
					{
						_ = arr[num + rect.size.x];
					}
					if (i < max.x - 1 && j < max.z - 1)
					{
						_ = arr[num + rect.size.x + 1];
					}
					num4 *= density;
					float num5 = random.Random(channel, i, j);
					int num6 = (int)num4;
					if (num4 - (float)num6 > num5)
					{
						num6++;
					}
					num6 = ((num4 > 0.001f) ? 1 : 0);
					detail[num3, num2] = (int)(num4 + 0.5f);
				}
			}
		}

		public void ExportTerrainData(TerrainData data)
		{
			ExportTerrainData(data, rect.offset, -1);
		}

		public void ExportTerrainData(TerrainData data, int channel)
		{
			ExportTerrainData(data, rect.offset, channel);
		}

		public void ExportTerrainData(TerrainData data, Coord dataOffset, int channel)
		{
			int num = ((channel == -1) ? data.heightmapResolution : data.alphamapResolution);
			Coord size = new Coord(num, num);
			CoordRect coordRect = CoordRect.Intersected(rect, new CoordRect(dataOffset, size));
			if (coordRect.size.x != 0 && coordRect.size.z != 0)
			{
				if (channel == -1)
				{
					float[,] heights = new float[coordRect.size.z, coordRect.size.x];
					ExportHeights(heights, coordRect.offset);
					data.SetHeights(coordRect.offset.x - dataOffset.x, coordRect.offset.z - dataOffset.z, heights);
				}
				else
				{
					float[,,] alphamaps = data.GetAlphamaps(coordRect.offset.x - dataOffset.x, coordRect.offset.z - dataOffset.z, coordRect.size.x, coordRect.size.z);
					ExportSplats(alphamaps, coordRect.offset, channel);
					data.SetAlphamaps(coordRect.offset.x - dataOffset.x, coordRect.offset.z - dataOffset.z, alphamaps);
				}
			}
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixFillVal")]
		private static extern void Fill(Matrix thisMatrix, float val);

		public void Fill(float val)
		{
			Fill(this, val);
		}

		public void Fill(Matrix m)
		{
			Fill(this, m);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixFill")]
		private static extern void Fill(Matrix thisMatrix, Matrix m);

		public void Fill(float val, float opacity)
		{
			Fill(this, val, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixFillOpacity")]
		private static extern void Fill(Matrix thisMatrix, float val, float opacity);

		public void Mix(Matrix m, float opacity = 1f)
		{
			MixEx(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMix")]
		private static extern void MixEx(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Mix(Matrix m, Matrix mask)
		{
			MixEx(this, m, mask);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMixMask")]
		private static extern void MixEx(Matrix thisMatrix, Matrix m, Matrix mask);

		public void Mix(Matrix m, Matrix mask, float opacity)
		{
			MixEx(this, m, mask, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMixMaskOpacity")]
		private static extern void MixEx(Matrix thisMatrix, Matrix m, Matrix mask, float opacity);

		public void Mix(Matrix m, Matrix mask, float maskMin, float maskMax, bool maskInvert, bool fallof, float opacity)
		{
			Mix(this, m, mask, maskMin, maskMax, maskInvert, fallof, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMixComplex")]
		private static extern void Mix(Matrix thisMatrix, Matrix m, Matrix mask, float maskMin, float maskMax, bool maskInvert, bool fallof, float opacity);

		public void InvMix(Matrix m, Matrix invMask, float opacity = 1f)
		{
			InvMix(this, m, invMask, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixInvMix")]
		private static extern void InvMix(Matrix thisMatrix, Matrix m, Matrix invMask, float opacity = 1f);

		public void Add(Matrix add, float opacity = 1f)
		{
			AddEx(this, add, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixAdd")]
		private static extern void AddEx(Matrix thisMatrix, Matrix add, float opacity = 1f);

		public void Add(Matrix add, Matrix mask, float opcaity = 1f)
		{
			AddEx(this, add, mask, opcaity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixAddMask")]
		private static extern void AddEx(Matrix thisMatrix, Matrix add, Matrix mask, float opcaity = 1f);

		public void Add(float add)
		{
			AddEx(this, add);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixAddVal")]
		private static extern void AddEx(Matrix thisMatrix, float add);

		public void Blend(Matrix matrix, Matrix mask, Matrix add, Matrix addMask)
		{
			Blend(this, matrix, mask, add, addMask);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixBlend")]
		private static extern void Blend(Matrix thisMatrix, Matrix matrix, Matrix mask, Matrix add, Matrix addMask);

		public void Max(Matrix matrix, Matrix mask, Matrix add, Matrix addMask)
		{
			Max(this, matrix, mask, add, addMask);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMaxComplex")]
		private static extern void Max(Matrix thisMatrix, Matrix matrix, Matrix mask, Matrix add, Matrix addMask);

		public void Step(float mid = 0.5f)
		{
			Step(this, mid);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixStep")]
		private static extern void Step(Matrix thisMatrix, float mid = 0.5f);

		public void Subtract(Matrix m, float opacity = 1f)
		{
			Subtract(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixSubtract")]
		private static extern void Subtract(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void InvSubtract(Matrix m, float opacity = 1f)
		{
			InvSubtract(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixInvSubtract")]
		private static extern void InvSubtract(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Multiply(Matrix m, float opacity = 1f)
		{
			MultiplyEx(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMultiply")]
		private static extern void MultiplyEx(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Multiply(float m)
		{
			MultiplyEx(this, m);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMultiplyVal")]
		private static extern void MultiplyEx(Matrix thisMatrix, float m);

		public void MultiplyInv(Matrix invFactor)
		{
			MultiplyInvEx(this, invFactor);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMultiplyInv")]
		private static extern void MultiplyInvEx(Matrix thisMatrix, Matrix invFactor);

		public void Contrast(float m)
		{
			Contrast(this, m);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixContrast")]
		private static extern void Contrast(Matrix thisMatrix, float m);

		public void Divide(Matrix m, float opacity = 1f)
		{
			Divide(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixDivide")]
		private static extern void Divide(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Difference(Matrix m, float opacity = 1f)
		{
			Difference(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixDifference")]
		private static extern void Difference(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Overlay(Matrix m, float opacity = 1f)
		{
			Overlay(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOverlay")]
		private static extern void Overlay(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void HardLight(Matrix m, float opacity = 1f)
		{
			HardLight(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixHardLight")]
		private static extern void HardLight(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void SoftLight(Matrix m, float opacity = 1f)
		{
			SoftLight(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixSoftLight")]
		private static extern void SoftLight(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Max(Matrix m, float opacity = 1f)
		{
			Max(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMax")]
		private static extern void Max(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Min(Matrix m, float opacity = 1f)
		{
			Min(this, m, opacity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMin")]
		private static extern void Min(Matrix thisMatrix, Matrix m, float opacity = 1f);

		public void Select(float level)
		{
			Select(this, level);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixSelect")]
		private static extern void Select(Matrix thisMatrix, float level);

		public void Invert()
		{
			Invert(this);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixInvert")]
		private static extern void Invert(Matrix thisMatrix);

		public void InvertOne()
		{
			InvertOne(this);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixInvertOne")]
		private static extern void InvertOne(Matrix thisMatrix);

		public void SelectRange(float minFrom, float minTo, float maxFrom, float maxTo)
		{
			SelectRange(this, minFrom, minTo, maxFrom, maxTo);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixSelectRange")]
		private static extern void SelectRange(Matrix thisMatrix, float minFrom, float minTo, float maxFrom, float maxTo);

		public void ChangeRange(float fromMin, float fromMax, float toMin, float toMax)
		{
			ChangeRange(this, fromMin, fromMax, toMin, toMax);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixChangeRange")]
		private static extern void ChangeRange(Matrix thisMatrix, float fromMin, float fromMax, float toMin, float toMax);

		public void Clamp01()
		{
			Clamp01(this);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixClamp01")]
		private static extern void Clamp01(Matrix thisMatrix);

		public float MaxValue()
		{
			return MaxValue(this);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMaxValue")]
		private static extern float MaxValue(Matrix thisMatrix);

		public float MinValue()
		{
			return MinValue(this);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixMinValue")]
		private static extern float MinValue(Matrix thisMatrix);

		public float Average()
		{
			return Average(this);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl)]
		private static extern float Average(Matrix thisMatrix);

		public virtual bool IsEmpty()
		{
			return IsEmpty(this);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixIsEmpty")]
		private static extern bool IsEmpty(Matrix thisMatrix);

		public virtual bool IsEmpty(float delta)
		{
			return IsEmpty(this, delta);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixIsEmptyDelta")]
		private static extern bool IsEmpty(Matrix thisMatrix, float delta);

		public void BlackWhite(float mid)
		{
			BlackWhite(this, mid);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixBlackWhite")]
		private static extern void BlackWhite(Matrix thisMatrix, float mid);

		public void BrighnesContrast(float brightness, float contrast)
		{
			BrighnesContrast(this, brightness, contrast);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixBrightnessContrast")]
		private static extern void BrighnesContrast(Matrix thisMatrix, float brightness, float contrast);

		public void Terrace(float[] terraces, float steepness)
		{
			Terrace(this, terraces, terraces.Length, steepness);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixTerrace")]
		private static extern void Terrace(Matrix thisMatrix, float[] terraces, int terraceCount, float steepness);

		public void Levels(float inMin, float inMax, float gamma, float outMin, float outMax)
		{
			Levels(this, inMin, inMax, gamma, outMin, outMax);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixLevels")]
		private static extern void Levels(Matrix thisMatrix, float inMin, float inMax, float gamma, float outMin, float outMax);

		public void UniformCurve(float[] lut)
		{
			UniformCurve(this, lut, lut.Length);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixUniformCurve")]
		private static extern void UniformCurve(Matrix thisMatrix, float[] lut, int lutCount);

		public void Pow(float powVal)
		{
			Pow(this, powVal);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixPow")]
		private static extern void Pow(Matrix thisMatrix, float powVal);

		public void Parallax(Vector2D offset, Matrix src, Matrix maskX, Matrix maskZ, int interpolation = 1)
		{
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - rect.offset.z) * rect.size.x + i - rect.offset.x;
					float num2 = ((maskX != null) ? maskX.arr[num] : 1f);
					float num3 = ((maskZ != null) ? maskZ.arr[num] : 1f);
					if (num2 < 0.0001f && num3 < 0.0001f)
					{
						arr[num] = src.arr[num];
						continue;
					}
					float num4 = (float)i - offset.x * num2;
					float num5 = (float)j - offset.z * num3;
					if (num4 < (float)min.x)
					{
						num4 = min.x;
					}
					if (num4 > (float)(max.x - 1))
					{
						num4 = max.x - 1;
					}
					if (num5 < (float)min.z)
					{
						num5 = min.z;
					}
					if (num5 > (float)(max.z - 1))
					{
						num5 = max.z - 1;
					}
					float num6 = interpolation switch
					{
						0 => src.GetFloored(num4 + 0.5f, num5 + 0.5f), 
						1 => src.GetInterpolated(num4, num5), 
						_ => (!(num2 > 0.999f) || !(num3 > 0.999f)) ? src.GetInterpolated(num4, num5) : src.GetFloored(num4 + 0.5f, num5 + 0.5f), 
					};
					arr[num] = num6;
				}
			}
		}

		public void Stroke(Vector2D pos, float radius, float hardness, bool smoothTransition = true, float bckgVal = 0f, float strokeVal = 1f)
		{
			Stroke(this, pos.x, pos.z, radius, hardness, smoothTransition, bckgVal, strokeVal);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixStroke")]
		private static extern void Stroke(Matrix thisMatrix, float posX, float posZ, float radius, float hardness, bool smoothTransition = true, float bckgVal = 0f, float strokeVal = 1f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleStripeCubic")]
		private static extern void Fallof(Matrix srcBackground, Matrix srcBrush, Matrix dst, Vector2D pos, float radius, float hardness, bool smoothTransition = true);

		public void BlendStamped(Matrix src, Matrix stamp, float centerX, float centerZ, float radius, float transition, bool smoothFallof = true)
		{
			BlendStamped(this, src, stamp, centerX, centerZ, radius, transition, smoothFallof);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixBlendStamped")]
		private static extern void BlendStamped(Matrix thisMatrix, Matrix src, Matrix stamp, float centerX, float centerZ, float radius, float transition, bool smoothFallof);

		public static void ReadMatrix(Matrix src, Matrix dst, CoordRect.TileMode tileMode = CoordRect.TileMode.Clamp)
		{
			ReadMatrix(src, dst, (int)tileMode);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ReadMatrixTile")]
		private static extern void ReadMatrix(Matrix src, Matrix dst, int tileMode);

		public static void CopyIntersected(Matrix src, Matrix dst)
		{
			ReadMatrix(src, dst);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ReadMatrixFull")]
		public static extern void ReadMatrix(Matrix src, Matrix dst);

		public static void CopyRect(Matrix src, Matrix dst, CoordRect rect)
		{
			ReadMatrix(src, dst, rect);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ReadMatrixRect")]
		public static extern void ReadMatrix(Matrix src, Matrix dst, CoordRect rect);

		public static void ReadMatrixUnfilled(Matrix src, Matrix dst, CoordRect rect)
		{
			CoordRect c = CoordRect.Intersected(rect, dst.rect);
			c = CoordRect.Intersected(src.rect, c);
			Coord min = c.Min;
			Coord max = c.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - dst.rect.offset.z) * dst.rect.size.x + i - dst.rect.offset.x;
					if (!(dst.arr[num] > 1E-05f))
					{
						int num2 = (j - src.rect.offset.z) * src.rect.size.x + i - src.rect.offset.x;
						dst.arr[num] = src.arr[num2];
					}
				}
			}
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixCopyResized")]
		public static extern void CopyResized(Matrix src, Matrix dst, Vector2D srcRectPos, Vector2D srcRectSize, Coord dstRectPos, Coord dstRectSize);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixResize")]
		public static extern void Resize(Matrix src, Matrix dst);

		public static void BlendLayers(Matrix[] matrices, float[] opacity = null)
		{
			CoordRect? coordRect = matrices.Any()?.rect;
			if (!coordRect.HasValue)
			{
				return;
			}
			int num = coordRect.Value.Count;
			for (int i = 0; i < num; i++)
			{
				float num2 = 1f;
				for (int num3 = matrices.Length - 1; num3 >= 0; num3--)
				{
					if (matrices[num3] != null)
					{
						float num4 = matrices[num3].arr[i];
						if (opacity != null)
						{
							num4 *= opacity[num3];
						}
						num4 *= num2;
						matrices[num3].arr[i] = num4;
						num2 -= num4;
						if (num2 < 0f)
						{
							break;
						}
					}
				}
			}
		}

		public static void NormalizeLayers(Matrix[] matrices, bool allowBelowOne = false)
		{
			CoordRect? coordRect = matrices.Any()?.rect;
			if (!coordRect.HasValue)
			{
				return;
			}
			int num = coordRect.Value.Count;
			for (int i = 0; i < num; i++)
			{
				float num2 = 0f;
				for (int j = 0; j < matrices.Length; j++)
				{
					num2 += matrices[j].arr[i];
				}
				if (num2 > 1f || !allowBelowOne)
				{
					for (int k = 0; k < matrices.Length; k++)
					{
						matrices[k].arr[i] /= num2;
					}
				}
			}
		}

		public static void NormalizeLayers(Matrix[] matrices, float[] opacities, bool allowBelowOne = false)
		{
			CoordRect? coordRect = matrices.Any()?.rect;
			if (!coordRect.HasValue)
			{
				return;
			}
			int num = coordRect.Value.Count;
			for (int i = 0; i < num; i++)
			{
				float num2 = 0f;
				for (int j = 0; j < matrices.Length; j++)
				{
					num2 += matrices[j].arr[i] * opacities[j];
				}
				if (num2 > 1f || !allowBelowOne)
				{
					for (int k = 0; k < matrices.Length; k++)
					{
						matrices[k].arr[i] = matrices[k].arr[i] * opacities[k] / num2;
					}
				}
			}
		}

		public static void NormalizeLayers(Matrix[] matrices, Matrix[] masks, bool allowBelowOne = false)
		{
			CoordRect? coordRect = matrices.Any()?.rect;
			if (!coordRect.HasValue)
			{
				return;
			}
			int num = coordRect.Value.Count;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < matrices.Length; j++)
				{
					matrices[j].arr[i] *= masks[j].arr[i];
				}
				float num2 = 0f;
				for (int k = 0; k < matrices.Length; k++)
				{
					num2 += matrices[k].arr[i];
				}
				if (num2 > 1f || !allowBelowOne)
				{
					for (int l = 0; l < matrices.Length; l++)
					{
						matrices[l].arr[i] /= num2;
					}
				}
			}
		}

		public bool ContainsNaN()
		{
			for (int i = 0; i < arr.Length; i++)
			{
				if (float.IsNaN(arr[i]))
				{
					return true;
				}
			}
			return false;
		}

		public void MultiplyRect(Matrix m)
		{
			CoordRect coordRect = CoordRect.Intersected(rect, m.rect);
			Coord min = coordRect.Min;
			Coord max = coordRect.Max;
			for (int i = min.z; i < max.z; i++)
			{
				for (int j = min.x; j < max.x; j++)
				{
					int num = (i - rect.offset.z) * rect.size.x + j - rect.offset.x;
					int num2 = (i - m.rect.offset.z) * m.rect.size.x + j - m.rect.offset.x;
					arr[num] *= arr[num2];
				}
			}
		}

		public void FillRect(float val, CoordRect fillRect)
		{
			CoordRect coordRect = CoordRect.Intersected(rect, fillRect);
			Coord min = coordRect.Min;
			Coord max = coordRect.Max;
			for (int i = min.z; i < max.z; i++)
			{
				for (int j = min.x; j < max.x; j++)
				{
					int num = (i - rect.offset.z) * rect.size.x + j - rect.offset.x;
					arr[num] = val;
				}
			}
		}

		public void FillOuterRect(float val, CoordRect fillRect)
		{
			Coord min = fillRect.Min;
			Coord max = fillRect.Max;
			Coord min2 = rect.Min;
			Coord max2 = rect.Max;
			for (int i = min2.z; i < max2.z; i++)
			{
				for (int j = min2.x; j < max2.x; j++)
				{
					if (j < min.x || j > max.x || i < min.z || i > max.z)
					{
						int num = (i - rect.offset.z) * rect.size.x + j - rect.offset.x;
						arr[num] = val;
					}
				}
			}
		}

		public void Crop(CoordRect newRect)
		{
			Coord min = newRect.Min;
			Coord max = newRect.Max;
			Coord min2 = rect.Min;
			Coord max2 = rect.Max;
			Matrix matrix = new Matrix(newRect);
			for (int i = min2.z; i < max2.z; i++)
			{
				for (int j = min2.x; j < max2.x; j++)
				{
					if (j >= min.x && j < max.x && i >= min.z && i < max.z)
					{
						int num = (i - rect.offset.z) * rect.size.x + j - rect.offset.x;
						int num2 = (i - newRect.offset.z) * newRect.size.x + j - newRect.offset.x;
						matrix.arr[num2] = arr[num];
					}
				}
			}
			arr = matrix.arr;
			rect = matrix.rect;
			count = matrix.count;
		}

		public float[] Histogram(int resolution, float max = 1f, bool normalize = true)
		{
			float[] array = new float[resolution];
			for (int i = 0; i < count; i++)
			{
				float num = arr[i];
				if (!(num < 0f) && !(num > max))
				{
					int num2 = (int)(num / max * (float)resolution);
					if (num2 == resolution)
					{
						num2--;
					}
					array[num2] += 1f;
				}
			}
			if (normalize)
			{
				float num3 = 0f;
				for (int j = 0; j < resolution; j++)
				{
					if (array[j] > num3)
					{
						num3 = array[j];
					}
				}
				for (int k = 0; k < resolution; k++)
				{
					array[k] /= num3;
				}
			}
			return array;
		}

		public byte[] HistogramBytes(int resolution, float max = 1f, bool normalize = true)
		{
			float[] array = Histogram(resolution, max, normalize);
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (byte)(array[i] * 255f);
			}
			return array2;
		}

		public static byte[] HistogramToTextureBytes(float[] quants, int height, byte empty = 0, byte top = 255, byte filled = 128)
		{
			int num = quants.Length;
			byte[] array = new byte[num * height];
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)(quants[i] * (float)height);
				if (num2 == height)
				{
					num2--;
				}
				for (int j = 0; j < height; j++)
				{
					byte b = empty;
					if (j == num2)
					{
						b = top;
					}
					else if (j < num2)
					{
						b = filled;
					}
					array[j * num + i] = b;
				}
			}
			return array;
		}

		public static Texture2D HistogramToTextureR8(float[] quants, int height, byte empty = 0, byte top = 255, byte filled = 128)
		{
			byte[] data = HistogramToTextureBytes(quants, height, empty, top, filled);
			Texture2D texture2D = new Texture2D(quants.Length, height, TextureFormat.R8, mipChain: false, linear: true);
			texture2D.LoadRawTextureData(data);
			texture2D.Apply(updateMipmaps: false);
			return texture2D;
		}

		public byte[] HistogramTextureBytes(int width, int height, byte empty = 0, byte top = 255, byte filled = 128)
		{
			return HistogramToTextureBytes(Histogram(width), height, empty, top, filled);
		}

		public void ToWindow(string name, bool useCopy = false, bool mainThread = false)
		{
			Matrix matrix = (useCopy ? new Matrix(this) : this);
			if (!mainThread)
			{
				onPreview?.Invoke(matrix, name);
				return;
			}
			CoroutineManager.Enqueue(delegate
			{
				onPreview?.Invoke(matrix, name);
			});
		}

		public void Line(Vector2D start, Vector2D end, float valStart = 1f, float valEnd = 1f, bool antialised = false, bool paddedOnePixel = false, bool endInclusive = false)
		{
			Line(this, start.x, start.z, end.x, end.z, valStart, valEnd, antialised, paddedOnePixel, endInclusive);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixLine")]
		private static extern void Line(Matrix thism, float startX, float startZ, float endX, float endZ, float valStart, float valEnd, bool antialised, bool paddedOnePixel, bool endInclusive);
	}
}
