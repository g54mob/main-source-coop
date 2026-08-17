using System;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Levels", iconName = "GeneratorIcons/Levels", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Levels")]
	public class Levels200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		public float inMin;

		public float inMax = 1f;

		public float gamma = 1f;

		public float outMin;

		public float outMax = 1f;

		[NonSerialized]
		public float[] histogram;

		public const int histogramSize = 256;

		public bool guiParams;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 103);
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
				if (stop != null && stop.stop)
				{
					return;
				}
				if (data.isPreview)
				{
					histogram = matrixWorld.Histogram(256);
				}
				if (stop != null && stop.stop)
				{
					return;
				}
				MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld);
				if (stop == null || !stop.stop)
				{
					matrixWorld2.Levels(inMin, inMax, gamma, outMin, outMax);
					if (stop == null || !stop.stop)
					{
						data.StoreProduct(this, matrixWorld2);
					}
				}
			}
		}
	}
}
