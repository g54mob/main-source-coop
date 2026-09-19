using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2
{
	public class JsonCredentialParameters
	{
		public const string AuthorizedUserCredentialType = "authorized_user";

		public const string ServiceAccountCredentialType = "service_account";

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("project_id")]
		public string ProjectId { get; set; }

		[JsonProperty("quota_project_id")]
		public string QuotaProject { get; set; }

		[JsonProperty("client_id")]
		public string ClientId { get; set; }

		[JsonProperty("client_secret")]
		public string ClientSecret { get; set; }

		[JsonProperty("client_email")]
		public string ClientEmail { get; set; }

		[JsonProperty("private_key")]
		public string PrivateKey { get; set; }

		[JsonProperty("private_key_id")]
		public string PrivateKeyId { get; set; }

		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }
	}
}
