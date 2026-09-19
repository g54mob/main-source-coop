using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TextRotation : IDirectResponseSchema
	{
		[JsonProperty("angle")]
		public virtual int? Angle { get; set; }

		[JsonProperty("vertical")]
		public virtual bool? Vertical { get; set; }

		public virtual string ETag { get; set; }
	}
}
