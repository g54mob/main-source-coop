using UnityEngine;

namespace NomadDrive.Features.Vehicle.Collision
{
	public static class TerrainTreeCollisionHelper
	{
		public static bool FindNearestTreeInstance(Terrain terrain, Vector3 worldPos, float searchRadius, out int treeIndex, out Vector3 treeWorldPos, out int prototypeIndex)
		{
			treeIndex = -1;
			treeWorldPos = Vector3.zero;
			prototypeIndex = -1;
			if (terrain == null)
			{
				return false;
			}
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = terrain.transform.position;
			Vector3 size = terrainData.size;
			TreeInstance[] treeInstances = terrainData.treeInstances;
			if (treeInstances.Length == 0)
			{
				return false;
			}
			float num = (worldPos.x - position.x) / size.x;
			float num2 = (worldPos.z - position.z) / size.z;
			float num3 = searchRadius / Mathf.Max(size.x, size.z);
			float num4 = searchRadius * searchRadius;
			bool result = false;
			for (int i = 0; i < treeInstances.Length; i++)
			{
				TreeInstance tree = treeInstances[i];
				float num5 = tree.position.x - num;
				float num6 = tree.position.z - num2;
				if (!(num5 * num5 + num6 * num6 > num3 * num3 * 4f))
				{
					Vector3 vector = CalculateTreeWorldPosition(terrain, tree);
					float sqrMagnitude = (vector - worldPos).sqrMagnitude;
					if (sqrMagnitude < num4)
					{
						num4 = sqrMagnitude;
						treeIndex = i;
						treeWorldPos = vector;
						prototypeIndex = tree.prototypeIndex;
						result = true;
					}
				}
			}
			return result;
		}

		public static void RemoveTreeInstance(Terrain terrain, int treeIndex)
		{
			TerrainData terrainData = terrain.terrainData;
			TreeInstance[] treeInstances = terrainData.treeInstances;
			if (treeIndex < 0 || treeIndex >= treeInstances.Length)
			{
				return;
			}
			TreeInstance[] array = new TreeInstance[treeInstances.Length - 1];
			int num = 0;
			for (int i = 0; i < treeInstances.Length; i++)
			{
				if (i != treeIndex)
				{
					array[num++] = treeInstances[i];
				}
			}
			terrainData.treeInstances = array;
			TerrainCollider component = terrain.GetComponent<TerrainCollider>();
			if (component != null)
			{
				component.enabled = false;
				component.enabled = true;
			}
		}

		public static GameObject GetTreePrototypePrefab(Terrain terrain, int prototypeIndex)
		{
			TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;
			if (prototypeIndex < 0 || prototypeIndex >= treePrototypes.Length)
			{
				return null;
			}
			return treePrototypes[prototypeIndex].prefab;
		}

		public static Vector3 CalculateTreeWorldPosition(Terrain terrain, TreeInstance tree)
		{
			Vector3 position = terrain.transform.position;
			Vector3 size = terrain.terrainData.size;
			return new Vector3(tree.position.x * size.x + position.x, tree.position.y * size.y + position.y, tree.position.z * size.z + position.z);
		}

		public static Quaternion CalculateTreeRotation(TreeInstance tree)
		{
			return Quaternion.Euler(0f, tree.rotation * 57.29578f, 0f);
		}
	}
}
