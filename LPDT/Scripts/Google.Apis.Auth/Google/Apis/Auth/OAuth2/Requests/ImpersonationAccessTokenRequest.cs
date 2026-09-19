using System.Collections.Generic;
using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2.Requests
{
	internal class ImpersonationAccessTokenRequest : ImpersonationRequest
	{
		[JsonProperty("scope")]
		public IEnumerable<string> Scopes { get; set; }

		[JsonProperty("lifetime")]
		public string Lifetime { get; set; }
	}
}
