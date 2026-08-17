using System;
using System.Runtime.InteropServices;
using Den.Tools.Matrices;

namespace Den.Tools
{
	public static class Erosion
	{
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public class MatrixInt
		{
			public CoordRect rect;

			public int count;

			public int pos;

			public int[] arr;

			public static implicit operator MatrixInt(Matrix2D<int> src)
			{
				return new MatrixInt(src);
			}

			public MatrixInt(Matrix2D<int> src)
			{
				rect = src.rect;
				count = src.count;
				pos = src.pos;
				arr = src.arr;
			}
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetOrder(Matrix refm, MatrixInt order);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl)]
		public static extern void MaskBorders(MatrixInt order);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl)]
		public static extern void CreateTorrents(Matrix heights, MatrixInt order, Matrix torrents);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl)]
		public static extern void Erode(Matrix heights, Matrix torrents, Matrix mudflow, MatrixInt order, float erosionDurability = 0.9f, float erosionAmount = 1f, float sedimentAmount = 0.5f);

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl)]
		public static extern void TransferSettleMudflow(Matrix heights, Matrix mudflow, Matrix sediments, MatrixInt order, int erosionFluidityIterations = 3);
	}
}
