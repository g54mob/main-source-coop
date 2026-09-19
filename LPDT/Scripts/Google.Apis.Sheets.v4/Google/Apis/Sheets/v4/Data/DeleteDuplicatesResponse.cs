using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteDuplicatesResponse : IDirectResponseSchema
	{
		[JsonProperty("duplicatesRemovedCount")]
		public virtual int? DuplicatesRemovedCount { get; set; }

		public virtual string ETag { get; set; }
	}
}
