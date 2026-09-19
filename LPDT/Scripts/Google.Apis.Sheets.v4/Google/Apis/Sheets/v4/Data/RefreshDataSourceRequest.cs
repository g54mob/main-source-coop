using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class RefreshDataSourceRequest : IDirectResponseSchema
	{
		[JsonProperty("dataSourceId")]
		public virtual string DataSourceId { get; set; }

		[JsonProperty("force")]
		public virtual bool? Force { get; set; }

		[JsonProperty("isAll")]
		public virtual bool? IsAll { get; set; }

		[JsonProperty("references")]
		public virtual DataSourceObjectReferences References { get; set; }

		public virtual string ETag { get; set; }
	}
}
