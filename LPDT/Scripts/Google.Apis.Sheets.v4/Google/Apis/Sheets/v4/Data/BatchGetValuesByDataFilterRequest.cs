using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchGetValuesByDataFilterRequest : IDirectResponseSchema
	{
		[JsonProperty("dataFilters")]
		public virtual IList<DataFilter> DataFilters { get; set; }

		[JsonProperty("dateTimeRenderOption")]
		public virtual string DateTimeRenderOption { get; set; }

		[JsonProperty("majorDimension")]
		public virtual string MajorDimension { get; set; }

		[JsonProperty("valueRenderOption")]
		public virtual string ValueRenderOption { get; set; }

		public virtual string ETag { get; set; }
	}
}
