using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EvilAnalytics.Shared.Events;

namespace EvilAnalytics.SDK.Network
{
	public class EventBatcher : IDisposable
	{
		public const int DefaultBatchSize = 50;

		public const int DefaultFlushIntervalSeconds = 10;

		private readonly ApiClient _apiClient;

		private readonly string _deviceId;

		private readonly Action<string> _logger;

		private readonly List<GameEvent> _eventQueue;

		private readonly object _queueLock = new object();

		private readonly Timer _flushTimer;

		private int _batchSize;

		private int _flushIntervalMs;

		private bool _isDisposed;

		public int BatchSize
		{
			get
			{
				return _batchSize;
			}
			set
			{
				_batchSize = Math.Max(1, Math.Min(value, 100));
			}
		}

		public int FlushIntervalMs
		{
			get
			{
				return _flushIntervalMs;
			}
			set
			{
				_flushIntervalMs = Math.Max(1000, Math.Min(value, 60000));
				_flushTimer?.Change(_flushIntervalMs, _flushIntervalMs);
			}
		}

		public int QueuedCount
		{
			get
			{
				lock (_queueLock)
				{
					return _eventQueue.Count;
				}
			}
		}

		public Action<List<GameEvent>> OnSendFailed { get; set; }

		public EventBatcher(ApiClient apiClient, string deviceId, Action<string> logger = null, int batchSize = 50, int flushIntervalSeconds = 10)
		{
			_apiClient = apiClient ?? throw new ArgumentNullException("apiClient");
			_deviceId = deviceId ?? throw new ArgumentNullException("deviceId");
			_logger = logger;
			_eventQueue = new List<GameEvent>();
			_batchSize = Math.Max(1, Math.Min(batchSize, 100));
			_flushIntervalMs = Math.Max(1000, Math.Min(flushIntervalSeconds * 1000, 60000));
			_flushTimer = new Timer(OnFlushTimerElapsed, null, _flushIntervalMs, _flushIntervalMs);
		}

		public void QueueEvent(GameEvent evt)
		{
			if (_isDisposed)
			{
				return;
			}
			bool flag = false;
			lock (_queueLock)
			{
				_eventQueue.Add(evt);
				flag = _eventQueue.Count >= _batchSize;
			}
			if (flag)
			{
				Task.Run(() => FlushAsync());
			}
		}

		public async Task FlushAsync()
		{
			if (_isDisposed)
			{
				return;
			}
			List<GameEvent> events;
			lock (_queueLock)
			{
				if (_eventQueue.Count == 0)
				{
					return;
				}
				events = new List<GameEvent>(_eventQueue);
				_eventQueue.Clear();
			}
			await SendBatchAsync(events);
		}

		private async Task SendBatchAsync(List<GameEvent> events)
		{
			if (events.Count == 0)
			{
				return;
			}
			try
			{
				EventBatch batch = new EventBatch
				{
					DeviceId = _deviceId,
					SdkVersion = "1.0.0",
					Events = events,
					SentAt = DateTimeOffset.UtcNow
				};
				Log($"Sending batch of {events.Count} events...");
				EventBatchResponse eventBatchResponse = await _apiClient.SendEventBatchAsync(batch);
				if (eventBatchResponse != null)
				{
					Log($"Batch sent: {eventBatchResponse.Accepted} accepted, {eventBatchResponse.Rejected} rejected");
					return;
				}
				Log("Failed to send batch, server unreachable");
				OnSendFailed?.Invoke(events);
			}
			catch (Exception ex)
			{
				Log("Failed to send batch: " + ex.Message);
				OnSendFailed?.Invoke(events);
			}
		}

		private void OnFlushTimerElapsed(object state)
		{
			if (!_isDisposed)
			{
				Task.Run(() => FlushAsync());
			}
		}

		private void Log(string message)
		{
			_logger?.Invoke("[EventBatcher] " + message);
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			_flushTimer?.Dispose();
			List<GameEvent> list;
			lock (_queueLock)
			{
				list = new List<GameEvent>(_eventQueue);
				_eventQueue.Clear();
			}
			if (list.Count <= 0)
			{
				return;
			}
			try
			{
				SendBatchAsync(list).Wait(TimeSpan.FromSeconds(5.0));
			}
			catch
			{
				OnSendFailed?.Invoke(list);
			}
		}
	}
}
