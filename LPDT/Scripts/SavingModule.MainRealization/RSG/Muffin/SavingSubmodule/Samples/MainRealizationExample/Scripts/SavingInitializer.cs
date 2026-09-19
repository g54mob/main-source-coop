using System.Collections.Generic;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;
using Zenject;

namespace RSG.Muffin.SavingSubmodule.Samples.MainRealizationExample.Scripts
{
	public class SavingInitializer : IInitializable
	{
		private readonly ISaveDataContainerInitializer _saveDataContainerInitializer;

		private readonly ISavesCloudInitializer _savesCloudInitializer;

		public SavingInitializer(ISaveDataContainerInitializer saveDataContainerInitializer, ISavesCloudInitializer savesCloudInitializer)
		{
			_saveDataContainerInitializer = saveDataContainerInitializer;
			_savesCloudInitializer = savesCloudInitializer;
		}

		public void Initialize()
		{
			_savesCloudInitializer.InitializeCloudAsync(new List<string>());
			_saveDataContainerInitializer.InitializeContainer();
		}
	}
}
