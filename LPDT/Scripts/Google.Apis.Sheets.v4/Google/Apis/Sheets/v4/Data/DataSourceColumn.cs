using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceColumn : IDirectResponseSchema
	{
		[JsonProperty("formula")]
		public virtual string Formula { get; set; }

		[JsonProperty("reference")]
		public virtual DataSourceColumnReference Reference { get; set; }

		public virtual string ETag { get; set; }
	}
}
