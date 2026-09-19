using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddDimensionGroupRequest : IDirectResponseSchema
	{
		[JsonProperty("range")]
		public virtual DimensionRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
