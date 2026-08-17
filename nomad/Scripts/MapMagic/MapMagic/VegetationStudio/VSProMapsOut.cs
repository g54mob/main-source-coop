using System;
using Den.Tools.Matrices;
using MapMagic.Nodes;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.VegetationStudio
{
	[Serializable]
	[GeneratorMenu(name = "VS Pro Maps", section = 2, drawButtons = false, colorType = typeof(MatrixWorld))]
	public class VSProMapsOut : OutputGenerator, IInlet<MatrixWorld>, IUnit
	{
		public OutputLevel outputLevel = OutputLevel.Main;

		public float density = 0.5f;

		public int maskGroup;

		public int textureChannel;

		public override OutputLevel OutputLevel => outputLevel;

		public override void Generate(TileData data, StopToken stop)
		{
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}
	}
}
