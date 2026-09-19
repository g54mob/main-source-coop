using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class KeyValueFormat : IDirectResponseSchema
	{
		[JsonProperty("position")]
		public virtual TextPosition Position { get; set; }

		[JsonProperty("textFormat")]
		public virtual TextFormat TextFormat { get; set; }

		public virtual string ETag { get; set; }
	}
}
