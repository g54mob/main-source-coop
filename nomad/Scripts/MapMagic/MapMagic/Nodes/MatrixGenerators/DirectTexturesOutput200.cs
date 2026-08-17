using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Products;
using MapMagic.Terrains;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "Direct Textures", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/TexturesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Textures")]
	public class DirectTexturesOutput200 : BaseTexturesOutput<DirectTexturesOutput200.DirectTexturesLayer>
	{
		public class DirectTexturesLayer : BaseTextureLayer
		{
		}

		public class ApplyData : IApplyData
		{
			public Color[][] textureColors;

			public string[] textureNames;

			public TextureFormat textureFormat;

			public static ApplyData Empty => new ApplyData
			{
				textureColors = new Color[0][],
				textureNames = new string[0]
			};

			public int Resolution
			{
				get
				{
					if (textureColors.Length == 0)
					{
						return 0;
					}
					return (int)Mathf.Sqrt(textureColors[0].Length);
				}
			}

			public virtual void Apply(Terrain terrain)
			{
				if (textureColors == null || textureColors.Length == 0 || textureColors.AllNull())
				{
					return;
				}
				int resolution = (int)Mathf.Sqrt(textureColors.Any().Length);
				DirectTexturesHolder directTexturesHolder = terrain.GetComponent<DirectTexturesHolder>();
				if (directTexturesHolder == null)
				{
					directTexturesHolder = terrain.gameObject.AddComponent<DirectTexturesHolder>();
				}
				DictionaryOrdered<string, Texture2D> dictionaryOrdered = new DictionaryOrdered<string, Texture2D>(textureNames);
				dictionaryOrdered.TakeMatchingValuesFrom(directTexturesHolder.textures);
				for (int i = 0; i < textureColors.Length; i++)
				{
					if (textureColors[i] != null)
					{
						string key = textureNames[i];
						Texture2D tex = dictionaryOrdered[key];
						CheckTexture(ref tex, resolution, textureFormat);
						tex.name = textureNames[i];
						tex.wrapMode = TextureWrapMode.Mirror;
						tex.SetPixels(0, 0, tex.width, tex.height, textureColors[i]);
						tex.Apply();
						dictionaryOrdered[key] = tex;
					}
				}
				directTexturesHolder.textures = dictionaryOrdered;
				directTexturesHolder.position = (Vector2D)terrain.transform.position;
				directTexturesHolder.size = (Vector2D)terrain.terrainData.size;
			}

			public static void CheckTexture(ref Texture2D tex, int resolution, TextureFormat format)
			{
				if (tex == null)
				{
					tex = new Texture2D(resolution, resolution, format, mipChain: false, linear: true);
				}
				else if (tex.width != resolution || tex.height != resolution || tex.format != format)
				{
					UnityEngine.Object.DestroyImmediate(tex);
					tex = new Texture2D(resolution, resolution, format, mipChain: false, linear: true);
				}
			}
		}

		public static FinalizeAction finalizeAction = Finalize;

		public override bool HideFirst => false;

		public override FinalizeAction FinalizeAction => finalizeAction;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\TexturesOut.cs", 575);
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
					data.StoreOutput(layers[j], typeof(DirectTexturesOutput200), (layers[j].name, layers[j].channelNum, layers[j].Opacity), array[j]);
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
			data.GatherOutputs<(string, int, float), MatrixWorld>(typeof(DirectTexturesOutput200), out var prototypes, out var products, out var masks, inSubs: true);
			string[] array = prototypes.Select(((string name, int chNum, float opacity) p) => p.name);
			prototypes.Select(((string name, int chNum, float opacity) p) => p.opacity);
			int[] array2 = prototypes.Select(((string name, int chNum, float opacity) p) => p.chNum);
			if (products.Length == 0)
			{
				if (stop == null || !stop.stop)
				{
					data.MarkApply(ApplyData.Empty);
				}
				return;
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			string[] array3 = array;
			foreach (string key in array3)
			{
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, dictionary.Count);
				}
			}
			Color[][] array4 = new Color[dictionary.Count][];
			string[] array5 = new string[dictionary.Count];
			for (int num2 = 0; num2 < products.Length; num2++)
			{
				int num3 = dictionary[array[num2]];
				array5[num3] = array[num2];
				if (products[num2] != null)
				{
					if (array4[num3] == null)
					{
						array4[num3] = new Color[data.area.active.rect.size.x * data.area.active.rect.size.z];
					}
					products[num2].ExportColors(array4[num3], data.area.active.rect.offset, data.area.active.rect.size, array2[num2], markOutrange: false, masks[num2]);
				}
			}
			if (stop == null || !stop.stop)
			{
				ApplyData applyData = new ApplyData
				{
					textureColors = array4,
					textureNames = array5,
					textureFormat = TextureFormat.RGBA32
				};
				Graph.OnOutputFinalized?.Invoke(typeof(DirectTexturesOutput200), data, applyData, stop);
				data.MarkApply(applyData);
			}
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}
	}
}
