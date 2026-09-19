using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Http
{
	internal sealed class TwoWayDelegatingHandler : DelegatingHandler
	{
		private sealed class AccessibleDelegatingHandler : DelegatingHandler
		{
			internal AccessibleDelegatingHandler(HttpMessageHandler innerHandler)
				: base(innerHandler)
			{
			}

			internal Task<HttpResponseMessage> InternalSendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				return SendAsync(request, cancellationToken);
			}
		}

		private readonly AccessibleDelegatingHandler _alternativeHandler;

		private readonly Func<HttpRequestMessage, bool> _useAlternative;

		private bool disposed;

		internal TwoWayDelegatingHandler(HttpMessageHandler normalHandler, HttpMessageHandler alternativeHandler, Func<HttpRequestMessage, bool> useAlternative)
			: base(normalHandler)
		{
			_alternativeHandler = new AccessibleDelegatingHandler(alternativeHandler);
			_useAlternative = useAlternative;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (!_useAlternative(request))
			{
				return base.SendAsync(request, cancellationToken);
			}
			return _alternativeHandler.InternalSendAsync(request, cancellationToken);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !disposed)
			{
				disposed = true;
				_alternativeHandler.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
