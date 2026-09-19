using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class MergeCellsRequest : IDirectResponseSchema
	{
		[JsonProperty("mergeType")]
		public virtual string MergeType { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
