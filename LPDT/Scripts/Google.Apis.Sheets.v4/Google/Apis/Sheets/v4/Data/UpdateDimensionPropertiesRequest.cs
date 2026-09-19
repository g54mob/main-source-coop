using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateDimensionPropertiesRequest : IDirectResponseSchema
	{
		[JsonProperty("dataSourceSheetRange")]
		public virtual DataSourceSheetDimensionRange DataSourceSheetRange { get; set; }

		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		[JsonProperty("properties")]
		public virtual DimensionProperties Properties { get; set; }

		[JsonProperty("range")]
		public virtual DimensionRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
