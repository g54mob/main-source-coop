namespace Features.PlayerStatesModule.Scripts
{
	public class PlayerStateData
	{
		public int PlayerId;

		public PlayerState PlayerState;

		public PlayerStateData(int playerId, PlayerState playerState)
		{
			PlayerId = playerId;
			PlayerState = playerState;
		}
	}
}
