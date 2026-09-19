namespace Features.RunningSessionModule.Scripts
{
	public interface IRunningSessionService
	{
		void InitializeSessionCode(string sessionCode);

		void SetSessionCodeSaved(string sessionCode);

		bool TryGetSavedSessionCode(out string sessionCode);

		void IncreaseCurrentGameIndex();
	}
}
