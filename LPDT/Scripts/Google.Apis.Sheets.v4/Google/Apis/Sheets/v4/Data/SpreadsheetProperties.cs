using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SpreadsheetProperties : IDirectResponseSchema
	{
		[JsonProperty("autoRecalc")]
		public virtual string AutoRecalc { get; set; }

		[JsonProperty("defaultFormat")]
		public virtual CellFormat DefaultFormat { get; set; }

		[JsonProperty("iterativeCalculationSettings")]
		public virtual IterativeCalculationSettings IterativeCalculationSettings { get; set; }

		[JsonProperty("locale")]
		public virtual string Locale { get; set; }

		[JsonProperty("spreadsheetTheme")]
		public virtual SpreadsheetTheme SpreadsheetTheme { get; set; }

		[JsonProperty("timeZone")]
		public virtual string TimeZone { get; set; }

		[JsonProperty("title")]
		public virtual string Title { get; set; }

		public virtual string ETag { get; set; }
	}
}
