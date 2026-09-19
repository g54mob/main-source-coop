using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ChartSourceRange : IDirectResponseSchema
	{
		[JsonProperty("sources")]
		public virtual IList<GridRange> Sources { get; set; }

		public virtual string ETag { get; set; }
	}
}
