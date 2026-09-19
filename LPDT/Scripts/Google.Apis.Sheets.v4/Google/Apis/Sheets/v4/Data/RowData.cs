using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class RowData : IDirectResponseSchema
	{
		[JsonProperty("values")]
		public virtual IList<CellData> Values { get; set; }

		public virtual string ETag { get; set; }
	}
}
