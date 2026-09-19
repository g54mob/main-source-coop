using System.Net;
using System.Net.Http;

namespace Google.Apis.Http
{
	public static class HttpExtenstions
	{
		internal static bool IsRedirectStatusCode(this HttpResponseMessage message)
		{
			HttpStatusCode statusCode = message.StatusCode;
			if ((uint)(statusCode - 301) <= 2u || statusCode == HttpStatusCode.TemporaryRedirect)
			{
				return true;
			}
			return false;
		}

		public static HttpContent SetEmptyContent(this HttpRequestMessage request)
		{
			request.Content = new ByteArrayContent(new byte[0]);
			request.Content.Headers.ContentLength = 0L;
			return request.Content;
		}
	}
}
