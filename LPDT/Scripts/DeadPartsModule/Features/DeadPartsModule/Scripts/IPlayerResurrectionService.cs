namespace Features.DeadPartsModule.Scripts
{
	public interface IPlayerResurrectionService
	{
		void ResurrectPlayer(int playerId, bool restoreHp, int resurrectedByPlayerId);

		void ResurrectAllPlayers(bool restoreHp);
	}
}
