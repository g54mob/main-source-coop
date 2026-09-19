using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceObjectReference : IDirectResponseSchema
	{
		[JsonProperty("chartId")]
		public virtual int? ChartId { get; set; }

		[JsonProperty("dataSourceFormulaCell")]
		public virtual GridCoordinate DataSourceFormulaCell { get; set; }

		[JsonProperty("dataSourcePivotTableAnchorCell")]
		public virtual GridCoordinate DataSourcePivotTableAnchorCell { get; set; }

		[JsonProperty("dataSourceTableAnchorCell")]
		public virtual GridCoordinate DataSourceTableAnchorCell { get; set; }

		[JsonProperty("sheetId")]
		public virtual string SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
