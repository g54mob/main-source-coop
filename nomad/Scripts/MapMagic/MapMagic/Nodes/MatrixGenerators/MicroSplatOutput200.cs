using System;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "MicroSplat", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/TexturesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/output_generators/Textures")]
	public class MicroSplatOutput200 : BaseTexturesOutput<MicroSplatOutput200.MicroSplatLayer>
	{
		public class MicroSplatLayer : BaseTextureLayer
		{
			[NonSerialized]
			public TerrainLayer prototype;
		}

		public static FinalizeAction finalizeAction = Finalize;

		public override FinalizeAction FinalizeAction => finalizeAction;

		public override void Generate(TileData data, StopToken stop)
		{
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}

		public static void Finalize(TileData data, StopToken stop)
		{
		}
	}
}
