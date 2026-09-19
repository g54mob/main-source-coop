using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ConditionalFormatRule : IDirectResponseSchema
	{
		[JsonProperty("booleanRule")]
		public virtual BooleanRule BooleanRule { get; set; }

		[JsonProperty("gradientRule")]
		public virtual GradientRule GradientRule { get; set; }

		[JsonProperty("ranges")]
		public virtual IList<GridRange> Ranges { get; set; }

		public virtual string ETag { get; set; }
	}
}
