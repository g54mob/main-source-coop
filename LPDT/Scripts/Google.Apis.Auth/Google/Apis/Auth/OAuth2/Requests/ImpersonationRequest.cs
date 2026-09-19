using System.Collections.Generic;
using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2.Requests
{
	internal abstract class ImpersonationRequest
	{
		[JsonProperty("delegates")]
		public IEnumerable<string> DelegateAccounts { get; set; }
	}
}
