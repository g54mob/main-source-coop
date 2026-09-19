using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeveloperMetadataLocation : IDirectResponseSchema
	{
		[JsonProperty("dimensionRange")]
		public virtual DimensionRange DimensionRange { get; set; }

		[JsonProperty("locationType")]
		public virtual string LocationType { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		[JsonProperty("spreadsheet")]
		public virtual bool? Spreadsheet { get; set; }

		public virtual string ETag { get; set; }
	}
}
