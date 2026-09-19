using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DimensionGroup : IDirectResponseSchema
	{
		[JsonProperty("collapsed")]
		public virtual bool? Collapsed { get; set; }

		[JsonProperty("depth")]
		public virtual int? Depth { get; set; }

		[JsonProperty("range")]
		public virtual DimensionRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
