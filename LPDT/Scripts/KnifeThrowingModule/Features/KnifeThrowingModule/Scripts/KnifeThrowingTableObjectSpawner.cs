using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KnifeThrowingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class KnifeThrowingTableObjectSpawner : NetworkBehaviour, ILevelContentSpawnProvider
	{
		[SerializeField]
		private NetworkObject _knifePrefab;

		[SerializeField]
		private NetworkObject _bottlePrefab;

		[SerializeField]
		private Transform[] _knifesSpawnPositions;

		[SerializeField]
		private Transform[] _bottlesSpawnPositions;

		private MultiplayerModel _multiplayerModel;

		private ILevelContentSpawnRegistry _levelContentSpawnRegistry;

		private IKnifeThrowingTableService _knifeThrowingTableService;

		public IReadOnlyList<Vector3> KnifeHomePositions => WorldPositions(_knifesSpawnPositions);

		public IReadOnlyList<Vector3> BottleHomePositions => WorldPositions(_bottlesSpawnPositions);

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel, ILevelContentSpawnRegistry levelContentSpawnRegistry, IKnifeThrowingTableService knifeThrowingTableService)
		{
			_multiplayerModel = multiplayerModel;
			_levelContentSpawnRegistry = levelContentSpawnRegistry;
			_knifeThrowingTableService = knifeThrowingTableService;
		}

		private void Start()
		{
			_knifeThrowingTableService.ConfigureObjectSpawning(this);
			_levelContentSpawnRegistry.Register(this);
			Debug.Log($"[KnifeSpawn] Table registered as content provider. table.world={base.transform.position}. If this logs AFTER '[ContentSpawn] SpawnAllAsync firing', the content gate already ran without the knife table — knives will not spawn from the gate.");
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_levelContentSpawnRegistry.Unregister(this);
			_knifeThrowingTableService.ClearTableSessionState();
		}

		public async UniTask SpawnLevelContentAsync()
		{
			if (CheckForHost(out var _))
			{
				Debug.Log("[KnifeSpawn] SpawnLevelContentAsync skipped — not the shared-mode master.");
				return;
			}
			Debug.Log($"[KnifeSpawn] SpawnLevelContentAsync running on master. table.world={base.transform.position}, knifePoints={_knifesSpawnPositions.Length}, bottlePoints={_bottlesSpawnPositions.Length}.");
			_knifeThrowingTableService.ClearTableSessionState();
			await SpawnObjectsAsync(_knifePrefab, _knifesSpawnPositions);
			await SpawnObjectsAsync(_bottlePrefab, _bottlesSpawnPositions);
		}

		public async UniTask<KnifeThrowingTableObjectTeleport> SpawnObjectAtPositionAsync(Vector3 spawnPosition, ThrowTableItemType objectType)
		{
			if (CheckForHost(out var runner))
			{
				return null;
			}
			return await SpawnObject(GetPrefabForSpawn(objectType), runner, spawnPosition);
		}

		private NetworkObject GetPrefabForSpawn(ThrowTableItemType objectType)
		{
			switch (objectType)
			{
			case ThrowTableItemType.Knife:
				return _knifePrefab;
			case ThrowTableItemType.Bottle:
				return _bottlePrefab;
			default:
			{
				global::_003CPrivateImplementationDetails_003E.ThrowSwitchExpressionException(objectType);
				NetworkObject result = default(NetworkObject);
				return result;
			}
			}
		}

		private async UniTask SpawnObjectsAsync(NetworkObject prefab, Transform[] spawnPoints)
		{
			if (CheckForHost(out var runner))
			{
				return;
			}
			foreach (Transform spawnPosition in spawnPoints)
			{
				try
				{
					Debug.Log(string.Format("[KnifeSpawn] spawning '{0}' at spawnPoint.world={1} (local={2}, table.world={3}).", (prefab == null) ? "<null>" : prefab.name, spawnPosition.position, spawnPosition.localPosition, base.transform.position));
					KnifeThrowingTableObjectTeleport teleportable = await SpawnObject(prefab, runner, spawnPosition.position);
					_knifeThrowingTableService.RegisterObject(teleportable);
				}
				catch (Exception ex) when (!(ex is OperationCanceledException))
				{
					Debug.LogError(string.Format("[KnifeThrowingTableObjectSpawner] failed to spawn '{0}' at {1} — continuing with the remaining table objects. {2}", (prefab == null) ? "<null prefab>" : prefab.name, spawnPosition.position, ex));
				}
			}
		}

		public IReadOnlyList<KnifeThrowingTableObjectTeleport> FindLiveTableObjects()
		{
			List<KnifeThrowingTableObjectTeleport> list = new List<KnifeThrowingTableObjectTeleport>();
			foreach (NetworkObject allNetworkObject in _multiplayerModel.NetworkRunner.GetAllNetworkObjects())
			{
				if (allNetworkObject != null && allNetworkObject.IsValid && allNetworkObject.TryGetComponent<KnifeThrowingTableObjectTeleport>(out var component))
				{
					list.Add(component);
				}
			}
			return list;
		}

		private List<Vector3> WorldPositions(Transform[] transforms)
		{
			List<Vector3> list = new List<Vector3>(transforms.Length);
			foreach (Transform transform in transforms)
			{
				list.Add(transform.position);
			}
			return list;
		}

		private bool CheckForHost(out NetworkRunner runner)
		{
			runner = _multiplayerModel.NetworkRunner;
			if (!runner.IsSharedModeMasterClient)
			{
				return true;
			}
			return false;
		}

		private async UniTask<KnifeThrowingTableObjectTeleport> SpawnObject(NetworkObject prefab, NetworkRunner runner, Vector3 spawnPosition)
		{
			NetworkObject obj = await runner.SpawnAsync(prefab, spawnPosition, Quaternion.identity, runner.LocalPlayer);
			obj.transform.SetParent(base.transform, worldPositionStays: true);
			if (!obj.TryGetComponent<KnifeThrowingTableObjectTeleport>(out var component))
			{
				return component;
			}
			component.SetSpawnPose(spawnPosition);
			return component;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
