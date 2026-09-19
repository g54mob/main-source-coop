using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteDeveloperMetadataRequest : IDirectResponseSchema
	{
		[JsonProperty("dataFilter")]
		public virtual DataFilter DataFilter { get; set; }

		public virtual string ETag { get; set; }
	}
}
