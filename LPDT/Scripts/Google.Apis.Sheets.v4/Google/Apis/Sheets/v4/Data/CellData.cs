using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CellData : IDirectResponseSchema
	{
		[JsonProperty("dataSourceFormula")]
		public virtual DataSourceFormula DataSourceFormula { get; set; }

		[JsonProperty("dataSourceTable")]
		public virtual DataSourceTable DataSourceTable { get; set; }

		[JsonProperty("dataValidation")]
		public virtual DataValidationRule DataValidation { get; set; }

		[JsonProperty("effectiveFormat")]
		public virtual CellFormat EffectiveFormat { get; set; }

		[JsonProperty("effectiveValue")]
		public virtual ExtendedValue EffectiveValue { get; set; }

		[JsonProperty("formattedValue")]
		public virtual string FormattedValue { get; set; }

		[JsonProperty("hyperlink")]
		public virtual string Hyperlink { get; set; }

		[JsonProperty("note")]
		public virtual string Note { get; set; }

		[JsonProperty("pivotTable")]
		public virtual PivotTable PivotTable { get; set; }

		[JsonProperty("textFormatRuns")]
		public virtual IList<TextFormatRun> TextFormatRuns { get; set; }

		[JsonProperty("userEnteredFormat")]
		public virtual CellFormat UserEnteredFormat { get; set; }

		[JsonProperty("userEnteredValue")]
		public virtual ExtendedValue UserEnteredValue { get; set; }

		public virtual string ETag { get; set; }
	}
}
