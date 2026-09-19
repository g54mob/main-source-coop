using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CellFormat : IDirectResponseSchema
	{
		[JsonProperty("backgroundColor")]
		public virtual Color BackgroundColor { get; set; }

		[JsonProperty("backgroundColorStyle")]
		public virtual ColorStyle BackgroundColorStyle { get; set; }

		[JsonProperty("borders")]
		public virtual Borders Borders { get; set; }

		[JsonProperty("horizontalAlignment")]
		public virtual string HorizontalAlignment { get; set; }

		[JsonProperty("hyperlinkDisplayType")]
		public virtual string HyperlinkDisplayType { get; set; }

		[JsonProperty("numberFormat")]
		public virtual NumberFormat NumberFormat { get; set; }

		[JsonProperty("padding")]
		public virtual Padding Padding { get; set; }

		[JsonProperty("textDirection")]
		public virtual string TextDirection { get; set; }

		[JsonProperty("textFormat")]
		public virtual TextFormat TextFormat { get; set; }

		[JsonProperty("textRotation")]
		public virtual TextRotation TextRotation { get; set; }

		[JsonProperty("verticalAlignment")]
		public virtual string VerticalAlignment { get; set; }

		[JsonProperty("wrapStrategy")]
		public virtual string WrapStrategy { get; set; }

		public virtual string ETag { get; set; }
	}
}
