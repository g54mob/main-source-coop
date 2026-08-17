using System;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "CTS", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/TexturesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/output_generators/Textures")]
	public class CTSOutput200 : BaseTexturesOutput<CTSOutput200.CTSLayer>
	{
		public class CTSLayer : BaseTextureLayer
		{
		}

		public string[] guiTextureNames;

		public static FinalizeAction finalizeAction = Finalize;

		public override FinalizeAction FinalizeAction => finalizeAction;

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld[] array = BaseGenerate(data, stop);
			if (stop != null && stop.stop)
			{
				return;
			}
			if (enabled)
			{
				for (int i = 0; i < layers.Length; i++)
				{
					data.StoreOutput(layers[i], typeof(CTSOutput200), layers[i], array[i]);
				}
				data.MarkFinalize(Finalize, stop);
			}
			else
			{
				data.RemoveFinalize(finalizeAction);
			}
		}

		public static void Finalize(TileData data, StopToken stop)
		{
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}
	}
}
