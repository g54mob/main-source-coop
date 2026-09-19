using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using Features.SessionManagementModule.Models;

namespace Features.SessionManagementModule.Installers
{
	public sealed class SessionLevelLoadObservation : ILevelLoadObservation
	{
		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		public SessionLevelLoadObservation(Features.LevelModule.Scripts.LevelModel levelModel)
		{
			_levelModel = levelModel;
		}

		public UniTask WaitUntilLoadedAsync(string levelName)
		{
			return UniTask.WaitUntil(() => _levelModel.CurrentLevel != LevelType.None);
		}

		public bool TryVerifyLoaded(string levelName, out string problem)
		{
			if (_levelModel.CurrentLevel == LevelType.None)
			{
				problem = "level '" + levelName + "' resources not loaded (LevelModel.CurrentLevel is None)";
				return false;
			}
			problem = null;
			return true;
		}
	}
}
