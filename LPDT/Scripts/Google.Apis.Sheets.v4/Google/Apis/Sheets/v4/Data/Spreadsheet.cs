using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Spreadsheet : IDirectResponseSchema
	{
		[JsonProperty("dataSourceSchedules")]
		public virtual IList<DataSourceRefreshSchedule> DataSourceSchedules { get; set; }

		[JsonProperty("dataSources")]
		public virtual IList<DataSource> DataSources { get; set; }

		[JsonProperty("developerMetadata")]
		public virtual IList<DeveloperMetadata> DeveloperMetadata { get; set; }

		[JsonProperty("namedRanges")]
		public virtual IList<NamedRange> NamedRanges { get; set; }

		[JsonProperty("properties")]
		public virtual SpreadsheetProperties Properties { get; set; }

		[JsonProperty("sheets")]
		public virtual IList<Sheet> Sheets { get; set; }

		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		[JsonProperty("spreadsheetUrl")]
		public virtual string SpreadsheetUrl { get; set; }

		public virtual string ETag { get; set; }
	}
}
