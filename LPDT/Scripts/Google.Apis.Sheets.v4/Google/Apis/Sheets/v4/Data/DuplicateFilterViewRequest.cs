using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DuplicateFilterViewRequest : IDirectResponseSchema
	{
		[JsonProperty("filterId")]
		public virtual int? FilterId { get; set; }

		public virtual string ETag { get; set; }
	}
}
