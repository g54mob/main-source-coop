using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BasicChartSeries : IDirectResponseSchema
	{
		[JsonProperty("color")]
		public virtual Color Color { get; set; }

		[JsonProperty("colorStyle")]
		public virtual ColorStyle ColorStyle { get; set; }

		[JsonProperty("dataLabel")]
		public virtual DataLabel DataLabel { get; set; }

		[JsonProperty("lineStyle")]
		public virtual LineStyle LineStyle { get; set; }

		[JsonProperty("pointStyle")]
		public virtual PointStyle PointStyle { get; set; }

		[JsonProperty("series")]
		public virtual ChartData Series { get; set; }

		[JsonProperty("styleOverrides")]
		public virtual IList<BasicSeriesDataPointStyleOverride> StyleOverrides { get; set; }

		[JsonProperty("targetAxis")]
		public virtual string TargetAxis { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
