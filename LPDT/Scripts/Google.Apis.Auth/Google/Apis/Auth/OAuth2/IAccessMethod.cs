using System.Net.Http;

namespace Google.Apis.Auth.OAuth2
{
	public interface IAccessMethod
	{
		void Intercept(HttpRequestMessage request, string accessToken);

		string GetAccessToken(HttpRequestMessage request);
	}
}
