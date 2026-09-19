using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class WaterfallChartSeries : IDirectResponseSchema
	{
		[JsonProperty("customSubtotals")]
		public virtual IList<WaterfallChartCustomSubtotal> CustomSubtotals { get; set; }

		[JsonProperty("data")]
		public virtual ChartData Data { get; set; }

		[JsonProperty("dataLabel")]
		public virtual DataLabel DataLabel { get; set; }

		[JsonProperty("hideTrailingSubtotal")]
		public virtual bool? HideTrailingSubtotal { get; set; }

		[JsonProperty("negativeColumnsStyle")]
		public virtual WaterfallChartColumnStyle NegativeColumnsStyle { get; set; }

		[JsonProperty("positiveColumnsStyle")]
		public virtual WaterfallChartColumnStyle PositiveColumnsStyle { get; set; }

		[JsonProperty("subtotalColumnsStyle")]
		public virtual WaterfallChartColumnStyle SubtotalColumnsStyle { get; set; }

		public virtual string ETag { get; set; }
	}
}
