using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Features.ItemSpawnerModule
{
	public sealed class SimpleItemSpawner : ItemSpawnerBase
	{
		[Header("Prefab")]
		[SerializeField]
		private NetworkObject _prefab;

		public NetworkObject Prefab => _prefab;

		protected override bool CanSpawn()
		{
			return _prefab != null;
		}

		protected override GameObject GetPreviewSource()
		{
			if (!(_prefab != null))
			{
				return null;
			}
			return _prefab.gameObject;
		}

		protected override async UniTask SpawnItemAsync()
		{
			if (!(_prefab == null))
			{
				GetSpawnPose(out var position, out var rotation);
				base.SpawnedInstance = await base.MultiplayerModel.NetworkRunner.SpawnAsync(_prefab, position, rotation, base.MultiplayerModel.NetworkRunner.LocalPlayer);
				ApplyParentToSpawnerIfNeeded();
			}
		}
	}
}
