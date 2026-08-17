using System;
using System.Runtime.InteropServices;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Erosion", iconName = "GeneratorIcons/Erosion", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Erision")]
	public class Erosion200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>, ICustomComplexity
	{
		[Val("Iterations")]
		public int iterations = 3;

		[Val("Durability")]
		public float terrainDurability = 0.9f;

		public float erosionAmount = 1f;

		[Val("Sediment")]
		public float sedimentAmount = 0.75f;

		[Val("Fluidity")]
		public int fluidityIterations = 3;

		[Val("Relax")]
		public float relax;

		public float Complexity => iterations * 2;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 1083);
		}

		[DllImport("NativePlugins")]
		private static extern void SetOrder(float[] refArr, int[] orderArr, int length);

		[DllImport("NativePlugins")]
		private static extern int MaskBorders(int[] orderArr, CoordRect matrixRect);

		[DllImport("NativePlugins")]
		private static extern int CreateTorrents(float[] heights, int[] order, float[] torrents, CoordRect matrixRect);

		[DllImport("NativePlugins")]
		private static extern int Erode(float[] heights, float[] torrents, float[] mudflow, int[] order, CoordRect matrixRect, float erosionDurability = 0.9f, float erosionAmount = 1f, float sedimentAmount = 0.5f);

		[DllImport("NativePlugins")]
		private static extern int TransferSettleMudflow(float[] heights, float[] mudflow, float[] sediments, int[] order, CoordRect matrixRect, int erosionFluidityIterations = 3);

		public float Progress(TileData data)
		{
			return data.GetProgress(this);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld == null)
			{
				return;
			}
			if (!enabled || iterations <= 0)
			{
				data.StoreProduct(this, matrixWorld);
				return;
			}
			MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld);
			Erosion(matrixWorld2, data.isDraft, data, iterations, terrainDurability, erosionAmount, sedimentAmount, fluidityIterations, relax, this, stop);
			if (stop == null || !stop.stop)
			{
				data.StoreProduct(this, matrixWorld2);
			}
		}

		public static void Erosion(MatrixWorld dstHeight, bool isDraft, TileData data, int iterations, float terrainDurability, float erosionAmount, float sedimentAmount, int fluidityIterations, float relax, ICustomComplexity thisGen, StopToken stop = null)
		{
			Matrix2D<int> matrix2D = new Matrix2D<int>(dstHeight.rect);
			Matrix torrents = new Matrix(dstHeight.rect);
			Matrix mudflow = new Matrix(dstHeight.rect);
			Matrix sediments = new Matrix(dstHeight.rect);
			int num = iterations;
			int erosionFluidityIterations = fluidityIterations;
			if (isDraft)
			{
				num = iterations / 3;
				erosionFluidityIterations = fluidityIterations / 3;
			}
			for (int i = 0; i < num; i++)
			{
				Den.Tools.Erosion.SetOrder(dstHeight, matrix2D);
				if (stop != null && stop.stop)
				{
					break;
				}
				Den.Tools.Erosion.MaskBorders(matrix2D);
				if (stop != null && stop.stop)
				{
					break;
				}
				Den.Tools.Erosion.CreateTorrents(dstHeight, matrix2D, torrents);
				if (stop != null && stop.stop)
				{
					break;
				}
				Den.Tools.Erosion.Erode(dstHeight, torrents, mudflow, matrix2D, terrainDurability, erosionAmount, sedimentAmount);
				if (stop != null && stop.stop)
				{
					break;
				}
				Den.Tools.Erosion.TransferSettleMudflow(dstHeight, mudflow, sediments, matrix2D, erosionFluidityIterations);
				if (stop != null && stop.stop)
				{
					break;
				}
				if (relax > 0.0001f && i != num - 1)
				{
					MatrixOps.GaussianBlur(dstHeight, relax / (float)(i + 1));
				}
				if (stop != null && stop.stop)
				{
					break;
				}
				data.SetProgress(thisGen, i * 2);
			}
		}
	}
}
