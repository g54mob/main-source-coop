using System;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Contrast", iconName = "GeneratorIcons/Contrast", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Contrast")]
	public class Contrast200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		[Val(name = "Intensity")]
		public float brightness;

		[Val(name = "Contrast")]
		public float contrast = 1f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 147);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, matrixWorld);
			}
			else
			{
				if ((stop != null && stop.stop) || (stop != null && stop.stop))
				{
					return;
				}
				MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld);
				if (stop == null || !stop.stop)
				{
					matrixWorld2.BrighnesContrast(brightness, contrast);
					if (stop == null || !stop.stop)
					{
						data.StoreProduct(this, matrixWorld2);
					}
				}
			}
		}
	}
}
