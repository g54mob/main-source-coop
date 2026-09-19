using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class RefreshDataSourceObjectExecutionStatus : IDirectResponseSchema
	{
		[JsonProperty("dataExecutionStatus")]
		public virtual DataExecutionStatus DataExecutionStatus { get; set; }

		[JsonProperty("reference")]
		public virtual DataSourceObjectReference Reference { get; set; }

		public virtual string ETag { get; set; }
	}
}
