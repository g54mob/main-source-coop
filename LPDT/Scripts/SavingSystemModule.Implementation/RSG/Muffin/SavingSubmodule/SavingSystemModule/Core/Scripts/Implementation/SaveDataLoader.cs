using System;
using System.Collections.Generic;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	public class SaveDataLoader : ISaveDataLoader
	{
		private readonly ISaveDataContainer _saveDataContainer;

		public SaveDataLoader(ISaveDataContainer saveDataContainer)
		{
			_saveDataContainer = saveDataContainer ?? throw new ArgumentNullException("saveDataContainer");
		}

		public T LoadDataForID<T>(string id)
		{
			return _saveDataContainer.GetDataForID<T>(id);
		}

		public void LoadDataForIDOverride(string id, object objectToOverride)
		{
			_saveDataContainer.GetDataForIDOverride(id, objectToOverride);
		}

		public Dictionary<string, T> LoadDataRangeForIDs<T>(List<string> ids)
		{
			return _saveDataContainer.GetDataRangeForIDs<T>(ids);
		}

		public void LoadDataRangeForIDsOverride(Dictionary<string, object> objectToOverrideByIDDictionary)
		{
			_saveDataContainer.GetDataRangeForIDsOverride(objectToOverrideByIDDictionary);
		}
	}
}
