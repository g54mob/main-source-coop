using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Requests
{
	public sealed class PageStreamer<TResource, TRequest, TResponse, TToken> where TRequest : IClientServiceRequest<TResponse> where TToken : class
	{
		private static readonly TResource[] emptyResources = new TResource[0];

		private readonly Action<TRequest, TToken> requestModifier;

		private readonly Func<TResponse, TToken> tokenExtractor;

		private readonly Func<TResponse, IEnumerable<TResource>> resourceExtractor;

		public PageStreamer(Action<TRequest, TToken> requestModifier, Func<TResponse, TToken> tokenExtractor, Func<TResponse, IEnumerable<TResource>> resourceExtractor)
		{
			if (requestModifier == null)
			{
				throw new ArgumentNullException("requestProvider");
			}
			if (tokenExtractor == null)
			{
				throw new ArgumentNullException("tokenExtractor");
			}
			if (resourceExtractor == null)
			{
				throw new ArgumentNullException("resourceExtractor");
			}
			this.requestModifier = requestModifier;
			this.tokenExtractor = tokenExtractor;
			this.resourceExtractor = resourceExtractor;
		}

		public IEnumerable<TResource> Fetch(TRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			TToken token;
			do
			{
				TResponse arg = request.Execute();
				token = tokenExtractor(arg);
				requestModifier(request, token);
				foreach (TResource item in resourceExtractor(arg) ?? emptyResources)
				{
					yield return item;
				}
			}
			while (token != null);
		}

		public async Task<IList<TResource>> FetchAllAsync(TRequest request, CancellationToken cancellationToken)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			List<TResource> results = new List<TResource>();
			TToken val;
			do
			{
				cancellationToken.ThrowIfCancellationRequested();
				TResponse arg = await request.ExecuteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				val = tokenExtractor(arg);
				requestModifier(request, val);
				results.AddRange(resourceExtractor(arg) ?? emptyResources);
			}
			while (val != null);
			return results;
		}
	}
}
