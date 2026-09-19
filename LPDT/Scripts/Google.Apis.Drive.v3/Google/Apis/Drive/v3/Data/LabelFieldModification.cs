using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class LabelFieldModification : IDirectResponseSchema
	{
		[JsonProperty("fieldId")]
		public virtual string FieldId { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("setDateValues")]
		public virtual IList<string> SetDateValues { get; set; }

		[JsonProperty("setIntegerValues")]
		public virtual IList<long?> SetIntegerValues { get; set; }

		[JsonProperty("setSelectionValues")]
		public virtual IList<string> SetSelectionValues { get; set; }

		[JsonProperty("setTextValues")]
		public virtual IList<string> SetTextValues { get; set; }

		[JsonProperty("setUserValues")]
		public virtual IList<string> SetUserValues { get; set; }

		[JsonProperty("unsetValues")]
		public virtual bool? UnsetValues { get; set; }

		public virtual string ETag { get; set; }
	}
}
