namespace Features.SettingsMenuModule.Scripts.Services
{
	public interface IQualitySettingsService
	{
		int GetCurrentQualityLevel();

		void SetQualityLevel(int qualityLevel);
	}
}
