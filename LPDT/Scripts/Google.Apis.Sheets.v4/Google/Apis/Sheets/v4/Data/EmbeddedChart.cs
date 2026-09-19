using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class EmbeddedChart : IDirectResponseSchema
	{
		[JsonProperty("border")]
		public virtual EmbeddedObjectBorder Border { get; set; }

		[JsonProperty("chartId")]
		public virtual int? ChartId { get; set; }

		[JsonProperty("position")]
		public virtual EmbeddedObjectPosition Position { get; set; }

		[JsonProperty("spec")]
		public virtual ChartSpec Spec { get; set; }

		public virtual string ETag { get; set; }
	}
}
