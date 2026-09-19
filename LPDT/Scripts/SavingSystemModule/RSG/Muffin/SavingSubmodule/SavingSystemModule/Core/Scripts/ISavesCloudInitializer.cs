using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public interface ISavesCloudInitializer
	{
		UniTask InitializeCloudAsync(List<string> saveFileNames);
	}
}
