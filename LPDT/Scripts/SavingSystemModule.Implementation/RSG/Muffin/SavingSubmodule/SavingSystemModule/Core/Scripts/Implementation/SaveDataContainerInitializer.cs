using System;
using System.Collections.Generic;
using RSG.Muffin.MockSubmodule.MockModule;
using UnityEngine;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	[MockRealization]
	public class SaveDataContainerInitializer : ISaveDataContainerInitializer
	{
		private readonly ISaveDataContainer _saveDataContainer;

		private readonly ISaveFilesManager _saveFilesManager;

		private readonly SavingSystemConfiguration _savingSystemConfiguration;

		public SaveDataContainerInitializer(ISaveDataContainer saveDataContainer, ISaveFilesManager saveFilesManager, SavingSystemConfiguration savingSystemConfiguration)
		{
			_saveDataContainer = saveDataContainer ?? throw new ArgumentNullException("saveDataContainer");
			_saveFilesManager = saveFilesManager ?? throw new ArgumentNullException("saveFilesManager");
			_savingSystemConfiguration = savingSystemConfiguration ?? throw new ArgumentNullException("savingSystemConfiguration");
		}

		public void InitializeContainer()
		{
			GetAllData();
		}

		private void GetAllData()
		{
			string rootFolderPathForCurrentPlatform = _savingSystemConfiguration.GetRootFolderPathForCurrentPlatform();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string allSaveFolder in _savingSystemConfiguration.GetAllSaveFolders())
			{
				foreach (KeyValuePair<string, string> dataFromAllSaveFile in _saveFilesManager.GetDataFromAllSaveFiles(rootFolderPathForCurrentPlatform, allSaveFolder))
				{
					if (!(_savingSystemConfiguration.GetSaveFolderForGroup(dataFromAllSaveFile.Key) != allSaveFolder))
					{
						dictionary[dataFromAllSaveFile.Key] = dataFromAllSaveFile.Value;
					}
				}
			}
			if (!_saveDataContainer.TryProcessDataStrings(dictionary))
			{
				Debug.LogException(new SaveProcessingException("Cannot process save files. Loading Back Up"));
			}
		}
	}
}
