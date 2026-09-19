using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TrimWhitespaceResponse : IDirectResponseSchema
	{
		[JsonProperty("cellsChangedCount")]
		public virtual int? CellsChangedCount { get; set; }

		public virtual string ETag { get; set; }
	}
}
