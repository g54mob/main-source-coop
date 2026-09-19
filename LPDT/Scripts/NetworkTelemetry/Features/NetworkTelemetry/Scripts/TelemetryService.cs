using System;
using System.Collections.Generic;
using Features.DebugModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryService : ITelemetryService, IInitializable, IDisposable, ITickable
	{
		private readonly TelemetryConfiguration _configuration = new TelemetryConfiguration();

		private readonly TelemetryBufferPool _bufferPool;

		private readonly TelemetryBackgroundWorker _backgroundWorker;

		private MultiplayerModel _multiplayerModel;

		private TelemetryNetworkObjectBandwidthCollector _networkObjectBandwidthCollector;

		private TelemetryPacket _activePacket;

		private TelemetrySessionPhase _phase;

		private string _sessionId;

		private string _localPlayerId = string.Empty;

		private string _sessionPlayerId = string.Empty;

		private float _flushTimer;

		private readonly Queue<(int sessionMs, long bytesInTotal, long bytesOutTotal)> _fusionBytesHistory = new Queue<(int, long, long)>();

		private const int TEMPBandwithWindowMs = 60000;

		public TelemetryConfiguration Configuration => _configuration;

		public bool IsTelemetryActiveForGameSampling
		{
			get
			{
				if (_configuration.IsEnabled)
				{
					return _phase != TelemetrySessionPhase.Inactive;
				}
				return false;
			}
		}

		internal bool IsNetworkSessionPhase
		{
			get
			{
				if (_phase != TelemetrySessionPhase.Lobby)
				{
					return _phase == TelemetrySessionPhase.InSession;
				}
				return true;
			}
		}

		public TelemetryService()
		{
			_bufferPool = new TelemetryBufferPool(_configuration.BufferPoolInitialSize);
			_backgroundWorker = new TelemetryBackgroundWorker(_configuration, _bufferPool);
			_activePacket = _bufferPool.Rent();
		}

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			if (_configuration.IsEnabled)
			{
				if (_configuration.CollectLogs)
				{
					Application.logMessageReceived += OnUnityLogReceived;
				}
				_localPlayerId = SystemInfo.deviceUniqueIdentifier;
				_backgroundWorker.Start();
				_phase = TelemetrySessionPhase.PreSession;
				RecordEvent("app_start", TelemetryAppStartSpecs.BuildExtras());
			}
		}

		public void Dispose()
		{
			Application.logMessageReceived -= OnUnityLogReceived;
			Flush();
			_backgroundWorker.Dispose();
		}

		public void Tick()
		{
			if (_configuration.IsEnabled && _phase != TelemetrySessionPhase.Inactive)
			{
				if (IsNetworkSessionPhase)
				{
					RefreshSessionIdFromRunner();
				}
				_flushTimer += Time.deltaTime;
				if (_flushTimer >= _configuration.FlushIntervalSeconds)
				{
					_flushTimer = 0f;
					Flush();
				}
			}
		}

		public void RecordEvent(string name, Dictionary<string, string> extras = null)
		{
			if (_configuration.IsEnabled && _configuration.CollectEvents && _phase != TelemetrySessionPhase.Inactive)
			{
				_activePacket.Events.Add(new TelemetryEvent
				{
					LocalTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
					SessionTimeMs = CurrentSessionTimeMs(),
					Name = name,
					Extras = extras
				});
			}
		}

		public void RecordItemInteractionEvent(string itemType)
		{
			bool flag = !string.IsNullOrEmpty(itemType);
			Dictionary<string, string> extras = new Dictionary<string, string>
			{
				["action"] = (flag ? "pickup" : "drop"),
				["item_type"] = (flag ? itemType : "none")
			};
			RecordEvent("item_interaction", extras);
		}

		internal void EnterPreSessionPhase()
		{
			Flush();
			_phase = TelemetrySessionPhase.PreSession;
			_sessionId = null;
			_flushTimer = 0f;
		}

		internal void EnterLobbyPhase(string sessionId)
		{
			_phase = TelemetrySessionPhase.Lobby;
			_sessionId = sessionId;
		}

		internal void EnterInSessionPhase(string sessionId, string sessionPlayerId)
		{
			_phase = TelemetrySessionPhase.InSession;
			_sessionId = sessionId;
			_sessionPlayerId = sessionPlayerId;
			_flushTimer = 0f;
		}

		internal void EnterInactivePhase()
		{
			Flush();
			_phase = TelemetrySessionPhase.Inactive;
		}

		internal void SubmitPositionSample(PositionSample sample)
		{
			_activePacket.Positions.Add(sample);
		}

		internal void SubmitFrameTimingSample(FrameTimingSample sample)
		{
			_activePacket.FrameTimingSamples.Add(sample);
		}

		internal void SubmitFusionSample(FusionTelemetryStatSample sample)
		{
			_ = _configuration.CollectPhotonTransport;
			_activePacket.FusionTelemetryStats.Add(sample);
		}

		private void LogPhotonBandwidthRollingWindow(FusionTelemetryStatSample sample)
		{
			int num = CurrentSessionTimeMs();
			long photonBytesInTotal = sample.PhotonBytesInTotal;
			long photonBytesOutTotal = sample.PhotonBytesOutTotal;
			_fusionBytesHistory.Enqueue((num, photonBytesInTotal, photonBytesOutTotal));
			while (_fusionBytesHistory.Count > 1 && num - _fusionBytesHistory.Peek().sessionMs > 60000)
			{
				_fusionBytesHistory.Dequeue();
			}
			if (_fusionBytesHistory.Count >= 2)
			{
				(int sessionMs, long bytesInTotal, long bytesOutTotal) tuple = _fusionBytesHistory.Peek();
				int item = tuple.sessionMs;
				long item2 = tuple.bytesInTotal;
				long item3 = tuple.bytesOutTotal;
				int num2 = num - item;
				if (num2 > 0)
				{
					double num3 = (double)(photonBytesInTotal - item2) * 60000.0 / (double)num2 / 1024.0;
					double num4 = (double)(photonBytesOutTotal - item3) * 60000.0 / (double)num2 / 1024.0;
					double num5 = (double)sample.PhotonBytesInDelta / 1024.0;
					double num6 = (double)sample.PhotonBytesOutDelta / 1024.0;
					double num7 = (double)photonBytesInTotal / 1024.0;
					double num8 = (double)photonBytesOutTotal / 1024.0;
					Debug.Log($"[FusionTelemetry] 1s delta: in {num5:F1} KB, out {num6:F1} KB | " + $"rolling ~60s: in {num3:F1} KB/min, out {num4:F1} KB/min | " + $"totals in {num7:F1} KB, out {num8:F1} KB (window {num2} ms, session {num} ms)");
				}
			}
		}

		internal void SubmitEnemyPositionSample(EnemyPositionSample sample)
		{
			_activePacket.EnemyPositions.Add(sample);
		}

		internal void SubmitGrabObjectPositionSample(GrabObjectPositionSample sample)
		{
			_activePacket.GrabObjectPositions.Add(sample);
		}

		internal void RegisterNetworkObjectBandwidthCollector(TelemetryNetworkObjectBandwidthCollector collector)
		{
			_networkObjectBandwidthCollector = collector;
		}

		internal void UnregisterNetworkObjectBandwidthCollector(TelemetryNetworkObjectBandwidthCollector collector)
		{
			if (_networkObjectBandwidthCollector == collector)
			{
				_networkObjectBandwidthCollector = null;
			}
		}

		internal void SubmitNetworkObjectBandwidthSample(NetworkObjectBandwidthSample sample)
		{
			_activePacket.NetworkObjectBandwidthSamples.Add(sample);
		}

		internal int CurrentSessionTimeMs()
		{
			NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return 0;
			}
			double num = 0.0;
			try
			{
				num = networkRunner.TickRate;
				if (num <= 0.0)
				{
					return 0;
				}
			}
			catch
			{
				return 0;
			}
			return (int)((double)networkRunner.Tick.Raw * 1000.0 / num);
		}

		private void RefreshSessionIdFromRunner()
		{
			NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			try
			{
				SessionInfo sessionInfo = networkRunner.SessionInfo;
				if (sessionInfo.IsValid && !string.IsNullOrEmpty(sessionInfo.Name))
				{
					_sessionId = sessionInfo.Name;
				}
			}
			catch (Exception)
			{
			}
		}

		private void OnUnityLogReceived(string message, string stackTrace, LogType type)
		{
			if (_configuration.IsEnabled && _configuration.CollectLogs && type switch
			{
				LogType.Log => _configuration.CaptureLogMessages, 
				LogType.Warning => _configuration.CaptureWarnings, 
				LogType.Error => _configuration.CaptureErrors, 
				LogType.Exception => _configuration.CaptureExceptions, 
				_ => false, 
			})
			{
				_activePacket.Logs.Add(new LogEntry
				{
					LocalTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
					SessionTimeMs = CurrentSessionTimeMs(),
					Level = type.ToString().ToLowerInvariant(),
					Message = BuildLogMessageBody(message, stackTrace, type)
				});
			}
		}

		private string BuildLogMessageBody(string message, string stackTrace, LogType type)
		{
			string text = message ?? string.Empty;
			if ((type == LogType.Error || type == LogType.Exception) && !string.IsNullOrWhiteSpace(stackTrace))
			{
				string text2 = stackTrace.Trim();
				if (text.Length == 0)
				{
					text = text2;
				}
				else if (!text.Contains(text2))
				{
					text = text + "\n" + text2;
				}
			}
			if (text.Length > 8000)
			{
				return text.Substring(0, 8000);
			}
			return text;
		}

		private void Flush()
		{
			_networkObjectBandwidthCollector?.SubmitAccumulatedSamples();
			if (!_activePacket.IsEmpty)
			{
				_activePacket.SessionId = _sessionId;
				_activePacket.PlayerId = _localPlayerId;
				_activePacket.SessionPlayerId = _sessionPlayerId;
				_activePacket.GitCommitShort = GitRevision.CommitHashShort ?? string.Empty;
				_activePacket.AppVersion = Application.version;
				_backgroundWorker.Enqueue(_activePacket);
				_activePacket = _bufferPool.Rent();
			}
		}
	}
}
