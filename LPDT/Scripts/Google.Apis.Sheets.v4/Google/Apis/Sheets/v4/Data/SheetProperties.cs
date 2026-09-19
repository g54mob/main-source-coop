using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SheetProperties : IDirectResponseSchema
	{
		[JsonProperty("dataSourceSheetProperties")]
		public virtual DataSourceSheetProperties DataSourceSheetProperties { get; set; }

		[JsonProperty("gridProperties")]
		public virtual GridProperties GridProperties { get; set; }

		[JsonProperty("hidden")]
		public virtual bool? Hidden { get; set; }

		[JsonProperty("index")]
		public virtual int? Index { get; set; }

		[JsonProperty("rightToLeft")]
		public virtual bool? RightToLeft { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		[JsonProperty("sheetType")]
		public virtual string SheetType { get; set; }

		[JsonProperty("tabColor")]
		public virtual Color TabColor { get; set; }

		[JsonProperty("tabColorStyle")]
		public virtual ColorStyle TabColorStyle { get; set; }

		[JsonProperty("title")]
		public virtual string Title { get; set; }

		public virtual string ETag { get; set; }
	}
}
