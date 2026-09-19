using Features.LevelModule.Scripts;
using Features.SessionManagementModule.Models;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class ReconnectPlacementAdapter : IReconnectPlacement
	{
		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		public int CurrentLevelId => (int)_levelModel.CurrentLevel;

		public ReconnectPlacementAdapter(Features.LevelModule.Scripts.LevelModel levelModel)
		{
			_levelModel = levelModel;
		}
	}
}
