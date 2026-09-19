using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceSheetProperties : IDirectResponseSchema
	{
		[JsonProperty("columns")]
		public virtual IList<DataSourceColumn> Columns { get; set; }

		[JsonProperty("dataExecutionStatus")]
		public virtual DataExecutionStatus DataExecutionStatus { get; set; }

		[JsonProperty("dataSourceId")]
		public virtual string DataSourceId { get; set; }

		public virtual string ETag { get; set; }
	}
}
