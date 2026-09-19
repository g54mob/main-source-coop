using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DimensionRange : IDirectResponseSchema
	{
		[JsonProperty("dimension")]
		public virtual string Dimension { get; set; }

		[JsonProperty("endIndex")]
		public virtual int? EndIndex { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		[JsonProperty("startIndex")]
		public virtual int? StartIndex { get; set; }

		public virtual string ETag { get; set; }
	}
}
