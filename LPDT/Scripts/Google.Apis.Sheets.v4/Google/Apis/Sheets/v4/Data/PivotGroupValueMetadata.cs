using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotGroupValueMetadata : IDirectResponseSchema
	{
		[JsonProperty("collapsed")]
		public virtual bool? Collapsed { get; set; }

		[JsonProperty("value")]
		public virtual ExtendedValue Value { get; set; }

		public virtual string ETag { get; set; }
	}
}
