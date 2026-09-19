using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ExtendedValue : IDirectResponseSchema
	{
		[JsonProperty("boolValue")]
		public virtual bool? BoolValue { get; set; }

		[JsonProperty("errorValue")]
		public virtual ErrorValue ErrorValue { get; set; }

		[JsonProperty("formulaValue")]
		public virtual string FormulaValue { get; set; }

		[JsonProperty("numberValue")]
		public virtual double? NumberValue { get; set; }

		[JsonProperty("stringValue")]
		public virtual string StringValue { get; set; }

		public virtual string ETag { get; set; }
	}
}
