using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using UnityEngine;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryBackgroundWorker : IDisposable
	{
		private readonly TelemetryConfiguration _configuration;

		private readonly TelemetryBufferPool _bufferPool;

		private readonly HttpClient _httpClient;

		private readonly ConcurrentQueue<TelemetryPacket> _sendQueue = new ConcurrentQueue<TelemetryPacket>();

		private readonly SemaphoreSlim _sendSignal = new SemaphoreSlim(0);

		private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

		private readonly ArrayBufferWriter<byte> _messagePackWriter = new ArrayBufferWriter<byte>(65536);

		private Thread _thread;

		private bool _disposed;

		private static readonly MessagePackSerializerOptions SerializerOptions = MessagePackSerializerOptions.Standard;

		private static readonly MediaTypeHeaderValue MsgPackContentType = new MediaTypeHeaderValue("application/x-msgpack");

		public TelemetryBackgroundWorker(TelemetryConfiguration configuration, TelemetryBufferPool bufferPool)
		{
			_configuration = configuration;
			_bufferPool = bufferPool;
			_httpClient = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(configuration.HttpTimeoutSeconds)
			};
		}

		public void Start()
		{
			if (_thread != null)
			{
				Debug.LogError("[TelemetryWorker] Already started. Second Start request is not expected. Ignored.");
				return;
			}
			_thread = new Thread(WorkerLoop)
			{
				IsBackground = true,
				Name = "TelemetryWorker"
			};
			_thread.Start();
		}

		public void Enqueue(TelemetryPacket packet)
		{
			_sendQueue.Enqueue(packet);
			_sendSignal.Release();
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				_cancellationSource.Cancel();
				_sendSignal.Release();
				_thread?.Join(2000);
				_httpClient.Dispose();
				_cancellationSource.Dispose();
				_sendSignal.Dispose();
			}
		}

		private void WorkerLoop()
		{
			while (!_cancellationSource.IsCancellationRequested)
			{
				try
				{
					_sendSignal.Wait(_cancellationSource.Token);
					if (_sendQueue.TryDequeue(out var result))
					{
						try
						{
							ProcessPacket(result);
						}
						finally
						{
							_bufferPool.Return(result);
						}
					}
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (Exception arg)
				{
					Debug.LogError($"[TelemetryWorker] {arg}");
				}
			}
		}

		private void ProcessPacket(TelemetryPacket packet)
		{
			_messagePackWriter.Clear();
			MessagePackSerializer.Serialize(_messagePackWriter, packet, SerializerOptions);
			PostMessagePackPayload(_messagePackWriter.WrittenMemory);
		}

		private void PostMessagePackPayload(ReadOnlyMemory<byte> payload)
		{
			try
			{
				MemoryMarshal.TryGetArray(payload, out var segment);
				using ByteArrayContent byteArrayContent = new ByteArrayContent(segment.Array, segment.Offset, segment.Count);
				byteArrayContent.Headers.ContentType = MsgPackContentType;
				using HttpResponseMessage httpResponseMessage = _httpClient.PostAsync(_configuration.ApiEndpoint, byteArrayContent).GetAwaiter().GetResult();
				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					Debug.LogWarning($"[TelemetryWorker] API returned HTTP {(int)httpResponseMessage.StatusCode}. Packet dropped.");
				}
			}
			catch (TaskCanceledException)
			{
				Debug.LogWarning("[TelemetryWorker] Request timed out. Packet dropped.");
			}
			catch (HttpRequestException ex2)
			{
				Debug.LogWarning("[TelemetryWorker] Request failed: " + ex2.Message + ". Packet dropped.");
			}
		}
	}
}
