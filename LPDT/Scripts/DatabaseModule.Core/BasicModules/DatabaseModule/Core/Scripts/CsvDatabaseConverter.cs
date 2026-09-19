using System;
using System.Collections.Generic;
using System.IO;
using BasicModules.CsvConverterModule.Core;
using UnityEngine;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class CsvDatabaseConverter : ICsvDatabaseConverter
	{
		private const string ROW_SEPARATOR = "\n";

		private const string CSV_FILE_EXTENSION = ".csv";

		private readonly CsvSettings _csvSettings;

		private readonly ICsvDatabaseReaderFactory _csvDatabaseReaderFactory;

		private readonly ICsvDatabaseWriterFactory _csvDatabaseWriterFactory;

		public CsvDatabaseConverter(CsvSettings csvSettings, ICsvDatabaseReaderFactory csvDatabaseReaderFactory, ICsvDatabaseWriterFactory csvDatabaseWriterFactory)
		{
			_csvSettings = csvSettings ?? throw new ArgumentNullException("csvSettings");
			_csvDatabaseReaderFactory = csvDatabaseReaderFactory ?? throw new ArgumentNullException("csvDatabaseReaderFactory");
			_csvDatabaseWriterFactory = csvDatabaseWriterFactory ?? throw new ArgumentNullException("csvDatabaseWriterFactory");
		}

		public List<object> FromMatrixToActualData(IList<IList<object>> matrix, Type type)
		{
			string csvString = FromMatrixToCsvString(matrix);
			return FromCsvStringToActualData(csvString, type);
		}

		public List<TDataType> FromMatrixToActualData<TDataType>(IList<IList<object>> matrix) where TDataType : new()
		{
			string csvString = FromMatrixToCsvString(matrix);
			return FromCsvStringToActualData<TDataType>(csvString);
		}

		public List<TDataType> GetTableData<TDataType>() where TDataType : new()
		{
			return FromCsvFileToActualDataByName<TDataType>(typeof(TDataType).Name);
		}

		public List<TDataType> GetTableData<TDataType>(string tableName) where TDataType : new()
		{
			return FromCsvFileToActualDataByName<TDataType>(tableName);
		}

		public Dictionary<Type, List<object>> GetAllTablesData(List<Type> types)
		{
			Dictionary<Type, List<object>> dictionary = new Dictionary<Type, List<object>>();
			foreach (Type type in types)
			{
				TextAsset textAsset = Resources.Load<TextAsset>(_csvSettings.GetFolderName() + "/" + type.Name);
				dictionary.Add(type, FromCsvStringToActualData(textAsset.text, type));
			}
			return dictionary;
		}

		public void FromDataToCsvFile(string tableName, IList<IList<string>> table)
		{
			string path = _csvSettings.GetCsvExportPath() + tableName + ".csv";
			using StreamWriter streamWriter = _csvDatabaseWriterFactory.CreateStreamWriter(path);
			_csvDatabaseWriterFactory.CreateCsvWriter(streamWriter, _csvSettings).CreateCsvFile(table);
		}

		private string FromMatrixToCsvString(IList<IList<object>> matrix)
		{
			string delimiter = _csvSettings.GetDelimiter();
			string text = string.Empty;
			foreach (IList<object> item in matrix)
			{
				text = text + RowAdding(item, delimiter) + "\n";
			}
			return text;
		}

		private List<TDataType> FromCsvFileToActualDataByName<TDataType>(string tableName) where TDataType : new()
		{
			TextAsset textAsset = Resources.Load<TextAsset>(_csvSettings.GetFolderName() + "/" + tableName);
			return FromCsvStringToActualData<TDataType>(textAsset.text);
		}

		private List<TDataType> FromCsvStringToActualData<TDataType>(string csvString) where TDataType : new()
		{
			using StringReader stringReader = _csvDatabaseReaderFactory.CreateStringReader(csvString);
			return _csvDatabaseReaderFactory.CreateCsvReader(stringReader, _csvSettings).GetRecords<TDataType>();
		}

		private List<object> FromCsvStringToActualData(string csvString, Type type)
		{
			using StringReader stringReader = _csvDatabaseReaderFactory.CreateStringReader(csvString);
			return _csvDatabaseReaderFactory.CreateCsvReader(stringReader, _csvSettings).GetRecords(type);
		}

		private string RowAdding(IList<object> row, string delimiter)
		{
			string text = string.Empty;
			for (int i = 0; i < row.Count; i++)
			{
				object obj = row[i];
				if (i != row.Count - 1)
				{
					obj = obj?.ToString() + delimiter;
				}
				text += obj;
			}
			return text;
		}
	}
}
