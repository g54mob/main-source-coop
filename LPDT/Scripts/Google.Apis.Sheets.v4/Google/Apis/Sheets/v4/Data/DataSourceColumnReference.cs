using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceColumnReference : IDirectResponseSchema
	{
		[JsonProperty("name")]
		public virtual string Name { get; set; }

		public virtual string ETag { get; set; }
	}
}
