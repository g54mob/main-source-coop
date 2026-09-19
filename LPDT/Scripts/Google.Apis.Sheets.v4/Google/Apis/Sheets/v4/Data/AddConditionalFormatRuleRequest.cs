using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddConditionalFormatRuleRequest : IDirectResponseSchema
	{
		[JsonProperty("index")]
		public virtual int? Index { get; set; }

		[JsonProperty("rule")]
		public virtual ConditionalFormatRule Rule { get; set; }

		public virtual string ETag { get; set; }
	}
}
