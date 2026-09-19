namespace Features.GameJournalingModule.Scripts.AnalyticsMarkService
{
	public interface IAnalyticsMarkUserService
	{
		void MarkAsDebugBuild();

		void MarkAsReleaseBuild();

		void MarkABGroup(string groupName);

		void MarkAsDeveloper();

		void MarkAsCheater();

		void MarkAsNeverVisitedInteractiveMine();

		void MarkAsVisitedInteractiveMine();

		void MarkAsFinishedInteractiveMine();
	}
}
