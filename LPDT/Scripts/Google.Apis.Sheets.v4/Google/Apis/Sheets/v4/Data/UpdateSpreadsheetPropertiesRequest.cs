using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateSpreadsheetPropertiesRequest : IDirectResponseSchema
	{
		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		[JsonProperty("properties")]
		public virtual SpreadsheetProperties Properties { get; set; }

		public virtual string ETag { get; set; }
	}
}
