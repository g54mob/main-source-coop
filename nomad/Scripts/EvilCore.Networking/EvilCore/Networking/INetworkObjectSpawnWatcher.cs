using System;
using UnityEngine;

namespace EvilCore.Networking
{
	public interface INetworkObjectSpawnWatcher
	{
		void RegisterPendingLookup(uint netId, Action<GameObject> callback, string requesterName = null, float timeoutSeconds = 0f);

		void UnregisterPendingLookup(uint netId, Action<GameObject> callback);

		void ClearAllPendingLookups();

		void NotifyObjectSpawned(uint netId, GameObject spawnedObject);

		int GetPendingLookupCount();
	}
}
