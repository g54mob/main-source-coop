using Newtonsoft.Json;

namespace Edgegap.Editor.Api.Models.Results
{
	public class GetYourPublicIpResult
	{
		[JsonProperty("public_ip")]
		public string PublicIp { get; set; }
	}
}
