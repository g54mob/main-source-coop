using System;
using System.Collections.Generic;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Sediment", iconName = "GeneratorIcons/Sediment", disengageable = true, colorType = typeof(MatrixWorld), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Sediment")]
	public class Sediment210 : Generator, IMultiInlet, IMultiOutlet
	{
		[Val("Original", "Outlet")]
		public readonly Inlet<MatrixWorld> origIn = new Inlet<MatrixWorld>();

		[Val("Eroded", "Outlet")]
		public readonly Inlet<MatrixWorld> erodedIn = new Inlet<MatrixWorld>();

		[Val("Cliff", "Outlet")]
		public readonly Outlet<MatrixWorld> cliffOut = new Outlet<MatrixWorld>();

		[Val("Sediment", "Outlet")]
		public readonly Outlet<MatrixWorld> sedimentOut = new Outlet<MatrixWorld>();

		[Val("Cliff")]
		public float cliffIntensity = 1f;

		[Val("Sediment")]
		public float sedimentIntensity = 1f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 1183);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return origIn;
			yield return erodedIn;
		}

		public IEnumerable<IOutlet<object>> Outlets()
		{
			yield return cliffOut;
			yield return sedimentOut;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = data.ReadInletProduct(origIn);
			MatrixWorld matrixWorld2 = data.ReadInletProduct(erodedIn);
			if (matrixWorld == null || matrixWorld2 == null)
			{
				return;
			}
			MatrixWorld matrixWorld3 = new MatrixWorld(matrixWorld.rect, matrixWorld.worldPos, matrixWorld.worldSize);
			MatrixWorld matrixWorld4 = new MatrixWorld(matrixWorld.rect, matrixWorld.worldPos, matrixWorld.worldSize);
			if (stop == null || !stop.stop)
			{
				CliffSediment(matrixWorld, matrixWorld2, matrixWorld3, matrixWorld4);
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(cliffOut, matrixWorld3);
					data.StoreProduct(sedimentOut, matrixWorld4);
				}
			}
		}

		public void CliffSediment(MatrixWorld orig, MatrixWorld eroded, MatrixWorld cliff, MatrixWorld sediment)
		{
			MatrixOps.Delta(orig, cliff);
			MatrixOps.Delta(eroded, sediment);
			for (int i = 0; i < orig.count; i++)
			{
				float num = orig.arr[i] - eroded.arr[i];
				if (num < 0f)
				{
					num = 0f;
				}
				float num2 = sediment.arr[i] - cliff.arr[i];
				if (num2 > 0f)
				{
					cliff.arr[i] = num2 * 1000f * cliffIntensity / orig.PixelSize.x;
					sediment.arr[i] = 0f;
				}
				else
				{
					sediment.arr[i] = num * 1000f * sedimentIntensity / orig.PixelSize.x;
					cliff.arr[i] = 0f;
				}
			}
		}
	}
}
