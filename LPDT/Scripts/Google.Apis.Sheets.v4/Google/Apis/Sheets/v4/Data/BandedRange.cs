using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BandedRange : IDirectResponseSchema
	{
		[JsonProperty("bandedRangeId")]
		public virtual int? BandedRangeId { get; set; }

		[JsonProperty("columnProperties")]
		public virtual BandingProperties ColumnProperties { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("rowProperties")]
		public virtual BandingProperties RowProperties { get; set; }

		public virtual string ETag { get; set; }
	}
}
