using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class LabelField : IDirectResponseSchema
	{
		[JsonProperty("dateString")]
		public virtual IList<string> DateString { get; set; }

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("integer")]
		public virtual IList<long?> Integer { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("selection")]
		public virtual IList<string> Selection { get; set; }

		[JsonProperty("text")]
		public virtual IList<string> Text { get; set; }

		[JsonProperty("user")]
		public virtual IList<User> User { get; set; }

		[JsonProperty("valueType")]
		public virtual string ValueType { get; set; }

		public virtual string ETag { get; set; }
	}
}
