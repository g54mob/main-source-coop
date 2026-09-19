using System.Collections.Generic;

namespace Features.RumModule.Scripts
{
	public interface IDrunkSoberSplashService
	{
		void ApplySoberSplash(float wetnessPulse = 1f);

		void ClearDrunkennessForPlayers(IReadOnlyList<int> playerIds);

		void ApplyLocalWetnessPulse(float wetnessPulse = 1f);
	}
}
