using System;
using System.Collections.Generic;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	public class SavingManager : ISavingManager
	{
		private readonly ISaveDataSaver _saveDataSaver;

		private readonly ISaveDataLoader _saveDataLoader;

		private readonly ISaveDataDeleter _saveDataDeleter;

		public SavingManager(ISaveDataSaver saveDataSaver, ISaveDataLoader saveDataLoader, ISaveDataDeleter saveDataDeleter)
		{
			_saveDataSaver = saveDataSaver ?? throw new ArgumentNullException("saveDataSaver");
			_saveDataLoader = saveDataLoader ?? throw new ArgumentNullException("saveDataLoader");
			_saveDataDeleter = saveDataDeleter ?? throw new ArgumentNullException("saveDataDeleter");
		}

		public void SaveDataRange(Dictionary<string, object> objectByIDDictionary, string dataGroupName)
		{
			_saveDataSaver.SaveDataRange(objectByIDDictionary, dataGroupName);
		}

		public void SaveDataWithID(string id, object dataObject, string dataContainerName)
		{
			_saveDataSaver.SaveDataWithID(id, dataObject, dataContainerName);
		}

		public void Commit(string fileName, string customSaveFolderName = null)
		{
			_saveDataSaver.Commit(fileName, customSaveFolderName);
		}

		public void CommitAsync(string inFile, string customSaveFolderName = null)
		{
			_saveDataSaver.CommitAsync(inFile, customSaveFolderName);
		}

		public T LoadDataForID<T>(string id)
		{
			return _saveDataLoader.LoadDataForID<T>(id);
		}

		public void LoadDataForIDOverride(string id, object objectToOverride)
		{
			_saveDataLoader.LoadDataForIDOverride(id, objectToOverride);
		}

		public Dictionary<string, T> LoadDataRangeForIDs<T>(List<string> ids)
		{
			return _saveDataLoader.LoadDataRangeForIDs<T>(ids);
		}

		public void LoadDataRangeForIDsOverride(Dictionary<string, object> objectToOverrideByIDDictionary)
		{
			_saveDataLoader.LoadDataRangeForIDsOverride(objectToOverrideByIDDictionary);
		}

		public void DeleteDataRangeForIDs(List<string> ids)
		{
			_saveDataDeleter.DeleteDataRangeForIDs(ids);
		}

		public void DeleteDataWithID(string id)
		{
			_saveDataDeleter.DeleteDataWithID(id);
		}
	}
}
