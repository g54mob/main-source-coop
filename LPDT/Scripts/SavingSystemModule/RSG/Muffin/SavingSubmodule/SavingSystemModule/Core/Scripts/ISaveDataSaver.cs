using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public interface ISaveDataSaver
	{
		void SaveDataWithID(string id, object dataObject, string dataGroupName);

		void SaveDataRange(Dictionary<string, object> objectByIDDictionary, string dataGroupName);

		void Commit(string fileName, [CanBeNull] string customSaveFolderName = null);

		UniTask CommitAsync(string fileName, [CanBeNull] string customSaveFolderName = null);
	}
}
