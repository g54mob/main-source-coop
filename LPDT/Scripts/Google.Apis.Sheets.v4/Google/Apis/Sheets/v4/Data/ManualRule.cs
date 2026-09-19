using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ManualRule : IDirectResponseSchema
	{
		[JsonProperty("groups")]
		public virtual IList<ManualRuleGroup> Groups { get; set; }

		public virtual string ETag { get; set; }
	}
}
