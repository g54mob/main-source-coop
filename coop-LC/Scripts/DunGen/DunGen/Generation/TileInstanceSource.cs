using System.Collections.Generic;
using DunGen.Pooling;
using UnityEngine;

namespace DunGen.Generation
{
	public class TileInstanceSource
	{
		protected readonly BucketedObjectPool<Tile, Tile> tilePool;

		protected bool enableTilePooling;

		protected GameObject dungeonRoot;

		protected Transform tilePoolRoot;

		protected TilePoolPreloader tilePoolPreloader;

		public event TileInstanceSpawnedDelegate TileInstanceSpawned;

		public event TileInstanceDespawnedDelegate TileInstanceDespawned;

		public TileInstanceSource()
		{
			tilePool = new BucketedObjectPool<Tile, Tile>(delegate(Tile template)
			{
				Tile tile = Object.Instantiate(template);
				if (tile.TryGetComponent<Tile>(out var component))
				{
					component.RefreshTileEventReceivers();
				}
				return tile;
			});
		}

		public virtual void Initialise(bool enableTilePooling, GameObject dungeonRoot)
		{
			this.enableTilePooling = enableTilePooling;
			this.dungeonRoot = dungeonRoot;
			if (enableTilePooling)
			{
				if (tilePoolPreloader == null)
				{
					TryPreloadTilePool();
				}
				if (tilePoolRoot == null)
				{
					GameObject gameObject = new GameObject("Tile Pool");
					gameObject.SetActive(value: false);
					tilePoolRoot = gameObject.transform;
				}
			}
		}

		protected void TryPreloadTilePool()
		{
			tilePoolPreloader = UnityUtil.FindObjectByType<TilePoolPreloader>();
			if (tilePoolPreloader == null)
			{
				return;
			}
			tilePoolPreloader.gameObject.SetActive(value: false);
			tilePoolRoot = tilePoolPreloader.transform;
			foreach (TilePoolPreloaderEntry entry in tilePoolPreloader.Entries)
			{
				Tile tilePrefab = entry.TilePrefab;
				if (tilePrefab == null)
				{
					continue;
				}
				IEnumerable<Tile> tileInstancesForPrefab = tilePoolPreloader.GetTileInstancesForPrefab(tilePrefab);
				if (tileInstancesForPrefab == null)
				{
					continue;
				}
				foreach (Tile item in tileInstancesForPrefab)
				{
					item.gameObject.SetActive(value: true);
					item.RefreshTileEventReceivers();
					tilePool.InsertObject(tilePrefab, item);
				}
			}
		}

		public virtual Tile SpawnTile(Tile tilePrefab, Vector3 position, Quaternion rotation)
		{
			if (enableTilePooling)
			{
				Tile obj;
				bool fromPool = tilePool.TryTakeObject(tilePrefab, out obj);
				Transform transform = obj.transform;
				transform.parent = dungeonRoot.transform;
				transform.localPosition = position;
				transform.localRotation = rotation;
				obj.TileSpawned();
				this.TileInstanceSpawned?.Invoke(tilePrefab, obj, fromPool);
				return obj;
			}
			Tile tile = Object.Instantiate(tilePrefab, dungeonRoot.transform);
			tile.transform.localPosition = position;
			tile.transform.localRotation = rotation;
			if (tile.TryGetComponent<Tile>(out var component))
			{
				component.RefreshTileEventReceivers();
				component.TileSpawned();
				TileInstanceSpawnedDelegate tileInstanceSpawnedDelegate = this.TileInstanceSpawned;
				if (tileInstanceSpawnedDelegate == null)
				{
					return tile;
				}
				tileInstanceSpawnedDelegate(tilePrefab, component, fromPool: false);
			}
			return tile;
		}

		public virtual void DespawnTile(Tile tileInstance)
		{
			bool flag = false;
			if (enableTilePooling)
			{
				flag = tilePool.ReturnObject(tileInstance);
				if (flag)
				{
					tileInstance.transform.parent = tilePoolRoot;
					tileInstance.TileDespawned();
					this.TileInstanceDespawned?.Invoke(tileInstance);
				}
			}
			if (!flag)
			{
				tileInstance.TileDespawned();
				this.TileInstanceDespawned?.Invoke(tileInstance);
				Object.DestroyImmediate(tileInstance.gameObject);
			}
		}
	}
}
