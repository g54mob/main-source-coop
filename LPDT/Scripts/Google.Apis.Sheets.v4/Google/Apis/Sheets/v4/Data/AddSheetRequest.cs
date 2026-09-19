using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddSheetRequest : IDirectResponseSchema
	{
		[JsonProperty("properties")]
		public virtual SheetProperties Properties { get; set; }

		public virtual string ETag { get; set; }
	}
}
