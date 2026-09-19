using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class IterativeCalculationSettings : IDirectResponseSchema
	{
		[JsonProperty("convergenceThreshold")]
		public virtual double? ConvergenceThreshold { get; set; }

		[JsonProperty("maxIterations")]
		public virtual int? MaxIterations { get; set; }

		public virtual string ETag { get; set; }
	}
}
