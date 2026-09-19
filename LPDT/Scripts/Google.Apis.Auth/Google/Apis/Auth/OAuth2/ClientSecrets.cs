using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2
{
	public sealed class ClientSecrets
	{
		[JsonProperty("client_id")]
		public string ClientId { get; set; }

		[JsonProperty("client_secret")]
		public string ClientSecret { get; set; }
	}
}
