namespace Features.PlayerSkinModule.Scripts
{
	public interface IPlayerStateSkinSwitchService
	{
		void ApplyPlayerSkinState(int playerId, bool isDead, bool hasBottomPart);

		void SwitchSkin(int playerId, bool isDead, bool isBodyVisible);
	}
}
