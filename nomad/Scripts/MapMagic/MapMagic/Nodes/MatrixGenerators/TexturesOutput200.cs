using System;
using System.Collections.Generic;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using MapMagic.Terrains;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "Textures", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/TexturesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Textures")]
	public class TexturesOutput200 : BaseTexturesOutput<TexturesOutput200.TextureLayer>
	{
		public class TextureLayer : BaseTextureLayer
		{
			[Val("Layer", "Layer")]
			public TerrainLayer prototype;

			public Color color = new Color(0.75f, 0.75f, 0.75f, 1f);

			public bool guiProperties;

			public bool guiRemapping;

			public bool guiTileSettings;
		}

		public class ApplyData : IApplyData
		{
			public float[,,] splats;

			public TerrainLayer[] prototypes;

			public static ApplyData Empty => new ApplyData
			{
				splats = new float[64, 64, 0],
				prototypes = new TerrainLayer[0]
			};

			public int Resolution
			{
				get
				{
					if (splats == null)
					{
						return 0;
					}
					return splats.GetLength(0);
				}
			}

			public virtual void Apply(Terrain terrain)
			{
				if (!(terrain == null) && !terrain.Equals(null) && !(terrain.terrainData == null))
				{
					TerrainData terrainData = terrain.terrainData;
					int length = splats.GetLength(0);
					if (terrainData.alphamapResolution != length)
					{
						terrainData.alphamapResolution = length;
					}
					terrain.terrainData.terrainLayers = prototypes;
					terrain.terrainData.SetAlphamaps(0, 0, splats);
				}
			}
		}

		[SerializeField]
		public int guiExpanded;

		public static FinalizeAction finalizeAction = Finalize;

		public override FinalizeAction FinalizeAction => finalizeAction;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\TexturesOut.cs", 115);
		}

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
					data.StoreOutput(layers[i], typeof(TexturesOutput200), layers[i].prototype, array[i]);
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
			if (stop != null && stop.stop)
			{
				return;
			}
			data.GatherOutputs<TerrainLayer, MatrixWorld>(typeof(TexturesOutput200), out var prototypes, out var products, out var masks, inSubs: true);
			if (stop == null || !stop.stop)
			{
				float[,,] splats = BlendLayers(products, masks, data.area, null, stop);
				if (stop == null || !stop.stop)
				{
					ApplyData applyData = new ApplyData
					{
						splats = splats,
						prototypes = prototypes
					};
					Graph.OnOutputFinalized?.Invoke(typeof(TexturesOutput200), data, applyData, stop);
					data.MarkApply(applyData);
				}
			}
		}

		public static float[,,] BlendLayers(IList<Matrix> matrices, IList<Matrix> masks, Area area, IList<int> channelNumbers = null, StopToken stop = null)
		{
			_ = area.full.rect;
			int x = area.active.rect.size.x;
			int margins = area.Margins;
			int num2;
			if (channelNumbers != null)
			{
				int num = 0;
				foreach (int channelNumber in channelNumbers)
				{
					if (channelNumber > num)
					{
						num = channelNumber;
					}
				}
				num2 = num + 1;
			}
			else
			{
				num2 = matrices.Count;
			}
			float[,,] array = new float[x, x, num2];
			for (int i = 0; i < x; i++)
			{
				if (stop != null && stop.stop)
				{
					return null;
				}
				for (int j = 0; j < x; j++)
				{
					int pos = area.full.rect.GetPos(i + area.full.rect.offset.x + margins, j + area.full.rect.offset.z + margins);
					float num3 = 0f;
					for (int k = 0; k < num2; k++)
					{
						float num4 = ((matrices[k] != null) ? matrices[k].arr[pos] : 0f);
						num4 *= ((masks[k] == null) ? 1f : masks[k].arr[pos]);
						num3 += num4;
					}
					if (num3 == 0f)
					{
						continue;
					}
					for (int l = 0; l < num2; l++)
					{
						float num5 = ((matrices[l] != null) ? matrices[l].arr[pos] : 0f);
						num5 *= ((masks[l] == null) ? 1f : masks[l].arr[pos]);
						num5 /= num3;
						if (num5 < 0f)
						{
							num5 = 0f;
						}
						if (num5 > 1f)
						{
							num5 = 1f;
						}
						int num6 = channelNumbers?[l] ?? l;
						array[j, i, num6] += num5;
					}
				}
			}
			return array;
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
			TerrainData terrainData = terrain.terrainData;
			terrainData.terrainLayers = new TerrainLayer[0];
			terrainData.alphamapResolution = 32;
		}
	}
}
