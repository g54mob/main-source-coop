using System;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "RTP", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/TexturesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/output_generators/Textures")]
	public class RTPOutput200 : BaseTexturesOutput<RTPOutput200.RTPLayer>
	{
		public class RTPLayer : BaseTextureLayer
		{
		}

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
					data.StoreOutput(layers[i], typeof(RTPOutput200), layers[i], array[i]);
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
			if (data.OutputsCount(typeof(RTPOutput200), inSubs: true) == 0)
			{
				if (stop == null || !stop.stop)
				{
					data.MarkApply(CustomShaderOutput200.ApplyData.Empty);
				}
				return;
			}
			Color[][] array = null;
			if (stop == null || !stop.stop)
			{
				CustomShaderOutput200.ApplyData applyData = new CustomShaderOutput200.ApplyData
				{
					textureColors = array,
					textureFormat = TextureFormat.RGBA32,
					textureBaseMapDistance = 10000000f,
					textureNames = new string[(array != null) ? array.Length : 0]
				};
				for (int i = 0; i < applyData.textureNames.Length; i++)
				{
					applyData.textureNames[i] = "_Control" + (i + 1);
				}
				Graph.OnOutputFinalized?.Invoke(typeof(RTPOutput200), data, applyData, stop);
				data.MarkApply(applyData);
			}
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}
	}
}
