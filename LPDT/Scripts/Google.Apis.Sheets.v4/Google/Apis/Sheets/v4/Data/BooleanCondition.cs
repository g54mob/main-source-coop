using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BooleanCondition : IDirectResponseSchema
	{
		[JsonProperty("type")]
		public virtual string Type { get; set; }

		[JsonProperty("values")]
		public virtual IList<ConditionValue> Values { get; set; }

		public virtual string ETag { get; set; }
	}
}
