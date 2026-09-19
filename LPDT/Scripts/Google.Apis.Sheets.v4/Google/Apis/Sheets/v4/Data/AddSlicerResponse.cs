using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddSlicerResponse : IDirectResponseSchema
	{
		[JsonProperty("slicer")]
		public virtual Slicer Slicer { get; set; }

		public virtual string ETag { get; set; }
	}
}
