using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ChartCustomNumberFormatOptions : IDirectResponseSchema
	{
		[JsonProperty("prefix")]
		public virtual string Prefix { get; set; }

		[JsonProperty("suffix")]
		public virtual string Suffix { get; set; }

		public virtual string ETag { get; set; }
	}
}
