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
	[GeneratorMenu(menu = "Map/Output", name = "Custom Material", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/TexturesOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Textures")]
	public class CustomShaderOutput200 : BaseTexturesOutput<CustomShaderOutput200.CustomShaderLayer>
	{
		public class CustomShaderLayer : BaseTextureLayer
		{
		}

		public class ApplyData : IApplyData
		{
			public Color[][] textureColors;

			public string[] textureNames;

			public string[] altTextureNames;

			public TextureFormat textureFormat;

			public float textureBaseMapDistance;

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
				if (textureColors == null || textureColors.Length == 0)
				{
					return;
				}
				int num = (int)Mathf.Sqrt(textureColors[0].Length);
				MaterialPropertySerializer materialPropertySerializer = terrain.GetComponent<MaterialPropertySerializer>();
				if (materialPropertySerializer == null)
				{
					materialPropertySerializer = terrain.gameObject.AddComponent<MaterialPropertySerializer>();
				}
				for (int i = 0; i < textureColors.Length; i++)
				{
					if (textureColors[i] == null)
					{
						continue;
					}
					string text = null;
					if (i < textureNames.Length)
					{
						text = textureNames[i];
					}
					Texture2D texture2D = materialPropertySerializer.GetTexture(textureNames[i]);
					if (texture2D == null || texture2D.width != num || texture2D.height != num || texture2D.format != textureFormat)
					{
						if (texture2D != null)
						{
							UnityEngine.Object.DestroyImmediate(texture2D);
						}
						texture2D = new Texture2D(num, num, textureFormat, mipChain: false, linear: true);
						texture2D.name = text;
						texture2D.wrapMode = TextureWrapMode.Mirror;
						materialPropertySerializer.SetTexture(textureNames[i], texture2D);
					}
					texture2D.SetPixels(0, 0, texture2D.width, texture2D.height, textureColors[i]);
					texture2D.Apply();
					if (text != null)
					{
						terrain.materialTemplate.SetTexture(text, texture2D);
					}
				}
				materialPropertySerializer.Apply();
				terrain.basemapDistance = textureBaseMapDistance;
			}
		}

		public static string[] controlTextureNames = new string[1] { "_ControlTexture1" };

		public static string[] controlTexturePossibleNames = new string[12]
		{
			"_ControlTexture1", "_ControlTexture2", "_ControlTexture3", "_ControlTexture4", "_ControlTexture5", "_ControlTexture6", "_ControlTexture7", "_ControlTexture8", "_ControlTexture9", "_ControlTexture10",
			"_ControlTexture11", "_ControlTexture12"
		};

		public static FinalizeAction finalizeAction = Finalize;

		public override FinalizeAction FinalizeAction => finalizeAction;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\TexturesOut.cs", 292);
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
					data.StoreOutput(layers[i], typeof(CustomShaderOutput200), layers[i], array[i]);
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
			data.GatherOutputs<(int, float), MatrixWorld>(typeof(CustomShaderOutput200), out var prototypes, out var products, out var masks, inSubs: true);
			float[] opacities = prototypes.Select(((int chNum, float opacity) p) => p.opacity);
			int[] channelNums = prototypes.Select(((int chNum, float opacity) p) => p.chNum);
			if (products.Length == 0)
			{
				if (stop == null || !stop.stop)
				{
					data.MarkApply(ApplyData.Empty);
				}
				return;
			}
			Color[][] textureColors = BlendMatrices(data.area.active.rect, products, masks, opacities, channelNums, normalize: true);
			if (stop == null || !stop.stop)
			{
				ApplyData applyData = new ApplyData
				{
					textureColors = textureColors,
					textureFormat = TextureFormat.RGBA32,
					textureBaseMapDistance = 10000000f,
					textureNames = (string[])controlTextureNames.Clone()
				};
				Graph.OnOutputFinalized?.Invoke(typeof(CustomShaderOutput200), data, applyData, stop);
				data.MarkApply(applyData);
			}
		}

		public static Color[][] BlendMatrices(CoordRect colorsRect, IList<Matrix> matrices, IList<Matrix> biomeMasks, IList<float> opacities, IList<int> channelNums, bool normalize = false)
		{
			int num = 0;
			foreach (int channelNum in channelNums)
			{
				if (channelNum > num)
				{
					num = channelNum;
				}
			}
			int num2 = num / 4 + 1;
			Color[][] array = new Color[num2][];
			int count = matrices.Count;
			CoordRect coordRect = new CoordRect(0, 0, 0, 0);
			for (int i = 0; i < count; i++)
			{
				if (matrices[i] != null)
				{
					coordRect = matrices[i].rect;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (matrices[j] != null && matrices[j].rect != coordRect)
				{
					throw new Exception("MapMagic: Matrix rect mismatch");
				}
			}
			for (int k = 0; k < count; k++)
			{
				if (biomeMasks[k] != null && biomeMasks[k].rect != coordRect)
				{
					throw new Exception("MapMagic: Biome matrix rect mismatch");
				}
			}
			float[] array2 = new float[num2 * 4];
			for (int l = 0; l < colorsRect.size.x; l++)
			{
				for (int m = 0; m < colorsRect.size.z; m++)
				{
					int num3 = colorsRect.offset.x + l;
					int num4 = (colorsRect.offset.z + m - coordRect.offset.z) * coordRect.size.x + num3 - coordRect.offset.x;
					int num5 = m * colorsRect.size.x + l;
					float num6 = 0f;
					for (int n = 0; n < array2.Length; n++)
					{
						array2[n] = 0f;
					}
					for (int num7 = 0; num7 < count; num7++)
					{
						Matrix matrix = matrices[num7];
						if (matrix != null)
						{
							float num8 = matrix.arr[num4];
							Matrix matrix2 = biomeMasks[num7];
							if (matrix2 != null)
							{
								num8 *= matrix2.arr[num4];
							}
							if (num8 < 0f)
							{
								num8 = 0f;
							}
							if (num8 > 1f)
							{
								num8 = 1f;
							}
							num6 += num8;
							array2[channelNums[num7]] += num8;
						}
					}
					for (int num9 = 0; num9 < array2.Length; num9++)
					{
						float num10 = array2[num9];
						if (normalize)
						{
							num10 = ((num6 != 0f) ? (num10 / num6) : 0f);
						}
						int num11 = num9 / 4;
						int num12 = num9 % 4;
						if (array[num11] == null)
						{
							array[num11] = new Color[colorsRect.size.x * colorsRect.size.z];
						}
						switch (num12)
						{
						case 0:
							array[num11][num5].r += num10;
							break;
						case 1:
							array[num11][num5].g += num10;
							break;
						case 2:
							array[num11][num5].b += num10;
							break;
						case 3:
							array[num11][num5].a += num10;
							break;
						}
					}
				}
			}
			return array;
		}

		public static Color[] MatricesToColors(CoordRect colorsRect, Matrix rMatrix, Matrix gMatrix, Matrix bMatrix, Matrix aMatrix)
		{
			CoordRect rect = rMatrix.rect;
			Color[] array = new Color[colorsRect.size.x * colorsRect.size.z];
			Color color = default(Color);
			Coord min = colorsRect.Min;
			Coord max = colorsRect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - rect.offset.z) * rect.size.x + i - rect.offset.x;
					int num2 = (j - colorsRect.offset.z) * colorsRect.size.x + i - colorsRect.offset.x;
					color.r = rMatrix.arr[num];
					if (gMatrix != null)
					{
						color.g = gMatrix.arr[num];
					}
					if (bMatrix != null)
					{
						color.b = bMatrix.arr[num];
					}
					if (aMatrix != null)
					{
						color.a = aMatrix.arr[num];
					}
					array[num2] = color;
				}
			}
			return array;
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
		}
	}
}
