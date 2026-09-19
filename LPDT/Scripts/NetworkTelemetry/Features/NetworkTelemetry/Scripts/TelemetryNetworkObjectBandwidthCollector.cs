using System.Collections.Generic;
using Fusion;
using Fusion.Statistics;
using UnityEngine;
using Zenject;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryNetworkObjectBandwidthCollector : SimulationBehaviour, IAfterUpdate, IPublicFacingInterface
	{
		private struct ObjectBandwidthAccumulation
		{
			public string ObjectName;

			public float InBytes;

			public float OutBytes;
		}

		private const float RegistrationIntervalSeconds = 1f;

		private TelemetryService _telemetryService;

		private bool _registeredWithRunner;

		private bool _loggedStatisticsUnavailable;

		private float _registrationTimer;

		private float _submissionTimer;

		private float _statisticsUnavailableTimer;

		private readonly HashSet<NetworkId> _monitoredNetworkIds = new HashSet<NetworkId>();

		private readonly List<NetworkId> _networkIdsToRemoveScratch = new List<NetworkId>();

		private readonly Dictionary<NetworkId, string> _networkObjectNameCache = new Dictionary<NetworkId, string>();

		private readonly Dictionary<NetworkId, ObjectBandwidthAccumulation> _accumulations = new Dictionary<NetworkId, ObjectBandwidthAccumulation>();

		[Inject]
		public void Construct(TelemetryService telemetryService)
		{
			_telemetryService = telemetryService;
			_telemetryService.RegisterNetworkObjectBandwidthCollector(this);
		}

		private void OnDestroy()
		{
			SubmitAccumulatedSamples();
			if (_registeredWithRunner)
			{
				NetworkRunner runner = base.Runner;
				if (runner != null)
				{
					runner.RemoveGlobal(this);
				}
				_registeredWithRunner = false;
			}
			_telemetryService?.UnregisterNetworkObjectBandwidthCollector(this);
		}

		internal void RegisterWithRunner(NetworkRunner runner)
		{
			if (!_registeredWithRunner && !(runner == null) && runner.IsRunning)
			{
				runner.AddGlobal(this);
				_registeredWithRunner = true;
			}
		}

		internal void UnregisterFromRunner(NetworkRunner runner)
		{
			if (_registeredWithRunner)
			{
				if (runner != null && runner.IsRunning)
				{
					runner.RemoveGlobal(this);
				}
				_registeredWithRunner = false;
			}
		}

		public void AfterUpdate()
		{
			TelemetryConfiguration configuration = _telemetryService.Configuration;
			if (_telemetryService == null || !configuration.IsEnabled || !configuration.CollectNetworkObjectBandwidth || !_telemetryService.IsTelemetryActiveForGameSampling || configuration.NetworkObjectBandwidthSubmissionRateHz <= 0f)
			{
				return;
			}
			NetworkRunner runner = base.Runner;
			if (runner == null || !runner.IsRunning)
			{
				return;
			}
			if (!runner.TryGetFusionStatistics(out var statisticsManager))
			{
				_statisticsUnavailableTimer += base.Runner.DeltaTime;
				if (!_loggedStatisticsUnavailable && _statisticsUnavailableTimer >= 10f)
				{
					_loggedStatisticsUnavailable = true;
					Debug.LogWarning("[Telemetry] Fusion statistics are unavailable; per-object bandwidth will not be collected. Use a Fusion DEBUG build (same requirement as the Fusion Statistics / Inspector window).");
				}
				return;
			}
			_statisticsUnavailableTimer = 0f;
			_registrationTimer += base.Runner.DeltaTime;
			if (_registrationTimer >= 1f)
			{
				_registrationTimer = 0f;
				UpdateMonitoredNetworkObjects(runner, statisticsManager);
			}
			AccumulateBytesFromLastFusionUpdateLoop(statisticsManager);
			_submissionTimer += base.Runner.DeltaTime;
			if (_submissionTimer >= 1f / configuration.NetworkObjectBandwidthSubmissionRateHz)
			{
				_submissionTimer = 0f;
				SubmitAccumulatedSamples();
			}
		}

		internal void SubmitAccumulatedSamples()
		{
			if (_accumulations.Count == 0)
			{
				return;
			}
			int sessionTimeMs = _telemetryService.CurrentSessionTimeMs();
			foreach (KeyValuePair<NetworkId, ObjectBandwidthAccumulation> accumulation in _accumulations)
			{
				ObjectBandwidthAccumulation value = accumulation.Value;
				if (!(value.InBytes <= 0f) || !(value.OutBytes <= 0f))
				{
					_telemetryService.SubmitNetworkObjectBandwidthSample(new NetworkObjectBandwidthSample
					{
						SessionTimeMs = sessionTimeMs,
						ObjectName = value.ObjectName,
						InBandwidth = value.InBytes,
						OutBandwidth = value.OutBytes
					});
				}
			}
			_accumulations.Clear();
		}

		private void AccumulateBytesFromLastFusionUpdateLoop(FusionStatisticsManager manager)
		{
			Dictionary<NetworkId, Dictionary<FusionObjectStatType, float>> networkObjectStatistics = manager.ObjectSnapshot.NetworkObjectStatistics;
			foreach (NetworkId monitoredNetworkId in _monitoredNetworkIds)
			{
				if (!networkObjectStatistics.TryGetValue(monitoredNetworkId, out var value))
				{
					continue;
				}
				value.TryGetValue(FusionObjectStatType.InBandwidth, out var value2);
				value.TryGetValue(FusionObjectStatType.OutBandwidth, out var value3);
				if (!(value2 <= 0f) || !(value3 <= 0f))
				{
					string value4;
					string objectName = (_networkObjectNameCache.TryGetValue(monitoredNetworkId, out value4) ? value4 : monitoredNetworkId.ToString());
					if (!_accumulations.TryGetValue(monitoredNetworkId, out var value5))
					{
						value5 = new ObjectBandwidthAccumulation
						{
							ObjectName = objectName
						};
					}
					value5.InBytes += value2;
					value5.OutBytes += value3;
					_accumulations[monitoredNetworkId] = value5;
				}
			}
		}

		private void UpdateMonitoredNetworkObjects(NetworkRunner runner, FusionStatisticsManager manager)
		{
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid)
				{
					NetworkId id = allNetworkObject.Id;
					if (_monitoredNetworkIds.Add(id))
					{
						_networkObjectNameCache[id] = allNetworkObject.gameObject.name;
					}
				}
			}
			_networkIdsToRemoveScratch.Clear();
			foreach (NetworkId monitoredNetworkId in _monitoredNetworkIds)
			{
				if (!runner.Exists(monitoredNetworkId))
				{
					_networkIdsToRemoveScratch.Add(monitoredNetworkId);
				}
			}
			foreach (NetworkId item in _networkIdsToRemoveScratch)
			{
				_monitoredNetworkIds.Remove(item);
				_networkObjectNameCache.Remove(item);
				_accumulations.Remove(item);
			}
		}
	}
}
