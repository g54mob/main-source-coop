using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TreemapChartSpec : IDirectResponseSchema
	{
		[JsonProperty("colorData")]
		public virtual ChartData ColorData { get; set; }

		[JsonProperty("colorScale")]
		public virtual TreemapChartColorScale ColorScale { get; set; }

		[JsonProperty("headerColor")]
		public virtual Color HeaderColor { get; set; }

		[JsonProperty("headerColorStyle")]
		public virtual ColorStyle HeaderColorStyle { get; set; }

		[JsonProperty("hideTooltips")]
		public virtual bool? HideTooltips { get; set; }

		[JsonProperty("hintedLevels")]
		public virtual int? HintedLevels { get; set; }

		[JsonProperty("labels")]
		public virtual ChartData Labels { get; set; }

		[JsonProperty("levels")]
		public virtual int? Levels { get; set; }

		[JsonProperty("maxValue")]
		public virtual double? MaxValue { get; set; }

		[JsonProperty("minValue")]
		public virtual double? MinValue { get; set; }

		[JsonProperty("parentLabels")]
		public virtual ChartData ParentLabels { get; set; }

		[JsonProperty("sizeData")]
		public virtual ChartData SizeData { get; set; }

		[JsonProperty("textFormat")]
		public virtual TextFormat TextFormat { get; set; }

		public virtual string ETag { get; set; }
	}
}
