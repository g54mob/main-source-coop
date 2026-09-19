using Features.LevelModule.Scripts;
using Features.SessionManagementModule.Models;

namespace Features.SessionManagementModule.Installers
{
	public sealed class SessionLevelSelectionSource : ILevelSelectionSource
	{
		private readonly ILevelService _levelService;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly ISessionGateWindowPolicy _sessionGateWindowPolicy;

		public string SelectedLevelName => _levelService.GetLevelNameByType(_levelService.GetFirstLevelOfChapter(_levelModel.SelectedChapterIndex));

		public int EnterGateWindowTicks => _sessionGateWindowPolicy.GenericWindowTicks;

		public SessionLevelSelectionSource(ILevelService levelService, Features.LevelModule.Scripts.LevelModel levelModel, ISessionGateWindowPolicy sessionGateWindowPolicy)
		{
			_levelService = levelService;
			_levelModel = levelModel;
			_sessionGateWindowPolicy = sessionGateWindowPolicy;
		}
	}
}
