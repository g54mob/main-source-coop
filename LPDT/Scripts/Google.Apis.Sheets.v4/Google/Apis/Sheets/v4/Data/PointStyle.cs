using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PointStyle : IDirectResponseSchema
	{
		[JsonProperty("shape")]
		public virtual string Shape { get; set; }

		[JsonProperty("size")]
		public virtual double? Size { get; set; }

		public virtual string ETag { get; set; }
	}
}
