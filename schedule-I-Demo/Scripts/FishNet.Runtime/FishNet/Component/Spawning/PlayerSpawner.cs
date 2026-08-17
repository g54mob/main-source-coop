using System;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Serialization;

namespace FishNet.Component.Spawning
{
	[AddComponentMenu("FishNet/Component/PlayerSpawner")]
	public class PlayerSpawner : MonoBehaviour
	{
		[Tooltip("Prefab to spawn for the player.")]
		[SerializeField]
		private NetworkObject _playerPrefab;

		[Tooltip("True to add player to the active scene when no global scenes are specified through the SceneManager.")]
		[SerializeField]
		private bool _addToDefaultScene = true;

		[Tooltip("Areas in which players may spawn.")]
		[FormerlySerializedAs("_spawns")]
		public Transform[] Spawns = new Transform[0];

		private NetworkManager _networkManager;

		private int _nextSpawn;

		public event Action<NetworkObject> OnSpawned;

		private void Start()
		{
			InitializeOnce();
		}

		private void OnDestroy()
		{
			if (_networkManager != null)
			{
				_networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
			}
		}

		private void InitializeOnce()
		{
			_networkManager = InstanceFinder.NetworkManager;
			if (_networkManager == null)
			{
				Debug.LogWarning("PlayerSpawner on " + base.gameObject.name + " cannot work as NetworkManager wasn't found on this object or within parent objects.");
			}
			else
			{
				_networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
			}
		}

		private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
		{
			if (!asServer)
			{
				return;
			}
			if (_playerPrefab == null)
			{
				Debug.LogWarning($"Player prefab is empty and cannot be spawned for connection {conn.ClientId}.");
				return;
			}
			Debug.Log("Spawning player prefab...");
			SetSpawn(_playerPrefab.transform, out var pos, out var rot);
			NetworkObject pooledInstantiated = _networkManager.GetPooledInstantiated(_playerPrefab, pos, rot, asServer: true);
			_networkManager.ServerManager.Spawn(pooledInstantiated, conn);
			if (_addToDefaultScene)
			{
				_networkManager.SceneManager.AddOwnerToDefaultScene(pooledInstantiated);
			}
			this.OnSpawned?.Invoke(pooledInstantiated);
		}

		private void SetSpawn(Transform prefab, out Vector3 pos, out Quaternion rot)
		{
			if (Spawns.Length == 0)
			{
				SetSpawnUsingPrefab(prefab, out pos, out rot);
				return;
			}
			Transform transform = Spawns[_nextSpawn];
			if (transform == null)
			{
				SetSpawnUsingPrefab(prefab, out pos, out rot);
			}
			else
			{
				pos = transform.position;
				rot = transform.rotation;
			}
			_nextSpawn++;
			if (_nextSpawn >= Spawns.Length)
			{
				_nextSpawn = 0;
			}
		}

		private void SetSpawnUsingPrefab(Transform prefab, out Vector3 pos, out Quaternion rot)
		{
			pos = prefab.position;
			rot = prefab.rotation;
		}
	}
}
