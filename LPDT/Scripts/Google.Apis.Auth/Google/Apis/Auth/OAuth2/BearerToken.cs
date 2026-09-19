using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Google.Apis.Auth.OAuth2
{
	public class BearerToken
	{
		public class AuthorizationHeaderAccessMethod : IAccessMethod
		{
			private const string Schema = "Bearer";

			public void Intercept(HttpRequestMessage request, string accessToken)
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			}

			public string GetAccessToken(HttpRequestMessage request)
			{
				if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Bearer")
				{
					return request.Headers.Authorization.Parameter;
				}
				return null;
			}
		}

		[Obsolete("https://developers.google.com/identity/protocols/oauth2/index.html#4.-send-the-access-token-to-an-api.")]
		public class QueryParameterAccessMethod : IAccessMethod
		{
			private const string AccessTokenKey = "access_token";

			public void Intercept(HttpRequestMessage request, string accessToken)
			{
				Uri requestUri = request.RequestUri;
				request.RequestUri = new Uri(string.Format("{0}{1}{2}={3}", requestUri.ToString(), string.IsNullOrEmpty(requestUri.Query) ? "?" : "&", "access_token", Uri.EscapeDataString(accessToken)));
			}

			public string GetAccessToken(HttpRequestMessage request)
			{
				string query = request.RequestUri.Query;
				if (string.IsNullOrEmpty(query))
				{
					return null;
				}
				query = query.Substring(1);
				string[] array = query.Split(new char[1] { '&' });
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[1] { '=' });
					if (array2[0].Equals("access_token"))
					{
						return array2[1];
					}
				}
				return null;
			}
		}
	}
}
