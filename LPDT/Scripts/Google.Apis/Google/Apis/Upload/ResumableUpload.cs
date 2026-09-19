using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Http;
using Google.Apis.Json;
using Google.Apis.Logging;
using Google.Apis.Requests;
using Google.Apis.Responses;
using Google.Apis.Services;
using Google.Apis.Testing;
using Google.Apis.Util;

namespace Google.Apis.Upload
{
	public abstract class ResumableUpload
	{
		private sealed class InitiatedResumableUpload : ResumableUpload
		{
			private Uri _initiatedUploadUri;

			public InitiatedResumableUpload(Uri uploadUri, Stream contentStream, ResumableUploadOptions options)
				: base(contentStream, options)
			{
				_initiatedUploadUri = uploadUri;
			}

			public override Task<Uri> InitiateSessionAsync(CancellationToken cancellationToken = default(CancellationToken))
			{
				return Task.FromResult(_initiatedUploadUri);
			}
		}

		private class ServerErrorCallback : IHttpUnsuccessfulResponseHandler, IHttpExceptionHandler
		{
			private ResumableUpload Owner { get; set; }

			public ServerErrorCallback(ResumableUpload resumable)
			{
				Owner = resumable;
			}

			public void AddToRequest(HttpRequestMessage request)
			{
				request.Properties["__ExceptionHandlerKey"] = new List<IHttpExceptionHandler> { this };
				request.Properties["__UnsuccessfulResponseHandlerKey"] = new List<IHttpUnsuccessfulResponseHandler> { this };
			}

			public Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
			{
				bool result = false;
				int statusCode = (int)args.Response.StatusCode;
				if (args.SupportsRetry && args.Request.RequestUri.Equals(Owner.UploadUri) && statusCode / 100 == 5)
				{
					result = OnServerError(args.Request);
				}
				TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
				taskCompletionSource.SetResult(result);
				return taskCompletionSource.Task;
			}

			public Task<bool> HandleExceptionAsync(HandleExceptionArgs args)
			{
				bool result = args.SupportsRetry && !args.CancellationToken.IsCancellationRequested && args.Request.RequestUri.Equals(Owner.UploadUri) && OnServerError(args.Request);
				TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
				taskCompletionSource.SetResult(result);
				return taskCompletionSource.Task;
			}

			private bool OnServerError(HttpRequestMessage request)
			{
				string value = string.Format("bytes */{0}", (Owner.StreamLength < 0) ? "*" : Owner.StreamLength.ToString());
				request.Headers.Clear();
				request.Method = HttpMethod.Put;
				request.SetEmptyContent().Headers.Add("Content-Range", value);
				return true;
			}
		}

		private class ResumableUploadProgress : IUploadProgress
		{
			public UploadStatus Status { get; private set; }

			public long BytesSent { get; private set; }

			public Exception Exception { get; private set; }

			public ResumableUploadProgress(UploadStatus status, long bytesSent)
			{
				Status = status;
				BytesSent = bytesSent;
			}

			public ResumableUploadProgress(Exception exception, long bytesSent)
			{
				Status = UploadStatus.Failed;
				BytesSent = bytesSent;
				Exception = exception;
			}
		}

		private class ResumeableUploadSessionData : IUploadSessionData
		{
			public Uri UploadUri { get; private set; }

			public ResumeableUploadSessionData(Uri uploadUri)
			{
				UploadUri = uploadUri;
			}
		}

		private class UploadBuffer
		{
			private class BufferViewStream : Stream
			{
				private readonly UploadBuffer _buffer;

				private readonly int _length;

				private int _position;

				public override bool CanRead => true;

				public override bool CanSeek => true;

				public override bool CanTimeout => false;

				public override bool CanWrite => false;

				public override long Length => _length;

				public override long Position
				{
					get
					{
						return _position;
					}
					set
					{
						if (value < 0 || value > int.MaxValue)
						{
							throw new ArgumentOutOfRangeException();
						}
						_position = (int)value;
					}
				}

