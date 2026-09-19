using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchClearValuesRequest : IDirectResponseSchema
	{
		[JsonProperty("ranges")]
		public virtual IList<string> Ranges { get; set; }

		public virtual string ETag { get; set; }
	}
}
