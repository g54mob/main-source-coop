using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TextPosition : IDirectResponseSchema
	{
		[JsonProperty("horizontalAlignment")]
		public virtual string HorizontalAlignment { get; set; }

		public virtual string ETag { get; set; }
	}
}
