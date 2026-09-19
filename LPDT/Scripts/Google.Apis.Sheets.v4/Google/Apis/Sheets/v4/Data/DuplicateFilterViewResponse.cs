using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DuplicateFilterViewResponse : IDirectResponseSchema
	{
		[JsonProperty("filter")]
		public virtual FilterView Filter { get; set; }

		public virtual string ETag { get; set; }
	}
}
