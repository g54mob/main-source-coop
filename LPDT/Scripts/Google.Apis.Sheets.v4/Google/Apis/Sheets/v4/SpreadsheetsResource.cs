using Google.Apis.Discovery;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util;

namespace Google.Apis.Sheets.v4
{
	public class SpreadsheetsResource
	{
		public class DeveloperMetadataResource
		{
			public class GetRequest : SheetsBaseServiceRequest<DeveloperMetadata>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				[RequestParameter("metadataId", RequestParameterType.Path)]
				public virtual int MetadataId { get; private set; }

				public override string MethodName => "get";

				public override string HttpMethod => "GET";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/developerMetadata/{metadataId}";

				public GetRequest(IClientService service, string spreadsheetId, int metadataId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					MetadataId = metadataId;
					InitParameters();
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("metadataId", new Parameter
					{
						Name = "metadataId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class SearchRequest : SheetsBaseServiceRequest<SearchDeveloperMetadataResponse>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				private SearchDeveloperMetadataRequest Body { get; set; }

				public override string MethodName => "search";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/developerMetadata:search";

				public SearchRequest(IClientService service, SearchDeveloperMetadataRequest body, string spreadsheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			private const string Resource = "developerMetadata";

			private readonly IClientService service;

			public DeveloperMetadataResource(IClientService service)
			{
				this.service = service;
			}

			public virtual GetRequest Get(string spreadsheetId, int metadataId)
			{
				return new GetRequest(service, spreadsheetId, metadataId);
			}

			public virtual SearchRequest Search(SearchDeveloperMetadataRequest body, string spreadsheetId)
			{
				return new SearchRequest(service, body, spreadsheetId);
			}
		}

		public class SheetsResource
		{
			public class CopyToRequest : SheetsBaseServiceRequest<SheetProperties>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				[RequestParameter("sheetId", RequestParameterType.Path)]
				public virtual int SheetId { get; private set; }

				private CopySheetToAnotherSpreadsheetRequest Body { get; set; }

				public override string MethodName => "copyTo";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/sheets/{sheetId}:copyTo";

				public CopyToRequest(IClientService service, CopySheetToAnotherSpreadsheetRequest body, string spreadsheetId, int sheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					SheetId = sheetId;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("sheetId", new Parameter
					{
						Name = "sheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			private const string Resource = "sheets";

			private readonly IClientService service;

			public SheetsResource(IClientService service)
			{
				this.service = service;
			}

			public virtual CopyToRequest CopyTo(CopySheetToAnotherSpreadsheetRequest body, string spreadsheetId, int sheetId)
			{
				return new CopyToRequest(service, body, spreadsheetId, sheetId);
			}
		}

		public class ValuesResource
		{
			public class AppendRequest : SheetsBaseServiceRequest<AppendValuesResponse>
			{
				public enum InsertDataOptionEnum
				{
					[StringValue("OVERWRITE")]
					OVERWRITE = 0,
					[StringValue("INSERT_ROWS")]
					INSERTROWS = 1
				}

				public enum ResponseDateTimeRenderOptionEnum
				{
					[StringValue("SERIAL_NUMBER")]
					SERIALNUMBER = 0,
					[StringValue("FORMATTED_STRING")]
					FORMATTEDSTRING = 1
				}

				public enum ResponseValueRenderOptionEnum
				{
					[StringValue("FORMATTED_VALUE")]
					FORMATTEDVALUE = 0,
					[StringValue("UNFORMATTED_VALUE")]
					UNFORMATTEDVALUE = 1,
					[StringValue("FORMULA")]
					FORMULA = 2
				}

				public enum ValueInputOptionEnum
				{
					[StringValue("INPUT_VALUE_OPTION_UNSPECIFIED")]
					INPUTVALUEOPTIONUNSPECIFIED = 0,
					[StringValue("RAW")]
					RAW = 1,
					[StringValue("USER_ENTERED")]
					USERENTERED = 2
				}

				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				[RequestParameter("range", RequestParameterType.Path)]
				public virtual string Range { get; private set; }

				[RequestParameter("includeValuesInResponse", RequestParameterType.Query)]
				public virtual bool? IncludeValuesInResponse { get; set; }

				[RequestParameter("insertDataOption", RequestParameterType.Query)]
				public virtual InsertDataOptionEnum? InsertDataOption { get; set; }

				[RequestParameter("responseDateTimeRenderOption", RequestParameterType.Query)]
				public virtual ResponseDateTimeRenderOptionEnum? ResponseDateTimeRenderOption { get; set; }

				[RequestParameter("responseValueRenderOption", RequestParameterType.Query)]
				public virtual ResponseValueRenderOptionEnum? ResponseValueRenderOption { get; set; }

				[RequestParameter("valueInputOption", RequestParameterType.Query)]
				public virtual ValueInputOptionEnum? ValueInputOption { get; set; }

				private ValueRange Body { get; set; }

				public override string MethodName => "append";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values/{range}:append";

				public AppendRequest(IClientService service, ValueRange body, string spreadsheetId, string range)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Range = range;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("range", new Parameter
					{
						Name = "range",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("includeValuesInResponse", new Parameter
					{
						Name = "includeValuesInResponse",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("insertDataOption", new Parameter
					{
						Name = "insertDataOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("responseDateTimeRenderOption", new Parameter
					{
						Name = "responseDateTimeRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("responseValueRenderOption", new Parameter
					{
						Name = "responseValueRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("valueInputOption", new Parameter
					{
						Name = "valueInputOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class BatchClearRequest : SheetsBaseServiceRequest<BatchClearValuesResponse>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				private BatchClearValuesRequest Body { get; set; }

				public override string MethodName => "batchClear";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values:batchClear";

				public BatchClearRequest(IClientService service, BatchClearValuesRequest body, string spreadsheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class BatchClearByDataFilterRequest : SheetsBaseServiceRequest<BatchClearValuesByDataFilterResponse>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				private BatchClearValuesByDataFilterRequest Body { get; set; }

				public override string MethodName => "batchClearByDataFilter";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values:batchClearByDataFilter";

				public BatchClearByDataFilterRequest(IClientService service, BatchClearValuesByDataFilterRequest body, string spreadsheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class BatchGetRequest : SheetsBaseServiceRequest<BatchGetValuesResponse>
			{
				public enum DateTimeRenderOptionEnum
				{
					[StringValue("SERIAL_NUMBER")]
					SERIALNUMBER = 0,
					[StringValue("FORMATTED_STRING")]
					FORMATTEDSTRING = 1
				}

				public enum MajorDimensionEnum
				{
					[StringValue("DIMENSION_UNSPECIFIED")]
					DIMENSIONUNSPECIFIED = 0,
					[StringValue("ROWS")]
					ROWS = 1,
					[StringValue("COLUMNS")]
					COLUMNS = 2
				}

				public enum ValueRenderOptionEnum
				{
					[StringValue("FORMATTED_VALUE")]
					FORMATTEDVALUE = 0,
					[StringValue("UNFORMATTED_VALUE")]
					UNFORMATTEDVALUE = 1,
					[StringValue("FORMULA")]
					FORMULA = 2
				}

				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				[RequestParameter("dateTimeRenderOption", RequestParameterType.Query)]
				public virtual DateTimeRenderOptionEnum? DateTimeRenderOption { get; set; }

				[RequestParameter("majorDimension", RequestParameterType.Query)]
				public virtual MajorDimensionEnum? MajorDimension { get; set; }

				[RequestParameter("ranges", RequestParameterType.Query)]
				public virtual Repeatable<string> Ranges { get; set; }

				[RequestParameter("valueRenderOption", RequestParameterType.Query)]
				public virtual ValueRenderOptionEnum? ValueRenderOption { get; set; }

				public override string MethodName => "batchGet";

				public override string HttpMethod => "GET";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values:batchGet";

				public BatchGetRequest(IClientService service, string spreadsheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					InitParameters();
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("dateTimeRenderOption", new Parameter
					{
						Name = "dateTimeRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("majorDimension", new Parameter
					{
						Name = "majorDimension",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("ranges", new Parameter
					{
						Name = "ranges",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("valueRenderOption", new Parameter
					{
						Name = "valueRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class BatchGetByDataFilterRequest : SheetsBaseServiceRequest<BatchGetValuesByDataFilterResponse>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				private BatchGetValuesByDataFilterRequest Body { get; set; }

				public override string MethodName => "batchGetByDataFilter";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values:batchGetByDataFilter";

				public BatchGetByDataFilterRequest(IClientService service, BatchGetValuesByDataFilterRequest body, string spreadsheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class BatchUpdateRequest : SheetsBaseServiceRequest<BatchUpdateValuesResponse>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				private BatchUpdateValuesRequest Body { get; set; }

				public override string MethodName => "batchUpdate";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values:batchUpdate";

				public BatchUpdateRequest(IClientService service, BatchUpdateValuesRequest body, string spreadsheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class BatchUpdateByDataFilterRequest : SheetsBaseServiceRequest<BatchUpdateValuesByDataFilterResponse>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				private BatchUpdateValuesByDataFilterRequest Body { get; set; }

				public override string MethodName => "batchUpdateByDataFilter";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values:batchUpdateByDataFilter";

				public BatchUpdateByDataFilterRequest(IClientService service, BatchUpdateValuesByDataFilterRequest body, string spreadsheetId)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class ClearRequest : SheetsBaseServiceRequest<ClearValuesResponse>
			{
				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				[RequestParameter("range", RequestParameterType.Path)]
				public virtual string Range { get; private set; }

				private ClearValuesRequest Body { get; set; }

				public override string MethodName => "clear";

				public override string HttpMethod => "POST";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values/{range}:clear";

				public ClearRequest(IClientService service, ClearValuesRequest body, string spreadsheetId, string range)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Range = range;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("range", new Parameter
					{
						Name = "range",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class GetRequest : SheetsBaseServiceRequest<ValueRange>
			{
				public enum DateTimeRenderOptionEnum
				{
					[StringValue("SERIAL_NUMBER")]
					SERIALNUMBER = 0,
					[StringValue("FORMATTED_STRING")]
					FORMATTEDSTRING = 1
				}

				public enum MajorDimensionEnum
				{
					[StringValue("DIMENSION_UNSPECIFIED")]
					DIMENSIONUNSPECIFIED = 0,
					[StringValue("ROWS")]
					ROWS = 1,
					[StringValue("COLUMNS")]
					COLUMNS = 2
				}

				public enum ValueRenderOptionEnum
				{
					[StringValue("FORMATTED_VALUE")]
					FORMATTEDVALUE = 0,
					[StringValue("UNFORMATTED_VALUE")]
					UNFORMATTEDVALUE = 1,
					[StringValue("FORMULA")]
					FORMULA = 2
				}

				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				[RequestParameter("range", RequestParameterType.Path)]
				public virtual string Range { get; private set; }

				[RequestParameter("dateTimeRenderOption", RequestParameterType.Query)]
				public virtual DateTimeRenderOptionEnum? DateTimeRenderOption { get; set; }

				[RequestParameter("majorDimension", RequestParameterType.Query)]
				public virtual MajorDimensionEnum? MajorDimension { get; set; }

				[RequestParameter("valueRenderOption", RequestParameterType.Query)]
				public virtual ValueRenderOptionEnum? ValueRenderOption { get; set; }

				public override string MethodName => "get";

				public override string HttpMethod => "GET";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values/{range}";

				public GetRequest(IClientService service, string spreadsheetId, string range)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Range = range;
					InitParameters();
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("range", new Parameter
					{
						Name = "range",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("dateTimeRenderOption", new Parameter
					{
						Name = "dateTimeRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("majorDimension", new Parameter
					{
						Name = "majorDimension",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("valueRenderOption", new Parameter
					{
						Name = "valueRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			public class UpdateRequest : SheetsBaseServiceRequest<UpdateValuesResponse>
			{
				public enum ResponseDateTimeRenderOptionEnum
				{
					[StringValue("SERIAL_NUMBER")]
					SERIALNUMBER = 0,
					[StringValue("FORMATTED_STRING")]
					FORMATTEDSTRING = 1
				}

				public enum ResponseValueRenderOptionEnum
				{
					[StringValue("FORMATTED_VALUE")]
					FORMATTEDVALUE = 0,
					[StringValue("UNFORMATTED_VALUE")]
					UNFORMATTEDVALUE = 1,
					[StringValue("FORMULA")]
					FORMULA = 2
				}

				public enum ValueInputOptionEnum
				{
					[StringValue("INPUT_VALUE_OPTION_UNSPECIFIED")]
					INPUTVALUEOPTIONUNSPECIFIED = 0,
					[StringValue("RAW")]
					RAW = 1,
					[StringValue("USER_ENTERED")]
					USERENTERED = 2
				}

				[RequestParameter("spreadsheetId", RequestParameterType.Path)]
				public virtual string SpreadsheetId { get; private set; }

				[RequestParameter("range", RequestParameterType.Path)]
				public virtual string Range { get; private set; }

				[RequestParameter("includeValuesInResponse", RequestParameterType.Query)]
				public virtual bool? IncludeValuesInResponse { get; set; }

				[RequestParameter("responseDateTimeRenderOption", RequestParameterType.Query)]
				public virtual ResponseDateTimeRenderOptionEnum? ResponseDateTimeRenderOption { get; set; }

				[RequestParameter("responseValueRenderOption", RequestParameterType.Query)]
				public virtual ResponseValueRenderOptionEnum? ResponseValueRenderOption { get; set; }

				[RequestParameter("valueInputOption", RequestParameterType.Query)]
				public virtual ValueInputOptionEnum? ValueInputOption { get; set; }

				private ValueRange Body { get; set; }

				public override string MethodName => "update";

				public override string HttpMethod => "PUT";

				public override string RestPath => "v4/spreadsheets/{spreadsheetId}/values/{range}";

				public UpdateRequest(IClientService service, ValueRange body, string spreadsheetId, string range)
					: base(service)
				{
					SpreadsheetId = spreadsheetId;
					Range = range;
					Body = body;
					InitParameters();
				}

				protected override object GetBody()
				{
					return Body;
				}

				protected override void InitParameters()
				{
					base.InitParameters();
					base.RequestParameters.Add("spreadsheetId", new Parameter
					{
						Name = "spreadsheetId",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("range", new Parameter
					{
						Name = "range",
						IsRequired = true,
						ParameterType = "path",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("includeValuesInResponse", new Parameter
					{
						Name = "includeValuesInResponse",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("responseDateTimeRenderOption", new Parameter
					{
						Name = "responseDateTimeRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("responseValueRenderOption", new Parameter
					{
						Name = "responseValueRenderOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
					base.RequestParameters.Add("valueInputOption", new Parameter
					{
						Name = "valueInputOption",
						IsRequired = false,
						ParameterType = "query",
						DefaultValue = null,
						Pattern = null
					});
				}
			}

			private const string Resource = "values";

			private readonly IClientService service;

			public ValuesResource(IClientService service)
			{
				this.service = service;
			}

			public virtual AppendRequest Append(ValueRange body, string spreadsheetId, string range)
			{
				return new AppendRequest(service, body, spreadsheetId, range);
			}

			public virtual BatchClearRequest BatchClear(BatchClearValuesRequest body, string spreadsheetId)
			{
				return new BatchClearRequest(service, body, spreadsheetId);
			}

			public virtual BatchClearByDataFilterRequest BatchClearByDataFilter(BatchClearValuesByDataFilterRequest body, string spreadsheetId)
			{
				return new BatchClearByDataFilterRequest(service, body, spreadsheetId);
			}

			public virtual BatchGetRequest BatchGet(string spreadsheetId)
			{
				return new BatchGetRequest(service, spreadsheetId);
			}

			public virtual BatchGetByDataFilterRequest BatchGetByDataFilter(BatchGetValuesByDataFilterRequest body, string spreadsheetId)
			{
				return new BatchGetByDataFilterRequest(service, body, spreadsheetId);
			}

			public virtual BatchUpdateRequest BatchUpdate(BatchUpdateValuesRequest body, string spreadsheetId)
			{
				return new BatchUpdateRequest(service, body, spreadsheetId);
			}

			public virtual BatchUpdateByDataFilterRequest BatchUpdateByDataFilter(BatchUpdateValuesByDataFilterRequest body, string spreadsheetId)
			{
				return new BatchUpdateByDataFilterRequest(service, body, spreadsheetId);
			}

			public virtual ClearRequest Clear(ClearValuesRequest body, string spreadsheetId, string range)
			{
				return new ClearRequest(service, body, spreadsheetId, range);
			}

			public virtual GetRequest Get(string spreadsheetId, string range)
			{
				return new GetRequest(service, spreadsheetId, range);
			}

			public virtual UpdateRequest Update(ValueRange body, string spreadsheetId, string range)
			{
				return new UpdateRequest(service, body, spreadsheetId, range);
			}
		}

		public class BatchUpdateRequest : SheetsBaseServiceRequest<BatchUpdateSpreadsheetResponse>
		{
			[RequestParameter("spreadsheetId", RequestParameterType.Path)]
			public virtual string SpreadsheetId { get; private set; }

			private BatchUpdateSpreadsheetRequest Body { get; set; }

			public override string MethodName => "batchUpdate";

			public override string HttpMethod => "POST";

			public override string RestPath => "v4/spreadsheets/{spreadsheetId}:batchUpdate";

			public BatchUpdateRequest(IClientService service, BatchUpdateSpreadsheetRequest body, string spreadsheetId)
				: base(service)
			{
				SpreadsheetId = spreadsheetId;
				Body = body;
				InitParameters();
			}

			protected override object GetBody()
			{
				return Body;
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("spreadsheetId", new Parameter
				{
					Name = "spreadsheetId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class CreateRequest : SheetsBaseServiceRequest<Spreadsheet>
		{
			private Spreadsheet Body { get; set; }

			public override string MethodName => "create";

			public override string HttpMethod => "POST";

			public override string RestPath => "v4/spreadsheets";

			public CreateRequest(IClientService service, Spreadsheet body)
				: base(service)
			{
				Body = body;
				InitParameters();
			}

			protected override object GetBody()
			{
				return Body;
			}

			protected override void InitParameters()
			{
				base.InitParameters();
			}
		}

		public class GetRequest : SheetsBaseServiceRequest<Spreadsheet>
		{
			[RequestParameter("spreadsheetId", RequestParameterType.Path)]
			public virtual string SpreadsheetId { get; private set; }

			[RequestParameter("includeGridData", RequestParameterType.Query)]
			public virtual bool? IncludeGridData { get; set; }

			[RequestParameter("ranges", RequestParameterType.Query)]
			public virtual Repeatable<string> Ranges { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "v4/spreadsheets/{spreadsheetId}";

			public GetRequest(IClientService service, string spreadsheetId)
				: base(service)
			{
				SpreadsheetId = spreadsheetId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("spreadsheetId", new Parameter
				{
					Name = "spreadsheetId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includeGridData", new Parameter
				{
					Name = "includeGridData",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("ranges", new Parameter
				{
					Name = "ranges",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class GetByDataFilterRequest : SheetsBaseServiceRequest<Spreadsheet>
		{
			[RequestParameter("spreadsheetId", RequestParameterType.Path)]
			public virtual string SpreadsheetId { get; private set; }

			private GetSpreadsheetByDataFilterRequest Body { get; set; }

			public override string MethodName => "getByDataFilter";

			public override string HttpMethod => "POST";

			public override string RestPath => "v4/spreadsheets/{spreadsheetId}:getByDataFilter";

			public GetByDataFilterRequest(IClientService service, GetSpreadsheetByDataFilterRequest body, string spreadsheetId)
				: base(service)
			{
				SpreadsheetId = spreadsheetId;
				Body = body;
				InitParameters();
			}

			protected override object GetBody()
			{
				return Body;
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("spreadsheetId", new Parameter
				{
					Name = "spreadsheetId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		private const string Resource = "spreadsheets";

		private readonly IClientService service;

		public virtual DeveloperMetadataResource DeveloperMetadata { get; }

		public virtual SheetsResource Sheets { get; }

		public virtual ValuesResource Values { get; }

		public SpreadsheetsResource(IClientService service)
		{
			this.service = service;
			DeveloperMetadata = new DeveloperMetadataResource(service);
			Sheets = new SheetsResource(service);
			Values = new ValuesResource(service);
		}

		public virtual BatchUpdateRequest BatchUpdate(BatchUpdateSpreadsheetRequest body, string spreadsheetId)
		{
			return new BatchUpdateRequest(service, body, spreadsheetId);
		}

		public virtual CreateRequest Create(Spreadsheet body)
		{
			return new CreateRequest(service, body);
		}

		public virtual GetRequest Get(string spreadsheetId)
		{
			return new GetRequest(service, spreadsheetId);
		}

		public virtual GetByDataFilterRequest GetByDataFilter(GetSpreadsheetByDataFilterRequest body, string spreadsheetId)
		{
			return new GetByDataFilterRequest(service, body, spreadsheetId);
		}
	}
}
