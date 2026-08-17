using System.Collections.Generic;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player.Downed.Chicken;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootLifecycleManager : MonoBehaviour
	{
		private struct WildLootEntry
		{
			public NetworkIdentity Identity;

			public HeldItem Held;

			public int Seed;

			public Vector2Int Coord;
		}

		[Header("Reaper")]
		[Tooltip("Uncollected wild loot is destroyed once it is farther than this (m) from EVERY player. Keep it >= the MapMagic loaded-tile reach (mainRange * tileSize, ~2048 with the current config) so loot is only reaped after its POI has unloaded for everyone -> a revisit reloads the tile and respawns the identical item, with no in-view pop and no respawn-band glitch.")]
		[SerializeField]
		private float despawnRadius = 2048f;

		[Tooltip("Seconds between reaper sweeps. Cheap; 4s plateaus the live count right behind a moving player.")]
		[SerializeField]
		private float sweepInterval = 4f;

		[Tooltip("Logs tracked/reaped counts each sweep. Development only.")]
		[SerializeField]
		private bool verboseLogging;

		private const float MinDespawnRadius = 512f;

		private const float MinSweepInterval = 0.5f;

		private readonly Dictionary<uint, WildLootEntry> _wild = new Dictionary<uint, WildLootEntry>();

		private readonly List<Vector3> _playerPositions = new List<Vector3>();

		private readonly List<uint> _toReap = new List<uint>();

		private float _timer;

		public static LootLifecycleManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void ServerTrackWildLoot(GameObject loot, int seed)
		{
			if (NetworkServer.active && !(loot == null) && loot.TryGetComponent<NetworkIdentity>(out var component) && component.netId != 0 && loot.TryGetComponent<HeldItem>(out var component2))
			{
				Vector2Int coord = default(Vector2Int);
				WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
				if (instance != null)
				{
					Vector3 worldPosition = FloatingOriginManager.ToTrueWorld(loot.transform.position);
					coord = instance.GetChunkCoordFromWorldPosition(worldPosition);
				}
				_wild[component.netId] = new WildLootEntry
				{
					Identity = component,
					Held = component2,
					Seed = seed,
					Coord = coord
				};
			}
		}

		public void ServerUntrack(NetworkIdentity identity)
		{
			if (identity != null)
			{
				_wild.Remove(identity.netId);
			}
		}

		public void ServerUntrack(uint netId)
		{
			_wild.Remove(netId);
		}

		private void Update()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			if (_wild.Count == 0)
			{
				_timer = sweepInterval;
				return;
			}
			_timer -= Time.deltaTime;
			if (!(_timer > 0f))
			{
				_timer = sweepInterval;
				ServerSweep();
			}
		}

		private void ServerSweep()
		{
			_playerPositions.Clear();
			foreach (NetworkConnectionToClient value3 in NetworkServer.connections.Values)
			{
				if (value3 != null && value3.isAuthenticated && !(value3.identity == null))
				{
					_playerPositions.Add(value3.identity.transform.position);
				}
			}
			if (_playerPositions.Count == 0)
			{
				return;
			}
			float num = despawnRadius * despawnRadius;
			_toReap.Clear();
			WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
			foreach (KeyValuePair<uint, WildLootEntry> item in _wild)
			{
				WildLootEntry value = item.Value;
				if (value.Identity == null)
				{
					_toReap.Add(item.Key);
					continue;
				}
				HeldItem held = value.Held;
				if ((held != null && (held.IsEquipped || held.AttachedSnappingPlane != null || held.PlacementNetworkData.ParentNetworkID != 0 || held is ChickenForm || held is AttachableObject { IsAttached: not false })) || instance == null || instance.TryGetTerrainByChunkCoord(value.Coord, out var _))
				{
					continue;
				}
				Vector3 position = value.Identity.transform.position;
				float num2 = 3.4028235E+38f;
				for (int i = 0; i < _playerPositions.Count; i++)
				{
					float sqrMagnitude = (_playerPositions[i] - position).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
					}
					if (num2 <= num)
					{
						break;
					}
				}
				if (num2 > num)
				{
					_toReap.Add(item.Key);
				}
			}
			if (_toReap.Count == 0)
			{
				return;
			}
			for (int j = 0; j < _toReap.Count; j++)
			{
				uint key = _toReap[j];
				if (_wild.TryGetValue(key, out var value2))
				{
					_wild.Remove(key);
					if (value2.Seed != 0)
					{
						NetworkSingleton<WorldGenerator>.Instance?.UnregisterLootSpawn(value2.Seed);
					}
					if (value2.Identity != null)
					{
						NetworkServer.Destroy(value2.Identity.gameObject);
					}
				}
			}
			_ = verboseLogging;
		}

		private void OnValidate()
		{
			if (despawnRadius < 512f)
			{
				despawnRadius = 512f;
			}
			if (sweepInterval < 0.5f)
			{
				sweepInterval = 0.5f;
			}
		}
	}
}
