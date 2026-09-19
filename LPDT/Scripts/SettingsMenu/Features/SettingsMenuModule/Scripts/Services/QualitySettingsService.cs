using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Services
{
	public class QualitySettingsService : IQualitySettingsService
	{
		public int GetCurrentQualityLevel()
		{
			return QualitySettings.GetQualityLevel();
		}

		public void SetQualityLevel(int qualityLevel)
		{
			QualitySettings.SetQualityLevel(qualityLevel);
		}
	}
}
