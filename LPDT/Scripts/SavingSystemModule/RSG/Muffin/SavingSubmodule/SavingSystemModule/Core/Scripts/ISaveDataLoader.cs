using System.Collections.Generic;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public interface ISaveDataLoader
	{
		T LoadDataForID<T>(string id);

		void LoadDataForIDOverride(string id, object objectToOverride);

		Dictionary<string, T> LoadDataRangeForIDs<T>(List<string> ids);

		void LoadDataRangeForIDsOverride(Dictionary<string, object> objectToOverrideByIDDictionary);
	}
}
