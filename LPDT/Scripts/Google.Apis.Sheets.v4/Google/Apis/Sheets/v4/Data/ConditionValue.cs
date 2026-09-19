using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ConditionValue : IDirectResponseSchema
	{
		[JsonProperty("relativeDate")]
		public virtual string RelativeDate { get; set; }

		[JsonProperty("userEnteredValue")]
		public virtual string UserEnteredValue { get; set; }

		public virtual string ETag { get; set; }
	}
}
