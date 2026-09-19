using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public sealed class MimicVoiceCaptureService : IMimicVoiceFrameSink, IMimicVoiceSegmentProvider, ISessionCleanup, IInitializable, IDisposable
	{
		private sealed class PlayerCaptureState
		{
			public readonly List<short> Samples = new List<short>();

			public bool IsRecording;

			public int SourceSamplingRate;

			public int SourceChannels;

			public float DurationSeconds;

			public float SilenceSeconds;

			public float PreviousMonoSample;

			public bool HasPreviousMonoSample;

			public double ResamplePhase;

			public void Reset()
			{
				Samples.Clear();
				Samples.TrimExcess();
				IsRecording = false;
				SourceSamplingRate = 0;
				SourceChannels = 0;
				DurationSeconds = 0f;
				SilenceSeconds = 0f;
				PreviousMonoSample = 0f;
				HasPreviousMonoSample = false;
				ResamplePhase = 0.0;
			}
		}

		private readonly MimicVoiceCaptureConfiguration _configuration;

		private readonly IMimicVoiceArchiveConverter _archiveConverter;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly SessionCleanupEvent _sessionCleanupEvent;

		private readonly Dictionary<int, PlayerCaptureState> _captureStates = new Dictionary<int, PlayerCaptureState>();

		private readonly Dictionary<int, Queue<MimicVoiceSegment>> _segmentsByPlayer = new Dictionary<int, Queue<MimicVoiceSegment>>();

		private readonly Dictionary<int, Queue<MimicVoiceSegment>> _pendingArchiveSegmentsByPlayer = new Dictionary<int, Queue<MimicVoiceSegment>>();

		private readonly Dictionary<int, List<MimicVoiceSegment>> _archivedSegmentsByPlayer = new Dictionary<int, List<MimicVoiceSegment>>();

		private readonly Dictionary<int, Queue<int>> _recentlyPlayedSegmentIdsByPlayer = new Dictionary<int, Queue<int>>();

		private readonly Dictionary<int, MimicVoiceDebugInfo> _debugInfos = new Dictionary<int, MimicVoiceDebugInfo>();

		private readonly object _gate = new object();

		private int _nextSegmentId;

		public IReadOnlyDictionary<int, MimicVoiceDebugInfo> DebugInfos => _debugInfos;

		public MimicVoiceCaptureService(MimicVoiceCaptureConfiguration configuration, IMimicVoiceArchiveConverter archiveConverter, NetworkRunnerEventBus eventBus, SessionCleanupEvent sessionCleanupEvent)
		{
			_configuration = configuration;
			_archiveConverter = archiveConverter;
			_eventBus = eventBus;
			_sessionCleanupEvent = sessionCleanupEvent;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Subscribe<OnShutdownEvent>(OnShutdown);
			_sessionCleanupEvent.OnSessionCleanup += Cleanup;
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Unsubscribe<OnShutdownEvent>(OnShutdown);
			_sessionCleanupEvent.OnSessionCleanup -= Cleanup;
		}

		public void CaptureShortFrame(int playerId, short[] samples, int samplingRate, int channels)
		{
			if (playerId <= 0 || samples == null || samples.Length == 0 || samplingRate <= 0 || channels <= 0)
			{
				return;
			}
			float rms = _archiveConverter.CalculateRms(samples, channels);
			float frameSeconds = (float)samples.Length / (float)(samplingRate * channels);
			lock (_gate)
			{
				ProcessCaptureFrame(playerId, rms, frameSeconds, samplingRate, channels, delegate(PlayerCaptureState state, int storageRate)
				{
					_archiveConverter.AppendResampledMonoPcm16(state.Samples, samples, samplingRate, channels, storageRate, ref state.PreviousMonoSample, ref state.HasPreviousMonoSample, ref state.ResamplePhase);
				});
			}
		}

		public void CaptureFloatFrame(int playerId, float[] samples, int samplingRate, int channels)
		{
			if (playerId <= 0 || samples == null || samples.Length == 0 || samplingRate <= 0 || channels <= 0)
			{
				return;
			}
			float rms = _archiveConverter.CalculateRms(samples, channels);
			float frameSeconds = (float)samples.Length / (float)(samplingRate * channels);
			lock (_gate)
			{
				ProcessCaptureFrame(playerId, rms, frameSeconds, samplingRate, channels, delegate(PlayerCaptureState state, int storageRate)
				{
					_archiveConverter.AppendResampledMonoPcm16(state.Samples, samples, samplingRate, channels, storageRate, ref state.PreviousMonoSample, ref state.HasPreviousMonoSample, ref state.ResamplePhase);
				});
			}
		}

		public void EndCapture(int playerId, string reason)
		{
			lock (_gate)
			{
				if (_captureStates.TryGetValue(playerId, out var value) && value.IsRecording)
				{
					CommitCapture(playerId, value, reason);
				}
			}
		}

		public bool TryGetSegmentBySeed(int playerId, int seed, out MimicVoiceSegment segment)
		{
			lock (_gate)
			{
				segment = null;
				DateTime utcNow = DateTime.UtcNow;
				PromotePendingArchiveSegments(playerId, utcNow);
				if (TryGetArchivedSegmentBySeed(playerId, seed, utcNow, out segment))
				{
					return true;
				}
				if (TryGetShortTermSegmentBySeed(playerId, seed, out segment))
				{
					return true;
				}
				GetDebugInfo(playerId).LastPlaybackResult = "miss: no segments";
				return false;
			}
		}

		public void MarkSegmentPlayed(int playerId, int segmentId)
		{
			lock (_gate)
			{
				RememberPlayedSegment(playerId, segmentId);
			}
		}

		public void Cleanup()
		{
			lock (_gate)
			{
				_captureStates.Clear();
				_segmentsByPlayer.Clear();
				_pendingArchiveSegmentsByPlayer.Clear();
				_archivedSegmentsByPlayer.Clear();
				_recentlyPlayedSegmentIdsByPlayer.Clear();
				_debugInfos.Clear();
				_nextSegmentId = 0;
			}
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent _)
		{
			Cleanup();
		}

		private void OnShutdown(OnShutdownEvent _)
		{
			Cleanup();
		}

		private void ProcessCaptureFrame(int playerId, float rms, float frameSeconds, int samplingRate, int channels, Action<PlayerCaptureState, int> appendSamples)
		{
			int storageSamplingRate = _configuration.StorageSamplingRate;
			if (storageSamplingRate <= 0)
			{
				return;
			}
			MimicVoiceDebugInfo debugInfo = GetDebugInfo(playerId);
			debugInfo.LastRms = rms;
			bool flag = rms >= _configuration.VoiceRmsThreshold;
			PlayerCaptureState captureState = GetCaptureState(playerId);
			if (captureState.IsRecording || flag)
			{
				if (!captureState.IsRecording)
				{
					StartCapture(playerId, captureState, samplingRate, channels);
				}
				if (captureState.SourceSamplingRate != samplingRate || captureState.SourceChannels != channels)
				{
					DropCapture(playerId, captureState, "format changed");
					StartCapture(playerId, captureState, samplingRate, channels);
				}
				appendSamples(captureState, storageSamplingRate);
				captureState.DurationSeconds += frameSeconds;
				captureState.SilenceSeconds = (flag ? 0f : (captureState.SilenceSeconds + frameSeconds));
				debugInfo.CurrentDurationSeconds = captureState.DurationSeconds;
				if (captureState.DurationSeconds >= _configuration.MaxSegmentSeconds)
				{
					CommitCapture(playerId, captureState, "max duration");
				}
				else if (captureState.SilenceSeconds >= _configuration.SilenceTimeoutSeconds)
				{
					CommitCapture(playerId, captureState, "silence");
				}
			}
		}

		private bool TryGetShortTermSegmentBySeed(int playerId, int seed, out MimicVoiceSegment segment)
		{
			segment = null;
			if (!_segmentsByPlayer.TryGetValue(playerId, out var value) || value.Count == 0)
			{
				return false;
			}
			List<MimicVoiceSegment> list = null;
			foreach (MimicVoiceSegment item in value)
			{
				if (!IsRecentlyPlayedSegment(playerId, item.SegmentId))
				{
					if (list == null)
					{
						list = new List<MimicVoiceSegment>();
					}
					list.Add(item);
				}
			}
			if (list == null || list.Count == 0)
			{
				return false;
			}
			int index = (int)((uint)seed % (uint)list.Count);
			segment = list[index];
			GetDebugInfo(playerId).LastPlaybackResult = $"hit short-term: segment {segment.SegmentId}";
			return true;
		}

		private bool TryGetArchivedSegmentBySeed(int playerId, int seed, DateTime nowUtc, out MimicVoiceSegment segment)
		{
			segment = null;
			if (!_archivedSegmentsByPlayer.TryGetValue(playerId, out var value) || value.Count == 0)
			{
				return false;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			List<MimicVoiceSegment> list = null;
			for (int i = 0; i < value.Count; i++)
			{
				MimicVoiceSegment mimicVoiceSegment = value[i];
				if (!((nowUtc - mimicVoiceSegment.CreatedAtUtc).TotalSeconds < (double)_configuration.MinArchivedPlaybackAgeSeconds) && (!(_configuration.ArchivedSegmentCooldownSeconds > 0f) || !(realtimeSinceStartup - mimicVoiceSegment.LastPlayedAtRealtime < _configuration.ArchivedSegmentCooldownSeconds)) && !IsRecentlyPlayedSegment(playerId, mimicVoiceSegment.SegmentId))
				{
					if (list == null)
					{
						list = new List<MimicVoiceSegment>();
					}
					list.Add(mimicVoiceSegment);
				}
			}
			if (list == null || list.Count == 0)
			{
				return false;
			}
			int index = (int)((uint)seed % (uint)list.Count);
			segment = list[index];
			segment.LastPlayedAtRealtime = realtimeSinceStartup;
			GetDebugInfo(playerId).LastPlaybackResult = $"hit archive: segment {segment.SegmentId}";
			return true;
		}

		private bool IsRecentlyPlayedSegment(int playerId, int segmentId)
		{
			int recentPlayedHistorySize = GetRecentPlayedHistorySize(playerId);
			if (recentPlayedHistorySize <= 0)
			{
				return false;
			}
			if (!_recentlyPlayedSegmentIdsByPlayer.TryGetValue(playerId, out var value))
			{
				return false;
			}
			TrimRecentPlayedHistory(value, recentPlayedHistorySize);
			foreach (int item in value)
			{
				if (item == segmentId)
				{
					return true;
				}
			}
			return false;
		}

		private void RememberPlayedSegment(int playerId, int segmentId)
		{
			int recentPlayedHistorySize = GetRecentPlayedHistorySize(playerId);
			if (recentPlayedHistorySize > 0)
			{
				if (!_recentlyPlayedSegmentIdsByPlayer.TryGetValue(playerId, out var value))
				{
					value = new Queue<int>();
					_recentlyPlayedSegmentIdsByPlayer[playerId] = value;
				}
				value.Enqueue(segmentId);
				TrimRecentPlayedHistory(value, recentPlayedHistorySize);
			}
		}

		private int GetRecentPlayedHistorySize(int playerId)
		{
			float num = Mathf.Clamp01(_configuration.RecentPlayedSegmentsHistoryPercent);
			if (num <= 0f)
			{
				return 0;
			}
			int availableSegmentCount = GetAvailableSegmentCount(playerId);
			if (availableSegmentCount <= 0)
			{
				return 0;
			}
			return Mathf.Max(1, Mathf.RoundToInt((float)availableSegmentCount * num));
		}

		private int GetAvailableSegmentCount(int playerId)
		{
			HashSet<int> hashSet = null;
			if (_segmentsByPlayer.TryGetValue(playerId, out var value))
			{
				foreach (MimicVoiceSegment item in value)
				{
					if (hashSet == null)
					{
						hashSet = new HashSet<int>();
					}
					hashSet.Add(item.SegmentId);
				}
			}
			if (_archivedSegmentsByPlayer.TryGetValue(playerId, out var value2))
			{
				for (int i = 0; i < value2.Count; i++)
				{
					if (hashSet == null)
					{
						hashSet = new HashSet<int>();
					}
					hashSet.Add(value2[i].SegmentId);
				}
			}
			return hashSet?.Count ?? 0;
		}

		private void TrimRecentPlayedHistory(Queue<int> recentlyPlayedSegmentIds, int historySize)
		{
			while (recentlyPlayedSegmentIds.Count > historySize)
			{
				recentlyPlayedSegmentIds.Dequeue();
			}
		}

		private void PromotePendingArchiveSegments(int playerId, DateTime nowUtc)
		{
			if (_configuration.MaxArchivedSegmentsPerPlayer <= 0 || _configuration.StorageSamplingRate <= 0 || !_pendingArchiveSegmentsByPlayer.TryGetValue(playerId, out var value))
			{
				return;
			}
			while (value.Count > 0)
			{
				MimicVoiceSegment mimicVoiceSegment = value.Peek();
				if ((nowUtc - mimicVoiceSegment.CreatedAtUtc).TotalSeconds < (double)_configuration.ArchiveSegmentDelaySeconds)
				{
					break;
				}
				value.Dequeue();
				ArchiveSegment(mimicVoiceSegment, nowUtc);
			}
			UpdateDebugCounts(playerId);
		}

		private void ArchiveSegment(MimicVoiceSegment segment, DateTime nowUtc)
		{
			if (segment.Samples != null && segment.Samples.Length != 0)
			{
				if (!_archivedSegmentsByPlayer.TryGetValue(segment.PlayerId, out var value))
				{
					value = new List<MimicVoiceSegment>();
					_archivedSegmentsByPlayer[segment.PlayerId] = value;
				}
				segment.ArchivedAtUtc = nowUtc;
				value.Add(segment);
				while (value.Count > _configuration.MaxArchivedSegmentsPerPlayer)
				{
					value.RemoveAt(0);
				}
				UpdateDebugCounts(segment.PlayerId);
			}
		}

		private void UpdateDebugCounts(int playerId)
		{
			MimicVoiceDebugInfo debugInfo = GetDebugInfo(playerId);
			debugInfo.SegmentCount = (_segmentsByPlayer.TryGetValue(playerId, out var value) ? value.Count : 0);
			debugInfo.ArchivedSegmentCount = (_archivedSegmentsByPlayer.TryGetValue(playerId, out var value2) ? value2.Count : 0);
		}

		private void EnqueueCommittedSegment(MimicVoiceSegment segment)
		{
			if (!_segmentsByPlayer.TryGetValue(segment.PlayerId, out var value))
			{
				value = new Queue<MimicVoiceSegment>();
				_segmentsByPlayer[segment.PlayerId] = value;
			}
			value.Enqueue(segment);
			while (value.Count > _configuration.MaxSegmentsPerPlayer)
			{
				value.Dequeue();
			}
			if (_configuration.MaxArchivedSegmentsPerPlayer > 0)
			{
				if (!_pendingArchiveSegmentsByPlayer.TryGetValue(segment.PlayerId, out var value2))
				{
					value2 = new Queue<MimicVoiceSegment>();
					_pendingArchiveSegmentsByPlayer[segment.PlayerId] = value2;
				}
				value2.Enqueue(segment);
			}
		}

		private PlayerCaptureState GetCaptureState(int playerId)
		{
			if (_captureStates.TryGetValue(playerId, out var value))
			{
				return value;
			}
			value = new PlayerCaptureState();
			_captureStates[playerId] = value;
			return value;
		}

		private MimicVoiceDebugInfo GetDebugInfo(int playerId)
		{
			if (_debugInfos.TryGetValue(playerId, out var value))
			{
				return value;
			}
			value = new MimicVoiceDebugInfo
			{
				PlayerId = playerId
			};
			_debugInfos[playerId] = value;
			return value;
		}

		private void StartCapture(int playerId, PlayerCaptureState state, int samplingRate, int channels)
		{
			state.Reset();
			state.IsRecording = true;
			state.SourceSamplingRate = samplingRate;
			state.SourceChannels = channels;
			MimicVoiceDebugInfo debugInfo = GetDebugInfo(playerId);
			debugInfo.IsRecording = true;
			debugInfo.CurrentDurationSeconds = 0f;
		}

		private void CommitCapture(int playerId, PlayerCaptureState state, string reason)
		{
			MimicVoiceDebugInfo debugInfo = GetDebugInfo(playerId);
			if (state.DurationSeconds < _configuration.MinSegmentSeconds || state.Samples.Count == 0)
			{
				DropCapture(playerId, state, "too short after " + reason);
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			int storageSamplingRate = _configuration.StorageSamplingRate;
			short[] array = state.Samples.ToArray();
			float durationSeconds = (float)array.Length / (float)storageSamplingRate;
			MimicVoiceSegment mimicVoiceSegment = new MimicVoiceSegment(playerId, ++_nextSegmentId, storageSamplingRate, array, durationSeconds, utcNow, reason);
			EnqueueCommittedSegment(mimicVoiceSegment);
			PromotePendingArchiveSegments(playerId, utcNow);
			UpdateDebugCounts(playerId);
			debugInfo.IsRecording = false;
			debugInfo.CurrentDurationSeconds = 0f;
			debugInfo.LastCommittedDurationSeconds = mimicVoiceSegment.DurationSeconds;
			debugInfo.LastDropReason = null;
			state.Reset();
		}

		private void DropCapture(int playerId, PlayerCaptureState state, string reason)
		{
			MimicVoiceDebugInfo debugInfo = GetDebugInfo(playerId);
			debugInfo.IsRecording = false;
			debugInfo.CurrentDurationSeconds = 0f;
			debugInfo.LastDropReason = reason;
			state.Reset();
		}
	}
}
