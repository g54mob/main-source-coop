using Features.SettingsMenuModule.Scripts.Data;

namespace Features.SettingsMenuModule.Scripts.Services
{
	public interface IPerformanceDetectionService
	{
		PerformanceLevel DeterminePerformanceLevel();
	}
}
