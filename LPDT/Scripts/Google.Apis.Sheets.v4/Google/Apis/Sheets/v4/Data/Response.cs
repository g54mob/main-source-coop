using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Response : IDirectResponseSchema
	{
		[JsonProperty("addBanding")]
		public virtual AddBandingResponse AddBanding { get; set; }

		[JsonProperty("addChart")]
		public virtual AddChartResponse AddChart { get; set; }

		[JsonProperty("addDataSource")]
		public virtual AddDataSourceResponse AddDataSource { get; set; }

		[JsonProperty("addDimensionGroup")]
		public virtual AddDimensionGroupResponse AddDimensionGroup { get; set; }

		[JsonProperty("addFilterView")]
		public virtual AddFilterViewResponse AddFilterView { get; set; }

		[JsonProperty("addNamedRange")]
		public virtual AddNamedRangeResponse AddNamedRange { get; set; }

		[JsonProperty("addProtectedRange")]
		public virtual AddProtectedRangeResponse AddProtectedRange { get; set; }

		[JsonProperty("addSheet")]
		public virtual AddSheetResponse AddSheet { get; set; }

		[JsonProperty("addSlicer")]
		public virtual AddSlicerResponse AddSlicer { get; set; }

		[JsonProperty("createDeveloperMetadata")]
		public virtual CreateDeveloperMetadataResponse CreateDeveloperMetadata { get; set; }

		[JsonProperty("deleteConditionalFormatRule")]
		public virtual DeleteConditionalFormatRuleResponse DeleteConditionalFormatRule { get; set; }

		[JsonProperty("deleteDeveloperMetadata")]
		public virtual DeleteDeveloperMetadataResponse DeleteDeveloperMetadata { get; set; }

		[JsonProperty("deleteDimensionGroup")]
		public virtual DeleteDimensionGroupResponse DeleteDimensionGroup { get; set; }

		[JsonProperty("deleteDuplicates")]
		public virtual DeleteDuplicatesResponse DeleteDuplicates { get; set; }

		[JsonProperty("duplicateFilterView")]
		public virtual DuplicateFilterViewResponse DuplicateFilterView { get; set; }

		[JsonProperty("duplicateSheet")]
		public virtual DuplicateSheetResponse DuplicateSheet { get; set; }

		[JsonProperty("findReplace")]
		public virtual FindReplaceResponse FindReplace { get; set; }

		[JsonProperty("refreshDataSource")]
		public virtual RefreshDataSourceResponse RefreshDataSource { get; set; }

		[JsonProperty("trimWhitespace")]
		public virtual TrimWhitespaceResponse TrimWhitespace { get; set; }

		[JsonProperty("updateConditionalFormatRule")]
		public virtual UpdateConditionalFormatRuleResponse UpdateConditionalFormatRule { get; set; }

		[JsonProperty("updateDataSource")]
		public virtual UpdateDataSourceResponse UpdateDataSource { get; set; }

		[JsonProperty("updateDeveloperMetadata")]
		public virtual UpdateDeveloperMetadataResponse UpdateDeveloperMetadata { get; set; }

		[JsonProperty("updateEmbeddedObjectPosition")]
		public virtual UpdateEmbeddedObjectPositionResponse UpdateEmbeddedObjectPosition { get; set; }

		public virtual string ETag { get; set; }
	}
}
