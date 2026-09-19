using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DateTimeRule : IDirectResponseSchema
	{
		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
