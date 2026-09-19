using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TreemapChartColorScale : IDirectResponseSchema
	{
		[JsonProperty("maxValueColor")]
		public virtual Color MaxValueColor { get; set; }

		[JsonProperty("maxValueColorStyle")]
		public virtual ColorStyle MaxValueColorStyle { get; set; }

		[JsonProperty("midValueColor")]
		public virtual Color MidValueColor { get; set; }

		[JsonProperty("midValueColorStyle")]
		public virtual ColorStyle MidValueColorStyle { get; set; }

		[JsonProperty("minValueColor")]
		public virtual Color MinValueColor { get; set; }

		[JsonProperty("minValueColorStyle")]
		public virtual ColorStyle MinValueColorStyle { get; set; }

		[JsonProperty("noDataColor")]
		public virtual Color NoDataColor { get; set; }

		[JsonProperty("noDataColorStyle")]
		public virtual ColorStyle NoDataColorStyle { get; set; }

		public virtual string ETag { get; set; }
	}
}
