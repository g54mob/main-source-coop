using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RSG.Muffin.MockSubmodule.MockModule;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	[MockRealization]
	public class MockSavesCloudManager : ISavesCloudManager
	{
		public UniTask SaveDataInCloudAsync(string dataString, string saveFileName)
		{
			return UniTask.CompletedTask;
		}

		public async UniTask<string> GetDataFromCloudAsync(string saveFileName)
		{
			return string.Empty;
		}

		public async UniTask<bool> SaveFileExistsOnCloudAsync(string pathToRootFolder, string saveFolderName, string saveFileName)
		{
			return false;
		}

		public async UniTask<TryGetDataFromSaveFileResultData> TryGetDataFromCloudAsync(string saveFileName)
		{
			return new TryGetDataFromSaveFileResultData(TryGetDataFromSaveFileResult.Failure, string.Empty);
		}

		public async UniTask<Dictionary<string, string>> GetAllDataFromCloudAsync(List<string> saveFileNames)
		{
			return new Dictionary<string, string>();
		}

		public UniTask ClearAllDataAsync(List<string> saveFileNames)
		{
			return UniTask.CompletedTask;
		}
	}
}
