using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BasicChartSpec : IDirectResponseSchema
	{
		[JsonProperty("axis")]
		public virtual IList<BasicChartAxis> Axis { get; set; }

		[JsonProperty("chartType")]
		public virtual string ChartType { get; set; }

		[JsonProperty("compareMode")]
		public virtual string CompareMode { get; set; }

		[JsonProperty("domains")]
		public virtual IList<BasicChartDomain> Domains { get; set; }

		[JsonProperty("headerCount")]
		public virtual int? HeaderCount { get; set; }

		[JsonProperty("interpolateNulls")]
		public virtual bool? InterpolateNulls { get; set; }

		[JsonProperty("legendPosition")]
		public virtual string LegendPosition { get; set; }

		[JsonProperty("lineSmoothing")]
		public virtual bool? LineSmoothing { get; set; }

		[JsonProperty("series")]
		public virtual IList<BasicChartSeries> Series { get; set; }

		[JsonProperty("stackedType")]
		public virtual string StackedType { get; set; }

		[JsonProperty("threeDimensional")]
		public virtual bool? ThreeDimensional { get; set; }

		[JsonProperty("totalDataLabel")]
		public virtual DataLabel TotalDataLabel { get; set; }

		public virtual string ETag { get; set; }
	}
}
