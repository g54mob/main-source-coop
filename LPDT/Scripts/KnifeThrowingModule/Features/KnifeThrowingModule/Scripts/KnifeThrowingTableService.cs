using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.TeleportModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.KnifeThrowingModule.Scripts
{
	public class KnifeThrowingTableService : IKnifeThrowingTableService
	{
		private const float POSITION_MATCH_SQR_THRESHOLD = 0.0001f;

		private readonly ITeleportService _teleportService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly Dictionary<NetworkId, KnifeThrowingTableObjectTeleport> _knives = new Dictionary<NetworkId, KnifeThrowingTableObjectTeleport>();

		private readonly Dictionary<NetworkId, KnifeThrowingTableObjectTeleport> _bottles = new Dictionary<NetworkId, KnifeThrowingTableObjectTeleport>();

		private readonly List<Vector3> _freeBottleSpawnPositions = new List<Vector3>();

		private readonly List<Vector3> _freeKnifeSpawnPositions = new List<Vector3>();

		private KnifeThrowingTableObjectSpawner _objectSpawner;

		public KnifeThrowingTableService(ITeleportService teleportService, MultiplayerModel multiplayerModel)
		{
			_teleportService = teleportService;
			_multiplayerModel = multiplayerModel;
		}

		public void ConfigureObjectSpawning(KnifeThrowingTableObjectSpawner spawner)
		{
			_objectSpawner = spawner;
		}

		public void RegisterObject(KnifeThrowingTableObjectTeleport teleportable)
		{
			switch (teleportable.ThrowTableItemType)
			{
			case ThrowTableItemType.Knife:
				RegisterKnife(teleportable);
				break;
			case ThrowTableItemType.Bottle:
				RegisterBottle(teleportable);
				break;
			}
		}

		public void TryResetObjectsPosition()
		{
			ResetObjectsAsync().Forget();
		}

		public void ClearTableSessionState()
		{
			_freeBottleSpawnPositions.Clear();
			_freeKnifeSpawnPositions.Clear();
			UnsubscribeObjectsEvents();
			_knives.Clear();
			_bottles.Clear();
		}

		private void RegisterBottle(KnifeThrowingTableObjectTeleport teleportable)
		{
			if (RegisterTrackedObject(_bottles, teleportable))
			{
				teleportable.OnRemovedFromTable += HandleObjectRemoved;
			}
		}

		private void RegisterKnife(KnifeThrowingTableObjectTeleport teleportable)
		{
			if (RegisterTrackedObject(_knives, teleportable))
			{
				teleportable.OnRemovedFromTable += HandleObjectRemoved;
			}
		}

		private bool RegisterTrackedObject(Dictionary<NetworkId, KnifeThrowingTableObjectTeleport> trackedObjects, KnifeThrowingTableObjectTeleport teleportable)
		{
			if (teleportable == null || teleportable.Object == null)
			{
				return false;
			}
			trackedObjects[teleportable.Object.Id] = teleportable;
			return true;
		}

		private void HandleObjectRemoved(KnifeThrowingTableObjectTeleport teleportable)
		{
			if (teleportable?.Object == null)
			{
				return;
			}
			ThrowTableItemType throwTableItemType = teleportable.ThrowTableItemType;
			NetworkId id = teleportable.Object.Id;
			if (!TryRemoveFromDict(throwTableItemType, id))
			{
				return;
			}
			teleportable.OnRemovedFromTable -= HandleObjectRemoved;
			if (teleportable.HasSpawnPose)
			{
				if (throwTableItemType == ThrowTableItemType.Bottle)
				{
					_freeBottleSpawnPositions.Add(teleportable.SpawnPosition);
				}
				if (throwTableItemType == ThrowTableItemType.Knife)
				{
					_freeKnifeSpawnPositions.Add(teleportable.SpawnPosition);
				}
			}
		}

		private bool TryRemoveFromDict(ThrowTableItemType objectType, NetworkId id)
		{
			if (objectType == ThrowTableItemType.Bottle && !_bottles.Remove(id))
			{
				return false;
			}
			if (objectType == ThrowTableItemType.Knife && !_knives.Remove(id))
			{
				return false;
			}
			return true;
		}

		private async UniTaskVoid ResetObjectsAsync()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient)
			{
				RebuildTrackingFromLive();
				await ResetTrackedObjectsAsync(_knives);
				await RespawnDestroyedObjectsAsync(_freeKnifeSpawnPositions, ThrowTableItemType.Knife);
				await ResetTrackedObjectsAsync(_bottles);
				await RespawnDestroyedObjectsAsync(_freeBottleSpawnPositions, ThrowTableItemType.Bottle);
			}
		}

		private async UniTask ResetTrackedObjectsAsync(Dictionary<NetworkId, KnifeThrowingTableObjectTeleport> trackedObjects)
		{
			KnifeThrowingTableObjectTeleport[] array = new KnifeThrowingTableObjectTeleport[trackedObjects.Count];
			trackedObjects.Values.CopyTo(array, 0);
			KnifeThrowingTableObjectTeleport[] array2 = array;
			foreach (KnifeThrowingTableObjectTeleport knifeThrowingTableObjectTeleport in array2)
			{
				if (!(knifeThrowingTableObjectTeleport?.NetworkObject == null) && knifeThrowingTableObjectTeleport.NetworkObject.IsValid && knifeThrowingTableObjectTeleport.CanTeleport)
				{
					await _teleportService.TeleportObject(knifeThrowingTableObjectTeleport, knifeThrowingTableObjectTeleport.NetworkObject.transform.position);
				}
			}
		}

		private async UniTask RespawnDestroyedObjectsAsync(List<Vector3> freePositions, ThrowTableItemType objectType)
		{
			if (_objectSpawner == null || freePositions.Count == 0)
			{
				return;
			}
			Vector3[] array = freePositions.ToArray();
			freePositions.Clear();
			Vector3[] array2 = array;
			foreach (Vector3 spawnPosition in array2)
			{
				Debug.Log($"[KnifeSpawn] Respawning destroyed {objectType} at freed spawn pos {spawnPosition}.");
				KnifeThrowingTableObjectTeleport knifeThrowingTableObjectTeleport = await _objectSpawner.SpawnObjectAtPositionAsync(spawnPosition, objectType);
				if (knifeThrowingTableObjectTeleport == null)
				{
					freePositions.Add(spawnPosition);
				}
				else
				{
					RegisterObject(knifeThrowingTableObjectTeleport);
				}
			}
		}

		private void RebuildTrackingFromLive()
		{
			if (_objectSpawner == null)
			{
				return;
			}
			ClearTableSessionState();
			foreach (KnifeThrowingTableObjectTeleport item in _objectSpawner.FindLiveTableObjects())
			{
				RegisterObject(item);
			}
			RebuildFreePositions(_objectSpawner.KnifeHomePositions, _knives, _freeKnifeSpawnPositions);
			RebuildFreePositions(_objectSpawner.BottleHomePositions, _bottles, _freeBottleSpawnPositions);
		}

		private void RebuildFreePositions(IReadOnlyList<Vector3> homePositions, Dictionary<NetworkId, KnifeThrowingTableObjectTeleport> trackedObjects, List<Vector3> freePositions)
		{
			freePositions.Clear();
			foreach (Vector3 homePosition in homePositions)
			{
				bool flag = false;
				foreach (KnifeThrowingTableObjectTeleport value in trackedObjects.Values)
				{
					if (value.HasSpawnPose && (value.SpawnPosition - homePosition).sqrMagnitude < 0.0001f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					freePositions.Add(homePosition);
				}
			}
		}

		private void UnsubscribeObjectsEvents()
		{
			foreach (KeyValuePair<NetworkId, KnifeThrowingTableObjectTeleport> bottle in _bottles)
			{
				bottle.Value.OnRemovedFromTable -= HandleObjectRemoved;
			}
			foreach (KeyValuePair<NetworkId, KnifeThrowingTableObjectTeleport> knife in _knives)
			{
				knife.Value.OnRemovedFromTable -= HandleObjectRemoved;
			}
		}
	}
}
