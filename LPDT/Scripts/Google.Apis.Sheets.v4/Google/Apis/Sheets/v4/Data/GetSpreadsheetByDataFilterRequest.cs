using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class GetSpreadsheetByDataFilterRequest : IDirectResponseSchema
	{
		[JsonProperty("dataFilters")]
		public virtual IList<DataFilter> DataFilters { get; set; }

		[JsonProperty("includeGridData")]
		public virtual bool? IncludeGridData { get; set; }

		public virtual string ETag { get; set; }
	}
}
