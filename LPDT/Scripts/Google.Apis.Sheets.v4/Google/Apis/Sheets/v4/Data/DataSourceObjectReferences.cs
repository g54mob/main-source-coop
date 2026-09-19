using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceObjectReferences : IDirectResponseSchema
	{
		[JsonProperty("references")]
		public virtual IList<DataSourceObjectReference> References { get; set; }

		public virtual string ETag { get; set; }
	}
}
