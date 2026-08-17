using Newtonsoft.Json;

namespace Edgegap.Editor.Api.Models.Results
{
	public class EdgegapErrorResult
	{
		[JsonProperty("message")]
		public string ErrorMessage { get; set; }
	}
}
