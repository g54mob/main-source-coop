using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TextFormat : IDirectResponseSchema
	{
		[JsonProperty("bold")]
		public virtual bool? Bold { get; set; }

		[JsonProperty("fontFamily")]
		public virtual string FontFamily { get; set; }

		[JsonProperty("fontSize")]
		public virtual int? FontSize { get; set; }

		[JsonProperty("foregroundColor")]
		public virtual Color ForegroundColor { get; set; }

		[JsonProperty("foregroundColorStyle")]
		public virtual ColorStyle ForegroundColorStyle { get; set; }

		[JsonProperty("italic")]
		public virtual bool? Italic { get; set; }

		[JsonProperty("link")]
		public virtual Link Link { get; set; }

		[JsonProperty("strikethrough")]
		public virtual bool? Strikethrough { get; set; }

		[JsonProperty("underline")]
		public virtual bool? Underline { get; set; }

		public virtual string ETag { get; set; }
	}
}
