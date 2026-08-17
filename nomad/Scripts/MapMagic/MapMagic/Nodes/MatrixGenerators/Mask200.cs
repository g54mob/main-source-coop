using System;
using System.Collections.Generic;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Mask", iconName = "GeneratorIcons/MapMask", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/MapMask")]
	public class Mask200 : Generator, IMultiInlet, IOutlet<MatrixWorld>, IUnit
	{
		[Val("Input A", "Inlet")]
		public readonly Inlet<MatrixWorld> aIn = new Inlet<MatrixWorld>();

		[Val("Input B", "Inlet")]
		public readonly Inlet<MatrixWorld> bIn = new Inlet<MatrixWorld>();

		[Val("Mask", "Inlet")]
		public readonly Inlet<MatrixWorld> maskIn = new Inlet<MatrixWorld>();

		[Val("Invert")]
		public bool invert;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 221);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return aIn;
			yield return bIn;
			yield return maskIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			MatrixWorld matrixWorld = data.ReadInletProduct(aIn);
			MatrixWorld matrixWorld2 = data.ReadInletProduct(bIn);
			MatrixWorld matrixWorld3 = data.ReadInletProduct(maskIn);
			if (matrixWorld == null || matrixWorld2 == null)
			{
				return;
			}
			if (!enabled || matrixWorld3 == null)
			{
				data.StoreProduct(this, matrixWorld);
			}
			else
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				MatrixWorld matrixWorld4 = new MatrixWorld(matrixWorld);
				if (stop == null || !stop.stop)
				{
					matrixWorld4.Mix(matrixWorld2, matrixWorld3, 0f, 1f, invert, fallof: false, 1f);
					if (stop == null || !stop.stop)
					{
						data.StoreProduct(this, matrixWorld4);
					}
				}
			}
		}
	}
}
