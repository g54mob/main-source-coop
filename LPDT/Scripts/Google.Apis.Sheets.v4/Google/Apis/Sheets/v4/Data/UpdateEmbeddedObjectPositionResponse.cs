using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateEmbeddedObjectPositionResponse : IDirectResponseSchema
	{
		[JsonProperty("position")]
		public virtual EmbeddedObjectPosition Position { get; set; }

		public virtual string ETag { get; set; }
	}
}
