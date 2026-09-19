using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Borders : IDirectResponseSchema
	{
		[JsonProperty("bottom")]
		public virtual Border Bottom { get; set; }

		[JsonProperty("left")]
		public virtual Border Left { get; set; }

		[JsonProperty("right")]
		public virtual Border Right { get; set; }

		[JsonProperty("top")]
		public virtual Border Top { get; set; }

		public virtual string ETag { get; set; }
	}
}
