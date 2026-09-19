using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PieChartSpec : IDirectResponseSchema
	{
		[JsonProperty("domain")]
		public virtual ChartData Domain { get; set; }

		[JsonProperty("legendPosition")]
		public virtual string LegendPosition { get; set; }

		[JsonProperty("pieHole")]
		public virtual double? PieHole { get; set; }

		[JsonProperty("series")]
		public virtual ChartData Series { get; set; }

		[JsonProperty("threeDimensional")]
		public virtual bool? ThreeDimensional { get; set; }

		public virtual string ETag { get; set; }
	}
}
