using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ValueRange : IDirectResponseSchema
	{
		[JsonProperty("majorDimension")]
		public virtual string MajorDimension { get; set; }

		[JsonProperty("range")]
		public virtual string Range { get; set; }

		[JsonProperty("values")]
		public virtual IList<IList<object>> Values { get; set; }

		public virtual string ETag { get; set; }
	}
}
