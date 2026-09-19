using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotGroupSortValueBucket : IDirectResponseSchema
	{
		[JsonProperty("buckets")]
		public virtual IList<ExtendedValue> Buckets { get; set; }

		[JsonProperty("valuesIndex")]
		public virtual int? ValuesIndex { get; set; }

		public virtual string ETag { get; set; }
	}
}
