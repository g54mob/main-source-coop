using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BaselineValueFormat : IDirectResponseSchema
	{
		[JsonProperty("comparisonType")]
		public virtual string ComparisonType { get; set; }

		[JsonProperty("description")]
		public virtual string Description { get; set; }

		[JsonProperty("negativeColor")]
		public virtual Color NegativeColor { get; set; }

		[JsonProperty("negativeColorStyle")]
		public virtual ColorStyle NegativeColorStyle { get; set; }

		[JsonProperty("position")]
		public virtual TextPosition Position { get; set; }

		[JsonProperty("positiveColor")]
		public virtual Color PositiveColor { get; set; }

		[JsonProperty("positiveColorStyle")]
		public virtual ColorStyle PositiveColorStyle { get; set; }

		[JsonProperty("textFormat")]
		public virtual TextFormat TextFormat { get; set; }

		public virtual string ETag { get; set; }
	}
}
