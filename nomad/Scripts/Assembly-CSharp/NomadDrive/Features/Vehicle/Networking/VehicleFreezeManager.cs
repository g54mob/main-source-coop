using System.Collections.Generic;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.WorldGeneration;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Networking
{
	public class VehicleFreezeManager : MonoBehaviour
	{
		[Header("Parking Freeze")]
		[Tooltip("A parked vehicle is frozen kinematic once it is farther than this (m) from EVERY player. Keep it comfortably BELOW the MapMagic load reach (tileSize * mainRange, ~2048 m with the current config) so the vehicle is always frozen before its tile can unload -> it can never fall. The default 1024 m leaves a wide margin.")]
		[SerializeField]
		private float freezeDistance = 1024f;

		[Tooltip("A frozen vehicle thaws back to normal physics once a player is within this (m). The gap to freezeDistance is a hysteresis band so a vehicle parked at the boundary doesn't flap each sweep.")]
		[SerializeField]
		private float unfreezeDistance = 896f;

		[Tooltip("Seconds between freeze sweeps. Cheap; vehicles change parking state slowly.")]
		[SerializeField]
		private float sweepInterval = 2f;

		[Tooltip("Logs froze/thawed counts each sweep. Development only.")]
		[SerializeField]
		private bool verboseLogging;

		private const float MinFreezeDistance = 256f;

		private const float MaxFreezeDistance = 1600f;

		private const float MinSweepInterval = 0.5f;

		private const float MinHysteresisBand = 32f;

		private readonly Dictionary<uint, NetworkedNWHVehicle> _vehicles = new Dictionary<uint, NetworkedNWHVehicle>();

		private readonly List<Vector3> _playerPositions = new List<Vector3>();

		private readonly List<uint> _toRemove = new List<uint>();

		private float _timer;

		public static VehicleFreezeManager Instance { get; private set; }

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

		public static void EnsureExists()
		{
			if (!(Instance != null))
			{
				new GameObject("[VehicleFreezeManager]").AddComponent<VehicleFreezeManager>();
			}
		}

		public void ServerRegister(NetworkedNWHVehicle vehicle)
		{
			if (NetworkServer.active && !(vehicle == null))
			{
				uint netId = vehicle.netId;
				if (netId != 0)
				{
					_vehicles[netId] = vehicle;
				}
			}
		}

		public void ServerUnregister(uint netId)
		{
			_vehicles.Remove(netId);
		}

		private void Update()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			if (_vehicles.Count == 0)
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
			GatherPlayerPositions();
			if (_playerPositions.Count == 0)
			{
				return;
			}
			float freezeSqr = freezeDistance * freezeDistance;
			float unfreezeSqr = unfreezeDistance * unfreezeDistance;
			_toRemove.Clear();
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<uint, NetworkedNWHVehicle> vehicle in _vehicles)
			{
				NetworkedNWHVehicle value = vehicle.Value;
				if (value == null)
				{
					_toRemove.Add(vehicle.Key);
					continue;
				}
				int num3 = EvaluateVehicle(value, freezeSqr, unfreezeSqr);
				if (num3 > 0)
				{
					num++;
				}
				else if (num3 < 0)
				{
					num2++;
				}
			}
			for (int i = 0; i < _toRemove.Count; i++)
			{
				_vehicles.Remove(_toRemove[i]);
			}
			if (verboseLogging && num <= 0)
			{
				_ = 0;
			}
		}

		public void ServerEvaluateNow(NetworkedNWHVehicle vehicle)
		{
			if (NetworkServer.active && !(vehicle == null))
			{
				GatherPlayerPositions();
				if (_playerPositions.Count != 0)
				{
					EvaluateVehicle(vehicle, freezeDistance * freezeDistance, unfreezeDistance * unfreezeDistance);
				}
			}
		}

		private void GatherPlayerPositions()
		{
			_playerPositions.Clear();
			foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
			{
				if (value != null && value.isAuthenticated && !(value.identity == null))
				{
					_playerPositions.Add(value.identity.transform.position);
				}
			}
		}

		private int EvaluateVehicle(NetworkedNWHVehicle v, float freezeSqr, float unfreezeSqr)
		{
			if (v.HasDriver)
			{
				if (v.IsParkedFrozen)
				{
					v.ServerSetParkedFrozen(frozen: false);
					return -1;
				}
				return 0;
			}
			Vector3 position = v.transform.position;
			float num = 3.4028235E+38f;
			for (int i = 0; i < _playerPositions.Count; i++)
			{
				float sqrMagnitude = (_playerPositions[i] - position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
				}
			}
			bool flag = num <= unfreezeSqr;
			bool flag2 = num >= freezeSqr;
			bool flag3 = HostHasTerrain(position);
			if (v.IsParkedFrozen)
			{
				if (flag && flag3)
				{
					v.ServerSetParkedFrozen(frozen: false);
					return -1;
				}
			}
			else if (flag2 || !flag3)
			{
				v.ServerSetParkedFrozen(frozen: true);
				return 1;
			}
			return 0;
		}

		private bool HostHasTerrain(Vector3 pos)
		{
			WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
			if (instance == null || !instance.IsReady)
			{
				return true;
			}
			Vector3 worldPosition = FloatingOriginManager.ToTrueWorld(pos);
			Terrain terrain;
			return instance.TryGetTerrainByChunkCoord(instance.GetChunkCoordFromWorldPosition(worldPosition), out terrain);
		}

		private void OnValidate()
		{
			freezeDistance = Mathf.Clamp(freezeDistance, 256f, 1600f);
			unfreezeDistance = Mathf.Clamp(unfreezeDistance, 128f, freezeDistance - 32f);
			if (sweepInterval < 0.5f)
			{
				sweepInterval = 0.5f;
			}
		}
	}
}
