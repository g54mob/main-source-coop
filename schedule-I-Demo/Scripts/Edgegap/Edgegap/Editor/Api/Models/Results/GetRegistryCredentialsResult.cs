using Newtonsoft.Json;

namespace Edgegap.Editor.Api.Models.Results
{
	public class GetRegistryCredentialsResult
	{
		[JsonProperty("registry_url")]
		public string RegistryUrl { get; set; }

		[JsonProperty("project")]
		public string Project { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("token")]
		public string Token { get; set; }
	}
}
