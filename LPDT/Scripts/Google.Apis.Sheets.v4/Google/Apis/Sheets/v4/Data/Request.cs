using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Request : IDirectResponseSchema
	{
		[JsonProperty("addBanding")]
		public virtual AddBandingRequest AddBanding { get; set; }

		[JsonProperty("addChart")]
		public virtual AddChartRequest AddChart { get; set; }

		[JsonProperty("addConditionalFormatRule")]
		public virtual AddConditionalFormatRuleRequest AddConditionalFormatRule { get; set; }

		[JsonProperty("addDataSource")]
		public virtual AddDataSourceRequest AddDataSource { get; set; }

		[JsonProperty("addDimensionGroup")]
		public virtual AddDimensionGroupRequest AddDimensionGroup { get; set; }

		[JsonProperty("addFilterView")]
		public virtual AddFilterViewRequest AddFilterView { get; set; }

		[JsonProperty("addNamedRange")]
		public virtual AddNamedRangeRequest AddNamedRange { get; set; }

		[JsonProperty("addProtectedRange")]
		public virtual AddProtectedRangeRequest AddProtectedRange { get; set; }

		[JsonProperty("addSheet")]
		public virtual AddSheetRequest AddSheet { get; set; }

		[JsonProperty("addSlicer")]
		public virtual AddSlicerRequest AddSlicer { get; set; }

		[JsonProperty("appendCells")]
		public virtual AppendCellsRequest AppendCells { get; set; }

		[JsonProperty("appendDimension")]
		public virtual AppendDimensionRequest AppendDimension { get; set; }

		[JsonProperty("autoFill")]
		public virtual AutoFillRequest AutoFill { get; set; }

		[JsonProperty("autoResizeDimensions")]
		public virtual AutoResizeDimensionsRequest AutoResizeDimensions { get; set; }

		[JsonProperty("clearBasicFilter")]
		public virtual ClearBasicFilterRequest ClearBasicFilter { get; set; }

		[JsonProperty("copyPaste")]
		public virtual CopyPasteRequest CopyPaste { get; set; }

		[JsonProperty("createDeveloperMetadata")]
		public virtual CreateDeveloperMetadataRequest CreateDeveloperMetadata { get; set; }

		[JsonProperty("cutPaste")]
		public virtual CutPasteRequest CutPaste { get; set; }

		[JsonProperty("deleteBanding")]
		public virtual DeleteBandingRequest DeleteBanding { get; set; }

		[JsonProperty("deleteConditionalFormatRule")]
		public virtual DeleteConditionalFormatRuleRequest DeleteConditionalFormatRule { get; set; }

		[JsonProperty("deleteDataSource")]
		public virtual DeleteDataSourceRequest DeleteDataSource { get; set; }

		[JsonProperty("deleteDeveloperMetadata")]
		public virtual DeleteDeveloperMetadataRequest DeleteDeveloperMetadata { get; set; }

		[JsonProperty("deleteDimension")]
		public virtual DeleteDimensionRequest DeleteDimension { get; set; }

		[JsonProperty("deleteDimensionGroup")]
		public virtual DeleteDimensionGroupRequest DeleteDimensionGroup { get; set; }

		[JsonProperty("deleteDuplicates")]
		public virtual DeleteDuplicatesRequest DeleteDuplicates { get; set; }

		[JsonProperty("deleteEmbeddedObject")]
		public virtual DeleteEmbeddedObjectRequest DeleteEmbeddedObject { get; set; }

		[JsonProperty("deleteFilterView")]
		public virtual DeleteFilterViewRequest DeleteFilterView { get; set; }

		[JsonProperty("deleteNamedRange")]
		public virtual DeleteNamedRangeRequest DeleteNamedRange { get; set; }

		[JsonProperty("deleteProtectedRange")]
		public virtual DeleteProtectedRangeRequest DeleteProtectedRange { get; set; }

		[JsonProperty("deleteRange")]
		public virtual DeleteRangeRequest DeleteRange { get; set; }

		[JsonProperty("deleteSheet")]
		public virtual DeleteSheetRequest DeleteSheet { get; set; }

		[JsonProperty("duplicateFilterView")]
		public virtual DuplicateFilterViewRequest DuplicateFilterView { get; set; }

		[JsonProperty("duplicateSheet")]
		public virtual DuplicateSheetRequest DuplicateSheet { get; set; }

		[JsonProperty("findReplace")]
		public virtual FindReplaceRequest FindReplace { get; set; }

		[JsonProperty("insertDimension")]
		public virtual InsertDimensionRequest InsertDimension { get; set; }

		[JsonProperty("insertRange")]
		public virtual InsertRangeRequest InsertRange { get; set; }

		[JsonProperty("mergeCells")]
		public virtual MergeCellsRequest MergeCells { get; set; }

		[JsonProperty("moveDimension")]
		public virtual MoveDimensionRequest MoveDimension { get; set; }

		[JsonProperty("pasteData")]
		public virtual PasteDataRequest PasteData { get; set; }

		[JsonProperty("randomizeRange")]
		public virtual RandomizeRangeRequest RandomizeRange { get; set; }

		[JsonProperty("refreshDataSource")]
		public virtual RefreshDataSourceRequest RefreshDataSource { get; set; }

		[JsonProperty("repeatCell")]
		public virtual RepeatCellRequest RepeatCell { get; set; }

		[JsonProperty("setBasicFilter")]
		public virtual SetBasicFilterRequest SetBasicFilter { get; set; }

		[JsonProperty("setDataValidation")]
		public virtual SetDataValidationRequest SetDataValidation { get; set; }

		[JsonProperty("sortRange")]
		public virtual SortRangeRequest SortRange { get; set; }

		[JsonProperty("textToColumns")]
		public virtual TextToColumnsRequest TextToColumns { get; set; }

		[JsonProperty("trimWhitespace")]
		public virtual TrimWhitespaceRequest TrimWhitespace { get; set; }

		[JsonProperty("unmergeCells")]
		public virtual UnmergeCellsRequest UnmergeCells { get; set; }

		[JsonProperty("updateBanding")]
		public virtual UpdateBandingRequest UpdateBanding { get; set; }

		[JsonProperty("updateBorders")]
		public virtual UpdateBordersRequest UpdateBorders { get; set; }

		[JsonProperty("updateCells")]
		public virtual UpdateCellsRequest UpdateCells { get; set; }

		[JsonProperty("updateChartSpec")]
		public virtual UpdateChartSpecRequest UpdateChartSpec { get; set; }

		[JsonProperty("updateConditionalFormatRule")]
		public virtual UpdateConditionalFormatRuleRequest UpdateConditionalFormatRule { get; set; }

		[JsonProperty("updateDataSource")]
		public virtual UpdateDataSourceRequest UpdateDataSource { get; set; }

		[JsonProperty("updateDeveloperMetadata")]
		public virtual UpdateDeveloperMetadataRequest UpdateDeveloperMetadata { get; set; }

		[JsonProperty("updateDimensionGroup")]
		public virtual UpdateDimensionGroupRequest UpdateDimensionGroup { get; set; }

		[JsonProperty("updateDimensionProperties")]
		public virtual UpdateDimensionPropertiesRequest UpdateDimensionProperties { get; set; }

		[JsonProperty("updateEmbeddedObjectBorder")]
		public virtual UpdateEmbeddedObjectBorderRequest UpdateEmbeddedObjectBorder { get; set; }

		[JsonProperty("updateEmbeddedObjectPosition")]
		public virtual UpdateEmbeddedObjectPositionRequest UpdateEmbeddedObjectPosition { get; set; }

		[JsonProperty("updateFilterView")]
		public virtual UpdateFilterViewRequest UpdateFilterView { get; set; }

		[JsonProperty("updateNamedRange")]
		public virtual UpdateNamedRangeRequest UpdateNamedRange { get; set; }

		[JsonProperty("updateProtectedRange")]
		public virtual UpdateProtectedRangeRequest UpdateProtectedRange { get; set; }

		[JsonProperty("updateSheetProperties")]
		public virtual UpdateSheetPropertiesRequest UpdateSheetProperties { get; set; }

		[JsonProperty("updateSlicerSpec")]
		public virtual UpdateSlicerSpecRequest UpdateSlicerSpec { get; set; }

		[JsonProperty("updateSpreadsheetProperties")]
		public virtual UpdateSpreadsheetPropertiesRequest UpdateSpreadsheetProperties { get; set; }

		public virtual string ETag { get; set; }
	}
}
