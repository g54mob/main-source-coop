using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class RepeatCellRequest : IDirectResponseSchema
	{
		[JsonProperty("cell")]
		public virtual CellData Cell { get; set; }

		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
