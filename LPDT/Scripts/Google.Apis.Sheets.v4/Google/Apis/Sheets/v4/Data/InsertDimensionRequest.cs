using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class InsertDimensionRequest : IDirectResponseSchema
	{
		[JsonProperty("inheritFromBefore")]
		public virtual bool? InheritFromBefore { get; set; }

		[JsonProperty("range")]
		public virtual DimensionRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
