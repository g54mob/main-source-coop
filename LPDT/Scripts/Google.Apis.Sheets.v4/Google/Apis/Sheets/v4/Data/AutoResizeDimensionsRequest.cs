using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AutoResizeDimensionsRequest : IDirectResponseSchema
	{
		[JsonProperty("dataSourceSheetDimensions")]
		public virtual DataSourceSheetDimensionRange DataSourceSheetDimensions { get; set; }

		[JsonProperty("dimensions")]
		public virtual DimensionRange Dimensions { get; set; }

		public virtual string ETag { get; set; }
	}
}
