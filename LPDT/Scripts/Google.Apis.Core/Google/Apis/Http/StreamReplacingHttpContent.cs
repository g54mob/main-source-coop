using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Google.Apis.Http
{
	internal sealed class StreamReplacingHttpContent : HttpContent
	{
		private readonly HttpContent _originalContent;

		private readonly Func<Stream, Stream> _replacement;

		private bool _streamCreated;

		internal StreamReplacingHttpContent(HttpContent originalContent, Func<Stream, Stream> replacement)
		{
			_originalContent = originalContent;
			_replacement = replacement;
			foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers)
			{
				base.Headers.Add(header.Key, header.Value);
			}
		}

		protected override bool TryComputeLength(out long length)
		{
			length = 0L;
			return false;
		}

		protected override async Task<Stream> CreateContentReadStreamAsync()
		{
			if (_streamCreated)
			{
				throw new InvalidOperationException("Cannot read content stream more than once");
			}
			_streamCreated = true;
			Stream arg = await _originalContent.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			return _replacement(arg);
		}

		protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			using Stream contentStream = await CreateContentReadStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			await contentStream.CopyToAsync(stream).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
