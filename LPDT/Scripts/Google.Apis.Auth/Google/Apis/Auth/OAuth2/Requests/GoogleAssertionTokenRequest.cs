using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public class GoogleAssertionTokenRequest : TokenRequest
	{
		[RequestParameter("assertion")]
		public string Assertion { get; set; }

		public GoogleAssertionTokenRequest()
		{
			base.GrantType = "urn:ietf:params:oauth:grant-type:jwt-bearer";
		}
	}
}
