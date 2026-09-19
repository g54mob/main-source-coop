using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SlicerSpec : IDirectResponseSchema
	{
		[JsonProperty("applyToPivotTables")]
		public virtual bool? ApplyToPivotTables { get; set; }

		[JsonProperty("backgroundColor")]
		public virtual Color BackgroundColor { get; set; }

		[JsonProperty("backgroundColorStyle")]
		public virtual ColorStyle BackgroundColorStyle { get; set; }

		[JsonProperty("columnIndex")]
		public virtual int? ColumnIndex { get; set; }

		[JsonProperty("dataRange")]
		public virtual GridRange DataRange { get; set; }

		[JsonProperty("filterCriteria")]
		public virtual FilterCriteria FilterCriteria { get; set; }

		[JsonProperty("horizontalAlignment")]
		public virtual string HorizontalAlignment { get; set; }

		[JsonProperty("textFormat")]
		public virtual TextFormat TextFormat { get; set; }

		[JsonProperty("title")]
		public virtual string Title { get; set; }

		public virtual string ETag { get; set; }
	}
}
