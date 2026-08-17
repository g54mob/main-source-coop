using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EvilAnalytics.Shared.Events;
using Newtonsoft.Json;

namespace EvilAnalytics.SDK.Network
{
	public class OfflineQueue : IDisposable
	{
		public const int DefaultMaxQueueSize = 1000;

		public const string DefaultFileName = "evil_analytics_offline_queue.json";

		private readonly string _filePath;

		private readonly int _maxQueueSize;

		private readonly Action<string> _logger;

		private readonly object _fileLock = new object();

		private readonly List<GameEvent> _queue;

		private bool _isDisposed;

		public int Count
		{
			get
			{
				lock (_fileLock)
				{
					return _queue.Count;
				}
			}
		}

		public bool HasEvents => Count > 0;

		public int MaxQueueSize => _maxQueueSize;

		public OfflineQueue(string persistPath, Action<string> logger = null, int maxQueueSize = 1000)
		{
			if (string.IsNullOrEmpty(persistPath))
			{
				throw new ArgumentNullException("persistPath");
			}
			_filePath = Path.Combine(persistPath, "evil_analytics_offline_queue.json");
			_logger = logger;
			_maxQueueSize = Math.Max(100, Math.Min(maxQueueSize, 10000));
			_queue = new List<GameEvent>();
			LoadQueue();
		}

		public void Enqueue(IEnumerable<GameEvent> events)
		{
			if (_isDisposed)
			{
				return;
			}
			lock (_fileLock)
			{
				foreach (GameEvent @event in events)
				{
					_queue.Add(@event);
				}
				while (_queue.Count > _maxQueueSize)
				{
					_queue.RemoveAt(0);
					Log("Dropped oldest event due to queue size limit");
				}
				SaveQueue();
			}
			Log($"Queued {_queue.Count} events for offline storage");
		}

		public void Enqueue(GameEvent evt)
		{
			Enqueue(new GameEvent[1] { evt });
		}

		public List<GameEvent> DequeueAll()
		{
			lock (_fileLock)
			{
				List<GameEvent> result = new List<GameEvent>(_queue);
				_queue.Clear();
				SaveQueue();
				return result;
			}
		}

		public List<GameEvent> Peek(int batchSize)
		{
			lock (_fileLock)
			{
				int count = Math.Min(batchSize, _queue.Count);
				return _queue.GetRange(0, count);
			}
		}

		public void Remove(int count)
		{
			lock (_fileLock)
			{
				int count2 = Math.Min(count, _queue.Count);
				_queue.RemoveRange(0, count2);
				SaveQueue();
			}
		}

		public async Task<int> TryFlushAsync(Func<List<GameEvent>, Task<bool>> sender, int batchSize = 50)
		{
			if (_isDisposed)
			{
				return 0;
			}
			if (!HasEvents)
			{
				return 0;
			}
			int totalSent = 0;
			while (HasEvents)
			{
				List<GameEvent> batch = Peek(batchSize);
				if (batch.Count == 0)
				{
					break;
				}
				try
				{
					if (await sender(batch))
					{
						Remove(batch.Count);
						totalSent += batch.Count;
						Log($"Sent {batch.Count} offline events");
						continue;
					}
				}
				catch (Exception ex)
				{
					Log("Failed to send offline events: " + ex.Message);
				}
				break;
			}
			return totalSent;
		}

		private void LoadQueue()
		{
			try
			{
				if (File.Exists(_filePath))
				{
					List<GameEvent> list = JsonConvert.DeserializeObject<List<GameEvent>>(File.ReadAllText(_filePath));
					if (list != null)
					{
						_queue.AddRange(list);
						Log($"Loaded {_queue.Count} events from offline queue");
					}
				}
			}
			catch (Exception ex)
			{
				Log("Failed to load offline queue: " + ex.Message);
			}
		}

		private void SaveQueue()
		{
			try
			{
				string directoryName = Path.GetDirectoryName(_filePath);
				if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				string contents = JsonConvert.SerializeObject(_queue, Formatting.None);
				File.WriteAllText(_filePath, contents);
			}
			catch (Exception ex)
			{
				Log("Failed to save offline queue: " + ex.Message);
			}
		}

		private void Log(string message)
		{
			_logger?.Invoke("[OfflineQueue] " + message);
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			lock (_fileLock)
			{
				if (_queue.Count > 0)
				{
					SaveQueue();
				}
			}
		}
	}
}
