using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class ModifyLabelsResponse : IDirectResponseSchema
	{
		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("modifiedLabels")]
		public virtual IList<Label> ModifiedLabels { get; set; }

		public virtual string ETag { get; set; }
	}
}
