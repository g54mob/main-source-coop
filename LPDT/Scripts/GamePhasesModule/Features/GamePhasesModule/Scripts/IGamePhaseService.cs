using Features.GamePhasesModule.Scripts.Data;
using Features.LevelModule.Scripts;

namespace Features.GamePhasesModule.Scripts
{
	public interface IGamePhaseService
	{
		void SubtractTime(float time);

		void ChangeSubtractByTimeMultiplier(float timeMultiplier);

		void ActivateGamePhaseLoop(GamePhasesData gamePhasesData, LevelType levelType);

		void ActivateNextGamePhaseInSequence();

		void ResumeAfterMigration(GamePhasesData gamePhasesData);
	}
}
