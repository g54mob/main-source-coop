using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateDataSourceResponse : IDirectResponseSchema
	{
		[JsonProperty("dataExecutionStatus")]
		public virtual DataExecutionStatus DataExecutionStatus { get; set; }

		[JsonProperty("dataSource")]
		public virtual DataSource DataSource { get; set; }

		public virtual string ETag { get; set; }
	}
}
