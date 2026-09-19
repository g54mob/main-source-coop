using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteRangeRequest : IDirectResponseSchema
	{
		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("shiftDimension")]
		public virtual string ShiftDimension { get; set; }

		public virtual string ETag { get; set; }
	}
}
