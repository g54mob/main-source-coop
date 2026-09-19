using System.Collections.Generic;
using JetBrains.Annotations;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	[PublicAPI]
	public interface ISaveDataContainer
	{
		string GetDataStringForGroup(string groupName);

		void SetDataForID(string id, object dataObject, string groupName);

		void SetDataRangeForIDs(Dictionary<string, object> dataObjectByIDDictionary, string groupName);

		T GetDataForID<T>(string id);

		Dictionary<string, T> GetDataRangeForIDs<T>(List<string> ids);

		void GetDataForIDOverride(string id, object dataToOverride);

		void GetDataRangeForIDsOverride(Dictionary<string, object> dataToOverrideByIDDictionary);

		bool TryGetDataForID<T>(string id, out T data);

		bool TryGetDataRangeForIDs<T>(List<string> ids, out Dictionary<string, T> dataObjectByIDDictionary);

		bool ContainsDataForID(string id);

		bool ContainsDataForAllIDs(List<string> ids);

		void RemoveDataForID(string id);

		void RemoveDataRangeForIDs(List<string> ids);

		void ProcessDataString(string dataString, string groupName);

		void ProcessDataStrings(Dictionary<string, string> dataStringByGroupNameDictionary);

		bool ValidateDataString(string dataString);

		bool ValidateDataStrings(List<string> dataStrings);

		bool TryProcessDataString(string dataString, string groupName);

		bool TryProcessDataStrings(Dictionary<string, string> dataStringByGroupNameDictionary);

		void ClearData();
	}
}