				public override int WriteTimeout
				{
					get
					{
						throw new NotSupportedException();
					}
					set
					{
						throw new NotSupportedException();
					}
				}

				internal BufferViewStream(UploadBuffer buffer, int length)
				{
					_buffer = buffer;
					_length = length;
				}

				public override long Seek(long offset, SeekOrigin origin)
				{
					return origin switch
					{
						SeekOrigin.Begin => Position = offset, 
						SeekOrigin.Current => Position += offset, 
						SeekOrigin.End => Position = Length + offset, 
						_ => throw new ArgumentOutOfRangeException("origin"), 
					};
				}

				public override int Read(byte[] buffer, int offset, int count)
				{
					if (_position >= _length)
					{
						return 0;
					}
					int blockOffset;
					byte[] array = _buffer.EnsureBlockAtPosition(_position, out blockOffset);
					int num = Math.Min(Math.Min(count, _length - _position), array.Length - blockOffset);
					System.Buffer.BlockCopy(array, blockOffset, buffer, offset, num);
					_position += num;
					return num;
				}

				public override void SetLength(long value)
				{
					throw new NotSupportedException();
				}

				public override void Flush()
				{
					throw new NotSupportedException();
				}

				public override Task FlushAsync(CancellationToken cancellationToken)
				{
					throw new NotSupportedException();
				}

				public override void Write(byte[] buffer, int offset, int count)
				{
					throw new NotSupportedException();
				}

				public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
				{
					throw new NotSupportedException();
				}
			}

			private const int BlockSize = 65536;

			private readonly ResumableUpload upload;

			private readonly byte[][] blocks;

			public int Capacity { get; }

			public int Length { get; private set; }

			public UploadBuffer(ResumableUpload upload, int capacity)
			{
				this.upload = upload;
				Capacity = capacity;
				int num = (capacity + 65536 - 1) / 65536;
				blocks = new byte[num][];
			}

			public async Task<bool> PopulateFromStreamAsync(Stream stream, CancellationToken cancellationToken)
			{
				int bytesLeft = Capacity - Length;
				while (bytesLeft > 0)
				{
					int blockOffset;
					byte[] array = EnsureBlockAtPosition(Length, out blockOffset);
					int count = Math.Min(array.Length - blockOffset, upload.BufferSize);
					int num = await stream.ReadAsync(array, blockOffset, count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					if (num == 0)
					{
						return true;
					}
					Length += num;
					bytesLeft -= num;
				}
				return false;
			}

			public void Reset()
			{
				Length = 0;
			}

			private byte[] EnsureBlockAtPosition(int bufferOffset, out int blockOffset)
			{
				int num = bufferOffset / 65536;
				blockOffset = bufferOffset % 65536;
				byte[] array = blocks[num];
				if (array == null)
				{
					array = new byte[Math.Min(65536, Capacity - num * 65536)];
					blocks[num] = array;
				}
				return array;
			}

			internal void MoveUnsentDataToStartOfBuffer(long bytesClientSent, long bytesServerReceived)
			{
				int num = (int)(bytesClientSent - bytesServerReceived);
				int num2 = GetActualUploadChunkSize() - num;
				int num3 = Length - num2;
				if (num3 != Length)
				{
					int num4 = Length - num3;
					int num5 = 0;
					int num6 = num3;
					while (num6 > 0)
					{
						int blockOffset;
						byte[] array = EnsureBlockAtPosition(num4, out blockOffset);
						int blockOffset2;
						byte[] array2 = EnsureBlockAtPosition(num5, out blockOffset2);
						int num7 = Math.Min(num6, Math.Min(array.Length - blockOffset, array2.Length - blockOffset2));
						System.Buffer.BlockCopy(array, blockOffset, array2, 0, num7);
						num6 -= num7;
						num4 += num7;
						num5 += num7;
					}
					Length = num3;
				}
			}

			internal HttpContent CreateContent(out int contentLength)
			{
				contentLength = GetActualUploadChunkSize();
				return new StreamContent(new BufferViewStream(this, contentLength), 65536);
			}

			internal void ExecuteInterceptor(StreamInterceptor interceptor, int bytesReceivedFromChunk)
			{
				if (interceptor != null)
				{
					for (int i = 0; i < bytesReceivedFromChunk; i += 65536)
					{
						byte[] buffer = blocks[i / 65536];
						interceptor(buffer, 0, Math.Min(65536, bytesReceivedFromChunk - i));
					}
				}
			}

			private int GetActualUploadChunkSize()
			{
				return Math.Min(upload.ChunkSize, Length);
			}
		}

		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<ResumableUpload>();

