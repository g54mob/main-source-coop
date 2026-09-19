using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class GoogleSheetsDatabaseDataWriter : IDatabaseDataWriter
	{
		private readonly SheetsService _sheetsService;

		private readonly IDataBaseSettings _dataBaseSettings;

		private readonly ITableNamer _dataHoldersNamer;

		public GoogleSheetsDatabaseDataWriter(IDataBaseSettings dataBaseSettings, SheetsService sheetsService, ITableNamer dataHoldersNamer)
		{
			_dataBaseSettings = dataBaseSettings ?? throw new ArgumentNullException("dataBaseSettings");
			_sheetsService = sheetsService ?? throw new ArgumentNullException("sheetsService");
			_dataHoldersNamer = dataHoldersNamer ?? throw new ArgumentNullException("dataHoldersNamer");
		}

		public void SetDataToDatabaseTable<TTable>(List<List<string>> csvData)
		{
			int sheetId = GetSheetId<TTable>();
			UpdateDatabaseTable(csvData, sheetId);
		}

		public void SetDataToDatabaseTable(string tableName, List<List<string>> csvData)
		{
			int sheetId = GetSheetId(tableName);
			UpdateDatabaseTable(csvData, sheetId);
		}

		private void UpdateDatabaseTable(List<List<string>> csvData, int sheetId)
		{
			List<Request> requestsForOnAllColumns = GetRequestsForOnAllColumns(csvData, sheetId);
			BatchUpdateSpreadsheetRequest body = new BatchUpdateSpreadsheetRequest
			{
				Requests = requestsForOnAllColumns
			};
			_sheetsService.Spreadsheets.BatchUpdate(body, _dataBaseSettings.GetConnectionString()).Execute();
		}

		private int GetSheetId<TTable>()
		{
			return _sheetsService.Spreadsheets.Get(_dataBaseSettings.GetConnectionString()).Execute().Sheets.FirstOrDefault((Sheet s) => s.Properties.Title == _dataHoldersNamer.ConvertFromClassNameToTableName(typeof(TTable).Name))?.Properties.SheetId ?? throw new Exception("Database does not have a table with \"" + typeof(TTable).Name + "\" name");
		}

		private int GetSheetId(string tableName)
		{
			return _sheetsService.Spreadsheets.Get(_dataBaseSettings.GetConnectionString()).Execute().Sheets.FirstOrDefault((Sheet s) => s.Properties.Title == tableName)?.Properties.SheetId ?? throw new Exception("Database does not have a table with \"" + tableName + "\" name");
		}

		private List<Request> GetRequestsForOnAllColumns(List<List<string>> csvData, int sheetId)
		{
			List<Request> list = new List<Request>();
			int num = 0;
			foreach (List<string> csvDatum in csvData)
			{
				list.Add(CreateNewRequest(csvDatum, sheetId, num));
				num++;
			}
			return list;
		}

		private Request CreateNewRequest(List<string> row, int sheetId, int rowIndex)
		{
			return new Request
			{
				UpdateCells = new UpdateCellsRequest
				{
					Rows = new List<RowData>
					{
						new RowData
						{
							Values = ConvertStringsToCellData(row)
						}
					},
					Fields = "userEnteredValue",
					Range = new GridRange
					{
						SheetId = sheetId,
						StartRowIndex = rowIndex,
						EndRowIndex = rowIndex + 1,
						StartColumnIndex = 0,
						EndColumnIndex = row.Count
					}
				}
			};
		}

		private List<CellData> ConvertStringsToCellData(List<string> row)
		{
			List<CellData> list = new List<CellData>();
			foreach (string item in row)
			{
				if (int.TryParse(item, out var result))
				{
					list.Add(new CellData
					{
						UserEnteredValue = new ExtendedValue
						{
							NumberValue = result
						}
					});
				}
				else
				{
					list.Add(new CellData
					{
						UserEnteredValue = new ExtendedValue
						{
							StringValue = item
						}
					});
				}
			}
			return list;
		}
	}
}
