using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataFilterValueRange : IDirectResponseSchema
	{
		[JsonProperty("dataFilter")]
		public virtual DataFilter DataFilter { get; set; }

		[JsonProperty("majorDimension")]
		public virtual string MajorDimension { get; set; }

		[JsonProperty("values")]
		public virtual IList<IList<object>> Values { get; set; }

		public virtual string ETag { get; set; }
	}
}
