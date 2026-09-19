using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteConditionalFormatRuleRequest : IDirectResponseSchema
	{
		[JsonProperty("index")]
		public virtual int? Index { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
