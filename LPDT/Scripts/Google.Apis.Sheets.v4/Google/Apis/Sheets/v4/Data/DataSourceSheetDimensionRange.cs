using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceSheetDimensionRange : IDirectResponseSchema
	{
		[JsonProperty("columnReferences")]
		public virtual IList<DataSourceColumnReference> ColumnReferences { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
