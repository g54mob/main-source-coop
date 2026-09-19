using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DimensionProperties : IDirectResponseSchema
	{
		[JsonProperty("dataSourceColumnReference")]
		public virtual DataSourceColumnReference DataSourceColumnReference { get; set; }

		[JsonProperty("developerMetadata")]
		public virtual IList<DeveloperMetadata> DeveloperMetadata { get; set; }

		[JsonProperty("hiddenByFilter")]
		public virtual bool? HiddenByFilter { get; set; }

		[JsonProperty("hiddenByUser")]
		public virtual bool? HiddenByUser { get; set; }

		[JsonProperty("pixelSize")]
		public virtual int? PixelSize { get; set; }

		public virtual string ETag { get; set; }
	}
}
