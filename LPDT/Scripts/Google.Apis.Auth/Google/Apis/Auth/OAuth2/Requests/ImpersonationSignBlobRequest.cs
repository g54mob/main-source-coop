using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2.Requests
{
	internal class ImpersonationSignBlobRequest : ImpersonationRequest
	{
		[JsonProperty("payload")]
		public byte[] Payload { get; set; }
	}
}
