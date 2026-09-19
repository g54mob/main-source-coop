using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataValidationRule : IDirectResponseSchema
	{
		[JsonProperty("condition")]
		public virtual BooleanCondition Condition { get; set; }

		[JsonProperty("inputMessage")]
		public virtual string InputMessage { get; set; }

		[JsonProperty("showCustomUi")]
		public virtual bool? ShowCustomUi { get; set; }

		[JsonProperty("strict")]
		public virtual bool? Strict { get; set; }

		public virtual string ETag { get; set; }
	}
}
