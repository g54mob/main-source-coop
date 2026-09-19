using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddDimensionGroupResponse : IDirectResponseSchema
	{
		[JsonProperty("dimensionGroups")]
		public virtual IList<DimensionGroup> DimensionGroups { get; set; }

		public virtual string ETag { get; set; }
	}
}
