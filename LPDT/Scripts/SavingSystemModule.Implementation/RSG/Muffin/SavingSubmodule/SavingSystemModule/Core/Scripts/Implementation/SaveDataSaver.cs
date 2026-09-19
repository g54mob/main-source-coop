using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	public class SaveDataSaver : ISaveDataSaver
	{
		private const string SAVE_FILE_EXTENSION = ".sav";

		private readonly ISaveDataContainer _saveDataContainer;

		private readonly ISaveFilesManager _saveFilesManager;

		private readonly SavingSystemConfiguration _savingSystemConfiguration;

		private readonly ISavesCloudManager _savesCloudManager;

		public SaveDataSaver(ISaveDataContainer saveDataContainer, ISaveFilesManager saveFilesManager, SavingSystemConfiguration savingSystemConfiguration, ISavesCloudManager savesCloudManager)
		{
			_saveDataContainer = saveDataContainer ?? throw new ArgumentNullException("saveDataContainer");
			_saveFilesManager = saveFilesManager ?? throw new ArgumentNullException("saveFilesManager");
			_savingSystemConfiguration = savingSystemConfiguration ?? throw new ArgumentNullException("savingSystemConfiguration");
			_savesCloudManager = savesCloudManager ?? throw new ArgumentNullException("savesCloudManager");
		}

		public void SaveDataRange(Dictionary<string, object> objectByIDDictionary, string dataGroupName)
		{
			_saveDataContainer.SetDataRangeForIDs(objectByIDDictionary, dataGroupName);
		}

		public void SaveDataWithID(string id, object dataObject, string dataGroupName)
		{
			_saveDataContainer.SetDataForID(id, dataObject, dataGroupName);
		}

		public void Commit(string fileName, string customSaveFolderName = null)
		{
			string saveFolderName = customSaveFolderName ?? _savingSystemConfiguration.GetSaveFolderForGroup(fileName);
			string dataStringForGroup = _saveDataContainer.GetDataStringForGroup(fileName);
			_savesCloudManager.SaveDataInCloudAsync(dataStringForGroup, fileName + ".sav");
			_saveFilesManager.SaveDataInFile(dataStringForGroup, _savingSystemConfiguration.GetRootFolderPathForCurrentPlatform(), saveFolderName, fileName + ".sav");
		}

		public async UniTask CommitAsync(string fileName, string customSaveFolderName = null)
		{
			string saveFolderName = customSaveFolderName ?? _savingSystemConfiguration.GetSaveFolderForGroup(fileName);
			string dataStringForGroup = _saveDataContainer.GetDataStringForGroup(fileName);
			await _savesCloudManager.SaveDataInCloudAsync(dataStringForGroup, fileName + ".sav");
			await _saveFilesManager.SaveDataInFileAsync(dataStringForGroup, _savingSystemConfiguration.GetRootFolderPathForCurrentPlatform(), saveFolderName, fileName + ".sav");
		}
	}
}
