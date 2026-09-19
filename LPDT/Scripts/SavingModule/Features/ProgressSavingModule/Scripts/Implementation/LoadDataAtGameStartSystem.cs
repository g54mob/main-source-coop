using Zenject;

namespace Features.ProgressSavingModule.Scripts.Implementation
{
	public class LoadDataAtGameStartSystem : IInitializable
	{
		private ISavingService _savingService;

		public LoadDataAtGameStartSystem(ISavingService savingService)
		{
			_savingService = savingService;
		}

		public void Initialize()
		{
			_savingService.LoadAllData();
		}
	}
}
