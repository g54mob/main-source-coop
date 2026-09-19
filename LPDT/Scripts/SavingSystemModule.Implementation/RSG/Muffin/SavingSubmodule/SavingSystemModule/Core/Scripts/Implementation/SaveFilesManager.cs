using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using RSG.Muffin.MockSubmodule.MockModule;
using RSG.Muffin.XOREncryptionSubmodule.XOREncryption.Scripts;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	[MockRealization]
	public class SaveFilesManager : ISaveFilesManager
	{
		private const string SAVE_FILE_EXTENSION = ".sav";

		private readonly Dictionary<string, FileStream> _fileStreamByFileNameDictionary = new Dictionary<string, FileStream>();

		private readonly SavingSystemConfiguration _savingSystemConfiguration;

		public SaveFilesManager(SavingSystemConfiguration savingSystemConfiguration)
		{
			_savingSystemConfiguration = savingSystemConfiguration;
		}

		public void SaveDataInFile(string dataString, string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			string dataString2 = XOREncryption.EncryptString(dataString, _savingSystemConfiguration.FilesEncryptionKey);
			FileInfo fileInfo = InitializeFile(Path.Combine(pathToRootFolder, saveFolderName, saveFileName));
			if (!FileInUse(fileInfo.FullName))
			{
				WriteDataToFile(dataString2, fileInfo.FullName);
			}
		}

		public async UniTask SaveDataInFileAsync(string dataString, string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			string dataString2 = XOREncryption.EncryptString(dataString, _savingSystemConfiguration.FilesEncryptionKey);
			FileInfo fileInfo = InitializeFile(Path.Combine(pathToRootFolder, saveFolderName, saveFileName));
			if (!FileInUse(fileInfo.FullName))
			{
				await WriteDataToFileAsync(dataString2, fileInfo.FullName);
			}
		}

		public string GetDataFromSaveFile(string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			string text = ((!SaveFileExists(pathToRootFolder, saveFolderName, saveFileName)) ? string.Empty : ReadDataFromFile(InitializeFile(Path.Combine(pathToRootFolder, saveFolderName, saveFileName)).FullName));
			if (!XOREncryption.IsBase64String(text))
			{
				return text;
			}
			return XOREncryption.DecryptString(text, _savingSystemConfiguration.FilesEncryptionKey);
		}

		public async UniTask<string> GetDataFromSaveFileAsync(string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			string text = ((await SaveFileExistsAsync(pathToRootFolder, saveFolderName, saveFileName)) ? (await ReadDataFromFileAsync(InitializeFile(Path.Combine(pathToRootFolder, saveFolderName, saveFileName)).FullName)) : string.Empty);
			string text2 = text;
			return XOREncryption.IsBase64String(text2) ? XOREncryption.DecryptString(text2, _savingSystemConfiguration.FilesEncryptionKey) : text2;
		}

		public bool SaveFileExists(string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			return File.Exists(Path.Combine(pathToRootFolder, saveFolderName, saveFileName));
		}

		public async UniTask<bool> SaveFileExistsAsync(string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			return File.Exists(Path.Combine(pathToRootFolder, saveFolderName, saveFileName));
		}

		public TryGetDataFromSaveFileResultData TryGetDataFromSaveFile(string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			string empty = string.Empty;
			if (!SaveFileExists(pathToRootFolder, saveFolderName, saveFileName))
			{
				return new TryGetDataFromSaveFileResultData(TryGetDataFromSaveFileResult.Failure, empty);
			}
			empty = ReadDataFromFile(Path.Combine(pathToRootFolder, saveFolderName, saveFileName));
			return new TryGetDataFromSaveFileResultData(TryGetDataFromSaveFileResult.Success, empty);
		}

		public async UniTask<TryGetDataFromSaveFileResultData> TryGetDataFromSaveFileAsync(string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			string saveData = string.Empty;
			if (!(await SaveFileExistsAsync(pathToRootFolder, saveFolderName, saveFileName)))
			{
				return new TryGetDataFromSaveFileResultData(TryGetDataFromSaveFileResult.Failure, saveData);
			}
			return new TryGetDataFromSaveFileResultData(TryGetDataFromSaveFileResult.Success, await ReadDataFromFileAsync(Path.Combine(pathToRootFolder, saveFolderName, saveFileName)));
		}

		public Dictionary<string, string> GetDataFromAllSaveFiles(string pathToRootFolder, string saveFolderName)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(pathToRootFolder, saveFolderName));
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			foreach (FileInfo item in from saveFile in directoryInfo.GetFiles()
				where saveFile.Extension == ".sav"
				select saveFile)
			{
				dictionary[item.Name.Replace(".sav", string.Empty)] = ReadDataFromFile(item.FullName);
			}
			return dictionary;
		}

		public async UniTask<Dictionary<string, string>> GetDataFromAllSaveFilesAsync(string pathToRootFolder, string saveFolderName)
		{
			Dictionary<string, string> fileDataByFileNameDictionary = new Dictionary<string, string>();
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(pathToRootFolder, saveFolderName));
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			foreach (FileInfo item in from saveFile in directoryInfo.GetFiles()
				where saveFile.Extension == ".sav"
				select saveFile)
			{
				Dictionary<string, string> dictionary = fileDataByFileNameDictionary;
				string key = item.Name.Replace(".sav", string.Empty);
				dictionary[key] = await ReadDataFromFileAsync(item.FullName);
			}
			return fileDataByFileNameDictionary;
		}

		public void ClearAllData(string pathToRootFolder, string saveFolderName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(pathToRootFolder, saveFolderName));
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (FileInfo item in from el in directoryInfo.GetFiles()
				where el.Extension == ".sav"
				select el)
			{
				item.Delete();
			}
		}

		public void DeleteFolder(string folderPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete(recursive: true);
			}
		}

		public void CreateFolder(string folderPath)
		{
			InitializeFolder(folderPath);
		}

		private bool FileInUse(string filePath)
		{
			return _fileStreamByFileNameDictionary[filePath] != null;
		}

		private void InitializeFolder(string folderPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
		}

		private async UniTask WriteDataToFileAsync(string dataString, string filePath)
		{
			await using FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
			_fileStreamByFileNameDictionary[filePath] = fileStream;
			await using StreamWriter streamWriter = new StreamWriter(fileStream);
			await streamWriter.WriteLineAsync(dataString);
			_fileStreamByFileNameDictionary[filePath] = null;
		}

		private void WriteDataToFile(string dataString, string filePath)
		{
			using FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, useAsync: false);
			_fileStreamByFileNameDictionary[filePath] = fileStream;
			using StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine(dataString);
			_fileStreamByFileNameDictionary[filePath] = null;
		}

		private string ReadDataFromFile(string filePath)
		{
			string text = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, useAsync: false)).ReadLine();
			if (!XOREncryption.IsBase64String(text))
			{
				return text;
			}
			return XOREncryption.DecryptString(text, _savingSystemConfiguration.FilesEncryptionKey);
		}

		private async UniTask<string> ReadDataFromFileAsync(string filePath)
		{
			string text = await new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, useAsync: false)).ReadLineAsync();
			return XOREncryption.IsBase64String(text) ? XOREncryption.DecryptString(text, _savingSystemConfiguration.FilesEncryptionKey) : text;
		}

		private FileInfo InitializeFile(string filePath)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			InitializeFolder(fileInfo.DirectoryName);
			if (!fileInfo.Exists)
			{
				fileInfo.Create().Close();
			}
			_fileStreamByFileNameDictionary.TryAdd(fileInfo.FullName, null);
			return fileInfo;
		}
	}
}
