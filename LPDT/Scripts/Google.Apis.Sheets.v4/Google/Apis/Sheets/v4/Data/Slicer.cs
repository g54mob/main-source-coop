using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Slicer : IDirectResponseSchema
	{
		[JsonProperty("position")]
		public virtual EmbeddedObjectPosition Position { get; set; }

		[JsonProperty("slicerId")]
		public virtual int? SlicerId { get; set; }

		[JsonProperty("spec")]
		public virtual SlicerSpec Spec { get; set; }

		public virtual string ETag { get; set; }
	}
}
