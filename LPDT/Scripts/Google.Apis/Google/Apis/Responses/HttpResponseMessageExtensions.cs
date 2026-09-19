using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Responses
{
	internal static class HttpResponseMessageExtensions
	{
		internal static async Task<RequestError> DeserializeErrorAsync(this HttpResponseMessage response, string name, ISerializer serializer)
		{
			if (response?.Content == null)
			{
				throw new GoogleApiException(name)
				{
					HttpStatusCode = (response?.StatusCode ?? ((HttpStatusCode)0))
				};
			}
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			RequestError requestError;
			try
			{
				requestError = serializer.Deserialize<StandardResponse<object>>(text)?.Error;
			}
			catch (JsonException inner)
			{
				throw new GoogleApiException(name, null, inner)
				{
					HttpStatusCode = response.StatusCode,
					Error = new RequestError
					{
						ErrorResponseContent = text
					}
				};
			}
			if (requestError == null)
			{
				throw new GoogleApiException(name)
				{
					HttpStatusCode = response.StatusCode,
					Error = new RequestError
					{
						ErrorResponseContent = text
					}
				};
			}
			requestError.ErrorResponseContent = text;
			return requestError;
		}
	}
}
