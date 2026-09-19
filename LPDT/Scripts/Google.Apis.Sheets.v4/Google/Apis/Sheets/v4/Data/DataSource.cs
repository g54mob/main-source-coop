using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSource : IDirectResponseSchema
	{
		[JsonProperty("calculatedColumns")]
		public virtual IList<DataSourceColumn> CalculatedColumns { get; set; }

		[JsonProperty("dataSourceId")]
		public virtual string DataSourceId { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		[JsonProperty("spec")]
		public virtual DataSourceSpec Spec { get; set; }

		public virtual string ETag { get; set; }
	}
}
