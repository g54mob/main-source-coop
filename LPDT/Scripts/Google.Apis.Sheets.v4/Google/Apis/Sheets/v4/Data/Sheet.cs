using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Sheet : IDirectResponseSchema
	{
		[JsonProperty("bandedRanges")]
		public virtual IList<BandedRange> BandedRanges { get; set; }

		[JsonProperty("basicFilter")]
		public virtual BasicFilter BasicFilter { get; set; }

		[JsonProperty("charts")]
		public virtual IList<EmbeddedChart> Charts { get; set; }

		[JsonProperty("columnGroups")]
		public virtual IList<DimensionGroup> ColumnGroups { get; set; }

		[JsonProperty("conditionalFormats")]
		public virtual IList<ConditionalFormatRule> ConditionalFormats { get; set; }

		[JsonProperty("data")]
		public virtual IList<GridData> Data { get; set; }

		[JsonProperty("developerMetadata")]
		public virtual IList<DeveloperMetadata> DeveloperMetadata { get; set; }

		[JsonProperty("filterViews")]
		public virtual IList<FilterView> FilterViews { get; set; }

		[JsonProperty("merges")]
		public virtual IList<GridRange> Merges { get; set; }

		[JsonProperty("properties")]
		public virtual SheetProperties Properties { get; set; }

		[JsonProperty("protectedRanges")]
		public virtual IList<ProtectedRange> ProtectedRanges { get; set; }

		[JsonProperty("rowGroups")]
		public virtual IList<DimensionGroup> RowGroups { get; set; }

		[JsonProperty("slicers")]
		public virtual IList<Slicer> Slicers { get; set; }

		public virtual string ETag { get; set; }
	}
}
