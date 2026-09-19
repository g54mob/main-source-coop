using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TimeOfDay : IDirectResponseSchema
	{
		[JsonProperty("hours")]
		public virtual int? Hours { get; set; }

		[JsonProperty("minutes")]
		public virtual int? Minutes { get; set; }

		[JsonProperty("nanos")]
		public virtual int? Nanos { get; set; }

		[JsonProperty("seconds")]
		public virtual int? Seconds { get; set; }

		public virtual string ETag { get; set; }
	}
}
