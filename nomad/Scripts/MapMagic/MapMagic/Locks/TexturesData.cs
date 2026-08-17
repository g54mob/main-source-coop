using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes;
using MapMagic.Nodes.MatrixGenerators;
using UnityEngine;

namespace MapMagic.Locks
{
	public class TexturesData : ILockData
	{
		private float[,,] lockSplats;

		private TerrainLayer[] lockPrototypes;

		private CoordCircle circle;

		private int resolution;

		public void Read(Terrain terrain, Lock lk)
		{
			TerrainData terrainData = terrain.terrainData;
			if (terrainData.terrainLayers == null || terrainData.terrainLayers.Length == 0)
			{
				lockSplats = null;
				lockPrototypes = null;
				return;
			}
			resolution = terrain.terrainData.alphamapResolution;
			circle = new CoordCircle(terrain, resolution, lk.worldPos, lk.worldRadius, lk.worldTransition);
			lockSplats = terrainData.GetAlphamaps(circle.rect.offset.x, circle.rect.offset.z, circle.rect.size.x, circle.rect.size.z);
			lockPrototypes = terrainData.terrainLayers;
		}

		public void WriteInThread(IApplyData applyData)
		{
			if (applyData is TexturesOutput200.ApplyData applyData2 && lockSplats != null && lockPrototypes != null && applyData2.Resolution == resolution)
			{
				UnifyPrototypes(ref applyData2.prototypes, ref applyData2.splats, ref lockPrototypes, ref lockSplats);
				Matrix matrix = new Matrix(circle.rect);
				Matrix matrix2 = new Matrix(circle.rect);
				for (int i = 0; i < lockPrototypes.Length; i++)
				{
					matrix.Fill(0f);
					matrix.ImportSplats(lockSplats, circle.rect.offset, i);
					matrix2.ImportSplats(applyData2.splats, new Coord(0, 0), i);
					matrix.ExtendCircular(circle.center, circle.radius - 1, circle.transition + 2);
					matrix.Clamp01();
					matrix2.BlendStamped(matrix2, matrix, circle.center.x, circle.center.z, circle.radius, circle.transition);
					matrix2.ExportSplats(applyData2.splats, new Coord(0, 0), i);
				}
			}
		}

		public void WriteInApply(Terrain terrain, bool resizeTerrain = false)
		{
			if (lockSplats == null || lockPrototypes == null)
			{
				return;
			}
			TerrainData terrainData = terrain.terrainData;
			if (terrain.terrainData.alphamapResolution != resolution)
			{
				if (!resizeTerrain)
				{
					return;
				}
				terrainData.alphamapResolution = resolution;
			}
			terrainData.terrainLayers = lockPrototypes;
			terrainData.SetAlphamaps(circle.rect.offset.x, circle.rect.offset.z, lockSplats);
		}

		public void ApplyHeightDelta(Matrix src, Matrix dst)
		{
		}

		public void ResizeFrom(ILockData otherData)
		{
			TexturesData texturesData = (TexturesData)otherData;
			if (texturesData.lockSplats == null || lockSplats == null)
			{
				return;
			}
			int num = Mathf.Min(texturesData.lockSplats.GetLength(2), lockSplats.GetLength(2));
			if (num != 0)
			{
				Matrix matrix = new Matrix(texturesData.circle.rect);
				Matrix matrix2 = new Matrix(circle.rect);
				for (int i = 0; i < num; i++)
				{
					matrix.ImportSplats(texturesData.lockSplats, i);
					MatrixOps.Resize(matrix, matrix2);
					matrix2.ExportSplats(lockSplats, i);
				}
			}
		}

		private static void UnifyPrototypes(ref TerrainLayer[] basePrototypes, ref float[,,] baseData, ref TerrainLayer[] addPrototypes, ref float[,,] addData)
		{
			if (ArrayTools.MatchExactly(basePrototypes, addPrototypes))
			{
				return;
			}
			List<TerrainLayer> list = new List<TerrainLayer>();
			list.AddRange(basePrototypes);
			for (int i = 0; i < addPrototypes.Length; i++)
			{
				if (!list.Contains(addPrototypes[i]))
				{
					list.Add(addPrototypes[i]);
				}
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			for (int j = 0; j < basePrototypes.Length; j++)
			{
				dictionary.Add(j, list.IndexOf(basePrototypes[j]));
			}
			for (int k = 0; k < addPrototypes.Length; k++)
			{
				dictionary2.Add(k, list.IndexOf(addPrototypes[k]));
			}
			float[,,] array = new float[baseData.GetLength(0), baseData.GetLength(1), list.Count];
			int length = baseData.GetLength(2);
			for (int l = 0; l < length; l++)
			{
				ArrayTools.CopyLayer(baseData, array, l, dictionary[l]);
			}
			baseData = array;
			float[,,] array2 = new float[addData.GetLength(0), addData.GetLength(1), list.Count];
			int length2 = addData.GetLength(2);
			for (int m = 0; m < length2; m++)
			{
				ArrayTools.CopyLayer(addData, array2, m, dictionary2[m]);
			}
			addData = array2;
			basePrototypes = list.ToArray();
			addPrototypes = list.ToArray();
		}

		private static bool ComparePrototypes(TerrainLayer p1, TerrainLayer p2)
		{
			if (p1.diffuseTexture == p2.diffuseTexture && p1.normalMapTexture == p2.normalMapTexture && p1.tileSize == p2.tileSize && p1.tileOffset == p2.tileOffset && p1.smoothness == p2.smoothness)
			{
				return p1.metallic == p2.metallic;
			}
			return false;
		}

		[Obsolete]
		private static void Stamp(float[,,] arr, int arrChannel, float[,,] stamp, Coord stampOffset, int stampChannel, Coord center, int radius)
		{
			int length = stamp.GetLength(1);
			int length2 = stamp.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					int num = stampOffset.x + i;
					int num2 = stampOffset.z + j;
					if (!(Mathf.Sqrt((num - center.x) * (num - center.x) + (num2 - center.z) * (num2 - center.z)) > (float)radius))
					{
						arr[num2, num, arrChannel] = stamp[j, i, stampChannel];
					}
				}
			}
		}
	}
}
