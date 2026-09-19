using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Http;
using Google.Apis.Logging;
using Google.Apis.Requests;
using Google.Apis.Responses;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Download
{
	public class MediaDownloader : IMediaDownloader
	{
		private class DownloadProgress : IDownloadProgress
		{
			public DownloadStatus Status { get; private set; }

			public long BytesDownloaded { get; private set; }

			public Exception Exception { get; private set; }

			public DownloadProgress(DownloadStatus status, long bytes)
			{
				Status = status;
				BytesDownloaded = bytes;
			}

			public DownloadProgress(Exception exception, long bytes)
			{
				Status = DownloadStatus.Failed;
				BytesDownloaded = bytes;
				Exception = exception;
			}
		}

		private class CountedBuffer
		{
			public byte[] Data { get; set; }

			public int Count { get; private set; }

			public bool IsEmpty => Count == 0;

			public CountedBuffer(int size)
			{
				Data = new byte[size];
				Count = 0;
			}

			public async Task Fill(Stream stream, CancellationToken cancellationToken)
			{
				while (Count < Data.Length)
				{
					int num = await stream.ReadAsync(Data, Count, Data.Length - Count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					if (num != 0)
					{
						Count += num;
						continue;
					}
					break;
				}
			}

			public void RemoveFromFront(int n)
			{
				if (n >= Count)
				{
					Count = 0;
					return;
				}
				Array.Copy(Data, n, Data, 0, Count - n);
				Count -= n;
			}
		}

		private static readonly ILogger Logger;

		private readonly IClientService service;

		private const int MB = 1048576;

		public const int MaximumChunkSize = 10485760;

		private int chunkSize = 10485760;

		public int ChunkSize
		{
			get
			{
				return chunkSize;
			}
			set
			{
				if (value > 10485760)
				{
					throw new ArgumentOutOfRangeException("ChunkSize");
				}
				chunkSize = value;
			}
		}

		public RangeHeaderValue Range { get; set; }

		public Func<HttpResponseMessage, StreamInterceptor> ResponseStreamInterceptorProvider { get; set; }

		public Action<HttpRequestMessage> ModifyRequest { get; set; }

		public event Action<IDownloadProgress> ProgressChanged;

		static MediaDownloader()
		{
			Logger = ApplicationContext.Logger.ForType<MediaDownloader>();
			UriPatcher.PatchUriQuirks();
		}

		private void UpdateProgress(IDownloadProgress progress)
		{
			this.ProgressChanged?.Invoke(progress);
		}

		public MediaDownloader(IClientService service)
		{
			this.service = service;
		}

		public IDownloadProgress Download(string url, Stream stream)
		{
			return DownloadCoreAsync(url, stream, CancellationToken.None).Result;
		}

		public async Task<IDownloadProgress> DownloadAsync(string url, Stream stream)
		{
			return await DownloadAsync(url, stream, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<IDownloadProgress> DownloadAsync(string url, Stream stream, CancellationToken cancellationToken)
		{
			return await DownloadCoreAsync(url, stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<IDownloadProgress> DownloadCoreAsync(string url, Stream stream, CancellationToken cancellationToken)
		{
			url.ThrowIfNull("url");
			stream.ThrowIfNull("stream");
			if (!stream.CanWrite)
			{
				throw new ArgumentException("stream doesn't support write operations");
			}
			UriBuilder uriBuilder = new UriBuilder(url);
			if (uriBuilder.Query == null || uriBuilder.Query.Length <= 1)
			{
				uriBuilder.Query = "alt=media";
			}
			else
			{
				uriBuilder.Query = uriBuilder.Query.Substring(1) + "&alt=media";
			}
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
			httpRequestMessage.Headers.Range = Range;
			if (ResponseStreamInterceptorProvider != null)
			{
				httpRequestMessage.Properties["__ResponseStreamInterceptorProviderKey"] = ResponseStreamInterceptorProvider;
			}
			ModifyRequest?.Invoke(httpRequestMessage);
			long bytesReturned = 0L;
			try
			{
				HttpCompletionOption completionOption = HttpCompletionOption.ResponseHeadersRead;
				using HttpResponseMessage response = await service.HttpClient.SendAsync(httpRequestMessage, completionOption, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!response.IsSuccessStatusCode)
				{
					RequestError error = await response.DeserializeErrorAsync(service.Name, service.Serializer).ConfigureAwait(continueOnCapturedContext: false);
					throw new GoogleApiException(service.Name)
					{
						Error = error,
						HttpStatusCode = response.StatusCode
					};
				}
				OnResponseReceived(response);
				using (Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false))
				{
					CountedBuffer buffer = new CountedBuffer(ChunkSize + 1);
					while (true)
					{
						await buffer.Fill(responseStream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						int bytesToReturn = Math.Min(ChunkSize, buffer.Count);
						OnDataReceived(buffer.Data, bytesToReturn);
						await stream.WriteAsync(buffer.Data, 0, bytesToReturn, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						bytesReturned += bytesToReturn;
						buffer.RemoveFromFront(ChunkSize);
						if (!buffer.IsEmpty)
						{
							UpdateProgress(new DownloadProgress(DownloadStatus.Downloading, bytesReturned));
							continue;
						}
						break;
					}
				}
				OnDownloadCompleted();
				DownloadProgress downloadProgress = new DownloadProgress(DownloadStatus.Completed, bytesReturned);
				UpdateProgress(downloadProgress);
				return downloadProgress;
			}
			catch (TaskCanceledException exception)
			{
				Logger.Error(exception, "Download media was canceled");
				UpdateProgress(new DownloadProgress(exception, bytesReturned));
				throw;
			}
			catch (Exception exception2)
			{
				Logger.Error(exception2, "Exception occurred while downloading media");
				DownloadProgress downloadProgress2 = new DownloadProgress(exception2, bytesReturned);
				UpdateProgress(downloadProgress2);
				return downloadProgress2;
			}
		}

		protected virtual void OnResponseReceived(HttpResponseMessage response)
		{
		}

		protected virtual void OnDataReceived(byte[] data, int length)
		{
		}

		protected virtual void OnDownloadCompleted()
		{
		}
	}
}
