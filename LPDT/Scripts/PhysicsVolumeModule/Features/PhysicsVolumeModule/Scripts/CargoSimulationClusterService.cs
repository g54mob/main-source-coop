using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class CargoSimulationClusterService : IDisposable
	{
		private static readonly Vector3 _clusterFieldOrigin = new Vector3(-10000f, -10000f, -10000f);

		private const float SLOT_SPACING = 500f;

		private readonly Dictionary<PhysicsInfluenceVolume, CargoSimulationCluster> _clusters = new Dictionary<PhysicsInfluenceVolume, CargoSimulationCluster>();

		private readonly Dictionary<PhysicsInfluenceVolume, int> _slots = new Dictionary<PhysicsInfluenceVolume, int>();

		private readonly HashSet<int> _usedSlots = new HashSet<int>();

		public CargoSimulationCluster Acquire(PhysicsInfluenceVolume volume, Transform carrier, IReadOnlyList<Collider> basketColliders)
		{
			if (volume == null || carrier == null)
			{
				return null;
			}
			if (_clusters.TryGetValue(volume, out var value))
			{
				return value;
			}
			int num = NextFreeSlot();
			_slots[volume] = num;
			_usedSlots.Add(num);
			CargoSimulationCluster cargoSimulationCluster = new CargoSimulationCluster(_clusterFieldOrigin + Vector3.right * (500f * (float)num), carrier, basketColliders);
			_clusters[volume] = cargoSimulationCluster;
			return cargoSimulationCluster;
		}

		public bool TryGet(PhysicsInfluenceVolume volume, out CargoSimulationCluster cluster)
		{
			cluster = null;
			if (volume != null)
			{
				return _clusters.TryGetValue(volume, out cluster);
			}
			return false;
		}

		public void Release(PhysicsInfluenceVolume volume)
		{
			if (!(volume == null))
			{
				if (_clusters.TryGetValue(volume, out var value))
				{
					value.Dispose();
					_clusters.Remove(volume);
				}
				if (_slots.TryGetValue(volume, out var value2))
				{
					_usedSlots.Remove(value2);
					_slots.Remove(volume);
				}
			}
		}

		public void Dispose()
		{
			foreach (KeyValuePair<PhysicsInfluenceVolume, CargoSimulationCluster> cluster in _clusters)
			{
				cluster.Value?.Dispose();
			}
			_clusters.Clear();
			_slots.Clear();
			_usedSlots.Clear();
		}

		private int NextFreeSlot()
		{
			int i;
			for (i = 0; _usedSlots.Contains(i); i++)
			{
			}
			return i;
		}
	}
}
