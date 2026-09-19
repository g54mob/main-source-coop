using Google.Apis.Http;

namespace Google.Apis.Auth.OAuth2
{
	public interface ICredential : IConfigurableHttpClientInitializer, ITokenAccess
	{
	}
}
