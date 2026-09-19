using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Util
{
	public sealed class StandardResponse<InnerType>
	{
		[JsonProperty("data")]
		public InnerType Data { get; set; }

		[JsonProperty("error")]
		public RequestError Error { get; set; }
	}
}
