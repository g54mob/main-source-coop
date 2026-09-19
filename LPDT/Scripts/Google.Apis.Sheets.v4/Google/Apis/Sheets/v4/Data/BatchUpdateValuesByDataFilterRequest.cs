using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchUpdateValuesByDataFilterRequest : IDirectResponseSchema
	{
		[JsonProperty("data")]
		public virtual IList<DataFilterValueRange> Data { get; set; }

		[JsonProperty("includeValuesInResponse")]
		public virtual bool? IncludeValuesInResponse { get; set; }

		[JsonProperty("responseDateTimeRenderOption")]
		public virtual string ResponseDateTimeRenderOption { get; set; }

		[JsonProperty("responseValueRenderOption")]
		public virtual string ResponseValueRenderOption { get; set; }

		[JsonProperty("valueInputOption")]
		public virtual string ValueInputOption { get; set; }

		public virtual string ETag { get; set; }
	}
}
