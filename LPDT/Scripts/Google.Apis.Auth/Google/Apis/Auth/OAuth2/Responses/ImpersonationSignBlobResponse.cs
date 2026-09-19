using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2.Responses
{
	internal class ImpersonationSignBlobResponse
	{
		[JsonProperty("signedBlob")]
		public string SignedBlob { get; set; }
	}
}
