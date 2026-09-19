using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ProtectedRange : IDirectResponseSchema
	{
		[JsonProperty("description")]
		public virtual string Description { get; set; }

		[JsonProperty("editors")]
		public virtual Editors Editors { get; set; }

		[JsonProperty("namedRangeId")]
		public virtual string NamedRangeId { get; set; }

		[JsonProperty("protectedRangeId")]
		public virtual int? ProtectedRangeId { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("requestingUserCanEdit")]
		public virtual bool? RequestingUserCanEdit { get; set; }

		[JsonProperty("unprotectedRanges")]
		public virtual IList<GridRange> UnprotectedRanges { get; set; }

		[JsonProperty("warningOnly")]
		public virtual bool? WarningOnly { get; set; }

		public virtual string ETag { get; set; }
	}
}
