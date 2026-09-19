using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class GeneratedIds : IDirectResponseSchema
	{
		[JsonProperty("ids")]
		public virtual IList<string> Ids { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("space")]
		public virtual string Space { get; set; }

		public virtual string ETag { get; set; }
	}
}
