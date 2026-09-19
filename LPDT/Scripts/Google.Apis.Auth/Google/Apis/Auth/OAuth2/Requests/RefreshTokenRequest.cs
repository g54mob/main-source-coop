using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public class RefreshTokenRequest : TokenRequest
	{
		[RequestParameter("refresh_token")]
		public string RefreshToken { get; set; }

		public RefreshTokenRequest()
		{
			base.GrantType = "refresh_token";
		}
	}
}
