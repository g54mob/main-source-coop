using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CopyPasteRequest : IDirectResponseSchema
	{
		[JsonProperty("destination")]
		public virtual GridRange Destination { get; set; }

		[JsonProperty("pasteOrientation")]
		public virtual string PasteOrientation { get; set; }

		[JsonProperty("pasteType")]
		public virtual string PasteType { get; set; }

		[JsonProperty("source")]
		public virtual GridRange Source { get; set; }

		public virtual string ETag { get; set; }
	}
}
