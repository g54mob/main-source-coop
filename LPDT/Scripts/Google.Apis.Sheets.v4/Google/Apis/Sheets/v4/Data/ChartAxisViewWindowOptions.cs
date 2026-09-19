using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ChartAxisViewWindowOptions : IDirectResponseSchema
	{
		[JsonProperty("viewWindowMax")]
		public virtual double? ViewWindowMax { get; set; }

		[JsonProperty("viewWindowMin")]
		public virtual double? ViewWindowMin { get; set; }

		[JsonProperty("viewWindowMode")]
		public virtual string ViewWindowMode { get; set; }

		public virtual string ETag { get; set; }
	}
}
