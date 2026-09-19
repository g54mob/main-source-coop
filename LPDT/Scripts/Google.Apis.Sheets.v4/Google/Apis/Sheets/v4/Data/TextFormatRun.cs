using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TextFormatRun : IDirectResponseSchema
	{
		[JsonProperty("format")]
		public virtual TextFormat Format { get; set; }

		[JsonProperty("startIndex")]
		public virtual int? StartIndex { get; set; }

		public virtual string ETag { get; set; }
	}
}
