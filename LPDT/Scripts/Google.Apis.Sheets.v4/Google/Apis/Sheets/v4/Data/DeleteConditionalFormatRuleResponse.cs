using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteConditionalFormatRuleResponse : IDirectResponseSchema
	{
		[JsonProperty("rule")]
		public virtual ConditionalFormatRule Rule { get; set; }

		public virtual string ETag { get; set; }
	}
}
