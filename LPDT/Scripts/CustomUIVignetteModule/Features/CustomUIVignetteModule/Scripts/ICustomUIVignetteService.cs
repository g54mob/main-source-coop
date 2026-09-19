namespace Features.CustomUIVignetteModule.Scripts
{
	public interface ICustomUIVignetteService
	{
		void StartVignetteShowCoroutineForPlayer(int playerId, float time, float currentTime);

		void DisableVignetteForPlayer(int playerId);
	}
}
