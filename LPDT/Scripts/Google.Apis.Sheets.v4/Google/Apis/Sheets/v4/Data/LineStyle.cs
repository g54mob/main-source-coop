using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class LineStyle : IDirectResponseSchema
	{
		[JsonProperty("type")]
		public virtual string Type { get; set; }

		[JsonProperty("width")]
		public virtual int? Width { get; set; }

		public virtual string ETag { get; set; }
	}
}
