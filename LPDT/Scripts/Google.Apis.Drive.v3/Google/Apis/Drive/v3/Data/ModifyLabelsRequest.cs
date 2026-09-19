using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class ModifyLabelsRequest : IDirectResponseSchema
	{
		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("labelModifications")]
		public virtual IList<LabelModification> LabelModifications { get; set; }

		public virtual string ETag { get; set; }
	}
}
