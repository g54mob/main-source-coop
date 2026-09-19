#define DEBUG
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Fusion.Statistics
{
	public class FusionStatisticsManager
	{
		private readonly FusionMemoryStatisticsSnapshot _memorySnapshot;

		private HashSet<NetworkId> _monitoredIds;

		private FusionStatisticsSnapshot _currentSimulationSnapshot;

		private FusionStatisticsSnapshot _previousSimulationSnapshot;

		private NetworkObjectStatisticsSnapshot _previousObjectSnapshot;

		private NetworkObjectStatisticsSnapshot _currentObjectSnapshot;

		private Simulation _simulation;

		public FusionStatisticsSnapshot SimulationSnapshot => _previousSimulationSnapshot;

		public NetworkObjectStatisticsSnapshot ObjectSnapshot => _previousObjectSnapshot;

		public LagCompensationStatisticsSnapshot LagCompensationSnapshot => _simulation?.Runner.LagCompensation?.LagCompensationSnapshot;

		public FusionMemoryStatisticsSnapshot MemorySnapshot
		{
			get
			{
				CollectAllocatorBucketsMemorySnapshot();
				return _memorySnapshot;
			}
		}

		internal FusionStatisticsManager(Simulation simulation)
		{
			_currentSimulationSnapshot = new FusionStatisticsSnapshot();
			_previousSimulationSnapshot = new FusionStatisticsSnapshot();
			_previousObjectSnapshot = new NetworkObjectStatisticsSnapshot();
			_currentObjectSnapshot = new NetworkObjectStatisticsSnapshot();
			_memorySnapshot = new FusionMemoryStatisticsSnapshot();
			_monitoredIds = new HashSet<NetworkId>();
			_simulation = simulation;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		internal void SwapSnapshots()
		{
			FusionStatisticsSnapshot previousSimulationSnapshot = _previousSimulationSnapshot;
			FusionStatisticsSnapshot currentSimulationSnapshot = _currentSimulationSnapshot;
			_currentSimulationSnapshot = previousSimulationSnapshot;
			_previousSimulationSnapshot = currentSimulationSnapshot;
			_currentSimulationSnapshot.Stats.Clear();
			NetworkObjectStatisticsSnapshot previousObjectSnapshot = _previousObjectSnapshot;
			NetworkObjectStatisticsSnapshot currentObjectSnapshot = _currentObjectSnapshot;
			_currentObjectSnapshot = previousObjectSnapshot;
			_previousObjectSnapshot = currentObjectSnapshot;
			_currentObjectSnapshot.NetworkObjectStatistics.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		internal void AddStat(FusionStatType statType, float value)
		{
			_currentSimulationSnapshot.Stats.TryAdd(statType, 0f);
			_currentSimulationSnapshot.Stats[statType] += value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		internal void SetStat(FusionStatType statType, float value)
		{
			if (!_currentSimulationSnapshot.Stats.TryAdd(statType, value))
			{
				_currentSimulationSnapshot.Stats[statType] = value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		internal void AddObjectStat(NetworkId id, FusionObjectStatType statType, float value)
		{
			if (_monitoredIds.Contains(id))
			{
				if (!_currentObjectSnapshot.NetworkObjectStatistics.TryGetValue(id, out var value2))
				{
					value2 = new Dictionary<FusionObjectStatType, float>();
					_currentObjectSnapshot.NetworkObjectStatistics.Add(id, value2);
				}
				value2.TryAdd(statType, 0f);
				value2[statType] += value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		internal void SetObjectStat(NetworkId id, FusionObjectStatType statType, float value)
		{
			if (_monitoredIds.Contains(id))
			{
				if (!_currentObjectSnapshot.NetworkObjectStatistics.TryGetValue(id, out var value2))
				{
					value2 = new Dictionary<FusionObjectStatType, float>();
					_currentObjectSnapshot.NetworkObjectStatistics.Add(id, value2);
				}
				if (!value2.TryAdd(statType, value))
				{
					value2[statType] = value;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		private void CollectAllocatorBucketsMemorySnapshot()
		{
			_memorySnapshot.CollectData(_simulation);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		public void MonitorNetworkObject(NetworkId id)
		{
			_monitoredIds.Add(id);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		public void StopMonitorNetworkObject(NetworkId id)
		{
			_monitoredIds.Remove(id);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsObjectMonitored(NetworkId id)
		{
			return _monitoredIds.Contains(id);
		}
	}
}
