using System;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Products;
using MapMagic.Terrains;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "DirectMatrices", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/TexturesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Textures")]
	public class DirectMatricesOutput200 : BaseTexturesOutput<DirectMatricesOutput200.DirectMatricesLayer>
	{
		public class DirectMatricesLayer : BaseTextureLayer
		{
		}

		public class ApplyData : IApplyData
		{
			public DictionaryOrdered<string, MatrixWorld> dict;

			public static ApplyData Empty => new ApplyData
			{
				dict = new DictionaryOrdered<string, MatrixWorld>()
			};

			public int Resolution
			{
				get
				{
					if (dict.Count == 0)
					{
						return 0;
					}
					return dict[0].rect.size.x;
				}
			}

			public virtual void Apply(Terrain terrain)
			{
				DirectMatricesHolder directMatricesHolder = terrain.GetComponent<DirectMatricesHolder>();
				if (directMatricesHolder == null)
				{
					directMatricesHolder = terrain.gameObject.AddComponent<DirectMatricesHolder>();
				}
				directMatricesHolder.maps = dict;
			}
		}

		public static FinalizeAction finalizeAction = Finalize;

		public override bool HideFirst => false;

		public override FinalizeAction FinalizeAction => finalizeAction;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\TexturesOut.cs", 758);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld[] array = new MatrixWorld[layers.Length];
			for (int i = 0; i < layers.Length; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				array[i] = data.ReadInletProduct(layers[i]);
			}
			if (stop != null && stop.stop)
			{
				return;
			}
			if (enabled)
			{
				for (int j = 0; j < layers.Length; j++)
				{
					data.StoreOutput(layers[j], typeof(DirectMatricesOutput200), layers[j].name, array[j]);
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
			data.GatherOutputs<string, MatrixWorld>(typeof(DirectMatricesOutput200), out var prototypes, out var products, out var masks, inSubs: true);
			if (products.Length == 0)
			{
				if (stop == null || !stop.stop)
				{
					data.MarkApply(ApplyData.Empty);
				}
				return;
			}
			DictionaryOrdered<string, MatrixWorld> dictionaryOrdered = new DictionaryOrdered<string, MatrixWorld>();
			for (int i = 0; i < products.Length; i++)
			{
				if (products[i] != null)
				{
					MatrixWorld matrixWorld = new MatrixWorld(products[i]);
					if (masks[i] != null)
					{
						matrixWorld.Multiply(masks[i]);
					}
					matrixWorld.Crop(data.area.active.rect);
					_ = matrixWorld.PixelSize;
					matrixWorld.worldPos = (Vector3)data.area.active.worldPos;
					matrixWorld.worldSize = (Vector3)data.area.active.worldSize;
					matrixWorld.worldSize.y = products[i].worldSize.y;
					if (dictionaryOrdered.ContainsKey(prototypes[i]))
					{
						dictionaryOrdered[prototypes[i]] = matrixWorld;
					}
					else
					{
						dictionaryOrdered.Add(prototypes[i], matrixWorld);
					}
				}
			}
			if (stop == null || !stop.stop)
			{
				ApplyData applyData = new ApplyData
				{
					dict = dictionaryOrdered
				};
				Graph.OnOutputFinalized?.Invoke(typeof(DirectMatricesOutput200), data, applyData, stop);
				data.MarkApply(applyData);
			}
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}
	}
}
