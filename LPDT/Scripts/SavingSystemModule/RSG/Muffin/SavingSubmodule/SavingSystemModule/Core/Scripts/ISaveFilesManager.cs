using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public interface ISaveFilesManager
	{
		void SaveDataInFile(string dataString, string pathToRootFolder, string saveFolderName, string saveFileName);

		UniTask SaveDataInFileAsync(string dataString, string pathToRootFolder, string saveFolderName, string saveFileName);

		string GetDataFromSaveFile(string pathToRootFolder, string saveFolderName, string saveFileName);

		UniTask<string> GetDataFromSaveFileAsync(string pathToRootFolder, string saveFolderName, string saveFileName);

		bool SaveFileExists(string pathToRootFolder, string saveFolderName, string saveFileName);

		UniTask<bool> SaveFileExistsAsync(string pathToRootFolder, string saveFolderName, string saveFileName);

		TryGetDataFromSaveFileResultData TryGetDataFromSaveFile(string pathToRootFolder, string saveFolderName, string saveFileName);

		UniTask<TryGetDataFromSaveFileResultData> TryGetDataFromSaveFileAsync(string pathToRootFolder, string saveFolderName, string saveFileName);

		Dictionary<string, string> GetDataFromAllSaveFiles(string pathToRootFolder, string saveFolderName);

		UniTask<Dictionary<string, string>> GetDataFromAllSaveFilesAsync(string pathToRootFolder, string saveFolderName);

		void ClearAllData(string pathToRootFolder, string saveFolderName);

		void DeleteFolder(string folderPath);

		void CreateFolder(string folderPath);
	}
}
