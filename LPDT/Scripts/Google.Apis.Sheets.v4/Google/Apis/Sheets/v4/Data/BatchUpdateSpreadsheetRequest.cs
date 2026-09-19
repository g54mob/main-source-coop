using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchUpdateSpreadsheetRequest : IDirectResponseSchema
	{
		[JsonProperty("includeSpreadsheetInResponse")]
		public virtual bool? IncludeSpreadsheetInResponse { get; set; }

		[JsonProperty("requests")]
		public virtual IList<Request> Requests { get; set; }

		[JsonProperty("responseIncludeGridData")]
		public virtual bool? ResponseIncludeGridData { get; set; }

		[JsonProperty("responseRanges")]
		public virtual IList<string> ResponseRanges { get; set; }

		public virtual string ETag { get; set; }
	}
}
