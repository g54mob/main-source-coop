using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ManualRuleGroup : IDirectResponseSchema
	{
		[JsonProperty("groupName")]
		public virtual ExtendedValue GroupName { get; set; }

		[JsonProperty("items")]
		public virtual IList<ExtendedValue> Items { get; set; }

		public virtual string ETag { get; set; }
	}
}
