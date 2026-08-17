using System;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "Grass", section = 2, drawButtons = false, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/GrassOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Grass")]
	public class GrassOutput200 : OutputGenerator, IInlet<MatrixWorld>, IUnit
	{
		public enum GrassRenderMode
		{
			Grass = 0,
			Billboard = 1,
			MeshVertexLit = 2,
			MeshUnlit = 3
		}

		public class ApplyData : IApplyData
		{
			public int[][,] detailLayers;

			public DetailPrototype[] detailPrototypes;

			public int patchResolution = 16;

			public DetailScatterMode scatterMode = DetailScatterMode.InstanceCountMode;

			public static ApplyData Empty => new ApplyData
			{
				detailLayers = new int[0][,],
				detailPrototypes = new DetailPrototype[0]
			};

			public int Resolution
			{
				get
				{
					if (detailLayers.Length == 0)
					{
						return 0;
					}
					return detailLayers[0].GetLength(0);
				}
			}

			public void Apply(Terrain terrain)
			{
				if (!(terrain == null) && !terrain.Equals(null) && !(terrain.terrainData == null))
				{
					terrain.terrainData.SetDetailScatterMode(scatterMode);
					int length = detailLayers[0].GetLength(1);
					terrain.terrainData.SetDetailResolution(length, patchResolution);
					terrain.terrainData.detailPrototypes = detailPrototypes;
					for (int i = 0; i < detailLayers.Length; i++)
					{
						terrain.terrainData.SetDetailLayer(0, 0, i, detailLayers[i]);
					}
				}
			}
		}

		public OutputLevel outputLevel = OutputLevel.Main;

		public float density = 0.5f;

		public DetailPrototype prototype = new DetailPrototype
		{
			dryColor = new Color(0.95f, 1f, 0.65f),
			healthyColor = new Color(0.5f, 0.65f, 0.35f)
		};

		public GrassRenderMode renderMode;

		public static FinalizeAction finalizeAction = Finalize;

		public override OutputLevel OutputLevel => outputLevel;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\GrassOut.cs", 25);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld != null && (stop == null || !stop.stop))
			{
				if (enabled)
				{
					data.StoreOutput(this, typeof(GrassOutput200), this, matrixWorld);
					data.MarkFinalize(Finalize, stop);
				}
				else
				{
					data.RemoveFinalize(finalizeAction);
				}
			}
		}

		public static void Finalize(TileData data, StopToken stop)
		{
			int num = data.OutputsCount(typeof(GrassOutput200), inSubs: true);
			_ = data.area.active.rect;
			int[][,] array = new int[num][,];
			DetailPrototype[] array2 = new DetailPrototype[num];
			int num2 = 0;
			foreach (var (grassOutput, matrixWorld, biomeMask) in data.Outputs<GrassOutput200, MatrixWorld, MatrixWorld>(typeof(GrassOutput200), inSubs: true))
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				array[num2] = CreateDetailLayer(matrixWorld, biomeMask, grassOutput.density * matrixWorld.PixelSize.x * matrixWorld.PixelSize.z, num2, data, stop);
				array2[num2] = grassOutput.prototype;
				num2++;
			}
			if (stop == null || !stop.stop)
			{
				ApplyData applyData = new ApplyData
				{
					detailLayers = array,
					detailPrototypes = array2,
					patchResolution = data.globals.grassResPerPatch
				};
				applyData.scatterMode = data.globals.grassScatterMode;
				Graph.OnOutputFinalized?.Invoke(typeof(GrassOutput200), data, applyData, stop);
				data.MarkApply(applyData);
			}
		}

		private static int[,] CreateDetailLayer(Matrix matrix, Matrix biomeMask, float density, int randomNum, TileData data, StopToken stop)
		{
			int grassResDownscale = data.globals.grassResDownscale;
			int num = (data.area.active.rect.size.x - 1) / grassResDownscale + 1;
			int x = data.area.full.rect.size.x;
			int margins = data.area.Margins;
			int[,] array = new int[num, num];
			if (matrix == null)
			{
				return array;
			}
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					if (stop != null && stop.stop)
					{
						return null;
					}
					int num2 = (j * grassResDownscale + margins) * x + (i * grassResDownscale + margins);
					float num3 = matrix.arr[num2];
					float num4 = matrix.arr[num2 + 1];
					if (num4 < num3)
					{
						num3 = num4;
					}
					float num5 = matrix.arr[num2 + x];
					if (num5 < num3)
					{
						num3 = num5;
					}
					float num6 = matrix.arr[num2 + x + 1];
					if (num6 < num3)
					{
						num3 = num6;
					}
					if (biomeMask != null)
					{
						num3 *= biomeMask.arr[num2];
					}
					if (num3 < 0f)
					{
						num3 = 0f;
					}
					if (num3 > 1f)
					{
						num3 = 1f;
					}
					num3 *= density * (float)grassResDownscale * (float)grassResDownscale;
					float num7 = data.random.Random(randomNum, i, j);
					int num8 = (int)num3;
					if (num3 - (float)num8 > num7)
					{
						num8++;
					}
					array[j, i] = num8;
				}
			}
			return array;
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
			TerrainData terrainData = terrain.terrainData;
			_ = terrainData.size;
			terrainData.detailPrototypes = new DetailPrototype[0];
			terrainData.SetDetailResolution(32, 32);
		}
	}
}
