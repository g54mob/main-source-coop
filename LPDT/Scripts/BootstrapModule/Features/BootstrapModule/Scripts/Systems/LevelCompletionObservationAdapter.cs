using Features.LevelModule.Scripts.LevelTransition;
using Features.MultiplayerSessionServices.Scripts;
using Features.QuotaModule.Scripts;
using Features.SessionManagementModule.Models;
using Fusion;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class LevelCompletionObservationAdapter : ILevelCompletionObservation
	{
		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly LevelModel _levelModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LevelTransitionConfiguration _levelTransitionConfiguration;

		private readonly ILevelTransitionBeachReadiness _levelTransitionBeachReadiness;

		public LevelCompletionObservationAdapter(QuotaCompletionModel quotaCompletionModel, LevelModel levelModel, MultiplayerModel multiplayerModel, LevelTransitionConfiguration levelTransitionConfiguration, ILevelTransitionBeachReadiness levelTransitionBeachReadiness)
		{
			_quotaCompletionModel = quotaCompletionModel;
			_levelModel = levelModel;
			_multiplayerModel = multiplayerModel;
			_levelTransitionConfiguration = levelTransitionConfiguration;
			_levelTransitionBeachReadiness = levelTransitionBeachReadiness;
		}

		public bool IsLevelComplete()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			return LevelCompletionResolver.IsComplete(_quotaCompletionModel.IsQuotaCompleted.Value, _quotaCompletionModel.IsBellActivated.Value, _levelModel.CountdownStartTick.Value, networkRunner.Tick, networkRunner.DeltaTime, _levelTransitionConfiguration.MaxTimer, _levelTransitionBeachReadiness.AreAllActivePlayersInBeach());
		}
	}
}
