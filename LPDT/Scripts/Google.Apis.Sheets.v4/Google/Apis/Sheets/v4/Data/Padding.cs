using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Padding : IDirectResponseSchema
	{
		[JsonProperty("bottom")]
		public virtual int? Bottom { get; set; }

		[JsonProperty("left")]
		public virtual int? Left { get; set; }

		[JsonProperty("right")]
		public virtual int? Right { get; set; }

		[JsonProperty("top")]
		public virtual int? Top { get; set; }

		public virtual string ETag { get; set; }
	}
}
