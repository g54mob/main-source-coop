using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Http
{
	internal sealed class StreamInterceptionHandler : DelegatingHandler
	{
		private sealed class InterceptingStream : Stream
		{
			private readonly Stream _original;

			private readonly StreamInterceptor _interceptor;

			public override bool CanRead => true;

			public override bool CanSeek => false;

			public override bool CanWrite => false;

			public override long Length => _original.Length;

			public override long Position
			{
				get
				{
					return _original.Position;
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			internal InterceptingStream(Stream original, StreamInterceptor interceptor)
			{
				_original = original;
				_interceptor = interceptor;
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				int num = _original.Read(buffer, offset, count);
				_interceptor(buffer, offset, num);
				return num;
			}

			public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
			{
				int num = await _original.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_interceptor(buffer, offset, num);
				return num;
			}

			protected override void Dispose(bool disposing)
			{
				_original.Dispose();
			}

			public override void Flush()
			{
				_original.Flush();
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotSupportedException();
			}

			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}
		}

		internal StreamInterceptionHandler(HttpMessageHandler handler)
			: base(handler)
		{
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Task<HttpResponseMessage> task = base.SendAsync(request, cancellationToken);
			Func<HttpResponseMessage, StreamInterceptor> interceptorProvider = GetInterceptorProvider(request);
			if (interceptorProvider != null)
			{
				return ReplaceAsync(task, interceptorProvider);
			}
			return task;
		}

		private async Task<HttpResponseMessage> ReplaceAsync(Task<HttpResponseMessage> responseTask, Func<HttpResponseMessage, StreamInterceptor> interceptorProvider)
		{
			HttpResponseMessage httpResponseMessage = await responseTask.ConfigureAwait(continueOnCapturedContext: false);
			StreamInterceptor interceptor = interceptorProvider(httpResponseMessage);
			if (interceptor != null)
			{
				httpResponseMessage.Content = new StreamReplacingHttpContent(httpResponseMessage.Content, (Stream stream) => new InterceptingStream(stream, interceptor));
			}
			return httpResponseMessage;
		}

		internal static Func<HttpResponseMessage, StreamInterceptor> GetInterceptorProvider(HttpRequestMessage request)
		{
			request.Properties.TryGetValue("__ResponseStreamInterceptorProviderKey", out var value);
			return value as Func<HttpResponseMessage, StreamInterceptor>;
		}
	}
}
