namespace NomadDrive.Features.Player.Core
{
	public interface IPlayerComponent
	{
		int SetupPriority => 0;

		void SetupForPlayer(bool isLocalPlayer);
	}
}
