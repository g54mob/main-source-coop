using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public class TokenRequest
	{
		[RequestParameter("scope")]
		public string Scope { get; set; }

		[RequestParameter("grant_type")]
		public string GrantType { get; set; }

		[RequestParameter("client_id")]
		public string ClientId { get; set; }

		[RequestParameter("client_secret")]
		public string ClientSecret { get; set; }
	}
}
