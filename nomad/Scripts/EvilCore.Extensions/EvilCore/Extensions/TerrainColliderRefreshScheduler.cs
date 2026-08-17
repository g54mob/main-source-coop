using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class TerrainColliderRefreshScheduler
	{
		private static readonly HashSet<Terrain> _dirtyTerrains = new HashSet<Terrain>();

		private static readonly Dictionary<Terrain, List<TreeInstance>> _pendingTreeEdits = new Dictionary<Terrain, List<TreeInstance>>();

		public static void MarkDirty(Terrain terrain)
		{
			if (terrain != null)
			{
				_dirtyTerrains.Add(terrain);
			}
		}

		public static List<TreeInstance> GetPendingTrees(Terrain terrain)
		{
			if (terrain == null || terrain.terrainData == null)
			{
				return new List<TreeInstance>();
			}
			if (_pendingTreeEdits.TryGetValue(terrain, out var value))
			{
				return value;
			}
			return new List<TreeInstance>(terrain.terrainData.treeInstances);
		}

		public static void SetPendingTrees(Terrain terrain, List<TreeInstance> survivors)
		{
			if (terrain != null && survivors != null)
			{
				_pendingTreeEdits[terrain] = survivors;
			}
		}

		public static void DropPendingTrees(Terrain terrain)
		{
			if (terrain != null)
			{
				_pendingTreeEdits.Remove(terrain);
			}
		}

		public static async UniTask FlushAllAsync()
		{
			if (_pendingTreeEdits.Count > 0)
			{
				List<KeyValuePair<Terrain, List<TreeInstance>>> treeEdits = new List<KeyValuePair<Terrain, List<TreeInstance>>>(_pendingTreeEdits);
				_pendingTreeEdits.Clear();
				for (int i = 0; i < treeEdits.Count; i++)
				{
					ApplyTreeEdit(treeEdits[i].Key, treeEdits[i].Value);
					await UniTask.Yield();
				}
			}
			if (_dirtyTerrains.Count != 0)
			{
				List<Terrain> terrains = new List<Terrain>(_dirtyTerrains);
				_dirtyTerrains.Clear();
				for (int i = 0; i < terrains.Count; i++)
				{
					RefreshCollider(terrains[i]);
					await UniTask.Yield();
				}
			}
		}

		public static void Clear()
		{
			_dirtyTerrains.Clear();
			_pendingTreeEdits.Clear();
		}

		private static void ApplyTreeEdit(Terrain terrain, List<TreeInstance> survivors)
		{
			if (!(terrain == null) && !(terrain.terrainData == null) && survivors != null)
			{
				terrain.terrainData.treeInstances = survivors.ToArray();
			}
		}

		private static void RefreshCollider(Terrain terrain)
		{
			if (!(terrain == null))
			{
				TerrainCollider component = terrain.GetComponent<TerrainCollider>();
				if (!(component == null))
				{
					component.enabled = false;
					component.enabled = true;
				}
			}
		}
	}
}
