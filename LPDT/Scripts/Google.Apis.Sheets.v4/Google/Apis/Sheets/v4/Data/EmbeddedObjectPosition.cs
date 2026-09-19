using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class EmbeddedObjectPosition : IDirectResponseSchema
	{
		[JsonProperty("newSheet")]
		public virtual bool? NewSheet { get; set; }

		[JsonProperty("overlayPosition")]
		public virtual OverlayPosition OverlayPosition { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
