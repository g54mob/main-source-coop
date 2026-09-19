using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateConditionalFormatRuleResponse : IDirectResponseSchema
	{
		[JsonProperty("newIndex")]
		public virtual int? NewIndex { get; set; }

		[JsonProperty("newRule")]
		public virtual ConditionalFormatRule NewRule { get; set; }

		[JsonProperty("oldIndex")]
		public virtual int? OldIndex { get; set; }

		[JsonProperty("oldRule")]
		public virtual ConditionalFormatRule OldRule { get; set; }

		public virtual string ETag { get; set; }
	}
}
