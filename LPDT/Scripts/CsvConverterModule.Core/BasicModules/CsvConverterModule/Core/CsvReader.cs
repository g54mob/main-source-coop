using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BasicModules.CsvConverterModule.Core
{
	public class CsvReader : ICsvReader
	{
		private const string OLD_FLOAT_SEPARATOR = ",";

		private const string NEW_FLOAT_SEPARATOR = ".";

		private const string CSV_FILE_IS_EMPTY_EXCEPTION = "Csv file is empty";

		private const string LINE_SEPARATOR = "\n";

		private readonly StringReader _stringReader;

		private readonly CsvSettings _csvSettings;

		private NumberFormatInfo _provider;

		public CsvReader(StringReader stringReader, CsvSettings csvSettings)
		{
			_stringReader = stringReader ?? throw new ArgumentNullException("stringReader");
			_csvSettings = csvSettings ?? throw new ArgumentNullException("csvSettings");
		}

		public void Initialize()
		{
			_provider = new NumberFormatInfo
			{
				NumberDecimalSeparator = "."
			};
		}

		public List<TDataType> GetRecords<TDataType>() where TDataType : new()
		{
			List<string> allLines = GetAllLines();
			List<TDataType> list = new List<TDataType>();
			List<PropertyInfo> propertyInfos = typeof(TDataType).GetProperties().ToList();
			string[] headers = allLines[0].Split(_csvSettings.GetDelimiter());
			for (int i = 1; i < allLines.Count; i++)
			{
				TDataType val = new TDataType();
				SetAllDataInDataHolders(allLines, i, headers, propertyInfos, val);
				list.Add(val);
			}
			return list;
		}

		public List<object> GetRecords(Type type)
		{
			List<string> allLines = GetAllLines();
			List<object> list = new List<object>();
			List<PropertyInfo> propertyInfos = type.GetProperties().ToList();
			string[] headers = allLines[0].Split(_csvSettings.GetDelimiter());
			for (int i = 1; i < allLines.Count; i++)
			{
				object obj = Activator.CreateInstance(type);
				SetAllDataInDataHolders(allLines, i, headers, propertyInfos, obj);
				list.Add(obj);
			}
			return list;
		}

		private void SetAllDataInDataHolders(List<string> allLines, int lineId, string[] headers, List<PropertyInfo> propertyInfos, object dataHolder)
		{
			string[] data = allLines[lineId].Split(_csvSettings.GetDelimiter());
			SetAllDataInDataHoldersByHeaders(headers, propertyInfos, dataHolder, data);
		}

		private void SetAllDataInDataHoldersByHeaders(string[] headers, List<PropertyInfo> propertyInfos, object dataHolder, string[] data)
		{
			for (int i = 0; i < headers.Length; i++)
			{
				SetDataInDataHolderForEachHeader(headers, propertyInfos, dataHolder, data, i);
			}
		}

		private void SetDataInDataHolderForEachHeader(string[] headers, List<PropertyInfo> propertyInfos, object dataHolder, string[] data, int headerIndex)
		{
			for (int i = 0; i < propertyInfos.Count; i++)
			{
				PropertyInfo propertyInfo = propertyInfos[i];
				if (propertyInfo.Name == headers[headerIndex])
				{
					TryToSetDataInDataHolder(propertyInfo, dataHolder, data, headerIndex);
				}
			}
		}

		private void TryToSetDataInDataHolder(PropertyInfo propertyInfo, object dataHolder, string[] data, int headerIndex)
		{
			try
			{
				propertyInfo.SetValue(dataHolder, ConvertFromStringToDataByType(data[headerIndex], propertyInfo));
			}
			catch (Exception ex)
			{
				string text = data.Aggregate(string.Empty, (string current, string cell) => current + cell + "      ");
				Debug.LogError("Data with error: " + text);
				if (data.Length < headerIndex)
				{
					Debug.LogError($"{propertyInfo.Name} with value {data[headerIndex]} cant be converted into {propertyInfo.PropertyType} \n" + "Exception text: " + ex.Message);
				}
				else
				{
					Debug.LogError($"{propertyInfo.Name} has empty values in Database. \n{propertyInfo.DeclaringType} -> {GetID(dataHolder)} \n" + "Exception text: " + ex.Message);
				}
			}
		}

		private string GetID(object dataHolder)
		{
			string name = "ID";
			PropertyInfo property = dataHolder.GetType().GetProperty(name);
			if (property != null)
			{
				return property.GetValue(dataHolder, null).ToString();
			}
			return "ID not found";
		}

		private object ConvertFromStringToDataByType(string data, PropertyInfo propertyInfo)
		{
			Type propertyType = propertyInfo.PropertyType;
			if (propertyType == typeof(double))
			{
				return Convert.ToDouble(data.Replace(",", "."), _provider);
			}
			if (propertyType == typeof(float))
			{
				return Convert.ToSingle(data.Replace(",", "."), _provider);
			}
			if (propertyType.IsEnum)
			{
				return Enum.Parse(propertyType, data.Replace(" ", string.Empty));
			}
			return Convert.ChangeType(data, propertyType);
		}

		private List<string> GetAllLines()
		{
			List<string> list = (_stringReader.ReadToEnd() ?? throw new Exception("Csv file is empty")).Split("\n").ToList();
			RemoveAllEmptyLines(list);
			if (list.Count < 2)
			{
				throw new Exception($"Csv file has incorrect number of lines ({list.Count})");
			}
			return list;
		}

		private void RemoveAllEmptyLines(List<string> allLines)
		{
			allLines.RemoveAll(IsStringEmpty);
		}

		private bool IsStringEmpty(string stringForCheck)
		{
			return stringForCheck == string.Empty;
		}
	}
}
