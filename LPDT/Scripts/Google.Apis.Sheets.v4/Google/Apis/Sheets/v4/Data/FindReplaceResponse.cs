using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class FindReplaceResponse : IDirectResponseSchema
	{
		[JsonProperty("formulasChanged")]
		public virtual int? FormulasChanged { get; set; }

		[JsonProperty("occurrencesChanged")]
		public virtual int? OccurrencesChanged { get; set; }

		[JsonProperty("rowsChanged")]
		public virtual int? RowsChanged { get; set; }

		[JsonProperty("sheetsChanged")]
		public virtual int? SheetsChanged { get; set; }

		[JsonProperty("valuesChanged")]
		public virtual int? ValuesChanged { get; set; }

		public virtual string ETag { get; set; }
	}
}