		private const int KB = 1024;

		private const int MB = 1048576;

		public const int MinimumChunkSize = 262144;

		public const int DefaultChunkSize = 10485760;

		internal int BufferSize = 4096;

		private const int UnknownSize = -1;

		private const string ZeroByteContentRangeHeader = "bytes */0";

		private static readonly string DefaultGoogleApiClientHeader = new VersionHeaderBuilder().AppendDotNetEnvironment().AppendAssemblyVersion("gdcl", typeof(ResumableUpload)).ToString();

		[VisibleForTestOnly]
		protected int chunkSize = 10485760;

		protected ResumableUploadOptions Options { get; }

		internal ConfigurableHttpClient HttpClient { get; }

		public Stream ContentStream { get; }

		internal long StreamLength { get; set; }

		private UploadBuffer Buffer { get; set; }

		private Uri UploadUri { get; set; }

		private long BytesServerReceived { get; set; }

		private long BytesClientSent { get; set; }

		public int ChunkSize
		{
			get
			{
				return chunkSize;
			}
			set
			{
				if (value < 262144)
				{
					throw new ArgumentOutOfRangeException("ChunkSize");
				}
				chunkSize = value;
			}
		}

		public StreamInterceptor UploadStreamInterceptor { get; set; }

		private ResumableUploadProgress Progress { get; set; }

		public event Action<IUploadProgress> ProgressChanged;

		public event Action<IUploadSessionData> UploadSessionData;

		protected ResumableUpload(Stream contentStream, ResumableUploadOptions options)
		{
			contentStream.ThrowIfNull("contentStream");
			ContentStream = contentStream;
			StreamLength = (ContentStream.CanSeek ? ContentStream.Length : (-1));
			HttpClient = options?.ConfigurableHttpClient ?? new HttpClientFactory().CreateHttpClient(new CreateHttpClientArgs
			{
				ApplicationName = "ResumableUpload",
				GZipEnabled = true,
				GoogleApiClientHeader = DefaultGoogleApiClientHeader
			});
			Options = options;
		}

		public static ResumableUpload CreateFromUploadUri(Uri uploadUri, Stream contentStream, ResumableUploadOptions options = null)
		{
			uploadUri.ThrowIfNull("uploadUri");
			return new InitiatedResumableUpload(uploadUri, contentStream, options);
		}

		private void UpdateProgress(ResumableUploadProgress progress)
		{
			Progress = progress;
			this.ProgressChanged?.Invoke(progress);
		}

		public IUploadProgress GetProgress()
		{
			return Progress;
		}

		private void SendUploadSessionData(ResumeableUploadSessionData sessionData)
		{
			this.UploadSessionData?.Invoke(sessionData);
		}

		public IUploadProgress Upload()
		{
			return UploadAsync(CancellationToken.None).Result;
		}

		public Task<IUploadProgress> UploadAsync()
		{
			return UploadAsync(CancellationToken.None);
		}

