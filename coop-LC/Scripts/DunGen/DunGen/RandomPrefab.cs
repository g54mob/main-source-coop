using System.Collections.Generic;
using DunGen.Pooling;
using UnityEngine;

namespace DunGen
{
	[AddComponentMenu("DunGen/Random Props/Random Prefab")]
	public class RandomPrefab : RandomProp, ITileSpawnEventReceiver
	{
		[AcceptGameObjectTypes(GameObjectFilter.Asset)]
		public GameObjectChanceTable Props = new GameObjectChanceTable();

		public bool ZeroPosition = true;

		public bool ZeroRotation = true;

		private GameObject propInstance;

		private void ClearExistingInstances()
		{
			if (!(propInstance == null))
			{
				Object.DestroyImmediate(propInstance);
				propInstance = null;
			}
		}

		public override void Process(RandomStream randomStream, Tile tile, ref List<GameObject> spawnedObjects)
		{
			ClearExistingInstances();
			if (Props.Weights.Count <= 0)
			{
				return;
			}
			GameObjectChance random = Props.GetRandom(randomStream, tile.Placement.IsOnMainPath, tile.Placement.NormalizedDepth, null, allowImmediateRepeats: true, removeFromTable: false, allowNullSelection: true);
			if (random != null && !(random.Value == null))
			{
				GameObject value = random.Value;
				propInstance = Object.Instantiate(value);
				propInstance.transform.parent = base.transform;
				spawnedObjects.Add(propInstance);
				if (ZeroPosition)
				{
					propInstance.transform.localPosition = Vector3.zero;
				}
				else
				{
					propInstance.transform.localPosition = value.transform.localPosition;
				}
				if (ZeroRotation)
				{
					propInstance.transform.localRotation = Quaternion.identity;
				}
				else
				{
					propInstance.transform.localRotation = value.transform.localRotation;
				}
			}
		}

		public void OnTileSpawned(Tile tile)
		{
		}

		public void OnTileDespawned(Tile tile)
		{
			ClearExistingInstances();
		}
	}
}
