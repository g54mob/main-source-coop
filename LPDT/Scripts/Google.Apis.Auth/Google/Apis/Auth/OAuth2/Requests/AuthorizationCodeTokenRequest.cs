using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public class AuthorizationCodeTokenRequest : TokenRequest
	{
		[RequestParameter("code")]
		public string Code { get; set; }

		[RequestParameter("redirect_uri")]
		public string RedirectUri { get; set; }

		public AuthorizationCodeTokenRequest()
		{
			base.GrantType = "authorization_code";
		}
	}
}
