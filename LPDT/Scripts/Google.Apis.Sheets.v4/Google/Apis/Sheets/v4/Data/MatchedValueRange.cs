using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class MatchedValueRange : IDirectResponseSchema
	{
		[JsonProperty("dataFilters")]
		public virtual IList<DataFilter> DataFilters { get; set; }

		[JsonProperty("valueRange")]
		public virtual ValueRange ValueRange { get; set; }

		public virtual string ETag { get; set; }
	}
}
