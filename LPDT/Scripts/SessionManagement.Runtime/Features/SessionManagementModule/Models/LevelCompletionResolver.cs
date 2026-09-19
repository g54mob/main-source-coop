namespace Features.SessionManagementModule.Models
{
	public static class LevelCompletionResolver
	{
		public static bool IsComplete(bool isQuotaCompleted, bool isBellActivated, int countdownStartTick, int currentTick, float secondsPerTick, float countdownSeconds, bool allActivePlayersInBeach)
		{
			if (!isQuotaCompleted)
			{
				return false;
			}
			if (!isBellActivated)
			{
				return false;
			}
			if (allActivePlayersInBeach)
			{
				return true;
			}
			if (countdownStartTick == 0)
			{
				return false;
			}
			return (float)(currentTick - countdownStartTick) * secondsPerTick >= countdownSeconds;
		}
	}
}
