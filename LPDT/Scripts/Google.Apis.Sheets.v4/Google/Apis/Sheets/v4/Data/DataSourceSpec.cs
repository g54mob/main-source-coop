using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceSpec : IDirectResponseSchema
	{
		[JsonProperty("bigQuery")]
		public virtual BigQueryDataSourceSpec BigQuery { get; set; }

		[JsonProperty("parameters")]
		public virtual IList<DataSourceParameter> Parameters { get; set; }

		public virtual string ETag { get; set; }
	}
}
