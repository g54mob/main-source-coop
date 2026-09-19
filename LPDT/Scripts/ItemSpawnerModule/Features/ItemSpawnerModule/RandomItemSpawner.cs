using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Features.ItemSpawnerModule
{
	public sealed class RandomItemSpawner : ItemSpawnerBase
	{
		[Header("Prefabs pool")]
		[SerializeField]
		private List<NetworkObject> _prefabPool;

		protected override bool CanSpawn()
		{
			return _prefabPool.Count > 0;
		}

		protected override GameObject GetPreviewSource()
		{
			if (_prefabPool.Count != 0)
			{
				return _prefabPool[0].gameObject;
			}
			return null;
		}

		protected override async UniTask SpawnItemAsync()
		{
			GetSpawnPose(out var position, out var rotation);
			NetworkObject prefab = _prefabPool[Random.Range(0, _prefabPool.Count)];
			base.SpawnedInstance = await base.MultiplayerModel.NetworkRunner.SpawnAsync(prefab, position, rotation, base.MultiplayerModel.NetworkRunner.LocalPlayer);
			ApplyParentToSpawnerIfNeeded();
		}
	}
}
