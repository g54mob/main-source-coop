using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Zenject;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class GoogleSheetsDatabaseDataReader : IDatabaseDataReader, IInitializable
	{
		private const char DELIMITER_BETWEEN_TABLE_NAME_AND_RANGE = '!';

		private const string COMMENT_SYMBOLS = "//";

		private const string IGNORED_TABLE_SYMBOLS = "[ignored]";

		private const string ENUM_TABLE_SYMBOLS = "[enum]";

		private const string ENUM_TABLE_NAME_SYMBOL = "'";

		private const char START_SPECIAL_HEADER_SYMBOL = '[';

		private const char END_SPECIAL_HEADER_SYMBOL = ']';

		private readonly IDataBaseSettings _dataBaseSettings;

		private readonly ITableNamer _tableNamer;

		private readonly ICsvDatabaseConverter _csvDatabaseConverter;

		private readonly SheetsService _sheetsService;

		private IList<ValueRange> _allTablesData;

		private IList<ValueRange> _allRawTablesData;

		private List<string> _dataTableNames;

		private List<string> _enumNames;

		private List<string> _allTableNames;

		public GoogleSheetsDatabaseDataReader(IDataBaseSettings dataBaseSettings, ITableNamer tableNamer, ICsvDatabaseConverter csvDatabaseConverter, SheetsService sheetsService)
		{
			_dataBaseSettings = dataBaseSettings ?? throw new ArgumentNullException("dataBaseSettings");
			_tableNamer = tableNamer ?? throw new ArgumentNullException("tableNamer");
			_csvDatabaseConverter = csvDatabaseConverter ?? throw new ArgumentNullException("csvDatabaseConverter");
			_sheetsService = sheetsService ?? throw new ArgumentNullException("sheetsService");
		}

		public void Initialize()
		{
			GetAllTablesNames(_sheetsService);
			BatchRequest();
			HandleCommentsAndEmptyRows(_allTablesData);
		}

		public List<TDataType> GetTableData<TDataType>() where TDataType : new()
		{
			string tableNameByType = _tableNamer.GetTableNameByType(typeof(TDataType));
			string text = _allTableNames.FirstOrDefault((string tn) => tn == tableNameByType);
			if (text == null)
			{
				throw new Exception("There is no such table " + tableNameByType + ". Check, maybe this table is [ignored].");
			}
			IList<IList<object>> dataFromTable = GetDataFromTable(text);
			return _csvDatabaseConverter.FromMatrixToActualData<TDataType>(dataFromTable);
		}

		public List<object> GetTableData(Type type)
		{
			string tableNameByType = _tableNamer.GetTableNameByType(type);
			string text = _allTableNames.FirstOrDefault((string tn) => tn == tableNameByType);
			if (text == null)
			{
				throw new Exception("There is no such table " + tableNameByType + ". Check, maybe this table is [ignored].");
			}
			IList<IList<object>> dataFromTable = GetDataFromTable(text);
			return _csvDatabaseConverter.FromMatrixToActualData(dataFromTable, type);
		}

		public IList<IList<object>> GetTableMatrix(string tableName)
		{
			foreach (ValueRange allTablesDatum in _allTablesData)
			{
				string text = HandleEnumName(allTablesDatum.Range);
				int num = text.IndexOf('!');
				if (text.Remove(num, text.Length - num) == tableName)
				{
					return allTablesDatum.Values;
				}
			}
			throw new Exception("There is no table with name " + tableName);
		}

		public IList<IList<object>> GetRawTableMatrix(string tableName)
		{
			if (_allRawTablesData == null)
			{
				SpreadsheetsResource.ValuesResource.BatchGetRequest batchGetRequest = _sheetsService.Spreadsheets.Values.BatchGet(_dataBaseSettings.GetConnectionString());
				List<string> list = new List<string>();
				list.AddRange(_allTableNames);
				list.AddRange(_enumNames);
				batchGetRequest.Ranges = list;
				BatchGetValuesResponse batchGetValuesResponse = batchGetRequest.Execute();
				_allRawTablesData = batchGetValuesResponse.ValueRanges;
			}
			foreach (ValueRange allRawTablesDatum in _allRawTablesData)
			{
				int num = allRawTablesDatum.Range.IndexOf('!');
				if (allRawTablesDatum.Range.Remove(num, allRawTablesDatum.Range.Length - num).Replace("'", string.Empty) == tableName)
				{
					return allRawTablesDatum.Values;
				}
			}
			throw new Exception("There is no table with name " + tableName);
		}

		public Dictionary<Type, List<object>> GetAllTablesDataFromDatabase()
		{
			Dictionary<Type, List<object>> dictionary = new Dictionary<Type, List<object>>();
			foreach (string allTableName in _allTableNames)
			{
				if (!allTableName.Contains("[enum]"))
				{
					Type typeByTableName = _tableNamer.GetTypeByTableName(allTableName);
					List<object> value = _csvDatabaseConverter.FromMatrixToActualData(GetDataFromTable(allTableName), typeByTableName);
					dictionary.Add(typeByTableName, value);
				}
			}
			return dictionary;
		}

		public List<string> GetAllTableNames()
		{
			return _dataTableNames;
		}

		public List<string> GetAllEnumNames()
		{
			return _enumNames;
		}

		private void BatchRequest()
		{
			SpreadsheetsResource.ValuesResource.BatchGetRequest batchGetRequest = _sheetsService.Spreadsheets.Values.BatchGet(_dataBaseSettings.GetConnectionString());
			_allTableNames = new List<string>();
			_allTableNames.AddRange(_dataTableNames);
			_allTableNames.AddRange(_enumNames);
			batchGetRequest.Ranges = _allTableNames;
			BatchGetValuesResponse batchGetValuesResponse = batchGetRequest.Execute();
			_allTablesData = batchGetValuesResponse.ValueRanges;
		}

		private IList<IList<object>> GetDataFromTable(string tableName)
		{
			foreach (ValueRange allTablesDatum in _allTablesData)
			{
				string text = HandleEnumName(allTablesDatum.Range);
				int num = text.IndexOf('!');
				if (text.Remove(num, text.Length - num) == tableName)
				{
					IList<IList<object>> values = allTablesDatum.Values;
					HandleSpecialHeaders(values);
					return values;
				}
			}
			throw new Exception("There is no table with name " + tableName);
		}

		private void HandleSpecialHeaders(IList<IList<object>> valueRangeValues)
		{
			for (int i = 0; i < valueRangeValues[0].Count; i++)
			{
				string text = valueRangeValues[0][i].ToString();
				int num = text.IndexOf('[');
				int num2 = text.IndexOf(']');
				if (num != -1 && num2 != -1)
				{
					string value = text.Replace(text.Substring(num, num2 - num + 1), "");
					valueRangeValues[0][i] = value;
				}
			}
		}

		private string HandleEnumName(string rangeString)
		{
			return rangeString.Replace("'", string.Empty);
		}

		private void HandleCommentsAndEmptyRows(IList<ValueRange> allTablesData)
		{
			foreach (ValueRange allTablesDatum in allTablesData)
			{
				for (int i = 0; i < allTablesDatum.Values.Count; i++)
				{
					IList<object> list = allTablesDatum.Values[i];
					if (list.Count == 0 || list[0].ToString().IndexOf("//", StringComparison.Ordinal) != -1)
					{
						allTablesDatum.Values.Remove(list);
						i--;
						continue;
					}
					if (IsRowWithOnlySpaces(list))
					{
						allTablesDatum.Values.Remove(list);
						i--;
						continue;
					}
					for (int j = 0; j < list.Count; j++)
					{
						string text = (string)list[j];
						list[j] = text.Replace("\r\n", "\\n").Replace("\n", "\\n");
					}
				}
			}
		}

		private void GetAllTablesNames(SheetsService sheetsService)
		{
			Spreadsheet spreadsheet = sheetsService.Spreadsheets.Get(_dataBaseSettings.GetConnectionString()).Execute();
			_dataTableNames = new List<string>();
			_enumNames = new List<string>();
			foreach (string item in spreadsheet.Sheets.Select((Sheet sheet) => sheet.Properties.Title))
			{
				if (item.IndexOf("[ignored]", StringComparison.Ordinal) == -1 && item.IndexOf("[enum]", StringComparison.Ordinal) == -1)
				{
					_dataTableNames.Add(item);
				}
				else if (item.IndexOf("[ignored]", StringComparison.Ordinal) == -1 && item.IndexOf("[enum]", StringComparison.Ordinal) != -1)
				{
					_enumNames.Add(item);
				}
			}
		}

		private bool IsRowWithOnlySpaces(IList<object> row)
		{
			for (int i = 0; i < row.Count; i++)
			{
				if (((string)row[i]).Replace(" ", string.Empty) != string.Empty)
				{
					return false;
				}
			}
			return true;
		}
	}
}
