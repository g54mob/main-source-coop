using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class NamedRange : IDirectResponseSchema
	{
		[JsonProperty("name")]
		public virtual string Name { get; set; }

		[JsonProperty("namedRangeId")]
		public virtual string NamedRangeId { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
