using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateConditionalFormatRuleRequest : IDirectResponseSchema
	{
		[JsonProperty("index")]
		public virtual int? Index { get; set; }

		[JsonProperty("newIndex")]
		public virtual int? NewIndex { get; set; }

		[JsonProperty("rule")]
		public virtual ConditionalFormatRule Rule { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
