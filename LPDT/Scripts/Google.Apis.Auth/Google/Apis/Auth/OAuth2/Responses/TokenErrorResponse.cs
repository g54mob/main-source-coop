using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2.Responses
{
	public class TokenErrorResponse
	{
		[JsonProperty("error")]
		public string Error { get; set; }

		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }

		[JsonProperty("error_uri")]
		public string ErrorUri { get; set; }

		public override string ToString()
		{
			return $"Error:\"{Error}\", Description:\"{ErrorDescription}\", Uri:\"{ErrorUri}\"";
		}

		public TokenErrorResponse()
		{
		}

		public TokenErrorResponse(AuthorizationCodeResponseUrl authorizationCode)
		{
			Error = authorizationCode.Error;
			ErrorDescription = authorizationCode.ErrorDescription;
			ErrorUri = authorizationCode.ErrorUri;
		}
	}
}
