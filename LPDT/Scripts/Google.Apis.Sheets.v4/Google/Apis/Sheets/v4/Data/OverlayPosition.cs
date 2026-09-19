using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class OverlayPosition : IDirectResponseSchema
	{
		[JsonProperty("anchorCell")]
		public virtual GridCoordinate AnchorCell { get; set; }

		[JsonProperty("heightPixels")]
		public virtual int? HeightPixels { get; set; }

		[JsonProperty("offsetXPixels")]
		public virtual int? OffsetXPixels { get; set; }

		[JsonProperty("offsetYPixels")]
		public virtual int? OffsetYPixels { get; set; }

		[JsonProperty("widthPixels")]
		public virtual int? WidthPixels { get; set; }

		public virtual string ETag { get; set; }
	}
}