		public async Task<IUploadProgress> UploadAsync(CancellationToken cancellationToken)
		{
			BytesServerReceived = 0L;
			UpdateProgress(new ResumableUploadProgress(UploadStatus.Starting, 0L));
			try
			{
				UploadUri = await InitiateSessionAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (ContentStream.CanSeek)
				{
					SendUploadSessionData(new ResumeableUploadSessionData(UploadUri));
				}
				Logger.Debug("MediaUpload[{0}] - Start uploading...", UploadUri);
			}
			catch (Exception exception)
			{
				Logger.Error(exception, "MediaUpload - Exception occurred while initializing the upload");
				UpdateProgress(new ResumableUploadProgress(exception, BytesServerReceived));
				return Progress;
			}
			return await UploadCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public IUploadProgress Resume()
		{
			return ResumeAsync(null, CancellationToken.None).Result;
		}

		public IUploadProgress Resume(Uri uploadUri)
		{
			return ResumeAsync(uploadUri, CancellationToken.None).Result;
		}

		public Task<IUploadProgress> ResumeAsync()
		{
			return ResumeAsync(null, CancellationToken.None);
		}

		public Task<IUploadProgress> ResumeAsync(CancellationToken cancellationToken)
		{
			return ResumeAsync(null, cancellationToken);
		}

		public Task<IUploadProgress> ResumeAsync(Uri uploadUri)
		{
			return ResumeAsync(uploadUri, CancellationToken.None);
		}

		public async Task<IUploadProgress> ResumeAsync(Uri uploadUri, CancellationToken cancellationToken)
		{
			if (uploadUri != null)
			{
				if (!ContentStream.CanSeek)
				{
					throw new NotImplementedException("Resume after program restart not allowed when ContentStream.CanSeek is false");
				}
				Logger.Info("Resuming after program restart: UploadUri={0}", uploadUri);
				UploadUri = uploadUri;
			}
			if (UploadUri == null)
			{
				Logger.Info("There isn't any upload in progress, so starting to upload again");
				return await UploadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			string value = string.Format("bytes */{0}", (StreamLength < 0) ? "*" : StreamLength.ToString());
			HttpRequestMessage httpRequestMessage = new RequestBuilder
			{
				BaseUri = UploadUri,
				Method = "PUT"
			}.CreateRequest();
			httpRequestMessage.SetEmptyContent().Headers.Add("Content-Range", value);
			try
			{
				new ServerErrorCallback(this).AddToRequest(httpRequestMessage);
				if (await HandleResponse(await HttpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false))
				{
					UpdateProgress(new ResumableUploadProgress(UploadStatus.Completed, BytesServerReceived));
					return Progress;
				}
			}
			catch (TaskCanceledException ex)
			{
				Logger.Error(ex, "MediaUpload[{0}] - Task was canceled", UploadUri);
				UpdateProgress(new ResumableUploadProgress(ex, BytesServerReceived));
				throw ex;
			}
			catch (Exception exception)
			{
				Logger.Error(exception, "MediaUpload[{0}] - Exception occurred while resuming uploading media", UploadUri);
				UpdateProgress(new ResumableUploadProgress(exception, BytesServerReceived));
				return Progress;
			}
			return await UploadCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<IUploadProgress> UploadCoreAsync(CancellationToken cancellationToken)
		{
			try
			{
				while (!(await SendNextChunkAsync(ContentStream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
				{
					UpdateProgress(new ResumableUploadProgress(UploadStatus.Uploading, BytesServerReceived));
				}
				UpdateProgress(new ResumableUploadProgress(UploadStatus.Completed, BytesServerReceived));
			}
			catch (TaskCanceledException ex)
			{
				Logger.Error(ex, "MediaUpload[{0}] - Task was canceled", UploadUri);
				UpdateProgress(new ResumableUploadProgress(ex, BytesServerReceived));
				throw ex;
			}
			catch (Exception exception)
			{
				Logger.Error(exception, "MediaUpload[{0}] - Exception occurred while uploading media", UploadUri);
				UpdateProgress(new ResumableUploadProgress(exception, BytesServerReceived));
			}
			return Progress;
		}

		public abstract Task<Uri> InitiateSessionAsync(CancellationToken cancellationToken = default(CancellationToken));

		protected virtual void ProcessResponse(HttpResponseMessage httpResponse)
		{
		}

		protected async Task<bool> SendNextChunkAsync(Stream stream, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			HttpRequestMessage request = new RequestBuilder
			{
				BaseUri = UploadUri,
				Method = "PUT"
			}.CreateRequest();
			new ServerErrorCallback(this).AddToRequest(request);
			await (ContentStream.CanSeek ? PrepareBufferKnownSizeAsync(stream, cancellationToken) : PrepareBufferUnknownSizeAsync(stream, cancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
			int contentLength;
			HttpContent httpContent = Buffer.CreateContent(out contentLength);
			httpContent.Headers.Add("Content-Range", GetContentRangeHeader(BytesServerReceived, contentLength));
			request.Content = httpContent;
			BytesClientSent = BytesServerReceived + contentLength;
			Logger.Debug("MediaUpload[{0}] - Sending bytes={1}-{2}", UploadUri, BytesServerReceived, BytesClientSent - 1);
			long bytesServerReceivedBefore = BytesServerReceived;
			bool num = await HandleResponse(await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
			int bytesReceivedFromChunk = (int)(BytesServerReceived - bytesServerReceivedBefore);
			Buffer.ExecuteInterceptor(UploadStreamInterceptor, bytesReceivedFromChunk);
			if (num)
			{
				Buffer = null;
			}
			return num;
		}

		private async Task<bool> HandleResponse(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				MediaCompleted(response);
				return true;
			}
			if (response.StatusCode == HttpStatusCode.PermanentRedirect)
			{
				string range = response.Headers.FirstOrDefault((KeyValuePair<string, IEnumerable<string>> x) => x.Key.Equals("range", StringComparison.OrdinalIgnoreCase)).Value?.First();
				BytesServerReceived = GetNextByte(range);
				Logger.Debug("MediaUpload[{0}] - {1} Bytes were sent successfully", UploadUri, BytesServerReceived);
				return false;
			}
			throw await ExceptionForResponseAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		}

		protected async Task<GoogleApiException> ExceptionForResponseAsync(HttpResponseMessage response)
		{
			string effectiveServiceName = Options?.ServiceName ?? "";
			RequestError error = await response.DeserializeErrorAsync(effectiveServiceName, Options?.Serializer ?? NewtonsoftJsonSerializer.Instance).ConfigureAwait(continueOnCapturedContext: false);
			return new GoogleApiException(effectiveServiceName)
			{
				Error = error,
				HttpStatusCode = response.StatusCode
			};
		}

		private void MediaCompleted(HttpResponseMessage response)
		{
			Logger.Debug("MediaUpload[{0}] - media was uploaded successfully", UploadUri);
			ProcessResponse(response);
			BytesServerReceived = StreamLength;
		}

		private async Task PrepareBufferUnknownSizeAsync(Stream stream, CancellationToken cancellationToken)
		{
			if (Buffer == null)
			{
				Buffer = new UploadBuffer(this, ChunkSize + 1);
			}
			else
			{
				Buffer.MoveUnsentDataToStartOfBuffer(BytesClientSent, BytesServerReceived);
			}
			if (await Buffer.PopulateFromStreamAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
			{
				StreamLength = BytesServerReceived + Buffer.Length;
			}
		}

		private async Task PrepareBufferKnownSizeAsync(Stream stream, CancellationToken cancellationToken)
		{
			if (Buffer == null)
			{
				Buffer = new UploadBuffer(this, ChunkSize);
			}
			else
			{
				Buffer.Reset();
			}
			if (stream.Position != BytesServerReceived)
			{
				stream.Position = BytesServerReceived;
			}
			await Buffer.PopulateFromStreamAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		private long GetNextByte(string range)
		{
			if (range != null)
			{
				return long.Parse(range.Substring(range.IndexOf('-') + 1)) + 1;
			}
			return 0L;
		}

		private string GetContentRangeHeader(long chunkStart, long chunkSize)
		{
			string arg = ((StreamLength < 0) ? "*" : StreamLength.ToString());
			if (chunkStart == 0L && chunkSize == 0L && StreamLength == 0L)
			{
				return "bytes */0";
			}
			long num = chunkStart + chunkSize - 1;
			return $"bytes {chunkStart}-{num}/{arg}";
		}
	}
	public class ResumableUpload<TRequest> : ResumableUpload
	{
		private const string PayloadContentTypeHeader = "X-Upload-Content-Type";

		private const string PayloadContentLengthHeader = "X-Upload-Content-Length";

		private const string UploadType = "uploadType";

		private const string Resumable = "resumable";

		public IClientService Service { get; private set; }

		public string Path { get; private set; }

		public string HttpMethod { get; private set; }

		public string ContentType { get; private set; }

		public TRequest Body { get; set; }

		protected ResumableUpload(IClientService service, string path, string httpMethod, Stream contentStream, string contentType)
			: base(contentStream, new ResumableUploadOptions
			{
				HttpClient = service.HttpClient,
				Serializer = service.Serializer,
				ServiceName = service.Name
			})
		{
			service.ThrowIfNull("service");
			path.ThrowIfNull("path");
			httpMethod.ThrowIfNullOrEmpty("httpMethod");
			contentStream.ThrowIfNull("contentStream");
			Service = service;
			Path = path;
			HttpMethod = httpMethod;
			ContentType = contentType;
		}

		public override async Task<Uri> InitiateSessionAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			HttpRequestMessage httpRequestMessage = CreateInitializeRequest();
			base.Options?.ModifySessionInitiationRequest?.Invoke(httpRequestMessage);
			HttpResponseMessage httpResponseMessage = await Service.HttpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw await ExceptionForResponseAsync(httpResponseMessage).ConfigureAwait(continueOnCapturedContext: false);
			}
			return httpResponseMessage.Headers.Location;
		}

		private HttpRequestMessage CreateInitializeRequest()
		{
			RequestBuilder requestBuilder = new RequestBuilder
			{
				BaseUri = new Uri(Service.BaseUri),
				Path = Path,
				Method = HttpMethod
			};
			requestBuilder.AddParameter(RequestParameterType.Query, "key", Service.ApiKey);
			requestBuilder.AddParameter(RequestParameterType.Query, "uploadType", "resumable");
			SetAllPropertyValues(requestBuilder);
			HttpRequestMessage httpRequestMessage = requestBuilder.CreateRequest();
			if (ContentType != null)
			{
				httpRequestMessage.Headers.Add("X-Upload-Content-Type", ContentType);
			}
			if (base.ContentStream.CanSeek)
			{
				httpRequestMessage.Headers.Add("X-Upload-Content-Length", base.StreamLength.ToString());
			}
			Service.SetRequestSerailizedContent(httpRequestMessage, Body);
			return httpRequestMessage;
		}

		private void SetAllPropertyValues(RequestBuilder requestBuilder)
		{
			PropertyInfo[] properties = GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				RequestParameterAttribute customAttribute = Utilities.GetCustomAttribute<RequestParameterAttribute>(propertyInfo);
				if (customAttribute == null)
				{
					continue;
				}
				string name = customAttribute.Name ?? propertyInfo.Name.ToLower();
				object value = propertyInfo.GetValue(this, null);
				if (value == null)
				{
					continue;
				}
				IEnumerable enumerable = value as IEnumerable;
				if (!(value is string) && enumerable != null)
				{
					foreach (object item in enumerable)
					{
						requestBuilder.AddParameter(customAttribute.Type, name, Utilities.ConvertToString(item));
					}
				}
				else
				{
					requestBuilder.AddParameter(customAttribute.Type, name, Utilities.ConvertToString(value));
				}
			}
		}
	}
	public class ResumableUpload<TRequest, TResponse> : ResumableUpload<TRequest>
	{
		public TResponse ResponseBody { get; private set; }

		public event Action<TResponse> ResponseReceived;

		protected ResumableUpload(IClientService service, string path, string httpMethod, Stream contentStream, string contentType)
			: base(service, path, httpMethod, contentStream, contentType)
		{
		}

		protected override void ProcessResponse(HttpResponseMessage response)
		{
			base.ProcessResponse(response);
			ResponseBody = base.Service.DeserializeResponse<TResponse>(response).Result;
			this.ResponseReceived?.Invoke(ResponseBody);
		}
	}
}
