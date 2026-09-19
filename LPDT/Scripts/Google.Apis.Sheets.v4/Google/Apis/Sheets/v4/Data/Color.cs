using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Color : IDirectResponseSchema
	{
		[JsonProperty("alpha")]
		public virtual float? Alpha { get; set; }

		[JsonProperty("blue")]
		public virtual float? Blue { get; set; }

		[JsonProperty("green")]
		public virtual float? Green { get; set; }

		[JsonProperty("red")]
		public virtual float? Red { get; set; }

		public virtual string ETag { get; set; }
	}
}
