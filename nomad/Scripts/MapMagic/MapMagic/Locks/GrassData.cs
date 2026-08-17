using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes;
using MapMagic.Nodes.MatrixGenerators;
using UnityEngine;

namespace MapMagic.Locks
{
	public class GrassData : ILockData
	{
		public int[][,] lockLayers;

		public DetailPrototype[] lockPrototypes;

		public int patchResolution = 16;

		private CoordCircle circle;

		private int resolution;

		public void Read(Terrain terrain, Lock lk)
		{
			TerrainData terrainData = terrain.terrainData;
			if (terrainData.detailPrototypes == null || terrainData.detailPrototypes.Length == 0)
			{
				lockLayers = null;
				lockPrototypes = null;
				return;
			}
			resolution = terrain.terrainData.detailResolution;
			circle = new CoordCircle(terrain, resolution, lk.worldPos, lk.worldRadius, lk.worldTransition);
			lockPrototypes = terrainData.detailPrototypes;
			lockLayers = new int[lockPrototypes.Length][,];
			for (int i = 0; i < lockPrototypes.Length; i++)
			{
				int[,] detailLayer = terrainData.GetDetailLayer(circle.rect.offset.x, circle.rect.offset.z, circle.rect.size.x, circle.rect.size.z, i);
				lockLayers[i] = detailLayer;
			}
			patchResolution = terrainData.detailResolutionPerPatch;
		}

		public void WriteInThread(IApplyData applyData)
		{
			if (!(applyData is GrassOutput200.ApplyData applyData2) || lockLayers == null || lockPrototypes == null || Mathf.ClosestPowerOfTwo(applyData2.Resolution) != resolution)
			{
				return;
			}
			UnifyPrototypes(ref applyData2.detailPrototypes, ref applyData2.detailLayers, ref lockPrototypes, ref lockLayers);
			for (int i = 0; i < lockPrototypes.Length; i++)
			{
				if (applyData2.detailLayers[i] == null)
				{
					applyData2.detailLayers[i] = new int[resolution, resolution];
				}
				if (lockLayers[i] == null)
				{
					lockLayers[i] = new int[circle.rect.size.x, circle.rect.size.z];
				}
				Stamp(applyData2.detailLayers[i], lockLayers[i], circle.rect.offset, circle.center, circle.fullRadius);
			}
		}

		public void WriteInApply(Terrain terrain, bool resizeTerrain = false)
		{
			TerrainData terrainData = terrain.terrainData;
			if (lockLayers == null || lockPrototypes == null)
			{
				return;
			}
			if (terrain.terrainData.detailResolution != resolution)
			{
				if (!resizeTerrain)
				{
					return;
				}
				terrainData.SetDetailResolution(resolution, patchResolution);
			}
			terrainData.detailPrototypes = lockPrototypes;
			for (int i = 0; i < lockLayers.Length; i++)
			{
				terrainData.SetDetailLayer(circle.rect.offset.x, circle.rect.offset.z, i, lockLayers[i]);
			}
		}

		public void ApplyHeightDelta(Matrix src, Matrix dst)
		{
		}

		public void ResizeFrom(ILockData otherData)
		{
			GrassData grassData = (GrassData)otherData;
			if (grassData.lockLayers == null || lockLayers == null)
			{
				return;
			}
			int num = Mathf.Min(grassData.lockLayers.Length, lockLayers.Length);
			if (num == 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				int[,] array = grassData.lockLayers[i];
				int[,] array2 = lockLayers[i];
				int length = array2.GetLength(0);
				int length2 = array2.GetLength(1);
				int length3 = array.GetLength(0);
				int length4 = array.GetLength(1);
				float num2 = 1f * (float)length3 / (float)length;
				float num3 = 1f * (float)length4 / (float)length2;
				for (int j = 0; j < length3; j++)
				{
					for (int k = 0; k < length4; k++)
					{
						int num4 = (int)((float)j / num2);
						int num5 = (int)((float)k / num3);
						array2[j, k] = array[num4, num5];
					}
				}
			}
		}

		private static void UnifyPrototypes(ref DetailPrototype[] basePrototypes, ref int[][,] baseData, ref DetailPrototype[] addPrototypes, ref int[][,] addData)
		{
			if (ArrayTools.MatchExactly(basePrototypes, addPrototypes))
			{
				return;
			}
			List<DetailPrototype> list = new List<DetailPrototype>();
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
			int[][,] array = new int[list.Count][,];
			int num = baseData.Length;
			for (int l = 0; l < num; l++)
			{
				array[dictionary[l]] = baseData[l];
			}
			baseData = array;
			int[][,] array2 = new int[list.Count][,];
			int num2 = addData.Length;
			for (int m = 0; m < num2; m++)
			{
				array2[dictionary2[m]] = addData[m];
			}
			addData = array2;
			basePrototypes = list.ToArray();
			addPrototypes = list.ToArray();
		}

		private static void Stamp(int[,] arr, int[,] stamp, Coord stampOffset, Coord center, int radius)
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
						arr[num2, num] = stamp[j, i];
					}
				}
			}
		}
	}
}
