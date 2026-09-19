using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateChartSpecRequest : IDirectResponseSchema
	{
		[JsonProperty("chartId")]
		public virtual int? ChartId { get; set; }

		[JsonProperty("spec")]
		public virtual ChartSpec Spec { get; set; }

		public virtual string ETag { get; set; }
	}
}
