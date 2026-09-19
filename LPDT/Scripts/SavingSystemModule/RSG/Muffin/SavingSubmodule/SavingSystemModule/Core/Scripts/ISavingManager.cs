using System.Collections.Generic;
using JetBrains.Annotations;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public interface ISavingManager
	{
		void SaveDataWithID(string id, object dataObject, string dataContainerName);

		void SaveDataRange(Dictionary<string, object> objectByIDDictionary, string dataGroupName);

		void Commit(string fileName, [CanBeNull] string customSaveFolderName = null);

		void CommitAsync(string inFile, [CanBeNull] string customSaveFolderName = null);

		T LoadDataForID<T>(string id);

		void LoadDataForIDOverride(string id, object objectToOverride);

		Dictionary<string, T> LoadDataRangeForIDs<T>(List<string> ids);

		void LoadDataRangeForIDsOverride(Dictionary<string, object> objectToOverrideByIDDictionary);

		void DeleteDataWithID(string id);

		void DeleteDataRangeForIDs(List<string> ids);
	}
}
