using System;
using System.Diagnostics;
using Den.Tools;
using MapMagic.Core;
using MapMagic.Nodes.MatrixGenerators;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Terrains
{
	public static class Weld
	{
		private static readonly Coord[] weldDirections = new Coord[4]
		{
			new Coord(1, 0),
			new Coord(-1, 0),
			new Coord(0, 1),
			new Coord(0, -1)
		};

		private static readonly Coord[] cornerDirections = new Coord[4]
		{
			new Coord(1, 1),
			new Coord(-1, 1),
			new Coord(1, -1),
			new Coord(-1, -1)
		};

		public static Action<TileData, EdgesSet> ReadEdgesCustom;

		public static Action<TileData, EdgesSet> WriteEdgesCustom;

		public static void WeldEdges(Edges src, Coord srcCoord, Edges dst, Coord dstCoord)
		{
			Coord coord = srcCoord - dstCoord;
			if (coord.x == 0 && coord.z == 1)
			{
				WeldArrays(src.arr_x, dst.arr_X);
			}
			else if (coord.x == 0 && coord.z == -1)
			{
				WeldArrays(src.arr_X, dst.arr_x);
			}
			else if (coord.x == 1 && coord.z == 0)
			{
				WeldArrays(src.arr_z, dst.arr_Z);
			}
			else if (coord.x == -1 && coord.z == 0)
			{
				WeldArrays(src.arr_Z, dst.arr_z);
			}
		}

		public static void WeldArrays(float[] src, float[] dst, bool lowerOnMismatch = false)
		{
			if (src.Length == dst.Length)
			{
				Array.Copy(src, dst, src.Length);
			}
			else
			{
				if (src.Length <= dst.Length)
				{
					return;
				}
				int num = (src.Length - 1) / (dst.Length - 1);
				for (int i = 0; i < dst.Length; i++)
				{
					dst[i] = src[i * num];
				}
				if (!lowerOnMismatch)
				{
					return;
				}
				float num2 = 0f;
				for (int j = 0; j < dst.Length - 1; j++)
				{
					float num3 = 0f;
					for (int k = 0; k < num; k++)
					{
						float num4 = 1f * (float)k / (float)num;
						float num5 = dst[j] * (1f - num4) + dst[j + 1] * num4;
						float num6 = src[j * num + k];
						float num7 = num5 - num6;
						if (num7 > num3)
						{
							num3 = num7;
						}
					}
					dst[j] -= ((num2 > num3) ? num2 : num3);
					num2 = num3;
				}
			}
		}

		public static void WeldArraysDebug(float[] src, float[] dst, bool lowerOnMismatch = false)
		{
		}

		public static void ReadEdges(TileData thisData, EdgesSet thisEdges)
		{
			HeightOutput200.ApplySetData applySetData = thisData.ApplyOfType<HeightOutput200.ApplySetData>();
			if (applySetData != null)
			{
				thisEdges.heightEdges.ReadFloats2D(applySetData.heights2D);
			}
			HeightOutput200.ApplySplitData applySplitData = thisData.ApplyOfType<HeightOutput200.ApplySplitData>();
			if (applySplitData != null)
			{
				thisEdges.heightEdges.ReadSplitFloats2D(applySplitData.heights2DSplits);
			}
			HeightOutput200.ApplyTexData applyTexData = thisData.ApplyOfType<HeightOutput200.ApplyTexData>();
			if (applyTexData != null)
			{
				thisEdges.heightEdges.ReadRawFloat(applyTexData.texBytes);
			}
			TexturesOutput200.ApplyData applyData = thisData.ApplyOfType<TexturesOutput200.ApplyData>();
			if (applyData != null && applyData.splats != null)
			{
				int length = applyData.splats.GetLength(2);
				if (thisEdges.splatEdges == null || thisEdges.splatEdges.Length != length)
				{
					Array.Resize(ref thisEdges.splatEdges, length);
				}
				for (int i = 0; i < length; i++)
				{
					if (thisEdges.splatEdges[i] == null)
					{
						thisEdges.splatEdges[i] = new Edges(0, 0);
					}
					thisEdges.splatEdges[i].ReadSplats(applyData.splats, i);
				}
			}
			ReadEdgesCustom?.Invoke(thisData, thisEdges);
			thisEdges.ready = true;
		}

		public static void WeldEdgesInThread(EdgesSet thisEdges, TerrainTileManager tileManager, Coord coord, bool isDraft = false)
		{
			if (tileManager == null)
			{
				return;
			}
			for (int i = 0; i < weldDirections.Length; i++)
			{
				TerrainTile terrainTile = tileManager[coord + weldDirections[i]];
				if (terrainTile == null || (isDraft && terrainTile.draft == null) || (isDraft && !terrainTile.draft.generateReady) || (!isDraft && terrainTile.main == null) || (!isDraft && !terrainTile.main.generateReady))
				{
					continue;
				}
				EdgesSet edgesSet = ((!isDraft) ? terrainTile.main?.edges : terrainTile.draft?.edges);
				if (edgesSet == null || !edgesSet.ready)
				{
					continue;
				}
				if (edgesSet.heightEdges != null && edgesSet.heightEdges.SizeX == thisEdges.heightEdges.SizeX)
				{
					WeldEdges(edgesSet.heightEdges, terrainTile.coord, thisEdges.heightEdges, coord);
				}
				if (edgesSet.splatEdges != null && thisEdges.splatEdges != null && thisEdges.splatEdges.Length == edgesSet.splatEdges.Length)
				{
					for (int j = 0; j < thisEdges.splatEdges.Length; j++)
					{
						if (thisEdges.splatEdges[j].SizeX == edgesSet.splatEdges[j].SizeX)
						{
							WeldEdges(edgesSet.splatEdges[j], terrainTile.coord, thisEdges.splatEdges[j], coord);
						}
					}
				}
				if (edgesSet.controlEdges == null || thisEdges.controlEdges == null || thisEdges.controlEdges.Length != edgesSet.controlEdges.Length)
				{
					continue;
				}
				for (int k = 0; k < thisEdges.controlEdges.Length; k++)
				{
					if (thisEdges.controlEdges[k].SizeX == edgesSet.controlEdges[k].SizeX)
					{
						WeldEdges(edgesSet.controlEdges[k], terrainTile.coord, thisEdges.controlEdges[k], coord);
					}
				}
			}
		}

		public static void WriteEdges(TileData thisData, EdgesSet thisEdges)
		{
			HeightOutput200.ApplySetData applySetData = thisData.ApplyOfType<HeightOutput200.ApplySetData>();
			if (applySetData != null)
			{
				thisEdges.heightEdges.WriteFloats2D(applySetData.heights2D);
			}
			HeightOutput200.ApplySplitData applySplitData = thisData.ApplyOfType<HeightOutput200.ApplySplitData>();
			if (applySplitData != null)
			{
				thisEdges.heightEdges.WriteSplitFloats2D(applySplitData.heights2DSplits);
			}
			HeightOutput200.ApplyTexData applyTexData = thisData.ApplyOfType<HeightOutput200.ApplyTexData>();
			if (applyTexData != null)
			{
				thisEdges.heightEdges.WriteRawFloat(applyTexData.texBytes);
			}
			TexturesOutput200.ApplyData applyData = thisData.ApplyOfType<TexturesOutput200.ApplyData>();
			if (applyData != null && applyData.splats != null)
			{
				int length = applyData.splats.GetLength(2);
				if (thisEdges.splatEdges == null || thisEdges.splatEdges.Length != length)
				{
					Array.Resize(ref thisEdges.splatEdges, length);
				}
				for (int i = 0; i < length; i++)
				{
					thisEdges.splatEdges[i].WriteSplats(applyData.splats, i);
				}
			}
			WriteEdgesCustom?.Invoke(thisData, thisEdges);
		}

		public static void WeldSurroundingDraftsToThisMain(TerrainTileManager tileManager, Coord coord)
		{
			if (tileManager == null)
			{
				return;
			}
			TerrainTile reference = tileManager[coord];
			for (int i = 0; i < weldDirections.Length; i++)
			{
				TerrainTile terrainTile = tileManager[coord + weldDirections[i]];
				if (terrainTile?.draft != null && terrainTile.draft.applyReady && terrainTile.draft.generateReady && terrainTile.ActiveTerrain == terrainTile.draft.terrain && !terrainTile.draft.edges.lowered[-weldDirections[i]])
				{
					WeldDraftToMain(reference, terrainTile, weldDirections[i]);
					terrainTile.draft.edges.lowered[-weldDirections[i]] = true;
				}
			}
		}

		public static void WeldThisDraftWithSurroundings(TerrainTileManager tileManager, Coord coord)
		{
			if (tileManager == null)
			{
				return;
			}
			TerrainTile terrainTile = tileManager[coord];
			for (int i = 0; i < weldDirections.Length; i++)
			{
				Coord coord2 = weldDirections[i];
				TerrainTile terrainTile2 = tileManager[coord + weldDirections[i]];
				if (terrainTile2 == null)
				{
					continue;
				}
				if (terrainTile2.draft != null && terrainTile2.ActiveTerrain == terrainTile2.draft.terrain)
				{
					if (terrainTile2.draft.edges.lowered[-coord2])
					{
						RestoreDraftWeld(terrainTile2, -coord2);
						terrainTile2.draft.edges.lowered[-coord2] = false;
					}
					if (terrainTile.draft.edges.lowered[coord2])
					{
						RestoreDraftWeld(terrainTile, coord2);
						terrainTile.draft.edges.lowered[coord2] = false;
					}
				}
				else if (terrainTile2.main != null && terrainTile2.ActiveTerrain == terrainTile2.main.terrain && !terrainTile.draft.edges.lowered[coord2])
				{
					WeldDraftToMain(terrainTile2, terrainTile, -coord2);
					terrainTile.draft.edges.lowered[coord2] = true;
				}
			}
		}

		private static void RestoreDraftWeld(TerrainTile welded, Coord weldDir)
		{
			float[] arr = welded.draft.edges.heightEdges.GetArr(weldDir);
			ApplyToTerrain(welded.draft.terrain.terrainData, arr, weldDir);
		}

		private static void WeldDraftToMain(TerrainTile reference, TerrainTile welded, Coord weldDir)
		{
			if (welded.draft.edges.heightEdges.IsEmpty)
			{
				welded.draft.edges.heightEdges.ReadFloats2D(welded.draft.terrain.terrainData.GetHeights(0, 0, welded.draft.terrain.terrainData.heightmapResolution, welded.draft.terrain.terrainData.heightmapResolution));
			}
			EdgesSet edges = reference.main.edges;
			EdgesSet edges2 = welded.draft.edges;
			float[] arr = edges.heightEdges.GetArr(weldDir);
			float[] array = new float[edges2.heightEdges.GetArr(-weldDir).Length];
			WeldArrays(arr, array, lowerOnMismatch: true);
			ApplyToTerrain(welded.draft.terrain.terrainData, array, -weldDir);
		}

		private static void ApplyToTerrain(TerrainData terrData, float[] arr, Coord dir)
		{
			if (arr.Length == 0)
			{
				throw new Exception("Empty weld array. Possibly terrain has not been generated yet");
			}
			int heightmapResolution = terrData.heightmapResolution;
			if (dir.x == -1 && dir.z == 0)
			{
				float[,] array = new float[heightmapResolution, 1];
				for (int i = 0; i < heightmapResolution; i++)
				{
					array[i, 0] = arr[i];
				}
				terrData.SetHeightsDelayLOD(0, 0, array);
			}
			else if (dir.x == 1 && dir.z == 0)
			{
				float[,] array2 = new float[heightmapResolution, 1];
				for (int j = 0; j < heightmapResolution; j++)
				{
					array2[j, 0] = arr[j];
				}
				terrData.SetHeightsDelayLOD(heightmapResolution - 1, 0, array2);
			}
			else if (dir.x == 0 && dir.z == -1)
			{
				float[,] array3 = new float[1, heightmapResolution];
				for (int k = 0; k < heightmapResolution; k++)
				{
					array3[0, k] = arr[k];
				}
				terrData.SetHeightsDelayLOD(0, 0, array3);
			}
			else if (dir.x == 0 && dir.z == 1)
			{
				float[,] array4 = new float[1, heightmapResolution];
				for (int l = 0; l < heightmapResolution; l++)
				{
					array4[0, l] = arr[l];
				}
				terrData.SetHeightsDelayLOD(0, heightmapResolution - 1, array4);
			}
		}

		public static void WeldCorners(TerrainTileManager tileManager, Coord coord, bool isDraft = false)
		{
			if (tileManager == null)
			{
				return;
			}
			TerrainTile terrainTile = tileManager[coord];
			Terrain terrain = ((!isDraft) ? terrainTile.main?.terrain : terrainTile.draft?.terrain);
			TerrainData terrainData = terrain.terrainData;
			if (terrain == null)
			{
				return;
			}
			int heightmapResolution = terrain.terrainData.heightmapResolution;
			float[,] array = new float[1, 1];
			for (int i = 0; i < cornerDirections.Length; i++)
			{
				Coord coord2 = (cornerDirections[i] + 1) / 2;
				for (int j = 0; j < cornerDirections.Length; j++)
				{
					Coord coord3 = (cornerDirections[j] + 1) / 2;
					TerrainTile terrainTile2 = tileManager[coord + coord3 + coord2 - 1];
					if (!(terrainTile2 == null) && !(terrainTile2 == terrainTile))
					{
						Terrain terrain2 = ((!isDraft) ? terrainTile2.main?.terrain : terrainTile2.draft?.terrain);
						if (!(terrain2 == null) && terrain2.isActiveAndEnabled)
						{
							TerrainData terrainData2 = terrain2.terrainData;
							float num = terrainData2.GetHeight((1 - coord3.x) * (heightmapResolution - 1), (1 - coord3.z) * (heightmapResolution - 1)) / terrainData2.size.y;
							array[0, 0] = num;
							terrainData.SetHeightsDelayLOD(coord2.x * (heightmapResolution - 1), coord2.z * (heightmapResolution - 1), array);
							break;
						}
					}
				}
			}
		}

		public static void SetNeighbors(TerrainTileManager tileManager, Coord coord)
		{
			if (tileManager != null)
			{
				_ = tileManager[coord];
				TerrainTile terrainTile = tileManager[coord];
				coord = terrainTile.coord;
				Terrain terrain = terrainTile.main?.terrain;
				Terrain terrain2 = tileManager[new Coord(coord.x - 1, coord.z)]?.main?.terrain;
				if (terrain2 != null && terrain2.isActiveAndEnabled)
				{
					terrain.SetNeighbors(terrain2, terrain.topNeighbor, terrain.rightNeighbor, terrain.bottomNeighbor);
					terrain2.SetNeighbors(terrain2.leftNeighbor, terrain2.topNeighbor, terrain, terrain2.bottomNeighbor);
				}
				Terrain terrain3 = tileManager[new Coord(coord.x + 1, coord.z)]?.main?.terrain;
				if (terrain3 != null && terrain3.isActiveAndEnabled)
				{
					terrain.SetNeighbors(terrain.leftNeighbor, terrain.topNeighbor, terrain3, terrain.bottomNeighbor);
					terrain3.SetNeighbors(terrain, terrain3.topNeighbor, terrain3.rightNeighbor, terrain3.bottomNeighbor);
				}
				Terrain terrain4 = tileManager[new Coord(coord.x, coord.z - 1)]?.main?.terrain;
				if (terrain4 != null && terrain4.isActiveAndEnabled)
				{
					terrain.SetNeighbors(terrain.leftNeighbor, terrain.topNeighbor, terrain.rightNeighbor, terrain4);
					terrain4.SetNeighbors(terrain4.leftNeighbor, terrain, terrain4.rightNeighbor, terrain4.bottomNeighbor);
				}
				Terrain terrain5 = tileManager[new Coord(coord.x, coord.z + 1)]?.main?.terrain;
				if (terrain5 != null && terrain5.isActiveAndEnabled)
				{
					terrain.SetNeighbors(terrain.leftNeighbor, terrain5, terrain.rightNeighbor, terrain.bottomNeighbor);
					terrain5.SetNeighbors(terrain5.leftNeighbor, terrain5.topNeighbor, terrain5.rightNeighbor, terrain);
				}
			}
		}

		public static void SetNeighborsAll(TerrainTileManager tileManager)
		{
			Stopwatch stopwatch = null;
			stopwatch = new Stopwatch();
			stopwatch.Start();
			if (tileManager == null)
			{
				return;
			}
			foreach (TerrainTile item in tileManager.Tiles())
			{
				SetNeighbors(tileManager, item.coord);
			}
			stopwatch.Stop();
			Debug.Log("Neighboring in " + stopwatch.Elapsed.TotalMilliseconds + "ms (" + stopwatch.ElapsedMilliseconds + ")");
		}

		private static Terrain GetNeighbor(Terrain terrain, Coord dir)
		{
			if (dir.x == 0 && dir.z == -1)
			{
				return terrain.bottomNeighbor;
			}
			if (dir.x == 0 && dir.z == 1)
			{
				return terrain.topNeighbor;
			}
			if (dir.x == -1 && dir.z == 0)
			{
				return terrain.leftNeighbor;
			}
			if (dir.x == 1 && dir.z == 0)
			{
				return terrain.rightNeighbor;
			}
			return null;
		}
	}
}
