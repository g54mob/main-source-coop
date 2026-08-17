using System;
using System.Collections;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Output", name = "Height", section = 2, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/HeightOut", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Height")]
	public class HeightOutput200 : OutputGenerator, IInlet<MatrixWorld>, IUnit
	{
		public enum Interpolation
		{
			None = 0,
			Smooth = 1,
			Scale2X = 2,
			Scale4X = 3
		}

		public enum ApplyType
		{
			SetHeights = 0,
			SetHeightsDelayLOD = 1,
			TextureToHeightmap = 2
		}

		public interface IApplyHeightData : IApplyData
		{
		}

		public class ApplySetData : IApplyData, IApplyHeightData
		{
			public float[,] heights2D;

			public float height;

			public Coord offset;

			public static ApplySetData Empty => new ApplySetData
			{
				heights2D = new float[33, 33]
			};

			public int Resolution => heights2D.GetLength(0);

			public void Read(Terrain terrain)
			{
				int heightmapResolution = terrain.terrainData.heightmapResolution;
				Read(terrain, new CoordRect(0, 0, heightmapResolution, heightmapResolution));
			}

			public void Read(Terrain terrain, CoordRect rect)
			{
				heights2D = terrain.terrainData.GetHeights(rect.offset.x, rect.offset.z, rect.size.x, rect.size.z);
				offset = rect.offset;
			}

			public void Apply(Terrain terrain)
			{
				if (!(terrain == null) && !terrain.Equals(null) && !(terrain.terrainData == null))
				{
					TerrainData terrainData = terrain.terrainData;
					Vector3 size = terrainData.size;
					terrainData.heightmapResolution = heights2D.GetLength(0);
					terrain.groupingID = terrainData.heightmapResolution;
					terrainData.size = new Vector3(size.x, height, size.z);
					terrainData.SetHeights(offset.x, offset.z, heights2D);
					terrain.Flush();
				}
			}
		}

		public class ApplySplitData : IApplyDataRoutine, IApplyData, IApplyHeightData
		{
			public float[][,] heights2DSplits;

			public float height;

			public static ApplySplitData Empty
			{
				get
				{
					ApplySplitData applySplitData = new ApplySplitData();
					applySplitData.heights2DSplits = new float[1][,] { new float[65, 65] };
					return applySplitData;
				}
			}

			public int Resolution
			{
				get
				{
					if (heights2DSplits.Length == 0)
					{
						return 0;
					}
					return heights2DSplits[0].GetLength(1);
				}
			}

			public void Apply(Terrain terrain)
			{
				if (!(terrain == null) && !terrain.Equals(null) && !(terrain.terrainData == null))
				{
					TerrainData terrainData = terrain.terrainData;
					FastHeightmapResize(terrain, heights2DSplits[0].GetLength(1), new Vector3(terrainData.size.x, height, terrainData.size.z));
					terrain.groupingID = terrainData.heightmapResolution;
					int num = 0;
					for (int i = 0; i < heights2DSplits.Length; i++)
					{
						terrainData.SetHeights(0, num, heights2DSplits[i]);
						num += heights2DSplits[i].GetLength(0);
					}
					terrain.Flush();
				}
			}

			public IEnumerator ApplyRoutine(Terrain terrain)
			{
				TerrainData data = terrain.terrainData;
				FastHeightmapResize(terrain, heights2DSplits[0].GetLength(1), new Vector3(data.size.x, height, data.size.z));
				terrain.groupingID = data.heightmapResolution;
				yield return null;
				int offset = 0;
				for (int i = 0; i < heights2DSplits.Length; i++)
				{
					data.SetHeightsDelayLOD(0, offset, heights2DSplits[i]);
					offset += heights2DSplits[i].GetLength(0);
					yield return null;
				}
				terrain.terrainData.SyncHeightmap();
				yield return null;
				terrain.Flush();
				terrain.terrainData.size = terrain.terrainData.size;
				yield return null;
			}

			public static void FastHeightmapResize(Terrain terrain, int res, Vector3 size)
			{
				TerrainData terrainData = terrain.terrainData;
				if ((terrainData.size - size).sqrMagnitude > 0.01f || terrainData.heightmapResolution != res)
				{
					if (res <= 64)
					{
						terrainData.heightmapResolution = res;
						terrainData.size = new Vector3(size.x, size.y, size.z);
						return;
					}
					terrainData.heightmapResolution = 65;
					terrain.Flush();
					int num = (res - 1) / 64;
					terrainData.size = new Vector3(size.x / (float)num, size.y, size.z / (float)num);
					terrainData.heightmapResolution = res;
				}
			}
		}

		public class ApplyTexData : IApplyDataRoutine, IApplyData, IApplyHeightData
		{
			public int res;

			public int margins;

			public int splitSize;

			public float height;

			public byte[] texBytes;

			private static Texture2D tempTex;

			private static RenderTexture renTex;

			public static ApplyTexData Empty => new ApplyTexData
			{
				texBytes = new byte[16],
				height = 0f,
				splitSize = 5
			};

			public int Resolution => (int)Mathf.Sqrt(texBytes.Length / 4);

			public void Apply(Terrain terrain)
			{
				if (!(terrain == null) && !terrain.Equals(null) && !(terrain.terrainData == null))
				{
					TerrainData terrainData = terrain.terrainData;
					RectInt rectInt = new RectInt(0, 0, res, res);
					if (tempTex == null || tempTex.width != res)
					{
						tempTex = new Texture2D(res, res, TextureFormat.RFloat, mipChain: false, linear: true);
					}
					tempTex.LoadRawTextureData(texBytes);
					tempTex.Apply(updateMipmaps: false);
					if (renTex == null || renTex.width != res)
					{
						renTex = new RenderTexture(res, res, 32, RenderTextureFormat.RFloat, 0);
					}
					Graphics.Blit(tempTex, renTex);
					Vector3 size = terrainData.size;
					terrainData.heightmapResolution = res;
					terrain.groupingID = res;
					terrainData.size = new Vector3(size.x, height, size.z);
					RenderTexture active = RenderTexture.active;
					RenderTexture.active = renTex;
					terrainData.CopyActiveRenderTextureToHeightmap(rectInt, rectInt.min, TerrainHeightmapSyncControl.None);
					terrainData.DirtyHeightmapRegion(rectInt, TerrainHeightmapSyncControl.HeightAndLod);
					RenderTexture.active = active;
				}
			}

			public IEnumerator ApplyRoutine(Terrain terrain)
			{
				if (terrain == null || terrain.Equals(null) || terrain.terrainData == null)
				{
					yield break;
				}
				TerrainData data = terrain.terrainData;
				new RectInt(0, 0, res, res);
				if (tempTex == null || tempTex.width != res)
				{
					tempTex = new Texture2D(res, res, TextureFormat.RFloat, mipChain: false, linear: true);
				}
				tempTex.LoadRawTextureData(texBytes);
				tempTex.Apply(updateMipmaps: false);
				if (renTex == null || renTex.width != res)
				{
					renTex = new RenderTexture(res, res, 32, RenderTextureFormat.RFloat, 0);
				}
				Graphics.Blit(tempTex, renTex);
				Vector3 size = data.size;
				data.size = new Vector3(size.x, height, size.z);
				ApplySplitData.FastHeightmapResize(terrain, res, new Vector3(size.x, height, size.z));
				terrain.groupingID = res;
				int numSplits = res / splitSize;
				if (numSplits * splitSize < res)
				{
					numSplits++;
				}
				for (int sx = 0; sx < numSplits; sx++)
				{
					for (int sz = 0; sz < numSplits; sz++)
					{
						RenderTexture active = RenderTexture.active;
						RenderTexture.active = renTex;
						RectInt rectInt = new RectInt(sx * splitSize, sz * splitSize, splitSize, splitSize);
						rectInt.xMax = Mathf.Min(rectInt.xMax, res);
						rectInt.yMax = Mathf.Min(rectInt.yMax, res);
						data.CopyActiveRenderTextureToHeightmap(rectInt, rectInt.min, TerrainHeightmapSyncControl.None);
						data.DirtyHeightmapRegion(rectInt, TerrainHeightmapSyncControl.HeightAndLod);
						RenderTexture.active = active;
						yield return null;
					}
				}
			}
		}

		public OutputLevel outputLevel = OutputLevel.Both;

		public bool guiApplyType;

		public static FinalizeAction finalizeAction = Finalize;

		public override OutputLevel OutputLevel => outputLevel;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\HeightOut.cs", 33);
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
					data.StoreOutput(this, typeof(HeightOutput200), this, matrixWorld);
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
			if (data.heights == null || data.heights.rect.size != data.area.full.rect.size || data.heights.worldPos != (Vector3)data.area.full.worldPos || data.heights.worldSize != (Vector3)data.area.full.worldSize)
			{
				data.heights = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			}
			data.heights.worldSize.y = data.globals.height;
			data.heights.Fill(0f);
			foreach (var item3 in data.Outputs<HeightOutput200, MatrixWorld, MatrixWorld>(typeof(HeightOutput200), inSubs: true))
			{
				MatrixWorld item = item3.Item2;
				MatrixWorld item2 = item3.Item3;
				if (data.heights == null)
				{
					return;
				}
				for (int i = 0; i < data.heights.arr.Length; i++)
				{
					if (stop != null && stop.stop)
					{
						return;
					}
					float num = item.arr[i];
					float num2 = ((item2 != null) ? item2.arr[i] : 1f);
					data.heights.arr[i] += num * num2;
				}
			}
			if (stop != null && stop.stop)
			{
				return;
			}
			Interpolation heightInterpolation = data.globals.heightInterpolation;
			int upscale = GetUpscale(heightInterpolation);
			int margins = data.area.Margins;
			int x = (data.heights.rect.size.x - margins * 2 - 1) * upscale + margins * 2 * upscale + 1;
			if (stop != null && stop.stop)
			{
				return;
			}
			Matrix matrix;
			switch (heightInterpolation)
			{
			default:
				matrix = data.heights;
				break;
			case Interpolation.Smooth:
				matrix = new Matrix(data.heights);
				MatrixOps.GaussianBlur(matrix, 0.5f);
				break;
			case Interpolation.Scale2X:
			case Interpolation.Scale4X:
				matrix = new Matrix(new CoordRect(data.heights.rect.offset, new Coord(x)));
				MatrixOps.Resize(data.heights, matrix);
				break;
			}
			matrix.Clamp01();
			int num3 = matrix.rect.size.x - margins * upscale * 2;
			int heightSplit = data.globals.heightSplit;
			int num4 = num3 / heightSplit;
			if (num3 % heightSplit != 0)
			{
				num4++;
			}
			IApplyData applyData;
			switch (data.isDraft ? data.globals.heightDraftApply : data.globals.heightMainApply)
			{
			case ApplyType.SetHeights:
			{
				float[,] array4 = new float[num3, num3];
				matrix.ExportHeights(array4, matrix.rect.offset + margins * upscale);
				applyData = new ApplySetData
				{
					heights2D = array4,
					height = data.globals.height
				};
				break;
			}
			case ApplyType.SetHeightsDelayLOD:
			{
				float[][,] array2 = new float[num4][,];
				int num6 = 0;
				for (int j = 0; j < num4; j++)
				{
					int num7 = Mathf.Min(heightSplit, num3 - num6);
					float[,] array3 = new float[num7, num3];
					Coord heightsOffset = new Coord(matrix.rect.offset.x + margins * upscale, matrix.rect.offset.z + margins * upscale + num6);
					matrix.ExportHeights(array3, heightsOffset);
					array2[j] = array3;
					num6 += num7;
				}
				applyData = new ApplySplitData
				{
					heights2DSplits = array2,
					height = data.globals.height
				};
				break;
			}
			default:
			{
				byte[] array = new byte[num3 * num3 * 4];
				float num5 = 1.5259022E-05f;
				matrix.ExportRawFloat(array, matrix.rect.offset + margins * upscale, new Coord(num3, num3), 0.5f - num5);
				applyData = new ApplyTexData
				{
					res = num3,
					margins = margins,
					splitSize = heightSplit,
					height = data.globals.height,
					texBytes = array
				};
				break;
			}
			}
			if (stop == null || !stop.stop)
			{
				Graph.OnOutputFinalized?.Invoke(typeof(HeightOutput200), data, applyData, stop);
				data.MarkApply(applyData);
			}
		}

		private static int GetUpscale(Interpolation interpolation)
		{
			return interpolation switch
			{
				Interpolation.Scale2X => 2, 
				Interpolation.Scale4X => 4, 
				_ => 1, 
			};
		}

		public override void ClearApplied(TileData data, Terrain terrain)
		{
			TerrainData terrainData = terrain.terrainData;
			Vector3 size = terrainData.size;
			terrainData.heightmapResolution = 33;
			terrain.groupingID = terrainData.heightmapResolution;
			terrainData.size = size;
			data.heights = null;
		}
	}
}
