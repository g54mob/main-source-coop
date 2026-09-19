using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BubbleChartSpec : IDirectResponseSchema
	{
		[JsonProperty("bubbleBorderColor")]
		public virtual Color BubbleBorderColor { get; set; }

		[JsonProperty("bubbleBorderColorStyle")]
		public virtual ColorStyle BubbleBorderColorStyle { get; set; }

		[JsonProperty("bubbleLabels")]
		public virtual ChartData BubbleLabels { get; set; }

		[JsonProperty("bubbleMaxRadiusSize")]
		public virtual int? BubbleMaxRadiusSize { get; set; }

		[JsonProperty("bubbleMinRadiusSize")]
		public virtual int? BubbleMinRadiusSize { get; set; }

		[JsonProperty("bubbleOpacity")]
		public virtual float? BubbleOpacity { get; set; }

		[JsonProperty("bubbleSizes")]
		public virtual ChartData BubbleSizes { get; set; }

		[JsonProperty("bubbleTextStyle")]
		public virtual TextFormat BubbleTextStyle { get; set; }

		[JsonProperty("domain")]
		public virtual ChartData Domain { get; set; }

		[JsonProperty("groupIds")]
		public virtual ChartData GroupIds { get; set; }

		[JsonProperty("legendPosition")]
		public virtual string LegendPosition { get; set; }

		[JsonProperty("series")]
		public virtual ChartData Series { get; set; }

		public virtual string ETag { get; set; }
	}
}
