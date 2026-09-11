using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen.Pooling
{
	[DisallowMultipleComponent]
	[AddComponentMenu("DunGen/Pooling/Tile Pool Pre-loader")]
	public class TilePoolPreloader : MonoBehaviour
	{
		[Serializable]
		public sealed class SpawnedTileInstances
		{
			public Tile TilePrefab;

			public List<Tile> Instances = new List<Tile>();
		}

		public List<TilePoolPreloaderEntry> Entries = new List<TilePoolPreloaderEntry>();

		[SerializeField]
		private List<SpawnedTileInstances> spawnedTileInstances = new List<SpawnedTileInstances>();

		public void ClearSpawnedInstances()
		{
			foreach (SpawnedTileInstances spawnedTileInstance in spawnedTileInstances)
			{
				foreach (Tile instance in spawnedTileInstance.Instances)
				{
					if (instance != null)
					{
						UnityEngine.Object.DestroyImmediate(instance.gameObject);
					}
				}
			}
			spawnedTileInstances.Clear();
		}

		public IEnumerable<Tile> GetTileInstancesForPrefab(Tile prefab)
		{
			if (prefab == null)
			{
				return null;
			}
			return spawnedTileInstances.Find((SpawnedTileInstances x) => x.TilePrefab == prefab)?.Instances;
		}

		public bool HasSpawnedInstances()
		{
			return spawnedTileInstances.Count > 0;
		}

		public void RefreshTileInstances()
		{
			for (int num = this.spawnedTileInstances.Count - 1; num >= 0; num--)
			{
				SpawnedTileInstances entry = this.spawnedTileInstances[num];
				if (!(entry.TilePrefab != null) || !Entries.Exists((TilePoolPreloaderEntry x) => x.TilePrefab == entry.TilePrefab))
				{
					foreach (Tile instance in entry.Instances)
					{
						if (instance != null)
						{
							UnityEngine.Object.DestroyImmediate(instance.gameObject);
						}
					}
					this.spawnedTileInstances.RemoveAt(num);
				}
			}
			foreach (TilePoolPreloaderEntry entry2 in Entries)
			{
				if (entry2.TilePrefab == null)
				{
					continue;
				}
				SpawnedTileInstances spawnedTileInstances = this.spawnedTileInstances.Find((SpawnedTileInstances x) => x.TilePrefab == entry2.TilePrefab);
				if (spawnedTileInstances == null)
				{
					spawnedTileInstances = new SpawnedTileInstances
					{
						TilePrefab = entry2.TilePrefab
					};
					this.spawnedTileInstances.Add(spawnedTileInstances);
				}
				spawnedTileInstances.Instances.RemoveAll((Tile x) => x == null);
				int count = spawnedTileInstances.Instances.Count;
				if (count < entry2.Count)
				{
					int num2 = entry2.Count - count;
					for (int num3 = 0; num3 < num2; num3++)
					{
						Tile tile = UnityEngine.Object.Instantiate(entry2.TilePrefab, base.transform);
						tile.gameObject.SetActive(value: false);
						spawnedTileInstances.Instances.Add(tile);
					}
				}
				else
				{
					if (count <= entry2.Count)
					{
						continue;
					}
					for (int num4 = count - 1; num4 >= entry2.Count; num4--)
					{
						Tile tile2 = spawnedTileInstances.Instances[num4];
						spawnedTileInstances.Instances.RemoveAt(num4);
						if (tile2 != null)
						{
							UnityEngine.Object.DestroyImmediate(tile2.gameObject);
						}
					}
				}
			}
		}
	}
}
