using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Json;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	internal static class ImpersonationTokenRequestExtensions
	{
		internal static async Task<HttpResponseMessage> ExecuteAsync(this ImpersonationRequest request, HttpClient httpClient, string url, CancellationToken cancellationToken)
		{
			HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new StringContent(NewtonsoftJsonSerializer.Instance.Serialize(request), Encoding.UTF8, "application/json")
			};
			return await httpClient.SendAsync(request2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		internal static async Task<TResponse> ExecuteAsync<TResponse>(this ImpersonationRequest request, HttpClient httpClient, string url, CancellationToken cancellationToken)
		{
			HttpResponseMessage obj = await request.ExecuteAsync(httpClient, url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			obj.EnsureSuccessStatusCode();
			NewtonsoftJsonSerializer instance = NewtonsoftJsonSerializer.Instance;
			return await instance.DeserializeAsync<TResponse>(await obj.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		internal static async Task<TokenResponse> ExecuteAsync(this ImpersonationRequest request, HttpClient httpClient, string url, IClock clock, ILogger logger, CancellationToken cancellationToken)
		{
			return await TokenResponse.FromHttpResponseAsync(await request.ExecuteAsync(httpClient, url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false), clock, logger).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
