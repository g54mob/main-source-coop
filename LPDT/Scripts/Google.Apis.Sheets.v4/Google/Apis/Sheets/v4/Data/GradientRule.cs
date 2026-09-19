using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class GradientRule : IDirectResponseSchema
	{
		[JsonProperty("maxpoint")]
		public virtual InterpolationPoint Maxpoint { get; set; }

		[JsonProperty("midpoint")]
		public virtual InterpolationPoint Midpoint { get; set; }

		[JsonProperty("minpoint")]
		public virtual InterpolationPoint Minpoint { get; set; }

		public virtual string ETag { get; set; }
	}
}
