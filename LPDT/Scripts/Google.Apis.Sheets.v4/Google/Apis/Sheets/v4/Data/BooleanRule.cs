using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BooleanRule : IDirectResponseSchema
	{
		[JsonProperty("condition")]
		public virtual BooleanCondition Condition { get; set; }

		[JsonProperty("format")]
		public virtual CellFormat Format { get; set; }

		public virtual string ETag { get; set; }
	}
}
