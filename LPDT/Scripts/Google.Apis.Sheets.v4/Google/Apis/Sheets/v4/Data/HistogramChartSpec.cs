using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class HistogramChartSpec : IDirectResponseSchema
	{
		[JsonProperty("bucketSize")]
		public virtual double? BucketSize { get; set; }

		[JsonProperty("legendPosition")]
		public virtual string LegendPosition { get; set; }

		[JsonProperty("outlierPercentile")]
		public virtual double? OutlierPercentile { get; set; }

		[JsonProperty("series")]
		public virtual IList<HistogramSeries> Series { get; set; }

		[JsonProperty("showItemDividers")]
		public virtual bool? ShowItemDividers { get; set; }

		public virtual string ETag { get; set; }
	}
}
