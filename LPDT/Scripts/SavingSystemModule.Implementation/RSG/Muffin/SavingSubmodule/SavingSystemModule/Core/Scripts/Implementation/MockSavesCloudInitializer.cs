using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RSG.Muffin.MockSubmodule.MockModule;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	[MockRealization]
	public class MockSavesCloudInitializer : ISavesCloudInitializer
	{
		public UniTask InitializeCloudAsync(List<string> saveFileNames)
		{
			return UniTask.CompletedTask;
		}
	}
}
