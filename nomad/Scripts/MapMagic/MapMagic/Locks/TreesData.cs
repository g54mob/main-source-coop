using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes;
using MapMagic.Nodes.ObjectsGenerators;
using UnityEngine;

namespace MapMagic.Locks
{
	public class TreesData : ILockData
	{
		public TreeInstance[] lockInstances;

		public TreePrototype[] lockPrototypes;

		public Vector2D center;

		public float radius;

		public float transition;

		public void Read(Terrain terrain, Lock lk)
		{
			Vector3 position = terrain.transform.position;
			TerrainData terrainData = terrain.terrainData;
			Vector3 size = terrainData.size;
			center = new Vector2D((lk.worldPos.x - position.x) / size.x, (lk.worldPos.z - position.z) / size.z);
			radius = lk.worldRadius / size.x;
			transition = lk.worldTransition / size.x;
			lockInstances = TreesInRange(terrainData.treeInstances, center, radius + transition).ToArray();
			lockPrototypes = terrainData.treePrototypes;
		}

		public void WriteInThread(IApplyData applyData)
		{
			if (applyData is TreesOutput.ApplyTreesData { treeInstances: var baseInstances, treePrototypes: var basePrototypes } applyTreesData)
			{
				UnifyPrototypes(ref basePrototypes, ref baseInstances, ref lockPrototypes, ref lockInstances);
				List<TreeInstance> list = TreesOutRange(baseInstances, center, radius + transition);
				list.AddRange(lockInstances);
				applyTreesData.treeInstances = list.ToArray();
				applyTreesData.treePrototypes = lockPrototypes;
			}
		}

		public void WriteInApply(Terrain terrain, bool resizeTerrain = false)
		{
			if (lockInstances != null && lockPrototypes != null)
			{
				terrain.terrainData.treePrototypes = lockPrototypes;
				List<TreeInstance> list = TreesOutRange(terrain.terrainData.treeInstances, center, radius + transition);
				list.AddRange(lockInstances);
				terrain.terrainData.treeInstances = list.ToArray();
			}
		}

		public void ApplyHeightDelta(Matrix srcHeights, Matrix dstHeights)
		{
			Vector2D vector2D = center - (radius + transition);
			float num = radius * 2f + transition * 2f;
			for (int i = 0; i < lockInstances.Length; i++)
			{
				Vector2D vector2D2 = ((Vector2D)lockInstances[i].position - vector2D) / num;
				Vector2D vector2D3 = new Vector2D(vector2D2.x * (float)srcHeights.rect.size.x + (float)srcHeights.rect.offset.x, vector2D2.z * (float)srcHeights.rect.size.z + (float)srcHeights.rect.offset.z);
				float interpolated = srcHeights.GetInterpolated(vector2D3.x, vector2D3.z);
				float num2 = dstHeights.GetInterpolated(vector2D3.x, vector2D3.z) - interpolated;
				lockInstances[i].position.y += num2;
			}
		}

		public void ResizeFrom(ILockData src)
		{
		}

		private static List<TreeInstance> TreesInRange(TreeInstance[] srcInstances, Vector2D center, float radius)
		{
			Vector2D vector2D = new Vector2D(center.x - radius, center.z - radius);
			Vector2D vector2D2 = new Vector2D(center.x + radius, center.z + radius);
			List<TreeInstance> list = new List<TreeInstance>();
			for (int i = 0; i < srcInstances.Length; i++)
			{
				TreeInstance item = srcInstances[i];
				Vector3 position = item.position;
				if (!(position.x < vector2D.x) && !(position.z < vector2D.z) && !(position.x > vector2D2.x) && !(position.z > vector2D2.z) && !(Mathf.Sqrt((position.x - center.x) * (position.x - center.x) + (position.z - center.z) * (position.z - center.z)) > radius))
				{
					list.Add(item);
				}
			}
			return list;
		}

		private static List<TreeInstance> TreesOutRange(TreeInstance[] srcInstances, Vector2D center, float radius)
		{
			List<TreeInstance> list = new List<TreeInstance>();
			for (int i = 0; i < srcInstances.Length; i++)
			{
				TreeInstance item = srcInstances[i];
				Vector3 position = item.position;
				if (!(Mathf.Sqrt((position.x - center.x) * (position.x - center.x) + (position.z - center.z) * (position.z - center.z)) < radius))
				{
					list.Add(item);
				}
			}
			return list;
		}

		private static void UnifyPrototypes(ref TreePrototype[] basePrototypes, ref TreeInstance[] baseInstances, ref TreePrototype[] addPrototypes, ref TreeInstance[] addInstances)
		{
			if (ArrayTools.MatchExactly(basePrototypes, addPrototypes))
			{
				return;
			}
			List<TreePrototype> list = new List<TreePrototype>();
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
			for (int l = 0; l < baseInstances.Length; l++)
			{
				baseInstances[l].prototypeIndex = dictionary[baseInstances[l].prototypeIndex];
			}
			for (int m = 0; m < addInstances.Length; m++)
			{
				addInstances[m].prototypeIndex = dictionary2[addInstances[m].prototypeIndex];
			}
			basePrototypes = list.ToArray();
			addPrototypes = list.ToArray();
		}
	}
}
