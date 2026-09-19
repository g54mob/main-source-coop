namespace Features.PlayerStatesModule.Scripts
{
	public interface IPlayerStateService
	{
		void ChangePlayerState(PlayerState newState, bool forced = true);

		void ChangePlayerState(int playerId, PlayerState newState, bool forced = true);

		bool IsPlayerAlive(int playerId);

		bool IsPlayerStunned(int playerId);

		bool IsPlayerDead(int playerId);

		bool IsPlayerTargetable(int playerId);

		PlayerState GetPlayerState(int playerId);

		bool IsLocalPlayerHealthDepleted();

		void SetStateChangeBlocked(bool isBlocked);
	}
}
