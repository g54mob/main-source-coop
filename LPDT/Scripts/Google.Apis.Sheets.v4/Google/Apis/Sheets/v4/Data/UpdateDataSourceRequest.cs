using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateDataSourceRequest : IDirectResponseSchema
	{
		[JsonProperty("dataSource")]
		public virtual DataSource DataSource { get; set; }

		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		public virtual string ETag { get; set; }
	}
}
