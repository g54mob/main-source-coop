using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CopySheetToAnotherSpreadsheetRequest : IDirectResponseSchema
	{
		[JsonProperty("destinationSpreadsheetId")]
		public virtual string DestinationSpreadsheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
