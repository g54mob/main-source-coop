using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Den.Tools.Matrices
{
	public static class MatrixOps
	{
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public class Stripe
		{
			public int length;

			public float[] arr;

			public Stripe()
			{
				arr = new float[0];
				length = 0;
			}

			public Stripe(float[] arr)
			{
				this.arr = arr;
				length = arr.Length;
			}

			public Stripe(int length)
			{
				this.length = length;
				arr = new float[length];
			}

			public Stripe(float[] arr, int length)
			{
				this.arr = arr;
				this.length = length;
			}

			public Stripe(Stripe stripe)
			{
				arr = new float[stripe.arr.Length];
				Array.Copy(stripe.arr, arr, stripe.arr.Length);
				length = stripe.length;
			}

			public void Expand(int newCount)
			{
				length = newCount;
				Array.Resize(ref arr, length);
			}

			public void Fill(float val)
			{
				for (int i = 0; i < arr.Length; i++)
				{
					arr[i] = val;
				}
			}

			public static void Copy(Stripe src, Stripe dst)
			{
				Array.Copy(src.arr, dst.arr, src.length);
				dst.length = src.length;
			}

			public static void Swap(Stripe s1, Stripe s2)
			{
				float[] array = s1.arr;
				s1.arr = s2.arr;
				s2.arr = array;
			}
		}

		public static void ReadLine(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				stripe.arr[i] = matrix.arr[num + i];
			}
		}

		public static void WriteLine(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				matrix.arr[num + i] = stripe.arr[i];
			}
		}

		public static void MaxLine(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				float num2 = matrix.arr[num + i];
				float num3 = stripe.arr[i];
				if (num3 > num2)
				{
					matrix.arr[num + i] = num3;
				}
			}
		}

		public static void MinLine(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				float num2 = matrix.arr[num + i];
				float num3 = stripe.arr[i];
				if (num3 < num2)
				{
					matrix.arr[num + i] = num3;
				}
			}
		}

		public static void AddLine(Stripe stripe, Matrix matrix, int x, int z, float opacity = 1f)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				matrix.arr[num + i] += stripe.arr[i] * opacity;
			}
		}

		public static void ReadRow(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				stripe.arr[i] = matrix.arr[num + i * matrix.rect.size.x];
			}
		}

		public static void WriteRow(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				matrix.arr[num + i * matrix.rect.size.x] = stripe.arr[i];
			}
		}

		public static void WriteRow(Stripe stripe, Matrix matrix, int x, int z, float[] mask)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				matrix.arr[num + i * matrix.rect.size.x] = stripe.arr[i] * mask[i];
			}
		}

		public static void MaxRow(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				float num2 = matrix.arr[num + i * matrix.rect.size.x];
				float num3 = stripe.arr[i];
				if (num3 > num2)
				{
					matrix.arr[num + i * matrix.rect.size.x] = num3;
				}
			}
		}

		public static void MinRow(Stripe stripe, Matrix matrix, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				float num2 = matrix.arr[num + i * matrix.rect.size.x];
				float num3 = stripe.arr[i];
				if (num3 < num2)
				{
					matrix.arr[num + i * matrix.rect.size.x] = num3;
				}
			}
		}

		public static void AddRow(Stripe stripe, Matrix matrix, int x, int z, float opacity = 1f)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				matrix.arr[num + i * matrix.rect.size.x] += stripe.arr[i] * opacity;
			}
		}

		public static void OverlayRow(Stripe stripe, Matrix matrix, int x, int z, float opacity = 1f)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				float num2 = matrix.arr[num + i * matrix.rect.size.x];
				float num3 = stripe.arr[i];
				num3 *= opacity;
				matrix.arr[num + i * matrix.rect.size.x] = 2f * num2 * num3 + num2 + num3;
			}
		}

		public static void MixRow(Matrix dst, Matrix matrix, Matrix matrixMask, Stripe stripe, Stripe stripeMask, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				int num2 = num + i * matrix.rect.size.x;
				float num3 = matrixMask.arr[num2] + stripeMask.arr[i];
				dst.arr[num2] = ((num3 > 0f) ? ((matrix.arr[num2] * matrixMask.arr[num2] + stripe.arr[i] * stripeMask.arr[i]) / num3) : (matrix.arr[num2] + stripe.arr[i]));
			}
		}

		public static void MaxRow(Matrix dst, Matrix matrix, Matrix matrixMask, Stripe stripe, Stripe stripeMask, int x, int z)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				int num2 = num + i * matrix.rect.size.x;
				dst.arr[num2] = ((matrixMask.arr[num2] > stripeMask.arr[i]) ? matrix.arr[num2] : stripe.arr[i]);
			}
		}

		public static void ReadDiagonal(Stripe stripe, Matrix matrix, int x, int z, float stepX = 1f, float stepZ = 1f)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			float num2 = matrix.rect.size.x + matrix.rect.size.z;
			if (stepX > 0.001f)
			{
				num2 = (float)(matrix.rect.offset.x + matrix.rect.size.x - x) / stepX;
			}
			if (stepX < -0.001f)
			{
				num2 = (float)(x - matrix.rect.offset.x) / (0f - stepX);
			}
			float num3 = matrix.rect.size.x + matrix.rect.size.z;
			if (stepZ > 0.001f)
			{
				num3 = (float)(matrix.rect.offset.z + matrix.rect.size.z - z) / stepZ;
			}
			if (stepZ < -0.001f)
			{
				num3 = (float)(z - matrix.rect.offset.z) / (0f - stepZ);
			}
			stripe.length = (int)(((num2 < num3) ? num2 : num3) - 0.5f);
			for (int i = 0; i < stripe.length; i++)
			{
				int num4 = (int)((float)i * stepX + 0.5f);
				int num5 = (int)((float)i * stepZ + 0.5f);
				stripe.arr[i] = matrix.arr[num + num4 + num5 * matrix.rect.size.x];
			}
		}

		public static void WriteDiagonal(Stripe stripe, Matrix matrix, int x, int z, float stepX = 1f, float stepZ = 1f)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				int num2 = (int)((float)i * stepX + 0.5f);
				int num3 = (int)((float)i * stepZ + 0.5f);
				matrix.arr[num + num2 + num3 * matrix.rect.size.x] = stripe.arr[i];
			}
		}

		public static void MaxDiagonal(Stripe stripe, Matrix matrix, int x, int z, float stepX = 1f, float stepZ = 1f)
		{
			int num = (z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + x;
			for (int i = 0; i < stripe.length; i++)
			{
				int num2 = (int)((float)i * stepX + 0.5f);
				int num3 = (int)((float)i * stepZ + 0.5f);
				float num4 = matrix.arr[num + num2 + num3 * matrix.rect.size.x];
				float num5 = stripe.arr[i];
				if (num5 > num4)
				{
					matrix.arr[num + num2 + num3 * matrix.rect.size.x] = num5;
				}
			}
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ReadInclined")]
		public static extern void ReadInclined(Stripe stripe, Matrix matrix, Vector2 start, Vector2 step);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_WriteInclined")]
		public static extern void WriteInclined(Stripe stripe, Matrix matrix, Vector2 start, Vector2 step);

		public static void ReadStrip(Stripe stripe, Matrix matrix, Vector2 start, Vector2 end)
		{
			Vector2 vector = end - start;
			Vector2 normalized = vector.normalized;
			Vector2 vector2 = new Vector2((normalized.x > 0f) ? normalized.x : (0f - normalized.x), (normalized.y > 0f) ? normalized.y : (0f - normalized.y));
			float num = ((vector2.x > vector2.y) ? vector2.x : vector2.y);
			Vector2 step = normalized / num;
			Vector2 vector3 = new Vector2((vector.x > 0f) ? vector.x : (0f - vector.x), (vector.y > 0f) ? vector.y : (0f - vector.y));
			int num2 = (int)((vector3.x > vector3.y) ? vector3.x : vector3.y) + 1;
			stripe.length = ((num2 < stripe.length) ? num2 : stripe.length);
			ReadInclined(stripe, matrix, start, step);
		}

		public static void WriteStrip(Stripe stripe, Matrix matrix, Vector2 start, Vector2 end)
		{
			Vector2 vector = end - start;
			Vector2 normalized = vector.normalized;
			Vector2 vector2 = new Vector2((normalized.x > 0f) ? normalized.x : (0f - normalized.x), (normalized.y > 0f) ? normalized.y : (0f - normalized.y));
			float num = ((vector2.x > vector2.y) ? vector2.x : vector2.y);
			Vector2 step = normalized / num;
			int num2 = (int)((vector.x > vector.y) ? vector.x : vector.y) + 1;
			stripe.length = ((num2 < stripe.length) ? num2 : stripe.length);
			WriteInclined(stripe, matrix, start, step);
		}

		public static void ReadSquare(Stripe stripe, Matrix matrix, Coord center, int radius)
		{
			int num = radius * 2 + 1;
			stripe.length = num * 4;
			for (int i = 0; i < num * 4; i++)
			{
				stripe.arr[i] = 0f - Mathf.Epsilon;
			}
			Coord coord = center - radius;
			Coord coord2 = center + radius;
			Coord offset = matrix.rect.offset;
			Coord coord3 = matrix.rect.offset + matrix.rect.size;
			int num2 = (coord.z - matrix.rect.offset.z - 1) * matrix.rect.size.x - matrix.rect.offset.x + coord.x;
			if (coord.z - 1 >= offset.z && coord.z - 1 < coord3.z)
			{
				for (int j = 0; j < num; j++)
				{
					if (j + coord.x >= offset.x && j + coord.x < coord3.x)
					{
						stripe.arr[j] = matrix.arr[num2 + j];
					}
				}
			}
			num2 = (coord.z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + coord2.x;
			if (coord2.x >= offset.x && coord2.x < coord3.x)
			{
				for (int k = 0; k < num; k++)
				{
					if (k + coord.z >= offset.z && k + coord.z < coord3.z)
					{
						stripe.arr[k + num] = matrix.arr[num2 + k * matrix.rect.size.x];
					}
				}
			}
			num2 = (coord2.z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + coord2.x;
			if (coord2.z >= offset.z && coord2.z < coord3.z)
			{
				for (int l = 0; l < num; l++)
				{
					if (coord2.x - l >= offset.x && coord2.x - l < coord3.x)
					{
						stripe.arr[l + num * 2] = matrix.arr[num2 - l];
					}
				}
			}
			num2 = (coord2.z - matrix.rect.offset.z - 1) * matrix.rect.size.x - matrix.rect.offset.x + coord.x;
			if (coord.x - 1 >= offset.x && coord.x < coord3.x)
			{
				for (int m = 0; m < num; m++)
				{
					if (coord2.z - 1 - m >= offset.z && coord2.z - 1 - m < coord3.z)
					{
						stripe.arr[m + num * 3] = matrix.arr[num2 - m * matrix.rect.size.x];
					}
				}
			}
			stripe.arr[0] = stripe.arr[stripe.length - 1];
		}

		public static void WriteSquare(Stripe stripe, Matrix matrix, Coord center, int radius)
		{
			int num = radius * 2 + 1;
			stripe.length = num * 4;
			Coord coord = center - radius;
			Coord coord2 = center + radius;
			Coord offset = matrix.rect.offset;
			Coord coord3 = matrix.rect.offset + matrix.rect.size;
			int num2 = (coord.z - matrix.rect.offset.z - 1) * matrix.rect.size.x - matrix.rect.offset.x + coord.x;
			if (coord.z - 1 >= offset.z && coord.z - 1 < coord3.z)
			{
				for (int i = 0; i < num; i++)
				{
					if (i + coord.x >= offset.x && i + coord.x < coord3.x)
					{
						matrix.arr[num2 + i] = stripe.arr[i];
					}
				}
			}
			num2 = (coord.z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + coord2.x;
			if (coord2.x >= offset.x && coord2.x < coord3.x)
			{
				for (int j = 0; j < num; j++)
				{
					if (j + coord.z >= offset.z && j + coord.z < coord3.z)
					{
						matrix.arr[num2 + j * matrix.rect.size.x] = stripe.arr[j + num];
					}
				}
			}
			num2 = (coord2.z - matrix.rect.offset.z) * matrix.rect.size.x - matrix.rect.offset.x + coord2.x;
			if (coord2.z >= offset.z && coord2.z < coord3.z)
			{
				for (int k = 0; k < num; k++)
				{
					if (coord2.x - k >= offset.x && coord2.x - k < coord3.x)
					{
						matrix.arr[num2 - k] = stripe.arr[k + num * 2];
					}
				}
			}
			num2 = (coord2.z - matrix.rect.offset.z - 1) * matrix.rect.size.x - matrix.rect.offset.x + coord.x;
			if (coord.x < offset.x || coord.x >= coord3.x)
			{
				return;
			}
			for (int l = 0; l < num; l++)
			{
				if (coord2.z - 1 - l >= offset.z && coord2.z - 1 - l < coord3.z)
				{
					matrix.arr[num2 - l * matrix.rect.size.x] = stripe.arr[l + num * 3];
				}
			}
		}

		public static void FlipVertical(Matrix src, Matrix dst)
		{
			Coord min = src.rect.Min;
			Coord max = src.rect.Max;
			Stripe stripe = new Stripe(src.rect.size.x);
			for (int i = min.z; i < max.z; i++)
			{
				ReadLine(stripe, src, src.rect.offset.x, i);
				WriteLine(stripe, dst, dst.rect.offset.x, max.z - (i - min.z) - 1);
			}
		}

		public static void Resize(Matrix src, CoordRect dstRect)
		{
			Matrix matrix = new Matrix(dstRect);
			Resize(src, matrix);
			src.rect = matrix.rect;
			src.arr = matrix.arr;
		}

		public static void Resize(Matrix src, Matrix dst, Matrix tmp = null)
		{
			Coord offset = dst.rect.offset;
			dst.rect.offset = src.rect.offset;
			Coord size = src.rect.size;
			Coord size2 = dst.rect.size;
			Stripe stripe = new Stripe(Mathf.Max(size.x, size.z, dst.rect.size.x, dst.rect.size.z));
			Stripe dstStripe = new Stripe(stripe.length);
			_ = src.rect.Min;
			_ = src.rect.Max;
			_ = dst.rect.Max;
			if (size2.x < size.x && size2.z < size.z)
			{
				if (tmp == null)
				{
					tmp = new Matrix(src.rect.offset.x, src.rect.offset.z, size2.x, size.z);
				}
				DownsizeHorizontally(src, tmp, stripe, dstStripe);
				DownsizeVertically(tmp, dst, stripe, dstStripe);
			}
			else if (size2.x > size.x && size2.z > size.z)
			{
				tmp = new Matrix(size: new Coord(size2.x, size.z), offset: src.rect.offset, array: dst.arr);
				UpsizeHorizontally(src, tmp, stripe, dstStripe);
				UpsizeVertically(tmp, dst, stripe, dstStripe);
			}
			else if (size2.x > size.x && size2.z < size.z)
			{
				tmp = new Matrix(size: new Coord(size.x, size2.z), offset: src.rect.offset, array: dst.arr);
				DownsizeVertically(src, tmp, stripe, dstStripe);
				UpsizeHorizontally(tmp, dst, stripe, dstStripe);
			}
			else if (size2.x < size.x && size2.z > size.z)
			{
				tmp = new Matrix(size: new Coord(size2.x, size.z), offset: src.rect.offset, array: dst.arr);
				DownsizeHorizontally(src, tmp, stripe, dstStripe);
				UpsizeVertically(tmp, dst, stripe, dstStripe);
			}
			else if (size2.x > size.x && size2.z == size.z)
			{
				UpsizeHorizontally(src, dst, stripe, dstStripe);
			}
			else if (size2.x < size.x && size2.z == size.z)
			{
				DownsizeHorizontally(src, dst, stripe, dstStripe);
			}
			else if (size2.x == size.x && size2.z > size.z)
			{
				UpsizeVertically(src, dst, stripe, dstStripe);
			}
			else if (size2.x == size.x && size2.z < size.z)
			{
				DownsizeVertically(src, dst, stripe, dstStripe);
			}
			else
			{
				dst.Fill(src);
			}
			dst.rect.offset = offset;
		}

		public static void Upsize(Matrix src, Matrix dst)
		{
			if (dst.rect.size.x < src.rect.size.x || dst.rect.size.z < src.rect.size.z)
			{
				throw new Exception("Couldn't upsize: src " + src.rect.ToString() + " is less than dst " + dst.rect.ToString());
			}
			Stripe stripe = new Stripe(Mathf.Max(src.rect.size.x, src.rect.size.z, dst.rect.size.x, dst.rect.size.z));
			Stripe dstStripe = new Stripe(stripe.length);
			Matrix matrix = new Matrix(size: new Coord(dst.rect.size.x, src.rect.size.z), offset: src.rect.offset, array: dst.arr);
			UpsizeHorizontally(src, matrix, stripe, dstStripe, 0f, src.rect.size.x);
			UpsizeVertically(matrix, dst, stripe, dstStripe, 0f, src.rect.size.z);
		}

		public static void Upsize(Matrix src, Vector2D srcOffset, Vector2D srcSize, Matrix dst)
		{
			if (dst.rect.size.x < src.rect.size.x || dst.rect.size.z < src.rect.size.z)
			{
				throw new Exception("Couldn't upsize: src " + src.rect.ToString() + " is less than dst " + dst.rect.ToString());
			}
			Stripe stripe = new Stripe(Mathf.Max(src.rect.size.x, src.rect.size.z, dst.rect.size.x, dst.rect.size.z));
			Stripe dstStripe = new Stripe(stripe.length);
			Matrix matrix = new Matrix(size: new Coord(dst.rect.size.x, src.rect.size.z), offset: src.rect.offset, array: dst.arr);
			UpsizeHorizontally(src, matrix, stripe, dstStripe, srcOffset.x - (float)src.rect.offset.x, srcSize.x);
			UpsizeVertically(matrix, dst, stripe, dstStripe, srcOffset.z - (float)src.rect.offset.z, srcSize.z);
		}

		public static void Downsize(Matrix src, Matrix dst, Matrix tmp = null)
		{
			if (dst.rect.size.x > src.rect.size.x || dst.rect.size.z > src.rect.size.z)
			{
				throw new Exception("Couldn't downsize: src " + src.rect.ToString() + " is more than dst " + dst.rect.ToString());
			}
			Stripe stripe = new Stripe(Mathf.Max(src.rect.size.x, src.rect.size.z, dst.rect.size.x, dst.rect.size.z));
			Stripe dstStripe = new Stripe(stripe.length);
			if (tmp == null)
			{
				tmp = new Matrix(src.rect.offset.x, src.rect.offset.z, dst.rect.size.x, src.rect.size.z);
			}
			DownsizeHorizontally(src, tmp, stripe, dstStripe);
			DownsizeVertically(tmp, dst, stripe, dstStripe);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_DownsizeHorizontally")]
		private static extern void DownsizeHorizontally(Matrix src, Matrix dst, Stripe srcStripe, Stripe dstStripe);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_DownsizeVertically")]
		private static extern void DownsizeVertically(Matrix src, Matrix dst, Stripe srcStripe, Stripe dstStripe);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_UpsizeHorizontally")]
		private static extern void UpsizeHorizontally(Matrix src, Matrix dst, Stripe srcStripe, Stripe dstStripe, float srcOffset = 0f, float srcLength = 0f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_UpsizeVertically")]
		private static extern void UpsizeVertically(Matrix src, Matrix dst, Stripe srcStripe, Stripe dstStripe, float srcOffset = 0f, float srcLength = 0f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleStripeCubic")]
		private static extern void ResampleStripeCubic(Stripe src, Stripe dst, float srcOffset = 0f, float srcLength = 0f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleStripeQuadratic")]
		private static extern void ResampleStripeQuadratic(Stripe src, Stripe dst);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleStripeLinear")]
		private static extern void ResampleStripeLinear(Stripe src, Stripe dst);

		public static void ResampleStripeLinearInterpolated(Stripe src, Stripe dst, double dstOffsetPercent, double dstSizePercent)
		{
			int num = dst.length - 1;
			int num2 = src.length - 1;
			for (int i = 0; i < dst.length; i++)
			{
				double num3 = (double)i / (double)num;
				_ = dstOffsetPercent + num3 * dstSizePercent;
				_ = src.length;
				double num4 = (double)(i - 1) / (double)num;
				double num5 = (double)(i + 1) / (double)num;
				double num6 = dstOffsetPercent + num4 * dstSizePercent;
				double num7 = dstOffsetPercent + num5 * dstSizePercent;
				int num8 = (int)(num6 * (double)num2);
				if (num8 < 0)
				{
					num8 = 0;
				}
				int num9 = (int)(num7 * (double)num2);
				if (num9 >= src.length)
				{
					num9 = src.length - 1;
				}
				float num10 = 0f;
				float num11 = 0f;
				for (int j = num8; j <= num9; j++)
				{
					float num12 = (float)(((double)((float)j / (float)num2) - dstOffsetPercent) / dstSizePercent * (double)num) - (float)i;
					if (num12 > 0.999f)
					{
						num12 = 0.999f;
					}
					if (num12 < -0.999f)
					{
						num12 = -0.999f;
					}
					if (num12 < 0f)
					{
						num12 = 0f - num12;
					}
					num12 = 1f - num12;
					num10 += src.arr[j] * num12;
					num11 += num12;
				}
				dst.arr[i] = ((num11 != 0f) ? (num10 / num11) : 0f);
			}
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResizeNearestNeighbor")]
		public static extern void ResizeNearestNeighbor(Matrix src, Matrix dst);

		public static void ResizeInterpolatedWithUpscale(Matrix src, Matrix dst, Vector2D dstPos, Vector2D dstSize, ref Matrix tmp)
		{
			Coord coord = new Coord(dst.rect.size.x, src.rect.size.z);
			Coord offset = new Coord(dst.rect.offset.x, src.rect.offset.z);
			if (tmp == null || tmp.rect.size != coord)
			{
				tmp = new Matrix(offset, coord);
			}
		}

		private static void DownsizeInterpolatedHorizontally(Matrix src, Matrix dst, Stripe srcStripe, Stripe dstStripe, float dstOffsetPercent, float dstSizePercent)
		{
			srcStripe.length = src.rect.size.x;
			dstStripe.length = dst.rect.size.x;
			for (int i = 0; i < src.rect.size.z; i++)
			{
				ReadLine(srcStripe, src, src.rect.offset.x, i + src.rect.offset.z);
				WriteLine(dstStripe, dst, dst.rect.offset.x, i + dst.rect.offset.z);
			}
		}

		public static void ResizeFast(Matrix src, Matrix dst)
		{
			if (dst.rect.size.x == src.rect.size.x * 2 || dst.rect.size.z == src.rect.size.z * 2)
			{
				UpscaleFast(src, dst);
				return;
			}
			if (dst.rect.size.x * 2 == src.rect.size.x || dst.rect.size.z * 2 == src.rect.size.z)
			{
				DownscaleFast(src, dst);
				return;
			}
			throw new Exception("Matrix ResizeFast: rect size mismatch: src:" + src.rect.size.ToString() + " dst:" + dst.rect.size.ToString());
		}

		public static Matrix[] GenerateMips(Matrix src, int count = -1, float blur = 0f, float multiply = 1f)
		{
			if (count < 0)
			{
				count = (int)Mathf.Log(src.rect.size.x, 2f) - 1;
			}
			CoordRect rect = src.rect;
			Matrix[] array = new Matrix[count];
			Matrix matrix = src;
			Matrix tmp = new Matrix(new CoordRect(rect.offset.x, rect.offset.z, rect.size.x / 2, rect.size.z));
			Stripe stripe = new Stripe(Mathf.Max(rect.size.x, rect.size.z));
			Stripe dstStripe = new Stripe(stripe.length);
			for (int i = 0; i < count; i++)
			{
				Matrix matrix2 = new Matrix(new CoordRect(matrix.rect.offset.x, matrix.rect.offset.z, matrix.rect.size.x / 2, matrix.rect.size.z / 2));
				DownscaleFast(matrix, matrix2, tmp, stripe, dstStripe);
				if (blur > 0.0001f)
				{
					GaussianBlur(matrix2, blur);
				}
				if (multiply != 1f)
				{
					matrix2.Multiply(multiply);
				}
				array[i] = matrix2;
				matrix = matrix2;
			}
			return array;
		}

		public static Matrix TestMips(Matrix[] mips)
		{
			int num = 0;
			for (int i = 0; i < mips.Length; i++)
			{
				num += mips[i].rect.size.x;
			}
			Matrix matrix = new Matrix(new CoordRect(0, 0, num, mips[0].rect.size.x));
			matrix.Fill(-1f);
			num = 0;
			foreach (Matrix matrix2 in mips)
			{
				CoordRect rect = matrix2.rect;
				for (int k = 0; k < rect.size.x; k++)
				{
					for (int l = 0; l < rect.size.z; l++)
					{
						int num2 = l * rect.size.x + k;
						int num3 = l * matrix.rect.size.x + k + num;
						matrix.arr[num3] = matrix2.arr[num2];
					}
				}
				num += rect.size.x;
			}
			return matrix;
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_UpscaleFast")]
		public static extern void UpscaleFast(Matrix src, Matrix dst);

		public static void DownscaleFast(Matrix src, Matrix dst)
		{
			DownscaleFast(src, dst, null, null, null);
		}

		public static void DownscaleFast(Matrix src, Matrix dst, Matrix tmp = null, Stripe srcStripe = null, Stripe dstStripe = null)
		{
			if (tmp == null)
			{
				tmp = new Matrix(new CoordRect(src.rect.offset.x, src.rect.offset.z, src.rect.size.x / 2, src.rect.size.z));
			}
			if (srcStripe == null)
			{
				srcStripe = new Stripe(src.rect.size.Maximal);
			}
			if (dstStripe == null)
			{
				dstStripe = new Stripe(srcStripe.length);
			}
			srcStripe.length = src.rect.size.x;
			dstStripe.length = dst.rect.size.x;
			for (int i = 0; i < src.rect.size.z; i++)
			{
				ReadLine(srcStripe, src, src.rect.offset.x, i + src.rect.offset.z);
				ResampleStripeDownFast(srcStripe, dstStripe);
				WriteLine(dstStripe, tmp, tmp.rect.offset.x, i + tmp.rect.offset.z);
			}
			srcStripe.length = src.rect.size.z;
			dstStripe.length = dst.rect.size.z;
			for (int j = 0; j < dst.rect.size.x; j++)
			{
				ReadRow(srcStripe, tmp, j + tmp.rect.offset.x, tmp.rect.offset.z);
				ResampleStripeDownFast(srcStripe, dstStripe);
				WriteRow(dstStripe, dst, j + dst.rect.offset.x, dst.rect.offset.z);
			}
		}

		private static void ResampleStripeDownFast(Stripe src, Stripe dst)
		{
			for (int i = 1; i < dst.length - 1; i++)
			{
				dst.arr[i] = src.arr[i * 2] * 0.5f + src.arr[i * 2 - 1] * 0.5f;
			}
			dst.arr[0] = src.arr[0] * 0.75f + src.arr[1] * 0.25f;
			dst.arr[dst.length - 1] = src.arr[src.length - 1] * 0.75f + src.arr[src.length - 2] * 0.25f;
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleStripeUpFast")]
		private static extern void ResampleStripeUpFast(Stripe src, Stripe dst);

		public static void GaussianBlur(Matrix matrix, float blur)
		{
			GaussianBlur(matrix, matrix, blur);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_GaussianBlur")]
		public static extern void GaussianBlur(Matrix src, Matrix dst, float blur);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_BlurStripe")]
		public static extern void BlurStripe(Stripe src, float blur);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_BlurIteration")]
		public static extern void BlurIteration(Stripe src, float blur);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_BlurIteration05")]
		public static extern void BlurIteration(Stripe src);

		public static void DownsampleBlur(Matrix matrix, int downsample, float blur)
		{
			DownsampleBlur(matrix, matrix, downsample, blur);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_DownsampleBlur")]
		public static extern void DownsampleBlur(Matrix src, Matrix dst, int downsample, float blur);

		public static void OverblurMipped(Matrix matrix, float downsample, float escalate = 4f, float blur = 1f)
		{
			OverblurMipped(matrix, matrix, downsample, escalate, blur);
		}

		public static void OverblurMipped(Matrix src, Matrix dst, float downsample, float escalate = 4f, float blur = 1f)
		{
			int count = (int)downsample + 1;
			Matrix[] array = GenerateMips(src, count, 1f, 2f);
			if (dst != src)
			{
				dst.Fill(src);
			}
			ArrayTools.Insert(ref array, 0, dst);
			float m = downsample - (float)(int)downsample;
			array[array.Length - 1].Multiply(m);
			Matrix tmp = new Matrix(new CoordRect(src.rect.offset.x, src.rect.offset.z, src.rect.size.x, src.rect.size.z / 2));
			Stripe stripe = new Stripe(Mathf.Max(src.rect.size.x, src.rect.size.z));
			Stripe dstStripe = new Stripe(stripe.length);
			for (int num = array.Length - 2; num >= 0; num--)
			{
				OverblurMippedIteration(array[num + 1], array[num], tmp, stripe, dstStripe, escalate);
			}
		}

		private static void OverblurMippedIteration(Matrix mip, Matrix mat, Matrix tmp, Stripe srcStripe, Stripe dstStripe, float escalate)
		{
			srcStripe.length = mip.rect.size.x;
			dstStripe.length = mat.rect.size.x;
			for (int i = mip.rect.offset.z; i < mip.rect.offset.z + mip.rect.size.z; i++)
			{
				ReadLine(srcStripe, mip, mip.rect.offset.x, i);
				ResampleStripeUpFast(srcStripe, dstStripe);
				WriteLine(dstStripe, tmp, mat.rect.offset.x, i);
			}
			srcStripe.length = mip.rect.size.z;
			dstStripe.length = mat.rect.size.z;
			for (int j = mat.rect.offset.x; j < mat.rect.offset.x + mat.rect.size.x; j++)
			{
				ReadRow(srcStripe, tmp, j, mat.rect.offset.z);
				ResampleStripeUpFast(srcStripe, dstStripe);
				OverlayRow(dstStripe, mat, j, mat.rect.offset.z, escalate);
			}
		}

		public static (Matrix r, Matrix g, Matrix b) NormalsSet(Matrix src, float pixelSize, float height)
		{
			Matrix matrix = new Matrix(src.rect);
			Matrix matrix2 = new Matrix(src.rect);
			Matrix matrix3 = new Matrix(src.rect);
			NormalsSet(src, matrix, matrix2, matrix3, pixelSize, height);
			return (r: matrix, g: matrix2, b: matrix3);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_NormalsSet")]
		public static extern void NormalsSet(Matrix src, Matrix normX, Matrix normZ, Matrix normBlue, float pixelSize, float height);

		public static void NormalsDir(Matrix src, Matrix dst, Vector3 dir, float pixelSize, float height, float intensity = 1f, float wrapping = 1f)
		{
			wrapping /= 2f;
			Matrix normX = new Matrix(src.rect);
			Matrix normZ = new Matrix(src.rect);
			dst.Fill(0f);
			NormalsSet(src, normX, normZ, dst, pixelSize, height);
			SetToDir(normX, normZ, dst, dst, dir.x, dir.y, dir.z, intensity, wrapping);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SetToDir")]
		public static extern void SetToDir(Matrix normX, Matrix normZ, Matrix normBlue, Matrix dst, float dirX, float dirY, float dirZ, float intensity = 1f, float wrapping = 1f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_Delta")]
		public static extern void Delta(Matrix src, Matrix dst);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_Silhouette")]
		public static extern void Silhouette(Matrix src, Matrix dst, float level, bool antialiasing = true);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SilhouetteStripe")]
		public static extern void SilhouetteStripe(Stripe stripe, float level, bool antiAliasing);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_Cavity")]
		public static extern void Cavity(Matrix src, Matrix dst);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_OverSpread")]
		public static extern void OverSpread(Matrix src, Matrix dst, float multiply);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SpreadLinear")]
		public static extern void SpreadLinear(Matrix src, Matrix dst, float subtract = 0.01f, bool diagonals = false, bool quarters = false, bool bulb = false);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SpreadLinearRight")]
		public static extern void SpreadLinearRight(Stripe stripe, float subtract = 0.01f, float regardSmallerValues = 1f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SpreadLinearLeft")]
		public static extern void SpreadLinearLeft(Stripe stripe, float subtract = 0.01f, float regardSmallerValues = 1f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SpreadMultiply")]
		public static extern void SpreadMultiply(Stripe stripe, float multiply = 1f, float regardSmallerValues = 1f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SpreadMultiplyLeft")]
		public static extern void SpreadMultiplyLeft(Stripe stripe, float multiply = 1f, float regardSmallerValues = 1f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_SpreadMultiplyRight")]
		public static extern void SpreadMultiplyRight(Stripe stripe, float multiply = 1f, float regardSmallerValues = 1f);

		public static void PaddingMipped(Matrix src, Matrix mask, Matrix dst, int mipsCount = -1, float mipContrast = 2f, float mipOnePxPadding = 0.5f)
		{
			if (src == dst)
			{
				throw new Exception("MatrixOps: same matrix is used as src and dst at the same time");
			}
			if (mipsCount < 0)
			{
				mipsCount = (int)Mathf.Log(src.rect.size.x, 2f) - 1;
			}
			Matrix[] array = new Matrix[mipsCount];
			Matrix[] array2 = new Matrix[mipsCount];
			Matrix matrix = src;
			Matrix srcMask = mask;
			CoordRect rect = src.rect;
			Matrix tmp = new Matrix(new CoordRect(rect.offset.x, rect.offset.z, rect.size.x / 2, rect.size.z));
			Matrix tmpMask = new Matrix(new CoordRect(rect.offset.x, rect.offset.z, rect.size.x / 2, rect.size.z));
			for (int i = 0; i < mipsCount; i++)
			{
				Matrix matrix2 = new Matrix(new CoordRect(matrix.rect.offset.x, matrix.rect.offset.z, matrix.rect.size.x / 2, matrix.rect.size.z / 2));
				Matrix matrix3 = new Matrix(matrix2.rect);
				DownscaleMaskedFast(matrix, srcMask, matrix2, matrix3, tmp, tmpMask);
				matrix3.Multiply(mipContrast);
				matrix3.Clamp01();
				PaddingOnePixel(matrix2, matrix3, mipOnePxPadding * (1f - 1f * (float)i / (float)mipsCount));
				array[i] = matrix2;
				array2[i] = matrix3;
				matrix = matrix2;
				srcMask = matrix3;
			}
			ArrayTools.Insert(ref array, 0, src);
			ArrayTools.Insert(ref array2, 0, mask);
			for (int num = array.Length - 2; num >= 0; num--)
			{
				Matrix obj = array[num + 1];
				tmp = ((num != 0) ? new Matrix(array[num].rect) : dst);
				tmpMask = new Matrix(array[num].rect);
				GaussianBlur(obj, 0.5f);
				GaussianBlur(obj, 0.5f);
				UpscaleMaskedFast(obj, array2[num + 1], tmp, tmpMask);
				tmp.Mix(array[num], array2[num]);
				array[num] = tmp;
				array2[num] = tmpMask;
			}
		}

		public static void PaddingOnePixel(Matrix matrix, Matrix mask, float intensity = 1f)
		{
			PaddingOnePixel(matrix, mask, matrix, mask, intensity);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PaddingOnePixel")]
		public static extern void PaddingOnePixel(Matrix src, Matrix srcMask, Matrix dst, Matrix dstMask, float intensity = 1f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PadStripeOnePixel")]
		public static extern void PadStripeOnePixel(Stripe stripe, Stripe mask, float intensity = 1f, bool toLeft = false);

		public static (Matrix[], Matrix[]) GenerateMaskedMips(Matrix src, Matrix mask, int count = -1, float blur = 0f)
		{
			if (count < 0)
			{
				count = (int)Mathf.Log(src.rect.size.x, 2f) - 1;
			}
			Matrix[] array = new Matrix[count];
			Matrix[] array2 = new Matrix[count];
			Matrix matrix = src;
			Matrix srcMask = mask;
			CoordRect rect = src.rect;
			Matrix tmp = new Matrix(new CoordRect(rect.offset.x, rect.offset.z, rect.size.x / 2, rect.size.z));
			Matrix tmpMask = new Matrix(new CoordRect(rect.offset.x, rect.offset.z, rect.size.x / 2, rect.size.z));
			for (int i = 0; i < count; i++)
			{
				Matrix matrix2 = new Matrix(new CoordRect(matrix.rect.offset.x, matrix.rect.offset.z, matrix.rect.size.x / 2, matrix.rect.size.z / 2));
				Matrix matrix3 = new Matrix(matrix2.rect);
				DownscaleMaskedFast(matrix, srcMask, matrix2, matrix3, tmp, tmpMask);
				array[i] = matrix2;
				array2[i] = matrix3;
				matrix = matrix2;
				srcMask = matrix3;
			}
			return (array, array2);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_DownscaleMaskedFast")]
		public static extern void DownscaleMaskedFast(Matrix src, Matrix srcMask, Matrix dst, Matrix dstMask, Matrix tmp, Matrix tmpMask);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleAutomaskedStripeDownFast")]
		public static extern void ResampleAutomaskedStripeDownFast(Stripe src, Stripe dst, float minVal);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleMaskedStripeDownFast")]
		public static extern void ResampleMaskedStripeDownFast(Stripe src, Stripe mask, Stripe dst);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_UpscaleMaskedFast")]
		public static extern void UpscaleMaskedFast(Matrix src, Matrix srcMask, Matrix dst, Matrix dstMask);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleAutomaskedStripeUpFast")]
		public static extern void ResampleAutomaskedStripeUpFast(Stripe src, Stripe dst, float minVal);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_ResampleMaskedStripeUpFast")]
		public static extern void ResampleMaskedStripeUpFast(Stripe src, Stripe mask, Stripe dst);

		public static void PaddingSpread(Matrix srcDst, Matrix mask)
		{
			PaddingSpread(srcDst, srcDst, mask, mask);
		}

		public static void PaddingSpread(Matrix src, Matrix dst, Matrix srcMask, Matrix dstMask, float horFade = 0.8f, float vertFade = 0.8f)
		{
			Stripe stripe = new Stripe(Mathf.Max(src.rect.size.x, src.rect.size.z));
			Stripe stripe2 = new Stripe(Mathf.Max(src.rect.size.x, src.rect.size.z));
			Coord min = src.rect.Min;
			Coord max = src.rect.Max;
			stripe.length = (stripe2.length = src.rect.size.x);
			for (int i = min.z; i < max.z; i++)
			{
				ReadLine(stripe, src, src.rect.offset.x, i);
				ReadLine(stripe2, srcMask, srcMask.rect.offset.x, i);
				PadStripe(stripe, stripe2, vertFade);
				PadStripe(stripe, stripe2, vertFade, toLeft: true);
				WriteLine(stripe, dst, dst.rect.offset.x, i);
				WriteLine(stripe2, dstMask, dstMask.rect.offset.x, i);
			}
			stripe.length = (stripe2.length = src.rect.size.z);
			for (int j = min.x; j < max.x; j++)
			{
				ReadRow(stripe, dst, j, src.rect.offset.z);
				ReadRow(stripe2, dstMask, j, src.rect.offset.z);
				for (int k = 0; k < stripe2.length; k++)
				{
					stripe2.arr[k] *= stripe2.arr[k];
				}
				PadStripe(stripe, stripe2, vertFade);
				PadStripe(stripe, stripe2, vertFade, toLeft: true);
				WriteRow(stripe, dst, j, dst.rect.offset.z);
				WriteRow(stripe2, dstMask, j, dstMask.rect.offset.z);
			}
		}

		private static void PadStripe(Stripe stripe, Stripe mask, float fade = 0.9f, bool toLeft = false)
		{
			float num = stripe.arr[toLeft ? (stripe.arr.Length - 1) : 0];
			float num2 = mask.arr[toLeft ? (stripe.arr.Length - 1) : 0];
			for (int i = (toLeft ? (stripe.arr.Length - 1) : 0); (!toLeft) ? (i < stripe.arr.Length) : (i >= 0); i += ((!toLeft) ? 1 : (-1)))
			{
				float num3 = stripe.arr[i];
				float num4 = mask.arr[i];
				float num5 = (num3 * num4 + num * num2) / ((num4 + num2 != 0f) ? (num4 + num2) : 1f);
				num3 = num3 * num4 + num5 * (1f - num4);
				num4 = (((long)(num4 * 4.611686E+18f) > (long)(num2 * 4.611686E+18f)) ? num4 : num2);
				num = num3;
				num2 = num4 * fade;
				stripe.arr[i] = num3;
				mask.arr[i] = num4;
			}
		}

		private static void PadStripeLeftWithBlur(Stripe stripe, float minVal = 0f, int blur = 30)
		{
			float num = stripe.arr[stripe.length - 1];
			float num2 = stripe.arr[stripe.length - 1];
			int num3 = 0;
			float num4 = 1f / (float)blur;
			float num5 = 1f - num4;
			for (int num6 = stripe.length - 2; num6 >= 0; num6--)
			{
				float num7 = stripe.arr[num6];
				if (num7 > minVal)
				{
					num = num7;
					num2 = ((num3 == 0) ? (num2 * num5 + num7 * num4) : num);
					num3 = 0;
				}
				else
				{
					if (num3 > blur)
					{
						num7 = num2;
					}
					else
					{
						float num8 = 1f * (float)num3 / (float)blur;
						num7 = num * (1f - num8) + num2 * num8;
					}
					stripe.arr[num6] = num7;
					num3++;
				}
			}
		}

		public static void StripeToMask(Stripe stripe, Stripe mask, float minVal = 0f)
		{
			for (int i = 0; i < stripe.length; i++)
			{
				mask.arr[i] = ((stripe.arr[i] > minVal) ? 1 : 0);
			}
		}

		public static void BlendStripes(Stripe leftStripe, Stripe rightStripe, Stripe leftMask, Stripe rightMask)
		{
			for (int i = 0; i < leftStripe.length; i++)
			{
				float num = leftMask.arr[i];
				float num2 = rightMask.arr[i];
				float num3 = num + num2;
				leftStripe.arr[i] = ((num3 > 0f) ? ((leftStripe.arr[i] * num + rightStripe.arr[i] * num2) / num3) : 0f);
				leftMask.arr[i] = num3 / 2f;
			}
		}

		public static void MaxStripes(Stripe leftStripe, Stripe rightStripe, Stripe leftMask, Stripe rightMask)
		{
			for (int i = 0; i < leftStripe.length; i++)
			{
				_ = leftMask.arr[i];
				float num = rightMask.arr[i];
				leftStripe.arr[i] = ((leftMask.arr[i] > rightMask.arr[i]) ? leftStripe.arr[i] : rightStripe.arr[i]);
				leftMask.arr[i] = (leftMask.arr[i] + rightMask.arr[i]) / 2f;
			}
		}

		public static void PaddingBac(Matrix matrix, Matrix mask, float sharpness = 0.25f, int iterations = 2)
		{
			float multiply = 1f - sharpness;
			Stripe stripe = new Stripe(Mathf.Max(matrix.rect.size.x, matrix.rect.size.z));
			Stripe stripe2 = new Stripe(stripe.length);
			Stripe stripe3 = new Stripe(stripe.length);
			Stripe stripe4 = new Stripe(stripe.length);
			Coord min = matrix.rect.Min;
			Coord max = matrix.rect.Max;
			for (int i = 0; i < iterations; i++)
			{
				stripe.length = matrix.rect.size.x;
				stripe2.length = stripe.length;
				stripe3.length = stripe.length;
				stripe4.length = stripe.length;
				for (int j = min.z; j < max.z; j++)
				{
					ReadLine(stripe3, matrix, matrix.rect.offset.x, j);
					ReadLine(stripe, mask, matrix.rect.offset.x, j);
					Stripe.Copy(stripe3, stripe4);
					Stripe.Copy(stripe, stripe2);
					PadLeft(stripe3, stripe, multiply);
					PadRight(stripe4, stripe2, multiply);
					for (int k = 0; k < stripe3.length; k++)
					{
						float num = stripe.arr[k] + stripe2.arr[k];
						stripe3.arr[k] = ((num > 0f) ? ((stripe3.arr[k] * stripe.arr[k] + stripe4.arr[k] * stripe2.arr[k]) / num) : 0f);
						stripe.arr[k] = num / 2f;
					}
					WriteLine(stripe3, matrix, matrix.rect.offset.x, j);
					WriteLine(stripe, mask, mask.rect.offset.x, j);
				}
				stripe.length = matrix.rect.size.z;
				stripe2.length = stripe.length;
				stripe3.length = stripe.length;
				stripe4.length = stripe.length;
				for (int l = min.x; l < max.x; l++)
				{
					ReadRow(stripe3, matrix, l, matrix.rect.offset.z);
					ReadRow(stripe, mask, l, matrix.rect.offset.z);
					Stripe.Copy(stripe3, stripe4);
					Stripe.Copy(stripe, stripe2);
					PadLeft(stripe3, stripe, multiply);
					PadRight(stripe4, stripe2, multiply);
					for (int m = 0; m < stripe3.length; m++)
					{
						float num2 = stripe.arr[m] + stripe2.arr[m];
						stripe3.arr[m] = ((num2 > 0f) ? ((stripe3.arr[m] * stripe.arr[m] + stripe4.arr[m] * stripe2.arr[m]) / num2) : 0f);
						stripe.arr[m] = num2 / 2f;
					}
					WriteRow(stripe3, matrix, l, matrix.rect.offset.z);
					WriteRow(stripe, mask, l, mask.rect.offset.z);
				}
			}
		}

		private static void PadLeft(Stripe stripe, Stripe mask, float multiply = 0.9f)
		{
			float num = stripe.arr[stripe.length - 1];
			float num2 = mask.arr[stripe.length - 1];
			for (int num3 = stripe.length - 2; num3 >= 0; num3--)
			{
				float num4 = mask.arr[num3];
				float num5 = stripe.arr[num3];
				float num6 = num4 + num2;
				num5 = ((num6 > 0f) ? ((num5 * num4 + num * num2) / num6) : 0f);
				stripe.arr[num3] = num5;
				if (num2 > num4)
				{
					num4 = num2 * multiply;
					mask.arr[num3] = num4;
				}
				num = num5;
				num2 = num4;
			}
		}

		private static void PadRight(Stripe stripe, Stripe mask, float multiply = 0.9f)
		{
			float num = stripe.arr[0];
			float num2 = mask.arr[0];
			for (int i = 1; i < stripe.length - 1; i++)
			{
				float num3 = mask.arr[i];
				float num4 = stripe.arr[i];
				float num5 = num3 + num2;
				num4 = ((num5 > 0f) ? ((num4 * num3 + num * num2) / num5) : 0f);
				stripe.arr[i] = num4;
				if (num2 > num3)
				{
					num3 = num2 * multiply;
					mask.arr[i] = num3;
				}
				num = num4;
				num2 = num3;
			}
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PredictPadding")]
		public static extern void PredictPadding(Matrix src, Matrix dst, float expandEdge = 0.1f, int expandPixels = 50);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PadStripe")]
		public static extern void PadStripe(Stripe leftStripe, Stripe rightStripe, Stripe leftMask, Stripe rightMask, float expandEdge = 0.1f, int expandPixels = 50);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PredictPadStripeLeft")]
		public static extern void PredictPadStripeLeft(Stripe stripe, float expandEdge = 0.1f, int expandPixels = 50);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PredictPadStripeRight")]
		public static extern void PredictPadStripeRight(Stripe stripe, float expandEdge = 0.1f, int expandPixels = 50);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PadStripeLeft")]
		public static extern void PadStripeLeft(Stripe stripe);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_PadStripeRight")]
		public static extern void PadStripeRight(Stripe stripe);

		public static void ExtendCircular(this Matrix matrix, Coord center, int radius, int extendRange, int expandPixels = 0)
		{
			RemoveOuter(matrix, center, radius);
			int num = Mathf.CeilToInt((float)Math.PI * (float)radius);
			float num2 = (float)Math.PI * 2f / (float)(num - 1);
			Stripe stripe = new Stripe(extendRange * 2);
			Stripe stripe2 = new Stripe(extendRange * 2);
			for (int i = 0; i < extendRange; i++)
			{
				stripe2.arr[i] = 1f;
			}
			for (int j = 0; j < num; j++)
			{
				float num3 = (float)j * num2;
				num3 -= (float)Math.PI * 3f / 4f;
				Vector2 normalized = new Vector2(Mathf.Sin(num3), Mathf.Cos(num3)).normalized;
				Vector2 vector = new Vector2((normalized.x > 0f) ? normalized.x : (0f - normalized.x), (normalized.y > 0f) ? normalized.y : (0f - normalized.y));
				float num4 = ((vector.x > vector.y) ? vector.x : vector.y);
				Vector2 vector2 = normalized / num4;
				Vector2 start = center.vector2 + normalized * radius;
				start -= vector2 * extendRange;
				ReadInclined(stripe, matrix, start, vector2);
				PredictPadStripeLeft(stripe, 0.1f, expandPixels);
				WriteInclined(stripe, matrix, start, vector2);
			}
			Vector2 step = new Vector2(-1f, -1f);
			Vector2 start2 = center.vector2 + step.normalized * radius;
			stripe = new Stripe(extendRange * 2);
			ReadInclined(stripe, matrix, start2, step);
			PredictPadStripeLeft(stripe, 0.1f, expandPixels);
			WriteInclined(stripe, matrix, start2, step);
			Stripe stripe3 = new Stripe((radius + extendRange + 1) * 2 * 4 + 1);
			stripe3.Fill(-1f);
			Stripe stripe4 = new Stripe(stripe3.length);
			stripe4.Fill(-1f);
			Stripe leftMask = new Stripe(stripe3.length);
			Stripe rightMask = new Stripe(stripe3.length);
			for (int k = (int)((float)radius * 0.7f); k < radius + extendRange; k++)
			{
				ReadSquare(stripe3, matrix, center, k);
				PadStripe(stripe3, stripe4, leftMask, rightMask, 0f, 0);
				WriteSquare(stripe3, matrix, center, k);
			}
			BlurCircular(matrix, center, radius + extendRange, extendRange);
			DownsampleBlurCircular(matrix, center, radius + (int)((float)extendRange * 0.05f), (int)((float)extendRange * 0.95f + 1f), 2, 4f);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_RemoveOuter")]
		public static extern void RemoveOuter(Matrix matrix, Coord coord, int radius);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_BlurCircular")]
		public static extern void BlurCircular(Matrix matrix, Coord coord, int radius, int extendRange);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MatrixOps_DownsampleBlurCircular")]
		public static extern void DownsampleBlurCircular(Matrix matrix, Coord coord, int radius, int extendRange, int downsample, float blur);
	}
}
