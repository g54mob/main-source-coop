using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Logging;
using Google.Apis.Requests.Parameters;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public static class TokenRequestExtenstions
	{
		public static Task<TokenResponse> ExecuteAsync(this TokenRequest request, HttpClient httpClient, string tokenServerUrl, CancellationToken taskCancellationToken, IClock clock)
		{
			return request.ExecuteAsync(httpClient, tokenServerUrl, taskCancellationToken, clock, ApplicationContext.Logger);
		}

		internal static async Task<TokenResponse> ExecuteAsync(this TokenRequest request, HttpClient httpClient, string tokenServerUrl, CancellationToken taskCancellationToken, IClock clock, ILogger logger)
		{
			HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Post, tokenServerUrl)
			{
				Content = ParameterUtils.CreateFormUrlEncodedContent(request)
			};
			return await TokenResponse.FromHttpResponseAsync(await httpClient.SendAsync(request2, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false), clock, logger).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
