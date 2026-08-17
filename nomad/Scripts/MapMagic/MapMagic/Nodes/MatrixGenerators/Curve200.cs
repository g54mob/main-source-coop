using System;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Curve", iconName = "GeneratorIcons/Curve", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Curve")]
	public class Curve200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		public Curve curve = new Curve(new Vector2(0f, 0f), new Vector2(1f, 1f));

		[NonSerialized]
		public float[] histogram;

		public const int histogramSize = 256;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 21);
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
				if (stop != null && stop.stop)
				{
					return;
				}
				curve.Refresh();
				if (stop == null || !stop.stop)
				{
					matrixWorld2.UniformCurve(curve.lut);
					if (stop == null || !stop.stop)
					{
						data.StoreProduct(this, matrixWorld2);
					}
				}
			}
		}
	}
}
