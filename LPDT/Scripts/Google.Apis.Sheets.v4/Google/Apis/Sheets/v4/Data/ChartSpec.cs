using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ChartSpec : IDirectResponseSchema
	{
		[JsonProperty("altText")]
		public virtual string AltText { get; set; }

		[JsonProperty("backgroundColor")]
		public virtual Color BackgroundColor { get; set; }

		[JsonProperty("backgroundColorStyle")]
		public virtual ColorStyle BackgroundColorStyle { get; set; }

		[JsonProperty("basicChart")]
		public virtual BasicChartSpec BasicChart { get; set; }

		[JsonProperty("bubbleChart")]
		public virtual BubbleChartSpec BubbleChart { get; set; }

		[JsonProperty("candlestickChart")]
		public virtual CandlestickChartSpec CandlestickChart { get; set; }

		[JsonProperty("dataSourceChartProperties")]
		public virtual DataSourceChartProperties DataSourceChartProperties { get; set; }

		[JsonProperty("filterSpecs")]
		public virtual IList<FilterSpec> FilterSpecs { get; set; }

		[JsonProperty("fontName")]
		public virtual string FontName { get; set; }

		[JsonProperty("hiddenDimensionStrategy")]
		public virtual string HiddenDimensionStrategy { get; set; }

		[JsonProperty("histogramChart")]
		public virtual HistogramChartSpec HistogramChart { get; set; }

		[JsonProperty("maximized")]
		public virtual bool? Maximized { get; set; }

		[JsonProperty("orgChart")]
		public virtual OrgChartSpec OrgChart { get; set; }

		[JsonProperty("pieChart")]
		public virtual PieChartSpec PieChart { get; set; }

		[JsonProperty("scorecardChart")]
		public virtual ScorecardChartSpec ScorecardChart { get; set; }

		[JsonProperty("sortSpecs")]
		public virtual IList<SortSpec> SortSpecs { get; set; }

		[JsonProperty("subtitle")]
		public virtual string Subtitle { get; set; }

		[JsonProperty("subtitleTextFormat")]
		public virtual TextFormat SubtitleTextFormat { get; set; }

		[JsonProperty("subtitleTextPosition")]
		public virtual TextPosition SubtitleTextPosition { get; set; }

		[JsonProperty("title")]
		public virtual string Title { get; set; }

		[JsonProperty("titleTextFormat")]
		public virtual TextFormat TitleTextFormat { get; set; }

		[JsonProperty("titleTextPosition")]
		public virtual TextPosition TitleTextPosition { get; set; }

		[JsonProperty("treemapChart")]
		public virtual TreemapChartSpec TreemapChart { get; set; }

		[JsonProperty("waterfallChart")]
		public virtual WaterfallChartSpec WaterfallChart { get; set; }

		public virtual string ETag { get; set; }
	}
}
