namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy
{
	public interface IHeadCrabVignetteService
	{
		void StartVignetteShowCoroutineForPlayer(int playerId, float time, float currentTime);

		void DisableVignetteForPlayer(int playerId);

		void SetVignettePausedForPlayer(int playerId, bool isPaused);
	}
}
