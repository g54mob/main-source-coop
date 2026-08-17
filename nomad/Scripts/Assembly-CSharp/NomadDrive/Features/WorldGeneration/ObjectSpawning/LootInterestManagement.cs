using System.Collections.Generic;
using EvilCore.Networking.Parenting;
using Mirror;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootInterestManagement : InterestManagement
	{
		private struct LootEntry
		{
			public bool IsLoot;

			public bool NonCullable;

			public float Range;

			public NetworkedTransform Nt;
		}

		[Header("Loot Interest Management")]
		[Tooltip("Default distance (m) loot stays visible. Overridden per-prefab by LootVisibilityRange.")]
		[SerializeField]
		private float lootVisRange = 80f;

		[Tooltip("Exit range = visRange * this. Greater than 1 prevents spawn/despawn flicker at the boundary.")]
		[SerializeField]
		private float hysteresisMultiplier = 1.2f;

		[Tooltip("Seconds between full observer rebuilds.")]
		[SerializeField]
		private float rebuildInterval = 1f;

		[Tooltip("Max identities whose observers are rebuilt per frame. The interval rebuild is spread across frames so it never spikes a single frame — high average FPS can still judder the camera when one frame stalls (frame-time variance, not an FPS drop). Raise if loot pops in too slowly with many players.")]
		[SerializeField]
		private int rebuildBudgetPerFrame = 40;

		[Tooltip("Logs tracked/loot counts each rebuild. Development only.")]
		[SerializeField]
		private bool verboseLogging;

		private double _lastRebuildTime;

		private readonly List<NetworkIdentity> _rebuildQueue = new List<NetworkIdentity>();

		private int _rebuildCursor;

		private readonly Dictionary<NetworkIdentity, LootEntry> _entries = new Dictionary<NetworkIdentity, LootEntry>();

		private readonly Dictionary<int, Transform> _centerOverrides = new Dictionary<int, Transform>();

		public void SetObserverCenter(NetworkConnectionToClient conn, Transform center)
		{
			if (conn != null && !(center == null))
			{
				_centerOverrides[conn.connectionId] = center;
			}
		}

		public void ClearObserverCenter(NetworkConnectionToClient conn)
		{
			if (conn != null)
			{
				_centerOverrides.Remove(conn.connectionId);
			}
		}

		private Vector3 GetObserverCenter(NetworkConnectionToClient conn)
		{
			if (_centerOverrides.TryGetValue(conn.connectionId, out var value) && value != null)
			{
				return value.position;
			}
			return conn.identity.transform.position;
		}

		public override void OnSpawned(NetworkIdentity identity)
		{
			_entries[identity] = Classify(identity);
		}

		public override void OnDestroyed(NetworkIdentity identity)
		{
			_entries.Remove(identity);
		}

		[ServerCallback]
		public override void ResetState()
		{
			if (NetworkServer.active)
			{
				_lastRebuildTime = 0.0;
				_entries.Clear();
				_centerOverrides.Clear();
				_rebuildQueue.Clear();
				_rebuildCursor = 0;
			}
		}

		private LootEntry Classify(NetworkIdentity identity)
		{
			LootItemDefinition component;
			bool isLoot = identity.TryGetComponent<LootItemDefinition>(out component);
			NonCullableNetworkObject component2;
			bool nonCullable = identity.TryGetComponent<NonCullableNetworkObject>(out component2);
			LootVisibilityRange component3;
			float range = (identity.TryGetComponent<LootVisibilityRange>(out component3) ? component3.VisRange : (-1f));
			NetworkedTransform component4;
			NetworkedTransform nt = (identity.TryGetComponent<NetworkedTransform>(out component4) ? component4 : null);
			return new LootEntry
			{
				IsLoot = isLoot,
				NonCullable = nonCullable,
				Range = range,
				Nt = nt
			};
		}

		private bool TryGetCullableRanges(NetworkIdentity identity, out float enterSqr, out float exitSqr)
		{
			if (!_entries.TryGetValue(identity, out var value))
			{
				value = Classify(identity);
				_entries[identity] = value;
			}
			float num = ((value.Range > 0f) ? value.Range : lootVisRange);
			enterSqr = num * num;
			float num2 = num * hysteresisMultiplier;
			exitSqr = num2 * num2;
			if (value.IsLoot && !value.NonCullable)
			{
				return !IsRidingNonCullable(in value);
			}
			return false;
		}

		private bool IsRidingNonCullable(in LootEntry entry)
		{
			NetworkedTransform nt = entry.Nt;
			if (nt == null)
			{
				return false;
			}
			uint parentNetworkedTransformNetId = nt.SyncedParentData.ParentNetworkedTransformNetId;
			if (parentNetworkedTransformNetId == 0)
			{
				return false;
			}
			NonCullableNetworkObject component;
			if (NetworkServer.spawned.TryGetValue(parentNetworkedTransformNetId, out var value) && value != null)
			{
				return value.TryGetComponent<NonCullableNetworkObject>(out component);
			}
			return false;
		}

		public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnectionToClient newObserver)
		{
			if (!TryGetCullableRanges(identity, out var enterSqr, out var _))
			{
				return true;
			}
			if (newObserver?.identity == null)
			{
				return false;
			}
			return (identity.transform.position - GetObserverCenter(newObserver)).sqrMagnitude < enterSqr;
		}

		public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnectionToClient> newObservers)
		{
			float enterSqr;
			float exitSqr;
			bool flag = TryGetCullableRanges(identity, out enterSqr, out exitSqr);
			Vector3 position = identity.transform.position;
			foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
			{
				if (value == null || !value.isAuthenticated || value.identity == null)
				{
					continue;
				}
				if (!flag)
				{
					newObservers.Add(value);
					continue;
				}
				float sqrMagnitude = (GetObserverCenter(value) - position).sqrMagnitude;
				if (identity.observers.ContainsKey(value.connectionId) ? (sqrMagnitude < exitSqr) : (sqrMagnitude < enterSqr))
				{
					newObservers.Add(value);
				}
			}
		}

		public override void SetHostVisibility(NetworkIdentity identity, bool visible)
		{
		}

		[ServerCallback]
		private void LateUpdate()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			if (_rebuildCursor >= _rebuildQueue.Count)
			{
				if (NetworkTime.localTime < _lastRebuildTime + (double)rebuildInterval)
				{
					return;
				}
				_lastRebuildTime = NetworkTime.localTime;
				RefillRebuildQueue();
				if (verboseLogging)
				{
					LogRebuildStats();
				}
			}
			int num = Mathf.Max(1, rebuildBudgetPerFrame);
			int num2 = 0;
			while (_rebuildCursor < _rebuildQueue.Count && num2 < num)
			{
				NetworkIdentity networkIdentity = _rebuildQueue[_rebuildCursor++];
				if (networkIdentity != null)
				{
					NetworkServer.RebuildObservers(networkIdentity, initialize: false);
				}
				num2++;
			}
		}

		private void RefillRebuildQueue()
		{
			_rebuildQueue.Clear();
			_rebuildCursor = 0;
			foreach (NetworkIdentity value in NetworkServer.spawned.Values)
			{
				if (value != null)
				{
					_rebuildQueue.Add(value);
				}
			}
		}

		private void LogRebuildStats()
		{
			int num = 0;
			foreach (LootEntry value in _entries.Values)
			{
				if (value.IsLoot)
				{
					num++;
				}
			}
		}

		private void OnValidate()
		{
			if (lootVisRange < 1f)
			{
				lootVisRange = 1f;
			}
			if (hysteresisMultiplier < 1f)
			{
				hysteresisMultiplier = 1f;
			}
			if (rebuildInterval < 0.1f)
			{
				rebuildInterval = 0.1f;
			}
			if (rebuildBudgetPerFrame < 1)
			{
				rebuildBudgetPerFrame = 1;
			}
		}
	}
}
