using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Beach", iconName = "GeneratorIcons/Beach", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Beach")]
	public class Beach210 : Generator, IInlet<MatrixWorld>, IUnit, IMultiInlet, IOutlet<MatrixWorld>, IMultiOutlet
	{
		[Val("Level")]
		public float level = 50f;

		[Val("Contour Relax")]
		public float relax = 100f;

		[Val("Size")]
		public float size = 40f;

		[Val("Height")]
		public float height = 5f;

		[Val("Sand Tex Blur")]
		public float sandBlur = 5f;

		[Val("Mask", "Inlet")]
		public readonly Inlet<MatrixWorld> beachMaskIn = new Inlet<MatrixWorld>();

		[Val("Sand", "Inlet")]
		public readonly Outlet<MatrixWorld> sandMaskOut = new Outlet<MatrixWorld>();

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 917);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return beachMaskIn;
		}

		public IEnumerable<IOutlet<object>> Outlets()
		{
			yield return sandMaskOut;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, matrixWorld);
				return;
			}
			MatrixWorld mask = data.ReadInletProduct(beachMaskIn);
			if (stop != null && stop.stop)
			{
				return;
			}
			Matrix shoreSpread = PrepareShoreMask(matrixWorld, level / data.globals.height, size / data.area.PixelSize.x, height / data.globals.height, relax, stop);
			if (stop != null && stop.stop)
			{
				return;
			}
			MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld.rect, matrixWorld.worldPos, matrixWorld.worldSize);
			BeachHeight(matrixWorld, matrixWorld2, shoreSpread, mask, level / data.globals.height, height / data.globals.height);
			if (stop != null && stop.stop)
			{
				return;
			}
			MatrixWorld matrixWorld3 = new MatrixWorld(matrixWorld.rect, matrixWorld.worldPos, matrixWorld.worldSize);
			BeachSand(matrixWorld, matrixWorld2, matrixWorld3, level / data.globals.height, sandBlur / data.globals.height);
			if (stop == null || !stop.stop)
			{
				matrixWorld2.Max(matrixWorld);
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, matrixWorld2);
					data.StoreProduct(sandMaskOut, matrixWorld3);
				}
			}
		}

		public static Matrix PrepareShoreMask(Matrix src, float level, float size, float height, float relax, StopToken stop)
		{
			Matrix matrix = new Matrix(src);
			matrix.Select(level + height / 2f);
			Matrix matrix2 = new Matrix(matrix.rect);
			if (stop != null && stop.stop)
			{
				return null;
			}
			matrix.InvertOne();
			MatrixOps.SpreadLinear(matrix, matrix2, 1f / relax, diagonals: true, quarters: true);
			matrix2.Select(0.01f);
			matrix2.InvertOne();
			if (stop != null && stop.stop)
			{
				return null;
			}
			MatrixOps.GaussianBlur(matrix2, 3.75f);
			matrix2.Select(0.5f);
			if (stop != null && stop.stop)
			{
				return null;
			}
			matrix.Fill(0f);
			MatrixOps.SpreadLinear(matrix2, matrix, 1f / relax, diagonals: true, quarters: true);
			matrix.Select(0.01f);
			if (stop != null && stop.stop)
			{
				return null;
			}
			Matrix matrix3 = matrix2;
			matrix3.Fill(0f);
			MatrixOps.SpreadLinear(matrix, matrix3, 1f / size, diagonals: true, quarters: true);
			matrix3.Clamp01();
			if (stop != null && stop.stop)
			{
				return null;
			}
			MatrixOps.GaussianBlur(matrix3, 3.75f);
			return matrix3;
		}

		public static void BeachHeight(Matrix src, Matrix dst, Matrix shoreSpread, Matrix mask, float level, float height)
		{
			for (int i = 0; i < dst.count; i++)
			{
				float num = src.arr[i];
				float num2 = shoreSpread.arr[i];
				float num3 = ((mask != null) ? (1f - (1f - mask.arr[i]) * (1f - mask.arr[i])) : 1f);
				if (num2 > 0.999f)
				{
					dst.arr[i] = level + height / 2f;
				}
				if (num2 < 0.0001f)
				{
					dst.arr[i] = src.arr[i];
				}
				float num4 = num2 * num3;
				float num5 = level - height / 2f + num4 * height;
				float num6 = (0.5f - num4) * 2f;
				if (num6 < 0f)
				{
					num6 = 0f;
				}
				num6 = 3f * num6 * num6 - 2f * num6 * num6 * num6;
				num5 = num5 * (1f - num6) + num * num6;
				dst.arr[i] = num5;
			}
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GeneratorBeachSand210")]
		private static extern void BeachSand(Matrix heights, Matrix beach, Matrix sand, float waterLevel, float maxDelta);
	}
}
