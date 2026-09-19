using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AppendDimensionRequest : IDirectResponseSchema
	{
		[JsonProperty("dimension")]
		public virtual string Dimension { get; set; }

		[JsonProperty("length")]
		public virtual int? Length { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
