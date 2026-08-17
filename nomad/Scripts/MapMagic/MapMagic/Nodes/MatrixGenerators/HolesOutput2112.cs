using System;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "Holes", section = 2, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/HeightOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Holes")]
	public class HolesOutput2112 : OutputGenerator, IInlet<MatrixWorld>, IUnit
	{
		public class ApplyData : IApplyData
		{
			public bool[,] holes2D;

			public static ApplyData Empty => new ApplyData
			{
				holes2D = new bool[33, 33]
			};

			public int Resolution => holes2D.GetLength(0);

			public void Apply(Terrain terrain)
			{
				if (!(terrain == null) && !terrain.Equals(null) && !(terrain.terrainData == null))
				{
					terrain.terrainData.SetHoles(0, 0, holes2D);
					terrain.Flush();
				}
			}
		}

		[Val(name = "Tolerance")]
		public float tolerance = 0.5f;

		public OutputLevel outputLevel = OutputLevel.Both;

		public static FinalizeAction finalizeAction = Finalize;

		public override OutputLevel OutputLevel => outputLevel;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\HolesOut.cs", 35);
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
					data.StoreOutput(this, typeof(HolesOutput2112), this, matrixWorld);
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
			int x = data.area.full.rect.size.x;
			int x2 = data.area.active.rect.size.x;
			int margins = data.area.Margins;
			int num = x2 - 1;
			bool[,] array = new bool[num, num];
			foreach (var (holesOutput, matrixWorld, matrixWorld2) in data.Outputs<HolesOutput2112, MatrixWorld, MatrixWorld>(typeof(HolesOutput2112), inSubs: true))
			{
				if (matrixWorld == null)
				{
					continue;
				}
				for (int i = 0; i < x2; i++)
				{
					for (int j = 0; j < x2; j++)
					{
						if (stop != null && stop.stop)
						{
							return;
						}
						int num2 = (j + margins) * x + (i + margins);
						float num3 = matrixWorld.arr[num2];
						float num4 = ((matrixWorld2 != null) ? matrixWorld2.arr[num2] : 1f);
						int num5 = (int)(1f * (float)i / (float)x2 * (float)num + 0.5f);
						int num6 = (int)(1f * (float)j / (float)x2 * (float)num + 0.5f);
						if (num3 * num4 > holesOutput.tolerance)
						{
							array[num6, num5] = false;
						}
						else
						{
							array[num6, num5] = true;
						}
					}
				}
			}
			if (stop == null || !stop.stop)
			{
				ApplyData applyData = new ApplyData
				{
					holes2D = array
				};
				Graph.OnOutputFinalized?.Invoke(typeof(HolesOutput2112), data, applyData, stop);
				data.MarkApply(applyData);
			}
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
			TerrainData terrainData = terrain.terrainData;
			_ = terrainData.size;
			int holesResolution = terrainData.holesResolution;
			bool[,] array = new bool[holesResolution, holesResolution];
			for (int i = 0; i < holesResolution; i++)
			{
				for (int j = 0; j < holesResolution; j++)
				{
					array[j, i] = true;
				}
			}
			terrainData.SetHoles(0, 0, array);
		}
	}
}
