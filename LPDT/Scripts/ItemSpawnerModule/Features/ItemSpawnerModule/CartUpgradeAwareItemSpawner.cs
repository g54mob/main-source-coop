using Cysharp.Threading.Tasks;
using Features.CartUpgradesModule.Scripts.Core;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.ItemSpawnerModule
{
	public sealed class CartUpgradeAwareItemSpawner : ItemSpawnerBase
	{
		private const float UPGRADE_STATE_TIMEOUT_SECONDS = 5f;

		[Header("Prefab")]
		[SerializeField]
		private NetworkObject _prefab;

		private ICartUpgradeService _cartUpgradeService;

		public NetworkObject Prefab => _prefab;

		[Inject]
		public void InjectDependencies(ICartUpgradeService cartUpgradeService)
		{
			_cartUpgradeService = cartUpgradeService;
		}

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
			await WaitForUpgradeStateAsync();
			NetworkObject networkObject = ResolvePrefab();
			if (!(networkObject == null))
			{
				GetSpawnPose(out var position, out var rotation);
				base.SpawnedInstance = await base.MultiplayerModel.NetworkRunner.SpawnAsync(networkObject, position, rotation, base.MultiplayerModel.NetworkRunner.LocalPlayer);
				ApplyParentToSpawnerIfNeeded();
			}
		}

		private async UniTask WaitForUpgradeStateAsync()
		{
			if (_cartUpgradeService != null && !_cartUpgradeService.IsStateReady)
			{
				float deadline = Time.realtimeSinceStartup + 5f;
				await UniTask.WaitUntil(() => _cartUpgradeService.IsStateReady || Time.realtimeSinceStartup >= deadline);
			}
		}

		private NetworkObject ResolvePrefab()
		{
			if (_prefab == null || _cartUpgradeService == null)
			{
				return _prefab;
			}
			if (!_cartUpgradeService.TryResolveCartPrefab(_prefab.gameObject, out var cartPrefab))
			{
				return _prefab;
			}
			return cartPrefab;
		}
	}
}
