using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2.Requests
{
	internal class ImpersonationOIdCTokenRequest : ImpersonationRequest
	{
		[JsonProperty("audience")]
		public string Audience { get; set; }

		[JsonProperty("includeEmail")]
		public bool IncludeEmail { get; set; }
	}
}
