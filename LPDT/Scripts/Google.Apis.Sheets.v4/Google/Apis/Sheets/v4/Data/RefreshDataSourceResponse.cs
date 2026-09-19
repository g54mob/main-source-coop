using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class RefreshDataSourceResponse : IDirectResponseSchema
	{
		[JsonProperty("statuses")]
		public virtual IList<RefreshDataSourceObjectExecutionStatus> Statuses { get; set; }

		public virtual string ETag { get; set; }
	}
}
