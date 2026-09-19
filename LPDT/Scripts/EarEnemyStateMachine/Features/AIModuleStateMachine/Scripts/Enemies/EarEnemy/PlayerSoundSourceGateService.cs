using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Features.LevelGatesModule.Data;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	public class PlayerSoundSourceGateService
	{
		private readonly PlayersGatesModelSynchronizedModel _playersGatesModel;

		public PlayerSoundSourceGateService(PlayersGatesModelSynchronizedModel playersGatesModel)
		{
			_playersGatesModel = playersGatesModel;
		}

		public bool IsSoundFromOutsideGatePlayer(HeardSound heardSound)
		{
			if (!heardSound.IsPlayerSource)
			{
				return false;
			}
			int playerId = heardSound.PlayerId;
			if (_playersGatesModel.TryGetPlayerState(playerId, out var state))
			{
				return !state.PlayerInsideGate;
			}
			return false;
		}
	}
}
