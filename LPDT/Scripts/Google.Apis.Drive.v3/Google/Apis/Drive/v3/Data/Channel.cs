using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class Channel : IDirectResponseSchema
	{
		[JsonProperty("address")]
		public virtual string Address { get; set; }

		[JsonProperty("expiration")]
		public virtual long? Expiration { get; set; }

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("params")]
		public virtual IDictionary<string, string> Params__ { get; set; }

		[JsonProperty("payload")]
		public virtual bool? Payload { get; set; }

		[JsonProperty("resourceId")]
		public virtual string ResourceId { get; set; }

		[JsonProperty("resourceUri")]
		public virtual string ResourceUri { get; set; }

		[JsonProperty("token")]
		public virtual string Token { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
