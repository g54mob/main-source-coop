using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SetBasicFilterRequest : IDirectResponseSchema
	{
		[JsonProperty("filter")]
		public virtual BasicFilter Filter { get; set; }

		public virtual string ETag { get; set; }
	}
}
