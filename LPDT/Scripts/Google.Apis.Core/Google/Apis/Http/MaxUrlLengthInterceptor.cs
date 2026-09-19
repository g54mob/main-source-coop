using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Testing;

namespace Google.Apis.Http
{
	[VisibleForTestOnly]
	public class MaxUrlLengthInterceptor : IHttpExecuteInterceptor
	{
		private readonly uint maxUrlLength;

		public MaxUrlLengthInterceptor(uint maxUrlLength)
		{
			this.maxUrlLength = maxUrlLength;
		}

		public Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (request.Method != HttpMethod.Get || request.RequestUri.AbsoluteUri.Length <= maxUrlLength)
			{
				return Task.FromResult(0);
			}
			request.Method = HttpMethod.Post;
			string query = request.RequestUri.Query;
			if (!string.IsNullOrEmpty(query))
			{
				request.Content = new StringContent(query.Substring(1));
				request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
				string text = request.RequestUri.ToString();
				request.RequestUri = new Uri(text.Remove(text.IndexOf("?")));
			}
			request.Headers.Add("X-HTTP-Method-Override", "GET");
			return Task.FromResult(0);
		}
	}
}
