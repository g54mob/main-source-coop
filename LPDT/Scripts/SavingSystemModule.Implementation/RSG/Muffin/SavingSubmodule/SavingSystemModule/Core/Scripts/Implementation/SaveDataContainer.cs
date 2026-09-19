using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces;
using UnityEngine;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	public class SaveDataContainer : ISaveDataContainer
	{
		private const string DATA_ENTRIES_SEPARATOR = "%";

		private readonly IJsonConvertor _jsonConvertor;

		private readonly Dictionary<string, Dictionary<string, string>> _dataByGroupDictionary = new Dictionary<string, Dictionary<string, string>>();

		public Dictionary<string, Dictionary<string, string>> DataByGroupDictionary => _dataByGroupDictionary;

		public SaveDataContainer(IJsonConvertor convertor)
		{
			_jsonConvertor = convertor ?? throw new ArgumentNullException("convertor");
		}

		public void ClearData()
		{
			_dataByGroupDictionary.Clear();
		}

		public string GetDataStringForGroup(string groupName)
		{
			if (!_dataByGroupDictionary.TryGetValue(groupName, out var value))
			{
				return string.Empty;
			}
			return ConvertDataByIDDictionaryToDataString(value);
		}

		public void SetDataForID(string id, object dataObject, string groupName)
		{
			if (!_dataByGroupDictionary.ContainsKey(groupName))
			{
				_dataByGroupDictionary[groupName] = new Dictionary<string, string>();
			}
			_dataByGroupDictionary[groupName][id] = ConvertDataObjectToDataString(dataObject);
		}

		public void SetDataRangeForIDs(Dictionary<string, object> dataObjectByIDDictionary, string groupName)
		{
			if (!_dataByGroupDictionary.ContainsKey(groupName))
			{
				_dataByGroupDictionary[groupName] = new Dictionary<string, string>();
			}
			Dictionary<string, string> dictionary = _dataByGroupDictionary[groupName];
			foreach (KeyValuePair<string, object> item in dataObjectByIDDictionary)
			{
				dictionary[item.Key] = ConvertDataObjectToDataString(item.Value);
			}
			_dataByGroupDictionary[groupName] = dictionary;
		}

		public T GetDataForID<T>(string id)
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> item in _dataByGroupDictionary)
			{
				if (item.Value.TryGetValue(id, out var value))
				{
					return ConvertDataStringToDataObject<T>(value);
				}
			}
			return default(T);
		}

		public Dictionary<string, T> GetDataRangeForIDs<T>(List<string> ids)
		{
			Dictionary<string, T> dictionary = new Dictionary<string, T>();
			foreach (string id in ids)
			{
				dictionary[id] = GetDataForID<T>(id);
			}
			return dictionary;
		}

		public void GetDataForIDOverride(string id, object dataToOverride)
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> item in _dataByGroupDictionary)
			{
				if (item.Value.TryGetValue(id, out var value))
				{
					ConvertDataStringToDataObjectOverride(value, dataToOverride);
				}
			}
		}

		public void GetDataRangeForIDsOverride(Dictionary<string, object> dataToOverrideByIDDictionary)
		{
			foreach (KeyValuePair<string, object> item in dataToOverrideByIDDictionary)
			{
				GetDataForIDOverride(item.Key, item.Value);
			}
		}

		public bool TryGetDataForID<T>(string id, out T data)
		{
			data = default(T);
			foreach (KeyValuePair<string, Dictionary<string, string>> item in _dataByGroupDictionary)
			{
				if (item.Value.ContainsKey(id))
				{
					data = GetDataForID<T>(id);
					return true;
				}
			}
			return false;
		}

		public bool TryGetDataRangeForIDs<T>(List<string> ids, out Dictionary<string, T> dataObjectByIDDictionary)
		{
			dataObjectByIDDictionary = new Dictionary<string, T>();
			foreach (string id in ids)
			{
				if (!TryGetDataForID<T>(id, out var data))
				{
					return false;
				}
				dataObjectByIDDictionary[id] = data;
			}
			return dataObjectByIDDictionary.Any();
		}

		public bool ContainsDataForID(string id)
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> item in _dataByGroupDictionary)
			{
				if (item.Value.ContainsKey(id))
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsDataForAllIDs(List<string> ids)
		{
			foreach (string id in ids)
			{
				if (!ContainsDataForID(id))
				{
					return false;
				}
			}
			return true;
		}

		public void RemoveDataForID(string id)
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> item in _dataByGroupDictionary)
			{
				if (item.Value.ContainsKey(id))
				{
					item.Value.Remove(id);
				}
			}
		}

		public void RemoveDataRangeForIDs(List<string> ids)
		{
			ids.ForEach(RemoveDataForID);
		}

		public void ProcessDataString(string dataString, string groupName)
		{
			_dataByGroupDictionary[groupName] = ConvertDataStringToDataByIDDictionary(dataString);
		}

		public void ProcessDataStrings(Dictionary<string, string> dataStringByGroupNameDictionary)
		{
			foreach (KeyValuePair<string, string> item in dataStringByGroupNameDictionary)
			{
				_dataByGroupDictionary[item.Key] = ConvertDataStringToDataByIDDictionary(item.Value);
			}
		}

		public bool TryProcessDataString(string dataString, string groupName)
		{
			if (string.IsNullOrEmpty(dataString))
			{
				return false;
			}
			try
			{
				ProcessDataString(dataString, groupName);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool TryProcessDataStrings(Dictionary<string, string> dataStringByGroupNameDictionary)
		{
			try
			{
				ProcessDataStrings(dataStringByGroupNameDictionary);
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogError("[SaveDataContainer.TryProcessDataStrings] Error processing data strings: " + ex.Message);
				return false;
			}
		}

		public bool ValidateDataString(string dataString)
		{
			if (string.IsNullOrEmpty(dataString))
			{
				return false;
			}
			try
			{
				ConvertDataStringToDataByIDDictionary(dataString);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool ValidateDataStrings(List<string> dataStrings)
		{
			try
			{
				foreach (string dataString in dataStrings)
				{
					if (!ValidateDataString(dataString))
					{
						return false;
					}
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private List<SaveDataEntry> ConvertDataStringsToDataEntries(string[] dataStrings)
		{
			List<SaveDataEntry> list = new List<SaveDataEntry>();
			foreach (string dataString in dataStrings)
			{
				list.Add(_jsonConvertor.Convert<SaveDataEntry>(dataString));
			}
			return list;
		}

		private string ConvertDataObjectToDataString(object dataObject)
		{
			return _jsonConvertor.Convert(dataObject);
		}

		private T ConvertDataStringToDataObject<T>(string dataString)
		{
			return _jsonConvertor.Convert<T>(dataString);
		}

		private void ConvertDataStringToDataObjectOverride(string dataString, object dataObject)
		{
			_jsonConvertor.ConvertDataFromStringOverwrite(dataString, dataObject);
		}

		private string ConvertDataByIDDictionaryToDataString(Dictionary<string, string> dataByIDDictionary)
		{
			if (dataByIDDictionary == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in dataByIDDictionary)
			{
				SaveDataEntry dataObject = new SaveDataEntry
				{
					ID = item.Key,
					Data = item.Value
				};
				stringBuilder.Append(_jsonConvertor.Convert(dataObject));
				stringBuilder.Append("%");
			}
			return stringBuilder.ToString();
		}

		private Dictionary<string, string> ConvertDataStringToDataByIDDictionary(string dataString)
		{
			string[] dataStrings = dataString.Split("%", StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (SaveDataEntry item in ConvertDataStringsToDataEntries(dataStrings))
			{
				dictionary[item.ID] = item.Data;
			}
			return dictionary;
		}
	}
}
