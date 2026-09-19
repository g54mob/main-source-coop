using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BigQueryTableSpec : IDirectResponseSchema
	{
		[JsonProperty("datasetId")]
		public virtual string DatasetId { get; set; }

		[JsonProperty("tableId")]
		public virtual string TableId { get; set; }

		[JsonProperty("tableProjectId")]
		public virtual string TableProjectId { get; set; }

		public virtual string ETag { get; set; }
	}
}
