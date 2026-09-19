using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class WaterfallChartCustomSubtotal : IDirectResponseSchema
	{
		[JsonProperty("dataIsSubtotal")]
		public virtual bool? DataIsSubtotal { get; set; }

		[JsonProperty("label")]
		public virtual string Label { get; set; }

		[JsonProperty("subtotalIndex")]
		public virtual int? SubtotalIndex { get; set; }

		public virtual string ETag { get; set; }
	}
}
