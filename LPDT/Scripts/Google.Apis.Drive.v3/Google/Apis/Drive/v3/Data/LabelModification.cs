using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class LabelModification : IDirectResponseSchema
	{
		[JsonProperty("fieldModifications")]
		public virtual IList<LabelFieldModification> FieldModifications { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("labelId")]
		public virtual string LabelId { get; set; }

		[JsonProperty("removeLabel")]
		public virtual bool? RemoveLabel { get; set; }

		public virtual string ETag { get; set; }
	}
}
