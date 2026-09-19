using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataLabel : IDirectResponseSchema
	{
		[JsonProperty("customLabelData")]
		public virtual ChartData CustomLabelData { get; set; }

		[JsonProperty("placement")]
		public virtual string Placement { get; set; }

		[JsonProperty("textFormat")]
		public virtual TextFormat TextFormat { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
