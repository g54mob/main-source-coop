using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BigQueryDataSourceSpec : IDirectResponseSchema
	{
		[JsonProperty("projectId")]
		public virtual string ProjectId { get; set; }

		[JsonProperty("querySpec")]
		public virtual BigQueryQuerySpec QuerySpec { get; set; }

		[JsonProperty("tableSpec")]
		public virtual BigQueryTableSpec TableSpec { get; set; }

		public virtual string ETag { get; set; }
	}
}
