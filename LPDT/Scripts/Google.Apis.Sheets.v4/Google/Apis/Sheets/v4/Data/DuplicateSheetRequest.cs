using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DuplicateSheetRequest : IDirectResponseSchema
	{
		[JsonProperty("insertSheetIndex")]
		public virtual int? InsertSheetIndex { get; set; }

		[JsonProperty("newSheetId")]
		public virtual int? NewSheetId { get; set; }

		[JsonProperty("newSheetName")]
		public virtual string NewSheetName { get; set; }

		[JsonProperty("sourceSheetId")]
		public virtual int? SourceSheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
