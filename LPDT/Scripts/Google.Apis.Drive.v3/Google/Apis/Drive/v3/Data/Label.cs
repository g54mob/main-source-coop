using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class Label : IDirectResponseSchema
	{
		[JsonProperty("fields")]
		public virtual IDictionary<string, LabelField> Fields { get; set; }

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("revisionId")]
		public virtual string RevisionId { get; set; }

		public virtual string ETag { get; set; }
	}
}
