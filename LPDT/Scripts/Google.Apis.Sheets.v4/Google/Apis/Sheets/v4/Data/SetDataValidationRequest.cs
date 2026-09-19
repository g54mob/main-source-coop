using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SetDataValidationRequest : IDirectResponseSchema
	{
		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("rule")]
		public virtual DataValidationRule Rule { get; set; }

		public virtual string ETag { get; set; }
	}
}
