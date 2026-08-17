using System;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Initial", name = "Constant", iconName = "GeneratorIcons/Constant", disengageable = true, codeFile = "MatrixInitial", codeLine = 17, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Constant")]
	public class Constant200 : Generator, IOutlet<MatrixWorld>, IUnit
	{
		[Val("Level", 0f)]
		public float level;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 21);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			matrixWorld.Fill(level);
			data.StoreProduct(this, matrixWorld);
		}
	}
}
