using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteDataSourceRequest : IDirectResponseSchema
	{
		[JsonProperty("dataSourceId")]
		public virtual string DataSourceId { get; set; }

		public virtual string ETag { get; set; }
	}
}
