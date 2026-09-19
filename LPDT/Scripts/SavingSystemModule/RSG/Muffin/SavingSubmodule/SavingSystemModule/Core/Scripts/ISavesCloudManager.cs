using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public interface ISavesCloudManager
	{
		UniTask SaveDataInCloudAsync(string dataString, string saveFileName);

		UniTask<string> GetDataFromCloudAsync(string saveFileName);

		UniTask<bool> SaveFileExistsOnCloudAsync(string pathToRootFolder, string saveFolderName, string saveFileName);

		UniTask<TryGetDataFromSaveFileResultData> TryGetDataFromCloudAsync(string saveFileName);

		UniTask<Dictionary<string, string>> GetAllDataFromCloudAsync(List<string> saveFileNames);

		UniTask ClearAllDataAsync(List<string> saveFileNames);
	}
}
