using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BigQueryQuerySpec : IDirectResponseSchema
	{
		[JsonProperty("rawQuery")]
		public virtual string RawQuery { get; set; }

		public virtual string ETag { get; set; }
	}
}
