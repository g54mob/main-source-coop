using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Initial", name = "Spot", section = 2, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/Spot", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Spot")]
	public class Spot210 : Generator, IOutlet<MatrixWorld>, IUnit
	{
		[Val("Intensity")]
		public float intensity = 1f;

		[Val("Position")]
		public Vector2D position;

		[Val("Radius")]
		public float radius = 30f;

		[Val("Hardness")]
		public float hardness = 0.5f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 487);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop == null || !stop.stop)
			{
				MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
				float x = matrixWorld.PixelSize.x;
				Vector2D pos = position / x;
				float num = radius / x;
				matrixWorld.Stroke(pos, num, hardness);
				if (intensity < 0.999f || intensity > 1.001f)
				{
					matrixWorld.Multiply(intensity);
				}
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, matrixWorld);
				}
			}
		}
	}
}
