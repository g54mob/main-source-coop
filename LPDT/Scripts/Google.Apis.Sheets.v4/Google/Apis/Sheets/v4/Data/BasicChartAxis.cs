using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BasicChartAxis : IDirectResponseSchema
	{
		[JsonProperty("format")]
		public virtual TextFormat Format { get; set; }

		[JsonProperty("position")]
		public virtual string Position { get; set; }

		[JsonProperty("title")]
		public virtual string Title { get; set; }

		[JsonProperty("titleTextPosition")]
		public virtual TextPosition TitleTextPosition { get; set; }

		[JsonProperty("viewWindowOptions")]
		public virtual ChartAxisViewWindowOptions ViewWindowOptions { get; set; }

		public virtual string ETag { get; set; }
	}
}
