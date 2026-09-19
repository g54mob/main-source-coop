using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Http
{
	internal sealed class GzipDeflateHandler : DelegatingHandler
	{
		private const string GzipEncoding = "gzip";

		private const string DeflateEncoding = "deflate";

		private static readonly StringWithQualityHeaderValue s_gzipAcceptHeader = new StringWithQualityHeaderValue("gzip");

		private static readonly StringWithQualityHeaderValue s_deflateAcceptHeader = new StringWithQualityHeaderValue("deflate");

		private static readonly Func<Stream, Stream> s_gzipReplacement = (Stream original) => new GZipStream(original, CompressionMode.Decompress);

		private static readonly Func<Stream, Stream> s_deflateReplacement = (Stream original) => new DeflateStream(original, CompressionMode.Decompress);

		internal GzipDeflateHandler(HttpMessageHandler innerHandler)
			: base(innerHandler)
		{
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			request.Headers.AcceptEncoding.Add(s_gzipAcceptHeader);
			request.Headers.AcceptEncoding.Add(s_deflateAcceptHeader);
			HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HttpContent content = httpResponseMessage.Content;
			string text = content.Headers.ContentEncoding.LastOrDefault();
			Func<Stream, Stream> func = ((text == "gzip") ? s_gzipReplacement : ((text == "deflate") ? s_deflateReplacement : null));
			if (func != null)
			{
				httpResponseMessage.Content = new StreamReplacingHttpContent(content, func);
				HttpContentHeaders headers = httpResponseMessage.Content.Headers;
				headers.ContentLength = null;
				headers.ContentEncoding.Clear();
				string text2 = null;
				foreach (string item in content.Headers.ContentEncoding)
				{
					if (text2 != null)
					{
						headers.ContentEncoding.Add(text2);
					}
					text2 = item;
				}
			}
			return httpResponseMessage;
		}
	}
}
