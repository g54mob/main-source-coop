using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using UnityEngine;

namespace EvilCore.Networking
{
	public class NetworkObjectSpawnWatcher : MonoBehaviour, INetworkObjectSpawnWatcher
	{
		private class PendingLookup
		{
			public Action<GameObject> Callback;

			public float ExpirationTime;

			public string RequesterName;
		}

		[Header("Configuration")]
		[SerializeField]
		private float pollIntervalSeconds = 0.5f;

		[SerializeField]
		private float defaultTimeoutSeconds = 120f;

		private readonly Dictionary<uint, List<PendingLookup>> _pendingLookups = new Dictionary<uint, List<PendingLookup>>();

		private readonly object _lockObject = new object();

		private float _nextPollTime;

		private void Update()
		{
			if (!(Time.time < _nextPollTime))
			{
				_nextPollTime = Time.time + pollIntervalSeconds;
				PollForSpawnedObjects();
				CleanupExpiredLookups();
			}
		}

		public void RegisterPendingLookup(uint netId, Action<GameObject> callback, string requesterName = null, float timeoutSeconds = 0f)
		{
			if (netId == 0 || callback == null)
			{
				return;
			}
			if (TryGetSpawnedObject(netId, out var networkObject))
			{
				callback?.Invoke(networkObject);
				return;
			}
			float num = ((timeoutSeconds > 0f) ? timeoutSeconds : defaultTimeoutSeconds);
			PendingLookup item = new PendingLookup
			{
				Callback = callback,
				ExpirationTime = Time.time + num,
				RequesterName = (requesterName ?? "Unknown")
			};
			lock (_lockObject)
			{
				if (!_pendingLookups.TryGetValue(netId, out var value))
				{
					value = new List<PendingLookup>();
					_pendingLookups[netId] = value;
				}
				value.Add(item);
			}
		}

		public void UnregisterPendingLookup(uint netId, Action<GameObject> callback)
		{
			if (callback == null)
			{
				return;
			}
			lock (_lockObject)
			{
				if (_pendingLookups.TryGetValue(netId, out var value))
				{
					value.RemoveAll((PendingLookup p) => p.Callback == callback);
					if (value.Count == 0)
					{
						_pendingLookups.Remove(netId);
					}
				}
			}
		}

		public void NotifyObjectSpawned(uint netId, GameObject spawnedObject)
		{
			List<PendingLookup> list = null;
			lock (_lockObject)
			{
				if (_pendingLookups.TryGetValue(netId, out var value))
				{
					list = new List<PendingLookup>(value);
					_pendingLookups.Remove(netId);
				}
			}
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (PendingLookup item in list)
			{
				try
				{
					item.Callback?.Invoke(spawnedObject);
				}
				catch (Exception ex)
				{
					EvilLogger.LogError($"[SpawnWatcher] Callback error for netId {netId} (requester: {item.RequesterName}): {ex.Message}", "NotifyObjectSpawned", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\NetworkObjectSpawnWatcher.cs", 142);
				}
			}
		}

		public int GetPendingLookupCount()
		{
			lock (_lockObject)
			{
				int num = 0;
				foreach (KeyValuePair<uint, List<PendingLookup>> pendingLookup in _pendingLookups)
				{
					num += pendingLookup.Value.Count;
				}
				return num;
			}
		}

		public void ClearAllPendingLookups()
		{
			lock (_lockObject)
			{
				GetPendingLookupCount();
				_pendingLookups.Clear();
				_ = 0;
			}
		}

		private void PollForSpawnedObjects()
		{
			if (!NetworkClient.active)
			{
				return;
			}
			List<uint> list = null;
			lock (_lockObject)
			{
				if (_pendingLookups.Count == 0)
				{
					return;
				}
				foreach (uint key in _pendingLookups.Keys)
				{
					if (TryGetSpawnedObject(key, out var _))
					{
						if (list == null)
						{
							list = new List<uint>();
						}
						list.Add(key);
					}
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (uint item in list)
			{
				if (TryGetSpawnedObject(item, out var networkObject2))
				{
					NotifyObjectSpawned(item, networkObject2);
				}
			}
		}

		private void CleanupExpiredLookups()
		{
			float currentTime = Time.time;
			List<(uint netId, string requesterName)> expiredLookups = null;
			lock (_lockObject)
			{
				List<uint> list = null;
				foreach (KeyValuePair<uint, List<PendingLookup>> kvp in _pendingLookups)
				{
					kvp.Value.RemoveAll(delegate(PendingLookup p)
					{
						if (p.ExpirationTime < currentTime)
						{
							if (expiredLookups == null)
							{
								expiredLookups = new List<(uint, string)>();
							}
							expiredLookups.Add((kvp.Key, p.RequesterName));
							return true;
						}
						return false;
					});
					if (kvp.Value.Count == 0)
					{
						if (list == null)
						{
							list = new List<uint>();
						}
						list.Add(kvp.Key);
					}
				}
				if (list != null)
				{
					foreach (uint item in list)
					{
						_pendingLookups.Remove(item);
					}
				}
			}
			if (expiredLookups == null)
			{
				return;
			}
			foreach (var (num, arg) in expiredLookups)
			{
				EvilLogger.LogError($"<color=red>[SpawnWatcher]</color> Fallback lookup for netId {num} expired (requester: {arg})", "CleanupExpiredLookups", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\NetworkObjectSpawnWatcher.cs", 255);
			}
		}

		private bool TryGetSpawnedObject(uint netId, out GameObject networkObject)
		{
			networkObject = null;
			if (NetworkServer.active && NetworkServer.spawned.TryGetValue(netId, out var value))
			{
				networkObject = value.gameObject;
				return true;
			}
			if (NetworkClient.active && NetworkClient.spawned.TryGetValue(netId, out var value2))
			{
				networkObject = value2.gameObject;
				return true;
			}
			return false;
		}
	}
}
