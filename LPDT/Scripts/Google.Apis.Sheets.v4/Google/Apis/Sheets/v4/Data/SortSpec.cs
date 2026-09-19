using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SortSpec : IDirectResponseSchema
	{
		[JsonProperty("backgroundColor")]
		public virtual Color BackgroundColor { get; set; }

		[JsonProperty("backgroundColorStyle")]
		public virtual ColorStyle BackgroundColorStyle { get; set; }

		[JsonProperty("dataSourceColumnReference")]
		public virtual DataSourceColumnReference DataSourceColumnReference { get; set; }

		[JsonProperty("dimensionIndex")]
		public virtual int? DimensionIndex { get; set; }

		[JsonProperty("foregroundColor")]
		public virtual Color ForegroundColor { get; set; }

		[JsonProperty("foregroundColorStyle")]
		public virtual ColorStyle ForegroundColorStyle { get; set; }

		[JsonProperty("sortOrder")]
		public virtual string SortOrder { get; set; }

		public virtual string ETag { get; set; }
	}
}
